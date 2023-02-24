using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;
using IEM.Areas.MASTERS.Controllers;
namespace IEM.Areas.EOW.Models
{
    public class ArfRaising : ArfRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ForMailController mailsender = new ForMailController();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public List<GetAdvancetype> GetAdvancetype(string adtype)
        {
            List<GetAdvancetype> objCountrytype = new List<GetAdvancetype>();
            try
            {
                string findavncetype = "";
                if (adtype == "Supplier")
                {
                    findavncetype = "2";
                }
                else if (adtype == "Petty")
                {
                    findavncetype = "3";
                }
                else
                {
                    findavncetype = "1";
                }
                GetAdvancetype objModel;
                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("select advancetype_gid,advancetype_name from iem_mst_tadvancetype where advancetype_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@emp_code", findavncetype);
                cmd.Parameters.AddWithValue("@ACTION", "GetAdvancetype");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetAdvancetype();
                    objModel.advancetype_gid = Convert.ToInt32(dt.Rows[i]["advancetype_gid"].ToString());
                    objModel.advancetype_name = Convert.ToString(dt.Rows[i]["advancetype_name"].ToString());
                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
            // throw new NotImplementedException();
        }
        public IEnumerable<EOW_arfraising> SelectViewdataarf(int ecfid, string type)
        {
            List<EOW_arfraising> objAttachment = new List<EOW_arfraising>();
            EOW_arfraising objModel;
            return objAttachment;
        }
        public DataTable SelectAdvance(string Empcode)
        {
            DataTable dt = new DataTable();
            try
            {
                List<EOW_arfraising> emp = new List<EOW_arfraising>();
                EOW_arfraising objModel;
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCE");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

            }
            return dt;
        }
        public IEnumerable<EOW_arfraising> SelectAdvanceecf(string Empcode)
        {
            List<EOW_arfraising> objNatureofExpenses = new List<EOW_arfraising>();
            try
            {
                DataTable dt = new DataTable();
                EOW_arfraising objModel;
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCE");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();
                    objModel.ecfarf_advancetype = Convert.ToString(dt.Rows[i]["ecfarf_advancetype_gid"].ToString());
                    objModel.ecfarf_desc = Convert.ToString(dt.Rows[i]["ecfarf_desc"]);
                    objModel.ecfarf_liq_date = Convert.ToString(dt.Rows[i]["ecfarf_liq_date"].ToString());
                    objModel.ecfarf_pi_gl_no = Convert.ToString(dt.Rows[i]["ecfarf_pi_gl_no"].ToString());
                    objModel.ecfarf_fc_code = Convert.ToString(dt.Rows[i]["ecfarf_fc_code"].ToString());
                    objModel.ecfarf_cc_code = Convert.ToString(dt.Rows[i]["ecfarf_cc_code"].ToString());
                    objModel.ecfarf_product_code = Convert.ToString(dt.Rows[i]["ecfarf_product_code"].ToString());
                    objModel.ecfarf_ou_code = Convert.ToString(dt.Rows[i]["ecfarf_ou_code"]);
                    objModel.ecfarf_amount = objCmnFunctions.GetINRAmount(Convert.ToString(dt.Rows[i]["ecfarf_amount"].ToString()));
                    objModel.AmountINRnew = Convert.ToString(dt.Rows[i]["ecfarf_amount"].ToString());
                    objModel.AmountINR = objCmnFunctions.GetINRAmount(Convert.ToString(dt.Rows[i]["ecfarf_amount"].ToString()));
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objNatureofExpenses;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<EOW_arfraising> SelectAdvanceprint(string Empcode)
        {
            List<EOW_arfraising> objNatureofExpenses = new List<EOW_arfraising>();
            try
            {
                DataTable dt = new DataTable();
                EOW_arfraising objModel;
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCEprint");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();
                    objModel.ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[i]["ecf_gid"].ToString());
                    objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"]);
                    objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["supplierheader_name"].ToString());
                    string po_no = Convert.ToString(dt.Rows[i]["ecfarf_po_no"].ToString());
                    if (po_no == "")
                    {
                        po_no = "-";
                    }
                    objModel.ecfarf_po_no = po_no;
                    string cbf_no = Convert.ToString(dt.Rows[i]["ecfarf_cbf_no"].ToString());
                    if (cbf_no == "")
                    {
                        cbf_no = "-";
                    }
                    objModel.ecfarf_cbf_no = cbf_no;
                    string ecf_date = Convert.ToString(dt.Rows[i]["invoice_date"].ToString());
                    if (ecf_date == "")
                    {
                        ecf_date = "-";
                    }
                    objModel.ecf_date = ecf_date;
                    string ecf_no = Convert.ToString(dt.Rows[i]["invoice_no"].ToString());
                    if (ecf_no == "")
                    {
                        ecf_no = "-";
                    }
                    objModel.ecf_no = ecf_no;
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objNatureofExpenses;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataTable Selectpaymentemp()
        {
            DataTable dt = new DataTable();
            try
            {
                List<EOW_arfraising> emp = new List<EOW_arfraising>();
                GetConnection();


                // SqlCommand cmd = new SqlCommand(" select ecfcreditline_gid,ecfcreditline_pay_mode,ecfcreditline_ref_no,ecfcreditline_beneficiary, ecfcreditline_desc from iem_trn_tecfcreditline  where ecfcreditline_isremoved='N' and ecfcreditline_pay_mode='ERA' order by ecfcreditline_ref_no desc", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Selectpaymentemp");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable Selectpaymentsup()
        {
            DataTable dt = new DataTable();
            try
            {
                List<EOW_arfraising> emp = new List<EOW_arfraising>();
                GetConnection();


                //SqlCommand cmd = new SqlCommand("  select paymode_gid from iem_mst_tpaymode where paymode_code='CHQ'  and paymode_isremoved='N' and paymode_active='Y'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Selectpaymentsup");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable Selectpayment(string Empcode)
        {
            DataTable dt = new DataTable();
            try
            {
                List<EOW_arfraising> emp = new List<EOW_arfraising>();
                GetConnection();


                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_PAYMENT");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public string selectpolicy1(string Gid)
        {
            try
            {
                string insertcommend = "";
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = Gid;
                //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SubCategoryHelp";
                //da = new SqlDataAdapter(cmd);
                cmd = new SqlCommand("select advancetype_help from iem_mst_tadvancetype where advancetype_isremoved='N' and advancetype_gid=" + Gid + "", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    insertcommend = Convert.ToString(dt.Rows[0]["advancetype_help"].ToString());
                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable Selectattach(string Empcode)
        {
            DataTable dt = new DataTable();
            try
            {
                List<EOW_arfraising> emp = new List<EOW_arfraising>();
                GetConnection();


                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ATTACHEMENT");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        //public IEnumerable<EOW_arfraising> SelectAdvance()
        //{
        //    try
        //    {
        //        List<EOW_arfraising> emp = new List<EOW_arfraising>();
        //        EOW_arfraising objModel;
        //        GetConnection();

        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
        //        cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCE");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        //foreach (DataRow row in dt.Rows)
        //        //{
        //        //    objModel = new EOW_arfraising()
        //        //    {
        //        //        ecfarf_gid = Convert.ToInt32(row["ecfarf_gid"].ToString()),
        //        //        ecfarf_ecf_gid = Convert.ToInt32(row["ecfarf_ecf_gid"].ToString()),
        //        //        ecfarf_advancetype_gid = Convert.ToInt32(row["ecfarf_advancetype_gid"].ToString()),
        //        //        ecfarf_desc = Convert.ToString(row["ecfarf_desc"]),
        //        //        ecfarf_liq_date = Convert.ToString(row["ecfarf_liq_date"].ToString()),
        //        //        ecfarf_po_no = Convert.ToString(row["ecfarf_po_no"].ToString()),
        //        //        ecfarf_cbf_no = Convert.ToString(row["ecfarf_cbf_no"].ToString()),
        //        //        ecfarf_fc_code = Convert.ToString(row["ecfarf_fc_code"].ToString()),
        //        //        ecfarf_cc_code = Convert.ToString(row["ecfarf_cc_code"].ToString()),
        //        //        ecfarf_product_code = Convert.ToString(row["ecfarf_product_code"].ToString()),
        //        //        ecfarf_ou_code = Convert.ToString(row["ecfarf_ou_code"]),
        //        //        ecfarf_amount = Convert.ToDecimal(row["ecfarf_amount"].ToString())
        //        //    };
        //        //    emp.Add(objModel);
        //        //}
        //        return dt;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public EOW_arfraising payamount(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();

                // SqlCommand cmd = new SqlCommand("select SUM(creditline_amount) as creditline_amount from iem_trn_tcreditline where creditline_ecf_gid=" + empid + " and creditline_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "payamount");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string eamount = Convert.ToString(dt.Rows[0]["creditline_amount"]);
                if (eamount != "")
                {
                    objModel = new EOW_arfraising()
                    {
                        ecf_amount = Convert.ToDecimal(eamount)

                    };
                    return objModel;
                }
                else
                {
                    objModel = new EOW_arfraising()
                    {
                        ecf_amount = 0
                    };
                    return objModel;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }
        public EOW_arfraising advanceamount(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                //  SqlCommand cmd = new SqlCommand("select sum(ecfarf_amount)as ecfarf_amount from iem_trn_tecfarf where ecfarf_ecf_gid=" + empid + " and ecfarf_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "advanceamount");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string eamount = Convert.ToString(dt.Rows[0]["ecfarf_amount"]);
                if (eamount != "")
                {
                    objModel = new EOW_arfraising()
                    {
                        ecf_amount = Convert.ToDecimal(eamount)

                    };
                    return objModel;
                }
                else
                {
                    objModel = new EOW_arfraising()
                    {
                        ecf_amount = 0
                    };
                    return objModel;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }
        public EOW_arfraising ecf_desc(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                //SqlCommand cmd = new SqlCommand("select ecf_description from iem_trn_tecf where ecf_gid =" + empid + " and ecf_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "ecf_desc");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string eamount = Convert.ToString(dt.Rows[0]["ecf_description"]);
                objModel = new EOW_arfraising()
                {
                    ecfarf_desc = (eamount)

                };

                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }
        public EOW_arfraising SelectAdvanceById(int advanceId)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@action", "SELECT_ADVANCEById");
                cmd.Parameters.AddWithValue("@gid", advanceId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string[] date = dt.Rows[0]["ecfarf_liq_date"].ToString().Split('-');
                string liqdate = date[1].ToString() + "-" + date[0].ToString() + "-" + date[2].ToString();

                objModel = new EOW_arfraising()
                {
                    ecfarf_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_gid"].ToString()),
                    ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_ecf_gid"].ToString()),
                    ecfarf_advancetype_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_advancetype_gid"].ToString()),
                    ecfarf_desc = Convert.ToString(dt.Rows[0]["ecfarf_desc"]),
                    ecfarf_liq_date = Convert.ToString(liqdate),
                    ecfarf_po_no = Convert.ToString(dt.Rows[0]["ecfarf_po_no"].ToString()),
                    ecfarf_cbf_no = Convert.ToString(dt.Rows[0]["ecfarf_cbf_no"].ToString()),
                    ecfarf_fc_code = Convert.ToString(dt.Rows[0]["ecfarf_fc_code"].ToString()),
                    ecfarf_cc_code = Convert.ToString(dt.Rows[0]["ecfarf_cc_code"].ToString()),
                    ecfarf_product_code = Convert.ToString(dt.Rows[0]["ecfarf_product_code"].ToString()),
                    ecfarf_ou_code = Convert.ToString(dt.Rows[0]["ecfarf_ou_code"]),
                    ecfarf_amount = Convert.ToString(dt.Rows[0]["ecfarf_amount"].ToString())
                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        //public IEnumerable<EOW_arfraising> Selectattach()
        //{
        //    try
        //    {
        //        List<EOW_arfraising> emp = new List<EOW_arfraising>();
        //        EOW_arfraising objModel;
        //        GetConnection();

        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
        //        cmd.Parameters.AddWithValue("@ACTION", "SELECT_ATTACHEMENT");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            objModel = new EOW_arfraising()
        //            {
        //                attachment_gid = Convert.ToInt32(row["attachment_gid"].ToString()),
        //                attachment_filename = Convert.ToString(row["attachment_filename"]),
        //                attachment_attachmenttype_gid = Convert.ToInt32(row["attachment_attachmenttype_gid"].ToString()),
        //                attachment_desc = Convert.ToString(row["attachment_desc"]),
        //                attachment_date = Convert.ToString(row["attachment_date"].ToString())
        //            };
        //            emp.Add(objModel);
        //        }
        //        return emp;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public EOW_arfraising SelectpaymentById(int paymentId)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@action", "SELECT_PAYMENTById");
                cmd.Parameters.AddWithValue("@gid", paymentId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EOW_arfraising()
                {
                    creditline_gid = Convert.ToInt32(dt.Rows[0]["creditline_gid"].ToString()),
                    ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[0]["creditline_ecf_gid"].ToString()),
                    creditline_pay_mode = Convert.ToString(dt.Rows[0]["creditline_pay_mode"]),
                    creditline_ref_no = Convert.ToString(dt.Rows[0]["creditline_ref_no"]),
                    creditline_beneficiary = Convert.ToString(dt.Rows[0]["creditline_beneficiary"].ToString()),
                    creditline_desc = Convert.ToString(dt.Rows[0]["creditline_desc"].ToString()),
                    creditline_amount = Convert.ToString(dt.Rows[0]["creditline_amount"].ToString())
                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        //public IEnumerable<EOW_arfraising> Selectpayment()
        //{
        //    try
        //    {
        //        List<EOW_arfraising> emp = new List<EOW_arfraising>();
        //        EOW_arfraising objModel;
        //        GetConnection();

        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
        //        cmd.Parameters.AddWithValue("@ACTION", "SELECT_PAYMENT");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            objModel = new EOW_arfraising()
        //            {
        //                creditline_gid = Convert.ToInt32(row["creditline_gid"].ToString()),
        //                ecfarf_ecf_gid = Convert.ToInt32(row["creditline_ecf_gid"].ToString()),
        //                creditline_pay_mode = Convert.ToString(row["creditline_pay_mode"]),
        //                creditline_ref_no = Convert.ToString(row["creditline_ref_no"]),
        //                creditline_beneficiary = Convert.ToString(row["creditline_beneficiary"].ToString()),
        //                creditline_desc = Convert.ToString(row["creditline_desc"].ToString()),
        //                creditline_amount = Convert.ToDecimal(row["creditline_amount"].ToString())
        //            };
        //            emp.Add(objModel);
        //        }
        //        return emp;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public EOW_EmployeeeExpense empraiserdetails(string empid)
        {
            EOW_EmployeeeExpense objModel = new EOW_EmployeeeExpense();
            try
            {
                List<EOW_EmployeeeExpense> objOrgType = new List<EOW_EmployeeeExpense>();

                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("select employee_code,employee_name,ecf_remark from iem_trn_tecf join iem_mst_temployee on ecf_raiser=employee_gid where ecf_gid=" + empid + " and ecf_isremoved='N' and employee_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "empraiserdetails");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EOW_EmployeeeExpense()
                {
                    ecfremark = (dt.Rows[0]["ecf_remark"].ToString()),
                    Raiser_Mode = (dt.Rows[0]["employee_code"].ToString()),
                    ecf_raisername = (dt.Rows[0]["employee_name"].ToString())

                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        public EOW_EmployeeeExpense supraiserdetails(string empid)
        {
            EOW_EmployeeeExpense objModel = new EOW_EmployeeeExpense();
            try
            {
                List<EOW_EmployeeeExpense> objOrgType = new List<EOW_EmployeeeExpense>();

                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand(" select supplierheader_suppliercode,supplierheader_name,supplierheader_gid,ecf_remark from iem_trn_tecf join asms_trn_tsupplierheader on ecf_supplier_gid=supplierheader_gid where ecf_gid=" + empid + "  and ecf_isremoved='N' and supplierheader_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "supraiserdetails");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EOW_EmployeeeExpense()
                {
                    ecfremark = (dt.Rows[0]["ecf_remark"].ToString()),
                    Raiser_Mode = (dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                    ecf_raisername = (dt.Rows[0]["supplierheader_name"].ToString()),
                    supplierheader_ggid = Convert.ToInt32(dt.Rows[0]["supplierheader_gid"].ToString())
                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        public EOW_arfraising empdetails(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("select employee_code,employee_grade_code,employee_fc_code,employee_cc_code,employee_product_code,employee_ou_code from iem_mst_temployee where employee_isremoved='N' and employee_gid=" + empid + "", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "empdetails");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EOW_arfraising()
                {
                    grade = (dt.Rows[0]["employee_grade_code"].ToString()),
                    ecf_raiser_gid = (dt.Rows[0]["employee_code"].ToString()),
                    ecfarf_fc_code = (dt.Rows[0]["employee_fc_code"].ToString()),
                    ecfarf_cc_code = (dt.Rows[0]["employee_cc_code"].ToString()),
                    ecfarf_product_code = (dt.Rows[0]["employee_product_code"].ToString()),
                    ecfarf_ou_code = (dt.Rows[0]["employee_ou_code"].ToString())
                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        public EOW_arfraising empdetailspay_id(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();

                GetConnection();
                DataTable dt = new DataTable();
                //  SqlCommand cmd = new SqlCommand("select creditline_gid,creditline_ecf_gid,creditline_beneficiary,creditline_amount,creditline_pay_mode,creditline_ref_no,creditline_desc from iem_trn_tcreditline where creditline_gid=" + empid + " and creditline_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "empdetailspay_id");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objModel = new EOW_arfraising()
                    {
                        creditline_gid = Convert.ToInt32(dt.Rows[0]["creditline_gid"].ToString()),
                        ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[0]["creditline_ecf_gid"].ToString()),
                        creditline_pay_mode = (dt.Rows[0]["creditline_pay_mode"].ToString()),
                        creditline_ref_no = (dt.Rows[0]["creditline_ref_no"].ToString()),
                        creditline_desc = (dt.Rows[0]["creditline_desc"].ToString()),
                        creditline_amount = Convert.ToString(dt.Rows[0]["creditline_amount"].ToString())
                    };
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        public EOW_arfraising empdetailsadvance_id(string empid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();


                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("select ecfarf_gid,ecfarf_pi_gl_no, CONVERT (varchar(10),ecfarf_liq_date,105) as ecfarf_liq_date,ecfarf_ou_code,ecfarf_cc_code,ecfarf_product_code,ecfarf_fc_code,ecfarf_ecf_gid,ecfarf_advancetype_gid,ecfarf_amount,ecfarf_po_no,ecfarf_cbf_no,ecfarf_desc from iem_trn_tecfarf where ecfarf_gid=" + empid + " and ecfarf_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "empdetailsadvance_id");
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EOW_arfraising()
                {
                    ecfarf_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_gid"].ToString()),
                    ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_ecf_gid"].ToString()),
                    ecfarf_advancetype_gid = Convert.ToInt32(dt.Rows[0]["ecfarf_advancetype_gid"].ToString()),
                    ecfarf_amount = Convert.ToString(dt.Rows[0]["ecfarf_amount"].ToString()),
                    ecfarf_po_no = Convert.ToString(dt.Rows[0]["ecfarf_po_no"]),
                    ecfarf_cbf_no = Convert.ToString(dt.Rows[0]["ecfarf_cbf_no"]),
                    ecfarf_desc = (dt.Rows[0]["ecfarf_desc"].ToString()),
                    ecfarf_fc_code = (dt.Rows[0]["ecfarf_fc_code"].ToString()),
                    ecfarf_product_code = (dt.Rows[0]["ecfarf_product_code"].ToString()),
                    ecfarf_cc_code = (dt.Rows[0]["ecfarf_cc_code"].ToString()),
                    ecfarf_ou_code = (dt.Rows[0]["ecfarf_ou_code"].ToString()),
                    ecfarf_liq_date = (dt.Rows[0]["ecfarf_liq_date"].ToString()),
                    ecfarf_pi_gl_no = (dt.Rows[0]["ecfarf_pi_gl_no"].ToString()),
                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }
        //public EOW_arfraising GetarfEmployeedetailssearch()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        GetConnection();
        //        List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
        //        EOW_arfraising objModel;
        //        SqlCommand cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //         objModel = new EOW_arfraising()
        //        {
        //            employee_name =(dt.Rows[0]["employee_name"].ToString()),
        //            employee_code = (dt.Rows[0]["employee_code"].ToString()),

        //       };
        //        return objModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public string Insertecfderaft(EOW_arfraising EmployeeeExpenseModel, string eempid)
        {
            try
            {
                string[,] codesUP = new string[,]
	               {
	      {"ecf_remark",EmployeeeExpenseModel.arfremark },
                  };
                string[,] whcosup = new string[,]
	               {     
	    {"ecf_gid", eempid}
                  };
                string tnameUP = "iem_trn_tecf";

                string insertcommendUP = objCommonIUD.UpdateCommon(codesUP, whcosup, tnameUP);
                return insertcommendUP;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        /*public string Insertecf(EOW_arfraising EmployeeeExpenseModel, string eempid, string user, string queid)
        {
            string Emp_Msg = "";
            string queue_gid = "";
            string Emp_Msgecfremark = "";
            string Emp_Msgsuper = "";
            string insertcommend = "";
            //string queid = "0";
            try
            {
                if (EmployeeeExpenseModel.arfremark == null)
                {
                    Emp_Msgecfremark = "";
                }
                else
                {
                    Emp_Msgecfremark = EmployeeeExpenseModel.arfremark.ToString();
                }

                GetConnection();
                DataTable dtempsup = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = eempid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetEmpSupper";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtempsup);
                if (dtempsup.Rows.Count > 0)
                {
                    Emp_Msgsuper = Convert.ToString(dtempsup.Rows[0]["employee_supervisor"].ToString());

                    GetConnection();
                    cmd = new SqlCommand("pr_ecfdelmat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = Convert.ToInt32(EmployeeeExpenseModel.ecf_ecf_gid.ToString());
                    cmd.Parameters.Add("@ecf_approver_gid", SqlDbType.Int).Value = Convert.ToInt32(Emp_Msgsuper.ToString());

                    cmd.Parameters.Add("@ecf_next_queue_to_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@ecf_next_queue_to_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_next_queue_to_type", SqlDbType.Char, 1);
                    cmd.Parameters["@ecf_next_queue_to_type"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_next_queue_to_additional_flag", SqlDbType.Char, 1);
                    cmd.Parameters["@ecf_next_queue_to_additional_flag"].Direction = ParameterDirection.Output;


                    cmd.Parameters.Add("@ecfdelmat_result", SqlDbType.Int, 32);
                    cmd.Parameters["@ecfdelmat_result"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_err_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@ecf_err_output"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_sql_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@ecf_sql_output"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var result = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_gid"].Value);
                    var Flag = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_type"].Value);
                    var Additionalflagnew = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_additional_flag"].Value);
                    var demmatresult = Convert.ToString(cmd.Parameters["@ecfdelmat_result"].Value);
                    var sqlerrors = Convert.ToString(cmd.Parameters["@ecf_err_output"].Value);
                    var ecferrors = Convert.ToString(cmd.Parameters["@ecf_sql_output"].Value);

                    if (demmatresult == "" || result == "")
                    {
                        //Emp_Msg = ecferrors + "-" + sqlerrors;
                        Emp_Msg = sqlerrors;
                        return Emp_Msg;
                    }
                    else if (demmatresult != "0" && demmatresult != "")
                    {
                        Emp_Msgsuper = result.ToString();
                    }
                    if (Emp_Msgsuper != "")
                    {

                        int EcfInprocess = Convert.ToInt32(ConfigurationManager.AppSettings["EcfInprocess"].ToString());
                        int ecf_status = Convert.ToInt32(ConfigurationManager.AppSettings["EcfApproved"].ToString());

                        insertcommend = "Success";
                        if (insertcommend != "")
                        {
                            if (queid.ToString().Trim() != "")
                            {
                                string[,] codesq = new string[,]
                               {
                      {"queue_action_date","sysdatetime()"},
                      {"queue_action_by",eempid.ToString() },
                      {"queue_action_status",ecf_status.ToString() },
                      {"queue_action_remark",Emp_Msgecfremark.ToString() }
                              };
                                string[,] whreq = new string[,]
                               {
                    {"queue_gid",queid.ToString() }
                              };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                            }
                            else
                            {
                                GetConnection();
                                cmd = new SqlCommand("pr_eow_trn_tarf", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ecfgid", SqlDbType.Int).Value = Convert.ToInt32(EmployeeeExpenseModel.ecf_ecf_gid);
                                cmd.Parameters.Add("@ecf_remark", SqlDbType.VarChar).Value = Emp_Msgecfremark;
                                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                                int data = cmd.ExecuteNonQuery();
                                insertcommend = "Success";
                                if (insertcommend == "Success")
                                {
                                    GetConnection();
                                    DataTable dt = new DataTable();
                                    cmd = new SqlCommand("select ecf_no from iem_trn_tecf where  ecf_gid='" + EmployeeeExpenseModel.ecf_ecf_gid + "' and ecf_isremoved='N'", con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        Emp_Msg = Convert.ToString(dt.Rows[0]["ecf_no"].ToString());
                                    }
                                }
                            }

                            string[,] codes = new string[,]
	               {
        {"queue_date","sysdatetime()"},
	    {"queue_ref_flag", "1"},
        {"queue_ref_gid",EmployeeeExpenseModel.ecf_ecf_gid },
	    {"queue_ref_status", EcfInprocess.ToString()},
        {"queue_from",eempid },
	    {"queue_to_type", Flag.ToString()},
        {"queue_to",Emp_Msgsuper.ToString()},
	    {"queue_action_for", "A"},    
        {"queue_prev_gid", queid},
        {"Additional_flag", Additionalflagnew.ToString()}
                  };

                            string tname = "iem_trn_tqueue";

                            string insertcommendecf = objCommonIUD.InsertCommon(codes, tname);

                            if (insertcommendecf == "success")
                            {
                                GetConnection();
                                DataTable dtempsupnew = new DataTable();
                                cmd = new SqlCommand("SELECT MAX(queue_gid) as queue_gid FROM iem_trn_tqueue  where queue_isremoved='N' and queue_from='" + eempid + "' and queue_ref_gid='" + EmployeeeExpenseModel.ecf_ecf_gid + "'", con);
                                cmd.CommandType = CommandType.Text;
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtempsupnew);
                                if (dtempsupnew.Rows.Count > 0)
                                {
                                    queue_gid = Convert.ToString(dtempsupnew.Rows[0]["queue_gid"].ToString());
                                }
                                if (queue_gid != "")
                                {
                                    string[,] codesUP = new string[,]
	               {
	    {"ecf_queue_gid", queue_gid},
        {"ecf_queue_to_type","E" },
	    {"ecf_queue_to", Emp_Msgsuper},
        {"ecf_status",EcfInprocess.ToString() },
        {"ecf_all_status",EcfInprocess.ToString()},
	    {"ecf_urgent_flag","N"},
        {"ecf_action_by",eempid },
	    {"ecf_action_date","sysdatetime()"}
                  };
                                    string[,] whcosup = new string[,]
	               {
        {"ecf_raiser",eempid },
	    {"ecf_gid", EmployeeeExpenseModel.ecf_ecf_gid}
                  };
                                    string tnameUP = "iem_trn_tecf";

                                    string insertcommendUP = objCommonIUD.UpdateCommon(codesUP, whcosup, tnameUP);

                                    string mail = queue_gid.ToString();
                                    GetConnection();
                                    DataTable dtdoctype = new DataTable();
                                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = mail;
                                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdocsubtype";
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtdoctype);
                                    if (dtdoctype.Rows.Count > 0)
                                    {
                                        string doctypeid = Convert.ToString(dtdoctype.Rows[0]["docsubtype_gid"].ToString());
                                        doctypeid = objCmnFunctions.GetSubDocType(doctypeid);
                                        mailsender.sendusermail("EOW", doctypeid, mail, "S", "0");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Emp_Msg = ecferrors + "-" + sqlerrors;
                        Emp_Msg = sqlerrors;
                        return Emp_Msg;
                    }
                }
                return "ARF Number is : " + Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }*/

        public string Insertecf(EOW_arfraising EmployeeeExpenseModel, string eempid, string user, string queid = "0")
        {

            //Pandiaraj 30-06-2018
            string Emp_Msg = "";
            string queue_gid = "";
            string Emp_Msgecfremark = "";
            string Emp_Msgsuper = "";
            string insertcommend = "";
            //string queid = "0";
            try
            {
                if (EmployeeeExpenseModel.arfremark == null)
                {
                    Emp_Msgecfremark = "";
                }
                else
                {
                    Emp_Msgecfremark = EmployeeeExpenseModel.arfremark.ToString();
                }

                GetConnection();
                cmd = new SqlCommand("InsertARF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = Convert.ToInt32(EmployeeeExpenseModel.ecf_ecf_gid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(eempid);
                cmd.Parameters.Add("@queid", SqlDbType.VarChar).Value = @queid;
                cmd.Parameters.Add("@arfremark", SqlDbType.VarChar).Value = Emp_Msgecfremark.ToString();

                cmd.Parameters.Add("@EcfInprocess", SqlDbType.Int).Value = Convert.ToInt32(ConfigurationManager.AppSettings["EcfInprocess"].ToString());
                cmd.Parameters.Add("@ecf_status", SqlDbType.Int).Value = Convert.ToInt32(ConfigurationManager.AppSettings["EcfApproved"].ToString());

                cmd.Parameters.Add("@errormsg", SqlDbType.VarChar, 10000);
                cmd.Parameters["@errormsg"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@ecfoutput", SqlDbType.VarChar, 10000);
                cmd.Parameters["@ecfoutput"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@queue_gid", SqlDbType.Int, 32);
                cmd.Parameters["@queue_gid"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@clear", SqlDbType.Int, 32);
                cmd.Parameters["@clear"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                var errormsgs = Convert.ToString(cmd.Parameters["@errormsg"].Value);
                var clear = Convert.ToString(cmd.Parameters["@clear"].Value);
                Emp_Msg = errormsgs.ToString();
                if (clear == "1")
                {
                    errormsgs = Convert.ToString(cmd.Parameters["@ecfoutput"].Value);
                    Emp_Msg = "ARF Number is : " + errormsgs.ToString();
                }
                else
                {
                    Emp_Msg = errormsgs.ToString();
                }



                queue_gid = Convert.ToString(cmd.Parameters["@queue_gid"].Value);
                if (queue_gid != "0" || queue_gid != null)
                {
                    string doctypeid = "0";
                    string mail = queue_gid.ToString();
                    GetConnection();
                    DataSet dtdoctype = new DataSet();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = mail;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdocsubtype";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdoctype);
                    if (dtdoctype.Tables[0].Rows.Count > 0)
                    {
                        doctypeid = Convert.ToString(dtdoctype.Tables[0].Rows[0]["docsubtype_gid"].ToString());
                        doctypeid = objCmnFunctions.GetSubDocType(doctypeid);
                        string mailsend = mailsender.sendusermail("EOW", doctypeid, mail, "S", "0");
                    }

                    if (dtdoctype.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdoctype.Tables[1].Rows.Count; i++)
                        {
                            string queueidformail = Convert.ToString(dtdoctype.Tables[1].Rows[0]["queue_gid"].ToString());
                            mailsender.sendusermail("EOW", doctypeid, queueidformail, "A", "0");
                        }
                    }


                }
                else
                {
                    Emp_Msg = "";
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
            //Pandiaraj 30-06-2018
        }


        public List<GetBenificary> GetBenificary()
        {
            List<GetBenificary> objCountrytype = new List<GetBenificary>();
            try
            {

                GetBenificary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("SELECT payment_beneficiaryname FROM asms_trn_tpayment WHERE payment_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "GetBenificary");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetBenificary();
                    objModel.payment_benificary = (dt.Rows[i]["payment_beneficiaryname"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }

        }

        public string GetBenificary(string empsup, string empSupgid, string AccNo)
        { 
            try
            {
                if (empsup == "Employee")
                {
                    empsup = "E";
                }
                else if (empsup == "Supplier")
                {
                    empsup = "S";
                }
                else if (empsup == "Petty Cash")
                {
                    empsup = "E";
                }

                //GetPaymode objModel;

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                //if (empsup == "E")
                //{
                //    GetConnection();
                //    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                //    cmd.Parameters.AddWithValue("@ACTION", "GetBenificary_emp");
                //    cmd.CommandType = CommandType.StoredProcedure;
                //}
                if (empsup == "S")
                {
                    //cmd = new SqlCommand("select distinct paymode_gid,paymode_name from  iem_mst_tpaymode join asms_trn_tpayment on paymode_gid=payment_paymode_gid where paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.Add("@Sup_Gid", SqlDbType.Int).Value = Convert.ToInt32(empSupgid);
                    cmd.Parameters.Add("@Account_no", SqlDbType.VarChar).Value = AccNo.ToString();
                    cmd.Parameters.AddWithValue("@ACTION", "GetBenificary_sup");
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Benifiary = dt.Rows[0]["payment_beneficiaryname"].ToString();
                return Benifiary;
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
            //throw new NotImplementedException();
        }
     
        public List<GetRef> GetRefant(string empsup)
        {
            List<GetRef> objCountrytype = new List<GetRef>();
            try
            {
                if (empsup == "Employee")
                {
                    empsup = "E";
                }
                else if (empsup == "Supplier")
                {
                    empsup = "S";
                }
                else if (empsup == "Petty Cash")
                {
                    empsup = "E";
                }

                GetRef objModel;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                if (empsup == "E")
                {
                    //cmd = new SqlCommand("SELECT payment_accountno FROM asms_trn_tpayment join iem_mst_tpaymode on paymode_gid=payment_paymode_gid WHERE payment_isremoved='N' and paymode_employee_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetRefant_emp");
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (empsup == "S")
                {
                    //cmd = new SqlCommand("SELECT payment_accountno FROM asms_trn_tpayment join iem_mst_tpaymode on paymode_gid=payment_paymode_gid WHERE payment_isremoved='N' and paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetRefant_sup");
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetRef();
                    objModel.payment_accountno = (dt.Rows[i]["payment_accountno"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }

        }
        public List<GetAttach> Getattach()
        {
            List<GetAttach> objCountrytype = new List<GetAttach>();
            try
            {

                GetAttach objModel;
                GetConnection();
                DataTable dt = new DataTable();
                // SqlCommand cmd = new SqlCommand("select attachmenttype_gid,attachmenttype_name from iem_mst_tattachmenttype  where attachmenttype_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Getattach");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetAttach();
                    objModel.attachment_attachmenttype_gid = Convert.ToInt32(dt.Rows[i]["attachmenttype_gid"].ToString());
                    objModel.attachment_type = Convert.ToString(dt.Rows[i]["attachmenttype_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }
        //correction dhasarathan
        public List<GetPaymode> GetPaymode(string empsup, string empSupgid)
        {
            List<GetPaymode> objCountrytype = new List<GetPaymode>();
            try
            {
                //if (empsup == "Employee")
                //{
                //    empsup = "E";
                //}
                //else if (empsup == "Supplier")
                //{
                //    empsup = "S";
                //}
                //else if (empsup == "Petty Cash")
                //{
                //    empsup = "E";
                //}
                if(empsup.ToString()=="E" || empsup.ToString()=="P")
                {
                    empsup = "E";
                }
                else
                {
                    empsup = "S";
                }
                GetPaymode objModel;

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                if (empsup == "E")
                {
                    //cmd = new SqlCommand("select paymode_gid,paymode_name from iem_mst_tpaymode where paymode_employee_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetPaymode_emp");
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (empsup == "S")
                {
                    //cmd = new SqlCommand("select distinct paymode_gid,paymode_name from  iem_mst_tpaymode join asms_trn_tpayment on paymode_gid=payment_paymode_gid where paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.Add("@emp_gid", SqlDbType.Int).Value = Convert.ToInt32(empSupgid);
                    cmd.Parameters.AddWithValue("@ACTION", "GetPaymode_sup");
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetPaymode();
                    objModel.paymode_Code = (dt.Rows[i]["paymode_code"].ToString());
                    objModel.paymode_gid = Convert.ToInt32(dt.Rows[i]["paymode_id"]);
                    objModel.paymode_name = Convert.ToString(dt.Rows[i]["paymode_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }

        public DataTable GetArfPaymode()
        {
            DataTable dt = new DataTable();
            try
            {
                List<GetAdvancetype> objCountrytype = new List<GetAdvancetype>();
                GetAdvancetype objModel;
                GetConnection();
                // SqlCommand cmd = new SqlCommand("select * from iem_trn_tecfarf where 1=1", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "GetArfPaymode");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {

            }
            return dt;
            //throw new NotImplementedException();
        }


        public IEnumerable<EOW_arfraising> Getarfadvacedetails()
        {
            DataTable dt = new DataTable();
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            try
            {
                EOW_arfraising objModel;
                GetConnection();
                //SqlCommand cmd = new SqlCommand("select * from iem_trn_tecfarf where ecfarf_isremoved='N' and 1=2", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Getarfadvacedetails");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();

                    objModel.ecfarf_gid = int.Parse(dt.Rows[i]["ecfarf_gid"].ToString());
                    objModel.ecfarf_advancetype_gid = int.Parse(dt.Rows[i]["ecfarf_advancetype_gid"].ToString());
                    objModel.ecfarf_dr_gl_no = dt.Rows[i]["ecfarf_dr_gl_no"].ToString();
                    objModel.ecfarf_desc = dt.Rows[i]["ecfarf_desc"].ToString();
                    objModel.ecfarf_amount = dt.Rows[i]["ecfarf_amount"].ToString();
                    objModel.ecfarf_exception = dt.Rows[i]["ecfarf_exception"].ToString();
                    objModel.ecfarf_po_no = dt.Rows[i]["ecfarf_po_no"].ToString();
                    objModel.ecfarf_cbf_no = dt.Rows[i]["ecfarf_cbf_no"].ToString();
                    objModel.ecfarf_fc_code = dt.Rows[i]["ecfarf_fc_code"].ToString();
                    objModel.ecfarf_cc_code = dt.Rows[i]["ecfarf_cc_code"].ToString();
                    objModel.ecfarf_product_code = dt.Rows[i]["ecfarf_product_code"].ToString();
                    objModel.ecfarf_ou_code = dt.Rows[i]["ecfarf_ou_code"].ToString();
                    objModel.ecfarf_liq_date = dt.Rows[i]["ecfarf_liq_date"].ToString();
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
        }
        // throw new NotImplementedException();


        public IEnumerable<EOW_arfraising> Getarfpaymentdetails()
        {
            DataTable dt = new DataTable();
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            try
            {
                EOW_arfraising objModel;
                GetConnection();
                //SqlCommand cmd = new SqlCommand("select * from iem_trn_tcreditline where 1=2", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Getarfpaymentdetails");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();

                    objModel.creditline_gid = int.Parse(dt.Rows[i]["creditline_gid"].ToString());
                    objModel.creditline_pay_mode = dt.Rows[i]["creditline_pay_mode"].ToString();
                    objModel.creditline_ref_no = dt.Rows[i]["creditline_ref_no"].ToString();
                    objModel.creditline_beneficiary = dt.Rows[i]["creditline_beneficiary"].ToString();
                    objModel.creditline_desc = dt.Rows[i]["creditline_desc"].ToString();
                    objModel.creditline_amount = dt.Rows[i]["creditline_amount"].ToString();
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            //throw new NotImplementedException();
        }
        public IEnumerable<EOW_arfraising> GetarfEmployeedetailssearch()
        {
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
                try
                {
                    EOW_arfraising objModel;
                    //SqlCommand cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_isremoved='N'", con);
                    SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetarfEmployeedetailssearch");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new EOW_arfraising();

                        objModel.employee_name = (dt.Rows[i]["employee_name"].ToString());
                        objModel.employee_code = (dt.Rows[i]["employee_code"].ToString());
                        objOrgType.Add(objModel);
                    }

                    return objOrgType;
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    return objOrgType;
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
        }
        public IEnumerable<EOW_arfraising> GetarfEmployeedetails(string Empcode)
        {
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            DataTable dt = new DataTable();
            try
            {
                List<GetPaymode> objCountrytype = new List<GetPaymode>();
                GetConnection();


                EOW_arfraising objModel;
                SqlCommand cmd = new SqlCommand();
                if (Empcode != "")
                {
                    //cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_code='" + Empcode + "' and employee_isremoved='N'", con);
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetarfEmployeedetails_emp");
                    cmd.Parameters.AddWithValue("@emp_code", Empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    // cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_isremoved='N'", con);
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetarfEmployeedetails_sup");
                    // cmd.Parameters.AddWithValue("@emp_code", Empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();

                    objModel.employee_code = dt.Rows[i]["employee_code"].ToString();
                    objModel.employee_name = dt.Rows[i]["employee_name"].ToString();
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {
                con.Close();
            }

        }
        public IEnumerable<EOW_arfraising> Getarfattachementdetails()
        {
            EOW_arfraising objModel;
            DataTable dt = new DataTable();
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            try
            {

                GetConnection();
                // SqlCommand cmd = new SqlCommand("select * from iem_trn_tattachment where 1=2", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "Getarfattachementdetails_show");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();

                    objModel.attachment_gid = int.Parse(dt.Rows[i]["attachment_gid"].ToString());
                    objModel.attachment_filename = dt.Rows[i]["attachment_filename"].ToString();
                    objModel.attachment_attachmenttype_gid = int.Parse(dt.Rows[i]["attachment_attachmenttype_gid"].ToString());
                    objModel.attachment_desc = dt.Rows[i]["attachment_desc"].ToString();
                    objModel.attachment_date = dt.Rows[i]["attachment_date"].ToString();
                    objModel.attachment_by = int.Parse(dt.Rows[i]["attachment_by"].ToString());
                    objOrgType.Add(objModel);
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {

            }
            return objOrgType;
            // throw new NotImplementedException();
        }
        public IEnumerable<EOW_arfraising> pono(string gid)
        {
            EOW_arfraising objModel;
            DataTable dt = new DataTable();
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            try
            {

                GetConnection();
                //SqlCommand cmd = new SqlCommand("select poheader_pono from dbo.fb_trn_tpoheader where poheader_isremoved='N' and poheader_raisor_gid=" + cmnfun.GetLoginUserGid() + "", con);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da = new SqlDataAdapter(cmd);
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.Int).Value = gid;
                //cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = pono;
                //cmd.Parameters.Add("@pototal", SqlDbType.VarChar).Value = pototal;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EOW_arfraising();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();
                    objModel.pototal = row["poheader_over_total"].ToString();
                    objOrgType.Add(objModel);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {

            }
            return objOrgType;
        }
        public IEnumerable<EOW_arfraising> cbfno(string gid)
        {
            EOW_arfraising objModel;
            DataTable dt = new DataTable();
            List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();
            try
            {

                GetConnection();
                //SqlCommand cmd = new SqlCommand("select cbfheader_cbfno from dbo.fb_trn_tcbfheader where cbfheader_isremoved='N' and cbfheader_rasier_gid=" + gid + "", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "cbfno");
                cmd.Parameters.AddWithValue("@emp_code", gid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_arfraising();

                    objModel.ecfarf_cbf_no = (dt.Rows[i]["cbfheader_cbfno"].ToString());
                    objOrgType.Add(objModel);
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {

            }
            return objOrgType;
        }
        //public DataTable cbfno()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        List<GetPaymode> objCountrytype = new List<GetPaymode>();
        //        GetConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select cbfheader_cbfno from dbo.fb_trn_tcbfheader where cbfheader_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }
        //    return dt;
        //}
        //public DataTable pono()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        List<GetPaymode> objCountrytype = new List<GetPaymode>();
        //        GetConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select poheader_pono from dbo.fb_trn_tpoheader where poheader_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }
        //    return dt;
        //}


        //public DataTable GetarfEmployeedetails(string Empcode)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        List<GetPaymode> objCountrytype = new List<GetPaymode>();
        //        GetConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        if (Empcode != "")
        //        {
        //            cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_code='" + Empcode + "' and employee_isremoved='N'", con);
        //        }
        //        else
        //        {
        //            cmd = new SqlCommand("select employee_code,employee_name from iem_mst_temployee where employee_isremoved='N'", con);
        //        }
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }
        //    return dt;
        //}
        public string _pono(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand("  select poheader_pono from fb_trn_tpoheader where poheader_pono='" + Dcoverify + "'  and poheader_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "_pono");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["poheader_pono"].ToString();
                }

                return Docresult;
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
        public string productcode(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand("  select product_code from iem_mst_tproduct where product_code='" + Dcoverify + "' and product_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "productcode");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["product_code"].ToString();
                }

                return Docresult;
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
        public string cccode(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand(" select fccc_cc_code from iem_mst_tfccc where fccc_cc_code='" + Dcoverify + "' and fccc_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "cccode");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["fccc_cc_code"].ToString();
                }

                return Docresult;
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
        public string cc_po(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand(" select cbfheader_cbfno,poheader_gid from fb_trn_tpodetails join fb_trn_tpoheader on poheader_gid=podetails_pohead_gid join fb_trn_tcbfheader on cbfheader_gid=podetails_cbfdet_gid where   poheader_gid=" + Dcoverify + " and poheader_isremoved='N' and podetails_isremoved='N' and cbfheader_isremoved='N' ", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "cc_po");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["cbfheader_cbfno"].ToString();
                }

                return Docresult;
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
        public string fccode(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand(" select fccc_fc_code from iem_mst_tfccc where fccc_fc_code='" + Dcoverify + "' and fccc_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "fccode");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["fccc_fc_code"].ToString();
                }

                return Docresult;
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
        public EOW_arfraising proxy(string proxyid)
        {
            EOW_arfraising objModel = new EOW_arfraising();
            try
            {
                List<EOW_arfraising> objOrgType = new List<EOW_arfraising>();


                GetConnection();
                DataTable dt = new DataTable();
                //SqlCommand cmd = new SqlCommand("select employee_code,employee_name from iem_mst_tproxy join iem_mst_temployee on employee_gid=proxy_to where proxy_by=" + proxyid + " and proxy_isremoved='N' and employee_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "proxy");
                cmd.Parameters.AddWithValue("@emp_code", proxyid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                objModel = new EOW_arfraising()
                {
                    ecf_raisersup = (dt.Rows[0]["employee_code"].ToString()),
                    ecf_raisername = (dt.Rows[0]["employee_name"].ToString())

                };
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }

        }

        public string GetDecnote(string subsype, string type)
        {
            string Emp_Msg = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("select declnote_desc from iem_mst_tdeclnote where declnote_docsubtype_gid='" + subsype + "' and declnote_at='" + type + "' and declnote_active='Y' and declnote_isremoved='N'", con);
                //cmd = new SqlCommand("select declnote_onsubmission from iem_mst_tdeclnote where declnote_docsubtype_gid='" + subsype + "'  and declnote_active='Y' and declnote_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "GetDecnote");
                cmd.Parameters.AddWithValue("@emp_code", subsype);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["declnote_onsubmission"].ToString());
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string oucode(string Dcoverify)
        {
            try
            {
                string Docresult = "";

                GetConnection();
                DataTable dt = new DataTable();

                //SqlCommand cmd = new SqlCommand(" select ou_code from iem_mst_tou where ou_code='" + Dcoverify + "' and ou_isremoved='N'", con);
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "oucode");
                cmd.Parameters.AddWithValue("@emp_code", Dcoverify);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Docresult = dt.Rows[0]["ou_code"].ToString();
                }

                return Docresult;
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
        public IEnumerable<EOW_arfraising> SelectSupplier()
        {
            List<EOW_arfraising> emp = new List<EOW_arfraising>();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();

                EOW_arfraising objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUPPLIERDETAILS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EOW_arfraising();
                    objModel.ecf_supplier_gid = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel.supplierheader_suppliercode = row["supplierheader_suppliercode"].ToString();
                    objModel.supplierheader_name = row["supplierheader_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<EOW_arfraising> SelectSupplierSearch(string SupplierName, string SupplierCode)
        {
            List<EOW_arfraising> emp = new List<EOW_arfraising>();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();

                EOW_arfraising objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = SupplierName;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = SupplierCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUPPLIERDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EOW_arfraising();
                    objModel.ecf_supplier_gid = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel.supplierheader_suppliercode = row["supplierheader_suppliercode"].ToString();
                    objModel.supplierheader_name = row["supplierheader_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetarfSupplierdetails(string Suppcode)
        {
            DataTable dt = new DataTable();
            try
            {
                List<GetPaymode> objCountrytype = new List<GetPaymode>();
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                //if (Suppcode != "")
                //{
                //    cmd = new SqlCommand("select supplierheader_suppliercode,supplierheader_name,supplierheader_gid from asms_trn_tsupplierheader where supplierheader_suppliercode='" + Suppcode + "' and supplierheader_isremoved='N'", con);
                //}
                //else
                {
                    //cmd = new SqlCommand("select supplierheader_suppliercode,supplierheader_name,supplierheader_gid from asms_trn_tsupplierheader where supplierheader_isremoved='N'", con);
                    cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetarfSupplierdetails");
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {

            }
            return dt;

        }
        public DataTable GetEcfsupDetails(string Ecfempsupgid, string ecf)
        {
            DataTable dt = new DataTable();
            try
            {
                List<GetPaymode> objCountrytype = new List<GetPaymode>();
                GetConnection();
                {
                    //SqlCommand cmd = new SqlCommand("select ecf_no,ecf_date,supplierheader_name,ecf_amount,ecf_status from iem_trn_tecf join asms_trn_tsupplierheader on  ecf_supplier_gid=supplierheader_gid where supplierheader_suppliercode='" + Ecfempsupgid + "' and ecf_doctype_gid=2 and ecf_gid!=" + ecf + " and ecf_no!=''  and ecf_isremoved='N' and supplierheader_isremoved='N' ", con);
                    SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetEcfsupDetails");
                    cmd.Parameters.AddWithValue("@emp_code", Ecfempsupgid);
                    cmd.Parameters.AddWithValue("@gid", ecf);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {

            }
            return dt;

        }
        public DataTable GetEcfDetails(string Ecfempsupgid, string ecf)
        {
            DataTable dt = new DataTable();
            try
            {
                List<GetPaymode> objCountrytype = new List<GetPaymode>();
                GetConnection();
                //if (ecfmode == "A")
                {
                    //SqlCommand cmd = new SqlCommand("select ecf_no,ecf_date,employee_name,ecf_amount,ecf_status from iem_trn_tecf join iem_mst_temployee on ecf_employee_gid=employee_gid where employee_code='" + Ecfempsupgid + "' and ecf_doctype_gid=2  and ecf_gid!=" + ecf + " and ecf_no!=''  and ecf_isremoved='N'  and employee_isremoved='N'", con);
                    SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd.Parameters.AddWithValue("@ACTION", "GetEcfDetails");
                    cmd.Parameters.AddWithValue("@emp_code", Ecfempsupgid);
                    cmd.Parameters.AddWithValue("@gid", ecf);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                //else
                //{
                //    ecfmode = "S";
                //    SqlCommand cmd = new SqlCommand("select ecf_no,ecf_date,ecf_raiser,ecf_amount from iem_trn_tecf where ecf_supplier_gid='" + Ecfempsupgid + "' and ecf_create_mode='" + ecfmode + "'", con);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    da = new SqlDataAdapter(cmd);
                //    da.Fill(dt);
                //}

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {

            }
            return dt;
            //throw new NotImplementedException();
        }
        public string InsertEmpAtt(HttpPostedFileBase savefile, EOW_arfraising EmployeeeExpense)
        {
            string filename = "";
            string Emp_Msg = "";
            try
            {
                string desvl = "";
                if (EmployeeeExpense.attachment_desc != "" && EmployeeeExpense.attachment_desc != null)
                {
                    desvl = EmployeeeExpense.attachment_desc;
                }

                filename = savefile.FileName.ToString();
                int index = filename.LastIndexOf("\\");
                if (index != -1)
                {
                    string[] seqNum = new string[] { filename.Substring(0, index), filename.Substring(index + 1) };
                    if (seqNum.Length == 2)
                    {
                        filename = seqNum[1].ToString();
                    }
                    else
                    {
                        filename = savefile.FileName.ToString();
                    }
                }
                else
                {
                    filename = savefile.FileName.ToString();
                }

                string[,] codes = new string[,]
	               {
        {"attachment_ref_flag","1" },
	    {"attachment_ref_gid", Convert.ToString(EmployeeeExpense.attachment_refgid)},
        {"attachment_filename",objCmnFunctions.Getreplacesinglequotes(filename.ToString()) },
        {"attachment_attachmenttype_gid", Convert.ToString( EmployeeeExpense.attachment_attachmenttype_gid)},
	    {"attachment_desc", cmnfun.Getreplacesinglequotes(desvl)},
	    {"attachment_date", "sysdatetime()"},
        {"attachment_by", Convert.ToString(EmployeeeExpense.attachment_by)},
        {"attachment_isremoved", "N"}
                  };
                string tname = "iem_trn_tattachment";

                string insertcommend = objCommonIUD.InsertCommon(codes, tname);

                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("select  MAX(attachment_gid) as attachment_gid from iem_trn_tattachment where attachment_ref_gid='" + EmployeeeExpense.attachment_refgid + "' and attachment_by='" + EmployeeeExpense.attachment_by + "' and attachment_isremoved='N'  ", con);
                cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "insert_attach");
                cmd.Parameters.AddWithValue("@emp_code", EmployeeeExpense.attachment_by);
                cmd.Parameters.AddWithValue("@gid", EmployeeeExpense.attachment_refgid);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["attachment_gid"].ToString());
                }
                return Emp_Msg;
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
        public string downloadAttachment(int EmployeeeAttachmentGID)
        {
            string Emp_Msg = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                // cmd = new SqlCommand("select  attachment_filename from iem_trn_tattachment where  attachment_gid='" + EmployeeeAttachmentGID + "' and attachment_isremoved='N'  ", con);
                cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "download_attach");
                // cmd.Parameters.AddWithValue("@emp_code", EmployeeeExpense.attachment_by);
                cmd.Parameters.AddWithValue("@gid", EmployeeeAttachmentGID);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["attachment_filename"].ToString());
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string DeleteAttachment(int attachId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"attachment_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"attachment_gid", attachId.ToString ()}
            };
                string tblname = "iem_trn_tattachment";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                result = "NotExists";
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
        public string DeleteAdvance(EOW_arfraising advance)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"ecfarf_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"ecfarf_gid",  advance.ecfarf_gid.ToString()}
            };
                string tblname = "iem_trn_tecfarf";
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(" select ecfarf_ecf_gid,invoice_gid from iem_trn_tecfarf join iem_trn_tinvoice on ecfarf_ecf_gid=invoice_ecf_gid where ecfarf_gid='" + advance.ecfarf_gid + "' ", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                string[,] codes1 = new string[,]
	       {
                 {"ecfdebitline_isremoved", "Y"}
	            
           };
                string[,] whrcol1 = new string[,]
	        {
                {"ecfdebitline_ecf_gid", dt.Rows[0]["ecfarf_ecf_gid"].ToString()},
                {"ecfdebitline_invoice_gid", dt.Rows[0]["invoice_gid"].ToString()},
                {"ecfdebitline_amount", advance.ecfarf_amount.ToString()}

            };
                string tblname1 = "iem_trn_tecfdebitline";




                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                string deletetbl1 = delecomm.DeleteCommon(codes1, whrcol1, tblname1);
                result = "NotExists";
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
        public string DeletePayment(int paymentId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"creditline_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"creditline_gid", paymentId.ToString ()}
            };
                string tblname = "iem_trn_tcreditline";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                result = "NotExists";
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
        public string InsertEmployeeePaymentbasic(string EmployeeeGid, string ecfgid, string amt)
        {
            string bankgid = "0";
            string bankgnno = "123";
            string result = "";
            string invoice_gid = "0";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = EmployeeeGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "EmpAccdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GetConnection();
                    DataTable dtpaybank = new DataTable();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = EmployeeeGid;
                    cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[0]["emp_paymode"].ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtpaybank);
                    if (dtpaybank.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                        {
                            bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                        }
                        if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                        {
                            bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                        }
                    }

                    DataTable dtup = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_com_invoicedetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "invoicemaxgiduseecf";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtup);
                    if (dtup.Rows.Count > 0)
                    {
                        invoice_gid = Convert.ToString(dtup.Rows[0]["invoice_gid"].ToString());
                    }

                    string[,] codes = new string[,]
	               {
        {"ecfcreditline_ecf_gid",ecfgid },
	    {"ecfcreditline_invoice_gid", invoice_gid},
        {"ecfcreditline_pay_mode",Convert.ToString(dt.Rows[0]["emp_paymode"].ToString())},
	    {"ecfcreditline_ref_no", Convert.ToString(dt.Rows[0]["employee_era_acc_no"].ToString())},
        {"ecfcreditline_beneficiary",objCmnFunctions.Getreplacesinglequotes(Convert.ToString(dt.Rows[0]["employee_name"].ToString())) },
	    {"ecfcreditline_bank_gid", bankgid},
        {"ecfcreditline_ifsc_code",Convert.ToString(dt.Rows[0]["employee_era_ifsc_code"].ToString())},
	    {"ecfcreditline_gl_no", bankgnno},
        {"ecfcreditline_desc", Convert.ToString(dt.Rows[0]["emp_paymodedesc"].ToString())},
        {"ecfcreditline_amount",amt }      
                  };

                    string tname = "iem_trn_tecfcreditline";
                    string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                    result = insertcommend.ToString();
                }

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
                da.Dispose();
            }
        }
        public string InsertEmployeeePaymentbasicupdate(string EmployeeeGid, string ecfgid, string amt)
        {
            string bankgid = "0";
            string bankgnno = "123";
            string result = "";
            string invoice_gid = "0";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = EmployeeeGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "EmpAccdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    GetConnection();
                    DataTable dtpaybank = new DataTable();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = EmployeeeGid;
                    cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[0]["emp_paymode"].ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtpaybank);
                    if (dtpaybank.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                        {
                            bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                        }
                        if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                        {
                            bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                        }
                    }

                    DataTable dtup = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_com_invoicedetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "invoicemaxgiduseecf";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtup);
                    if (dtup.Rows.Count > 0)
                    {
                        invoice_gid = Convert.ToString(dtup.Rows[0]["invoice_gid"].ToString());
                    }

                    string[,] codes = new string[,]
	               {
        {"ecfcreditline_ecf_gid",ecfgid },
	    {"ecfcreditline_invoice_gid", invoice_gid},
	    {"ecfcreditline_ref_no", Convert.ToString(dt.Rows[0]["employee_era_acc_no"].ToString())},
        {"ecfcreditline_beneficiary",Convert.ToString(dt.Rows[0]["employee_name"].ToString()) },
	    {"ecfcreditline_bank_gid", bankgid},
        {"ecfcreditline_ifsc_code",Convert.ToString(dt.Rows[0]["employee_era_ifsc_code"].ToString())},
	    {"ecfcreditline_gl_no", bankgnno},
        {"ecfcreditline_desc", Convert.ToString(dt.Rows[0]["emp_paymodedesc"].ToString())},
        {"ecfcreditline_amount",amt }      
                  };

                    string tname = "iem_trn_tecfcreditline";

                    string[,] whcosup = new string[,]
	               {
        {"ecfcreditline_ecf_gid",ecfgid },
	    {"ecfcreditline_pay_mode", Convert.ToString(dt.Rows[0]["emp_paymode"].ToString())}
                  };

                    string insertcommend = objCommonIUD.UpdateCommon(codes, whcosup, tname);
                    result = insertcommend.ToString();
                }
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
                da.Dispose();
            }
        }
        public DataTable Getpaymodedata(string valatate, string action)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valatate;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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
            return dt;
        }
        public string GetStatusexcel(string valatate, string action)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valatate.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
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
                da.Dispose();
            }
            return result;
        }
        public string insertsuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername)
        {
            string bankgid = "0";
            string bankgnno = "123";
            string invoice_gid = "0";
            string Emp_Msg = "";
            try
            {
                DataTable dtup = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_invoicedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "invoicemaxgiduseecf";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtup);
                if (dtup.Rows.Count > 0)
                {
                    invoice_gid = Convert.ToString(dtup.Rows[0]["invoice_gid"].ToString());
                }

                //GetConnection();
                //DataTable dtdatds = new DataTable();
                //string datas = "select top 1 c.paymode_code,c.paymode_name,b.payment_beneficiaryname,b.payment_accountno,b.payment_bank_gid,b.payment_ifsccode  ";
                //datas += " from asms_trn_tsupplierheader  as a";
                //datas += " inner join  asms_trn_tpayment as b on b.payment_supplierheader_gid=a.supplierheader_gid";
                //datas += " inner join iem_mst_tpaymode as c on c.paymode_gid=b.payment_paymode_gid";
                //datas += " where  payment_isremoved='N' and c.paymode_code='EFT' and supplierheader_gid='" + suppliergids + "' and c.paymode_isremoved='N' and a.supplierheader_isremoved='N'";
                //cmd = new SqlCommand(datas, con);
                //cmd.CommandType = CommandType.Text;
                //da = new SqlDataAdapter(cmd);
                //da.Fill(dtdatds);

                DataTable dtdatds = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_sup_getpaymodedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Supplierid", SqlDbType.VarChar).Value = suppliergids;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "CHECKEFT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtdatds);
                if (dtdatds.Rows.Count > 0)
                {
                    GetConnection();
                    DataTable dtpaybank = new DataTable();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                    cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = Convert.ToString(dtdatds.Rows[0]["paymode_code"].ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtpaybank);
                    if (dtpaybank.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                        {
                            bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                        }
                        if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                        {
                            bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                        }
                    }

                    string[,] codes = new string[,]
	                       {       
                            {"ecfcreditline_ecf_gid",ecfgid },
	                        {"ecfcreditline_invoice_gid", invoice_gid},
                            {"ecfcreditline_pay_mode",Convert.ToString(dtdatds.Rows[0]["paymode_code"].ToString())},    
	                        {"ecfcreditline_ref_no", Convert.ToString(dtdatds.Rows[0]["payment_accountno"].ToString())},
                            {"ecfcreditline_beneficiary",Convert.ToString(dtdatds.Rows[0]["payment_beneficiaryname"].ToString()) },
	                        {"ecfcreditline_bank_gid", bankgid},
                            {"ecfcreditline_ifsc_code",Convert.ToString(dtdatds.Rows[0]["payment_ifsccode"].ToString())},
	                        {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount",tolrowinvoiceamt.ToString() }      
                          };

                    string tname = "iem_trn_tecfcreditline";

                    string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                }
                else
                {
                    //GetConnection();
                    //DataTable dtdatd = new DataTable();
                    //string data = "select top 1 c.paymode_code,c.paymode_name,b.payment_beneficiaryname,b.payment_accountno,b.payment_bank_gid,b.payment_ifsccode  ";
                    //data += " from asms_trn_tsupplierheader  as a";
                    //data += " inner join  asms_trn_tpayment as b on b.payment_supplierheader_gid=a.supplierheader_gid";
                    //data += " inner join iem_mst_tpaymode as c on c.paymode_gid=b.payment_paymode_gid";
                    //data += " where  payment_isremoved='N' and supplierheader_gid='" + suppliergids + "' and c.paymode_isremoved='N' and a.supplierheader_isremoved='N'";
                    //cmd = new SqlCommand(data, con);
                    //cmd.CommandType = CommandType.Text;
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtdatd);
                    DataTable dtdatd = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_sup_getpaymodedetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Supplierid", SqlDbType.VarChar).Value = suppliergids;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "OTHER";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdatd);
                    if (dtdatd.Rows.Count > 0)
                    {
                        GetConnection();
                        DataTable dtpaybank = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                        cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = Convert.ToString(dtdatd.Rows[0]["paymode_code"].ToString());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtpaybank);
                        if (dtpaybank.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                            {
                                bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                            }
                            if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                            {
                                bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                            }
                        }

                        string[,] codes = new string[,]
	                       {       
                            {"ecfcreditline_ecf_gid",ecfgid },
	                        {"ecfcreditline_invoice_gid", invoice_gid},
                            {"ecfcreditline_pay_mode",Convert.ToString(dtdatd.Rows[0]["paymode_code"].ToString())},    
	                        {"ecfcreditline_ref_no", Convert.ToString(dtdatd.Rows[0]["payment_accountno"].ToString())},
                            {"ecfcreditline_beneficiary",Convert.ToString(dtdatd.Rows[0]["payment_beneficiaryname"].ToString()) },
	                        {"ecfcreditline_bank_gid", bankgid},
                            {"ecfcreditline_ifsc_code",Convert.ToString(dtdatd.Rows[0]["payment_ifsccode"].ToString())},
	                        {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount",tolrowinvoiceamt.ToString() }      
                          };

                        string tname = "iem_trn_tecfcreditline";

                        string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                    }
                    else
                    {
                        GetConnection();
                        DataTable dtpaybank = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                        cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = "CHQ";
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtpaybank);
                        if (dtpaybank.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                            {
                                bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                            }
                            if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                            {
                                bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                            }
                        }

                        string[,] codesc = new string[,]
                                       {
                            {"ecfcreditline_ecf_gid",ecfgid },
                            {"ecfcreditline_invoice_gid", invoice_gid},
                            {"ecfcreditline_pay_mode","CHQ" },
                            {"ecfcreditline_ref_no", ""},
                            {"ecfcreditline_beneficiary",suppliername.ToString().Trim() },
                            {"ecfcreditline_bank_gid",bankgid},
                            {"ecfcreditline_ifsc_code",""},
                            {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount", tolrowinvoiceamt.ToString() }      
                                      };

                        string tnamec = "iem_trn_tecfcreditline";

                        string insertcommendc = objCommonIUD.InsertCommon(codesc, tnamec);
                    }
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string updatesuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername)
        {
            string bankgid = "0";
            string bankgnno = "123";
            string invoice_gid = "0";
            string Emp_Msg = "";
            try
            {
                DataTable dtup = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_invoicedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "invoicemaxgiduseecf";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtup);
                if (dtup.Rows.Count > 0)
                {
                    invoice_gid = Convert.ToString(dtup.Rows[0]["invoice_gid"].ToString());
                }

                //GetConnection();
                //DataTable dtdatds = new DataTable();
                //string datas = "select top 1 c.paymode_code,c.paymode_name,b.payment_beneficiaryname,b.payment_accountno,b.payment_bank_gid,b.payment_ifsccode  ";
                //datas += " from asms_trn_tsupplierheader  as a";
                //datas += " inner join  asms_trn_tpayment as b on b.payment_supplierheader_gid=a.supplierheader_gid";
                //datas += " inner join iem_mst_tpaymode as c on c.paymode_gid=b.payment_paymode_gid";
                //datas += " where  payment_isremoved='N' and c.paymode_code='EFT' and supplierheader_gid='" + suppliergids + "' and c.paymode_isremoved='N' and a.supplierheader_isremoved='N'";
                //cmd = new SqlCommand(datas, con);
                //cmd.CommandType = CommandType.Text;
                //da = new SqlDataAdapter(cmd);
                //da.Fill(dtdatds);

                DataTable dtdatds = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_sup_getpaymodedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Supplierid", SqlDbType.VarChar).Value = suppliergids;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "CHECKEFT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtdatds);
                if (dtdatds.Rows.Count > 0)
                {
                    GetConnection();
                    DataTable dtpaybank = new DataTable();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                    cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = "EFT";
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtpaybank);
                    if (dtpaybank.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                        {
                            bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                        }
                        if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                        {
                            bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                        }
                    }

                    string[,] codes = new string[,]
	                       {                                 
	                        {"ecfcreditline_invoice_gid", invoice_gid},                             
	                        {"ecfcreditline_ref_no", Convert.ToString(dtdatds.Rows[0]["payment_accountno"].ToString())},
                            {"ecfcreditline_beneficiary",Convert.ToString(dtdatds.Rows[0]["payment_beneficiaryname"].ToString()) },
	                        {"ecfcreditline_bank_gid", bankgid},
                            {"ecfcreditline_ifsc_code",Convert.ToString(dtdatds.Rows[0]["payment_ifsccode"].ToString())},
	                        {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount",tolrowinvoiceamt.ToString() }      
                          };

                    string[,] codeswh = new string[,]
	                       {
                             {"ecfcreditline_ecf_gid",ecfgid },
                            {"ecfcreditline_pay_mode","EFT"},    
                          };
                    string tname = "iem_trn_tecfcreditline";
                    string insertcommend = objCommonIUD.UpdateCommon(codes, codeswh, tname);
                }
                else
                {
                    //GetConnection();
                    //DataTable dtdatd = new DataTable();
                    //string data = "select top 1 c.paymode_code,c.paymode_name,b.payment_beneficiaryname,b.payment_accountno,b.payment_bank_gid,b.payment_ifsccode  ";
                    //data += " from asms_trn_tsupplierheader  as a";
                    //data += " inner join  asms_trn_tpayment as b on b.payment_supplierheader_gid=a.supplierheader_gid";
                    //data += " inner join iem_mst_tpaymode as c on c.paymode_gid=b.payment_paymode_gid";
                    //data += " where  payment_isremoved='N' and supplierheader_gid='" + suppliergids + "' and c.paymode_isremoved='N' and a.supplierheader_isremoved='N'";
                    //cmd = new SqlCommand(data, con);
                    //cmd.CommandType = CommandType.Text;
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtdatd);
                    DataTable dtdatd = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_sup_getpaymodedetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Supplierid", SqlDbType.VarChar).Value = suppliergids;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "OTHER";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdatd);
                    if (dtdatd.Rows.Count > 0)
                    {
                        GetConnection();
                        DataTable dtpaybank = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                        cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = Convert.ToString(dtdatd.Rows[0]["paymode_code"].ToString());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtpaybank);
                        if (dtpaybank.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                            {
                                bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                            }
                            if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                            {
                                bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                            }
                        }

                        string[,] codes = new string[,]
	                       {       
	                        {"ecfcreditline_invoice_gid", invoice_gid},    
	                        {"ecfcreditline_ref_no", Convert.ToString(dtdatd.Rows[0]["payment_accountno"].ToString())},
                            {"ecfcreditline_beneficiary",Convert.ToString(dtdatd.Rows[0]["payment_beneficiaryname"].ToString()) },
	                        {"ecfcreditline_bank_gid", bankgid},
                            {"ecfcreditline_ifsc_code",Convert.ToString(dtdatd.Rows[0]["payment_ifsccode"].ToString())},
	                        {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount",tolrowinvoiceamt.ToString() }      
                          };

                        string[,] codeswh = new string[,]
	                       {
                             {"ecfcreditline_ecf_gid",ecfgid },
                            {"ecfcreditline_pay_mode",Convert.ToString(dtdatd.Rows[0]["paymode_code"].ToString())},    
                          };
                        string tname = "iem_trn_tecfcreditline";
                        string insertcommend = objCommonIUD.UpdateCommon(codes, codeswh, tname);
                    }
                    else
                    {
                        GetConnection();
                        DataTable dtpaybank = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = suppliergids;
                        cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = "CHQ";
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtpaybank);
                        if (dtpaybank.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString()) != "")
                            {
                                bankgid = Convert.ToString(dtpaybank.Rows[0]["bank_gid"].ToString());
                            }
                            if (Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString()) != "")
                            {
                                bankgnno = Convert.ToString(dtpaybank.Rows[0]["bankgl_no"].ToString());
                            }
                        }

                        string[,] codesc = new string[,]
                                       {
                            {"ecfcreditline_invoice_gid", invoice_gid},
                            {"ecfcreditline_ref_no", ""},
                            {"ecfcreditline_beneficiary",suppliername.ToString().Trim() },
                            {"ecfcreditline_bank_gid", bankgid},
                            {"ecfcreditline_ifsc_code",""},
                            {"ecfcreditline_gl_no", bankgnno},
                            {"ecfcreditline_desc", "Supplier Account"},
                            {"ecfcreditline_amount", tolrowinvoiceamt.ToString() }      
                                      };

                        string[,] codeswh = new string[,]
	                       {
                             {"ecfcreditline_ecf_gid",ecfgid },
                            {"ecfcreditline_pay_mode","CHQ"},    
                          };
                        string tname = "iem_trn_tecfcreditline";
                        string insertcommend = objCommonIUD.UpdateCommon(codesc, codeswh, tname);
                    }
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string updatePayment(EOW_arfraising Payment)
        {
            string result = "";
            try
            {
                string glgid = "";
                CommonIUD comm = new CommonIUD();
                GetConnection();
                string enm = "";
                CommonIUD delecomm = new CommonIUD();
                string benifi = Convert.ToString(Payment.creditline_beneficiary);
                if (benifi == null)
                {
                    benifi = "";
                }
                string desvl = "";
                if (Payment.creditline_desc != "" && Payment.creditline_desc != null)
                {
                    desvl = Payment.creditline_desc;
                }
                string[,] codes = new string[,]
                 
                   {
                         // {"creditline_ecf_gid", ECFGID},
                          {"ecfcreditline_pay_mode", Payment.creditline_pay_mode.ToString()},
                         {"ecfcreditline_ref_no",Convert.ToString(Payment.creditline_ref_no)},
                         {"ecfcreditline_beneficiary",benifi},
                         {"ecfcreditline_desc",cmnfun.Getreplacesinglequotes(desvl)},
                         {"ecfcreditline_amount",Payment.creditline_amount.ToString()},
                         {"ecfcreditline_invoice_gid",glgid},
                         {"ecfcreditline_gl_no",glgid}               
                  };
                string[,] whrcol = new string[,]
	                 {
                          {"ecfcreditline_gid", Payment.creditline_ecf_gid.ToString()}
            
                     };
                string tblname = "iem_trn_tecfcreditline";
                result = "NotExists";
                string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);

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
        public string Savepaymentcredit(EOW_arfraising Payment)
        {
            string result = "";
            try
            {
                string glgid = "";
                GetConnection();
                CommonIUD comm = new CommonIUD();
                string benifi = Convert.ToString(Payment.creditline_beneficiary);
                if (benifi == null)
                {
                    benifi = "";
                }
                string[,] codes = new string[,]            
                    {
                         {"creditline_ecf_gid", Payment.creditline_ecf_gid.ToString()},
                          {"creditline_pay_mode", Payment.creditline_pay_mode.ToString()},
                         {"creditline_ref_no", Convert.ToString( Payment.creditline_ref_no)},
                         {"creditline_beneficiary",benifi},
                         {"creditline_desc",Payment.creditline_desc.ToString()},
                         {"creditline_amount",Payment.creditline_amount.ToString()},
                         {"creditline_invoice_gid",glgid},
                         {"creditline_gl_no",glgid},
                         {"creditline_isremoved","N"}
                         //{"docpouch_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         //{"docpouch_insert_date","sysdatetime()"}
                    };
                string tname = "iem_trn_tcreditline";
                result = comm.InsertCommon(codes, tname);
                result = "NotExists";
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
        public string Savepayment(EOW_arfraising Payment)
        {
            string result = "";
            try
            {
                string glgid = "";
                GetConnection();
                CommonIUD comm = new CommonIUD();
                string benifi = Convert.ToString(Payment.creditline_beneficiary);
                if (benifi == null)
                {
                    benifi = "";
                }
                string desvl = "";
                if (Payment.creditline_desc != "" && Payment.creditline_desc != null)
                {
                    desvl = Payment.creditline_desc;
                }
                string[,] codes = new string[,]            
                    {
                         {"creditline_ecf_gid", Payment.creditline_ecf_gid.ToString()},
                          {"creditline_pay_mode", Payment.creditline_pay_mode.ToString()},
                         {"creditline_ref_no", Convert.ToString( Payment.creditline_ref_no)},
                         {"creditline_beneficiary",benifi},
                         {"creditline_desc", cmnfun.Getreplacesinglequotes(desvl)},                        
                         {"creditline_amount",Payment.creditline_amount.ToString()},
                         {"creditline_invoice_gid",glgid},
                         {"creditline_gl_no",glgid}
                         //{"docpouch_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         //{"docpouch_insert_date","sysdatetime()"}
                    };
                string tname = "iem_trn_tcreditline";
                result = comm.InsertCommon(codes, tname);
                result = "NotExists";
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
        public string GetStatusexcel(string valatate, string valatate1, string valatate2, string action)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valatate.ToString();
                cmd.Parameters.Add("@chkdata1", SqlDbType.VarChar).Value = valatate1.ToString();
                cmd.Parameters.Add("@chkdata2", SqlDbType.VarChar).Value = valatate2.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
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
                da.Dispose();
            }
            return result;
        }
        public string SaveAdvance(EOW_arfraising advance)
        {
            string result = "";
            string status = "";
            string GlNO = "";
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                CommonIUD comm = new CommonIUD();
                //SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                //cmd.Parameters.AddWithValue("@action", "SAVE_ADVANCE");
                //cmd.Parameters.AddWithValue("@gid", (advance.ecf_ecf_gid.ToString()));
                //cmd.CommandType = CommandType.StoredProcedure;
                //result = (string)cmd.ExecuteScalar();
                //if (result == "Not There")
                //{
                //string[] date = advance.ecfarf_liq_date.Split('-');
                //string liqdate = date[1].ToString() + "-" + date[0].ToString() + "-" + date[2].ToString();
                if (Convert.ToString(advance.ecfarf_promo_invoice) == null)
                {
                    advance.ecfarf_promo_invoice = "";
                }
                if (Convert.ToString(advance.ecfarf_po_no) == null)
                {
                    advance.ecfarf_po_no = "";
                }
                if (Convert.ToString(advance.ecfarf_cbf_no) == null)
                {
                    advance.ecfarf_cbf_no = "";
                }

                string desvl = "";
                if (advance.ecfarf_desc != "" && advance.ecfarf_desc != null)
                {
                    desvl = advance.ecfarf_desc;
                }

                //string Productsplit = advance.ecfarf_product_code.ToString();
                //string[] Productsp = Productsplit.Split('-');

                //string ousplit = advance.ecfarf_ou_code.ToString();
                //string[] oussp = ousplit.Split('-'); ;

                status = GetStatusexcel(advance.ecfarf_fc_code.ToString(), "", "", "FunctionCode");
                if (status == "notexists")
                {
                    result = "Invalid Function Code";
                    return result;
                }
                status = GetStatusexcel(advance.ecfarf_cc_code.ToString(), "", "", "CostCode");
                if (status == "notexists")
                {
                    result = "Invalid Cost Code";
                    return result;
                }
                string Productsplit = advance.ecfarf_product_code.ToString();
                string[] Productsp = Productsplit.Split('-');
                status = GetStatusexcel(Productsp[0].ToString(), "", "", "ProductCode");
                if (status == "notexists")
                {
                    result = "Invalid Product Code";
                    return result;
                }
                string ousplit = advance.ecfarf_ou_code.ToString();
                string[] oussp = ousplit.Split('-');
                status = GetStatusexcel(oussp[0].ToString(), "", "", "OUCode");
                if (status == "notexists")
                {
                    result = "Invalid OU Code";
                    return result;
                }
                if (advance.ecfarf_advancetype_gid != 0)
                {
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@advancetypeid", SqlDbType.Int).Value = advance.ecfarf_advancetype_gid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getAdvanceTypeGL";
                    GlNO = (string)cmd.ExecuteScalar();
                }


                string[,] codes = new string[,]            
                    {
                         {"ecfarf_ecf_gid", advance.ecf_ecf_gid.ToString()},
                          {"ecfarf_dr_gl_no", Convert.ToString(GlNO)},
                          {"ecfarf_pi_gl_no", Convert.ToString(advance.ecfarf_promo_invoice)},
                         {"ecfarf_advancetype_gid",advance.ecfarf_advancetype_gid.ToString()},
                         {"ecfarf_desc",cmnfun.Getreplacesinglequotes(desvl)},                         
                         {"ecfarf_liq_date",cmnfun.convertoDateTimeString(advance.ecfarf_liq_date)},
                         {"ecfarf_po_no",Convert.ToString(advance.ecfarf_po_no)},
                         {"ecfarf_cbf_no",Convert.ToString(advance.ecfarf_cbf_no)},
                         {"ecfarf_fc_code",advance.ecfarf_fc_code.ToString()},
                         {"ecfarf_cc_code",advance.ecfarf_cc_code.ToString()},
                          {"ecfarf_product_code",Productsp[0].ToString()},
                         {"ecfarf_ou_code",oussp[0].ToString()},
                           {"ecfarf_exception",advance.ecfarf_amount.ToString()},
                         {"ecfarf_amount",advance.ecfarf_amount.ToString()}
                         //{"docpouch_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         //{"docpouch_insert_date","sysdatetime()"}
                    };
                string tname = "iem_trn_tecfarf";
                result = comm.InsertCommon(codes, tname);
                result = "NotExists";
                // }

                string ecfarfgid = "0";
                string ecfarfglno = "0";
                GetConnection();
                DataTable dtarfgid = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = advance.ecf_ecf_gid.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getecfarfgid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtarfgid);
                string invoice_gid = "0";
                if (dtarfgid.Rows.Count > 0)
                {
                    DataTable dtup = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_com_invoicedetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = advance.ecf_ecf_gid.ToString();
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "invoicemaxgiduseecf";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtup);
                    if (dtup.Rows.Count > 0)
                    {
                        invoice_gid = Convert.ToString(dtup.Rows[0]["invoice_gid"].ToString());
                    }

                    ecfarfgid = Convert.ToString(dtarfgid.Rows[0]["ecfarf_gid"].ToString());
                    ecfarfglno = Convert.ToString(dtarfgid.Rows[0]["advancetype_gl_no"].ToString());

                    string[,] codesi = new string[,]
	               {
        {"ecfdebitline_ecf_gid",advance.ecf_ecf_gid.ToString() },
	    {"ecfdebitline_invoice_gid", invoice_gid},
        {"ecfdebitline_expnature_gid","0"},
	    {"ecfdebitline_expcat_gid", "0"},
        {"ecfdebitline_expsubcat_gid","0" },
	    {"ecfdebitline_gl_no", ecfarfglno},       
        {"ecfdebitline_fc_code",advance.ecfarf_fc_code.ToString() },
	    {"ecfdebitline_cc_code", advance.ecfarf_cc_code.ToString()},
        {"ecfdebitline_product_code", Productsp[0].ToString()},
        {"ecfdebitline_ou_code",oussp[0].ToString() },
	    {"ecfdebitline_amount", advance.ecfarf_amount.ToString()}       
                  };

                    string tnamei = "iem_trn_tecfdebitline";
                    string insertcommendi = objCommonIUD.InsertCommon(codesi, tnamei);
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
        public string UpdatAdvance(EOW_arfraising advance)
        {
            string result = "";
            string status = "";
            string GlNO = "";
            try
            {
                CommonIUD comm = new CommonIUD();
                GetConnection();
                string enm = "";
                CommonIUD delecomm = new CommonIUD();
                //string[] date = advance.ecfarf_liq_date.Split('-');
                //string liqdate = date[1].ToString() + "-" + date[0].ToString() + "-" + date[2].ToString();
                //string liqdate = string.Format("{0:MM-dd-yyyy}", advance.ecfarf_liq_date);
                //string recdate1 = DateTime.Now.ToString();
                if (Convert.ToString(advance.ecfarf_promo_invoice) == null)
                {
                    advance.ecfarf_promo_invoice = "";
                }
                if (Convert.ToString(advance.ecfarf_po_no) == null)
                {
                    advance.ecfarf_po_no = "";
                }
                if (Convert.ToString(advance.ecfarf_cbf_no) == null)
                {
                    advance.ecfarf_cbf_no = "";
                }
                string desvl = "";
                if (advance.ecfarf_desc != "" && advance.ecfarf_desc != null)
                {
                    desvl = advance.ecfarf_desc;
                }

                status = GetStatusexcel(advance.ecfarf_fc_code.ToString(), "", "", "FunctionCode");
                if (status == "notexists")
                {
                    result = "Invalid Function Code";
                    return result;
                }
                status = GetStatusexcel(advance.ecfarf_cc_code.ToString(), "", "", "CostCode");
                if (status == "notexists")
                {
                    result = "Invalid Cost Code";
                    return result;
                }
                string Productsplit = advance.ecfarf_product_code.ToString();
                string[] Productsp = Productsplit.Split('-');
                status = GetStatusexcel(Productsp[0].ToString(), "", "", "ProductCode");
                if (status == "notexists")
                {
                    result = "Invalid Product Code";
                    return result;
                }
                string ousplit = advance.ecfarf_ou_code.ToString();
                string[] oussp = ousplit.Split('-');
                status = GetStatusexcel(oussp[0].ToString(), "", "", "OUCode");
                if (status == "notexists")
                {
                    result = "Invalid OU Code";
                    return result;
                }
                if (advance.ecfarf_advancetype_gid != 0)
                {
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@advancetypeid", SqlDbType.Int).Value = advance.ecfarf_advancetype_gid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getAdvanceTypeGL";
                    GlNO = (string)cmd.ExecuteScalar();
                }

                string[,] codes = new string[,]
                 
                   {
                           {"ecfarf_ecf_gid", advance.ecfarf_ecf_gid.ToString()},
                           {"ecfarf_dr_gl_no", Convert.ToString(GlNO)},
                          {"ecfarf_pi_gl_no", Convert.ToString(advance.ecfarf_promo_invoice)},
                         {"ecfarf_advancetype_gid",advance.ecfarf_advancetype_gid.ToString()},
                         {"ecfarf_desc",cmnfun.Getreplacesinglequotes(desvl)},
                         {"ecfarf_liq_date",cmnfun.convertoDateTimeString(advance.ecfarf_liq_date)},
                         {"ecfarf_po_no",Convert.ToString(advance.ecfarf_po_no)},
                         {"ecfarf_cbf_no",Convert.ToString(advance.ecfarf_cbf_no)},
                         {"ecfarf_fc_code",advance.ecfarf_fc_code.ToString()},
                         {"ecfarf_cc_code",advance.ecfarf_cc_code.ToString()},
                          {"ecfarf_product_code",advance.ecfarf_product_code.ToString()},
                         {"ecfarf_ou_code",advance.ecfarf_ou_code.ToString()},
                           {"ecfarf_exception",advance.ecfarf_amount.ToString()},
                         {"ecfarf_amount",advance.ecfarf_amount.ToString()}                
                  };
                string[,] whrcol = new string[,]
	                 {
                          {"ecfarf_gid", advance.ecfarf_gid.ToString()}
            
                     };
                string tblname = "iem_trn_tecfarf";
                result = "NotExists";
                string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);

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
        public string SaveRequestval(EOW_arfraising cen)
        {
            string result = "";
            string docsubtypes = "";
            try
            {
                CommonIUD comm = new CommonIUD();
                DataTable dt = new DataTable();
                DataTable dtrelive = new DataTable();
                GetConnection();
                string branch = "";
                SqlCommand cmdrelive = new SqlCommand();
                //cmdrelive = new SqlCommand("   select * from iem_mst_temployee where employee_gid=" + cmnfun.GetLoginUserGid() + " and employee_isremoved='N' and employee_lastworkingdate is  null", con);
                cmdrelive = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmdrelive.Parameters.AddWithValue("@ACTION", "save_login");
                cmdrelive.Parameters.AddWithValue("@gid", cmnfun.GetLoginUserGid());
                cmdrelive.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter darelive = new SqlDataAdapter(cmdrelive);
                darelive = new SqlDataAdapter(cmdrelive);
                darelive.Fill(dtrelive);
                if (dtrelive.Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand();
                    if (cen.arf_type == "E" || cen.arf_type == "P")
                    {
                        docsubtypes = "E";
                        if (cen.arf_type == "P")
                        {
                            docsubtypes = "P";
                        }
                        //cmd = new SqlCommand("select employee_gid from iem_mst_temployee where employee_code='" + cen.supplierheader_suppliercode + "' and employee_isremoved='N'", con);
                        cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                        cmd.Parameters.AddWithValue("@ACTION", "save_emp");
                        cmd.Parameters.AddWithValue("@emp_code", cen.supplierheader_suppliercode);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (cen.arf_type == "S")
                    {
                        docsubtypes = "S";
                        //cmd = new SqlCommand("select supplierheader_gid,supplierheader_suppliercode,supplierheader_name from asms_trn_tsupplierheader where supplierheader_suppliercode='" + cen.supplierheader_suppliercode + "' and supplierheader_isremoved='N'", con);
                        cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                        cmd.Parameters.AddWithValue("@ACTION", "save_sup");
                        cmd.Parameters.AddWithValue("@emp_code", cen.supplierheader_suppliercode);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (cen.arf_type == "E" || cen.arf_type == "P")
                        {
                            cen.ecf_employee_gid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                        }
                        else if (cen.arf_type == "S")
                        {
                            cen.ecf_employee_gid = Convert.ToInt32(dt.Rows[0]["supplierheader_gid"].ToString());
                        }
                    }
                    con.Close();
                    con.Open();
                    DataTable dtt = new DataTable();
                    //SqlCommand cmd1 = new SqlCommand("select employee_gid,employee_branch_gid from iem_mst_temployee where employee_code='" + cen.employee_code + "' and employee_isremoved='N'", con);
                    SqlCommand cmd1 = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd1.Parameters.AddWithValue("@ACTION", "save_emp_branch");
                    cmd1.Parameters.AddWithValue("@emp_code", cen.employee_code);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    da1 = new SqlDataAdapter(cmd1);
                    da1.Fill(dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        branch = dtt.Rows[0]["employee_branch_gid"].ToString();
                        cen.ecf_raiser_gid = dtt.Rows[0]["employee_gid"].ToString();
                    }
                    //con.Close();
                    //con.Open();
                    //DataTable dtt1 = new DataTable();
                    //string enm = "";
                    //// SqlCommand cmd3 = new SqlCommand("SELECT docsubtype_gid FROM iem_mst_tdocsubtype where docsubtype_supplier_employee='" + cen.arf_type + "' and docsubtype_doctype_code='ARF' and docsubtype_isremoved='N'", con);
                    //SqlCommand cmd3 = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    //cmd3.Parameters.AddWithValue("@ACTION", "save_emp_doc");
                    //cmd3.Parameters.AddWithValue("@emp_code", docsubtypes);
                    //cmd3.CommandType = CommandType.StoredProcedure;
                    //SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    //da3 = new SqlDataAdapter(cmd3);
                    //da3.Fill(dtt1);
                    //if (dtt1.Rows.Count > 0)
                    //{
                    //    if (cen.arf_type == "E")
                    //    {
                    //        enm = (dtt1.Rows[0]["docsubtype_gid"].ToString());
                    //    }
                    //    else if (cen.arf_type == "P")
                    //    {
                    //        enm = (dtt1.Rows[2]["docsubtype_gid"].ToString());
                    //    }
                    //    else
                    //    {
                    //        enm = (dtt1.Rows[0]["docsubtype_gid"].ToString());
                    //    }
                    //}
                    //con.Close();
                    //con.Open();

                    string enm = "";
                    if (cen.arf_type == "E")
                    {
                        enm = "6";
                    }
                    else if (cen.arf_type == "P")
                    {
                        enm = "6";
                    }
                    else
                    {
                        enm = "7";
                    }

                    DataTable dttcurr2 = new DataTable();
                    string currencyrate = "";
                    string currency = "";
                    //SqlCommand cmd4 = new SqlCommand("select currency_gid,currencyrate_value from iem_mst_tcurrency join iem_mst_tcurrencyrate on currency_gid=currencyrate_currency_gid where currency_isremoved='N' and currency_code='INR' and currencyrate_isremoved='N'", con);
                    SqlCommand cmd4 = new SqlCommand("pr_eow_trn_Raisingarf", con);
                    cmd4.Parameters.AddWithValue("@ACTION", "save_emp_currrency");
                    cmd4.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                    da4 = new SqlDataAdapter(cmd4);
                    da4.Fill(dttcurr2);
                    if (dttcurr2.Rows.Count > 0)
                    {
                        currency = (dttcurr2.Rows[0]["currency_gid"].ToString());
                        currencyrate = (dttcurr2.Rows[0]["currencyrate_value"].ToString());
                    }
                    else
                    {
                        currency = "99";
                        currencyrate = "1.00";

                    }
                    con.Close();

                    con.Open();
                    string egid = "";
                    if (cen.arf_type == "E" || cen.arf_type == "P")
                    {
                        egid = "ecf_employee_gid";
                    }
                    else if (cen.arf_type == "S")
                    {
                        egid = "ecf_supplier_gid";
                    }
                    string desvl = "";
                    if (cen.ecfarf_desc != "" && cen.ecfarf_desc != null)
                    {
                        desvl = cen.ecfarf_desc;
                    }
                    //string[] ecfdate = cen.ecf_date.Split('-');
                    //string ecfdatte = ecfdate[1].ToString() + "-" + ecfdate[0].ToString() + "-" + ecfdate[2].ToString();
                    string gid = "";
                    string suplieremp = "";
                    string Emp_Msg = "";
                    if (cen.arf_type == "E" || cen.arf_type == "P")
                    {
                        gid = "and ecf_employee_gid=" + cen.ecf_employee_gid + "";
                        suplieremp = "invoice_employee_gid";

                    }
                    else if (cen.arf_type == "S")
                    {
                        gid = "and ecf_supplier_gid=" + cen.ecf_employee_gid + "";
                        suplieremp = "invoice_supplier_gid";
                    }

                    if (cen.ecf_ecf_gid == "")
                    {
                        string[,] codes = new string[,]            
                    {
                         {"ecf_supplier_employee",docsubtypes},
                         {egid, Convert.ToString(cen.ecf_employee_gid)},
                         {"ecf_date",objCmnFunctions.convertoDateTimeString(cen.ecf_date).ToString()},
                         {"ecf_create_mode",cen.ecf_create_mode},
                         {"ecf_raiser",cen.ecf_raiser_gid.ToString()},
                         {"ecf_amount",cen.ecf_amount.ToString()},         
                         {"ecf_description",cmnfun.Getreplacesinglequotes(desvl)},
                         {"ecf_doctype_gid","2"},
                         {"ecf_docsubtype_gid",enm},
                         {"ecf_claim_month",objCmnFunctions.convertoDateTimeString(cen.ecf_date).ToString()},
                         {"ecf_currency_code","INR"}, 
                         {"ecf_currency_gid",currency},
                         {"ecf_branch_gid",branch}, 
                         {"ecf_status","0"},
                         {"ecf_all_status","0"},
                         {"ecf_currency_rate",currencyrate},
                         {"ecf_currency_amount",cen.ecf_amount.ToString()},
                         {"ecf_delmat_amount",cen.ecf_amount.ToString()},                       
                         {"ecf_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"ecf_insert_date","sysdatetime()"}
                    };
                        string tname = "iem_trn_tecf";
                        result = comm.InsertCommon(codes, tname);
                    }
                    else
                    {
                        string[,] codes = new string[,]
	                {
                           {"ecf_date",objCmnFunctions.convertoDateTimeString(cen.ecf_date).ToString()},                     
                           {"ecf_amount",cen.ecf_amount.ToString()},  
                             {"ecf_currency_amount",cen.ecf_amount.ToString()},
                         {"ecf_delmat_amount",cen.ecf_amount.ToString()},     
                           {"ecf_description",cmnfun.Getreplacesinglequotes(desvl)}                  
                       
	                };
                        string[,] whrcol = new string[,]
	                {
                        {"ecf_gid", cen.ecf_ecf_gid.ToString ()}
                    };

                        string updcomm = comm.UpdateCommon(codes, whrcol, "iem_trn_tecf");

                        string[,] codesinv = new string[,]
	               {
	    {suplieremp,cen.ecf_employee_gid.ToString()},
        {"invoice_type",cen.arf_type },	  
	    {"invoice_amount",cen.ecf_amount.ToString()},
        {"invoice_wotax_amount", cen.ecf_amount.ToString()},	   
        {"invoice_no", "0"},       
        {"invoice_desc", cmnfun.Getreplacesinglequotes(desvl)},
        {"invoice_date",objCmnFunctions.convertoDateTimeString(cen.ecf_date).ToString()},
        {"invoice_provision_flag", "N"},
        {"invoice_retention_flag", "N"},
        {"invoice_retention_rate", "0"},
        {"invoice_retention_amount", "0"},
        {"invoice_retention_exception", "0"},  
        {"invoice_retention_releaseon",""}, 
                  };
                        string[,] whoreinv = new string[,]
	               {
        {"invoice_ecf_gid",cen.ecf_ecf_gid.ToString()},  
                  };
                        string tnameinv = "iem_trn_tinvoice";

                        string insertcommend = objCommonIUD.UpdateCommon(codesinv, whoreinv, tnameinv);
                        Emp_Msg = insertcommend.ToString();

                    }
                    con.Close();
                    con.Open();
                    DataTable dt1 = new DataTable();
                    SqlCommand cmd2 = new SqlCommand("select max(ecf_gid) as ecf_gid from iem_trn_tecf where   ecf_raiser=" + cen.ecf_raiser_gid + " " + gid + " and ecf_amount=" + cen.ecf_amount + " and ecf_isremoved='N'", con);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        result = Convert.ToString(dt1.Rows[0]["ecf_gid"].ToString());

                        if (cen.ecf_ecf_gid == "")
                        {
                            string[,] codes = new string[,]
	               {
        {"invoice_ecf_gid",result},
        {suplieremp,cen.ecf_employee_gid.ToString()},
        {"invoice_type",cen.arf_type },	  
	    {"invoice_amount",cen.ecf_amount.ToString()},
        {"invoice_wotax_amount", cen.ecf_amount.ToString()},	   
        {"invoice_no", "0"},       
        {"invoice_desc", cmnfun.Getreplacesinglequotes(desvl)},
        {"invoice_date",objCmnFunctions.convertoDateTimeString(cen.ecf_date).ToString()},
        {"invoice_provision_flag", "N"},
        {"invoice_retention_flag", "N"},
        {"invoice_retention_rate", "0"},
        {"invoice_retention_amount", "0"},
        {"invoice_retention_exception", "0"},  
        {"invoice_retention_releaseon",""}, 
        {"invoice_dedup_status","0" }    
                  };
                            string tname = "iem_trn_tinvoice";

                            string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                            Emp_Msg = insertcommend.ToString();
                        }
                    }
                }
                else
                {
                    result = "Can't Proceed This Login Employee";
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


        //added by Dhasarathan 
        public string GetEmployeePayModeEraAcc(int EmployeeeGid)
        {

            var EraBankAcc = "";
            var bankgid = "";
            var emp_paymode = "";
            var result = "Yes";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = EmployeeeGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "EmpAccdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    emp_paymode = Convert.ToString(dt.Rows[0]["emp_paymode"]);
                    if (emp_paymode != "CHQ")
                    {
                        EraBankAcc = Convert.ToString(dt.Rows[0]["employee_era_acc_no"]);
                        if (!string.IsNullOrWhiteSpace(EraBankAcc) && EraBankAcc != "0")
                        {
                            bankgid = Convert.ToString(dt.Rows[0]["employee_era_bank_gid"]);
                            if (!string.IsNullOrWhiteSpace(bankgid) && bankgid != "0")
                            {

                                result = "Yes";
                            }
                            else
                            {
                                result = "No";
                            }
                        }
                        else
                        {
                            result = "No";
                        }

                    }


                }
                return result;
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ex.Message.ToString();
            }


        }
        public string GetSupplierBankDetailsBypayMode(string ecfgid)
        {
            var EraBankAcc = "";
            var bankgid = "";
            var ifsccode = "";
            var emp_paymode = "";
            var result = "Yes";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = ecfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetSupplierBankdetailsByEft";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    emp_paymode = Convert.ToString(dt.Rows[0]["paymode"]);
                    if (emp_paymode == "EFT")
                    {


                        EraBankAcc = Convert.ToString(dt.Rows[0]["payment_accountno"]);
                        if (!string.IsNullOrWhiteSpace(EraBankAcc) && EraBankAcc != "0")
                        {
                            bankgid = Convert.ToString(dt.Rows[0]["payment_bank_gid"]);
                            if (!string.IsNullOrWhiteSpace(bankgid) && bankgid != "0")
                            {
                                if (!string.IsNullOrWhiteSpace(ifsccode) && ifsccode != "0")
                                {
                                    ifsccode = Convert.ToString(dt.Rows[0]["payment_ifsccode"]);
                                    if (!string.IsNullOrWhiteSpace(ifsccode) || ifsccode != "0")
                                    {
                                        result = "Yes";
                                    }
                                    else
                                    {
                                        result = "No";
                                    }
                                }
                                else { result = "Yes"; };
                            }
                            else
                            {
                                result = "No";
                            }
                        }
                        else
                        {
                            result = "No";
                        }

                    }


                }
                return result;
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ex.Message.ToString();
            }
            return "";
        }
        //added by Dhasarathan 
    }
}