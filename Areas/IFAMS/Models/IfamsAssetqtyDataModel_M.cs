using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsAssetqtyDataModel_M : AssetqtyRepository_M
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfun = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objErrLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }


        public string GetStatusExcel(string assetdata, string valid, string action)
        {
            string resultacc = "";

            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_AssetQty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Qtychkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@Qtyassetdata", SqlDbType.VarChar).Value = assetdata.ToString();
                cmd.Parameters.Add("@Qtyresult", SqlDbType.VarChar).Value = action;
                resultacc = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return resultacc;
        }


        public string UpdateAQty(DataTable exldata, string filename)
        {
            List<AssetqtyModel> aqtyList = new List<AssetqtyModel>();
            AssetqtyModel aqty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                dt = (DataTable)exldata;
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    aqty = new AssetqtyModel();
                    aqty.assetdetid = dt.Rows[i]["Asset id"].ToString().Trim();
                    aqty.assetqty = Convert.ToInt32(dt.Rows[i]["Asset Quantity"].ToString().Trim());

                    string[,] codes = new string[,]
                    {
                        {"assetdetails_qty", aqty.assetqty.ToString()}
                    };
                    string[,] wher = new string[,]
                    {
                        {"assetdetails_assetdet_id",aqty.assetdetid.ToString()}
                    };
                    string updcom = objcmnIUD.UpdateCommon(codes, wher, "fa_trn_tassetdetails");

                }
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); 
            }
            finally
            {
                con.Close();
            }
            return "Success";
        }


        public string UpdateASer(DataTable exldata, string filename)
        {
            List<AssetqtyModel> aqtyList = new List<AssetqtyModel>();
            AssetqtyModel aqty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                dt = (DataTable)exldata;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    aqty = new AssetqtyModel();
                    aqty.assetdetid = dt.Rows[i]["Asset id"].ToString().Trim();
                    aqty.aqtySheetname = dt.Rows[i]["Asset Serialno"].ToString().Trim();

                    string[,] codes = new string[,]
                    {
                        {"assetdetails_asset_serialno", aqty.aqtySheetname.ToString()}
                    };
                    string[,] wher = new string[,]
                    {
                        {"assetdetails_assetdet_id",aqty.assetdetid.ToString()}
                    };
                    string updcom = objcmnIUD.UpdateCommon(codes, wher, "fa_trn_tassetdetails");

                }
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return "success";
        }
    
    }
}