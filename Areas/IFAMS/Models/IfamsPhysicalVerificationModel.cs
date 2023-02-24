using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
namespace IEM.Areas.IFAMS.Models
{
    public class IfamsPhysicalVerificationModel
    {

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();

        public string GetStatusexcel(string assetdata, string valid, string action)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_exceldataverification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@assetdata", SqlDbType.VarChar).Value = assetdata.ToString();
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
                GetOffConnection();
            }
            return result;
        }


        public DataTable GetLocFromYear(string Year)
        {
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@YearSearch", SqlDbType.VarChar).Value = "YS";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = Year.ToString();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                GetOffConnection();
            }
            return dt;
        }

        public int AsperFACount(string Location)
        {
            int result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FACount", SqlDbType.VarChar).Value = "FACount";
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = Location.ToString();
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }


        public int FARegister()
        {
            int result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FAYear", SqlDbType.VarChar).Value = "FARegister";
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }

        public long FARegisterWDV()
        {
            long result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FAYearWDV", SqlDbType.VarChar).Value = "FARegisterWDV";
                cmd.CommandTimeout = 1000;
                result = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                GetOffConnection();
            }
            return result;

        }

        public int NotinLoc(string Branch)
        {
            int result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SummaryLoc", SqlDbType.VarChar).Value = "NotinLoc";
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = Branch;
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }

        public string FAVerified()
        {
            string result;

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FAYearV", SqlDbType.VarChar).Value = "YVerified";
                dt.Load(cmd.ExecuteReader());
                result = dt.Rows[0][0] + "," + dt.Rows[0][1];

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "0";
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }


        public int FileByLoc(string FileName)
        {
            int result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Summary", SqlDbType.VarChar).Value = "FileByBranch";
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }


        public string FANotVerified()
        {
            string result = "0";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FAYear", SqlDbType.VarChar).Value = "FANotVerified";
                dt.Load(cmd.ExecuteReader());
                result = dt.Rows[0][0] + "," + dt.Rows[0][1];
                //result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "0";
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }


        public string PVNotVerified()
        {
            string result = "0";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FAYear", SqlDbType.VarChar).Value = "PVNotVerified";
                dt.Load(cmd.ExecuteReader());
                result = dt.Rows[0][0] + "," + dt.Rows[0][1];
                //  result = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "0";
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }





        public string CheckLocationPV(string FinancialYear, string LocationCode)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CheckLocation", SqlDbType.VarChar).Value = "Location";
                cmd.Parameters.Add("@assetloccode", SqlDbType.VarChar).Value = LocationCode;
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = FinancialYear;
                result = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }

        public string UpdatePV(string FinancialYear, string LocationCode)
        {



            string[,] colm = new string[,]
                    {
                            
                             {"pv_isremoved","Y"},   
                                              
                    };
            string[,] whrecol = new string[,]
                    {
                              {"pv_assetdet_assetbranch", LocationCode} ,    
                              {"pv_assetdet_financialyear", FinancialYear}                                           
                    };
            string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "fa_trn_tphysicalverfication");

            return updcmd;

        }

        public string GetPVStatus(string Barcode, string Location)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_PVexceldataverification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = Barcode;
                cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = Location;
                //cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
                result = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }





        public string GetAssetId(string barcode)
        {
            string AssetId = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GetAssetId", SqlDbType.VarChar).Value = "Barcode";
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = barcode;
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {

                    foreach (DataRow row in dt.Rows)
                    {

                        AssetId = row["assetdetails_assetdet_id"].ToString();

                    }
                }
                else
                {
                    AssetId = "";
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return AssetId.ToString();


        }




        public List<PhysicalVerificationAsset> PVImportFileDrop()
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FY", SqlDbType.VarChar).Value = "FinancialYear";
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    // obj.PV_gid =Convert.ToInt32( row["pv_gid"].ToString());
                    obj.FinancialYear = row["pv_assetdet_financialyear"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;



        }
        public List<PhysicalVerificationAsset> YearPVImportFileDrop(string year)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = "FileName";
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.FinancialYear = row["pv_assetdet_financialyear"].ToString();
                    obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;



        }


        public List<PhysicalVerificationAsset> AutoStatus(string term)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "AutoStatus";
                cmd.Parameters.Add("@AutoStatus", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.PVstatus = row["pv_assetdet_assetstatus"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;

        }


        public List<PhysicalVerificationAsset> AutoFileName(string term)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "AutoFileName";
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;

        }


        public List<PhysicalVerificationAsset> AutoBarcode(string term)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "Barcode";
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.Barcode = row["barcodedetials_bar_no"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;

        }


        public List<PhysicalVerificationAsset> AutoBranch(string term)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "BranchCode";
                cmd.Parameters.Add("@assetloccode", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.Branch = row["branch_code"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;

        }


        public List<PhysicalVerificationAsset> BranchPVImportFileDrop(string year, string SheetName)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = "Branch";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;
                cmd.Parameters.Add("@SheetName", SqlDbType.VarChar).Value = SheetName;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.SheetName = row["pv_assetdet_assetfilename"].ToString();
                    obj.Branch = row["pv_assetdet_assetbranch"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;



        }
        public List<PhysicalVerificationAsset> StatusPVImportFileDrop(string year, string SheetName, string Branch)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Status";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;
                cmd.Parameters.Add("@SheetName", SqlDbType.VarChar).Value = SheetName;
                cmd.Parameters.Add("@assetloccode", SqlDbType.VarChar).Value = Branch;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.Branch = row["pv_assetdet_assetbranch"].ToString();
                    obj.Status = row["pv_assetdet_assetstatus"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;



        }




        public List<PhysicalVerificationAsset> PVImportSerach()
        {
            string Year = "";
          //  string Status = "";
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PVSerach", SqlDbType.VarChar).Value = "PVSerach";
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.PV_gid = Convert.ToInt32(row["pv_gid"].ToString());
                    obj.FinancialYear = row["pv_assetdet_financialyear"].ToString();
                    Year = row["pv_assetdet_financialyear"].ToString();
                    obj.AssetID = row["pv_assetdet_assetid"].ToString();
                    obj.AssetCode = row["pv_assetdet_assetcode"].ToString();
                    obj.AssetDesc = row["pv_assetdet_assetdesc"].ToString();
                    obj.Status = row["pv_assetdet_assetstatus"].ToString();
                    obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    obj.Branch = row["pv_assetdet_assetbranch"].ToString();
                    obj.Barcode = row["pv_assetdet_barcode"].ToString();
                    obj.PVstatus = row["pv_status"].ToString();
                    obj.ReceiptDate = convertoDate(row["pv_assetdet_updatedon"].ToString());
                    obj.Asset_Category = row["Asset_Category"].ToString();
                    obj.Asset_Sub_Category = row["Asset_SubCategory"].ToString();
                    obj.wdv = Convert.ToDecimal(row["wdv"].ToString());
                    Model.Add(obj);
                }
                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetDetReport", SqlDbType.VarChar).Value = "AssetDetReport";
                dt1.Load(cmd.ExecuteReader());
                obj.CountFA = 0;
                foreach (DataRow row in dt1.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.PV_gid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.FinancialYear = Year;
                    obj.AssetID = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.Asset_Sub_Category = row["asset_description"].ToString();
                    obj.Status = "Not Verified";
                    obj.FileName = "";
                    obj.Branch = row["branch_code"].ToString();
                    obj.Barcode = row["barcodedetials_bar_no"].ToString();
                    obj.PVstatus = "";
                    obj.ReceiptDate = "";
                    obj.Asset_Category = row["assetcategory_name"].ToString();
                    obj.wdv = Convert.ToDecimal(row["wdv"].ToString());
                    obj.CountFA = dt1.Rows.Count;
                    Model.Add(obj);

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;



        }






        public string InsertPV(string AssetId, string LocationCode, string Status, string Financialyear, string FileName, string Barcode)
        {


            string msg = "";
            try
            {


                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdet_assetid", SqlDbType.VarChar).Value = AssetId;
                cmd.Parameters.Add("@assetloccode", SqlDbType.VarChar).Value = LocationCode;
                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {


                        string[,] col = new string[,]
	                        {
                                     {"pv_assetdet_gid",row["assetdetails_gid"].ToString()}, 

                                     {"Asset_Category",row["assetcategory_name"].ToString()}, 
                                     {"Asset_SubCategory",row["asset_description"].ToString()}, 

                                     {"pv_assetdet_assetid",row["assetdetails_assetdet_id"].ToString()}, 
                                     {"pv_assetdet_assetcode",row["asset_code"].ToString()},
                                     {"pv_assetdet_assetdesc",row["assetdetails_asset_description"].ToString()},
                                     {"pv_assetdet_assetstatus",Status.ToString()},
                                     {"pv_assetdet_assetfilename",FileName},
                                     {"pv_assetdet_assetbranch",row["branch_code"].ToString()},
                                     {"pv_assetdet_updatedon",convertoDate( Convert.ToString(objCommonIUD.GetCurrentDate()))},
                                     {"pv_assetdet_financialyear",Financialyear.ToString()},
                                     {"pv_assetdet_updatedby",objCmnFunctions.GetLoginUserGid().ToString()},
                                      {"pv_assetdet_barcode",row["barcodedetials_bar_no"].ToString()},
                                     {"pv_status","Verified"},
                                     {"pv_isremoved","N"},
                                 

	                      };
                        string inst = objCommonIUD.InsertCommon(col, "fa_trn_tphysicalverfication");



                    }

                }
                else
                {


                    string[,] col = new string[,]
	                        {
                                     {"pv_assetdet_gid",""}, 
                                     {"pv_assetdet_assetid",AssetId.ToString()}, 
                                     {"pv_assetdet_assetcode",""},
                                     {"pv_assetdet_assetdesc",""},
                                     {"pv_assetdet_assetstatus",Status.ToString()},
                                     {"pv_assetdet_assetfilename",FileName},
                                     {"pv_assetdet_assetbranch",LocationCode},
                                     {"pv_assetdet_updatedon",convertoDate(Convert.ToString(objCommonIUD.GetCurrentDate()))},
                                     {"pv_assetdet_financialyear",Financialyear.ToString()},
                                     {"pv_assetdet_updatedby",objCmnFunctions.GetLoginUserGid().ToString()},
                                     {"pv_assetdet_barcode",Barcode},
                                     {"pv_status","Verified"},
                                     {"pv_isremoved","N"},
                                 

	                      };
                    string inst = objCommonIUD.InsertCommon(col, "fa_trn_tphysicalverfication");


                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return msg;

        }

        public string SheetExists(string Financialyear, string FileName)
        {


            string SheetName = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@VerifyTemp", SqlDbType.VarChar).Value = FileName;
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = Financialyear;
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    SheetName = dt.Rows[0][0].ToString();
                }
                else
                {
                    SheetName = "";
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return SheetName.ToString();

        }




        public string convertoDate(string date1)
        {
            string convdate = string.Empty;
            try
            {
                string date2 = date1;

                if (date2.ToString() != "")
                {

                    DateTime convdate2 = Convert.ToDateTime(date2);
                    string format = "dd/MMM/yyyy";
                    convdate = convdate2.ToString(format);
                }
                else
                {
                    convdate = "";

                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            return convdate;
        }






        public List<PhysicalVerificationAsset> Summary(string year, string SheetName)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Summary", SqlDbType.VarChar).Value = "Summary";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = SheetName;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.AsPerFA = Convert.ToInt32(row["AsPerFA"].ToString());
                    obj.NoOfRecords = Convert.ToInt32(row["NoOfRecords"].ToString());
                    obj.NoOfFiles = Convert.ToInt32(row["NoOfFiles"].ToString());
                    obj.FAMatched = Convert.ToInt32(row["FAMatched"].ToString());
                    obj.FAMissMatched = Convert.ToInt32(row["LocationMissMatch"].ToString());
                    obj.NotinFA = Convert.ToInt32(row["NotinFA"].ToString());
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;


        }

        public List<PhysicalVerificationAsset> SummaryLoc(string year, string Location)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SummaryLoc", SqlDbType.VarChar).Value = "SummaryLoc";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = Location;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new PhysicalVerificationAsset();
                    obj.AsPerFA = Convert.ToInt32(row["AsPerFA"].ToString());
                    obj.NoOfRecords = Convert.ToInt32(row["NoOfRecords"].ToString());
                    obj.NoOfFiles = Convert.ToInt32(row["NoOfFiles"].ToString());
                    obj.FAMatched = Convert.ToInt32(row["FAMatched"].ToString());
                    obj.FAMissMatched = Convert.ToInt32(row["LocationMissMatch"].ToString());
                    obj.NotinFA = Convert.ToInt32(row["NotinFA"].ToString());
                    obj.NotVerfied = Convert.ToInt32(row["FinancialNotVerified"].ToString());
                    obj.TotalFileLoc = Convert.ToInt32(row["ByLocNoOfRecords"].ToString());
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;


        }

        public int NotVerified(string year)
        {
            List<PhysicalVerificationAsset> Model = new List<PhysicalVerificationAsset>();
            PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
            int NotVerified = 0;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NotV", SqlDbType.VarChar).Value = "NotV";
                cmd.Parameters.Add("@FinacialYear", SqlDbType.VarChar).Value = year;

                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {


                    NotVerified = Convert.ToInt32(row["NotVerified"].ToString());

                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return NotVerified;


        }


        public void inserttopv(string pv_file_name, string pv_fa_overall, string pv_fa_location, string pv_verified
       , string pv_mis_matched, string pv_not_verified_inloc, string pv_not_in_fa, string pv_file_count,
            string pv_fa_overall_wdv, string pv_fa_location_wdv, string pv_verified_wdv
       , string pv_mis_matched_wdv, string pv_not_verified_inloc_wdv, string pv_not_in_fa_wdv, string pv_file_count_wdv)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "truncate_temp";
                cmd.ExecuteNonQuery();

                string[,] col = new string[,]
	                        {
                            {"pv_file_name",pv_file_name}, 
                            {"pv_fa_overall",pv_fa_overall}, 
                            {"pv_fa_location",pv_fa_location},
                            {"pv_verified",pv_verified},
                            {"pv_mis_matched",pv_mis_matched},
                            {"pv_not_verified_inloc",pv_not_verified_inloc},
                            {"pv_not_in_fa",pv_not_in_fa},
                            {"pv_file_count",pv_file_count}, 
                            {"pv_fa_overall_wdv ",pv_fa_overall_wdv},
                            {"pv_fa_location_wdv ",pv_fa_location_wdv},
                            {"pv_verified_wdv ",pv_verified_wdv},
                            {"pv_mis_matched_wdv ",pv_mis_matched_wdv},
                            {"pv_not_verified_inloc_wdv ",pv_not_verified_inloc_wdv},
                            {"pv_not_in_fa_wdv ",pv_not_in_fa_wdv},
                            {"pv_file_count_wdv",pv_file_count_wdv},
                            
	                      };
                string inst = objCommonIUD.InsertCommon(col, "fa_tmp_tpvdetails");

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


        public DataTable GetReconDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_physicalverfication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "select_temp";
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public void PrintRecon()
        {
            try
            {
                string barcodepath = ConfigurationManager.AppSettings["Barcodepath"].ToString();
                DataTable dt = GetReconDetails();
                string Filename = dt.Rows[0]["pv_file_name"].ToString();
                string FAOverAll = dt.Rows[0]["pv_fa_overall"].ToString();
                string FALocation = dt.Rows[0]["pv_fa_location"].ToString();
                string Verified = dt.Rows[0]["pv_verified"].ToString();
                string MisMatched = dt.Rows[0]["pv_mis_matched"].ToString();
                string NotVerifiedInLoc = dt.Rows[0]["pv_not_verified_inloc"].ToString();
                string NotInFA = dt.Rows[0]["pv_not_in_fa"].ToString();
                string FileCount = dt.Rows[0]["pv_file_count"].ToString();
                string FAOverAll_wdv = dt.Rows[0]["pv_fa_overall_wdv"].ToString();
                string FALocation_wdv = dt.Rows[0]["pv_fa_location_wdv"].ToString();
                string Verified_wdv = dt.Rows[0]["pv_verified_wdv"].ToString();
                string MisMatched_wdv = dt.Rows[0]["pv_mis_matched_wdv"].ToString();
                string NotVerifiedInLoc_wdv = dt.Rows[0]["pv_not_verified_inloc_wdv"].ToString();
                string NotInFA_wdv = dt.Rows[0]["pv_not_in_fa_wdv"].ToString();
                string FileCount_wdv = dt.Rows[0]["pv_file_count_wdv"].ToString();

                using (StringWriter sw = new StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<div  ><table width='100%' cellspacing='0' cellpadding='2'>");
                        sb.Append("<tr><td ></td></tr><tr><td>");
                        sb.Append("");
                        sb.Append("</td><td align='left'> ");
                        sb.Append("<img height='25%' src='" + ConfigurationManager.AppSettings["ficc"].ToString() + "'/><table><tr><td align='center'> ");
                        sb.Append("<b>" + System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "</b>"); ;
                        sb.Append("</td></tr></table></td></tr>");
                        sb.Append("</table><font size='.5'><table><tr><td td align='center'>Corporate Office: Floor 5 & 6, B Wing, Supreme IT Park, Supreme City, Behind Lake Castle, Powai, Mumbai 400 076");
                        sb.Append("<br/>Regd Office: Venkat Tower, 307, Poonamallee High Road, Maduravoyal, Chennai 600095");
                        sb.Append("</td></tr><tr><td align='center' style='color:#000;height:10px;'>");
                        sb.Append("____________________________________________________________________________________________________________________________</td></tr></table></font></div>");
                        sb.Append("<table align='center'   width='100%' cellspacing='0' cellpadding='2'>");
                        sb.Append("<tr><td align = 'center'><b>Physical Verification Report (Recon)</b></td></tr>");
                        sb.Append("</table><br/><font size='1'>");
                        sb.Append(" <table border='.5' style='' width='100%' align='center'>");
                        sb.Append("<thead><tr bgcolor='rgb(51, 122, 183)' style='color: #FFF;'><th colspan = '2'><b>Description</b></th><th><b>Status</b></th><th><b>Formula</b></th><th><b>Count - 1</b></th><th><b>Count - 2</b></th><th><b> WD value</b></th></tr></thead>");
                        sb.Append("<tr><td colspan = '2'><span>As per FA Register (Overall):</span></td><td></td><td>(A+B+C)</td><td></td>");
                        sb.Append("<td align='right'><b><span style=' color: rgb(52, 108, 196);'><text>" + FAOverAll + "</text></span></b> </td>");
                        sb.Append("<td align='right'><b><span style=' color: rgb(52, 108, 196);'><text>" + FAOverAll_wdv + "</text></span></b> </td> </tr>");
                        sb.Append("<tr><td colspan = '2'><span>As per FA Register (Location):</span></td><td></td><td>(A+C)</td><td></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + FALocation + "</text></span></b></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + FALocation_wdv + "</text></span></b></td></tr>");
                        sb.Append("<tr><td colspan = '2'><span>ID Avlb in FA, Location matched:</span></td><td>Verified</td><td>A</td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + Verified + "</text></span></b></td><td></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + Verified_wdv + "</text></span></b></td>");
                        sb.Append("</tr><tr><td colspan = '2'><span>ID Avlb in FA , location mismatch:</span></td><td>Not Verified</td><td>B</td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + MisMatched + " </text></span></b></td><td></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + MisMatched_wdv + " </text></span></b></td>");
                        sb.Append("</tr><tr><td colspan = '2'><span>ID Avlb in FA, not Avlb in File:</span></td><td>Not Verified</td><td>C</td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + NotVerifiedInLoc + " </text></span></b></td>");
                        sb.Append("<td></td><td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + NotVerifiedInLoc_wdv + " </text></span></b></td>");
                        sb.Append("</tr><tr><td colspan = '2'><span>ID not Avlb n FA, Avlb in File:</span></td><td>NA</td><td>D</td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + NotInFA + "</text></span></b></td><td></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text>" + NotInFA_wdv + "</text></span></b></td>");
                        sb.Append("</tr><tr><td colspan = '2'><span>As per File Import (A+C):</span></td><td></td><td>(A+B+D)</td><td></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text id='NotinFA'>" + FileCount + "</text></span></b></td>");
                        sb.Append("<td align='right'><b><span style='color: rgb(52, 108, 196);'><text id='NotinFA'>" + FileCount_wdv + "</text></span></b></td>");
                        sb.Append("</tr></table></font><br/>");
                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        FileStream stream = new FileStream(barcodepath + "PVReport_" + Filename + ".pdf", FileMode.Create);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        string barcode = string.Empty;
                        string lpath = ConfigurationManager.AppSettings["Barcodepath"].ToString(); ;
                        string fileName = string.Empty;
                        barcode = lpath + "PVReport_" + Filename + ".pdf";
                        string Filpath = barcode;
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PVReport_" + Filename + ".pdf");
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment");
                        HttpContext.Current.Response.TransmitFile(Filpath);
                        HttpContext.Current.Response.End();
                        // PdfAction action = new PdfAction(PdfAction.PRINTDIALOG);
                    }
                }
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








        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }

        }
        private void GetOffConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
            if (con.State == ConnectionState.Open)
            {
                //confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Close();
            }
        }

    }
}