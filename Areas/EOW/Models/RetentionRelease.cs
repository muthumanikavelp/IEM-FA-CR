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
    public class RetentionRelease : RetentionRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<Retention_Release> RetentionReleaseGrid()
        {
            HttpContext.Current.Session["suphead"] = "";
            List<Retention_Release> GridReceipt = new List<Retention_Release>();
            try
            {

                Retention_Release objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    HttpContext.Current.Session["suphead"] = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel = new Retention_Release()
                    {
                        invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString()),
                        invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString()),
                        ecf_date = row["ecf_date"].ToString(),
                        ecf_no = row["ecf_no"].ToString(),
                        ecf_suppliercode = row["supplierheader_suppliercode"].ToString(),
                        ecf_suppliername = row["supplierheader_name"].ToString(),
                        invoice_retention_amount = cmnfun.GetINRAmount(row["invoice_retention_amount"].ToString()),
                        invoice_retention_exception = cmnfun.GetINRAmount(row["invoice_retention_exception"].ToString()),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_amount = cmnfun.GetINRAmount(row["invoice_amount"].ToString()),
                        //invoice_retention_releaseon = row["invoice_retention_releaseon"].ToString(),
                        release = cmnfun.GetINRAmount(row["retentionrelease_amount"].ToString()),
                    };
                    GridReceipt.Add(objModel);
                }

                return GridReceipt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return GridReceipt;
            }

            finally
            {
                con.Close();
            }
        }
        public DataTable debitgrid(string invoice_ecf, string invoice)
        {
            DataTable dt = new DataTable();
            try
            {
                List<Retention_Release> emp = new List<Retention_Release>();
                GetConnection();


                SqlCommand cmd = new SqlCommand("select ecfcreditline_pay_mode,gl_name,ecfcreditline_ref_no,ecfcreditline_beneficiary,ecfcreditline_amount,ecfcreditline_desc from iem_trn_tecfcreditline inner join iem_mst_tgl on ecfcreditline_gl_no=gl_no where ecfcreditline_isremoved='N' and ecfcreditline_pay_mode='RET' and gl_isremoved='N' and ecfcreditline_ecf_gid=" + invoice_ecf + " and ecfcreditline_invoice_gid=" + invoice + "", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public string InsertEmpAtt(HttpPostedFileBase savefile, Retention_Release EmployeeeExpense)
        {
            string Emp_Msg = "";
            try
            {
                string remarkecf = "";
                if (Convert.ToString(EmployeeeExpense.attachment_desc) == null)
                {
                    remarkecf = "";
                }
                else
                {
                    remarkecf = EmployeeeExpense.attachment_desc.ToString();
                }
                string[,] codes = new string[,]
	               {
                    {"attachment_ref_flag","1" },
	                {"attachment_ref_gid", Convert.ToString(EmployeeeExpense.attachment_refgid)},
                    {"attachment_filename",savefile.FileName.ToString() },
                    {"attachment_attachmenttype_gid", Convert.ToString( EmployeeeExpense.attachment_attachmenttype_gid)},
	                {"attachment_desc", cmnfun.Getreplacesinglequotes(remarkecf)},
	                {"attachment_date", "sysdatetime()"},
                    {"attachment_by", Convert.ToString(EmployeeeExpense.attachment_by)},
                    {"attachment_isremoved", "N"}
                  };
                string tname = "iem_trn_tattachment";

                string insertcommend = objCommonIUD.InsertCommon(codes, tname);

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select  MAX(attachment_gid) as attachment_gid from iem_trn_tattachment where attachment_ref_gid='" + EmployeeeExpense.attachment_refgid + "' and attachment_by='" + EmployeeeExpense.attachment_by + "' and attachment_isremoved='N'  ", con);
                cmd.CommandType = CommandType.Text;
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
        public List<GetAttach> Getattach()
        {
            List<GetAttach> objCountrytype = new List<GetAttach>();
            try
            {

                GetAttach objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("select attachmenttype_gid,attachmenttype_name from iem_mst_tattachmenttype  where attachmenttype_isremoved='N'", con);
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

        public string GetSerialNo()
        {
            string serial = "";
            DataTable getserilno = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("select MAX(retention_serialno)  as 'Serial No' from iem_trn_tretentionlog", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(getserilno);
                if (getserilno.Rows.Count > 0)
                {
                    serial = getserilno.Rows[0]["Serial No"].ToString();
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
            return serial;
        }
        public IEnumerable<Retention_Release> RetentionReleaseGridSearch(string filter_name, string filter_name1, string ECFNo, string InvoiceNo, string Suppliercode, string Suppliername, string extendeddate)
        {
            List<Retention_Release> GridReceipt = new List<Retention_Release>();
            try
            {

                Retention_Release objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SEARCH_RELEASE");
                cmd.Parameters.AddWithValue("@retentionrelease_date", filter_name);
                cmd.Parameters.AddWithValue("@ecfdate", filter_name1);
                cmd.Parameters.AddWithValue("@ecfno", ECFNo);
                cmd.Parameters.AddWithValue("@invoice_no", InvoiceNo);
                cmd.Parameters.AddWithValue("@supplierheader_suppliercode", Suppliercode);
                cmd.Parameters.AddWithValue("@supplierheader_name", Suppliername);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new Retention_Release()
                    {
                        invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString()),
                        invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString()),
                        ecf_date = row["ecf_date"].ToString(),
                        ecf_no = row["ecf_no"].ToString(),
                        ecf_suppliercode = row["supplierheader_suppliercode"].ToString(),
                        ecf_suppliername = row["supplierheader_name"].ToString(),
                        invoice_retention_amount = cmnfun.GetINRAmount(row["invoice_retention_amount"].ToString()),
                        invoice_retention_exception = cmnfun.GetINRAmount(row["invoice_retention_exception"].ToString()),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_amount = cmnfun.GetINRAmount(row["invoice_amount"].ToString()),
                        //  invoice_retention_releaseon = row["invoice_retention_releaseon"].ToString(),
                        release = Convert.ToString(row["retentionrelease_amount"].ToString()),
                    };
                    GridReceipt.Add(objModel);
                }

                return GridReceipt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return GridReceipt;
            }
            finally
            {
                con.Close();
            }
        }
        //public List<Getfc> Getfcid()
        //{
        //    try
        //    {
        //        List<Getfc> objCountrytype = new List<Getfc>();
        //        Getfc objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select fc_code+'-'+fc_name as fc_name,fc_gid from dbo.iem_mst_tfc where fc_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getfc();
        //            objModel.fc_gid = Convert.ToInt32(dt.Rows[i]["fc_gid"].ToString());
        //            objModel.fc_name = Convert.ToString(dt.Rows[i]["fc_name"].ToString());
        //            objCountrytype.Add(objModel);
        //        }
        //        return objCountrytype;
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
        //public IEnumerable<Getexpensecategory> Getexpcategory(Retention_Release expcat)
        //{
        //    try
        //    {
        //        List<Getexpensecategory> objCountrytype = new List<Getexpensecategory>();
        //        Getexpensecategory objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("select expcat_gid,expcat_name from iem_mst_texpcat where expcat_isremoved='N' and expcat_expnature_gid=" + expcat.expense_gid + "", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getexpensecategory();
        //            objModel.expensecate_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
        //            objModel.expensecate_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());

        //            objCountrytype.Add(objModel);
        //        }

        //        return objCountrytype;
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
        //public List<Getexpense> Getexpenseid()
        //{
        //    try
        //    {
        //        List<Getexpense> objCountrytype = new List<Getexpense>();
        //        Getexpense objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select expnature_gid,expnature_name from iem_mst_texpnature where expnature_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getexpense();
        //            objModel.expense_gid = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
        //            objModel.expense_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
        //            objCountrytype.Add(objModel);
        //        }
        //        return objCountrytype;
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
        //public List<Getexpensesubcategory> Getexpsubcategoryload()
        //{
        //    try
        //    {
        //        List<Getexpensesubcategory> objCountrytype = new List<Getexpensesubcategory>();
        //        Getexpensesubcategory objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand(" select expsubcat_gid,expsubcat_name from iem_mst_texpsubcat where expsubcat_isremoved='N' ", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getexpensesubcategory();
        //            objModel.expensesubcate_gid = Convert.ToInt32(dt.Rows[i]["expsubcat_gid"].ToString());
        //            objModel.expensesubcate_name = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());

        //            objCountrytype.Add(objModel);
        //        }

        //        return objCountrytype;
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
        //public List<Getexpensecategory> Getexpcategoryload()
        //{
        //    try
        //    {
        //        List<Getexpensecategory> objCountrytype = new List<Getexpensecategory>();
        //        Getexpensecategory objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("select expcat_gid,expcat_name from iem_mst_texpcat where expcat_isremoved='N' ", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getexpensecategory();
        //            objModel.expensecate_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
        //            objModel.expensecate_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());

        //            objCountrytype.Add(objModel);
        //        }

        //        return objCountrytype;
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
        public IEnumerable<GetRefcredit> Getrefcreditpaymode(Retention_Release expsubcat)
        {
            List<GetRefcredit> objCountrytype = new List<GetRefcredit>();
            try
            {

                GetRefcredit objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(" SELECT payment_accountno FROM asms_trn_tpayment join iem_mst_tpaymode on paymode_gid=payment_paymode_gid WHERE payment_isremoved='N' and paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'and payment_paymode_gid=" + expsubcat.creditpaymode + " and payment_accountno not in('NA','')", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetRefcredit();
                    objModel.payment_accountnocredit = dt.Rows[i]["payment_accountno"].ToString();
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
        //public IEnumerable<Getexpensesubcategory> Getexpsubcategory(Retention_Release expsubcat)
        //{
        //    try
        //    {
        //        List<Getexpensesubcategory> objCountrytype = new List<Getexpensesubcategory>();
        //        Getexpensesubcategory objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand(" select expsubcat_gid,expsubcat_name from iem_mst_texpsubcat where expsubcat_isremoved='N' and expsubcat_expcat_gid=" + expsubcat.expensecate_gid + "", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getexpensesubcategory();
        //            objModel.expensesubcate_gid = Convert.ToInt32(dt.Rows[i]["expsubcat_gid"].ToString());
        //            objModel.expensesubcate_name = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());

        //            objCountrytype.Add(objModel);
        //        }

        //        return objCountrytype;
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

        //public List<Getcc> Getccid()
        //{
        //    try
        //    {
        //        List<Getcc> objCountrytype = new List<Getcc>();
        //        Getcc objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select cc_code+'-'+cc_name as cc_name,cc_gid from dbo.iem_mst_tcc where cc_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getcc();
        //            objModel.cc_gid = Convert.ToInt32(dt.Rows[i]["cc_gid"].ToString());
        //            objModel.cc_name = Convert.ToString(dt.Rows[i]["cc_name"].ToString());
        //            objCountrytype.Add(objModel);
        //        }
        //        return objCountrytype;
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
        //public List<Getfccc> Getfcccid()
        //{
        //    try
        //    {
        //        List<Getfccc> objCountrytype = new List<Getfccc>();
        //        Getfccc objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("select fccc_code+'-'+fccc_name as fccc_name,fccc_gid from dbo.iem_mst_tfccc where fccc_isremoved='N'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new Getfccc();
        //            objModel.fccc_gid = Convert.ToInt32(dt.Rows[i]["fccc_gid"].ToString());
        //            objModel.fccc_name = Convert.ToString(dt.Rows[i]["fccc_name"].ToString());
        //            objCountrytype.Add(objModel);
        //        }

        //        return objCountrytype;
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
        public Retention_Release employeedetil()
        {
            Retention_Release objModel = new Retention_Release();
            try
            {
                List<Retention_Release> GridInward = new List<Retention_Release>();

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("select employee_product_code,employee_ou_code from iem_mst_temployee where employee_isremoved='N'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new Retention_Release()
               {
                   product_code = (dt.Rows[0]["employee_product_code"].ToString()),
                   ou_code = (dt.Rows[0]["employee_ou_code"].ToString())
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

        public List<GetPaymodecredit> GetPaymodecredit()
        {
            List<GetPaymodecredit> objCountrytype = new List<GetPaymodecredit>();
            try
            {

                GetPaymodecredit objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("select distinct paymode_gid,paymode_code from  iem_mst_tpaymode join asms_trn_tpayment on paymode_gid=payment_paymode_gid where paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetPaymodecredit();
                    objModel.paymode_gidcredit = Convert.ToInt32(dt.Rows[i]["paymode_gid"].ToString());
                    objModel.paymode_namecredit = Convert.ToString(dt.Rows[i]["paymode_code"].ToString());
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
        public List<GetRefcredit> Getrefcredit()
        {
            List<GetRefcredit> objCountrytype = new List<GetRefcredit>();
            try
            {


                GetRefcredit objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("SELECT payment_accountno FROM asms_trn_tpayment join iem_mst_tpaymode on paymode_gid=payment_paymode_gid WHERE payment_isremoved='N' and paymode_supplier_flag='Y' and paymode_isremoved='N' and paymode_active='Y'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetRefcredit();
                    objModel.payment_accountnocredit = (dt.Rows[i]["payment_accountno"].ToString());
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
        public DataTable creditgrid(string invoice_ecf, string invoice)
        {
            DataTable dt = new DataTable();
            try
            {
                List<Retention_Release> emp = new List<Retention_Release>();
                GetConnection();


                SqlCommand cmd = new SqlCommand("  select max(ecfcreditline_gid) as ecfcreditline_ecf_gid,ecfcreditline_pay_mode,ecfcreditline_ref_no,ecfcreditline_beneficiary,ecfcreditline_amount,ecfcreditline_desc from iem_trn_tecfcreditline where ecfcreditline_isremoved='N' and ecfcreditline_pay_mode!='RET' and ecfcreditline_ecf_gid=" + invoice_ecf + " and ecfcreditline_invoice_gid=" + invoice + " group by ecfcreditline_pay_mode,ecfcreditline_ref_no,ecfcreditline_beneficiary,ecfcreditline_amount,ecfcreditline_desc", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable RetentionReleaseGrid_log(string Docid)
        {
            DataTable dt = new DataTable();
            try
            {
                List<Retention_Release> GridInward = new List<Retention_Release>();

                GetConnection();


                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_log");
                cmd.Parameters.AddWithValue("@invoice_gid", Docid);
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
        public Retention_Release RetentionReleaseGrid_id(int Docid)
        {
            Retention_Release objModel = new Retention_Release();
            try
            {
                List<Retention_Release> GridInward = new List<Retention_Release>();

                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ID");
                cmd.Parameters.AddWithValue("@invoice_gid", Docid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                objModel = new Retention_Release()
                {
                    ecf_raiser = Convert.ToString(dt.Rows[0]["ecf_raiser"].ToString()),
                    ecf_date = Convert.ToString(dt.Rows[0]["ecf_date"].ToString()),
                    ecf_amount = cmnfun.GetINRAmount(dt.Rows[0]["ecf_amount"].ToString()),
                    ecf_no = (dt.Rows[0]["ecf_no"].ToString()),
                    ecf_suppliercode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                    ecf_suppliername = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString()),
                    invoice_date = Convert.ToString(dt.Rows[0]["invoice_date"].ToString()),
                    invoice_no = Convert.ToString(dt.Rows[0]["invoice_no"].ToString()),
                    invoice_amount = cmnfun.GetINRAmount(dt.Rows[0]["invoice_amount"].ToString()),
                    // Retention_date = Convert.ToString(dt.Rows[0]["invoice_retention_releaseon"].ToString()),
                    Retention_amount = cmnfun.GetINRAmount(dt.Rows[0]["invoice_retention_amount"].ToString()),
                    Balance_amount = cmnfun.GetINRAmount(dt.Rows[0]["invoice_retention_exception"].ToString()),
                    invoice_desc = Convert.ToString(dt.Rows[0]["invoice_desc"].ToString()),
                    ecf_remarks = Convert.ToString(dt.Rows[0]["ecf_remark"].ToString()),
                    invoice_gid = Convert.ToInt32(dt.Rows[0]["invoice_gid"].ToString()),
                    invoice_ecf_gid = Convert.ToInt32(dt.Rows[0]["invoice_ecf_gid"].ToString()),
                    BranchCode = Convert.ToString(dt.Rows[0]["branch_code"].ToString())
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
        public string ReleaseVerifyamount(Retention_Release verify)
        {
            string result = "";
            string Balanceamount = verify.Balance_amount;
            string releaseamount = verify.release_amount;
            Balanceamount = cmnfun.GetRemovecommas(Balanceamount);
            releaseamount = cmnfun.GetRemovecommas(releaseamount);

            int balance = Convert.ToInt32(Balanceamount);
            int release = Convert.ToInt32(releaseamount);
            if (balance >= release)
            {
                result = cmnfun.GetINRAmount(Convert.ToString(balance - release));
            }
            else
            {
                result = "";
            }
            return result;
        }
        public string update(Retention_Release cen)
        {
            string result = "";
            try
            {
                GetConnection();

                string[,] codes = new string[,]            
	                {
                         //{"invoice_retention_amount", Convert.ToString(cen.release_amount)},
                         // {"invoice_retention_rate", Convert.ToString(cen.release_amount)},
                            {"invoice_retention_exception", Convert.ToString(cen.Balance_amount)},
                    };
                string[,] whrcol = new string[,]
	                 {
                          {"invoice_gid", cen.invoice_gid.ToString()}
            
                     };
                string tname = "iem_trn_tinvoice";
                result = objCommonIUD.UpdateCommon(codes, whrcol, tname);
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
        //public string SaveReleasedebit(Retention_Release cen)
        //{
        //    string result = "";
        //    try
        //    {
        //        string ref_no = "";
        //        if (Convert.ToString(cen.creditref_no) == "")
        //        {
        //            ref_no = "";
        //        }
        //        string[,] codesecf112 = new string[,]  
        //                {                           
        //                     {"ecfdebitline_ecf_gid", Convert.ToString(cen.invoice_ecf_gid)},
        //                     {"ecfdebitline_invoice_gid",cen.invoice_gid.ToString()},
        //                     {"ecfdebitline_expnature_gid", Convert.ToString(cen.expense_gid)},
        //                     {"ecfdebitline_expcat_gid", Convert.ToString(cen.expensecate_gid)},  
        //                     {"ecfdebitline_expsubcat_gid",Convert.ToString(cen.expensesubcate_gid)},
        //                     {"ecfdebitline_desc",cen.creditdesc},
        //                     {"ecfdebitline_fc_code", Convert.ToString(cen.fc_gid)},        
        //                     {"ecfdebitline_cc_code", Convert.ToString(cen.cc_gid)},        
        //                       {"ecfdebitline_product_code", cen.product_code},  
        //                     {"ecfdebitline_ou_code",cen.ou_code},
        //                     {"ecfdebitline_amount",cen.creditamount},                                
        //                     {"ecfdebitline_gl_no", ""},   
        //                     {"ecfdebitline_isremoved","N"}
        //                };
        //        string tname1ecf112 = "iem_trn_tecfdebitline";
        //        result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        public string SaveReleasecredit(Retention_Release cen)
        {
            string result = "";
            try
            {
                string ref_no = "";
                if (Convert.ToString(cen.creditref_no) == "")
                {
                    ref_no = "";
                }
                //string[,] codesecf112 = new string[,]  
                //        {                           
                //             {"ecfcreditline_ecf_gid", Convert.ToString(cen.invoice_ecf_gid)},
                //             {"ecfcreditline_invoice_gid",cen.invoice_gid.ToString()},
                //             {"ecfcreditline_pay_mode",cen.creditpaymode},
                //             {"ecfcreditline_ref_no", ref_no},  
                //             {"ecfcreditline_beneficiary",cen.creditbecificary},
                //             {"ecfcreditline_desc",cen.creditdesc},
                //             {"ecfcreditline_amount", Convert.ToString(cen.creditamount)},        
                //             {"ecfcreditline_gl_no", ""},        
                //             {"ecfcreditline_isremoved","N"}
                //        };
                //string tname1ecf112 = "iem_trn_tecfcreditline";
                //result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);
                string[,] codesecf112 = new string[,]  
                 {                     
                             {"ecfcreditline_pay_mode",cen.creditpaymode},
                             {"ecfcreditline_ref_no", ref_no}, 
                    };
                string[,] whrcol = new string[,]
	                 {                        
                          {"ecfcreditline_gid", cen.payrefergid.ToString()}
            
                     };
                string tname = "iem_trn_tecfcreditline";
                result = objCommonIUD.UpdateCommon(codesecf112, whrcol, tname);
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
        public string dummyinvoice(string ecf)
        {
            string result = "";
            try
            {
                con.Open();
                DataTable dtttype = new DataTable();
                string enmtype = "";
                SqlCommand cmdtype = new SqlCommand(" select invoice_no from iem_trn_tinvoice  where invoice_ecf_gid=" + ecf + " and invoice_isremoved='N'", con);
                SqlDataAdapter datype = new SqlDataAdapter(cmdtype);
                datype = new SqlDataAdapter(cmdtype);
                datype.Fill(dtttype);
                if (dtttype.Rows.Count > 0)
                {
                    enmtype = (dtttype.Rows[0]["invoice_no"].ToString());
                    enmtype = enmtype + "RETREL_";
                    DataTable dtttype1 = new DataTable();
                    SqlCommand cmdtype1 = new SqlCommand(" select invoice_no from iem_trn_tinvoice  where invoice_no like '%'+'" + enmtype + "'+'%' and invoice_isremoved='N'  order by invoice_no desc", con);
                    SqlDataAdapter datype1 = new SqlDataAdapter(cmdtype1);
                    datype1 = new SqlDataAdapter(cmdtype1);
                    datype1.Fill(dtttype1);
                    if (dtttype1.Rows.Count > 0)
                    {
                        string[] emtypearray = dtttype1.Rows[0]["invoice_no"].ToString().Split('_');
                        int cnt = Convert.ToInt32(emtypearray[1].ToString());
                        cnt++;
                        enmtype = emtypearray[0].ToString() + "_" + Convert.ToString(cnt);
                    }
                    else
                    {
                        enmtype = enmtype + "01";
                    }
                }
                con.Close();
                con.Open();
                string[,] codes = new string[,]            
	                 {                         
                             {"invoice_no",enmtype},                                              
                        };
                string[,] whrcol = new string[,]
	                 {
                          {"invoice_ecf_gid", ecf}
                     };
                string tname = "iem_trn_tinvoice";
                result = objCommonIUD.UpdateCommon(codes, whrcol, tname);
                result = enmtype;
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string SaveRelease(Retention_Release cen)
        {
            string result = "";
            try
            {
                update(cen);
                string[] dateecf = cen.Retention_date.Split('-');
                string inwarddateecf = dateecf[1].ToString() + "-" + dateecf[0].ToString() + "-" + dateecf[2].ToString();
                string remarkecf = "";
                if (Convert.ToString(cen.release_remarks) == null)
                {
                    remarkecf = "";
                }
                else
                {
                    remarkecf = cen.release_remarks.ToString();
                }
                con.Close();
                con.Open();
                DataTable dtttype = new DataTable();
                string enmtype = "";
                SqlCommand cmdtype = new SqlCommand("select doctype_gid from dbo.iem_mst_tdoctype where doctype_name like 'Supplier'+'%' and doctype_isremoved='N'", con);
                SqlDataAdapter datype = new SqlDataAdapter(cmdtype);
                datype = new SqlDataAdapter(cmdtype);
                datype.Fill(dtttype);
                if (dtttype.Rows.Count > 0)
                {

                    enmtype = (dtttype.Rows[0]["doctype_gid"].ToString());
                }
                con.Close();
                con.Open();
                con.Close();
                con.Open();
                DataTable dtt1 = new DataTable();
                string enm = "";
                SqlCommand cmd3 = new SqlCommand("select docsubtype_gid from  dbo.iem_mst_tdocsubtype where docsubtype_name ='Supplier Invoice' and docsubtype_isremoved='N'", con);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                da3 = new SqlDataAdapter(cmd3);
                da3.Fill(dtt1);
                if (dtt1.Rows.Count > 0)
                {

                    enm = (dtt1.Rows[0]["docsubtype_gid"].ToString());
                }
                con.Close();
                con.Open();
                DataTable dtt2 = new DataTable();
                string currencyrate = "";
                string currency = "";
                SqlCommand cmd4 = new SqlCommand("select currency_gid,currencyrate_value from iem_mst_tcurrency join iem_mst_tcurrencyrate on currency_gid=currencyrate_currency_gid where currency_isremoved='N' and currency_code='INR' and currencyrate_isremoved='N'", con);
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                da4 = new SqlDataAdapter(cmd4);
                da4.Fill(dtt2);
                if (dtt2.Rows.Count > 0)
                {
                    currency = (dtt2.Rows[0]["currency_gid"].ToString());
                    currencyrate = (dtt2.Rows[0]["currencyrate_value"].ToString());
                }
                else
                {
                    currency = "99";
                    currencyrate = "1";
                }
                con.Close();

                con.Open();

                DataTable dtttypebranch = new DataTable();
                string branch = "";
                SqlCommand cmdtypebranch = new SqlCommand("select employee_branch_gid from iem_mst_temployee where employee_gid='" + cmnfun.GetLoginUserGid() + "'", con);
                SqlDataAdapter datypebranch = new SqlDataAdapter(cmdtypebranch);
                datypebranch = new SqlDataAdapter(cmdtypebranch);
                datypebranch.Fill(dtttypebranch);
                if (dtttypebranch.Rows.Count > 0)
                {

                    branch = (dtttypebranch.Rows[0]["employee_branch_gid"].ToString());
                }

                con.Close();


                con.Open();

                DataTable dtsupplier = new DataTable();
                string supplier = "";
                SqlCommand cmdsupplier = new SqlCommand("select invoice_supplier_gid from iem_trn_tinvoice where invoice_no='" + cen.invoice_no + "' and invoice_gid='" + cen.invoice_gid + "' and invoice_isremoved='N'", con);
                SqlDataAdapter dasupplier = new SqlDataAdapter(cmdsupplier);
                dasupplier = new SqlDataAdapter(cmdsupplier);
                dasupplier.Fill(dtsupplier);
                if (dtsupplier.Rows.Count > 0)
                {

                    supplier = (dtsupplier.Rows[0]["invoice_supplier_gid"].ToString());
                }

                con.Close();


                con.Open();
                string[,] codesecf = new string[,]  
                        {
                             {"ecf_supplier_employee","S"},
                            // {"ecf_supplier_gid",HttpContext.Current.Session["suphead"].ToString()},
                            {"ecf_supplier_gid",supplier},
                             {"ecf_date",inwarddateecf},  
                             {"ecf_raiser",cen.ecf_raiser},
                              {"ecf_all_status","262144"},
                             {"ecf_status","262144"},
                             {"ecf_doctype_gid",enmtype},
                             {"ecf_po_type","P"},
                             {"ecf_docsubtype_gid",enm},
                             //{"ecf_doctype_gid","select doctype_gid from dbo.iem_mst_tdoctype where doctype_name like 'Supplier'+'%' and doctype_isremoved='N'"},
                             //{"ecf_docsubtype_gid","select docsubtype_gid from  dbo.iem_mst_tdocsubtype where docsubtype_name ='Supplier Invoice' and docsubtype_isremoved='N'"},
                             {"ecf_branch_gid",branch},
                             {"ecf_claim_month",inwarddateecf},
                             {"ecf_create_mode","R"},
                             {"ecf_currency_gid",currency},
                             {"ecf_currency_code","INR"},
                             {"ecf_currency_rate",currencyrate},
                             {"ecf_currency_amount",cen.release_amount.ToString()},
                             {"ecf_amount",cen.release_amount.ToString()},
                             {"ecf_delmat_amount",cen.release_amount.ToString()},                            
                             {"ecf_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                             {"ecf_insert_date","sysdatetime()"},                        
                             {"ecf_isremoved","N"},  
                             {"ecf_remark",cmnfun.Getreplacesinglequotes(remarkecf)},
                            
                             
                        };
                string tname1ecf = "iem_trn_tecf";
                result = objCommonIUD.InsertCommon(codesecf, tname1ecf);
                con.Close();
                con.Open();
                DataTable dttecfgid = new DataTable();
                string ecfgid = "";
                SqlCommand cmdecfgid = new SqlCommand("select ecf_gid from iem_trn_tecf where ecf_supplier_employee='S' and ecf_amount=" + cen.release_amount.ToString() + " and ecf_remark='" + remarkecf + "' and ecf_date='" + inwarddateecf + "' and  ecf_isremoved='N'", con);
                SqlDataAdapter daecfgid = new SqlDataAdapter(cmdecfgid);
                daecfgid = new SqlDataAdapter(cmdecfgid);
                daecfgid.Fill(dttecfgid);
                if (dttecfgid.Rows.Count > 0)
                {
                    ecfgid = (dttecfgid.Rows[0]["ecf_gid"].ToString());
                }
                con.Close();
                con.Open();
                DataTable dttecfno = new DataTable();
                string ecfno = "";
                SqlCommand cmdecfno = new SqlCommand("select ecf_no from iem_trn_tecf where ecf_gid='" + cen.invoice_ecf_gid + "' and  ecf_isremoved='N'", con);
                SqlDataAdapter daecfno = new SqlDataAdapter(cmdecfgid);
                daecfno = new SqlDataAdapter(cmdecfno);
                daecfno.Fill(dttecfno);
                string[,] codesecfdesc = new string[,]  
                        {
                             {"ecf_description",dttecfno.Rows[0]["ecf_no"].ToString() + "RETENTION RELEASE"},
                             
                        };
                string[,] whereecfdesc = new string[,]  
                        {
                             {"ecf_gid",ecfgid},
                             
                        };
                string tname1ecfdes = "iem_trn_tecf";
                result = objCommonIUD.UpdateCommon(codesecfdesc, whereecfdesc, tname1ecfdes);
                con.Close();

                con.Open();
                SqlCommand cmdecfgidgen = new SqlCommand("pr_eow_trn_tecfRelease", con);
                cmdecfgidgen.CommandType = CommandType.StoredProcedure;
                cmdecfgidgen.Parameters.Add("@ecfgid", SqlDbType.Int).Value = Convert.ToInt32(ecfgid);
                cmdecfgidgen.Parameters.Add("@ecf_remark", SqlDbType.VarChar).Value = remarkecf;
                cmdecfgidgen.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                int data = cmdecfgidgen.ExecuteNonQuery();

                con.Close();
                con.Open();
                string suprvosor = "";
                DataTable dttecfgidsup = new DataTable();
                SqlCommand cmdecfgidsup = new SqlCommand("select employee_supervisor from iem_mst_temployee where employee_gid=" + cmnfun.GetLoginUserGid() + " and employee_isremoved='N'", con);
                SqlDataAdapter daecfgidsup = new SqlDataAdapter(cmdecfgidsup);
                daecfgidsup = new SqlDataAdapter(cmdecfgidsup);
                daecfgidsup.Fill(dttecfgidsup);
                if (dttecfgidsup.Rows.Count > 0)
                {
                    suprvosor = (dttecfgidsup.Rows[0]["employee_supervisor"].ToString());
                }
                con.Close();
                con.Open();
                string[,] codesecf1 = new string[,]  
                        {
                             {"queue_to_type","G"},
                             {"queue_ref_flag","1"},
                             {"queue_date",inwarddateecf},  
                             {"queue_ref_gid",ecfgid},
                             {"queue_ref_status","262144"},
                             {"queue_from","17"},
                             {"queue_action_for","A"},  
                             {"queue_action_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                             {"queue_action_status","0"},
                             {"queue_action_remark",remarkecf},                            
                             {"queue_action_date","sysdatetime()"},                        
                             {"queue_isremoved","N"},
                             {"queue_prev_gid","0"},
                             {"queue_to","18"}
                        };
                string tname1ecf1 = "iem_trn_tqueue";
                result = objCommonIUD.InsertCommon(codesecf1, tname1ecf1);

                string[] servicemonth;
           
                DataTable dttservicemonth = new DataTable();
                SqlCommand cmdservicemonth = new SqlCommand("select convert(varchar,ecf_claim_month,111) as ecf_claim_month from iem_trn_tecf where ecf_gid='" + cen.invoice_ecf_gid + "' and  ecf_isremoved='N'", con);
                SqlDataAdapter daservicemonth = new SqlDataAdapter(cmdecfgid);
                daservicemonth = new SqlDataAdapter(cmdservicemonth);
                daservicemonth.Fill(dttservicemonth);
                string[] dat1 = dttservicemonth.Rows[0]["ecf_claim_month"].ToString().Split('/');
                string invoiceservicemonth = dat1[0].ToString() + "-" + dat1[1].ToString() + "-" + dat1[2].ToString();
                //  cmnfun.Getreplacesinglequotes(cen.invoice_desc.Substring(0,125))
                string invdesc = string.IsNullOrEmpty(cen.invoice_desc) ? "" : cen.invoice_desc;
                string des = cmnfun.Getreplacesinglequotes(invdesc);
                if (des.Length > 125)
                {
                    des = des.Substring(0, 125);
                }
                string[,] codesecf11 = new string[,]  
                        {
                             {"invoice_type","S"},
                             //{"invoice_supplier_gid",HttpContext.Current.Session["suphead"].ToString()},
                          {"invoice_supplier_gid",supplier},
                             {"invoice_date","sysdatetime()"},  
                             {"invoice_ecf_gid", Convert.ToString(ecfgid)},
                             {"invoice_no",cen.invoice_no},
                             {"invoice_service_month",invoiceservicemonth},
                             {"invoice_desc",des},
                             {"invoice_amount",cen.release_amount.ToString()},  
                             {"invoice_wotax_amount","0"},
                             {"invoice_payment_nett","N"},
                             {"invoice_dedup_no","0"},                            
                             {"invoice_dedup_status","0"},                        
                             {"invoice_provision_flag","N"},
                             {"invoice_retention_flag","N"},                                                    
                             {"invoice_retention_rate","0"},                        
                             {"invoice_retention_amount","0"},
                             {"invoice_retention_exception","0"},
                               {"invoice_retention_status","0"},                        
                             {"invoice_retention_releaseon","sysdatetime()"},
                             {"invoice_isremoved","N"},
                             {"invoice_retention_curr_status","0"}
                        };
                string tname1ecf11 = "iem_trn_tinvoice";
                result = objCommonIUD.InsertCommon(codesecf11, tname1ecf11);
                con.Close();
                con.Open();
                string invoicegid = "";
                DataTable dttecfgidsupinvoice = new DataTable();
                SqlCommand cmdecfgidsupincoice = new SqlCommand("   select invoice_gid from iem_trn_tinvoice where invoice_ecf_gid=" + ecfgid + " and invoice_supplier_gid=" + supplier + "  and invoice_isremoved='N'", con);
                SqlDataAdapter daecfgidsupinvoice = new SqlDataAdapter(cmdecfgidsupincoice);
                daecfgidsupinvoice = new SqlDataAdapter(cmdecfgidsupincoice);
                daecfgidsupinvoice.Fill(dttecfgidsupinvoice);
                if (dttecfgidsupinvoice.Rows.Count > 0)
                {
                    invoicegid = (dttecfgidsupinvoice.Rows[0]["invoice_gid"].ToString());
                }
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@action", "Exist_Release");
                cmd.Parameters.AddWithValue("@invoice_gid", (cen.invoice_gid.ToString()));
                cmd.Parameters.AddWithValue("@invoice_ecf_gid", cen.invoice_ecf_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                // if (res1 == "Not There")
                {

                    string[] date = cen.ReleaseDate.Split('-');
                    string inwarddate = date[1].ToString() + "-" + date[0].ToString() + "-" + date[2].ToString();
                    string remark = "";
                    if (Convert.ToString(cen.release_remarks) == null)
                    {
                        remark = "";
                    }
                    else
                    {
                        remark = cen.release_remarks.ToString();
                    }
                    string[,] codes = new string[,]  
                        {
                             {"retentionrelease_invoice_gid",(cen.invoice_gid.ToString())},
                             {"retentionrelease_date",inwarddate},
                             {"retentionrelease_amount",cen.release_amount.ToString()},
                             {"retentionrelease_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                             {"retentionrelease_on","sysdatetime()"},
                             {"retentionrelease_ecf_gid",ecfgid},
                             {"retentionrelease_isremoved","N"},  
                               {"retentionrelease_remarks",cmnfun.Getreplacesinglequotes(remark)}                             
                        };
                    string tname = "iem_trn_tretentionrelease";
                    result = objCommonIUD.InsertCommon(codes, tname);
                }

                con.Close();
                con.Open();
                DataTable dt1 = new DataTable();
                SqlCommand cmd2 = new SqlCommand("select retentionrelease_gid from iem_trn_tretentionrelease where retentionrelease_invoice_gid=" + cen.invoice_gid.ToString() + " and retentionrelease_ecf_gid=" + ecfgid + " and retentionrelease_isremoved='N'", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                da1 = new SqlDataAdapter(cmd2);
                da1.Fill(dt1);
                cen.release_gid = Convert.ToInt32(dt1.Rows[0]["retentionrelease_gid"].ToString());

                con.Close();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd1.Parameters.AddWithValue("@action", "Exist_Releaselog");
                cmd1.Parameters.AddWithValue("@invoice_gid", (cen.invoice_gid.ToString()));
                cmd1.Parameters.AddWithValue("@invoice_ecf_gid", cen.release_gid);
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();
                string res = cmd1.Parameters["@res"].Value.ToString();
                //if (res == "Not There")
                {

                    string[] date1 = cen.Retention_date.Split('-');
                    string inwarddate1 = date1[1].ToString() + "-" + date1[0].ToString() + "-" + date1[2].ToString();
                    string remark1 = "";
                    if (Convert.ToString(cen.release_remarks) == null)
                    {
                        remark1 = "";
                    }
                    else
                    {
                        remark1 = cen.release_remarks.ToString();
                    }
                    string[,] codes1 = new string[,]  
                        {
                             {"retention_invoice_gid",(cen.invoice_gid.ToString())},
                             {"retention_date",inwarddate1},
                             {"retention_rate",cen.Retention_amount.ToString()},
                              {"retention_amount",cen.Retention_amount.ToString()},
                               {"retention_exception",cen.Balance_amount.ToString()},
                              {"retention_release_gid", cen.release_gid.ToString()},
                               {"retention_releaseamount",cen.release_amount.ToString()},
                             {"retention_insertby",Convert.ToString (cmnfun.GetLoginUserGid())},
                             {"retention_inserton","sysdatetime()"},
                             {"retention_status","Amount Released"},
                             {"retention_isactive","Y"},  
                             {"retention_serialno",cen.serialno.ToString()},
                               {"remarks",remark1}                             
                        };
                    string tname1 = "iem_trn_tretentionlog";
                    result = objCommonIUD.InsertCommon(codes1, tname1);

                }
                con.Close();
                con.Open();
                //DataTable dt = new DataTable();
                //SqlCommand cmdret = new SqlCommand("  select ecfcreditline_pay_mode,ecfcreditline_ref_no,ecfcreditline_beneficiary,ecfcreditline_amount,ecfcreditline_desc from iem_trn_tecfcreditline where ecfcreditline_isremoved='N' and ecfcreditline_pay_mode='RET' and ecfcreditline_ecf_gid=" + cen.invoice_ecf_gid + " and ecfcreditline_invoice_gid=" + cen.invoice_gid + "", con);
                //SqlDataAdapter da = new SqlDataAdapter(cmdret);
                //da.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    string ref_no = "";
                //    if (Convert.ToString(dt.Rows[0]["ecfcreditline_ref_no"]) == "")
                //    {
                //        ref_no = "";
                //    }
                //    string gl = "";
                //    DataTable dtgl = new DataTable();
                //    SqlCommand cmdretgl = new SqlCommand("SELECT gl_no,gl_name FROM iem_mst_tgl where gl_name like '%retention%' and gl_isremoved='N'", con);
                //    SqlDataAdapter dagl = new SqlDataAdapter(cmdretgl);
                //    dagl.Fill(dtgl);
                //    if (dtgl.Rows.Count > 0)
                //    {
                //        gl = Convert.ToString(dtgl.Rows[0]["gl_no"]);
                //    }
                //    string[,] codesecf112 = new string[,]  
                //        {                           
                //             {"ecfcreditline_ecf_gid", Convert.ToString(ecfgid)},
                //             {"ecfcreditline_invoice_gid",invoicegid},
                //             {"ecfcreditline_pay_mode",dt.Rows[0]["ecfcreditline_pay_mode"].ToString()},
                //             {"ecfcreditline_ref_no", ref_no},  
                //             {"ecfcreditline_beneficiary",dt.Rows[0]["ecfcreditline_beneficiary"].ToString()},
                //             {"ecfcreditline_desc",cmnfun.Getreplacesinglequotes(dt.Rows[0]["ecfcreditline_desc"].ToString())},
                //             {"ecfcreditline_amount", Convert.ToString(cen.release_amount)},        
                //             {"ecfcreditline_gl_no", gl},        
                //             {"ecfcreditline_isremoved","N"}
                //        };
                //    string tname1ecf112 = "iem_trn_tecfcreditline";
                //    result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);

                //}
                DataTable dt = new DataTable();
               // SqlCommand cmdret = new SqlCommand(" select paymode_code,payment_accountno,payment_beneficiaryname,payment_bank_gid,payment_ifsccode from asms_trn_tpayment join iem_mst_tpaymode on paymode_gid=payment_paymode_gid where payment_supplierheader_gid='"+supplier+"'", con);
                SqlCommand cmdret = new SqlCommand("  select  top 1 c.paymode_code,c.paymode_name,b.payment_beneficiaryname,b.payment_accountno,b.payment_bank_gid,b.payment_ifsccode from asms_trn_tsupplierheader  as a  inner join  asms_trn_tpayment as b on b.payment_supplierheader_gid=a.supplierheader_gid inner join iem_mst_tpaymode as c on c.paymode_gid=b.payment_paymode_gid where supplierheader_gid='" + supplier + "' and a.supplierheader_isremoved='N' and payment_isremoved='N' and  c.paymode_isremoved='N'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmdret);
                da.Fill(dt);
                DataTable dtpaybank = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = supplier.ToString();
                cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = dt.Rows[0]["paymode_code"].ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "togetpaybankgid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtpaybank);

                if (dt.Rows.Count > 0)
                {
                    string[,] codesecf112 = new string[,]  
                        {                           
                             {"ecfcreditline_ecf_gid", Convert.ToString(ecfgid)},
                             {"ecfcreditline_invoice_gid",invoicegid},
                             {"ecfcreditline_pay_mode",dt.Rows[0]["paymode_code"].ToString()},
                             {"ecfcreditline_ref_no", dt.Rows[0]["payment_accountno"].ToString()},  
                             {"ecfcreditline_beneficiary",dt.Rows[0]["payment_beneficiaryname"].ToString()},
                             {"ecfcreditline_ifsc_code",dt.Rows[0]["payment_ifsccode"].ToString()},
                             {"ecfcreditline_bank_gid",dtpaybank.Rows[0]["bank_gid"].ToString()},
                             {"ecfcreditline_desc","RETENTION"},
                             {"ecfcreditline_amount", Convert.ToString(cen.release_amount)},        
                             {"ecfcreditline_gl_no",dtpaybank.Rows[0]["bankgl_no"].ToString()},        
                             {"ecfcreditline_isremoved","N"}
                        };
                    string tname1ecf112 = "iem_trn_tecfcreditline";
                    result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);

                }
               
                DataTable dt1credit = new DataTable();
             //   SqlCommand cmdret1 = new SqlCommand("  select ecfcreditline_pay_mode,ecfcreditline_ref_no,ecfcreditline_beneficiary,ecfcreditline_amount,ecfcreditline_desc,ecfcreditline_gl_no from iem_trn_tecfcreditline where ecfcreditline_isremoved='N' and ecfcreditline_pay_mode='RET' and ecfcreditline_ecf_gid=" + cen.invoice_ecf_gid + " and ecfcreditline_invoice_gid=" + cen.invoice_gid + "", con);
                SqlCommand cmdret1 = new SqlCommand("select creditline_pay_mode,creditline_ref_no,creditline_beneficiary,creditline_amount,creditline_desc,creditline_gl_no from iem_trn_tcreditline where creditline_isremoved='N' and creditline_pay_mode='RET' and creditline_ecf_gid=" + cen.invoice_ecf_gid + " and creditline_invoice_gid=" + cen.invoice_gid + "", con);
                SqlDataAdapter da1credit = new SqlDataAdapter(cmdret1);
                da1credit.Fill(dt1credit);
                if (dt1credit.Rows.Count > 0)
                {
                    DataTable dt1credit1 = new DataTable();
                    SqlCommand cmdret11 = new SqlCommand("select ecfdebitline_fc_code,ecfdebitline_cc_code,ecfdebitline_product_code,ecfdebitline_ou_code,ecfdebitline_gl_no from iem_trn_tecfdebitline where ecfdebitline_ecf_gid='" + cen.invoice_ecf_gid + "' and ecfdebitline_invoice_gid='" + cen.invoice_gid + "' and ecfdebitline_isremoved='N'", con);
                    SqlDataAdapter da1credit1 = new SqlDataAdapter(cmdret11);
                    da1credit1.Fill(dt1credit1);
                    if (dt1credit1.Rows.Count > 0)
                    {
                        DataTable dt1credit1exp = new DataTable();
                       // SqlCommand cmdret11exp = new SqlCommand("select expnature_gid,expnature_name,expcat_gid,expcat_name,expsubcat_gid,expsubcat_name from iem_mst_texpnature join iem_mst_texpcat on expnature_gid=expcat_expnature_gid join iem_mst_texpsubcat on expcat_gid=expsubcat_expcat_gid and expnature_gid=expsubcat_expnature_gid  inner join iem_trn_tecfdebitline on ecfdebitline_expnature_gid=expnature_gid and ecfdebitline_expcat_gid=expcat_gid  and ecfdebitline_expsubcat_gid=expsubcat_gid  where expnature_isremoved='N' and expnature_active='Y' and expcat_isremoved='N' and expsubcat_isremoved='N' and  ecfdebitline_ecf_gid=" + cen.invoice_ecf_gid + " and ecfdebitline_invoice_gid=" + cen.invoice_gid + "", con);
                        SqlCommand cmdret11exp = new SqlCommand("pr_eow_mst_expense", con);
                        SqlDataAdapter da1credit1exp = new SqlDataAdapter(cmdret11exp);
                        da1credit1exp.Fill(dt1credit1exp);
                        string Client = System.Configuration.ConfigurationManager.AppSettings["Client"];
                        if (Client.ToString().Equals("HFC"))
                        {
                            if (dt1credit1exp.Rows.Count > 0)
                            {
                                string[,] codesecf112 = new string[,]  
                        {                                  
                             {"ecfdebitline_ecf_gid", Convert.ToString(ecfgid)},
                             {"ecfdebitline_invoice_gid",invoicegid},
                             {"ecfdebitline_expnature_gid", dt1credit1exp.Rows[0]["expnature_gid"].ToString()},
                             {"ecfdebitline_expcat_gid", dt1credit1exp.Rows[0]["expcat_gid"].ToString()},  
                             {"ecfdebitline_expsubcat_gid",dt1credit1exp.Rows[0]["expsubcat_gid"].ToString()},
                               {"ecfdebitline_desc","RETENTION"},
                                 {"ecfdebitline_fc_code", "55"},        
                             {"ecfdebitline_cc_code", "999"},        
                               {"ecfdebitline_product_code", "499"},  
                             {"ecfdebitline_ou_code","3550002"},
                             {"ecfdebitline_amount",Convert.ToString(cen.release_amount)},                                
                             {"ecfdebitline_gl_no", dt1credit.Rows[0]["creditline_gl_no"].ToString()},   
                             {"ecfdebitline_isremoved","N"}
                         
                        };
                                string tname1ecf112 = "iem_trn_tecfdebitline";
                                result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);
                            }
                            else
                            {
                                if (dt1credit1exp.Rows.Count > 0)
                                {
                                    string[,] codesecf112 = new string[,]  
                        {     
                             
                             {"ecfdebitline_ecf_gid", Convert.ToString(ecfgid)},
                             {"ecfdebitline_invoice_gid",invoicegid},
                             {"ecfdebitline_expnature_gid", dt1credit1exp.Rows[0]["expnature_gid"].ToString()},
                             {"ecfdebitline_expcat_gid", dt1credit1exp.Rows[0]["expcat_gid"].ToString()},  
                             {"ecfdebitline_expsubcat_gid",dt1credit1exp.Rows[0]["expsubcat_gid"].ToString()},
                               {"ecfdebitline_desc","RETENTION"},
                             //{"ecfdebitline_desc",cmnfun.Getreplacesinglequotes(dt1credit.Rows[0]["ecfcreditline_desc"].ToString())},
                             //{"ecfdebitline_fc_code", dt1credit1.Rows[0]["ecfdebitline_fc_code"].ToString()},        
                             //{"ecfdebitline_cc_code", dt1credit1.Rows[0]["ecfdebitline_cc_code"].ToString()},        
                             //  {"ecfdebitline_product_code", dt1credit1.Rows[0]["ecfdebitline_product_code"].ToString()},  
                             //{"ecfdebitline_ou_code",dt1credit1.Rows[0]["ecfdebitline_ou_code"].ToString()},
                            
                              {"ecfdebitline_fc_code", "41"},        
                             {"ecfdebitline_cc_code", "999"},        
                               {"ecfdebitline_product_code", "500"},  
                             {"ecfdebitline_ou_code","3503543"},                               
                             {"ecfdebitline_amount",Convert.ToString(cen.release_amount)},                                
                             {"ecfdebitline_gl_no", dt1credit.Rows[0]["creditline_gl_no"].ToString()},   
                             {"ecfdebitline_isremoved","N"}
                         
                        };
                                    string tname1ecf112 = "iem_trn_tecfdebitline";
                                    result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dt1credit1exp1 = new DataTable();
                        SqlCommand cmdret11exp1 = new SqlCommand("pr_eow_mst_expense", con);
                        SqlDataAdapter da1credit1exp1 = new SqlDataAdapter(cmdret11exp1);
                        da1credit1exp1.Fill(dt1credit1exp1);
                        string[,] codesecf112 = new string[,]  
                        {                           
                             {"ecfdebitline_ecf_gid", Convert.ToString(ecfgid)},
                             {"ecfdebitline_invoice_gid",invoicegid},
                             {"ecfdebitline_expnature_gid", dt1credit1exp1.Rows[0]["expnature_gid"].ToString()},
                             {"ecfdebitline_expcat_gid", dt1credit1exp1.Rows[0]["expcat_gid"].ToString()},  
                             {"ecfdebitline_expsubcat_gid",dt1credit1exp1.Rows[0]["expsubcat_gid"].ToString()},
                             {"ecfdebitline_desc","RETENTION"},
                             //{"ecfdebitline_fc_code","41" },        
                             //{"ecfdebitline_cc_code","999" },        
                             //  {"ecfdebitline_product_code","500" },  
                             //{"ecfdebitline_ou_code","3503543"},
                            {"ecfdebitline_fc_code","55" },        
                             {"ecfdebitline_cc_code","999" },        
                               {"ecfdebitline_product_code","499" },  
                             {"ecfdebitline_ou_code","3550002"},
                             {"ecfdebitline_amount",Convert.ToString(cen.release_amount)},                                
                             {"ecfdebitline_gl_no", "211100026"},   
                             {"ecfdebitline_isremoved","N"}
                        };
                        string tname1ecf112 = "iem_trn_tecfdebitline";
                        result = objCommonIUD.InsertCommon(codesecf112, tname1ecf112);
                    }
                 
                }
                result = ecfgid + "-" + invoicegid;
                //    }
                //    else
                //    {
                //        result = "Please Insert Credit & Debit Line And Then Proceed";
                //    }
                //}
                //else
                //{
                //    result = "Please Insert Credit & Debit Line And Then Proceed";
                //}
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

    }
}