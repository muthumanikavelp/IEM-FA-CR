using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace IEM.Areas.IFAMS.Models
{
    public class IfamsQueryDataModel : IfamsQueryRepository
    {
        SqlConnection con = new SqlConnection();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfun = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<IfamsQueryEntity> GetQueryDetails()
        {
            try
            {
                List<IfamsQueryEntity> objOrgType = new List<IfamsQueryEntity>();
                IfamsQueryEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new IfamsQueryEntity();
                    objModel.barcodedetials_bar_no = Convert.ToString(dt.Rows[i]["Barcode"].ToString());
                    objModel.assetdetails_assetdet_id = Convert.ToString(dt.Rows[i]["AssedID"].ToString());
                    objModel.branch_legacy_code = Convert.ToString(dt.Rows[i]["Location"].ToString());
                    objModel.assetcategory_name = Convert.ToString(dt.Rows[i]["Asset Cat"].ToString());
                    objModel.assetsubcategory_name = Convert.ToString(dt.Rows[i]["Sub catename"].ToString());
                    objModel.assetdetails_cap_date = Convert.ToString(dt.Rows[i]["Captilization Date"].ToString());
                    objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                    objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    objModel.assetdetails_assetdet_value = Convert.ToDecimal(dt.Rows[i]["Asset value"].ToString());
                    objModel.poheader_pono = Convert.ToString(dt.Rows[i]["PO No"].ToString());
                    objModel.excelvalidate_name = Convert.ToString(dt.Rows[i]["status name"].ToString());
                    objModel.cbfheader_cbfno = Convert.ToString(dt.Rows[i]["CBF No"].ToString());
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

        public IEnumerable<IfamsQueryEntity> GetQueryDetails(string barcode, string assetid, string location, string assetcat, string assetsub, string capdate, string ecfno, string invoiceno, string assetstatus, string assetvalue, string pono, string cbfno)
        {
            try
            {
                List<IfamsQueryEntity> objOrgType = new List<IfamsQueryEntity>();
                IfamsQueryEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "Search");
                cmd.Parameters.AddWithValue("@assetdetails_assetdet_id", assetid);
                cmd.Parameters.AddWithValue("@assetdetails_asset_value", assetvalue);
                cmd.Parameters.AddWithValue("@asset_description", assetsub);
                cmd.Parameters.AddWithValue("@barcodedetials_bar_no", barcode);
                cmd.Parameters.AddWithValue("@invoice_no", invoiceno);
                cmd.Parameters.AddWithValue("@ecf_no", ecfno);
                cmd.Parameters.AddWithValue("@poheader_pono", pono);
                cmd.Parameters.AddWithValue("@branch_name", location);
                cmd.Parameters.AddWithValue("@assetcategory_name",assetcat);
                cmd.Parameters.AddWithValue("@excelvalidate_name",assetstatus);
                cmd.Parameters.AddWithValue("@cbfheader_cbfno", cbfno);
                if (capdate != "")
                {
                    cmd.Parameters.AddWithValue("@assetdetails_cap_date", objcmnfun.convertoDateTimeString(capdate).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@assetdetails_cap_date", capdate);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new IfamsQueryEntity();
                    objModel.barcodedetials_bar_no = Convert.ToString(dt.Rows[i]["Barcode"].ToString());
                    objModel.assetdetails_assetdet_id = Convert.ToString(dt.Rows[i]["AssedID"].ToString());
                    objModel.branch_legacy_code = Convert.ToString(dt.Rows[i]["Location"].ToString());
                    objModel.assetcategory_name = Convert.ToString(dt.Rows[i]["Asset Cat"].ToString());
                    objModel.assetsubcategory_name = Convert.ToString(dt.Rows[i]["Sub catename"].ToString());
                    objModel.assetdetails_cap_date = Convert.ToString(dt.Rows[i]["Captilization Date"].ToString());
                    objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                    objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    objModel.assetdetails_assetdet_value = Convert.ToDecimal(dt.Rows[i]["Asset value"].ToString());
                    objModel.poheader_pono = Convert.ToString(dt.Rows[i]["PO No"].ToString());
                    objModel.excelvalidate_name = Convert.ToString(dt.Rows[i]["status name"].ToString());
                    objModel.cbfheader_cbfno = Convert.ToString(dt.Rows[i]["CBF No"].ToString());
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
        public IEnumerable<Assetcategory> Assetcategory()
        {
            List<Assetcategory> getdoc = new List<Assetcategory>();
            Assetcategory obj;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "Assetcategory");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Assetcategory()
                    {
                        assetcatid = Convert.ToInt32(row["assetcategory_gid"].ToString()),
                        assetcatname = row["assetcategory_name"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }

        public IEnumerable<Assetsubcategory> Assetsubcategory()
        {
            List<Assetsubcategory> getdoc = new List<Assetsubcategory>();
            Assetsubcategory obj;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "Assetsubcategory");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Assetsubcategory()
                    {
                        assetsubcatid = Convert.ToInt32(row["asset_gid"].ToString()),
                        assetsubcatname = row["asset_description"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }


        public IEnumerable<AssetStatusName> AssetStatusName()
        {
            List<AssetStatusName> getdoc = new List<AssetStatusName>();
            AssetStatusName obj;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "Assetstatus");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetStatusName()
                    {
                        assetstatusid = Convert.ToInt32(row["excelvalidate_gid"].ToString()),
                        assetstatusname = row["excelvalidate_name"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }


        public IEnumerable<GetLocation> GetLocation()
        {
            List<GetLocation> getdoc = new List<GetLocation>();
            GetLocation obj;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_Query", con);
                cmd.Parameters.AddWithValue("@action", "Location");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GetLocation()
                    {
                        BrnachId = Convert.ToInt32(row["branch_gid"].ToString()),
                        BranchNmae = row["branch_code"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }


        public DataSet GetTrackerScreenData()
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_querysearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueryresult";
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

    }
}