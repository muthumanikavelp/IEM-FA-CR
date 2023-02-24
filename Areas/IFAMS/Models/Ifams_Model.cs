using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using IEM.Common;
using System.Data;
using System.Web.Providers.Entities;
using System.Web.Mvc;
using IEM.Areas.EOW.Models;

namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_Model : Ifams_Repository
    {
        SqlConnection confa = new SqlConnection();
        SqlCommand cmdfa = new SqlCommand();
        SqlDataAdapter dafa = new SqlDataAdapter();
        SqlTransaction tran = null;
        CmnFunctions comuidfa = new CmnFunctions();
        CommonIUD commfa = new CommonIUD();
        DataModel dm = new DataModel();
        DataSet dsfa;
        DataTable dtfa;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetfaConnection()
        {
            if (confa.State == ConnectionState.Closed)
            {
                confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                confa.Open();
            }
            //if (confa.State == ConnectionState.Open)
            //else
            //{
            //    //confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            //    confa.Close();
            //}
        }

        public List<Ifams_Property> Getassetcategory()
        {
            GetfaConnection();

            List<Ifams_Property> objlget = new List<Ifams_Property>();
            try
            {
                cmdfa = new SqlCommand("pr_ifams_Getassetcategorty", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        Ifams_Property obj = new Ifams_Property();
                        obj.assetCategoryId = Convert.ToInt32(dtfa.Rows[i]["assetcategory_gid"].ToString());
                        obj.assetCategoryName = dtfa.Rows[i]["assetcategory_name"].ToString();
                        objlget.Add(obj);
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            GetfaConnection();
            return objlget;
        }

        #region // add by sugumar HSN code
        public List<Ifams_Property> GetHsncode()
        {
            try
            {
                List<Ifams_Property> objhsttype = new List<Ifams_Property>();
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "getcombo");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ifams_Property objModel = new Ifams_Property();
                    objModel.Hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"].ToString());
                    objModel.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());

                    objhsttype.Add(objModel);
                }

                return objhsttype;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                confa.Close();
            }
            //  throw new NotImplementedException();
        }
        #endregion

        #region // add by sugumar HSN code
        public List<SelectListItem> GetHsncode1()
        {
            try
            {
                List<SelectListItem> objhsttype = new List<SelectListItem>();
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "getcombo");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    objhsttype.Add(new SelectListItem
                    {
                        Text = sdr["hsn_code"].ToString(),
                        Value = sdr["hsn_gid"].ToString(),
                        Selected = true
                    });
                }
                return objhsttype;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                confa.Close();
            }
            //  throw new NotImplementedException();
        }
        #endregion

        #region //Multiple hsn code select
        public List<SelectListItem> GetHsnforasset(string id)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "gethsn");
                cmd.Parameters.AddWithValue("@hsn_id",id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["hsn_code"].ToString(),
                        Value = sdr["hsn_gid"].ToString(),
                        
                    });
                }
                return items;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                confa.Close();
            }
            //  throw new NotImplementedException();
        }
        #endregion


        public List<Ifams_Property> Getsubcategory(int cate)
        {
            GetfaConnection();
            List<Ifams_Property> objsget = new List<Ifams_Property>();
            try
            {
                cmdfa = new SqlCommand("pr_ifams_Getsubcategory", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@asscate_gid", cate);
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        Ifams_Property objs = new Ifams_Property();
                        objs.assetSubCategoryId = Convert.ToInt32(dtfa.Rows[i]["assetsubcategory_gid"].ToString());
                        objs.assetSubCategoryName = dtfa.Rows[i]["assetsubcategory_name"].ToString();
                        objsget.Add(objs);
                    }

                }
            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            GetfaConnection();
            return objsget;
        }
        public List<Ifams_Property> GetAssetDetails(Ifams_Property objfa)
        {
            GetfaConnection();
            List<Ifams_Property> objass = new List<Ifams_Property>();
            try
            {
                cmdfa = new SqlCommand("pr_ifams_GetAssetdetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                DataSet ds = new DataSet();
                dtfa.TableName = "AssetSubCategory";
                ds.Tables.Add(dtfa);
                HttpContext.Current.Session["ExcelExportAsset"] = ds;
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        Ifams_Property objassd = new Ifams_Property();
                        objassd.assetgid = dtfa.Rows[i]["AssetGid"].ToString();
                        objassd.assetCode = dtfa.Rows[i]["AsssetCode"].ToString();
                        objassd.assetDesc = dtfa.Rows[i]["AssetDescription"].ToString();
                        objassd.assetDescType = dtfa.Rows[i]["AssetDepreciationType"].ToString();
                        objassd.descRate = dtfa.Rows[i]["AssetDepreciationRate"].ToString();
                        objassd.glCode = dtfa.Rows[i]["AssetGLCode"].ToString();
                        objassd.descResGlCode = dtfa.Rows[i]["AssetDepResGLCode"].ToString();
                        objassd.descGlCode = dtfa.Rows[i]["AssetDepGLDesc"].ToString();
                        //  objassd.Barcode = (dtfa.Rows[i]["AssetBarcodeReq"].ToString() == "1") ? true : false;
                        objassd.BarcodeIsMandatory = string.IsNullOrEmpty(dtfa.Rows[i]["AssetBarcodeReq"].ToString()) ? "" : dtfa.Rows[i]["AssetBarcodeReq"].ToString();
                        objassd.classficationName = dtfa.Rows[i]["AssetClassification"].ToString();
                        objassd.ownedByName = dtfa.Rows[i]["AssetOwner"].ToString();
                        objassd.assetCategoryName = dtfa.Rows[i]["CategoryName"].ToString();
                        objassd.assetCategoryId = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[i]["Categorygid"].ToString()) ? "0" : dtfa.Rows[i]["Categorygid"].ToString());
                        objassd.umonths = (Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[i]["Lifemonth"].ToString()) ? "0" : dtfa.Rows[i]["Lifemonth"].ToString()));
                        objassd.Mandatory = string.IsNullOrEmpty(dtfa.Rows[i]["SerialNo"].ToString()) ? "" : dtfa.Rows[i]["SerialNo"].ToString();
                        objassd.NonMantadatory = string.IsNullOrEmpty(dtfa.Rows[i]["SerialNo"].ToString()) ? "" : dtfa.Rows[i]["SerialNo"].ToString();
                        objassd.IsQuantified = string.IsNullOrEmpty(dtfa.Rows[i]["AssetQtyfy"].ToString()) ? "" : dtfa.Rows[i]["AssetQtyfy"].ToString();
                        objassd.SLM = string.IsNullOrEmpty(dtfa.Rows[i]["AssetDepreciationType"].ToString()) ? "" : dtfa.Rows[i]["AssetDepreciationType"].ToString();
                        objassd.LPM = string.IsNullOrEmpty(dtfa.Rows[i]["AssetDepreciationType"].ToString()) ? "" : dtfa.Rows[i]["AssetDepreciationType"].ToString();
                        //    objassd.LPM = (dtfa.Rows[i]["AssetDepreciationType"].ToString() == "0") ? "true" : "false";
                        objassd.Verifiable = string.IsNullOrEmpty(dtfa.Rows[i]["AssetVerifiable"].ToString()) ? "" : dtfa.Rows[i]["AssetVerifiable"].ToString();
                        objassd.NonVerifiable = string.IsNullOrEmpty(dtfa.Rows[i]["AssetVerifiable"].ToString()) ? "" : dtfa.Rows[i]["AssetVerifiable"].ToString();
                        objassd.Movable = string.IsNullOrEmpty(dtfa.Rows[i]["AssetType"].ToString()) ? "" : dtfa.Rows[i]["AssetType"].ToString();
                        objassd.Immovable = string.IsNullOrEmpty(dtfa.Rows[i]["AssetType"].ToString()) ? "" : dtfa.Rows[i]["AssetType"].ToString();
                        objassd.Hsn_gid = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[i]["hsngid"].ToString()) ? "0" : dtfa.Rows[i]["hsngid"].ToString());
                        objassd.hsn_code = string.IsNullOrEmpty(dtfa.Rows[i]["hsn_code"].ToString()) ? "" : dtfa.Rows[i]["hsn_code"].ToString();
                        objassd.hsn_desc = string.IsNullOrEmpty(dtfa.Rows[i]["hsndesc"].ToString()) ? "" : dtfa.Rows[i]["hsndesc"].ToString();
                        objass.Add(objassd);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            GetfaConnection();
            return objass;
        }
        public string InsertAssetDetails(Ifams_Property objin, string hsn)
        {
            string Result = string.Empty;
            int res;
            try
            {
                string CAT = objin.assetCode.ToUpper();
                CAT = (CAT.ToString());
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetfaConnection();
                cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = objin.assetCategoryId;
                cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = CAT;
                cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "Existasset";
                // cmdfa.Parameters.Add("@res", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
                Result = Convert.ToString(cmdfa.ExecuteScalar());
                //   Result = cmdfa.Parameters["@res"].Value.ToString();
                if (Result == "Not There")
                {
                    //kavitha

                    GetfaConnection();
                    cmdfa = new SqlCommand("PR_FA_SET_ASSET", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.Add("@asset_code", SqlDbType.VarChar).Value = objin.assetCode.ToString().ToUpper();
                    cmdfa.Parameters.Add("@asset_description", SqlDbType.VarChar).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objin.assetDesc);
                    cmdfa.Parameters.Add("@asset_owner", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objin.ownedByName) ? "" : objin.ownedByName.ToString().ToUpper();
                    cmdfa.Parameters.Add("@asset_dep_rate", SqlDbType.VarChar).Value = objin.descRate.ToString();
                    cmdfa.Parameters.Add("@asset_glcode", SqlDbType.Int).Value = objin.glCode.ToString();
                    cmdfa.Parameters.Add("@asset_dep_reservglcode", SqlDbType.Int).Value = objin.descResGlCode.ToString();
                    cmdfa.Parameters.Add("@asset_dep_glcode", SqlDbType.Int).Value = string.IsNullOrEmpty(objin.descGlCode.ToString()) ? "" : objin.descGlCode.ToString();
                    cmdfa.Parameters.Add("@asset_barcode_req", SqlDbType.VarChar).Value = objin.BarcodeIsMandatory;
                    cmdfa.Parameters.Add("@asset_classification", SqlDbType.VarChar).Value = objin.classficationName;
                    cmdfa.Parameters.Add("@asset_qtyfy", SqlDbType.VarChar).Value = objin.IsQuantified;
                    cmdfa.Parameters.Add("@aset_serial_no", SqlDbType.VarChar).Value = objin.Mandatory.ToString();
                    cmdfa.Parameters.Add("@Asset_life_month", SqlDbType.Int).Value = objin.umonths.ToString();
                    cmdfa.Parameters.Add("@asset_category_gid", SqlDbType.Int).Value = objin.assetCategoryId.ToString();
                    cmdfa.Parameters.Add("@asset_verifiable", SqlDbType.VarChar).Value = objin.Verifiable.ToString();
                    cmdfa.Parameters.Add("@asset_type", SqlDbType.VarChar).Value = objin.Movable.ToString();
                    cmdfa.Parameters.Add("@asset_isremoved", SqlDbType.VarChar).Value = "N";
                    cmdfa.Parameters.Add("@asset_dep_type", SqlDbType.VarChar).Value = objin.assetDescType.ToString();
                    cmdfa.Parameters.Add("@asset_hsngid", SqlDbType.VarChar).Value = hsn.ToString();
                    cmdfa.Parameters.Add("@user", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid(); 
                    cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "create";
                    res=cmdfa.ExecuteNonQuery();
                    if(res==-1)
                    {
                        Result="success";
                    }
                    //    string[,] incode = new string[,]
                    //{
                    //{"asset_code",objin.assetCode.ToString().ToUpper()},
                    //{"asset_description",CultureInfo.CurrentCulture.TextInfo.ToTitleCase( objin.assetDesc)},
                    //{"asset_owner", string.IsNullOrEmpty(objin.ownedByName)?"":objin.ownedByName.ToString().ToUpper()},
                    //{"asset_dep_rate",objin.descRate.ToString()},
                    //{"asset_glcode",objin.glCode.ToString()},
                    //{"asset_dep_reservglcode",objin.descResGlCode.ToString()},
                    //{"asset_dep_glcode", string.IsNullOrEmpty(objin.descGlCode.ToString())?"":objin.descGlCode.ToString()},
                    //{"asset_barcode_req",objin.BarcodeIsMandatory},
                    //{"asset_classification",objin.classficationName},
                    //{"asset_qtyfy",objin.IsQuantified},
                    //{"aset_serial_no",objin.Mandatory.ToString()},
                    //{"Asset_life_month",objin.umonths.ToString()},    
                    //{"asset_category_gid",objin.assetCategoryId.ToString()},
                    //{"asset_verifiable",objin.Verifiable.ToString()},
                    //{"asset_type",objin.Movable.ToString()},
                    //{"asset_isremoved","N"},
                    //{"asset_dep_type",objin.assetDescType.ToString()}, 
                    //{"asset_hsngid",objin.Hsn_gid.ToString()}
                    //};

                    //    string tblname = "iem_mst_tasset";
                    //    Result = commfa.InsertCommon(incode, tblname);
                }
                else { return Result; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }

        public string UpdateAssetDetails(Ifams_Property objup, string hsn)
        {
            string Result = string.Empty;
            int res;
            try
            {
                string CAT = objup.assetCode.ToUpper();
                CAT = (CAT.ToString());
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetfaConnection();
                cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = objup.assetCategoryId;
                cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = CAT;
                cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "Existasset";
                // cmdfa.Parameters.Add("@res", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
                Result = Convert.ToString(cmdfa.ExecuteScalar());
                //   Result = cmdfa.Parameters["@res"].Value.ToString();
                if (Result == "Already Exist")
                {

                    GetfaConnection();
                    cmdfa = new SqlCommand("PR_FA_SET_ASSET", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = objup.assetgid;
                    cmdfa.Parameters.Add("@asset_code", SqlDbType.VarChar).Value = objup.assetCode.ToString().ToUpper();
                    cmdfa.Parameters.Add("@asset_description", SqlDbType.VarChar).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objup.assetDesc);
                    cmdfa.Parameters.Add("@asset_owner", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objup.ownedByName) ? "" : objup.ownedByName.ToString().ToUpper();
                    cmdfa.Parameters.Add("@asset_dep_rate", SqlDbType.VarChar).Value = objup.descRate.ToString();
                    cmdfa.Parameters.Add("@asset_glcode", SqlDbType.Int).Value = objup.glCode.ToString();
                    cmdfa.Parameters.Add("@asset_dep_reservglcode", SqlDbType.Int).Value = objup.descResGlCode.ToString();
                    cmdfa.Parameters.Add("@asset_dep_glcode", SqlDbType.Int).Value = string.IsNullOrEmpty(objup.descGlCode.ToString()) ? "" : objup.descGlCode.ToString();
                    cmdfa.Parameters.Add("@asset_barcode_req", SqlDbType.VarChar).Value = objup.BarcodeIsMandatory;
                    cmdfa.Parameters.Add("@asset_classification", SqlDbType.VarChar).Value = objup.classficationName;
                    cmdfa.Parameters.Add("@asset_qtyfy", SqlDbType.VarChar).Value = objup.IsQuantified;
                    cmdfa.Parameters.Add("@aset_serial_no", SqlDbType.VarChar).Value = objup.Mandatory.ToString();
                    cmdfa.Parameters.Add("@Asset_life_month", SqlDbType.Int).Value = objup.umonths.ToString();
                    cmdfa.Parameters.Add("@asset_category_gid", SqlDbType.Int).Value = objup.assetCategoryId.ToString();
                    cmdfa.Parameters.Add("@asset_verifiable", SqlDbType.VarChar).Value = objup.Verifiable.ToString();
                    cmdfa.Parameters.Add("@asset_type", SqlDbType.VarChar).Value = objup.Movable.ToString();
                    cmdfa.Parameters.Add("@asset_isremoved", SqlDbType.VarChar).Value = "N";
                    cmdfa.Parameters.Add("@asset_dep_type", SqlDbType.VarChar).Value = objup.assetDescType.ToString();
                    cmdfa.Parameters.Add("@asset_hsngid", SqlDbType.VarChar).Value = hsn.ToString();
                    cmdfa.Parameters.Add("@user", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                    res = cmdfa.ExecuteNonQuery();
                    if (res == -1)
                        Result = "success";
                //    string[,] upcode = new string[,]
                //{
                //{"asset_code",objup.assetCode.ToString().ToUpper()},
                //{"asset_description",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objup.assetDesc)},
                //{"asset_owner", string.IsNullOrEmpty(objup.ownedByName)?"":objup.ownedByName.ToString()},
                //{"asset_dep_rate",objup.descRate.ToString()},
                //{"asset_glcode",objup.glCode.ToString()},
                //{"asset_dep_reservglcode",objup.descResGlCode.ToString()},
                //{"asset_dep_glcode", string.IsNullOrEmpty(objup.descGlCode.ToString())?"":objup.descGlCode.ToString()},
                //{"asset_barcode_req",objup.BarcodeIsMandatory},
                //{"asset_classification",objup.classficationName},
                //{"asset_qtyfy",objup.IsQuantified},
                //{"aset_serial_no",objup.Mandatory.ToString()},
                //{"Asset_life_month",objup.umonths.ToString()},    
                //{"asset_category_gid",objup.assetCategoryId.ToString()},
                //{"asset_verifiable",objup.Verifiable.ToString()},
                //{"asset_type",objup.Movable.ToString()},
                //{"asset_isremoved","N"},
                //{"asset_dep_type",objup.assetDescType.ToString()},
                //{"asset_hsngid",objup.Hsn_gid.ToString()}
                //};

                //    string[,] upcon = new string[,]
                //{
                //    {"asset_gid",objup.assetgid}
                //};

                //    string tblupname = "iem_mst_tasset";
                //    Result = commfa.UpdateCommon(upcode, upcon, tblupname);
                }
                else { return Result; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }

        public string DeleteAssetDetails(string id)
        {
            string dresult = string.Empty;
            int res = 0;
            try
            {
                //string[,] decode = new string[,]
                //{
                //    {"asset_isremoved","Y"}
                //};
                //string[,] dewcode = new string[,]
                //{
                //    {"asset_gid",id}
                //};
                //string tbldname = "iem_mst_tasset";
                //dresult = commfa.UpdateCommon(decode, dewcode, tbldname);

                GetfaConnection();
                cmdfa = new SqlCommand("PR_FA_SET_ASSET", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                res = cmdfa.ExecuteNonQuery();

                if (res == -1)
                    dresult = "success";



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dresult;
        }

        public List<capitalizationMaker> GetECFdetails()
        {
            List<capitalizationMaker> objcpl = new List<capitalizationMaker>();
            capitalizationMaker obj = new capitalizationMaker();
            try
            {
                GetfaConnection();
                //   cmdfa = new SqlCommand("pr_ifams_getecfdetails", confa);
                cmdfa = new SqlCommand("pr_ifams_getinvoiceNew", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@status", ConfigurationManager.AppSettings["EApproved"].ToString());
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        obj = new capitalizationMaker();
                        obj.Ecfgid = Convert.ToInt32(dtfa.Rows[i]["Ecfgid"].ToString());
                        obj.EcfNo = dtfa.Rows[i]["EcfNo"].ToString();
                        obj.Ecfdate = dtfa.Rows[i]["EcfDate"].ToString();
                        obj.EcfAmount = dtfa.Rows[i]["EcfAmount"].ToString();
                        obj.invoicegid = dtfa.Rows[i]["Invoicegid"].ToString();
                        obj.invoiceno = dtfa.Rows[i]["InvoiceNo"].ToString();
                        obj.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["InvoiceAmount"].ToString());
                        obj.Vendor = dtfa.Rows[i]["VendorName"].ToString();
                        cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
                        cmdfa.CommandType = CommandType.StoredProcedure;
                        cmdfa.Parameters.AddWithValue("@for", "qty");
                        cmdfa.Parameters.AddWithValue("@invid", obj.invoicegid);
                        string MATCH = Convert.ToString(cmdfa.ExecuteScalar());
                        //if (MATCH == "doesnt match" || string.IsNullOrEmpty(MATCH))
                        //{
                        //    objcpl.Add(obj);
                        //}
                        objcpl.Add(obj);
                    }
                }
                GetfaConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objcpl;
        }

        public DataTable Getinvoice(int id)
        {
            DataTable dtGetInvoice = new DataTable();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_getinvoice", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@ecfgid", id);
                dafa = new SqlDataAdapter(cmdfa);
                dafa.Fill(dtGetInvoice);
                GetfaConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dtGetInvoice;
        }

        public List<capitalizationMaker> Getpoqtydetails(int id)
        {
            List<capitalizationMaker> objqty = new List<capitalizationMaker>();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_getpoqtydetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invgid", id);
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        capitalizationMaker objb = new capitalizationMaker();
                        objb.pohgid = Convert.ToInt32(dtfa.Rows[i]["Poheadergid"].ToString());
                        objb.ponumber = dtfa.Rows[i]["PoNumber"].ToString();
                        objb.orderdqty = Convert.ToInt32(dtfa.Rows[i]["orderedquantity"].ToString());
                        objb.invoiceqty = Convert.ToInt32(dtfa.Rows[i]["InvoiceQuantity"].ToString());
                        objb.Receivedqty = Convert.ToInt32(dtfa.Rows[i]["Recevedquantity"].ToString());
                        objb.invoiceno = dtfa.Rows[i]["InvoiceNo"].ToString();
                        HttpContext.Current.Session["invoiceNo"] = objb.invoiceno;
                        objqty.Add(objb);
                    }

                }
                GetfaConnection();
            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            return objqty;
        }

        //   public List<capitalizationMaker> Getpodetails(capitalizationMaker CAP)
        public List<capitalizationMaker> Getpodetails(int id)
        {
            List<capitalizationMaker> objpod = new List<capitalizationMaker>();
            int subcat_gid;
            int loc_gid = 0;
            capitalizationMaker objpo = new capitalizationMaker();
            decimal tax = Convert.ToDecimal("0.00");
            decimal total = Convert.ToDecimal("0.00");
            try
            {

                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_getpodetailsse_new", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@inv", id);
                //cmdfa.Parameters.AddWithValue("@pogid",string.Empty);
                // cmdfa.Parameters.AddWithValue("@status", ConfigurationManager.AppSettings["EApproved"].ToString());
                //cmdfa.Parameters.AddWithValue("@locationCode", string.Empty);
                //cmdfa.Parameters.AddWithValue("@AssetCode", comuidfa.GetLoginUserGid().ToString());
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);

                if (dtfa.Rows.Count > 0)
                {

                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        
                        // loc_gid = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[i]["location_gid"].ToString()) ? "0" : dtfa.Rows[i]["location_gid"].ToString());
                        //   capitalizationMaker objpo = new capitalizationMaker();
                        //   objpo.index = i + 1;
                        //   objpo.Ecfgid = Convert.ToInt32(dtfa.Rows[i]["ECFgid"]);
                        //   objpo.invoicegid = dtfa.Rows[i]["invoice_gid"].ToString();
                        //   objpo.invoiceamount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["invoice_amount"].ToString()));
                        //   objpo.invoiceno = dtfa.Rows[i]["invoice_no"].ToString();
                        //   objpo.pohgid = Convert.ToInt32(dtfa.Rows[i]["Pogid"].ToString());
                        //   objpo.ponumber = dtfa.Rows[i]["PONumber"].ToString();
                        //   objpo.AssetCode = dtfa.Rows[i]["AssetCode"].ToString();
                        //   objpo.AssetName = dtfa.Rows[i]["AssetDesc"].ToString();
                        //   objpo.glcode = dtfa.Rows[i]["GLCode"].ToString();
                        //   objpo.UOM = dtfa.Rows[i]["UOM"].ToString();
                        //   objpo.Quantity = Convert.ToInt32(dtfa.Rows[i]["POQty"].ToString());
                        //   objpo.Receivedqty = Convert.ToInt32(dtfa.Rows[i]["ReceviedQty"].ToString());
                        //   objpo.Amount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Price"].ToString()));
                        //   objpo.Discount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Discount"].ToString()));
                        //   objpo.Tax1 = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Taxone"].ToString()));
                        //   objpo.Tax2 = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Taxtwo"].ToString()));
                        //   objpo.othres = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Taxthree"].ToString()));
                        ////   objpo.TotalAmount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["POTotal"].ToString()));
                        //   objpo.TotalAmount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Price"].ToString()));
                        //   objpo.AssetCategory = dtfa.Rows[i]["assetcategory_name"].ToString();
                        //   objpo.AssetSubcategpry = dtfa.Rows[i]["assetsubcategory_name"].ToString();
                        //   objpo.ecfamt = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["ecf_amount"].ToString()));
                        //   objpo.EcfNo = dtfa.Rows[i]["ecf_no"].ToString();
                        //   objpo.Ecfdate = dtfa.Rows[i]["ecf_date"].ToString();
                        //   objpo.Vendor = dtfa.Rows[i]["supplierheader_name"].ToString();
                        //   objpo.ddlSubcategory = new SelectList(GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry", selectedValue: subcat_gid);
                        
                        SqlCommand cmdpo = new SqlCommand("pr_ifams_getaddpodetails", confa);
                        cmdpo.CommandType = CommandType.StoredProcedure;                        
                        cmdpo.Parameters.AddWithValue("@invoicegid", dtfa.Rows[i]["invoice_gid"].ToString());
                        cmdpo.Parameters.AddWithValue("@grnStatus", ConfigurationManager.AppSettings["EApproved"].ToString());
                        cmdpo.Parameters.AddWithValue("@Poheader_gid", dtfa.Rows[i]["Pogid"].ToString());
                        DataTable dtpo = new DataTable();
                        SqlDataAdapter dapo = new SqlDataAdapter(cmdpo);
                        dapo.Fill(dtpo);
                        if (dtpo.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtpo.Rows.Count; j++)
                            {
                                //string CheckBranch = "true";
                                //if (dtfa.Rows[i]["DepType"].ToString() == "SLM" && dtpo.Rows[j]["BranchStartDate"].ToString() == "")
                                //{
                                //    CheckBranch = "false";
                                //}
                                //else if (dtfa.Rows[i]["DepType"].ToString() == "LPM" && (dtpo.Rows[j]["LeaseStartDate"].ToString() == "" || dtpo.Rows[j]["LeaseEndDate"].ToString() == "" || dtpo.Rows[j]["BranchStartDate"].ToString() == ""))
                                //{
                                //    CheckBranch = "false";
                                //}
                                //if (CheckBranch == "true")
                                //{
                                objpo = new capitalizationMaker();
                                subcat_gid = Convert.ToInt32(string.IsNullOrEmpty(dtpo.Rows[j]["assetsubcategory_gid"].ToString()) ? "0" : dtpo.Rows[j]["assetsubcategory_gid"].ToString());
                                loc_gid = Convert.ToInt32(string.IsNullOrEmpty(dtpo.Rows[j]["location_gid"].ToString()) ? "0" : dtpo.Rows[j]["location_gid"].ToString());
                                objpo.index = j + 1;
                                objpo.Ecfgid = Convert.ToInt32(dtfa.Rows[i]["ECFgid"]);
                                objpo.invoicegid = dtfa.Rows[i]["invoice_gid"].ToString();
                                objpo.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["invoice_amount"].ToString());
                                objpo.invoiceno = dtfa.Rows[i]["invoice_no"].ToString();
                                objpo.pohgid = Convert.ToInt32(dtfa.Rows[i]["Pogid"].ToString());
                                objpo.podetgid = Convert.ToInt32(dtpo.Rows[j]["podetails_gid"].ToString());
                                objpo.ponumber = dtpo.Rows[j]["PONumber"].ToString();
                                objpo.AssetCode = dtpo.Rows[j]["AssetCode"].ToString();
                                objpo.AssetName = dtpo.Rows[j]["AssetDesc"].ToString();
                                objpo.DEPRATE = dtpo.Rows[j]["DEPRATE"].ToString();
                                objpo.glcode = dtpo.Rows[j]["GLCode"].ToString();
                                objpo.UOM = dtpo.Rows[j]["UOM"].ToString();
                                objpo.Quantity = Convert.ToDecimal(dtpo.Rows[j]["POQty"].ToString());
                                objpo.Amount = Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["Price"].ToString()) ? "0" : dtpo.Rows[j]["Price"].ToString());
                                objpo.Discount = Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["Discount"].ToString()) ? "0" : dtpo.Rows[j]["Discount"].ToString());
                                //objpo.Tax1 = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Taxone"].ToString()));
                                //  objpo.Tax1 = Math.Round(tax);
                                objpo.Tax1 = Math.Round(Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["SplitedInvTaxAmnt"].ToString()) ? "0" : dtpo.Rows[j]["SplitedInvTaxAmnt"].ToString()), 2);
                                objpo.Tax2 = Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["Taxtwo"].ToString()) ? "0" : dtpo.Rows[j]["Taxtwo"].ToString());
                                objpo.othres = Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["Taxthree"].ToString()) ? "0" : dtpo.Rows[j]["Taxthree"].ToString());
                                // objpo.TotalAmount = Math.Round(Convert.ToDecimal(dtfa.Rows[i]["Price"].ToString()));
                                objpo.TotalAmount = Convert.ToDecimal(string.IsNullOrEmpty(dtpo.Rows[j]["Total"].ToString()) ? "0" : dtpo.Rows[j]["Total"].ToString());
                                decimal totamntnew = (objpo.TotalAmount + objpo.Tax2 + objpo.othres) - objpo.Discount;
                                objpo.TotalAmount = Math.Round(totamntnew, 2);
                                objpo.AssetCategory = dtpo.Rows[j]["assetcategory_name"].ToString();
                                objpo.Assetcategoryid = Convert.ToInt32(string.IsNullOrEmpty(dtpo.Rows[j]["asset_category_gid"].ToString()) ? "0" : dtpo.Rows[j]["asset_category_gid"].ToString());
                                objpo.AssetSubcategpry = dtpo.Rows[j]["assetsubcategory_name"].ToString();
                                objpo.ecfamt = Convert.ToDecimal(dtfa.Rows[i]["ecf_amount"].ToString());
                                objpo.EcfNo = dtfa.Rows[i]["ecf_no"].ToString();
                                objpo.Ecfdate = dtfa.Rows[i]["ecf_date"].ToString();
                                objpo.Vendor = dtfa.Rows[i]["supplierheader_name"].ToString();
                                objpo.ddlCategory = new SelectList(GetAssetCategory(), "Assetcategoryid", "Assetcategpry");
                                objpo.ddlSubcategory = new SelectList(GetAssetSubCategory(objpo.Assetcategoryid.ToString()), "Assetsubcategoryid", "AssetSubcategpry");
                                objpo.Receivedqty = Convert.ToDecimal(dtpo.Rows[j]["ReceviedQty"].ToString());
                                objpo.GRNNo = dtpo.Rows[j]["GRNNo"].ToString();
                                objpo.GRNDate = dtpo.Rows[j]["GRNDate"].ToString();
                                objpo.GRNStaus = dtpo.Rows[j]["GRNStatus"].ToString();
                                //   objpo.CAPDATE = dtpo.Rows[j]["CAPDATE"].ToString();
                                objpo.DCNo = dtpo.Rows[j]["DCNo"].ToString();
                                objpo.locationName = dtpo.Rows[j]["LocationName"].ToString();
                                objpo.Grn_gid = Convert.ToInt32(dtpo.Rows[j]["grninwrdheader_gid"].ToString());
                                objpo.Grn_detgid = Convert.ToInt32(dtpo.Rows[j]["grninwrddet_gid"].ToString());
                                objpo.shipmentType = dtpo.Rows[j]["shipment_type"].ToString();
                                objpo.LocationCode = dtpo.Rows[j]["LocationCode"].ToString();
                                objpo.AssetSrlno = dtpo.Rows[j]["grninwrddet_assetsrlno"].ToString();
                                objpo.Assetmfn = dtpo.Rows[j]["grninwrddet_mft_name"].ToString();
                                objpo.SplitedTaxAmnt = dtpo.Rows[j]["SplitedInvTaxAmnt"].ToString();
                                objpo.Description = dtpo.Rows[j]["podetails_desc"].ToString();
                                //  objpo.AssetSubcategpry = subcat_gid;
                                objpod.Add(objpo);

                                string[,] incode = new string[,]
                                       {
                                           {"podetails_index", objpo.index.ToString()},
                                           {"podetails_ecfno",objpo.Ecfgid.ToString()},
                                           {"podetails_amount",objpo.Amount.ToString()},
                                           {"podetails_tax1",objpo.Tax1.ToString()},
                                           {"podetails_tax2",objpo.Tax2.ToString()},
                                           {"podetails_discount",objpo.Discount .ToString()},
                                           {"podetails_others",objpo.othres.ToString()},
                                           {"podetails_invoiceno",objpo.invoiceno.ToString()},
                                           {"podetails_ponumber",objpo.ponumber.ToString()},
                                           {"podetails_assetCode",objpo.AssetCode .ToString()},
                                           {"podetails_assetname",objpo.AssetName.ToString()},
                                           {"podetails_assetglcode", objpo.glcode .ToString()},
                                           {"podetails_location",objpo.LocationCode.ToString()},
                                           {"podetails_grnno", objpo.GRNNo},
                                           {"podetails_grndate",objpo.GRNDate},
                                           {"podetails_uom",objpo.UOM },
                                           {"podetails_poQty", objpo.Quantity.ToString()},
                                           {"podetails_poRevQty", objpo.Receivedqty.ToString()},
                                           {"podetails_DCno",objpo.DCNo.ToString()},
                                           {"podetails_Totalamt",objpo.TotalAmount.ToString() },
                                           {"podetails_invoice_gid", objpo.invoicegid.ToString()},
                                           {"podetails_invoiceamt",objpo.invoiceamount.ToString()},
                                            {"podetails_insertby",comuidfa.GetLoginUserGid().ToString()},
                                            {"Assetcategory",objpo.AssetCategory.ToString()},
                                            {"AssetSubcategory",objpo.AssetSubcategpry.ToString()},
                                            {"ShipmentType",objpo.shipmentType.ToString()},
                                            {"Subcategorygid",subcat_gid.ToString()},
                                            {"loc_gid",loc_gid.ToString()},
                                            {"Grn_gid",objpo.Grn_gid.ToString()},
                                            {"podetails_asssrlno",objpo.AssetSrlno},
                                            {"podetails_assmfn",objpo.Assetmfn},
                                            {"podetails_po_gid",objpo.pohgid.ToString()},
                                            {"podetials_Grn_detgid",objpo.Grn_detgid.ToString()},
                                            {"podetails_podetgid",objpo.podetgid.ToString()},
                                            {"podetails_locationname",objpo.locationName},
                                           {"podetails_desc",objpo.Description.Replace("'","")}
                                       };
                                string Result = commfa.InsertCommon(incode, "fa_tmp_podetails");

                            }
                            //}

                        }

                        else
                        {
                            if (objpod.Count == 0 || objpod == null)
                            {
                                objpod = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                            }

                        }




                    }
                }

                GetfaConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }


            return objpod;

        }


        public List<capitalizationMaker> Getcapitalized(int id, string status)
        {
            List<capitalizationMaker> objpodl = new List<capitalizationMaker>();
            capitalizationMaker objpo = new capitalizationMaker();
            try
            {
                GetfaConnection();
                SqlCommand cmdpo = new SqlCommand("pr_ifams_getcapfulldetilsnew", confa);
                cmdpo.CommandType = CommandType.StoredProcedure;
                cmdpo.Parameters.AddWithValue("@invgid", id);
                cmdpo.Parameters.AddWithValue("@Status", status);

                DataTable dtpo = new DataTable();
                SqlDataAdapter dapo = new SqlDataAdapter(cmdpo);
                dapo.Fill(dtpo);
                if (dtpo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtpo.Rows.Count; i++)
                    {
                        objpo = new capitalizationMaker();
                        //loc_gid = Convert.ToInt32(string.IsNullOrEmpty(dtpo.Rows[j]["location_gid"].ToString()) ? "0" : dtpo.Rows[j]["location_gid"].ToString());
                        objpo.index = i + 1;
                        objpo.Ecfgid = Convert.ToInt32(dtpo.Rows[i]["cap_Ecfid"]);
                        objpo.invoicegid = dtpo.Rows[i]["cap_Invoiceid"].ToString();
                        objpo.ponumber = dtpo.Rows[i]["cap_Ponumber"].ToString();
                        objpo.GRNNo = dtpo.Rows[i]["cap_GrnNo"].ToString();
                        objpo.LocationCode = dtpo.Rows[i]["cap_AssetLocation"].ToString();
                        objpo.locationName = dtpo.Rows[i]["cap_Loc_Code"].ToString();
                        objpo.Amount = Convert.ToDecimal(dtpo.Rows[i]["cap_Amount"].ToString());
                        objpo.Quantity = Convert.ToDecimal(dtpo.Rows[i]["cap_qty"].ToString());
                        objpo.Discount = Convert.ToDecimal(dtpo.Rows[i]["cap_Discount"].ToString());
                        objpo.Tax1 = Convert.ToDecimal(dtpo.Rows[i]["cap_Tax1"].ToString());
                        objpo.SplitedTaxAmnt = string.IsNullOrEmpty(dtpo.Rows[i]["cap_Tax1"].ToString()) ? "0" : dtpo.Rows[i]["cap_Tax1"].ToString();
                        objpo.othres = Convert.ToDecimal(dtpo.Rows[i]["cap_others"].ToString());
                        objpo.TotalAmount = Convert.ToDecimal(dtpo.Rows[i]["cap_Total"].ToString());
                        objpo.glcode = dtpo.Rows[i]["cap_glcode"].ToString();
                        objpo.AssetCode = dtpo.Rows[i]["cap_AssetCode"].ToString();
                        objpo.AssetName = dtpo.Rows[i]["cap_AssetName"].ToString();
                        objpo.invoiceamount = Convert.ToDecimal(dtpo.Rows[i]["cap_Invoiceamt"].ToString());
                        objpo.Receivedqty = Convert.ToDecimal(dtpo.Rows[i]["cap_Rqty"].ToString());
                        objpo.UOM = dtpo.Rows[i]["cap_uom"].ToString();
                        objpo.AssetCategory = dtpo.Rows[i]["cap_Category"].ToString();
                        objpo.AssetSubcategpry = dtpo.Rows[i]["cap_Subcategory"].ToString();
                        objpo.shipmentType = dtpo.Rows[i]["cap_ShipmentType"].ToString();
                        objpo.GRNDate = dtpo.Rows[i]["cap_Grndate"].ToString();
                        objpo.DCNo = dtpo.Rows[i]["cap_DcNo"].ToString();
                        objpo.AssetSrlno = dtpo.Rows[i]["cap_assetsrlno"].ToString();
                        objpo.pohgid = Convert.ToInt32(dtpo.Rows[i]["cap_po_gid"].ToString());
                        objpo.invoiceno = dtpo.Rows[i]["invoice_no"].ToString();
                        objpo.CAPDATE = dtpo.Rows[i]["cap_Grndate"].ToString();
                        objpo.DEPRATE = dtpo.Rows[i]["ASSETDEP"].ToString();

                        objpo.Tax2 = Convert.ToDecimal(dtpo.Rows[i]["Taxtwo"].ToString());

                        objpo.ecfamt = Convert.ToDecimal(dtpo.Rows[i]["ecf_amount"].ToString());
                        objpo.EcfNo = dtpo.Rows[i]["ecf_no"].ToString();
                        objpo.Ecfdate = dtpo.Rows[i]["ecf_date"].ToString();

                        objpo.Vendor = dtpo.Rows[i]["supplierheader_name"].ToString();

                        objpo.ddlSubcategory = new SelectList(GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry");
                        objpo.GRNStaus = dtpo.Rows[i]["GRNStatus"].ToString();
                        // objpo.ddlbranch = new SelectList(GetBranchandlocation(), "ddllocaid", "ddllocacode", selectedValue: loc_gid);
                        objpo.locationName = dtpo.Rows[i]["LocationName"].ToString();
                        objpo.Grn_gid = Convert.ToInt32(dtpo.Rows[i]["grninwrdheader_gid"].ToString());
                        objpo.Assetmfn = dtpo.Rows[i]["grninwrddet_mft_name"].ToString();
                        objpo.Description = dtpo.Rows[i]["cap_podetails_desc"].ToString();
                        objpodl.Add(objpo);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objpodl;
        }

        public List<capitalizationMaker> GetCapitalizationgrid(int id)
        {
            List<capitalizationMaker> objca = new List<capitalizationMaker>();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_getpodetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@pogid", id);
                cmdfa.Parameters.AddWithValue("@status", 5);
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        capitalizationMaker objcab = new capitalizationMaker();
                        objcab.AssetCode = dtfa.Rows[i]["AssetCode"].ToString();
                        objcab.glcode = dtfa.Rows[i]["GLCode"].ToString();
                        objcab.LocationCode = dtfa.Rows[i]["LocationCode"].ToString();
                        objcab.Quantity = Convert.ToDecimal(dtfa.Rows[i]["POQty"].ToString());
                        objcab.Amount = Convert.ToDecimal(dtfa.Rows[i]["Price"].ToString());
                        objcab.Discount = Convert.ToDecimal(dtfa.Rows[i]["Discount"].ToString());
                        objcab.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["Taxone"].ToString());
                        objcab.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["Taxtwo"].ToString());
                        objcab.othres = Convert.ToDecimal(dtfa.Rows[i]["Taxthree"].ToString());
                        objcab.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["POTotal"].ToString());
                        objca.Add(objcab);
                    }
                }
                GetfaConnection();
            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            return objca;
        }

        public string UpdateAmtDetails(capitalizationMaker cap)
        {
            List<capitalizationMaker> objca = new List<capitalizationMaker>();
            try
            {
                GetfaConnection();




            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return "success";


        }



        public List<capitalizationMaker> getapprovedetails()
        {
            List<capitalizationMaker> objGetInvoice = new List<capitalizationMaker>();
            capitalizationMaker obj = new capitalizationMaker();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_getcapinvoice", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        obj = new capitalizationMaker();
                        obj.EcfNo = dtfa.Rows[i]["EcfNo"].ToString();
                        obj.Ecfdate = dtfa.Rows[i]["ecf_date"].ToString();
                        obj.EcfAmount = dtfa.Rows[i]["ecf_amount"].ToString();
                        obj.invoicegid = dtfa.Rows[i]["Invoicegid"].ToString();
                        obj.invoiceno = dtfa.Rows[i]["InvoiceNo"].ToString();
                        obj.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["InvoiceAmount"].ToString());
                        obj.Status = dtfa.Rows[i]["Status"].ToString();
                        obj.Vendor = dtfa.Rows[i]["supplierheader_name"].ToString();
                        objGetInvoice.Add(obj);
                    }
                    obj.GetApproveList = objGetInvoice;
                }
                else
                {
                    obj.GetApproveList = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            finally { GetfaConnection(); }
            return obj.GetApproveList;

        }

        public List<capitalizationMaker> getapprovedetailswait()
        {
            List<capitalizationMaker> objGetInvoice = new List<capitalizationMaker>();
            capitalizationMaker obj = new capitalizationMaker();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_getcapinvoicewait", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@status", ConfigurationManager.AppSettings["Waitingforapproval"].ToString());
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        obj = new capitalizationMaker();
                        obj.EcfNo = dtfa.Rows[i]["EcfNo"].ToString();
                        obj.Ecfdate = dtfa.Rows[i]["ecf_date"].ToString();
                        obj.EcfAmount = dtfa.Rows[i]["ecf_amount"].ToString();
                        obj.invoicegid = dtfa.Rows[i]["Invoicegid"].ToString();
                        obj.invoiceno = dtfa.Rows[i]["InvoiceNo"].ToString();
                        obj.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["InvoiceAmount"].ToString());
                        obj.Status = dtfa.Rows[i]["Status"].ToString();
                        obj.Vendor = dtfa.Rows[i]["supplierheader_name"].ToString();
                        objGetInvoice.Add(obj);
                    }
                    obj.Getapplist = objGetInvoice;
                }
                else
                {
                    obj.Getapplist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            finally { GetfaConnection(); }
            return obj.Getapplist;

        }


        public capitalizationMaker getcapapprovaldetails(int id)
        {
            List<capitalizationMaker> objgetasset = new List<capitalizationMaker>();
            capitalizationMaker objdetail = new capitalizationMaker();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_getcapdetials", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invoicegid", id);
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        objdetail = new capitalizationMaker();
                        objdetail.invoicegid = dtfa.Rows[i]["invoice_gid"].ToString();
                        objdetail.invoiceno = dtfa.Rows[i]["invoice_no"].ToString();
                        objdetail.ponumber = dtfa.Rows[i]["cap_Ponumber"].ToString();
                        objdetail.AssetCode = dtfa.Rows[i]["cap_AssetCode"].ToString();
                        // objdetail.LocationCode = dtfa.Rows[i]["location_code"].ToString();
                        objdetail.locationName = dtfa.Rows[i]["cap_AssetLocation"].ToString();
                        objdetail.GRNNo = dtfa.Rows[i]["cap_GrnNo"].ToString();
                        objdetail.Amount = Convert.ToDecimal(dtfa.Rows[i]["cap_Amount"].ToString());
                        objdetail.orderdqty = Convert.ToDecimal(dtfa.Rows[i]["cap_qty"].ToString());
                        objdetail.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["cap_Rqty"].ToString());
                        objdetail.Discount = Convert.ToDecimal(dtfa.Rows[i]["cap_Discount"].ToString());
                        objdetail.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["cap_Tax1"].ToString());
                        objdetail.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["cap_Tax2"].ToString());
                        objdetail.othres = Convert.ToDecimal(dtfa.Rows[i]["cap_others"].ToString());
                        objdetail.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["cap_Total"].ToString());
                        objdetail.Description = dtfa.Rows[i]["cap_Descr"].ToString();
                        objdetail.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["cap_Invoiceamt"].ToString());
                        objdetail.Status = dtfa.Rows[i]["cap_Status"].ToString();
                        objdetail.glcode = dtfa.Rows[i]["cap_glcode"].ToString();
                        objdetail.AssetCategory = dtfa.Rows[i]["cap_Category"].ToString();
                        objdetail.AssetSubcategpry = dtfa.Rows[i]["cap_Subcategory"].ToString();
                        objdetail.shipmentType = dtfa.Rows[i]["cap_ShipmentType"].ToString();
                        objdetail.AssetName = dtfa.Rows[i]["cap_AssetName"].ToString();
                        objdetail.UOM = dtfa.Rows[i]["cap_uom"].ToString();
                        objdetail.GRNDate = dtfa.Rows[i]["cap_Grndate"].ToString();
                        objdetail.DCNo = dtfa.Rows[i]["cap_DcNo"].ToString();
                        objgetasset.Add(objdetail);

                    }
                    objdetail.Getdetapplist = objgetasset;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { GetfaConnection(); }
            return objdetail;
        }

        public string deletetemp(string id)
        {
            try
            {
                confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                confa.Open();
                string squery = "delete from fa_tmp_podetails where podetails_invoice_gid=" + id + "";
                cmdfa = new SqlCommand(squery, confa);
                cmdfa.CommandType = CommandType.Text;
                cmdfa.ExecuteNonQuery();
            }
            catch (Exception ex) { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            finally { confa.Close(); }
            return "";
        }
        public capitalizationMaker UpdateIndexDetails(capitalizationMaker model, string invoice)
        {
            string Result = string.Empty;
            capitalizationMaker objdetail = new capitalizationMaker();
            List<capitalizationMaker> objgetasset = new List<capitalizationMaker>();
            string code = string.Empty,
             category = string.Empty,
             Subcategory = string.Empty,
             glcode = string.Empty;
            try
            {
                GetfaConnection();
                SqlCommand com = new SqlCommand("pr_ifams_trn_getglmaincategory", confa);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Subcategorygid", model.AssetSubcategpry);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    code = dt.Rows[0]["asset_code"].ToString();
                    category = dt.Rows[0]["assetcategory_name"].ToString();
                    Subcategory = dt.Rows[0]["assetsubcategory_name"].ToString();
                    glcode = dt.Rows[0]["assetcategory_glno"].ToString();
                }

                string[,] incode = new string[,]
                   {
                       {"podetails_tax1",model.Tax1.ToString()},
                       {"podetails_assetCode",code.ToString()},
                       {"podetails_discount",model.Discount.ToString()},
                       {"podetails_others",model.othres.ToString()},
                       {"podetails_Totalamt",model.TotalAmount.ToString()},
                       {"Assetcategory",category},
                       {"AssetSubcategory",Subcategory},
                       {"podetails_assetglcode",glcode},
                       {"podetails_amount",model.Amount.ToString()},
                        {"podetails_desc",model.Description.ToString()},
                       {"podetails_insertby",comuidfa.GetLoginUserGid().ToString()}
                    //   {"podetails_location",Getloname(model.locationName)}
                                         
                   };
                string[,] wincode = new string[,]
                   {
                        {"podetails_index",model.index.ToString()},
                        {"podetails_invoiceno",invoice}
                   };

                Result = commfa.UpdateCommon(incode, wincode, "fa_tmp_podetails");
                if (Result == "Success")
                {

                    cmdfa = new SqlCommand("pr_ifams_trn_Getmodpodetails", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@invoiceno", invoice);
                    cmdfa.Parameters.AddWithValue("@user", comuidfa.GetLoginUserGid().ToString());
                    dafa = new SqlDataAdapter(cmdfa);
                    dtfa = new DataTable();
                    dafa.Fill(dtfa);
                    if (dtfa.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtfa.Rows.Count; i++)
                        {
                            objdetail = new capitalizationMaker();
                            objdetail.index = Convert.ToInt32(dtfa.Rows[i]["podetails_index"]);
                            objdetail.invoiceno = dtfa.Rows[i]["podetails_invoiceno"].ToString();
                            objdetail.ponumber = dtfa.Rows[i]["podetails_ponumber"].ToString();
                            objdetail.AssetCode = dtfa.Rows[i]["podetails_assetCode"].ToString();
                            objdetail.AssetName = dtfa.Rows[i]["podetails_assetname"].ToString();
                            objdetail.LocationCode = dtfa.Rows[i]["podetails_location"].ToString();
                            objdetail.locationName = dtfa.Rows[i]["podetails_locationname"].ToString();
                            objdetail.GRNNo = dtfa.Rows[i]["podetails_grnno"].ToString();
                            objdetail.GRNDate = dtfa.Rows[i]["podetails_grndate"].ToString();
                            objdetail.glcode = dtfa.Rows[i]["podetails_assetglcode"].ToString();
                            objdetail.Amount = Convert.ToDecimal(dtfa.Rows[i]["podetails_amount"].ToString());
                            objdetail.Quantity = Convert.ToDecimal(dtfa.Rows[i]["podetails_poQty"].ToString());
                            objdetail.UOM = dtfa.Rows[i]["podetails_uom"].ToString();
                            objdetail.Discount = Convert.ToDecimal(dtfa.Rows[i]["podetails_discount"].ToString());
                            objdetail.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString());
                            objdetail.SplitedTaxAmnt = string.IsNullOrEmpty(dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString()) ? "0" : dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString();
                            // objdetail.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax2"].ToString());
                            objdetail.othres = Convert.ToDecimal(dtfa.Rows[i]["podetails_others"].ToString());
                            objdetail.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["podetails_Totalamt"].ToString());
                            objdetail.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["podetails_poRevQty"].ToString());
                            //    objdetail.Description = dtfa.Rows[i]["cap_Descr"].ToString();
                            objdetail.DCNo = dtfa.Rows[i]["podetails_DCno"].ToString();
                            objdetail.invoicegid = dtfa.Rows[i]["podetails_invoice_gid"].ToString();
                            objdetail.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["podetails_invoiceamt"].ToString());
                            objdetail.AssetCategory = dtfa.Rows[i]["Assetcategory"].ToString();
                            objdetail.Assetcategoryid = Convert.ToInt32(dtfa.Rows[i]["asset_category_gid"].ToString());
                            objdetail.AssetSubcategpry = dtfa.Rows[i]["AssetSubcategory"].ToString();
                            objdetail.shipmentType = dtfa.Rows[i]["ShipmentType"].ToString();
                            objdetail.Grn_detgid = Convert.ToInt32(dtfa.Rows[i]["podetials_Grn_detgid"].ToString());
                            objdetail.ddlCategory = new SelectList(GetAssetCategory(), "Assetcategoryid", "Assetcategpry");
                            objdetail.ddlSubcategory = new SelectList(GetAssetSubCategory(objdetail.Assetcategoryid.ToString()), "Assetsubcategoryid", "AssetSubcategpry");
                            // objdetail.ddlbranch = new SelectList(GetBranchandlocation(), "ddllocaid", "ddllocacode", selectedValue: dtfa.Rows[i]["loc_gid"]);
                            objdetail.Assetmfn = dtfa.Rows[i]["podetails_assmfn"].ToString();
                            objdetail.AssetSrlno = dtfa.Rows[i]["podetails_asssrlno"].ToString();
                            objdetail.pohgid = Convert.ToInt32(dtfa.Rows[i]["podetails_po_gid"].ToString());
                            objdetail.Grn_gid = Convert.ToInt32(dtfa.Rows[i]["Grn_gid"].ToString());
                            objdetail.DEPRATE = dtfa.Rows[i]["asset_dep_rate"].ToString();
                            objdetail.Description = dtfa.Rows[i]["podetails_desc"].ToString();
                            objgetasset.Add(objdetail);




                        }
                        objdetail.Itemlevellist = objgetasset;
                    }
                }
            }
            catch (Exception ex) { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            finally { GetfaConnection(); }
            return objdetail;
        }

        private string Getloname(string locationName)
        {
            string locaname = string.Empty;
            try
            {
                SqlCommand cmm = new SqlCommand("pr_ifams_trn_GetLocBranchnamenew", confa);
                cmm.CommandType = CommandType.StoredProcedure;
                cmm.Parameters.AddWithValue("@loc_gid", locationName);
                DataTable dtt = new DataTable();
                SqlDataAdapter daa = new SqlDataAdapter(cmm);
                daa.Fill(dtt);
                if (dtt.Rows.Count > 0)
                {
                    locaname = dtt.Rows[0]["branchname"].ToString();
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return locaname;
        }

        public List<capitalizationMaker> GetAssetSubCategory()
        {
            List<capitalizationMaker> objl = new List<capitalizationMaker>();
            capitalizationMaker objcat = new capitalizationMaker();
            try
            {
                GetfaConnection();
                SqlCommand cmdfanew = new SqlCommand("pr_ifams_GetAssetsubcategory", confa);
                cmdfanew.CommandType = CommandType.StoredProcedure;
                cmdfanew.Parameters.AddWithValue("@action", "1");
                SqlDataAdapter dafanew = new SqlDataAdapter(cmdfanew);
                DataTable dtfanew = new DataTable();
                dafanew.Fill(dtfanew);
                if (dtfanew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfanew.Rows.Count; i++)
                    {
                        objcat = new capitalizationMaker();
                        objcat.Assetsubcategoryid = Convert.ToInt32(dtfanew.Rows[i]["assetsubcategory_gid"].ToString());
                        objcat.AssetSubcategpry = dtfanew.Rows[i]["assetsubcategory_name"].ToString();
                        objl.Add(objcat);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { confa.Close(); }
            return objl;
        }


        public List<capitalizationMaker> GetAssetSubCategory(string Assetcategoryid)
        {
            List<capitalizationMaker> objl = new List<capitalizationMaker>();
            capitalizationMaker objcat = new capitalizationMaker();
            try
            {
                GetfaConnection();
                SqlCommand cmdfanew = new SqlCommand("pr_ifams_GetAssetsubcategory", confa);
                cmdfanew.CommandType = CommandType.StoredProcedure;
                cmdfanew.Parameters.AddWithValue("@action", "1");
                cmdfanew.Parameters.AddWithValue("@assetcat", Assetcategoryid.ToString());
                SqlDataAdapter dafanew = new SqlDataAdapter(cmdfanew);
                DataTable dtfanew = new DataTable();
                dafanew.Fill(dtfanew);
                if (dtfanew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfanew.Rows.Count; i++)
                    {
                        objcat = new capitalizationMaker();
                        objcat.Assetsubcategoryid = Convert.ToInt32(dtfanew.Rows[i]["assetsubcategory_gid"].ToString());
                        objcat.AssetSubcategpry = dtfanew.Rows[i]["assetsubcategory_name"].ToString();
                        objl.Add(objcat);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { confa.Close(); }
            return objl;
        }


        public List<capitalizationMaker> GetAssetCategory()
        {
            List<capitalizationMaker> obj1 = new List<capitalizationMaker>();
            capitalizationMaker objcat = new capitalizationMaker();
            try
            {
                GetfaConnection();
                //SqlCommand cmdfanew = new SqlCommand("pr_ifams_GetAssetsubcategory", confa);
                //cmdfanew.CommandType = CommandType.StoredProcedure;
                //cmdfanew.Parameters.AddWithValue("@action", "2");

                SqlCommand cmdfanew = new SqlCommand("pr_ifams_GetAssetsubcategory", confa);
                cmdfanew.CommandType = CommandType.StoredProcedure;

                cmdfanew.Parameters.Add("@action", SqlDbType.Text);
                cmdfanew.Parameters["@action"].Value = "category";
                SqlDataAdapter dafanew = new SqlDataAdapter(cmdfanew);
                DataTable dtfanew = new DataTable();
                dafanew.Fill(dtfanew);
                if (dtfanew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfanew.Rows.Count; i++)
                    {
                        objcat = new capitalizationMaker();
                        objcat.Assetcategoryid = Convert.ToInt32(dtfanew.Rows[i]["assetcategory_gid"].ToString());
                        objcat.Assetcategpry = dtfanew.Rows[i]["assetcategory_name"].ToString();
                        obj1.Add(objcat);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { confa.Close(); }
            return obj1;
        }

        public List<capitalizationMaker> GetBranchandlocation()
        {
            List<capitalizationMaker> objb = new List<capitalizationMaker>();
            capitalizationMaker objloc = new capitalizationMaker();
            try
            {
                SqlCommand cmdfasec = new SqlCommand("pr_ifams_trn_GetLocBranchname", confa);
                cmdfasec.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter daco = new SqlDataAdapter(cmdfasec);
                DataTable dtco = new DataTable();
                daco.Fill(dtco);
                if (dtco.Rows.Count > 0)
                {
                    for (int j = 0; j < dtco.Rows.Count; j++)
                    {
                        objloc = new capitalizationMaker();
                        objloc.ddllocacode = dtco.Rows[j]["branchname"].ToString();
                        objloc.ddllocaid = Convert.ToInt32(dtco.Rows[j]["Loc_gid"].ToString());
                        objb.Add(objloc);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objb;
        }


        public List<capitalizationMaker> Getpodetails(string id)
        {
            List<capitalizationMaker> objgetasset = new List<capitalizationMaker>();
            capitalizationMaker objdetail = new capitalizationMaker();
            try
            {
                GetfaConnection();
                string squery = "select podetails_invoiceno from fa_tmp_podetails where podetails_invoice_gid=" + id + "";
                cmdfa = new SqlCommand(squery, confa);
                cmdfa.CommandType = CommandType.Text;
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                    id = dtfa.Rows[0]["podetails_invoiceno"].ToString();
                cmdfa = new SqlCommand("pr_ifams_trn_Getmodpodetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invoiceno", id);
                cmdfa.Parameters.AddWithValue("@user", comuidfa.GetLoginUserGid().ToString());
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        objdetail = new capitalizationMaker();
                        objdetail.EcfNo = dtfa.Rows[i]["podetails_ecfno"].ToString();
                        objdetail.index = Convert.ToInt32(dtfa.Rows[i]["podetails_index"]);
                        objdetail.invoiceno = dtfa.Rows[i]["podetails_invoiceno"].ToString();
                        objdetail.ponumber = dtfa.Rows[i]["podetails_ponumber"].ToString();
                        objdetail.AssetCode = dtfa.Rows[i]["podetails_assetCode"].ToString();
                        objdetail.AssetName = dtfa.Rows[i]["podetails_assetname"].ToString();
                        objdetail.LocationCode = dtfa.Rows[i]["podetails_location"].ToString();
                        objdetail.locationName = dtfa.Rows[i]["podetails_locationname"].ToString();
                        objdetail.Grn_gid = Convert.ToInt32(dtfa.Rows[i]["Grn_gid"].ToString());
                        objdetail.GRNNo = dtfa.Rows[i]["podetails_grnno"].ToString();
                        objdetail.GRNDate = dtfa.Rows[i]["podetails_grndate"].ToString();
                        objdetail.glcode = dtfa.Rows[i]["podetails_assetglcode"].ToString();
                        objdetail.Amount = Convert.ToDecimal(dtfa.Rows[i]["podetails_amount"].ToString());
                        objdetail.Quantity = Convert.ToDecimal(dtfa.Rows[i]["podetails_poQty"].ToString());
                        objdetail.UOM = dtfa.Rows[i]["podetails_uom"].ToString();
                        objdetail.Discount = Convert.ToDecimal(dtfa.Rows[i]["podetails_discount"].ToString());
                        objdetail.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString());
                        objdetail.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax2"].ToString());
                        objdetail.othres = Convert.ToDecimal(dtfa.Rows[i]["podetails_others"].ToString());
                        objdetail.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["podetails_Totalamt"].ToString());
                        objdetail.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["podetails_poRevQty"].ToString());
                        objdetail.DCNo = dtfa.Rows[i]["podetails_DCno"].ToString();
                        objdetail.invoicegid = dtfa.Rows[i]["podetails_invoice_gid"].ToString();
                        objdetail.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["podetails_invoiceamt"].ToString());
                        objdetail.AssetCategory = dtfa.Rows[i]["Assetcategory"].ToString();
                        objdetail.AssetSubcategpry = dtfa.Rows[i]["AssetSubcategory"].ToString();
                        objdetail.shipmentType = dtfa.Rows[i]["ShipmentType"].ToString();
                        objdetail.ddlSubcategory = new SelectList(GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry", selectedValue: dtfa.Rows[i]["Subcategorygid"]);
                        // objdetail.ddlbranch = new SelectList(GetBranchandlocation(), "ddllocaid", "ddllocacode", selectedValue: dtfa.Rows[i]["loc_gid"]);
                        objdetail.AssetSrlno = dtfa.Rows[i]["podetails_asssrlno"].ToString();
                        objdetail.Assetmfn = dtfa.Rows[i]["podetails_assmfn"].ToString();
                        objdetail.pohgid = Convert.ToInt32(dtfa.Rows[i]["podetails_po_gid"].ToString());
                        objdetail.podetgid = Convert.ToInt32(dtfa.Rows[i]["podetails_podetgid"].ToString());
                        objdetail.Grn_detgid = Convert.ToInt32(dtfa.Rows[i]["podetials_Grn_detgid"].ToString());
                        objdetail.branchgid = Convert.ToInt32(dtfa.Rows[i]["loc_gid"].ToString());
                        objdetail.DEPRATE = dtfa.Rows[i]["asset_dep_rate"].ToString();
                        objdetail.Description = dtfa.Rows[i]["podetails_desc"].ToString();
                        objgetasset.Add(objdetail);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { GetfaConnection(); }
            return objgetasset;
        }
        public string SubmittoChecker(List<capitalizationMaker> inobj, string[] ids)
        {

            string Result = string.Empty;
            string Assetid = string.Empty;
            try
            {
                for (int i = 0; i < inobj.Count; i++)
                {
                    // Assetid = cmf.generateAssetid(inobj[i].LocationCode, inobj[i].AssetCode);
                    string[] array = inobj[i].LocationCode.ToString().Split('-');
                    string loc = array[1];
                    //Assetid = objCmnFunctions.generateAssetid(loc, inobj[i].AssetCode);
                    for (int i1 = 0; i1 < ids.Length; i1++)
                    {
                        string id1 = ids[i1];
                        if (id1 != string.Empty && id1 != "undefined " && inobj[i].Grn_detgid.ToString() == id1)
                        {
                            string[,] incode = new string[,]
                               {
                                    {"cap_Ecfid",inobj[0].EcfNo.ToString()},
                                    {"cap_Invoiceid",inobj[i].invoicegid},
                                    {"cap_Invoiceamt",inobj[i].invoiceamount.ToString()},
                                    {"cap_Ponumber",inobj[i].ponumber.ToString()},
                                    {"cap_AssetCode",inobj[i].AssetCode},
                                    {"cap_GrnNo",inobj[i].GRNNo.ToString()},
                                    //  {"cap_Loc_Code",loc.ToString()},
                                    {"cap_Loc_Code",inobj[i].locationName.ToString()},
                                    {"cap_Amount",inobj[i].Amount.ToString()},
                                    {"cap_qty",inobj[i].Quantity.ToString()},
                                    {"cap_Discount",inobj[i].Discount.ToString()},
                                    {"cap_Tax1",inobj[i].Tax1.ToString()},
                                    {"cap_Tax2",inobj[i].Tax2.ToString()},
                                    {"cap_others",inobj[i].othres.ToString()},
                                    {"cap_Total",inobj[i].TotalAmount.ToString()},
                                    {"cap_Descr",string.IsNullOrEmpty(inobj[i].Description)? string.Empty:inobj[i].Description},
                                    {"cap_glcode",string.IsNullOrEmpty(inobj[i].glcode.ToString())?"0":inobj[i].glcode.ToString()},
                                    {"cap_Status",ConfigurationManager.AppSettings["Waitingforapproval"].ToString()},
                                    {"cap_Rqty",inobj[i].Receivedqty.ToString()},
                                    {"cap_uom",inobj[i].UOM.ToString()},
                                    {"cap_Category",inobj[i].AssetCategory},
                                    {"cap_Subcategory",inobj[i].AssetSubcategpry},
                                    {"cap_ShipmentType",inobj[i].shipmentType},
                                    {"cap_AssetName",inobj[i].AssetName},
                                    {"cap_AssetLocation",inobj[i].LocationCode},
                                    {"cap_Grndate",inobj[i].GRNDate},
                                    {"cap_DcNo",inobj[i].DCNo},
                                    {"cap_assetsrlno",inobj[i].AssetSrlno},
                                    {"cap_po_gid",inobj[i].pohgid.ToString()},
                                    {"cap_grn_gid",inobj[i].Grn_gid.ToString()},
                                    {"cap_inwdetailgid",inobj[i].Grn_detgid.ToString()},
                                    {"cap_podetgid",inobj[i].podetgid.ToString()},
                                    {"cap_assmfname",inobj[i].Assetmfn.ToString()},
                                    {"cap_branch_gid",inobj[i].branchgid.ToString()},
                                    {"cap_isremoved","N"},
                                    {"cap_makerid",objCmnFunctions.GetLoginUserGid().ToString()},
                                    {"cap_podetails_desc",inobj[i].Description.ToString()}
                               };
                            string tblname = "fa_trn_tcapitalization";
                            Result = commfa.InsertCommon(incode, tblname);
                            //string[,] grncol = new string[,]
                            //{
                            //     {"grniwrdheader_capstatus",ConfigurationManager.AppSettings["Waitingforapproval"].ToString()}
                            //};
                            //string[,] grnwfrcol = new string[,]
                            //{
                            //    {"grninwrdheader_gid",inobj[0].Grn_gid.ToString()},
                            //    {"grninwardheader_poheader",inobj[0].pohgid.ToString()},
                            //    {"grninwrdheader_status",ConfigurationManager.AppSettings["EApproved"].ToString()}
                            //};
                            //string grnstatus = commfa.UpdateCommon(grncol, grnwfrcol, "fb_trn_tgrninwrdheader");
                            string[,] grncol1 = new string[,]
                            {
                                 {"grninwrddet_capstatus",ConfigurationManager.AppSettings["Waitingforapproval"].ToString()}
                            };
                            string[,] grnwfrcol1 = new string[,]
                            {
                                //{"grninwrddet_grninwrdhead_gid",inobj[0].Grn_gid.ToString()},
                                {"grninwrddet_grninwrdhead_gid",inobj[i].Grn_gid.ToString()},
                                {"grninwrddet_gid",inobj[i].Grn_detgid.ToString()},                               
                            };
                            string grnstatus1 = commfa.UpdateCommon(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                        }
                    }
                    // }

                    cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
                    GetfaConnection();
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@for", "qty");
                   // cmdfa.Parameters.AddWithValue("@invid", inobj[0].invoicegid);
                    cmdfa.Parameters.AddWithValue("@invid", inobj[i].invoicegid);
                    string MATCH = Convert.ToString(cmdfa.ExecuteScalar());
                    if (MATCH == "matched")
                    {
                        string[,] grncol = new string[,]
                    {
                         {"grniwrdheader_capstatus",ConfigurationManager.AppSettings["Waitingforapproval"].ToString()}
                    };
                        string[,] grnwfrcol = new string[,]
                    {
                        //{"grninwrdheader_gid",inobj[0].Grn_gid.ToString()},
                        //{"grninwardheader_poheader",inobj[0].pohgid.ToString()},
                        {"grninwrdheader_gid",inobj[i].Grn_gid.ToString()},
                        {"grninwardheader_poheader",inobj[i].pohgid.ToString()},
                        {"grninwrdheader_status",ConfigurationManager.AppSettings["EApproved"].ToString()}
                    };
                        string grnstatus = commfa.UpdateCommon(grncol, grnwfrcol, "fb_trn_tgrninwrdheader");
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }

        public string FinalApprovalstage(int inv, string status, string remarks)
        {
            string Result = string.Empty, dep = string.Empty, asset = string.Empty, cbf = string.Empty;
            DataTable dtfa = new DataTable();
            int assetid = 0;
            int grngid = 0;
            int pogid = 0;
            string success = "";
            string group_id = "";
            List<int> a = new List<int>();
            List<int> a1 = new List<int>();
            try
            {

                GetfaConnection();
                //  tran = confa.BeginTransaction("Transaction1");
                cmdfa = new SqlCommand("pr_ifams_getcapfulldetils", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invgid", inv);
                cmdfa.Parameters.AddWithValue("@Status", ConfigurationManager.AppSettings["Waitingforapproval"].ToString());
                // cmdfa.Transaction = tran;
                dafa = new SqlDataAdapter(cmdfa);
                // dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    if (status == "APPROVED")
                    {
                        //cmdfa = new SqlCommand("pr_ifams_trn_capcheckerinsert", confa);
                        //cmdfa.CommandType = CommandType.StoredProcedure;
                        //cmdfa.Parameters.AddWithValue("@inv", inv);
                        //cmdfa.Parameters.AddWithValue("@qry", "total");
                        //dafa = new SqlDataAdapter(cmdfa);
                        //dtfa = new DataTable();
                        //dafa.Fill(dtfa);
                        //cmdfa = new SqlCommand("pr_ifams_getcapfulldetils", confa);
                        //cmdfa.CommandType = CommandType.StoredProcedure;
                        //cmdfa.Parameters.AddWithValue("@invgid", inv);
                        //cmdfa.Parameters.AddWithValue("@Status", ConfigurationManager.AppSettings["Waitingforapproval"].ToString());
                        //dafa = new SqlDataAdapter(cmdfa);
                        //// dtfa = new DataTable();
                        //dafa.Fill(dtfa);
                        cmdfa = new SqlCommand("pr_ifams_trn_cwip", confa);
                        cmdfa.CommandType = CommandType.StoredProcedure;
                        cmdfa.Parameters.AddWithValue("@action", "gettranuploadgid");
                        // cmdfa.Transaction = tran;
                        string tran_upload_gid = Convert.ToString(cmdfa.ExecuteScalar());
                        if (dtfa.Rows.Count > 0)
                        {

                            for (int i = 0; i < dtfa.Rows.Count; i++)
                            {
                                //var lowNums = from n in a where n == i select n;
                                //int formID = Convert.ToInt32(lowNums);
                                //int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 

                                // var lowNums =   from n in a  where n != i   select n;
                                //if(i!=lowNums)
                                string id1 = dtfa.Rows[i]["cap_inwdetailgid"].ToString();
                                string GRNDate = dtfa.Rows[i]["cap_Grndate"].ToString();
                                string LocationCode = dtfa.Rows[i]["cap_Loc_Code"].ToString();
                                string TotalAmount = dtfa.Rows[i]["cap_Total"].ToString();
                                string AssetCode = dtfa.Rows[i]["cap_AssetCode"].ToString();

                                if (i == 0)
                                {
                                    for (int j = 0; j < dtfa.Rows.Count; j++)
                                    {
                                        if (GRNDate == dtfa.Rows[j]["cap_Grndate"].ToString() && LocationCode == dtfa.Rows[j]["cap_Loc_Code"].ToString()
                                            && TotalAmount == dtfa.Rows[j]["cap_Total"].ToString() && AssetCode == dtfa.Rows[j]["cap_AssetCode"].ToString())
                                        {
                                            success = "Success";
                                            a.Add(j);
                                            a1.Add(j);

                                        }
                                    }
                                    if (success == "Success")
                                    {
                                        group_id = comuidfa.GetSequenceNoFam("2").ToString();
                                        for (int k = 0; k < dtfa.Rows.Count; k++)
                                        {
                                            if (a1.Contains(k))
                                            {

                                                string GL = string.Empty;
                                                //Muthu 07-12-2016
                                                DataTable dta = new DataTable();
                                                cmdfa = new SqlCommand("pr_ifams_trn_SplitMaker", confa);
                                                cmdfa.CommandType = CommandType.StoredProcedure;
                                                cmdfa.Parameters.AddWithValue("@assetcodes", dtfa.Rows[k]["cap_AssetCode"]);
                                                cmdfa.Parameters.AddWithValue("@action", "SelectGL");
                                                //cmdfa.Transaction = tran;
                                                dafa = new SqlDataAdapter(cmdfa);
                                                dafa.Fill(dta);
                                                for (int j = 0; j < dta.Rows.Count; j++)
                                                {
                                                    GL = dta.Rows[0]["asset_glcode"].ToString();
                                                }

                                                string yn = "Y";
                                                /*if (Convert.ToDecimal(dtfa.Rows[k]["cap_Amount"]) > 5000 && GL != "121010002")
                                                {
                                                    yn = "Y";
                                                }
                                                else
                                                {
                                                    yn = "N";
                                                }*/
                                                string capdate = "";
                                                string LeaseStartDate = "";
                                                string LeaseEndDate = "";
                                                if (dtfa.Rows[k]["asset_dep_type"].ToString() == "LPM")
                                                {
                                                    LeaseStartDate = dtfa.Rows[k]["branch_lease_start_date"].ToString();
                                                    LeaseEndDate = dtfa.Rows[k]["branch_lease_end_date"].ToString();

                                                    if (Convert.ToDateTime(dtfa.Rows[k]["cap_Grndate"].ToString()) < Convert.ToDateTime(dtfa.Rows[k]["branch_lease_start_date"].ToString()))
                                                    {
                                                        capdate = objCmnFunctions.convertoDateTimeString(LeaseStartDate);
                                                    }
                                                    else
                                                    {
                                                        capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToDateTime(dtfa.Rows[k]["cap_Grndate"].ToString()) < Convert.ToDateTime(dtfa.Rows[k]["branch_start_date"].ToString()))
                                                    {
                                                        capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["branch_start_date"].ToString());
                                                    }
                                                    else
                                                    {
                                                        capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString());
                                                    }
                                                }

                                                string assetsetailno = string.Empty, verifiable = string.Empty;
                                                DataTable qty = new DataTable();
                                                cmdfa = new SqlCommand("pr_ifams_trn_cwip", confa);
                                                cmdfa.CommandType = CommandType.StoredProcedure;
                                                cmdfa.Parameters.AddWithValue("@Code", dtfa.Rows[k]["cap_AssetCode"]);
                                                cmdfa.Parameters.AddWithValue("@action", "Asset");
                                                //cmdfa.Transaction = tran;
                                                dafa = new SqlDataAdapter(cmdfa);
                                                dafa.Fill(qty);
                                                if (qty.Rows.Count > 0)
                                                {
                                                    assetsetailno = qty.Rows[0]["aset_serial_no"].ToString();
                                                    verifiable = qty.Rows[0]["asset_verifiable"].ToString();
                                                }
                                                assetsetailno = qty.Rows[0]["aset_serial_no"].ToString();
                                                verifiable = qty.Rows[0]["asset_verifiable"].ToString();
                                                if (assetsetailno == "N" && verifiable == "Y")
                                                {
                                                    for (int t = 0; t < Convert.ToInt64(dtfa.Rows[k]["cap_qty"]); t++)
                                                    {
                                                        string ASSET_ID = comuidfa.generateAssetid(dtfa.Rows[k]["cap_Loc_Code"].ToString(), dtfa.Rows[k]["cap_AssetCode"].ToString());
                                                        decimal Amount = Math.Round((Convert.ToDecimal(dtfa.Rows[k]["cap_Amount"]) / Convert.ToInt64(dtfa.Rows[k]["cap_qty"])), 2);
                                                        string[,] incodeinst = new string[,]
                                            {
                                                {"assetdetails_assetdet_id",ASSET_ID}                    
                                                ,{"assetdetails_qty","1"} 
                                                ,{"assetdetails_asset_serialno", dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                                ,{"assetdetails_asset_code",dtfa.Rows[k]["asset_gid"].ToString()}
                                                //,{"assetdetails_asset_value",dtfa.Rows[k]["cap_Amount"].ToString()}
                                                ,{"assetdetails_asset_value",Amount.ToString()}
                                               // ,{"assetdetails_asset_description",dtfa.Rows[k]["cap_Subcategory"].ToString()}
                                               ,{"assetdetails_asset_description",dtfa.Rows[k]["cap_podetails_desc"].ToString()}
                                                ,{"assetdetails_cap_date",capdate}
                                                ,{"assetdetails_po_number",dtfa.Rows[k]["cap_po_gid"].ToString()}
                                                ,{"assetdetails_tfr_status","N"}   
                                                ,{"assetdetails_active_status","Y"}
                                                ,{"assetdetails_sale_status","N"}    
                                                ,{"assetdetails_not_5kcase",yn}
                                                ,{"assetdetails_trf_value",dtfa.Rows[k]["cap_Total"].ToString()} 
                                                ,{"assetdetails_asset_owner","F"}
                                                ,{"assetdetails_trn_date",capdate}
                                                ,{"assetdetails_impair_asset","N"}
                                                ,{"assetdetails_assetcode_changestatus","N"}
                                                ,{"assetdetails_isremoved","N"}
                                                ,{"assetdetails_dep_rate","0"}                                    
                                                ,{"assetdetails_branch_gid",dtfa.Rows[k]["branch_gid"].ToString()}
                                                ,{"assetdetails_addate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                                ,{"assetdetails_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                                ,{"assetdetails_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}    
                                                ,{"assetdetails_takenby","14"}     
                                                ,{"assetdetails_assetid_source","1"}
                                                ,{"assetdetails_invoice_gid",inv.ToString()}
                                                ,{"assetdetails_ecf_gid",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                                ,{"assetdetails_entity_gid","1"}
                                                ,{"assetdetails_asset_groupid",group_id.ToString()}
                                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                                ,{"assetdetails_assetcode_changeid",string.Empty}
                                                ,{"assetdetails_barcode",string.Empty}
                                                ,{"assetdetails_capnew_date",string.Empty}
                                                ,{"assetdetails_capold_date",string.Empty}
                                                ,{"assetdetails_cbf_gid",dtfa.Rows[k]["cbfheader_gid"].ToString()}
                                                ,{"assetdetails_cbfnum",dtfa.Rows[k]["cbfheader_cbfno"].ToString()}
                                                ,{"assetdetails_dep_onhold","N"}
                                                ,{"assetdetails_ecfnum",dtfa.Rows[k]["ecf_no"].ToString()}
                                                ,{"assetdetails_imp_date",string.Empty}
                                                ,{"assetdetails_inwdetailgid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()}
                                                ,{"assetdetails_inwheadergid",dtfa.Rows[k]["cap_grn_gid"].ToString()}
                                           //   ,{"assetdetails_lease_enddate",LeaseEndDate}
                                             //   ,{"assetdetails_lease_startdate",LeaseStartDate}
                                                ,{"assetdetails_lease_enddate",string.IsNullOrEmpty(LeaseEndDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseEndDate.ToString())}
                                                ,{"assetdetails_lease_startdate",string.IsNullOrEmpty(LeaseStartDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseStartDate.ToString())}
                                                ,{"assetdetails_physical_assetid","0"}
                                                ,{"assetdetails_physical_idrelease",string.Empty}
                                                ,{"assetdetails_ponum",dtfa.Rows[k]["cap_Ponumber"].ToString()}
                                                ,{"assetdetails_recon_reference",string.Empty}
                                                ,{"assetdetails_reduced_value","0"}
                                                ,{"assetdetails_sale_date",string.Empty}
                                                ,{"assetdetails_sale_id",string.Empty}
                                                ,{"assetdetails_spoke_asset","N"}
                                                ,{"assetdetails_status",string.Empty}
                                                ,{"assetdetails_tfr_date",string.Empty}
                                                ,{"assetdetails_tfr_id",string.Empty}
                                                ,{"assetdetails_trf_from",string.Empty}
                                                ,{"assetdetails_trf_reason",string.Empty}
                                                ,{"assetdetails_trf_to",string.Empty}
                                                ,{"assetdetails_update_by",string.Empty}
                                                ,{"assetdetails_update_date",string.Empty}
                                                ,{"assetdetails_upld_ref",string.Empty}
                                                ,{"assetdetails_upld_status","N"}
                                                ,{"assetdetails_wrt_date",string.Empty}
                                                ,{"assetdetails_wrt_id",string.Empty}
                                                ,{"assetdetails_wrt_status","N"}
                                                ,{"assetdetails_vendorname",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                                ,{"assetdetails_inwarddate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                              
                                            };
                                                        Result = commfa.InsertCommon(incodeinst, "fa_trn_tassetdetails");
                                                        //  Result = commfa.InsertCommon_FA(incodeinst, "fa_trn_tassetdetails");

                                                        string[,] incode = new string[,]
                                            {
                                            {"h_asst_groupid_no",group_id }
                                            ,{"h_asst_code",asset}
                                            ,{"h_asst_owner","F"}
                                            ,{"h_asst_desc",dtfa.Rows[k]["cap_Subcategory"].ToString()}                   
                                            ,{"h_emp_code",comuidfa.GetLoginUserGid().ToString() }   
                                            ,{"h_capt_date",objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString())}
                                            //,{"h_purc_cost",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_purc_cost",Amount.ToString()}
                                            ,{"h_sales_tax_amt",dtfa.Rows[k]["cap_Tax1"].ToString()}
                                            //,{"h_org_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_org_capt_amt",Amount.ToString()}
                                            //,{"h_cur_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_cur_capt_amt",Amount.ToString()}
                                            ,{"h_org_qty_old","1"}
                                            ,{"h_org_qty","1"}
                                            ,{"h_cur_qty","1"}
                                            ,{"h_model_no",""}
                                            ,{"h_supplier_code",dtfa.Rows[k]["supplierheader_suppliercode"].ToString()}
                                             ,{"h_supplier_name",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                            ,{"h_serial_no",dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                            ,{"h_ecfno",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                            ,{"h_status","Y"}
                                            ,{"h_po_mast_po_no",dtfa.Rows[k]["cap_po_gid"].ToString()}                   
                                            ,{"h_depr_glcode",dtfa.Rows[k]["cap_glcode"].ToString()}
                                            ,{"h_narration",""}                    
                                            ,{"h_isremoved","N"}
                                            ,{"h_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                            ,{"h_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                            };
                                                        // GetfaConnection();
                                                        Result = commfa.InsertCommon(incode, "fa_trn_th");
                                                        // Result = commfa.InsertCommon_FA(incode, "fa_trn_th");
                                                        cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                                                        cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_ID;
                                                        //  cmdfa.Transaction = tran;
                                                        string ASSET_GID = Convert.ToString(cmdfa.ExecuteScalar());
                                                        cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                                                        cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_GID;
                                                        //cmdfa.Transaction = tran;
                                                        dafa = new SqlDataAdapter(cmdfa);
                                                        string glCode = "";
                                                        string branch = "";
                                                        string cat = "";
                                                        string subcat = "";
                                                        string asset_id = "";
                                                        string BS = "";
                                                        string CC = "";
                                                        string ratio = "";
                                                        DataTable UPLOADDATATBL = new DataTable();
                                                        dafa.Fill(UPLOADDATATBL);
                                                        if (UPLOADDATATBL.Rows.Count > 0)
                                                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                            {
                                                                glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                                                branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                                                cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                                                if (cat.Length > 16)
                                                                    cat = cat.Substring(0, 16);
                                                                subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                                                asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                                            }

                                                        cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[k]["cap_inwdetailgid"].ToString()) ? "0" : dtfa.Rows[k]["cap_inwdetailgid"].ToString());
                                                        cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnbscc";
                                                        // cmdfa.Transaction = tran;
                                                        dafa = new SqlDataAdapter(cmdfa);
                                                        UPLOADDATATBL = new DataTable();
                                                        dafa.Fill(UPLOADDATATBL);
                                                        if (UPLOADDATATBL.Rows.Count > 0)
                                                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                            {
                                                                BS = UPLOADDATATBL.Rows[0]["bs_code"].ToString();
                                                                CC = UPLOADDATATBL.Rows[0]["cc_code"].ToString();
                                                                ratio = "100";
                                                            }
                                                        DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(dtfa.Rows[k]["cap_makerid"].ToString()));
                                                        string[,] glCoulmsD = new string[,]
                                            {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[k]["cap_glcode"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                //{"tran_amount",dtfa.Rows[k]["cap_Total"].ToString()},
                                                {"tran_amount",Amount.ToString()},
                                                {"tran_acc_mode","D".ToString()},
                                                {"tran_mult","-1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                              //  {"tran_ref_gid",""}, ASSET_GID
                                                 {"tran_ref_gid",ASSET_GID.ToString()},
                                              //  {"tran_upload_gid",tran_upload_gid.ToString()}, 
                                                {"tran_upload_gid","0"},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                            };
                                                        string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                                                        // string insertforglD = objCommonIUD.InsertCommon_FA(glCoulmsD, "fa_trn_ttran");
                                                        string[,] glCoulmsC = new string[,]
                                        {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[i]["assetcategory_assset_clearing"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                //{"tran_amount",dtfa.Rows[i]["cap_Total"].ToString()},
                                                {"tran_amount",Amount.ToString()},
                                                {"tran_acc_mode","C".ToString()},
                                                {"tran_mult","1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                //{"tran_upload_gid",tran_upload_gid.ToString()}, 
                                                {"tran_upload_gid","0"},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                        };

                                                        string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                                                        //   string insertforglC = objCommonIUD.InsertCommon_FA(glCoulmsC, "fa_trn_ttran");
                                                        string[,] cval = new string[,]
                                        {
                                            {"c_br_code","2"},
                                            {"c_ecf_no",dtfa.Rows[k]["cap_Ecfid"].ToString()},
                                            {"c_in_no",inv.ToString()},
                                            {"c_inv_srlno",dtfa.Rows[k]["cap_Invoiceid"].ToString()},
                                            {"c_ASST_ID_NO",group_id},
                                            {"c_COST_CODE",CC},
                                            {"c_BUSS_CODE",BS},
                                            {"c_RATIO",ratio},
                                            {"c_CCBS",CC + BS},
                                            // {"c_loc",dtfa.Rows[i]["location_gid"].ToString()},
                                            {"c_isremoved","N"},
                                            {"c_insert_by",comuidfa.GetLoginUserGid().ToString()},
                                            {"c_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                        };

                                                        cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(branch) ? "0" : branch);
                                                        cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbranchlegacy";
                                                        //    cmdfa.Transaction = tran;
                                                        string branchlegacycode = Convert.ToString(cmdfa.ExecuteScalar());

                                                        string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",ASSET_ID.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",CC + BS.ToString()},
                                {"assetlocation_location_fc",BS.ToString()},
                                {"assetlocation_location_cc",CC.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}
                            };

                                                        string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");
                                                        //   string insertlocationcmn = objCommonIUD.InsertCommon_FA(codeloc, "fa_trn_tassetlocation");


                                                        Result = commfa.InsertCommon(cval, "fa_trn_tc");

                                                    } // Spliting for Close

                                                    //  Result = commfa.InsertCommon_FA(cval, "fa_trn_tc");
                                                    HttpContext.Current.Session["grngid"] = Convert.ToInt32(dtfa.Rows[0]["cap_grn_gid"].ToString());
                                                    HttpContext.Current.Session["pogid"] = Convert.ToInt32(dtfa.Rows[0]["cap_po_gid"].ToString());
                                                    string aa = dtfa.Rows[k]["cap_grn_gid"].ToString();
                                                    string[,] grncol1 = new string[,]
                                        {
                                            {"grninwrddet_capstatus",ConfigurationManager.AppSettings["EApproved"].ToString()}
                                        };
                                                    string[,] grnwfrcol1 = new string[,]
                                        {
                                        //{"grninwrddet_grninwrdhead_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},
                                       
                                        {"grninwrddet_grninwrdhead_gid",dtfa.Rows[k]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_gid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()},                               
                                        };
                                                    string grnstatus1 = commfa.UpdateCommon(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                    // string grnstatus1 = commfa.UpdateCommon_FA(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                }//Asset spiting if close


                                                else /* Asset spliting else*/
                                                {

                                                    string ASSET_ID = comuidfa.generateAssetid(dtfa.Rows[k]["cap_Loc_Code"].ToString(), dtfa.Rows[k]["cap_AssetCode"].ToString());
                                                    // decimal Amount = Math.Round((Convert.ToDecimal(dtfa.Rows[k]["cap_Amount"]) / 2), 2);
                                                    string[,] incodeinst = new string[,]
                                            {
                                                {"assetdetails_assetdet_id",ASSET_ID}                    
                                                ,{"assetdetails_qty","1"} 
                                                ,{"assetdetails_asset_serialno", dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                                ,{"assetdetails_asset_code",dtfa.Rows[k]["asset_gid"].ToString()}
                                                ,{"assetdetails_asset_value",dtfa.Rows[k]["cap_Amount"].ToString()}
                                                //,{"assetdetails_asset_value",Amount.ToString()}
                                                //,{"assetdetails_asset_description",dtfa.Rows[k]["cap_Subcategory"].ToString()}
                                                ,{"assetdetails_asset_description",dtfa.Rows[k]["cap_podetails_desc"].ToString()}
                                                ,{"assetdetails_cap_date",capdate}
                                                ,{"assetdetails_po_number",dtfa.Rows[k]["cap_po_gid"].ToString()}
                                                ,{"assetdetails_tfr_status","N"}   
                                                ,{"assetdetails_active_status","Y"}
                                                ,{"assetdetails_sale_status","N"}    
                                                ,{"assetdetails_not_5kcase",yn}
                                                ,{"assetdetails_trf_value",dtfa.Rows[k]["cap_Total"].ToString()} 
                                                ,{"assetdetails_asset_owner","F"}
                                                ,{"assetdetails_trn_date",capdate}
                                                ,{"assetdetails_impair_asset","N"}
                                                ,{"assetdetails_assetcode_changestatus","N"}
                                                ,{"assetdetails_isremoved","N"}
                                                ,{"assetdetails_dep_rate","0"}                                    
                                                ,{"assetdetails_branch_gid",dtfa.Rows[k]["branch_gid"].ToString()}
                                                ,{"assetdetails_addate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                                ,{"assetdetails_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                                ,{"assetdetails_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}    
                                                ,{"assetdetails_takenby","14"}     
                                                ,{"assetdetails_assetid_source","1"}
                                                ,{"assetdetails_invoice_gid",inv.ToString()}
                                                ,{"assetdetails_ecf_gid",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                                ,{"assetdetails_entity_gid","1"}
                                                ,{"assetdetails_asset_groupid",group_id.ToString()}
                                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                                ,{"assetdetails_assetcode_changeid",string.Empty}
                                                ,{"assetdetails_barcode",string.Empty}
                                                ,{"assetdetails_capnew_date",string.Empty}
                                                ,{"assetdetails_capold_date",string.Empty}
                                                ,{"assetdetails_cbf_gid",dtfa.Rows[k]["cbfheader_gid"].ToString()}
                                                ,{"assetdetails_cbfnum",dtfa.Rows[k]["cbfheader_cbfno"].ToString()}
                                                ,{"assetdetails_dep_onhold","N"}
                                                ,{"assetdetails_ecfnum",dtfa.Rows[k]["ecf_no"].ToString()}
                                                ,{"assetdetails_imp_date",string.Empty}
                                                ,{"assetdetails_inwdetailgid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()}
                                                ,{"assetdetails_inwheadergid",dtfa.Rows[k]["cap_grn_gid"].ToString()}
                                           //   ,{"assetdetails_lease_enddate",LeaseEndDate}
                                             //   ,{"assetdetails_lease_startdate",LeaseStartDate}
                                                ,{"assetdetails_lease_enddate",string.IsNullOrEmpty(LeaseEndDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseEndDate.ToString())}
                                                ,{"assetdetails_lease_startdate",string.IsNullOrEmpty(LeaseStartDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseStartDate.ToString())}
                                                ,{"assetdetails_physical_assetid","0"}
                                                ,{"assetdetails_physical_idrelease",string.Empty}
                                                ,{"assetdetails_ponum",dtfa.Rows[k]["cap_Ponumber"].ToString()}
                                                ,{"assetdetails_recon_reference",string.Empty}
                                                ,{"assetdetails_reduced_value","0"}
                                                ,{"assetdetails_sale_date",string.Empty}
                                                ,{"assetdetails_sale_id",string.Empty}
                                                ,{"assetdetails_spoke_asset","N"}
                                                ,{"assetdetails_status",string.Empty}
                                                ,{"assetdetails_tfr_date",string.Empty}
                                                ,{"assetdetails_tfr_id",string.Empty}
                                                ,{"assetdetails_trf_from",string.Empty}
                                                ,{"assetdetails_trf_reason",string.Empty}
                                                ,{"assetdetails_trf_to",string.Empty}
                                                ,{"assetdetails_update_by",string.Empty}
                                                ,{"assetdetails_update_date",string.Empty}
                                                ,{"assetdetails_upld_ref",string.Empty}
                                                ,{"assetdetails_upld_status","N"}
                                                ,{"assetdetails_wrt_date",string.Empty}
                                                ,{"assetdetails_wrt_id",string.Empty}
                                                ,{"assetdetails_wrt_status","N"}
                                                ,{"assetdetails_vendorname",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                                ,{"assetdetails_inwarddate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                              
                                            };
                                                    Result = commfa.InsertCommon(incodeinst, "fa_trn_tassetdetails");
                                                    //  Result = commfa.InsertCommon_FA(incodeinst, "fa_trn_tassetdetails");

                                                    string[,] incode = new string[,]
                                            {
                                            {"h_asst_groupid_no",group_id }
                                            ,{"h_asst_code",asset}
                                            ,{"h_asst_owner","F"}
                                            ,{"h_asst_desc",dtfa.Rows[k]["cap_Subcategory"].ToString()}                   
                                            ,{"h_emp_code",comuidfa.GetLoginUserGid().ToString() }   
                                            ,{"h_capt_date",objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString())}
                                            ,{"h_purc_cost",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_sales_tax_amt",dtfa.Rows[k]["cap_Tax1"].ToString()}
                                            ,{"h_org_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_cur_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_org_qty_old","1"}
                                            ,{"h_org_qty","1"}
                                            ,{"h_cur_qty","1"}
                                            ,{"h_model_no",""}
                                            ,{"h_supplier_code",dtfa.Rows[k]["supplierheader_suppliercode"].ToString()}
                                             ,{"h_supplier_name",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                            ,{"h_serial_no",dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                            ,{"h_ecfno",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                            ,{"h_status","Y"}
                                            ,{"h_po_mast_po_no",dtfa.Rows[k]["cap_po_gid"].ToString()}                   
                                            ,{"h_depr_glcode",dtfa.Rows[k]["cap_glcode"].ToString()}
                                            ,{"h_narration",""}                    
                                            ,{"h_isremoved","N"}
                                            ,{"h_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                            ,{"h_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                            };
                                                    // GetfaConnection();
                                                    Result = commfa.InsertCommon(incode, "fa_trn_th");
                                                    // Result = commfa.InsertCommon_FA(incode, "fa_trn_th");
                                                    cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                                                    cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_ID;
                                                    //  cmdfa.Transaction = tran;
                                                    string ASSET_GID = Convert.ToString(cmdfa.ExecuteScalar());
                                                    cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                                                    cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_GID;
                                                    //cmdfa.Transaction = tran;
                                                    dafa = new SqlDataAdapter(cmdfa);
                                                    string glCode = "";
                                                    string branch = "";
                                                    string cat = "";
                                                    string subcat = "";
                                                    string asset_id = "";
                                                    string BS = "";
                                                    string CC = "";
                                                    string ratio = "";
                                                    DataTable UPLOADDATATBL = new DataTable();
                                                    dafa.Fill(UPLOADDATATBL);
                                                    if (UPLOADDATATBL.Rows.Count > 0)
                                                        foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                        {
                                                            glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                                            branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                                            cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                                            if (cat.Length > 16)
                                                                cat = cat.Substring(0, 16);
                                                            subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                                            asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                                        }

                                                    cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[k]["cap_inwdetailgid"].ToString()) ? "0" : dtfa.Rows[k]["cap_inwdetailgid"].ToString());
                                                    cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnbscc";
                                                    // cmdfa.Transaction = tran;
                                                    dafa = new SqlDataAdapter(cmdfa);
                                                    UPLOADDATATBL = new DataTable();
                                                    dafa.Fill(UPLOADDATATBL);
                                                    if (UPLOADDATATBL.Rows.Count > 0)
                                                        foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                        {
                                                            BS = UPLOADDATATBL.Rows[0]["bs_code"].ToString();
                                                            CC = UPLOADDATATBL.Rows[0]["cc_code"].ToString();
                                                            ratio = "100";
                                                        }
                                                    DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(dtfa.Rows[k]["cap_makerid"].ToString()));
                                                    string[,] glCoulmsD = new string[,]
                                            {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[k]["cap_glcode"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                {"tran_amount",dtfa.Rows[k]["cap_Total"].ToString()},
                                                {"tran_acc_mode","D".ToString()},
                                                {"tran_mult","-1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                              //  {"tran_ref_gid",""}, ASSET_GID
                                                 {"tran_ref_gid",ASSET_GID.ToString()},
                                              //  {"tran_upload_gid",tran_upload_gid.ToString()}, 
                                                {"tran_upload_gid","0"},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                            };
                                                    string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                                                    // string insertforglD = objCommonIUD.InsertCommon_FA(glCoulmsD, "fa_trn_ttran");
                                                    string[,] glCoulmsC = new string[,]
                                        {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[i]["assetcategory_assset_clearing"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                {"tran_amount",dtfa.Rows[i]["cap_Total"].ToString()},
                                                {"tran_acc_mode","C".ToString()},
                                                {"tran_mult","1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                //{"tran_upload_gid",tran_upload_gid.ToString()}, 
                                                {"tran_upload_gid","0"},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                        };

                                                    string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                                                    //   string insertforglC = objCommonIUD.InsertCommon_FA(glCoulmsC, "fa_trn_ttran");
                                                    string[,] cval = new string[,]
                                        {
                                            {"c_br_code","2"},
                                            {"c_ecf_no",dtfa.Rows[k]["cap_Ecfid"].ToString()},
                                            {"c_in_no",inv.ToString()},
                                            {"c_inv_srlno",dtfa.Rows[k]["cap_Invoiceid"].ToString()},
                                            {"c_ASST_ID_NO",group_id},
                                            {"c_COST_CODE",CC},
                                            {"c_BUSS_CODE",BS},
                                            {"c_RATIO",ratio},
                                            {"c_CCBS",CC + BS},
                                            // {"c_loc",dtfa.Rows[i]["location_gid"].ToString()},
                                            {"c_isremoved","N"},
                                            {"c_insert_by",comuidfa.GetLoginUserGid().ToString()},
                                            {"c_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                        };

                                                    cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(branch) ? "0" : branch);
                                                    cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbranchlegacy";
                                                    //    cmdfa.Transaction = tran;
                                                    string branchlegacycode = Convert.ToString(cmdfa.ExecuteScalar());

                                                    string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",ASSET_ID.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",CC + BS.ToString()},
                                {"assetlocation_location_fc",BS.ToString()},
                                {"assetlocation_location_cc",CC.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}
                            };

                                                    string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");
                                                    //   string insertlocationcmn = objCommonIUD.InsertCommon_FA(codeloc, "fa_trn_tassetlocation");


                                                    Result = commfa.InsertCommon(cval, "fa_trn_tc");



                                                    //  Result = commfa.InsertCommon_FA(cval, "fa_trn_tc");
                                                    HttpContext.Current.Session["grngid"] = Convert.ToInt32(dtfa.Rows[0]["cap_grn_gid"].ToString());
                                                    HttpContext.Current.Session["pogid"] = Convert.ToInt32(dtfa.Rows[0]["cap_po_gid"].ToString());
                                                    string aa = dtfa.Rows[k]["cap_grn_gid"].ToString();
                                                    string[,] grncol1 = new string[,]
                                        {
                                            {"grninwrddet_capstatus",ConfigurationManager.AppSettings["EApproved"].ToString()}
                                        };
                                                    string[,] grnwfrcol1 = new string[,]
                                        {
                                        //{"grninwrddet_grninwrdhead_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},
                                       
                                        {"grninwrddet_grninwrdhead_gid",dtfa.Rows[k]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_gid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()},                               
                                        };
                                                    string grnstatus1 = commfa.UpdateCommon(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                    // string grnstatus1 = commfa.UpdateCommon_FA(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");


                                                } /* Asset spliting else end*/

                                            }
                                            success = "";
                                        } a1.Clear();

                                    }
                                }
                                else// 2nd group id generation grid
                                {

                                    //List<string> range = a.GetRange(1, 2);
                                    //foreach (string river in range)
                                    //{
                                    //    Console.WriteLine(river);
                                    //}
                                    //IEnumerable<int> result = from n in Enumerable.Range(0, i)  select value;
                                    //foreach (int value in result)
                                    //{
                                    //    Console.WriteLine(value);
                                    //}

                                    //int lowNums = (from n in a where n == i select n).FirstOrDefault();
                                    //IEnumerable<int> ints = lowNums.Select(int.Parse);
                                    //IEnumerable<int> Values = (from n in a where n == i select n).Single();
                                    // int formID = Convert.ToInt32(lowNums);
                                    if (!a.Contains(i))
                                    {
                                        for (int j = 0; j < dtfa.Rows.Count; j++)
                                        {
                                            if (GRNDate == dtfa.Rows[j]["cap_Grndate"].ToString() && LocationCode == dtfa.Rows[j]["cap_Loc_Code"].ToString()
                                                && TotalAmount == dtfa.Rows[j]["cap_Total"].ToString() && AssetCode == dtfa.Rows[j]["cap_AssetCode"].ToString())
                                            {
                                                success = "Success";
                                                a.Add(j);
                                                a1.Add(j);
                                            }
                                        }
                                        if (success == "Success")
                                        {
                                            group_id = comuidfa.GetSequenceNoFam("2").ToString();

                                            for (int k = 0; k < dtfa.Rows.Count; k++)
                                            {
                                                if (a1.Contains(k))
                                                {

                                                    string GL = string.Empty;
                                                    //Muthu 07-12-2016
                                                    DataTable dta = new DataTable();
                                                    cmdfa = new SqlCommand("pr_ifams_trn_SplitMaker", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.AddWithValue("@assetcodes", dtfa.Rows[k]["cap_AssetCode"]);
                                                    cmdfa.Parameters.AddWithValue("@action", "SelectGL");
                                                    // cmdfa.Transaction = tran;
                                                    dafa = new SqlDataAdapter(cmdfa);
                                                    dafa.Fill(dta);
                                                    for (int j = 0; j < dta.Rows.Count; j++)
                                                    {
                                                        GL = dta.Rows[0]["asset_glcode"].ToString();
                                                    }

                                                    string yn = "Y";
                                                /*    if (Convert.ToDecimal(dtfa.Rows[k]["cap_Amount"]) > 5000 && GL != "121010002")
                                                    {
                                                        yn = "Y";
                                                    }
                                                    else
                                                    {
                                                        yn = "N";
                                                    }*/
                                                    string capdate = "";
                                                    string LeaseStartDate = "";
                                                    string LeaseEndDate = "";
                                                    if (dtfa.Rows[k]["asset_dep_type"].ToString() == "LPM")
                                                    {
                                                        LeaseStartDate = dtfa.Rows[k]["branch_lease_start_date"].ToString();
                                                        LeaseEndDate = dtfa.Rows[k]["branch_lease_end_date"].ToString();

                                                        if (Convert.ToDateTime(dtfa.Rows[k]["cap_Grndate"].ToString()) < Convert.ToDateTime(dtfa.Rows[k]["branch_lease_start_date"].ToString()))
                                                        {
                                                            capdate = objCmnFunctions.convertoDateTimeString(LeaseStartDate);
                                                        }
                                                        else
                                                        {
                                                            capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDateTime(dtfa.Rows[k]["cap_Grndate"].ToString()) < Convert.ToDateTime(dtfa.Rows[k]["branch_start_date"].ToString()))
                                                        {
                                                            capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["branch_start_date"].ToString());
                                                        }
                                                        else
                                                        {
                                                            capdate = objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString());
                                                        }
                                                    }



                                                    string assetsetailno = string.Empty, verifiable = string.Empty;
                                                    DataTable qty = new DataTable();
                                                    cmdfa = new SqlCommand("pr_ifams_trn_cwip", confa);
                                                    cmdfa.CommandType = CommandType.StoredProcedure;
                                                    cmdfa.Parameters.AddWithValue("@Code", dtfa.Rows[k]["cap_AssetCode"]);
                                                    cmdfa.Parameters.AddWithValue("@action", "Asset");
                                                    //cmdfa.Transaction = tran;
                                                    dafa = new SqlDataAdapter(cmdfa);
                                                    dafa.Fill(qty);
                                                    if (qty.Rows.Count > 0)
                                                    {
                                                        assetsetailno = qty.Rows[0]["aset_serial_no"].ToString();
                                                        verifiable = qty.Rows[0]["asset_verifiable"].ToString();
                                                    }

                                                    if (assetsetailno == "N" && verifiable == "Y")
                                                    {
                                                        for (int t = 0; t < Convert.ToInt64(dtfa.Rows[k]["cap_qty"]); t++)
                                                        {
                                                            string ASSET_ID = comuidfa.generateAssetid(dtfa.Rows[k]["cap_Loc_Code"].ToString(), dtfa.Rows[k]["cap_AssetCode"].ToString());
                                                            decimal Amount = Math.Round((Convert.ToDecimal(dtfa.Rows[k]["cap_Amount"]) / Convert.ToInt64(dtfa.Rows[k]["cap_qty"])), 2);
                                                            string[,] incodeinst = new string[,]
                                            {
                                                {"assetdetails_assetdet_id",ASSET_ID}                    
                                                ,{"assetdetails_qty","1"} 
                                                ,{"assetdetails_asset_serialno", dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                                ,{"assetdetails_asset_code",dtfa.Rows[k]["asset_gid"].ToString()}
                                                //,{"assetdetails_asset_value",dtfa.Rows[k]["cap_Amount"].ToString()}
                                                ,{"assetdetails_asset_value",Amount.ToString()}
                                                //,{"assetdetails_asset_description",dtfa.Rows[k]["cap_Subcategory"].ToString()}
                                                ,{"assetdetails_asset_description",dtfa.Rows[k]["cap_podetails_desc"].ToString()}
                                                ,{"assetdetails_cap_date",capdate}
                                                ,{"assetdetails_po_number",dtfa.Rows[k]["cap_po_gid"].ToString()}
                                                ,{"assetdetails_tfr_status","N"}   
                                                ,{"assetdetails_active_status","Y"}
                                                ,{"assetdetails_sale_status","N"}    
                                                ,{"assetdetails_not_5kcase",yn}
                                                ,{"assetdetails_trf_value",dtfa.Rows[k]["cap_Total"].ToString()} 
                                                ,{"assetdetails_asset_owner","F"}
                                                ,{"assetdetails_trn_date",capdate}
                                                ,{"assetdetails_impair_asset","N"}
                                                ,{"assetdetails_assetcode_changestatus","N"}
                                                ,{"assetdetails_isremoved","N"}
                                                ,{"assetdetails_dep_rate","0"}                                    
                                                ,{"assetdetails_branch_gid",dtfa.Rows[k]["branch_gid"].ToString()}
                                                ,{"assetdetails_addate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                                ,{"assetdetails_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                                ,{"assetdetails_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}    
                                                ,{"assetdetails_takenby","14"}     
                                                ,{"assetdetails_assetid_source","1"}
                                                ,{"assetdetails_invoice_gid",inv.ToString()}
                                                ,{"assetdetails_ecf_gid",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                                ,{"assetdetails_entity_gid","1"}
                                                ,{"assetdetails_asset_groupid",group_id.ToString()}
                                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                                ,{"assetdetails_assetcode_changeid",string.Empty}
                                                ,{"assetdetails_barcode",string.Empty}
                                                ,{"assetdetails_capnew_date",string.Empty}
                                                ,{"assetdetails_capold_date",string.Empty}
                                                ,{"assetdetails_cbf_gid",dtfa.Rows[k]["cbfheader_gid"].ToString()}
                                                ,{"assetdetails_cbfnum",dtfa.Rows[k]["cbfheader_cbfno"].ToString()}
                                                ,{"assetdetails_dep_onhold","N"}
                                                ,{"assetdetails_ecfnum",dtfa.Rows[k]["ecf_no"].ToString()}
                                                ,{"assetdetails_imp_date",string.Empty}
                                                ,{"assetdetails_inwdetailgid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()}
                                                ,{"assetdetails_inwheadergid",dtfa.Rows[k]["cap_grn_gid"].ToString()}
                                                ,{"assetdetails_lease_enddate",string.IsNullOrEmpty(LeaseEndDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseEndDate.ToString())}
                                                ,{"assetdetails_lease_startdate",string.IsNullOrEmpty(LeaseStartDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseStartDate.ToString())}
                                                ,{"assetdetails_physical_assetid","0"}
                                                ,{"assetdetails_physical_idrelease",string.Empty}
                                                ,{"assetdetails_ponum",dtfa.Rows[k]["cap_Ponumber"].ToString()}
                                                ,{"assetdetails_recon_reference",string.Empty}
                                                ,{"assetdetails_reduced_value","0"}
                                                ,{"assetdetails_sale_date",string.Empty}
                                                ,{"assetdetails_sale_id",string.Empty}
                                                ,{"assetdetails_spoke_asset","N"}
                                                ,{"assetdetails_status",string.Empty}
                                                ,{"assetdetails_tfr_date",string.Empty}
                                                ,{"assetdetails_tfr_id",string.Empty}
                                                ,{"assetdetails_trf_from",string.Empty}
                                                ,{"assetdetails_trf_reason",string.Empty}
                                                ,{"assetdetails_trf_to",string.Empty}
                                                ,{"assetdetails_update_by",string.Empty}
                                                ,{"assetdetails_update_date",string.Empty}
                                                ,{"assetdetails_upld_ref",string.Empty}
                                                ,{"assetdetails_upld_status","N"}
                                                ,{"assetdetails_wrt_date",string.Empty}
                                                ,{"assetdetails_wrt_id",string.Empty}
                                                ,{"assetdetails_wrt_status","N"}
                                                ,{"assetdetails_vendorname",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                                ,{"assetdetails_inwarddate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                              
                                            };
                                                            Result = commfa.InsertCommon(incodeinst, "fa_trn_tassetdetails");
                                                            // Result = commfa.InsertCommon_FA(incodeinst, "fa_trn_tassetdetails");

                                                            string[,] incode = new string[,]
                                            {
                                            {"h_asst_groupid_no",group_id }
                                            ,{"h_asst_code",asset}
                                            ,{"h_asst_owner","F"}
                                            ,{"h_asst_desc",dtfa.Rows[k]["cap_Subcategory"].ToString()}                   
                                            ,{"h_emp_code",comuidfa.GetLoginUserGid().ToString() }   
                                            ,{"h_capt_date",objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString())}
                                            //,{"h_purc_cost",dtfa.Rows[k]["cap_Total"].ToString()} //Amount
                                            ,{"h_purc_cost",Amount.ToString()} 
                                            ,{"h_sales_tax_amt",dtfa.Rows[k]["cap_Tax1"].ToString()}
                                           // ,{"h_org_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                           ,{"h_org_capt_amt",Amount.ToString()}
                                            //,{"h_cur_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_cur_capt_amt",Amount.ToString()}
                                            ,{"h_org_qty_old","1"}
                                            ,{"h_org_qty","1"}
                                            ,{"h_cur_qty","1"}
                                            ,{"h_model_no",""}
                                            ,{"h_supplier_code",dtfa.Rows[k]["supplierheader_suppliercode"].ToString()}
                                             ,{"h_supplier_name",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                            ,{"h_serial_no",dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                            ,{"h_ecfno",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                            ,{"h_status","Y"}
                                            ,{"h_po_mast_po_no",dtfa.Rows[k]["cap_po_gid"].ToString()}                   
                                            ,{"h_depr_glcode",dtfa.Rows[k]["cap_glcode"].ToString()}
                                            ,{"h_narration",""}                    
                                            ,{"h_isremoved","N"}
                                            ,{"h_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                            ,{"h_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                            };
                                                            // GetfaConnection();
                                                            Result = commfa.InsertCommon(incode, "fa_trn_th");
                                                            //Result = commfa.InsertCommon_FA(incode, "fa_trn_th");
                                                            cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                            cmdfa.CommandType = CommandType.StoredProcedure;
                                                            cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                                                            cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_ID;
                                                            //    cmdfa.Transaction = tran;
                                                            string ASSET_GID = Convert.ToString(cmdfa.ExecuteScalar());
                                                            cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                            cmdfa.CommandType = CommandType.StoredProcedure;
                                                            cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                                                            cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_GID;
                                                            //  cmdfa.Transaction = tran;
                                                            dafa = new SqlDataAdapter(cmdfa);
                                                            string glCode = "";
                                                            string branch = "";
                                                            string cat = "";
                                                            string subcat = "";
                                                            string asset_id = "";
                                                            string BS = "";
                                                            string CC = "";
                                                            string ratio = "";
                                                            DataTable UPLOADDATATBL = new DataTable();
                                                            dafa.Fill(UPLOADDATATBL);
                                                            if (UPLOADDATATBL.Rows.Count > 0)
                                                                foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                                {
                                                                    glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                                                    branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                                                    cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                                                    if (cat.Length > 16)
                                                                        cat = cat.Substring(0, 16);
                                                                    subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                                                    asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                                                }

                                                            cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                            cmdfa.CommandType = CommandType.StoredProcedure;
                                                            cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[k]["cap_inwdetailgid"].ToString()) ? "0" : dtfa.Rows[k]["cap_inwdetailgid"].ToString());
                                                            cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnbscc";
                                                            //    cmdfa.Transaction = tran;
                                                            dafa = new SqlDataAdapter(cmdfa);
                                                            UPLOADDATATBL = new DataTable();
                                                            dafa.Fill(UPLOADDATATBL);
                                                            if (UPLOADDATATBL.Rows.Count > 0)
                                                                foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                                {
                                                                    BS = UPLOADDATATBL.Rows[0]["bs_code"].ToString();
                                                                    CC = UPLOADDATATBL.Rows[0]["cc_code"].ToString();
                                                                    ratio = "100";
                                                                }
                                                            DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(dtfa.Rows[k]["cap_makerid"].ToString()));
                                                            string[,] glCoulmsD = new string[,]
                                            {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[k]["cap_glcode"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                               // {"tran_amount",dtfa.Rows[k]["cap_Total"].ToString()},
                                                {"tran_amount",Amount.ToString()},
                                                {"tran_acc_mode","D".ToString()},
                                                {"tran_mult","-1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},ASSET_GID
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                {"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                            };
                                                            string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                                                            // string insertforglD = objCommonIUD.InsertCommon_FA(glCoulmsD, "fa_trn_ttran");
                                                            string[,] glCoulmsC = new string[,]
                                        {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[i]["assetcategory_assset_clearing"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                //{"tran_amount",dtfa.Rows[i]["cap_Total"].ToString()},
                                                {"tran_amount",Amount.ToString()},
                                                {"tran_acc_mode","C".ToString()},
                                                {"tran_mult","1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                {"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                        };

                                                            string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                                                            // string insertforglC = objCommonIUD.InsertCommon_FA(glCoulmsC, "fa_trn_ttran");
                                                            string[,] cval = new string[,]
                                        {
                                            {"c_br_code","2"},
                                            {"c_ecf_no",dtfa.Rows[k]["cap_Ecfid"].ToString()},
                                            {"c_in_no",inv.ToString()},
                                            {"c_inv_srlno",dtfa.Rows[k]["cap_Invoiceid"].ToString()},
                                            {"c_ASST_ID_NO",group_id},
                                            {"c_COST_CODE",CC},
                                            {"c_BUSS_CODE",BS},
                                            {"c_RATIO",ratio},
                                            {"c_CCBS",CC + BS},
                                            // {"c_loc",dtfa.Rows[i]["location_gid"].ToString()},
                                            {"c_isremoved","N"},
                                            {"c_insert_by",comuidfa.GetLoginUserGid().ToString()},
                                            {"c_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                        };

                                                            cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                            cmdfa.CommandType = CommandType.StoredProcedure;
                                                            cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(branch) ? "0" : branch);
                                                            cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbranchlegacy";
                                                            //     cmdfa.Transaction = tran;
                                                            string branchlegacycode = Convert.ToString(cmdfa.ExecuteScalar());

                                                            string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",ASSET_ID.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",CC + BS.ToString()},
                                {"assetlocation_location_fc",BS.ToString()},
                                {"assetlocation_location_cc",CC.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}
                            };

                                                            string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");
                                                            //  string insertlocationcmn = objCommonIUD.InsertCommon_FA(codeloc, "fa_trn_tassetlocation");


                                                            Result = commfa.InsertCommon(cval, "fa_trn_tc");
                                                            //  Result = commfa.InsertCommon_FA(cval, "fa_trn_tc");
                                                        }//Asset spliting else for end
                                                        HttpContext.Current.Session["grngid"] = Convert.ToInt32(dtfa.Rows[0]["cap_grn_gid"].ToString());
                                                        HttpContext.Current.Session["pogid"] = Convert.ToInt32(dtfa.Rows[0]["cap_po_gid"].ToString());
                                                        string aaa = dtfa.Rows[k]["cap_grn_gid"].ToString();
                                                        string[,] grncol1 = new string[,]
                                        {
                                            {"grninwrddet_capstatus",ConfigurationManager.AppSettings["EApproved"].ToString()}
                                        };
                                                        string[,] grnwfrcol1 = new string[,]
                                        {
                                       // {"grninwrddet_grninwrdhead_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_grninwrdhead_gid",dtfa.Rows[k]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_gid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()},                               
                                        };
                                                        string grnstatus1 = commfa.UpdateCommon(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                        // string grnstatus1 = commfa.UpdateCommon_FA(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                    }//Asset spliting else if end

                                                    else/* ELse fro second group id generate group*/
                                                    {
                                                        string ASSET_ID = comuidfa.generateAssetid(dtfa.Rows[k]["cap_Loc_Code"].ToString(), dtfa.Rows[k]["cap_AssetCode"].ToString());
                                                        string[,] incodeinst = new string[,]
                                            {
                                                {"assetdetails_assetdet_id",ASSET_ID}                    
                                                ,{"assetdetails_qty","1"} 
                                                ,{"assetdetails_asset_serialno", dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                                ,{"assetdetails_asset_code",dtfa.Rows[k]["asset_gid"].ToString()}
                                                ,{"assetdetails_asset_value",dtfa.Rows[k]["cap_Amount"].ToString()}
                                                //,{"assetdetails_asset_description",dtfa.Rows[k]["cap_Subcategory"].ToString()}
                                                ,{"assetdetails_asset_description",dtfa.Rows[k]["cap_podetails_desc"].ToString()}
                                                ,{"assetdetails_cap_date",capdate}
                                                ,{"assetdetails_po_number",dtfa.Rows[k]["cap_po_gid"].ToString()}
                                                ,{"assetdetails_tfr_status","N"}   
                                                ,{"assetdetails_active_status","Y"}
                                                ,{"assetdetails_sale_status","N"}    
                                                ,{"assetdetails_not_5kcase",yn}
                                                ,{"assetdetails_trf_value",dtfa.Rows[k]["cap_Total"].ToString()} 
                                                ,{"assetdetails_asset_owner","F"}
                                                ,{"assetdetails_trn_date",capdate}
                                                ,{"assetdetails_impair_asset","N"}
                                                ,{"assetdetails_assetcode_changestatus","N"}
                                                ,{"assetdetails_isremoved","N"}
                                                ,{"assetdetails_dep_rate","0"}                                    
                                                ,{"assetdetails_branch_gid",dtfa.Rows[k]["branch_gid"].ToString()}
                                                ,{"assetdetails_addate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                                ,{"assetdetails_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                                ,{"assetdetails_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}    
                                                ,{"assetdetails_takenby","14"}     
                                                ,{"assetdetails_assetid_source","1"}
                                                ,{"assetdetails_invoice_gid",inv.ToString()}
                                                ,{"assetdetails_ecf_gid",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                                ,{"assetdetails_entity_gid","1"}
                                                ,{"assetdetails_asset_groupid",group_id.ToString()}
                                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                                ,{"assetdetails_assetcode_changeid",string.Empty}
                                                ,{"assetdetails_barcode",string.Empty}
                                                ,{"assetdetails_capnew_date",string.Empty}
                                                ,{"assetdetails_capold_date",string.Empty}
                                                ,{"assetdetails_cbf_gid",dtfa.Rows[k]["cbfheader_gid"].ToString()}
                                                ,{"assetdetails_cbfnum",dtfa.Rows[k]["cbfheader_cbfno"].ToString()}
                                                ,{"assetdetails_dep_onhold","N"}
                                                ,{"assetdetails_ecfnum",dtfa.Rows[k]["ecf_no"].ToString()}
                                                ,{"assetdetails_imp_date",string.Empty}
                                                ,{"assetdetails_inwdetailgid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()}
                                                ,{"assetdetails_inwheadergid",dtfa.Rows[k]["cap_grn_gid"].ToString()}
                                                ,{"assetdetails_lease_enddate",string.IsNullOrEmpty(LeaseEndDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseEndDate.ToString())}
                                                ,{"assetdetails_lease_startdate",string.IsNullOrEmpty(LeaseStartDate.ToString()) ? string.Empty : objCmnFunctions.convertoDateTimeString(LeaseStartDate.ToString())}
                                                ,{"assetdetails_physical_assetid","0"}
                                                ,{"assetdetails_physical_idrelease",string.Empty}
                                                ,{"assetdetails_ponum",dtfa.Rows[k]["cap_Ponumber"].ToString()}
                                                ,{"assetdetails_recon_reference",string.Empty}
                                                ,{"assetdetails_reduced_value","0"}
                                                ,{"assetdetails_sale_date",string.Empty}
                                                ,{"assetdetails_sale_id",string.Empty}
                                                ,{"assetdetails_spoke_asset","N"}
                                                ,{"assetdetails_status",string.Empty}
                                                ,{"assetdetails_tfr_date",string.Empty}
                                                ,{"assetdetails_tfr_id",string.Empty}
                                                ,{"assetdetails_trf_from",string.Empty}
                                                ,{"assetdetails_trf_reason",string.Empty}
                                                ,{"assetdetails_trf_to",string.Empty}
                                                ,{"assetdetails_update_by",string.Empty}
                                                ,{"assetdetails_update_date",string.Empty}
                                                ,{"assetdetails_upld_ref",string.Empty}
                                                ,{"assetdetails_upld_status","N"}
                                                ,{"assetdetails_wrt_date",string.Empty}
                                                ,{"assetdetails_wrt_id",string.Empty}
                                                ,{"assetdetails_wrt_status","N"}
                                                ,{"assetdetails_vendorname",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                                ,{"assetdetails_inwarddate",dm.convertoDate (DateTime.Today.ToShortDateString())}
                                              
                                            };
                                                        Result = commfa.InsertCommon(incodeinst, "fa_trn_tassetdetails");
                                                        // Result = commfa.InsertCommon_FA(incodeinst, "fa_trn_tassetdetails");

                                                        string[,] incode = new string[,]
                                            {
                                            {"h_asst_groupid_no",group_id }
                                            ,{"h_asst_code",asset}
                                            ,{"h_asst_owner","F"}
                                            ,{"h_asst_desc",dtfa.Rows[k]["cap_Subcategory"].ToString()}                   
                                            ,{"h_emp_code",comuidfa.GetLoginUserGid().ToString() }   
                                            ,{"h_capt_date",objCmnFunctions.convertoDateTimeString(dtfa.Rows[k]["cap_Grndate"].ToString())}
                                            ,{"h_purc_cost",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_sales_tax_amt",dtfa.Rows[k]["cap_Tax1"].ToString()}
                                            ,{"h_org_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_cur_capt_amt",dtfa.Rows[k]["cap_Total"].ToString()}
                                            ,{"h_org_qty_old","1"}
                                            ,{"h_org_qty","1"}
                                            ,{"h_cur_qty","1"}
                                            ,{"h_model_no",""}
                                            ,{"h_supplier_code",dtfa.Rows[k]["supplierheader_suppliercode"].ToString()}
                                             ,{"h_supplier_name",dtfa.Rows[k]["supplierheader_name"].ToString()}
                                            ,{"h_serial_no",dtfa.Rows[k]["cap_assetsrlno"].ToString()}
                                            ,{"h_ecfno",dtfa.Rows[k]["cap_Ecfid"].ToString()}
                                            ,{"h_status","Y"}
                                            ,{"h_po_mast_po_no",dtfa.Rows[k]["cap_po_gid"].ToString()}                   
                                            ,{"h_depr_glcode",dtfa.Rows[k]["cap_glcode"].ToString()}
                                            ,{"h_narration",""}                    
                                            ,{"h_isremoved","N"}
                                            ,{"h_insert_by",comuidfa.GetLoginUserGid().ToString()}
                                            ,{"h_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                            };
                                                        // GetfaConnection();
                                                        Result = commfa.InsertCommon(incode, "fa_trn_th");
                                                        //Result = commfa.InsertCommon_FA(incode, "fa_trn_th");
                                                        cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                                                        cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_ID;
                                                        //    cmdfa.Transaction = tran;
                                                        string ASSET_GID = Convert.ToString(cmdfa.ExecuteScalar());
                                                        cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                                                        cmdfa.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_GID;
                                                        //  cmdfa.Transaction = tran;
                                                        dafa = new SqlDataAdapter(cmdfa);
                                                        string glCode = "";
                                                        string branch = "";
                                                        string cat = "";
                                                        string subcat = "";
                                                        string asset_id = "";
                                                        string BS = "";
                                                        string CC = "";
                                                        string ratio = "";
                                                        DataTable UPLOADDATATBL = new DataTable();
                                                        dafa.Fill(UPLOADDATATBL);
                                                        if (UPLOADDATATBL.Rows.Count > 0)
                                                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                            {
                                                                glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                                                branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                                                cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                                                if (cat.Length > 16)
                                                                    cat = cat.Substring(0, 16);
                                                                subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                                                asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                                            }

                                                        cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@prodgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[k]["cap_inwdetailgid"].ToString()) ? "0" : dtfa.Rows[k]["cap_inwdetailgid"].ToString());
                                                        cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnbscc";
                                                        //    cmdfa.Transaction = tran;
                                                        dafa = new SqlDataAdapter(cmdfa);
                                                        UPLOADDATATBL = new DataTable();
                                                        dafa.Fill(UPLOADDATATBL);
                                                        if (UPLOADDATATBL.Rows.Count > 0)
                                                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                                            {
                                                                BS = UPLOADDATATBL.Rows[0]["bs_code"].ToString();
                                                                CC = UPLOADDATATBL.Rows[0]["cc_code"].ToString();
                                                                ratio = "100";
                                                            }
                                                        DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(dtfa.Rows[k]["cap_makerid"].ToString()));
                                                        string[,] glCoulmsD = new string[,]
                                            {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[k]["cap_glcode"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                {"tran_amount",dtfa.Rows[k]["cap_Total"].ToString()},
                                                {"tran_acc_mode","D".ToString()},
                                                {"tran_mult","-1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},ASSET_GID
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                {"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                            };
                                                        string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                                                        // string insertforglD = objCommonIUD.InsertCommon_FA(glCoulmsD, "fa_trn_ttran");
                                                        string[,] glCoulmsC = new string[,]
                                        {                                
                                                {"tran_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dtfa.Rows[i]["assetcategory_assset_clearing"].ToString()},
                                                {"tran_desc"," | CAPITALIZATION OF ASSET | " + dtfa.Rows[k]["ecf_no"].ToString() + "|" + dtfa.Rows[k]["cap_podetails_desc"].ToString() + "|" + group_id.ToString() },
                                                {"tran_amount",dtfa.Rows[i]["cap_Total"].ToString()},
                                                {"tran_acc_mode","C".ToString()},
                                                {"tran_mult","1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",dm.GetRef("COA")},
                                                //{"tran_ref_gid",""},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                {"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",dm.convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dtfa.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                        };

                                                        string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                                                        // string insertforglC = objCommonIUD.InsertCommon_FA(glCoulmsC, "fa_trn_ttran");
                                                        string[,] cval = new string[,]
                                        {
                                            {"c_br_code","2"},
                                            {"c_ecf_no",dtfa.Rows[k]["cap_Ecfid"].ToString()},
                                            {"c_in_no",inv.ToString()},
                                            {"c_inv_srlno",dtfa.Rows[k]["cap_Invoiceid"].ToString()},
                                            {"c_ASST_ID_NO",group_id},
                                            {"c_COST_CODE",CC},
                                            {"c_BUSS_CODE",BS},
                                            {"c_RATIO",ratio},
                                            {"c_CCBS",CC + BS},
                                            // {"c_loc",dtfa.Rows[i]["location_gid"].ToString()},
                                            {"c_isremoved","N"},
                                            {"c_insert_by",comuidfa.GetLoginUserGid().ToString()},
                                            {"c_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                        };

                                                        cmdfa = new SqlCommand("pr_fb_trn_wonew", confa);
                                                        cmdfa.CommandType = CommandType.StoredProcedure;
                                                        cmdfa.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(branch) ? "0" : branch);
                                                        cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbranchlegacy";
                                                        //     cmdfa.Transaction = tran;
                                                        string branchlegacycode = Convert.ToString(cmdfa.ExecuteScalar());

                                                        string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",ASSET_ID.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",CC + BS.ToString()},
                                {"assetlocation_location_fc",BS.ToString()},
                                {"assetlocation_location_cc",CC.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",dm.convertoDate (DateTime.Today.ToShortDateString())}
                            };

                                                        string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");
                                                        //  string insertlocationcmn = objCommonIUD.InsertCommon_FA(codeloc, "fa_trn_tassetlocation");


                                                        Result = commfa.InsertCommon(cval, "fa_trn_tc");
                                                        //  Result = commfa.InsertCommon_FA(cval, "fa_trn_tc");

                                                        HttpContext.Current.Session["grngid"] = Convert.ToInt32(dtfa.Rows[0]["cap_grn_gid"].ToString());
                                                        HttpContext.Current.Session["pogid"] = Convert.ToInt32(dtfa.Rows[0]["cap_po_gid"].ToString());
                                                        string aaa = dtfa.Rows[k]["cap_grn_gid"].ToString();
                                                        string[,] grncol1 = new string[,]
                                        {
                                            {"grninwrddet_capstatus",ConfigurationManager.AppSettings["EApproved"].ToString()}
                                        };
                                                        string[,] grnwfrcol1 = new string[,]
                                        {
                                       // {"grninwrddet_grninwrdhead_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_grninwrdhead_gid",dtfa.Rows[k]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_gid",dtfa.Rows[k]["cap_inwdetailgid"].ToString()},                               
                                        };
                                                        string grnstatus1 = commfa.UpdateCommon(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");
                                                        // string grnstatus1 = commfa.UpdateCommon_FA(grncol1, grnwfrcol1, "fb_trn_tgrninwrddet");

                                                    } //else end
                                                }
                                            }
                                            success = "";
                                            a1.Clear();
                                        }
                                    }
                                }


                            }
                            // group_id = comuidfa.GetSequenceNoFam("2").ToString();


                            //grp gl entry by asset code, branch , cap date , asset value


                            //Muthu 09-11-2016 

                            //cmdfa = new SqlCommand("pr_ifams_trn_cwip", confa);
                            //cmdfa.CommandType = CommandType.StoredProcedure;
                            //cmdfa.Parameters.AddWithValue("@action", "InsertGprGLEnrties");
                            //cmdfa.Parameters.AddWithValue("@person", Convert.ToString(HttpContext.Current.Session["employee_code"]));
                            //cmdfa.Parameters.AddWithValue("@AssetIds", tran_upload_gid);
                            //cmdfa.Parameters.AddWithValue("@code", dm.GetRef("COA"));
                            //cmdfa.ExecuteNonQuery();


                            string[,] upcol = new string[,]
                                        {
                                        {"cap_Status","3"},
                                        {"cap_Remarks",remarks}
                                        };
                            string[,] upcon = new string[,]
                                        {
                                        {"cap_Ecfid",dtfa.Rows[0]["cap_Ecfid"].ToString()},
                                        {"cap_Invoiceid",dtfa.Rows[0]["cap_Invoiceid"].ToString()},
                                        {"cap_Status",ConfigurationManager.AppSettings["Waitingforapproval"].ToString()}
                                        };
                            string upresult = commfa.UpdateCommon(upcol, upcon, "fa_trn_tcapitalization");
                            //  string upresult = commfa.UpdateCommon_FA(upcol, upcon, "fa_trn_tcapitalization");
                            string[,] uptax = new string[,]
                                        {
                                        {"exp_status","2"}
                                        };
                            string[,] upwhrtax = new string[,]
                                        {
                                        {"exp_invoicegid",inv.ToString()}
                                        };
                            string taxr = commfa.UpdateCommon(uptax, upwhrtax, "fa_tmp_texpensedet");
                            //  string taxr = commfa.UpdateCommon_FA(uptax, upwhrtax, "fa_tmp_texpensedet");

                            // string[,] grncola = new string[,]
                            //          {
                            //              {"grniwrdheader_capstatus","3"}
                            //          };
                            // string[,] grnwfrcola = new string[,]
                            //         {
                            //              {"grninwrdheader_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},
                            //              {"grninwardheader_poheader",dtfa.Rows[0]["cap_po_gid"].ToString()},
                            //              {"grninwrdheader_status",ConfigurationManager.AppSettings["EApproved"].ToString()}
                            //          };
                            //string grnstatusa = commfa.UpdateCommon(grncola, grnwfrcola, "fb_trn_tgrninwrdheader");
                        }
                    }
                    else if (status == "REJECTED")
                    {
                        //SqlCommand cmdfa1 = new SqlCommand("pr_ifams_trn_capcheckerinsert", confa);
                        //cmdfa1.CommandType = CommandType.StoredProcedure;
                        //cmdfa1.Parameters.AddWithValue("@inv", inv);
                        //cmdfa1.Parameters.AddWithValue("@qry", "total");
                        //SqlDataAdapter dafa1 = new SqlDataAdapter(cmdfa1);
                        //DataTable dtfa1 = new DataTable();
                        //dafa1.Fill(dtfa1);
                        //string inv_id="";
                        //if (dtfa1.Rows.Count > 0)
                        //{
                        //    inv_id = dtfa1.Rows[0]["invoice_gid"].ToString();
                        //}
                        string Reject = "4";
                        string[,] upcol1 = new string[,]
                                {
                                {"cap_Status",Reject}
                                };
                        string[,] upval = new string[,]
                                {                   
                                {"cap_Invoiceid",inv.ToString()},
                                {"cap_Status","2"}
                                };
                        string rejresult = commfa.UpdateCommon(upcol1, upval, "fa_trn_tcapitalization");
                        // string rejresult = commfa.UpdateCommon_FA(upcol1, upval, "fa_trn_tcapitalization");
                        cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);

                        cmdfa.CommandType = CommandType.StoredProcedure;
                        cmdfa.Parameters.AddWithValue("@for", "exptax");
                        cmdfa.Parameters.AddWithValue("@invid", inv.ToString());
                        //   cmdfa.Transaction = tran;
                        string taxamt = Convert.ToString(cmdfa.ExecuteScalar());
                        string[,] uptax = new string[,]
                                        {
                                        {"exp_status","1"},
                                        {"exp_balance",string.IsNullOrEmpty(taxamt)?"0":taxamt},
                                        {"exp_expense_now","0"},                                        
                                        {"exp_cap_now","0"},
                                        {"exp_al_cap","0"}
                                        };
                        string[,] upwhrtax = new string[,]
                                        {
                                        {"exp_invoicegid",inv.ToString()}
                                        };
                        string taxr = commfa.UpdateCommon(uptax, upwhrtax, "fa_tmp_texpensedet");
                        // string taxr = commfa.UpdateCommon_FA(uptax, upwhrtax, "fa_tmp_texpensedet");
                        if (dtfa.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtfa.Rows.Count; i++)
                            {
                                string[,] grncolr = new string[,]
                                {
                                {"grniwrdheader_capstatus","4"}
                                };
                                string[,] grnwfrcolr = new string[,]
                                {
                                //{"grninwrdheader_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()}, //Muthu 22-02-2017 (Multiple grn reject time only one grn update issue)
                                //{"grninwardheader_poheader",dtfa.Rows[0]["cap_po_gid"].ToString()}             
                                {"grninwrdheader_gid",dtfa.Rows[i]["cap_grn_gid"].ToString()},
                                {"grninwardheader_poheader",dtfa.Rows[i]["cap_po_gid"].ToString()}             
                                };
                                string grnstatusar = commfa.UpdateCommon(grncolr, grnwfrcolr, "fb_trn_tgrninwrdheader");
                                // string grnstatusar = commfa.UpdateCommon_FA(grncolr, grnwfrcolr, "fb_trn_tgrninwrdheader");
                                string[,] grncol11 = new string[,]
                                        {
                                            {"grninwrddet_capstatus","4"}
                                        };
                                string[,] grnwfrcol11 = new string[,]
                                        {
                                        //{"grninwrddet_grninwrdhead_gid",dtfa.Rows[0]["cap_grn_gid"].ToString()},//Muthu 22-02-2017 (Multiple grn reject time only one grn update issue)
                                        //{"grninwrddet_gid",dtfa.Rows[i]["cap_inwdetailgid"].ToString()},    
                                        {"grninwrddet_grninwrdhead_gid",dtfa.Rows[i]["cap_grn_gid"].ToString()},
                                        {"grninwrddet_gid",dtfa.Rows[i]["cap_inwdetailgid"].ToString()},    
                                        };
                                string grnstatus1 = commfa.UpdateCommon(grncol11, grnwfrcol11, "fb_trn_tgrninwrddet");
                                // string grnstatus1 = commfa.UpdateCommon_FA(grncol11, grnwfrcol11, "fb_trn_tgrninwrddet");
                            }
                        }

                    }
                }
                // tran.Commit();

            }
            catch (Exception ex)
            {
                // tran.Rollback();
                confa.Close();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                // Result = ex.Message.ToString();
                // return Result;
            }
            finally
            {
                confa.Close();

            }
            return Result;

        }

        public DataTable Getcapamount(int ecfgid, int id)
        {
            dtfa = new DataTable();
            try
            {
                cmdfa = new SqlCommand("pr_ifams_getprecapamt", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@ecfgid", ecfgid);
                //cmdfa.Parameters.AddWithValue("@ecfStatus", ConfigurationManager.AppSettings["faApproved"].ToString());
                cmdfa.Parameters.AddWithValue("@ecfStatus", "3");
                cmdfa.Parameters.AddWithValue("@invgid", id);
                dafa = new SqlDataAdapter(cmdfa);

                dafa.Fill(dtfa);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dtfa;
        }

        public List<capitalizationMaker> Getinvoicetaxdet(int invgid, string view)
        {
            List<capitalizationMaker> objtaxlist = new List<capitalizationMaker>();
            capitalizationMaker objtax = new capitalizationMaker();
            decimal sample = Convert.ToDecimal("0.00");
            int count = 0;
            string TaxResult = string.Empty;
            decimal expense = Convert.ToDecimal("0.00");
            int status = 0;
            if (view == "approval")
            {
                status = 2;
            }
            else
            {
                status = 1;
            }

            try
            {
                SqlCommand cmdex = new SqlCommand("pr_ifams_getprecapamount", confa);
                cmdex.CommandType = CommandType.StoredProcedure;
                cmdex.Parameters.AddWithValue("@invgid", invgid);
                cmdex.Parameters.AddWithValue("@Status", status);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdex);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        count = i + 1;
                        objtax = new capitalizationMaker();
                        objtax.indextax = count;
                        objtax.TaxName = dt.Rows[i]["exp_taxtype"].ToString();
                        // objtax.TaxSubTypeName = dtfa.Rows[i]["taxsubtype_name"].ToString();
                        objtax.Invoicetaxamount = Convert.ToDecimal(dt.Rows[i]["exp_taxamount"].ToString());
                        objtax.invoiceno = dt.Rows[i]["exp_invoiceno"].ToString();
                        objtax.Invoicetaxglno = dt.Rows[i]["exp_glcode"].ToString();
                        objtax.Already_Capiptal = Convert.ToDecimal(dt.Rows[i]["exp_al_cap"].ToString());
                        objtax.Cap_Now = Convert.ToDecimal(dt.Rows[i]["exp_cap_now"].ToString());
                        objtax.Already_Exp = Convert.ToDecimal(dt.Rows[i]["exp_al_expense"].ToString());
                        objtax.Exp_Now = Convert.ToDecimal(dt.Rows[i]["exp_expense_now"].ToString());
                        objtax.Balance = Convert.ToDecimal(dt.Rows[i]["exp_balance"].ToString());                         
                        objtax.provider_location=  string.IsNullOrEmpty(dt.Rows[i]["provider_location"].ToString())?"":dt.Rows[i]["provider_location"].ToString();
                        objtax.receiver_location = string.IsNullOrEmpty(dt.Rows[i]["receiver_location"].ToString()) ? "" : dt.Rows[i]["receiver_location"].ToString();  
                        objtax.invoicegid = invgid.ToString();
                        expense = expense + objtax.Exp_Now;
                        objtax.Expense = expense;
                        objtaxlist.Add(objtax);


                        //                        string[,] coltemp = new string[,]
                        //                        {
                        //{"exp_index",count.ToString()},
                        //{"exp_invoicegid",invgid.ToString()},
                        //{"exp_invoiceno",objtax.invoiceno},
                        //{"exp_glcode", objtax.Invoicetaxglno},
                        //{"exp_taxtype",objtax.TaxName},
                        //{"exp_taxamount",objtax.Invoicetaxamount.ToString()},
                        //{"exp_al_cap",objtax.Already_Capiptal.ToString()},
                        //{"exp_cap_now",objtax.Cap_Now.ToString()},
                        //{"exp_al_expense",objtax.Already_Exp.ToString()},
                        //{"exp_expense_now",objtax.Exp_Now.ToString()},
                        //{"exp_balance",objtax.Balance.ToString()},
                        //{"exp_status","1"},
                        //{"exp_isremoved","N"}
                        //                        };
                        //                        TaxResult = commfa.InsertCommon(coltemp, "fa_tmp_texpensedet");

                    }
                    objtax.Addtaxlist = objtaxlist;
                }
                else
                {
                    TaxResult = deletetax(invgid);
                    cmdfa = new SqlCommand("pr_ifams_getinvoicetaxdet", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@invgid", invgid);
                    dtfa = new DataTable();
                    dafa = new SqlDataAdapter(cmdfa);

                    dafa.Fill(dtfa);
                    if (dtfa.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtfa.Rows.Count; k++)
                        {
                            count = k + 1;
                            objtax = new capitalizationMaker();
                            objtax.indextax = count;
                            objtax.TaxName = dtfa.Rows[k]["tax_name"].ToString();
                            // objtax.TaxSubTypeName = dtfa.Rows[k]["taxsubtype_name"].ToString();
                            objtax.Invoicetaxamount = Convert.ToDecimal(dtfa.Rows[k]["invoicetax_amount"].ToString());
                            objtax.invoiceno = dtfa.Rows[k]["invoice_no"].ToString();
                            objtax.Invoicetaxglno = dtfa.Rows[k]["invoicetax_gl_no"].ToString();
                            objtax.Already_Capiptal = sample;
                            // objtax.Cap_Now = sample;
                            objtax.Already_Exp = sample;
                            // objtax.Exp_Now = sample;
                            objtax.Cap_Now = Convert.ToDecimal(string.IsNullOrEmpty(dtfa.Rows[k]["capitalised"].ToString()) ? "0" : dtfa.Rows[k]["capitalised"].ToString());
                            objtax.Exp_Now = Convert.ToDecimal(string.IsNullOrEmpty(dtfa.Rows[k]["inputcredit"].ToString()) ? "0" : dtfa.Rows[k]["inputcredit"].ToString());
                            objtax.provider_location = Convert.ToString(string.IsNullOrEmpty(dtfa.Rows[k]["provider_location"].ToString()) ? "-" : dtfa.Rows[k]["provider_location"].ToString());
                            objtax.receiver_location = Convert.ToString(string.IsNullOrEmpty(dtfa.Rows[k]["receiver_location"].ToString()) ? "-" : dtfa.Rows[k]["receiver_location"].ToString());
                            objtax.Balance = sample;
                            objtax.invoicegid = invgid.ToString();
                            objtaxlist.Add(objtax);


                            string[,] coltemp = new string[,]
                        {
                            {"exp_index",count.ToString()},
                            {"exp_invoicegid",invgid.ToString()},
                            {"exp_invoiceno",objtax.invoiceno},
                            {"exp_glcode", objtax.Invoicetaxglno},
                            {"exp_taxtype",objtax.TaxName},
                            {"exp_taxamount",objtax.Invoicetaxamount.ToString()},
                            {"exp_al_cap",objtax.Already_Capiptal.ToString()},
                            {"exp_cap_now",objtax.Cap_Now.ToString()},
                            {"exp_al_expense",objtax.Already_Exp.ToString()},
                            {"exp_expense_now",objtax.Exp_Now.ToString()},
                            {"exp_balance",objtax.Balance.ToString()},
                            {"exp_status","1"},
                            {"exp_isremoved","N"}
                        };
                            TaxResult = commfa.InsertCommon(coltemp, "fa_tmp_texpensedet");

                        }
                        objtax.Addtaxlist = objtaxlist;
                    }
                    else
                    {
                        objtax.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();

                    }
                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objtax.Addtaxlist;
        }


        private string deletetax(int id)
        {

            int res = 0;
            try
            {
                SqlCommand cmddel = new SqlCommand("pr_ifams_deletetax", confa);
                cmddel.CommandType = CommandType.StoredProcedure;
                cmddel.Parameters.AddWithValue("@ingid", id);
                cmddel.Parameters.AddWithValue("@status", "1");
                confa.Open();
                res = cmddel.ExecuteNonQuery();
                confa.Close();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                res = 0;
            }

            return res.ToString();
        }

        public List<capitalizationMaker> UpdateIndexDetailstax(capitalizationMaker tax)
        {
            List<capitalizationMaker> objlist = new List<capitalizationMaker>();
            capitalizationMaker objtax = new capitalizationMaker();
            string taxres = string.Empty;
            try
            {
                string[,] colval = new string[,]
                {
                    {"exp_al_cap",tax.Already_Capiptal.ToString()},
                    {"exp_cap_now",tax.Cap_Now.ToString()},
                    {"exp_al_expense",tax.Already_Exp.ToString()},
                    {"exp_expense_now",tax.Exp_Now.ToString()},
                    {"exp_glcode",tax.Invoicetaxglno.ToString()},
                    {"exp_balance",tax.Balance.ToString()}
                };

                string[,] colwhr = new string[,]
                {
                   {"exp_index",tax.indextax.ToString()},
                   {"exp_invoicegid",tax.invoicegid.ToString()}
                };

                taxres = commfa.UpdateCommon(colval, colwhr, "fa_tmp_texpensedet");

                if (taxres == "Success")
                {
                    GetfaConnection();
                    cmdfa = new SqlCommand("pr_ifams_trn_updatepotax", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@invoicegid", tax.invoicegid);
                    cmdfa.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid().ToString());
                    cmdfa.ExecuteNonQuery();

                    GetfaConnection();
                    cmdfa = new SqlCommand("pr_ifams_getexpdetail", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@inv_gid", tax.invoicegid);
                    cmdfa.Parameters.AddWithValue("@status", "1");
                    dtfa = new DataTable();
                    dafa = new SqlDataAdapter(cmdfa);
                    dafa.Fill(dtfa);
                    if (dtfa.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtfa.Rows.Count; i++)
                        {
                            objtax = new capitalizationMaker();
                            objtax.indextax = Convert.ToInt32(dtfa.Rows[i]["exp_index"].ToString());
                            objtax.invoiceno = dtfa.Rows[i]["exp_invoiceno"].ToString();
                            objtax.Invoicetaxglno = dtfa.Rows[i]["exp_glcode"].ToString();
                            objtax.TaxName = dtfa.Rows[i]["exp_taxtype"].ToString();
                            objtax.Invoicetaxamount = Convert.ToDecimal(dtfa.Rows[i]["exp_taxamount"].ToString());
                            objtax.Already_Capiptal = Convert.ToDecimal(dtfa.Rows[i]["exp_al_cap"].ToString());
                            objtax.Cap_Now = Convert.ToDecimal(dtfa.Rows[i]["exp_cap_now"].ToString());
                            objtax.Already_Exp = Convert.ToDecimal(dtfa.Rows[i]["exp_al_expense"].ToString());
                            objtax.Exp_Now = Convert.ToDecimal(dtfa.Rows[i]["exp_expense_now"].ToString());
                            objtax.Balance = Convert.ToDecimal(dtfa.Rows[i]["exp_balance"].ToString());
                            objtax.invoicegid = dtfa.Rows[i]["exp_invoicegid"].ToString();
                            objtax.provider_location = string.IsNullOrEmpty(dtfa.Rows[i]["provider_location"].ToString()) ? "" : dtfa.Rows[i]["provider_location"].ToString();
                            objtax.receiver_location = string.IsNullOrEmpty(dtfa.Rows[i]["receiver_location"].ToString()) ? "" : dtfa.Rows[i]["receiver_location"].ToString();  
                            objlist.Add(objtax);
                        }
                        objtax.Addtaxlist = objlist;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetfaConnection();
            }
            return objtax.Addtaxlist;
        }

        public DataTable Getexpsum(string invoice, string status)
        {
            decimal expsum = Convert.ToDecimal("0.00");
            decimal exp_exp = Convert.ToDecimal("0.00");
            DataTable dtfa = new DataTable();
            try
            {

                cmdfa = new SqlCommand("pr_ifams_getexpsum", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invgid", invoice);
                cmdfa.Parameters.AddWithValue("@status", status);
                dafa = new SqlDataAdapter(cmdfa);
                dafa.Fill(dtfa);
                //if(dtfa.Rows.Count>0)
                //{
                //    expsum = Convert.ToDecimal(string.IsNullOrEmpty(dtfa.Rows[0]["exp_cap_now"].ToString()) ? "0" : dtfa.Rows[0]["exp_cap_now"].ToString());
                //    exp_exp =Convert.ToDecimal(string.IsNullOrEmpty(dtfa.Rows[0]["exp_expense_now"].ToString())?"0" :dtfa.Rows[0]["exp_expense_now"].ToString());
                //}
                //confa.Open();
                //expsum =(decimal) cmdfa.ExecuteScalar();
                //confa.Close();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dtfa;
        }

        public List<capitalizationMaker> Updatefulltax(int invgid, decimal amt, decimal dis, decimal tax, decimal others, decimal total)
        {
            capitalizationMaker objtax = new capitalizationMaker();
            List<capitalizationMaker> objtaxlist = new List<capitalizationMaker>();
            try
            {
                string[,] Taxcol = new string[,]
                {
                   {"podetails_amount",amt.ToString()},
                   {"podetails_tax1",tax.ToString()},
                   {"podetails_discount",dis.ToString()},
                   {"podetails_others",others.ToString()},
                   {"podetails_Totalamt",total.ToString()}
                };
                string[,] taxwhr = new string[,]
                {
                    {"podetails_invoice_gid",invgid.ToString()}
                };
                string result = commfa.UpdateCommon(Taxcol, taxwhr, "fa_tmp_podetails");
                if (result == "Success")
                {
                    GetfaConnection();
                    cmdfa = new SqlCommand("pr_ifams_trn_Getmodpodetailsnew", confa);
                    cmdfa.CommandType = CommandType.StoredProcedure;
                    cmdfa.Parameters.AddWithValue("@ingid", invgid);
                    cmdfa.Parameters.AddWithValue("@user", comuidfa.GetLoginUserGid().ToString());
                    dafa = new SqlDataAdapter(cmdfa);
                    dtfa = new DataTable();
                    dafa.Fill(dtfa);
                    if (dtfa.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtfa.Rows.Count; i++)
                        {
                            objtax = new capitalizationMaker();
                            objtax.index = Convert.ToInt32(dtfa.Rows[i]["podetails_index"]);
                            objtax.invoiceno = dtfa.Rows[i]["podetails_invoiceno"].ToString();
                            objtax.ponumber = dtfa.Rows[i]["podetails_ponumber"].ToString();
                            objtax.AssetCode = dtfa.Rows[i]["podetails_assetCode"].ToString();
                            objtax.AssetName = dtfa.Rows[i]["podetails_assetname"].ToString();
                            objtax.LocationCode = dtfa.Rows[i]["podetails_location"].ToString();
                            objtax.locationName = dtfa.Rows[i]["podetails_locationname"].ToString();
                            objtax.GRNNo = dtfa.Rows[i]["podetails_grnno"].ToString();
                            objtax.GRNDate = dtfa.Rows[i]["podetails_grndate"].ToString();
                            objtax.glcode = dtfa.Rows[i]["podetails_assetglcode"].ToString();
                            objtax.Amount = Convert.ToDecimal(dtfa.Rows[i]["podetails_amount"].ToString());
                            objtax.Quantity = Convert.ToDecimal(dtfa.Rows[i]["podetails_poQty"].ToString());
                            objtax.UOM = dtfa.Rows[i]["podetails_uom"].ToString();
                            objtax.Discount = Convert.ToDecimal(dtfa.Rows[i]["podetails_discount"].ToString());
                            objtax.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax1"].ToString());
                            // objdetail.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax2"].ToString());
                            objtax.othres = Convert.ToDecimal(dtfa.Rows[i]["podetails_others"].ToString());
                            objtax.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["podetails_Totalamt"].ToString());
                            objtax.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["podetails_poRevQty"].ToString());
                            //    objdetail.Description = dtfa.Rows[i]["cap_Descr"].ToString();
                            objtax.DCNo = dtfa.Rows[i]["podetails_DCno"].ToString();
                            objtax.invoicegid = dtfa.Rows[i]["podetails_invoice_gid"].ToString();
                            objtax.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["podetails_invoiceamt"].ToString());
                            objtax.AssetCategory = dtfa.Rows[i]["Assetcategory"].ToString();
                            objtax.AssetSubcategpry = dtfa.Rows[i]["AssetSubcategory"].ToString();
                            objtax.shipmentType = dtfa.Rows[i]["ShipmentType"].ToString();
                            // objdetail.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["cap_Invoiceamt"].ToString());
                            objtax.ddlSubcategory = new SelectList(GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry", selectedValue: dtfa.Rows[i]["Subcategorygid"]);
                            // objdetail.ddlbranch = new SelectList(GetBranchandlocation(), "ddllocaid", "ddllocacode", selectedValue: dtfa.Rows[i]["loc_gid"]);
                            objtax.Assetmfn = dtfa.Rows[i]["podetails_assmfn"].ToString();
                            objtax.AssetSrlno = dtfa.Rows[i]["podetails_asssrlno"].ToString();
                            objtax.pohgid = Convert.ToInt32(dtfa.Rows[i]["podetails_po_gid"].ToString());
                            objtax.Grn_gid = Convert.ToInt32(dtfa.Rows[i]["Grn_gid"].ToString());
                            objtax.Grn_detgid = Convert.ToInt32(dtfa.Rows[i]["podetials_Grn_detgid"].ToString());
                            objtaxlist.Add(objtax);
                        }
                        objtax.Itemlevellist = objtaxlist;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetfaConnection();
            }
            return objtax.Itemlevellist;
        }




        public IEnumerable<EOW_PO> GetPoDetailssingledata(string id)
        {
            EOW_PO objModel;
            DataTable dt = new DataTable();
            List<EOW_PO> objOrgType = new List<EOW_PO>();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                GetfaConnection();
                cmd = new SqlCommand("pr_eow_sup_getpodetails", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@POid", SqlDbType.Int).Value = Convert.ToInt32(id);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Singleponoifams";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_PO();

                    objModel.PONo = dt.Rows[i]["poheader_pono"].ToString();
                    objModel.POdate = dt.Rows[i]["poheader_date"].ToString();
                    string open = dt.Rows[i]["poheader_isclosed"].ToString();
                    if (open == "N")
                    {
                        open = "Open";
                    }
                    else
                    {
                        open = "Close";
                    }
                    objModel.POStatus = open;
                    objModel.POGid = dt.Rows[i]["poheader_gid"].ToString();
                    objModel.POAmount = dt.Rows[i]["poheader_over_total"].ToString();
                    objModel.POApprovedStatus = dt.Rows[i]["status_name"].ToString();

                    objModel.POBalamount = dt.Rows[i]["poheader_over_total"].ToString();
                    objModel.POUtilizedamount = dt.Rows[i]["poheader_over_total"].ToString();
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
                confa.Close();
                da.Dispose();
            }
            return objOrgType;
        }




        public IEnumerable<EOW_PO> GetPoDetailsitem(string ecfid, string invoiceid, string id)
        {
            List<EOW_PO> objExpense = new List<EOW_PO>();
            decimal poampamt = 0;
            decimal poinvoicedampamt = 0;
            decimal porecdampamt = 0;
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {

                EOW_PO objModel;
                DataSet dt = new DataSet();

                GetfaConnection();
                cmd = new SqlCommand("pr_eow_trn_Getpomappeddetails", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = invoiceid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "POmapped";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    string amt = "";
                    if (poampamt == 0)
                    {
                        amt = Convert.ToString(dt.Tables[0].Rows[i]["invoicepoitem_amount"].ToString());
                        if (amt != "")
                        {
                            poampamt = Convert.ToDecimal(amt);
                        }
                    }
                    else
                    {
                        amt = Convert.ToString(dt.Tables[0].Rows[i]["invoicepoitem_amount"].ToString());
                        if (amt != "")
                        {
                            poampamt = poampamt + Convert.ToDecimal(amt);
                        }

                    }
                    objModel = new EOW_PO();
                    objModel.POGid = Convert.ToString(dt.Tables[0].Rows[i]["podetails_gid"].ToString());
                    objModel.POserprogid = Convert.ToString(dt.Tables[0].Rows[i]["prodservice_gid"].ToString());
                    objModel.POserprocode = Convert.ToString(dt.Tables[0].Rows[i]["prodservice_code"].ToString());
                    objModel.POserprodesc = Convert.ToString(dt.Tables[0].Rows[i]["prodservice_name"].ToString());
                    objModel.POassetgl = Convert.ToString("1234");
                    objModel.POorderqty = Convert.ToString(dt.Tables[0].Rows[i]["podetails_qty"].ToString());
                    objModel.POreceivedqty = Convert.ToString(dt.Tables[0].Rows[i]["grninwrddet_reced_qty"].ToString());
                    string invoicedqty = Convert.ToString(dt.Tables[0].Rows[i]["POinvoiceqty"].ToString());
                    if (invoicedqty == "")
                    {
                        invoicedqty = "0";
                    }
                    string invoicedqtymain = "0";
                    if (dt.Tables[1].Rows.Count > i)
                    {
                        invoicedqtymain = Convert.ToString(dt.Tables[1].Rows[i]["POinvoiceqty"].ToString());
                        if (invoicedqtymain == "")
                        {
                            invoicedqtymain = "0";
                        }
                    }
                    objModel.POcurrentqty = invoicedqty;
                    objModel.POinvoiceqty = invoicedqtymain;

                    decimal tootalamts = 0;
                    tootalamts = Convert.ToDecimal(invoicedqtymain) * Convert.ToDecimal(dt.Tables[0].Rows[i]["podetails_unitprice"].ToString());
                    objModel.POinvoiceqtyamt = tootalamts.ToString();

                    if (poinvoicedampamt == 0)
                    {
                        poinvoicedampamt = Convert.ToDecimal(tootalamts);
                    }
                    else
                    {
                        poinvoicedampamt = poinvoicedampamt + Convert.ToDecimal(tootalamts);
                    }
                    decimal tootalamtsre = 0;
                    tootalamtsre = Convert.ToDecimal(dt.Tables[0].Rows[i]["POreceivedamt"].ToString());
                    if (porecdampamt == 0)
                    {
                        porecdampamt = Convert.ToDecimal(tootalamtsre);
                    }
                    else
                    {
                        porecdampamt = porecdampamt + Convert.ToDecimal(tootalamtsre);
                    }
                    objModel.POreceivedqtyamt = tootalamtsre.ToString();
                    objModel.POrate = Convert.ToString(dt.Tables[0].Rows[i]["podetails_unitprice"].ToString());
                    string invoicedamont = Convert.ToString(dt.Tables[0].Rows[i]["invoicepoitem_amount"].ToString());
                    if (invoicedamont == "")
                    {
                        invoicedamont = "0";
                    }
                    objModel.POAmount = invoicedamont;
                    objExpense.Add(objModel);
                }
                decimal diff = 0;
                diff = porecdampamt - poinvoicedampamt;
                HttpContext.Current.Session["Totalporeceivedamt"] = porecdampamt.ToString();
                HttpContext.Current.Session["Totalpoinvoicedsamt"] = poinvoicedampamt.ToString();
                HttpContext.Current.Session["Totalpomapamt"] = poampamt.ToString();
                HttpContext.Current.Session["Totalpobalacedamt"] = diff.ToString();
                return objExpense;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objExpense;
            }
            finally
            {
                confa.Close();
                da.Dispose();
            }
        }

        public capitalizationMaker Getpodetailstoedit(int grndetgid, int invid)
        {
            capitalizationMaker objpo = new capitalizationMaker();
            DataTable dtfa = new DataTable();
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@grnid", SqlDbType.VarChar).Value = grndetgid;
                // cmdfa.Parameters.Add("@poindex", SqlDbType.VarChar).Value = poindexgid;
                cmdfa.Parameters.Add("@invid", SqlDbType.VarChar).Value = invid;
                cmdfa.Parameters.Add("@for", SqlDbType.VarChar).Value = "details";
                dafa = new SqlDataAdapter(cmdfa);
                dafa.Fill(dtfa);
                for (int i = 0; i < dtfa.Rows.Count; i++)
                {
                    objpo.index = Convert.ToInt32(dtfa.Rows[i]["podetails_index"]);
                    objpo.invoiceno = dtfa.Rows[i]["podetails_invoiceno"].ToString();
                    objpo.ponumber = dtfa.Rows[i]["podetails_ponumber"].ToString();
                    objpo.AssetCode = dtfa.Rows[i]["podetails_assetCode"].ToString();
                    objpo.AssetName = dtfa.Rows[i]["podetails_assetname"].ToString();
                    objpo.LocationCode = dtfa.Rows[i]["podetails_location"].ToString();
                    objpo.locationName = dtfa.Rows[i]["podetails_locationname"].ToString();
                    objpo.GRNNo = dtfa.Rows[i]["podetails_grnno"].ToString();
                    objpo.GRNDate = dtfa.Rows[i]["podetails_grndate"].ToString();
                    objpo.glcode = dtfa.Rows[i]["podetails_assetglcode"].ToString();
                    objpo.Amount = Convert.ToDecimal(dtfa.Rows[i]["podetails_amount"].ToString());
                    objpo.Quantity = Convert.ToDecimal(dtfa.Rows[i]["podetails_poQty"].ToString());
                    objpo.UOM = dtfa.Rows[i]["podetails_uom"].ToString();
                    objpo.Discount = Convert.ToDecimal(dtfa.Rows[i]["podetails_discount"].ToString());
                    objpo.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax1"].ToString());
                    objpo.othres = Convert.ToDecimal(dtfa.Rows[i]["podetails_others"].ToString());
                    objpo.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["podetails_Totalamt"].ToString());
                    objpo.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["podetails_poRevQty"].ToString());
                    objpo.DCNo = dtfa.Rows[i]["podetails_DCno"].ToString();
                    objpo.invoicegid = dtfa.Rows[i]["podetails_invoice_gid"].ToString();
                    objpo.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["podetails_invoiceamt"].ToString());
                    objpo.AssetCategory = dtfa.Rows[i]["Assetcategory"].ToString();
                    objpo.AssetSubcategpry = dtfa.Rows[i]["AssetSubcategory"].ToString();
                    objpo.shipmentType = dtfa.Rows[i]["ShipmentType"].ToString();
                    objpo.ddlSubcategory = new SelectList(GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry", selectedValue: dtfa.Rows[i]["Subcategorygid"]);
                    objpo.Assetmfn = dtfa.Rows[i]["podetails_assmfn"].ToString();
                    objpo.AssetSrlno = dtfa.Rows[i]["podetails_asssrlno"].ToString();
                    objpo.pohgid = Convert.ToInt32(dtfa.Rows[i]["podetails_po_gid"].ToString());
                    objpo.Grn_gid = Convert.ToInt32(dtfa.Rows[i]["Grn_gid"].ToString());
                    objpo.DEPRATE = Convert.ToString(dtfa.Rows[i]["asset_dep_rate"].ToString());
                    objpo.br_st_date = Convert.ToString(dtfa.Rows[i]["branch_start_date"].ToString());
                    objpo.lpm_st_date = Convert.ToString(dtfa.Rows[i]["branch_lease_start_date"].ToString());
                    objpo.lpm_ed_date = Convert.ToString(dtfa.Rows[i]["branch_lease_end_date"].ToString());
                    objpo.Description = dtfa.Rows[i]["podetails_desc"].ToString();
                    objpo.podetgid = Convert.ToInt32(string.IsNullOrEmpty(dtfa.Rows[i]["podetails_podetgid"].ToString()) ? 0 : dtfa.Rows[i]["podetails_podetgid"]);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objpo;
        }

        public string GetCatNSubcatId(string subcat)
        {
            string catnSbcat = string.Empty;
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@for", SqlDbType.VarChar).Value = "catnsubcat";
                cmdfa.Parameters.Add("@invid", SqlDbType.VarChar).Value = subcat;
                catnSbcat = Convert.ToString(cmdfa.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return catnSbcat;
        }


        public string GetDepNGLcode(string values)
        {
            string catnSbcat = string.Empty;
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@for", SqlDbType.VarChar).Value = "glndep";
                cmdfa.Parameters.Add("@invid", SqlDbType.VarChar).Value = values;
                catnSbcat = Convert.ToString(cmdfa.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return catnSbcat;
        }

        public string Verify_details(string[] values, string invid)
        {

            string success = "SUCCESS";
            try
            {
                for (int i = 0; i < values.Length; i++)
                {
                    string id1 = values[i];
                    if (id1 != string.Empty && id1 != "undefined ")
                        for (int length = (values.Length) - 1; length >= 0; length--)
                        {
                            string id2 = values[length];
                            if (id2 != string.Empty && id2 != "undefined ")
                                if (success == "SUCCESS")
                                {
                                    capitalizationMaker obj1 = Getpodetailstoedit(Convert.ToInt32(id1), Convert.ToInt32(invid));
                                    capitalizationMaker obj2 = Getpodetailstoedit(Convert.ToInt32(id2), Convert.ToInt32(invid));
                                    //if (obj1.GRNDate == obj2.GRNDate && obj1.LocationCode == obj2.LocationCode && obj1.TotalAmount == obj2.TotalAmount && obj1.AssetCode == obj2.AssetCode)
                                    //    success = "SUCCESS";
                                    if (obj1.podetgid == 0) { success = "Please check the podetails"; }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(obj1.br_st_date))
                                            success = "branch_date";
                                        else
                                            success = "SUCCESS";
                                    }
                                }
                        }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return success;

        }


        public string GetInvoiceQty(string invoicegid = null)
        {
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_rpt_SplitReport", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmdfa.Parameters.Add("@action", SqlDbType.VarChar).Value = "getinvoiceqty";
                string result = Convert.ToString(cmdfa.ExecuteScalar());

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "1";
            }
        }

        public string GetGLDetails(string catgid)
        {
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.Add("@assetid", SqlDbType.Int).Value = catgid;
                cmdfa.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "getGLforCat";
                string result = Convert.ToString(cmdfa.ExecuteScalar());

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "1";
            }
        }

        public capitalizationMaker RefreshCapitalisationdetails(string invoicegid = null)
        {
            string Result = string.Empty;
            capitalizationMaker objdetail = new capitalizationMaker();
            List<capitalizationMaker> objgetasset = new List<capitalizationMaker>();
            string code = string.Empty,
             category = string.Empty,
             Subcategory = string.Empty,
             glcode = string.Empty;
            try
            {
                GetfaConnection();
                cmdfa = new SqlCommand("pr_ifams_trn_GetmodpodetailsRefresh", confa);
                cmdfa.CommandType = CommandType.StoredProcedure;
                cmdfa.Parameters.AddWithValue("@invoiceno", invoicegid);
                cmdfa.Parameters.AddWithValue("@user", comuidfa.GetLoginUserGid().ToString());
                dafa = new SqlDataAdapter(cmdfa);
                dtfa = new DataTable();
                dafa.Fill(dtfa);
                if (dtfa.Rows.Count > 0)
                {
                    for (int i = 0; i < dtfa.Rows.Count; i++)
                    {
                        objdetail = new capitalizationMaker();
                        objdetail.index = Convert.ToInt32(dtfa.Rows[i]["podetails_index"]);
                        objdetail.invoiceno = dtfa.Rows[i]["podetails_invoiceno"].ToString();
                        objdetail.ponumber = dtfa.Rows[i]["podetails_ponumber"].ToString();
                        objdetail.AssetCode = dtfa.Rows[i]["podetails_assetCode"].ToString();
                        objdetail.AssetName = dtfa.Rows[i]["podetails_assetname"].ToString();
                        objdetail.LocationCode = dtfa.Rows[i]["podetails_location"].ToString();
                        objdetail.locationName = dtfa.Rows[i]["podetails_locationname"].ToString();
                        objdetail.GRNNo = dtfa.Rows[i]["podetails_grnno"].ToString();
                        objdetail.GRNDate = dtfa.Rows[i]["podetails_grndate"].ToString();
                        objdetail.glcode = dtfa.Rows[i]["podetails_assetglcode"].ToString();
                        objdetail.Amount = Convert.ToDecimal(dtfa.Rows[i]["podetails_amount"].ToString());
                        objdetail.Quantity = Convert.ToDecimal(dtfa.Rows[i]["podetails_poQty"].ToString());
                        objdetail.UOM = dtfa.Rows[i]["podetails_uom"].ToString();
                        objdetail.Discount = Convert.ToDecimal(dtfa.Rows[i]["podetails_discount"].ToString());
                        //   objdetail.Tax1 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax1"].ToString());
                        objdetail.SplitedTaxAmnt = string.IsNullOrEmpty(dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString()) ? "0" : dtfa.Rows[i]["SplitedInvTaxAmnt"].ToString();
                        // objdetail.Tax2 = Convert.ToDecimal(dtfa.Rows[i]["podetails_tax2"].ToString());
                        objdetail.othres = Convert.ToDecimal(dtfa.Rows[i]["podetails_others"].ToString());
                        objdetail.TotalAmount = Convert.ToDecimal(dtfa.Rows[i]["podetails_Totalamt"].ToString());
                        objdetail.Receivedqty = Convert.ToDecimal(dtfa.Rows[i]["podetails_poRevQty"].ToString());
                        //    objdetail.Description = dtfa.Rows[i]["cap_Descr"].ToString();
                        objdetail.DCNo = dtfa.Rows[i]["podetails_DCno"].ToString();
                        objdetail.invoicegid = dtfa.Rows[i]["podetails_invoice_gid"].ToString();
                        objdetail.invoiceamount = Convert.ToDecimal(dtfa.Rows[i]["podetails_invoiceamt"].ToString());
                        objdetail.AssetCategory = dtfa.Rows[i]["Assetcategory"].ToString();
                        objdetail.Assetcategoryid = Convert.ToInt32(dtfa.Rows[i]["asset_category_gid"].ToString());
                        objdetail.AssetSubcategpry = dtfa.Rows[i]["AssetSubcategory"].ToString();
                        objdetail.shipmentType = dtfa.Rows[i]["ShipmentType"].ToString();
                        objdetail.Grn_detgid = Convert.ToInt32(dtfa.Rows[i]["podetials_Grn_detgid"].ToString());
                        objdetail.ddlCategory = new SelectList(GetAssetCategory(), "Assetcategoryid", "Assetcategpry");
                        objdetail.ddlSubcategory = new SelectList(GetAssetSubCategory(objdetail.Assetcategoryid.ToString()), "Assetsubcategoryid", "AssetSubcategpry");
                        // objdetail.ddlbranch = new SelectList(GetBranchandlocation(), "ddllocaid", "ddllocacode", selectedValue: dtfa.Rows[i]["loc_gid"]);
                        objdetail.Assetmfn = dtfa.Rows[i]["podetails_assmfn"].ToString();
                        objdetail.AssetSrlno = dtfa.Rows[i]["podetails_asssrlno"].ToString();
                        objdetail.pohgid = Convert.ToInt32(dtfa.Rows[i]["podetails_po_gid"].ToString());
                        objdetail.Grn_gid = Convert.ToInt32(dtfa.Rows[i]["Grn_gid"].ToString());
                        objdetail.DEPRATE = dtfa.Rows[i]["asset_dep_rate"].ToString();
                        objgetasset.Add(objdetail);


                    }
                    objdetail.Itemlevellist = objgetasset;
                }
            }
            catch (Exception ex) { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            finally { GetfaConnection(); }
            return objdetail;
        }

        public List<SelectListItem> GetAllHsn()
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "getcombo");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["hsn_code"].ToString(),
                        Value = sdr["hsn_gid"].ToString(),

                    });
                }
                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                confa.Close();
            }
            //  throw new NotImplementedException();
        }


        public string Gethsndesc(string hsngid)
        {
            string hsn_desc = string.Empty;
            try
            {
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "Getdesc");
                cmd.Parameters.AddWithValue("@hsn_id", hsngid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        hsn_desc = hsn_desc + dt.Rows[i]["hsn_description"].ToString();
                    }
                }

            }

            catch (Exception ex)
            {

            }
            return hsn_desc;
        }
        public string GethsndescASSET(string Exphsn)
        {
            string hsn_desc = string.Empty;
            try
            {
                GetfaConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", confa);
                cmd.Parameters.AddWithValue("@action", "MULTIHSNDESCASSET");
                cmd.Parameters.AddWithValue("@hsn_id", Exphsn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows.Count > 0)
                    {
                        hsn_desc = hsn_desc + dt.Rows[i]["hsn_description"].ToString();
                    }
                }

            }

            catch (Exception ex)
            {

            }
            return hsn_desc;
        }
        //public List<capitalizationMaker> GetECFdetailsGRN()
        //{
        //    List<capitalizationMaker> objcpl = new List<capitalizationMaker>();
        //    capitalizationMaker obj = new capitalizationMaker();
        //    try
        //    {
        //        GetfaConnection();
        //        //   cmdfa = new SqlCommand("pr_ifams_getecfdetails", confa);
        //        cmdfa = new SqlCommand("pr_ifams_getinvoiceNew_GRN", confa);
        //        cmdfa.CommandType = CommandType.StoredProcedure;
        //        cmdfa.Parameters.AddWithValue("@action", "GRN");
        //        dafa = new SqlDataAdapter(cmdfa);
        //        dtfa = new DataTable();
        //        dafa.Fill(dtfa);
        //        if (dtfa.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtfa.Rows.Count; i++)
        //            {
        //                obj = new capitalizationMaker();
        //                obj.Ecfgidg = Convert.ToInt32(dtfa.Rows[i]["Ecfgid"].ToString());
        //                obj.EcfNog = dtfa.Rows[i]["EcfNo"].ToString();
        //                obj.Ecfdateg = dtfa.Rows[i]["EcfDate"].ToString();
        //                obj.EcfAmountg = dtfa.Rows[i]["EcfAmount"].ToString();
        //                obj.invoicegidg = dtfa.Rows[i]["Invoicegid"].ToString();
        //                obj.invoicenog = dtfa.Rows[i]["InvoiceNo"].ToString();
        //                obj.invoiceamountg = Convert.ToDecimal(dtfa.Rows[i]["InvoiceAmount"].ToString());
        //                obj.Vendorg = dtfa.Rows[i]["VendorName"].ToString();
        //                cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
        //                cmdfa.CommandType = CommandType.StoredProcedure;
        //                cmdfa.Parameters.AddWithValue("@for", "qty");
        //                cmdfa.Parameters.AddWithValue("@invid", obj.invoicegid);
        //                string MATCH = Convert.ToString(cmdfa.ExecuteScalar());
        //                if (MATCH == "doesnt match" || string.IsNullOrEmpty(MATCH))
        //                {
        //                    objcpl.Add(obj);
        //                }
        //            }
        //        }
        //        GetfaConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return objcpl;
        //}

        //public List<capitalizationMaker> GetECFdetailsBranch()
        //{
        //    List<capitalizationMaker> objcpl = new List<capitalizationMaker>();
        //    capitalizationMaker obj = new capitalizationMaker();
        //    try
        //    {
        //        GetfaConnection();
        //        //   cmdfa = new SqlCommand("pr_ifams_getecfdetails", confa);
        //        cmdfa = new SqlCommand("pr_ifams_getinvoiceNew_GRN", confa);
        //        cmdfa.CommandType = CommandType.StoredProcedure;
        //        cmdfa.Parameters.AddWithValue("@action", "GRN");
        //        dafa = new SqlDataAdapter(cmdfa);
        //        dtfa = new DataTable();
        //        dafa.Fill(dtfa);
        //        if (dtfa.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtfa.Rows.Count; i++)
        //            {
        //                obj = new capitalizationMaker();
        //                obj.Ecfgidg = Convert.ToInt32(dtfa.Rows[i]["Ecfgid"].ToString());
        //                obj.EcfNog = dtfa.Rows[i]["EcfNo"].ToString();
        //                obj.Ecfdateg = dtfa.Rows[i]["EcfDate"].ToString();
        //                obj.EcfAmountg = dtfa.Rows[i]["EcfAmount"].ToString();
        //                obj.invoicegidg = dtfa.Rows[i]["Invoicegid"].ToString();
        //                obj.invoicenog = dtfa.Rows[i]["InvoiceNo"].ToString();
        //                obj.invoiceamountg = Convert.ToDecimal(dtfa.Rows[i]["InvoiceAmount"].ToString());
        //                obj.invoicedateg = dtfa.Rows[i]["InvoiceDate"].ToString();
        //                obj.Vendorg = dtfa.Rows[i]["VendorName"].ToString();
        //                cmdfa = new SqlCommand("pr_ifams_trn_teditlineitemdetails", confa);
        //                cmdfa.CommandType = CommandType.StoredProcedure;
        //                cmdfa.Parameters.AddWithValue("@for", "qty");
        //                cmdfa.Parameters.AddWithValue("@invid", obj.invoicegid);
        //                string MATCH = Convert.ToString(cmdfa.ExecuteScalar());
        //                if (MATCH == "doesnt match" || string.IsNullOrEmpty(MATCH))
        //                {
        //                    objcpl.Add(obj);
        //                }
        //            }
        //        }
        //        GetfaConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return objcpl;
        //}

    }
}