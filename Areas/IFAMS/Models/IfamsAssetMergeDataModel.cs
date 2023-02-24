using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsAssetMergeDataModel : IfamsAssetMergeRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        DataModel objidm = new DataModel();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        public List<AssetParentModel> GetAssetParent()
        {

            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_getassetparent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["asset_description"].ToString();
                    obj.AssetValue = row["assetdetails_asset_value"].ToString();
                    Model.Add(obj);
                }
                //Model. = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                Model.Clear();

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



        public List<AssetParentModel> GetAssetParentSummary()
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_mergesummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["assetdetails_asset_description"].ToString();
                    obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                    obj.status = row["status_name"].ToString();
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

        public List<AssetParentModel> CheckerSummary()
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_checkersummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@statusflag", SqlDbType.BigInt).Value = Convert.ToInt32(GetStatus("WAITING FOR APPROVAL"));
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["assetdetails_asset_description"].ToString();
                    obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                    obj.status = row["status_name"].ToString();
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



        public List<AssetParentModel> ViewMergedDet(int ParentID)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewmergedet", con);
                cmd.Parameters.Add("@spm_assetdet_gid", SqlDbType.BigInt).Value = Convert.ToInt32(ParentID);
                // cmd.Parameters.Add("@statusid", SqlDbType.BigInt).Value = Convert.ToInt32(GetStatus(Status));
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetsplitmerge_assetdet_gid"].ToString());
                    obj.AssetId = row["assetsplitmerge_new_assetid"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["assetdetails_asset_description"].ToString();
                    obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();

                    if (row["assetmerge_type"].ToString() == "P")
                    {
                        obj.AssetType = "Parent";
                    }
                    else if (row["assetmerge_type"].ToString() == "C")
                    {
                        obj.AssetType = "Child";
                    }
                    obj.status = row["status_name"].ToString();
                    Model.Add(obj);
                }
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetsplitmerge_assetdet_gid"].ToString());
                    obj.AssetId = row["assetsplitmerge_new_assetid"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["assetdetails_asset_description"].ToString();
                    if (row["assetmerge_type"].ToString() == "P")
                    {
                        obj.AssetType = "Parent";
                    }
                    else if (row["assetmerge_type"].ToString() == "C")
                    {
                        obj.AssetType = "Child";
                    }
                    obj.status = row["status_name"].ToString();
                    if (row["assetsplitmerge_new_assetvalue"].ToString() != "")
                    {


                        obj.AssetValue = "Merged Value=" + row["assetsplitmerge_new_assetvalue"].ToString();
                        HttpContext.Current.Session["MergeId"] = row["assetsplitmerge_new_assetid"].ToString();
                        obj.status = row["status_name"].ToString();
                        Model.Add(obj);
                    }
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















        public string GetViewMergeStatus(int ParentID)
        {

            DataTable dt = new DataTable();
            string Status = "";
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewmergedet", con);
                cmd.Parameters.Add("@spm_assetdet_gid", SqlDbType.BigInt).Value = Convert.ToInt32(ParentID);
                // cmd.Parameters.Add("@statusid", SqlDbType.BigInt).Value = Convert.ToInt32(GetStatus(status));
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    Status = row["status_name"].ToString();

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
            return Status.ToString();

        }




        public string GetRoleUser(string groupCode)
        {
            string success = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "group";
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = groupCode;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int userid = objCmnFunctions.GetLoginUserGid();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["roleemployee_employee_gid"].ToString() == userid.ToString())
                        {
                            success = "success";
                        }
                    }
                }
                return success;
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
        }





        public List<AssetParentModel> GetAssetIdParent(string AssetId)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_getparent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetId", SqlDbType.VarChar).Value = AssetId.ToString();
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    HttpContext.Current.Session["AssetIdChecked"] = Convert.ToInt32(row["assetdetails_gid"]);
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["asset_description"].ToString();
                    obj.AssetValue = row["assetdetails_asset_value"].ToString();
                    Model.Add(obj);
                }
                //Model. = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                // Model.Clear();

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



        public int ParentId(string AssetId)
        {
            int ParentId = 0;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_getparent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetId", SqlDbType.VarChar).Value = AssetId.ToString();
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    ParentId = Convert.ToInt32(row["assetdetails_gid"].ToString());

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

            return ParentId;



        }

        public List<AssetParentModel> GetAssetIdChild()
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_getassetparent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetCode = row["asset_code"].ToString();
                    obj.AssetDes = row["asset_description"].ToString();
                    obj.AssetValue = row["assetdetails_asset_value"].ToString();
                    Model.Add(obj);
                }
                //Model. = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                //Model.Clear();

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


        public string InsertMerge(string Childvalues, int ParentGid, string[] CheckedValues)
        {
            string instcomm = "";
            string[] ChildAssetValues;
            ChildAssetValues = Childvalues.Split(',').ToArray();


            if (UpdateParentMerge(ParentGid) == true)
            {

                UpdateQMaker(Convert.ToString(ParentGid.ToString()));

            }
            else
            {
                InsertQMaker(Convert.ToString(ParentGid.ToString()));


            }

            InsertChildValues(CheckedValues, ParentGid);
            Decimal Parentvalue = Convert.ToDecimal(GetChildValues(ParentGid));
            //Decimal Parentvalue1 = Convert.ToDecimal(GetChildValues(ParentGid));
            for (int i = 0; i < ChildAssetValues.Length; i++)
            {
                if (ChildAssetValues[i].ToString() != "")
                {
                    Parentvalue = Parentvalue + Convert.ToDecimal(ChildAssetValues[i]);
                }
            }
            try
            {


                if (UpdateParentMerge(ParentGid) == true)
                {


                    // Parentvalue = GetAssetvalues(ParentGid);


                    string[,] code = new string[,]
                    { 
                            
                             {"assetsplitmerge_asset_value",Parentvalue.ToString()},
                             {"assetsplitmerge_status", GetStatus("WAITING FOR APPROVAL")},
                             {"assetsplitmerge_isremoved", "N"}
                          
                    };


                    string[,] wher = new string[,]
                    { 
                            
                                {"assetsplitmerge_assetdet_gid", ParentGid.ToString()},
                                {"assetmerge_type", "P"},
                            
                                
                    };

                    objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");

                    // return "";
                    instcomm = "success";

                }

                else
                {

                    GetConnection();

                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_ifams_trn_getparent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = ParentGid;
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                    {

                        string[,] codes = new string[,]
                            {
                             {"assetsplitmerge_assetdet_gid",ParentGid.ToString()},
                             {"assetsplitmerge_asset_value",Parentvalue.ToString()},
                             {"assetsplitmerge_new_assetid ",row["assetdetails_assetdet_id"].ToString()},
                            // {"assetsplitmerge_new_assetvalue " , Parentvalue.ToString()},
                             {"assetsplitmerge_entry_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetsplitmerge_entry_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                             {"assetsplitmerge_isremoved","N"},
                             {"assetsplitmerge_status",GetStatus("WAITING FOR APPROVAL")},
                             {"assetmerge_type","P"},
                             {"assetsplitmerge_detail","M"}
                          
                            };

                        string tblname = "fa_trn_tassetsplitmerge";
                        instcomm = objCommonIUD.InsertCommon(codes, tblname);


                        //update Parent Id
                        string[,] code = new string[,]
                    { 
                            
                             {"assetdetails_isremoved","Y"},
                             {"assetdetails_takenby",objCmnFunctions.GetLoginUserGid().ToString()}
                    
                            
                    };


                        string[,] wher = new string[,]
                    { 
                            
                             {"assetdetails_gid", row["assetdetails_gid"].ToString()}
                    };

                        objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                    }



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



            return instcomm.ToString();
        }




        public decimal GetAssetvalues(int parentId)
        {


            decimal Assetvalues = 0;
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getassetvalues", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = parentId;
                // cmd.Parameters.Add("@Statusflag", SqlDbType.BigInt).Value = GetStatus("WAITING FOR APPROVAL");
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    Assetvalues = Assetvalues + Convert.ToDecimal(row["assetsplitmerge_asset_value"]);


                }
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());

            }
            finally
            {
                GetOffConnection();
            }


            return Assetvalues;


        }



        public bool UpdateParentMerge(int parentId)
        {
            bool True = false;
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_checkmerge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = parentId;
                // cmd.Parameters.Add("@statusflag", SqlDbType.BigInt).Value = GetStatus("WAITING FOR APPROVAL");
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["assetsplitmerge_assetdet_gid"]) == parentId)
                    {
                        True = true;
                    }

                }
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());

            }
            finally
            {
                GetOffConnection();
            }


            return True;

        }


        public string InsertQMaker(string AssetGid)
        {

            string[,] col = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                                     {"queue_ref_flag",GetRef("MOA").ToString()},
                                     {"queue_ref_gid", AssetGid},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",RoleGroupGid("MERGECHK")},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}                                     
	                      };
            string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
            return inst;

        }

        public string UpdateQMaker(string AssetGid)
        {
            string msg = "";


            string[,] code = new string[,]
                    { 
                            
                                    {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}      
                    
                            
                    };


            string[,] wher = new string[,]
                    { 
                            
                             {"queue_ref_gid", AssetGid.ToString()},
                              {"queue_ref_flag",GetRef("MOA").ToString()},
                                  {"queue_to",RoleGroupGid("MERGECHK")},
                    };

            msg = objCommonIUD.UpdateCommon(code, wher, "iem_trn_tqueue");

            return msg;

        }



        public string GetRef(string ref_name)
        {
            try
            {

                SqlDataAdapter da;
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "refno";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = ref_name.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                { ref_name = dt.Rows[0][0].ToString(); }
                return ref_name;
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return "";
            }
            finally { }
        }


        public string RoleGroupGid(string Rolegroup)
        {
            string RoleGroupGid = "";
            SqlCommand cmd;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = Rolegroup;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row1 in dt.Rows)
                    RoleGroupGid = row1["rolegroup_gid"].ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return RoleGroupGid;


        }


        public string InsertChildValues(string[] ChildGids, int ParentId)
        {
            string instcomm = "";
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            string ChildAssetValue = "";
            try
            {



                for (int i = 0; i < ChildGids.Length; i++)
                {
                    GetConnection();
                    if (ChildGids[i].ToString() != "")
                    {
                        DataTable dt = new DataTable();
                        SqlCommand cmd = new SqlCommand("pr_ifams_trn_getparent", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = Convert.ToInt32(ChildGids[i]);
                        dt.Load(cmd.ExecuteReader());
                        foreach (DataRow row in dt.Rows)
                        {

                            string[,] codes = new string[,]
                            {
                             {"assetsplitmerge_assetdet_gid",ParentId.ToString()},
                             {"assetsplitmerge_asset_value",row["assetdetails_asset_value"].ToString()},
                             {"assetsplitmerge_new_assetid ",row["assetdetails_assetdet_id"].ToString()},
                             {"assetsplitmerge_entry_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetsplitmerge_entry_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                             {"assetsplitmerge_isremoved","N"},
                             {"assetsplitmerge_status",GetStatus("WAITING FOR APPROVAL")},
                             {"assetmerge_type","C"},
                             {"assetsplitmerge_detail","M"}
                            };

                            string tblname = "fa_trn_tassetsplitmerge";
                            instcomm = objCommonIUD.InsertCommon(codes, tblname);




                            //update child Id
                            string[,] code = new string[,]
                             { 
                            
                             {"assetdetails_isremoved","Y"},
                             {"assetdetails_takenby",objCmnFunctions.GetLoginUserGid().ToString()}
                    
                            
                            };


                            string[,] wher = new string[,]
                             { 
                            
                             {"assetdetails_gid", row["assetdetails_gid"].ToString()}

                                };

                            objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");


                            GetOffConnection();

                        }
                    }
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

            return ChildAssetValue;

        }



        public string GetStatus(string Status)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_getdraftid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = Status;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }


            return dt.Rows[0][0].ToString();
        }



        public string GetChildValues(int AssetGid)
        {

            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            string ChildAssetValue = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_getparent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = AssetGid;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    ChildAssetValue = row["assetdetails_asset_value"].ToString();


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

            return ChildAssetValue;

        }


        public string AppChecker(int Parent)
        {
            string Message = "";

            decimal MergeAssetValue;
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewmergedet", con);
                cmd.Parameters.Add("@spm_assetdet_gid", SqlDbType.BigInt).Value = Convert.ToInt32(Parent);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    //update Asset value 
                    if (row["assetmerge_type"].ToString() == "P")
                    {
                        MergeAssetValue = Convert.ToInt32(row["assetsplitmerge_asset_value"].ToString());


                        string[,] codes = new string[,]
                             { 
                            
                             {"assetdetails_asset_value",MergeAssetValue.ToString()},
                             {"assetdetails_takenby",""},
                              {"assetdetails_isremoved","N"},
                            
                            };


                        string[,] where = new string[,]
                             { 
                            
                              {"assetdetails_gid",Parent.ToString()}

                                };

                        objCommonIUD.UpdateCommon(codes, where, "fa_trn_tassetdetails");




                        /////////////////////

                        string[,] code = new string[,]
                             { 
                            
                             {"assetsplitmerge_isremoved","N"},
                             {"assetsplitmerge_status",GetStatus("APPROVED").ToString()},
                             {"assetsplitmerge_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                              {"assetsplitmerge_update_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())}
                    
                            
                            };


                        string[,] wher = new string[,]
                             { 
                            
                             {"assetsplitmerge_assetdet_gid", Parent.ToString()}

                                };

                        Message = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");



                    }

                    //Update Asset details table by checker

                    else if (row["assetmerge_type"].ToString() == "C")
                    {


                        string[,] codes = new string[,]
                             { 
                            
                             {"assetdetails_takenby",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetdetails_isremoved","N"}
                            
                  
                            };


                        string[,] where = new string[,]
                             { 
                            
                             {"assetdetails_assetdet_id",row["assetsplitmerge_new_assetid"].ToString()}

                                };

                        objCommonIUD.UpdateCommon(codes, where, "fa_trn_tassetdetails");




                        ////////////////////////

                        string[,] code = new string[,]
                             { 
                            
                            // {"assetsplitmerge_isremoved","Y"},
                             {"assetsplitmerge_status",GetStatus("APPROVED").ToString()},
                             {"assetsplitmerge_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                              {"assetsplitmerge_update_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())}
                    
                            
                            };


                        string[,] wher = new string[,]
                             { 
                            
                             {"assetsplitmerge_assetdet_gid", Parent.ToString()}

                                };

                        Message = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");


                    }



                }


                //update tqueue table  for approval

                string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","1"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","N"},   
                                              
                    };
                string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", Parent.ToString()} ,    
                              {"queue_ref_flag", GetRef("MOA").ToString()}                                           
                    };
                string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");





            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }




            return Message.ToString();
        }




        public string RejectChecker(int Parent)
        {
            string Message = "";


            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewmergedet", con);
                cmd.Parameters.Add("@spm_assetdet_gid", SqlDbType.BigInt).Value = Convert.ToInt32(Parent);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    if (row["assetmerge_type"].ToString() == "P" && row["status_name"].ToString() == "WAITING FOR APPROVAL")
                    {
                        //Update Merge table by checker
                        string[,] code = new string[,]
                             { 
                            
                             {"assetsplitmerge_isremoved","N"},
                             {"assetsplitmerge_status",GetStatus("REJECTED")},
                             {"assetsplitmerge_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                              {"assetsplitmerge_update_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())}
                    
                            
                            };


                        string[,] wher = new string[,]
                             { 
                            
                             {"assetsplitmerge_assetdet_gid", Parent.ToString()}

                                };

                        Message = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");


                    }
                    else if (row["assetmerge_type"].ToString() == "C" && row["status_name"].ToString() == "WAITING FOR APPROVAL")
                    {
                        string[,] code = new string[,]
                             { 
                            
                             {"assetsplitmerge_isremoved","Y"},
                             {"assetsplitmerge_status",GetStatus("REJECTED")},
                             {"assetsplitmerge_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                              {"assetsplitmerge_update_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())}
                    
                            
                            };


                        string[,] wher = new string[,]
                             { 
                            
                             {"assetsplitmerge_assetdet_gid", Parent.ToString()}

                                };

                        Message = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");

                    }




                }

                foreach (DataRow row in dt.Rows)
                {
                    if (row["assetmerge_type"].ToString() == "C" && row["status_name"].ToString() == "WAITING FOR APPROVAL")
                    {
                        //Update Merge table by checker
                        string[,] code = new string[,]
                             { 
                            
                             {"assetdetails_takenby",""},
                             {"assetdetails_isremoved","N"},
                       
                            };


                        string[,] wher = new string[,]
                             { 
                            
                             {"assetdetails_assetdet_id", row["assetsplitmerge_new_assetid"].ToString()}

                                };

                        Message = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");


                    }
                }






                string[,] codes = new string[,]
                             { 
                            
                  
                             {"assetdetails_takenby",""},
                              {"assetdetails_isremoved","N"},
                            
                            };


                string[,] where = new string[,]
                             { 
                            
                              {"assetdetails_gid",Parent.ToString()}

                                };

                objCommonIUD.UpdateCommon(codes, where, "fa_trn_tassetdetails");



                //update tqueue table  for Reject

                string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","0"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","N"},   
                                              
                    };
                string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", Parent.ToString()} ,    
                              {"queue_ref_flag", GetRef("MOA").ToString()}                                           
                    };
                string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");




            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }




            return Message.ToString();
        }




        public List<AssetParentModel> MergeAuditTrial(int id)
        {


            string qdate = "";
            int qfrom = 0;
            string qactiondate = "";
            int qactionby = 0;
            string qstatus = "";
            string code = "";
            string name = "";
            string status = "";

            List<AssetParentModel> Model = new List<AssetParentModel>();

            AssetParentModel obj = new AssetParentModel();

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_mergeaudittrial", con);
                cmd.Parameters.AddWithValue("@Qdata", "Q");
                cmd.Parameters.AddWithValue("@queue_ref_flag", Convert.ToInt32(GetRef("MOA")));
                cmd.Parameters.AddWithValue("@queue_ref_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                status = GetViewMergeStatus(id);
                foreach (DataRow row in dt.Rows)
                {

                    qdate = convertoDate(row["queue_date"].ToString());
                    qfrom = Convert.ToInt32(row["queue_from"].ToString());
                    qactiondate = convertoDate(row["queue_action_date"].ToString());

                    if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "WAITING FOR APPROVAL";
                    }
                    else
                    {
                        qactionby = Convert.ToInt32(row["queue_action_by"].ToString());
                        qstatus = Convert.ToString(row["queue_action_status"].ToString());

                    }



                }

                dt = GetEmp(qfrom);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel(); ;

                    code = row["employee_code"].ToString();
                    name = row["employee_name"].ToString();
                    obj.empname_id = code + ":" + name;
                    obj.date = qdate;
                    obj.role = "MERGE Maker";
                    if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);

                    }
                    else if (status.ToString() == "APPROVED")
                    {

                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                    else if (status.ToString() == "REJECTED")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);

                    }

                }

                if (status.ToString() == "WAITING FOR APPROVAL")
                {
                    obj = new AssetParentModel(); ;
                    obj.role = "MERGE Checker";
                    obj.status = "PENDING";
                    Model.Add(obj);
                }
                else
                {
                    dt = GetEmp(qactionby);
                    foreach (DataRow row in dt.Rows)
                    {
                        obj = new AssetParentModel(); ;
                        code = row["employee_code"].ToString();
                        name = row["employee_name"].ToString();
                        obj.empname_id = code + ":" + name;
                        obj.date = qactiondate;
                        obj.role = "MERGE Checker";

                        if (status.ToString() == "WAITING FOR APPROVAL")
                        {
                            obj.status = "PENDING";
                            Model.Add(obj);

                        }

                        else if (status.ToString() == "APPROVED")
                        {
                            obj.status = "APPROVED";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "REJECTED")
                        {
                            obj.status = "REJECTED";
                            Model.Add(obj);

                        }

                    }


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


        public DataTable GetEmp(int EmpId)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_ifams_trn_mergeaudittrial", con);
                cmd.Parameters.AddWithValue("@Edata", "E");
                cmd.Parameters.AddWithValue("@EmpGid", EmpId);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                GetOffConnection();
            }


            return dt;

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




        /// <summary>
        /// Capitalization Date Change*************************************************
        /// </summary>

        //Capitalization Change Date for Maker
        public List<captializationdate> CapitalizationDateMaker()
        {
            List<captializationdate> Model = new List<captializationdate>();

            captializationdate obj = new captializationdate();

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_capitalizationdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "CapMaker";
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new captializationdate();
                    obj.assetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcode = row["asset_code"].ToString();
                    obj.assetDesc = row["asset_description"].ToString();
                    obj.location = row["assetdetails_branch_gid"].ToString();
                    obj.capnewdate = convertoDate(row["assetdetails_capnew_date"].ToString());
                    obj.status = row["status_name"].ToString();
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



        public List<captializationdate> CapitalizationDateChecker()
        {
            List<captializationdate> Model = new List<captializationdate>();

            captializationdate obj = new captializationdate();

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_capitalizationdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "DateChangeChecker";
                cmd.Parameters.Add("@status_flag", SqlDbType.BigInt).Value = Convert.ToInt32(GetStatus("WAITING FOR APPROVAL"));
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new captializationdate();
                    obj.assetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcode = row["asset_code"].ToString();
                    obj.assetDesc = row["asset_description"].ToString();
                    obj.location = row["assetdetails_branch_gid"].ToString();
                    obj.capnewdate = convertoDate(row["assetdetails_cap_date"].ToString());
                    obj.capchangenewdate = convertoDate(row["assetdetails_capnew_date"].ToString());
                    obj.status = row["status_name"].ToString();
                    obj.rectifamount = objidm.getRetificationAmount(obj.assetgid.ToString());
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









        //Change date for Capitalization 
        public List<captializationdate> ChangeDateMaker(string grp, string assetid)
        {
            List<captializationdate> Model = new List<captializationdate>();

            captializationdate obj = new captializationdate();

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_capitalizationdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "DateChangeMaker";
                cmd.Parameters.Add("@assetgrp", SqlDbType.VarChar).Value = grp;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = assetid;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new captializationdate();
                    obj.assetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetid = row["assetdetails_assetdet_id"].ToString();
                    obj.location = row["assetdetails_branch_gid"].ToString();
                    obj.assetcode = row["asset_code"].ToString();
                    obj.assetDesc = row["asset_description"].ToString();
                    obj.capnewdate = convertoDate(row["assetdetails_cap_date"].ToString());
                    // obj.status = row["status_name"].ToString();
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


        //Change Capitalization New date by maker
        public string ChangeCapdateMaker(string CapDate, string AssetGid)
        {
            string Msg = "";
            string[] CapDate1 = CapDate.Split(',');
            InsertQMakerForCap(AssetGid);
            GetConnection();
            string[,] code = new string[,]
                   {             
                   {"assetdetails_takenby","27".ToString()},                                     
                    };
            string[,] wher = new string[,]
                    {                             
                   {"assetdetails_gid", AssetGid.ToString()}
                    };

           Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
            string[,] code1 = new string[,]
                    { 
                        {"capitalisation_entitygid","1"},
                        {"capitalisation_assetid",AssetGid},
                        {"capitalisation_olddate",convertoDate(CapDate1[0].ToString())},
                        {"capitalisation_newdate",convertoDate(CapDate1[1].ToString())},
                        {"capitalisation_rectf_amount",objidm.getRetificationAmount(AssetGid.ToString()).ToString()},
                        {"capitalisation_makerid",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"capitalisation_checkerid",""},
                        {"capitalisation_mcstatus",GetStatus("WAITING FOR APPROVAL")},
                        {"capitalisation_process_date","SYSDATETIME()"},
                        {"capitalisation_uploadflag",""},
                        {"capitalisation_jobcode",""},
                        {"capitalisation_valuedate",""},
                        {"capitalisation_trans_date",""},
                        {"capitalisation_isremoved","N"},
                        {"capitalisation_insert_by",objCmnFunctions.GetLoginUserGid().ToString() },
                        {"capitalisation_insert_date","SYSDATETIME()"},
                        {"capitalisation_update_by",""},
                        {"capitalisation_update_date",""},
                };
            Msg = objCommonIUD.InsertCommon(code1, "fa_trn_tassetcapitalisation");
            GetOffConnection();
            return Msg;
        }


        public string InsertQMakerForCap(string AssetdGid)
        {

            string[,] col = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                                     {"queue_ref_flag",GetRef("CHCAPDATE").ToString()},
                                     {"queue_ref_gid", AssetdGid},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","I" },
                                     {"queue_to",RoleGroupGid("CDCHK")},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status",""},
                                     {"queue_isremoved","N"}                                     
	                      };
            string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
            return inst;

        }




        //Approve Capitalization date Changes by Checker
        public string ChangeCapdateMaker(string[] AssetDetGit)
        {
            string Msg = "";
            try
            {
                GetConnection();
                //GetConnection();

                for (int i = 0; i < AssetDetGit.Length; i++)
                {
                    if (AssetDetGit[i].ToString() != "")
                    {
                        string isid = AssetDetGit[i].ToString();
                        string[,] codesdep = new string[,]
                            {
                                {"depreciation_amount", objidm.getRetificationAmount(isid.ToString()).ToString()},                             
                                {"depreciation_assetdet_gid", isid.ToString()},
                                {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                                {"depreciation_asset_groupid", "".ToString()},
                                {"depreciation_asset_owner", "F"},
                                {"depreciation_type","C"},
                                {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                            };
                        string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

                        string[,] code1 = new string[,]
                        { 
                        {"capitalisation_checkerid",objCmnFunctions.GetLoginUserGid().ToString() },
                        {"capitalisation_mcstatus",GetStatus("APPROVED")},
                        {"capitalisation_valuedate",""},
                        {"capitalisation_trans_date",""},
                        {"capitalisation_update_by",objCmnFunctions.GetLoginUserGid().ToString() },
                        {"capitalisation_update_date","SYSDATETIME()"},
                      
                        };
                        string[,] wher1 = new string[,]
                        {     {"capitalisation_assetid",isid},                           
                        };
                        string inst = objCommonIUD.UpdateCommon(code1, wher1, "fa_trn_tassetcapitalisation");


                        DataTable dt = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_capitalizationdate", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@assetdetgid", SqlDbType.BigInt).Value = Convert.ToInt32(AssetDetGit[i]);
                        dt.Load(cmd.ExecuteReader());
                        foreach (DataRow row in dt.Rows)
                        {

                            string olddate = row["assetdetails_cap_date"].ToString();
                            string[,] code = new string[,]
                             { 
                             {"assetdetails_capold_date",convertoDate(olddate)},
                             {"assetdetails_cap_date",convertoDate(row["assetdetails_capnew_date"].ToString())},
                             {"assetdetails_takenby","14"},
                             {"assetdetails_status",GetStatus("APPROVED")}
                            };
                            string[,] wher = new string[,]
                             {                              {"assetdetails_gid", AssetDetGit[i].ToString()}
                                };
                            Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                        }
                        //update Capitalization date in tqueue table  for Approve

                        string[,] colm = new string[,]
                        {
                        {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"queue_action_status","1"},
                        {"queue_action_date",objCommonIUD.GetCurrentDate()},
                        {"queue_isremoved","N"},   
                                              
                        };
                        string[,] whrecol = new string[,]
                        {
                        {"queue_ref_gid", AssetDetGit[i]} ,    
                        {"queue_ref_flag", GetRef("CHCAPDATE").ToString()}                                           
                        };
                        string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");

                    }
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
            return Msg;
        }

        //Reject Capitalization date Changes by Checker
        public string RejChangeCapdateMaker(string[] AssetDetGit)
        {
            string Msg = "";
            try
            {                //GetConnection();
                for (int i = 0; i < AssetDetGit.Length; i++)
                {
                    if (AssetDetGit[i].ToString() != "")
                    {
                        GetConnection();
                        DataTable dt = new DataTable();
                        SqlCommand cmd = new SqlCommand("pr_ifams_trn_capitalizationdate", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@assetdetgid", SqlDbType.BigInt).Value = Convert.ToInt32(AssetDetGit[i]);
                        dt.Load(cmd.ExecuteReader());
                        foreach (DataRow row in dt.Rows)
                        {
                            string olddate = row["assetdetails_cap_date"].ToString();
                            string[,] code = new string[,]
                             { 
                              // {"assetdetails_capnew_date",convertoDate(olddate)},
                             {"assetdetails_takenby","14"},                             
                            };
                            string[,] wher = new string[,]
                                { 
                                {"assetdetails_gid", AssetDetGit[i].ToString()}
                                };

                            Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                            string[,] code1 = new string[,]
                    { 
                      
                                             
                        {"capitalisation_mcstatus",GetStatus("REJECTED")}, 
                        {"capitalisation_valuedate","SYSDATETIME()"},
                        {"capitalisation_trans_date","SYSDATETIME()"},
                        {"capitalisation_isremoved","N"},
                        {"capitalisation_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"capitalisation_update_date","SYSDATETIME()"},
                };
                            string[,] WHR1 = new string[,] { { "capitalisation_assetid", AssetDetGit[i] } };
                            Msg = objCommonIUD.UpdateCommon(code1,WHR1, "fa_trn_tassetcapitalisation");
                        }
                        //update Capitalization date in tqueue table  for Reject                       

                        
                       
                        string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","0"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","N"},                                                 
                    };
                        string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", AssetDetGit[i]} ,    
                              {"queue_ref_flag", GetRef("CHCAPDATE").ToString()}                                           
                    };
                        string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                    }
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
            return Msg;
        }


        //View Capitalization Changed Date details
        public List<captializationdate> ViewChangeDate(int Assetdetgid)
        {
            List<captializationdate> Model = new List<captializationdate>();

            captializationdate obj = new captializationdate();
            string status = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewcapdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdetgid", SqlDbType.BigInt).Value = Assetdetgid;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new captializationdate();
                    obj.assetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetid = row["assetdetails_assetdet_id"].ToString();
                    HttpContext.Current.Session["AssetId"] = row["assetdetails_assetdet_id"].ToString();
                    obj.location = row["assetdetails_branch_gid"].ToString();
                    obj.assetcode = row["asset_code"].ToString();
                    obj.assetDesc = row["asset_description"].ToString();
                    obj.capnewdate = convertoDate(row["assetdetails_capnew_date"].ToString());
                    status = GetCapstatus(Assetdetgid);
                    if (status.ToString() == "WAITING FOR APPROVAL" || status.ToString() == "REJECTED")
                    {
                        obj.capolddate = convertoDate(row["assetdetails_cap_date"].ToString());
                    }
                    else
                    {

                        obj.capolddate = convertoDate(row["assetdetails_capold_date"].ToString());
                    }

                    obj.status = row["status_name"].ToString();
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




        public List<captializationdate> CapDateAuditTrial(int id)
        {


            string qdate = "";
            int qfrom = 0;
            string qactiondate = "";
            int qactionby = 0;
            string qstatus = "";
            string code = "";
            string name = "";
            string status = "";

            List<captializationdate> Model = new List<captializationdate>();

            captializationdate obj = new captializationdate();


            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_mergeaudittrial", con);
                cmd.Parameters.AddWithValue("@Qdata", "Q");
                cmd.Parameters.AddWithValue("@queue_ref_flag", Convert.ToInt32(GetRef("CHCAPDATE")));
                cmd.Parameters.AddWithValue("@queue_ref_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                status = GetCapstatus(id);
                foreach (DataRow row in dt.Rows)
                {

                    qdate = convertoDate(row["queue_date"].ToString());
                    qfrom = Convert.ToInt32(row["queue_from"].ToString());
                    qactiondate = convertoDate(row["queue_action_date"].ToString());

                    if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "WAITING FOR APPROVAL";
                    }
                    else
                    {
                        qactionby = Convert.ToInt32(row["queue_action_by"].ToString());
                        qstatus = Convert.ToString(row["queue_action_status"].ToString());

                    }



                }

                dt = GetEmp(qfrom);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new captializationdate();

                    code = row["employee_code"].ToString();
                    name = row["employee_name"].ToString();
                    obj.employeeidname = code + ":" + name;
                    obj.actiondate = qdate;
                    obj.role = "CDCHK Maker";
                    if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);

                    }
                    else if (status.ToString() == "APPROVED")
                    {

                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                    else if (status.ToString() == "REJECTED")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);

                    }

                }

                if (status.ToString() == "WAITING FOR APPROVAL")
                {
                    obj = new captializationdate();
                    obj.role = "CDCHK Checker";
                    obj.status = "PENDING";
                    Model.Add(obj);
                }
                else
                {
                    dt = GetEmp(qactionby);
                    foreach (DataRow row in dt.Rows)
                    {
                        obj = new captializationdate();
                        code = row["employee_code"].ToString();
                        name = row["employee_name"].ToString();
                        obj.employeeidname = code + ":" + name;
                        obj.actiondate = qactiondate;
                        obj.role = "CDCHK Checker";

                        if (status.ToString() == "DRAFT")
                        {
                            obj.status = "PENDING";
                            Model.Add(obj);

                        }
                        else if (status.ToString() == "WAITING FOR APPROVAL")
                        {
                            obj.status = "WAITING FOR APPROVAL";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "APPROVED")
                        {
                            obj.status = "APPROVED";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "REJECTED")
                        {
                            obj.status = "REJECTED";
                            Model.Add(obj);

                        }

                    }
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


        public string GetCapstatus(int Assetdetgid)
        {


            string status = "";

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_viewcapdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdetgid", SqlDbType.BigInt).Value = Assetdetgid;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {

                    status = row["status_name"].ToString();

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


            return status;

        }



        /// <summary>
        ///////////////////////// AssetMerging
        /// </summary>

        //xavier

        public List<AssetParentModel> GetAssetInGroup(string AssetId, string grpid)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (AssetId != "")
                {
                    cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "AssetIdInGroup";
                    cmd.Parameters.Add("@Term", SqlDbType.VarChar).Value = AssetId;
                }
                if (grpid != "")
                {
                    cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "SearchAssetIdInGroup";
                    cmd.Parameters.Add("@Term", SqlDbType.VarChar).Value = grpid;
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.assetcap = row["assetdetails_cap_date"].ToString();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["branch_code"].ToString();
                    obj.Assetgrpid = row["assetdetails_asset_groupid"].ToString();

                    //obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                    //obj.status = row["status_name"].ToString();
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

        //samsudeen 

        public List<AssetParentModel> Getsearchmakersummary(string AssetId, string Getsearchmakersummary)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "SearchAssetmergesummary";
                if (AssetId != "")
                {
                    cmd.Parameters.Add("@AssetCode", SqlDbType.VarChar).Value = AssetId;
                }
                if (Getsearchmakersummary != "")
                {
                    // cmd.Parameters.Add("@AssetCode", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@BranchLocation", SqlDbType.VarChar).Value = Getsearchmakersummary;
                }
                if (AssetId == "" && Getsearchmakersummary == "")
                {
                    AssetId = "";
                    Getsearchmakersummary = "";
                }
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.Location = row["Branch"].ToString();
                    obj.status = row["AssetStatus"].ToString();
                    obj.NewAssetId = row["assetsplitmerge_new_assetid"].ToString();
                    Model.Add(obj);

                    //obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                    //obj.status = row["status_name"].ToString();
                    //Model.Add(obj);
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


        public string GetAssetMerging(string AssetId, string[] Checkevalues)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            string Msg = "";
            string BranchCode = "";
            string AssetCode = "";
            string NewAssetId = "";
            string Groupid = "";
            string Assetcodegid = "";
            string AssetDes = "";
            string Branchgid = "";
            string Capdate = "";
            double MergedValues = 0;
            string splimergestatus = "";
            string Tempidvalue = "";
            Int32 getsequenceno = 0;
            Int32 getn = 0;

            try
            {

                //getsequenceno

                GetConnection();
                DataTable getseq = new DataTable();
                SqlCommand cmd1 = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@Depreciation", SqlDbType.VarChar).Value = "GetSequenceno";
                getseq.Load(cmd1.ExecuteReader());
                foreach (DataRow seq in getseq.Rows)
                {
                    getsequenceno = Convert.ToInt32(seq["sequence_no"].ToString());
                    getn = getsequenceno;
                }






                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "SearchBYAssetId";
                cmd.Parameters.Add("@term", SqlDbType.VarChar).Value = AssetId;
                dt.Load(cmd.ExecuteReader());


                for (int i = 0; i < Checkevalues.Length; i++)
                {
                    if (Checkevalues[i].ToString() != "")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new AssetParentModel();
                            if (Convert.ToInt32(row["assetdetails_gid"].ToString()) == Convert.ToInt32(Checkevalues[i]))
                            {
                                obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                                obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                                obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                                obj.AssetCatName = row["assetcategory_name"].ToString();
                                obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                                obj.AssetDes = row["AssetSubCatName"].ToString();
                                obj.Location = row["branch_code"].ToString();
                                BranchCode = row["branch_code"].ToString();
                                AssetCode = row["AssetSubCategoryCode"].ToString();
                                Groupid = row["assetdetails_asset_groupid"].ToString();
                                Assetcodegid = row["assetdetails_asset_code"].ToString();
                                AssetDes = row["assetdetails_asset_description"].ToString();
                                Branchgid = row["assetdetails_branch_gid"].ToString();
                                Capdate = row["assetdetails_cap_date"].ToString();
                                MergedValues = MergedValues + Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                                Model.Add(obj);

                                if (getsequenceno == 0)
                                {
                                    getsequenceno++;
                                    //Tempidvalue = "MOA" + string.Format("{0:0000}", 1);
                                    Tempidvalue = Convert.ToString(getsequenceno);
                                }
                                else
                                {

                                    if (getn == getsequenceno)
                                    {
                                        getsequenceno++;
                                        Tempidvalue = Convert.ToString(getsequenceno);
                                    }
                                    else
                                    {
                                        Tempidvalue = Convert.ToString(getsequenceno);
                                    }
                                }


                                splimergestatus = System.Configuration.ConfigurationManager.AppSettings["WFA"].ToString();
                                string getvalue = Checkevalues[i].ToString();

                                string[,] codes = new string[,]
                             {
                             {"assetsplitmerge_assetdet_gid",getvalue},
                             {"assetsplitmerge_asset_value",obj.MAssetValue.ToString ()},                             
                             {"assetsplitmerge_new_assetvalue",MergedValues.ToString()},
                             {"assetsplitmerge_entry_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetsplitmerge_entry_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                             {"assetsplitmerge_status",splimergestatus},
                             {"assetsplitmerge_detail","M"},
                             {"assetsplitmerge_new_assetid",Tempidvalue},
                             {"assetsplitmerge_isremoved","N"}


                            };

                                string tblname = "fa_trn_tassetsplitmerge";
                                Msg = objCommonIUD.InsertCommon(codes, tblname);
                            }
                        }
                    }
                }




                //update sequenc

                string[,] code11 = new string[,]
                    { 
                            
                             {"sequence_no",Tempidvalue}                                      
                            
                    };
                string[,] wher11 = new string[,]
                    { 
                            
                             {"sequence_code","MOA"}
                             
                    };
                Msg = objCommonIUD.UpdateCommon(code11, wher11, "iem_mst_tsequence");


                for (int i = 0; i < Checkevalues.Length; i++)
                {
                    if (Checkevalues[i].ToString() != "")
                    {
                        string[,] code1 = new string[,]
                    { 
                            
                             {"assetdetails_active_status","N"},
                             {"assetdetails_takenby",AssetMergeId()}
                    
                            
                    };


                        string[,] wher1 = new string[,]
                    { 
                            
                             {"assetdetails_gid", Checkevalues[i].ToString()}
                    };

                        Msg = objCommonIUD.UpdateCommon(code1, wher1, "fa_trn_tassetdetails");
                    }

                    string[,] code = new string[,]
                    { 
                            
                             {"assetsplitmerge_new_assetvalue",MergedValues.ToString ()},
                             {"assetsplitmerge_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetsplitmerge_update_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                             {"assetsplitmerge_new_assetid",Tempidvalue.ToString ()}                  
                            
                    };
                    string[,] wher = new string[,]
                    { 
                            
                             {"assetsplitmerge_status",splimergestatus},
                             {"assetsplitmerge_detail","M"},
                             {"assetsplitmerge_assetdet_gid",Checkevalues[i].ToString ()}
                    };
                    Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");

                    //Que Entry 


                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "MERGECHK";
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    string grpid = "";
                    foreach (DataRow row in dt1.Rows)
                        grpid = row["rolegroup_gid"].ToString();
                    string[,] col = new string[,]
                {
                                     {"queue_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                                     {"queue_ref_flag",objidm.GetRef("MOA").ToString()},
                                     {"queue_ref_gid",  Checkevalues[i].ToString ()},
                                     {"queue_action_for", "A"},
                                     {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                     {"queue_to_type","G" },
                                     {"queue_to",grpid},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}     
                };

                    Msg = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");

                }


                // NewAssetId = objCmnFunctions.generateAssetid(BranchCode, AssetCode);

                //NewAssetId = CheckAssetIdExists(BranchCode, AssetCode);

                // HttpContext.Current.Session["NewAssetId"] = NewAssetId;

                //string[,] codes = new string[,]
                //            {
                //             {"assetdetails_assetdet_id",NewAssetId.ToString()},
                //             {"assetdetails_asset_groupid",Groupid.ToString()},
                //             {"assetdetails_asset_code ",Assetcodegid.ToString()},
                //             {"assetdetails_asset_value",MergedValues.ToString()},
                //             {"assetdetails_asset_description",AssetDes.ToString()},
                //             {"assetdetails_active_status","N"},
                //             {"assetdetails_upld_status","N"},
                //             {"assetdetails_dep_onhold","N"},
                //             {"assetdetails_spoke_asset","N"},
                //             {"assetdetails_impair_asset","N"},
                //             {"assetdetails_assetcode_changestatus","N"},
                //             {"assetdetails_isremoved","N"},
                //             {"assetdetails_branch_gid",Branchgid.ToString()},
                //             {"assetdetails_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                //             {"assetdetails_insert_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                //             {"assetdetails_takenby","14"},
                //             {"assetdetails_assetid_source",GetSourceid()},
                //              {"assetdetails_physical_assetid","0"},
                //              {"assetdetails_cap_date",convertoDate(Capdate.ToString())}
                //            };



                // string Assetgid = DepAssetgid(NewAssetId);

                //  int[] DepSum = new int[500];
                //  string[] Depdate = new string[500];
                //  string[] Depgroup = new string[300];
                //// int[] DepSum;

                // for (int i = 0; i < Checkevalues.Length; i++)
                // {
                //     if (Checkevalues[i].ToString() != "")
                //     {
                //         int j = 0;
                //         DataTable dt1 = GetDepreciation(Convert.ToInt32(Checkevalues[i].ToString()));
                //         foreach (DataRow row in dt1.Rows) {

                //             DepSum[j] = DepSum[j] + Convert.ToInt32(row["depreciation_amount"]);
                //             Depdate[j] = row["depreciation_month"].ToString();
                //             Depgroup[j] = row["depreciation_asset_groupid"].ToString();
                //             j++;
                //         }

                //     }
                // }
                //  //Insert Depreciation
                // for (int i = 0; i < DepSum.Length; i++)
                // {
                //     if (Depdate[i] == null)  { 

                //     Depdate[i] ="";
                //     }

                //     if (Depgroup[i] == null) {
                //         Depgroup[i] = "";
                //     }

                //     if (Depdate[i] != null && Depgroup[i] != null)
                //     {
                //         string[,] cod = new string[,]
                //              {
                //               {"depreciation_assetdet_gid",Assetgid},
                //               {"depreciation_amount",DepSum[i].ToString()},
                //               {"depreciation_month ",Depdate[i].ToString()},
                //               {"depreciation_asset_groupid",Depgroup[i]},
                //               {"depreciation_asset_owner","F"},
                //               {"depreciation_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                //               {"depreciation_insert_date",convertoDate(objCommonIUD.GetCurrentDate())},
                //              };

                //         string tblnam = "fa_trn_tdepreciation";
                //         Msg = objCommonIUD.InsertCommon(cod, tblnam);
                //     }
                // }



                //  //Delete Depreciation
                // for (int i = 0; i < Checkevalues.Length; i++)
                // {
                //     if (Checkevalues[i].ToString() != "")
                //     {
                //         Depdelete(Convert.ToInt32(Checkevalues[i].ToString()));
                //     }
                // }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Msg;

        }

        public string Depdelete(int AssetGID)
        {

            string Assetgid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Depreciation", SqlDbType.VarChar).Value = "DeleteDepreciation";
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = AssetGID;
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }


            return Assetgid;


        }

        public string DepAssetgid(string AssetID)
        {

            int Assetgid = 0;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Depreciation", SqlDbType.VarChar).Value = "NewAssetgid";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetID;
                dt.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt.Rows)
                {

                    Assetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
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


            return Assetgid.ToString();


        }

        public DataTable GetDepreciation(int Assetgid)
        {

            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Depreciation", SqlDbType.VarChar).Value = "Depreciation";
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = Assetgid;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }


            return dt;
        }






        public string CheckAssetIdExists(string Branch, string AssetCode)
        {
            string AssetId = "";
            string Status = "";

            AssetId = objCmnFunctions.generateAssetid(Branch, AssetCode);
            Status = AssetIDexists(AssetId);
            if (Status.ToString() == "Exists")
            {
                CheckAssetIdExists(Branch, AssetCode);
            }


            return AssetId;

        }

        public string AssetIDexists(string AssetID)
        {

            string AssetIdStatus = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "AssetIdExist";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetID;
                AssetIdStatus = (string)cmd.ExecuteScalar();


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }


            return AssetIdStatus;


        }


        public string GetSourceid()
        {
            string Sourceid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetSource", SqlDbType.VarChar).Value = "GetAssetSourceId";
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    Sourceid = dt.Rows[0]["assetsource_flag"].ToString();

                }
                else
                {

                    Sourceid = "";
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


            return Sourceid;
        }




        public string AssetMergeId()
        {
            string Mergeid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetSource", SqlDbType.VarChar).Value = "AssetMerge";
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    Mergeid = dt.Rows[0]["excelvalidate_gid"].ToString();

                }
                else
                {

                    Mergeid = "";
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


            return Mergeid;
        }

        //samsudeen
        public string AssetMergeIdApprove()
        {
            string Mergeid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetSource", SqlDbType.VarChar).Value = "AssetMerged";
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    Mergeid = dt.Rows[0]["excelvalidate_gid"].ToString();

                }
                else
                {

                    Mergeid = "";
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


            return Mergeid;
        }

        public List<AssetParentModel> NewAsset(string AssetId)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "NewAssetId";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetId;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["branch_code"].ToString();

                    //obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                    //obj.status = row["status_name"].ToString();
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
        public List<AssetParentModel> OldMergedAsset(string[] AssetGId)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            Int64 MergedValues = 0;
            try
            {

                for (int i = 0; i < AssetGId.Length; i++)
                {

                    if (AssetGId[i].ToString() != "")
                    {


                        DataTable dt = GetMergedAssets(Convert.ToInt32(AssetGId[i]));
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new AssetParentModel();
                            obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                            obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                            obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                            obj.AssetCatName = row["assetcategory_name"].ToString();
                            obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                            obj.AssetDes = row["AssetSubCatName"].ToString();
                            obj.Location = row["branch_code"].ToString();
                            MergedValues = MergedValues + Convert.ToInt64(row["assetdetails_asset_value"].ToString());
                            //obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                            //obj.status = row["status_name"].ToString();
                            Model.Add(obj);
                        }

                    }
                }
                if (MergedValues != 0)
                {

                    obj = new AssetParentModel();
                    obj.MAssetValue = MergedValues;
                    Model.Add(obj);
                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {



            }

            return Model;

        }

        public DataTable GetMergedAssets(int Assetgid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "MergedAssetId";
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = Assetgid;
                dt.Load(cmd.ExecuteReader());

                GetOffConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dt;

        }
        public DataTable GroupMerging(int Assetgid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "G";
                cmd.Parameters.Add("@Assetgid", SqlDbType.BigInt).Value = Assetgid;
                dt.Load(cmd.ExecuteReader());

                GetOffConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dt;

        }

        public List<AssetParentModel> ValidateGroup(string[] AssetGId)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            Int64 MergedValues = 0;
            try
            {

                for (int i = 0; i < AssetGId.Length; i++)
                {

                    if (AssetGId[i].ToString() != "")
                    {


                        DataTable dt = GroupMerging(Convert.ToInt32(AssetGId[i]));
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new AssetParentModel();
                            obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                            obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                            obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                            obj.AssetCatName = row["assetcategory_name"].ToString();
                            obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                            obj.AssetDes = row["AssetSubCatName"].ToString();
                            obj.Location = row["branch_code"].ToString();
                            obj.Physicalid = row["assetdetails_physical_assetid"].ToString();
                            obj.date = convertoDate(row["assetdetails_cap_date"].ToString());
                            obj.Assetgrpid = row["assetdetails_asset_groupid"].ToString();
                            MergedValues = MergedValues + Convert.ToInt64(row["assetdetails_asset_value"]);
                            //obj.AssetValue = row["assetsplitmerge_asset_value"].ToString();
                            //obj.status = row["status_name"].ToString();
                            Model.Add(obj);
                        }

                    }
                }




            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {



            }

            return Model;

        }




        public string CheckPhyAsset(string AssetId)
        {
            string physicalId = "";
            string Message = "";
            int Count = 0;
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "AssetIdInGroup";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetId;
                dt.Load(cmd.ExecuteReader());

                physicalId = GetPhysicalId(AssetId);
                if (physicalId == "" || physicalId == "0")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["assetdetails_physical_assetid"].ToString() == physicalId)
                        {

                            Count++;

                        }
                    }

                    if (Count == dt.Rows.Count)
                    {

                        Message = "PhysicalID is Not Refered";
                        Count = 0;

                    }
                    else
                    {
                        Message = "Assets Cant be Merged";
                        Count = 0;

                    }
                }
                else
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        int GroupPhyCount = GroupCount(AssetId);
                        if (dt.Rows.Count == GroupPhyCount)
                        {
                            if (row["assetdetails_physical_assetid"].ToString() == physicalId)
                            {
                                //Message = "Asset is Containing Physical ID";
                                Count++;

                            }

                        }

                    }
                    if (Count == dt.Rows.Count)
                    {
                        Message = "PhysicalID is Refered";
                        Count = 0;
                    }
                    else
                    {

                        Message = "Assets Cant be Merged";
                        Count = 0;

                    }
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

            return Message.ToString();

        }

        public string GetPhysicalId(string AssetId)
        {
            string physicalId = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "PhysicalId";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetId;
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["assetdetails_physical_assetid"].ToString() == "" || dt.Rows[0]["assetdetails_physical_assetid"].ToString() == "0")
                    {
                        physicalId = dt.Rows[0]["assetdetails_physical_assetid"].ToString();

                    }
                    else
                    {

                    }


                }
                else
                {

                    physicalId = "";
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


            return physicalId;
        }



        public int GroupCount(string AssetId)
        {
            int GroupCount = 0;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "GetGroupCount";
                cmd.Parameters.Add("@AssetId", SqlDbType.VarChar).Value = AssetId;
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count != 0)
                {
                    GroupCount = Convert.ToInt32(dt.Rows[0]["goaheader_asset_count"].ToString());

                }
                else
                {

                    GroupCount = 0;
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


            return GroupCount;
        }





        public List<AssetParentModel> AutoAssetId(string term)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ActionAutoStatus", SqlDbType.VarChar).Value = "AssetId";
                cmd.Parameters.Add("@Term", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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

        public List<AssetParentModel> GetAssettmergeDetails()
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "Assetmergesummary";
                //cmd.Parameters.Add("@Term", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    //obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.Location = row["Branch"].ToString();
                    obj.status = row["AssetStatus"].ToString();
                    obj.NewAssetId = row["assetsplitmerge_new_assetid"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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


        public List<AssetParentModel> GetAssettmergeCheckerDetails()
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "AssetCheckersummary";
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    // obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.Location = row["Branch"].ToString();
                    obj.status = row["AssetStatus"].ToString();
                    obj.NewAssetId = row["assetsplitmerge_new_assetid"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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

        public List<AssetParentModel> getsearchckeckerdetails(string get, string branch)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "SearchAssetCheckersummary";
                if (get != "")
                {
                    cmd.Parameters.Add("@AssetCode", SqlDbType.VarChar).Value = get;
                }
                if (branch != "")
                {
                    cmd.Parameters.Add("@BranchLocation", SqlDbType.VarChar).Value = branch;
                }
                if (get == "" && branch == "")
                {
                    cmd.Parameters.Add("@AssetCode", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@BranchLocation", SqlDbType.VarChar).Value = "";
                }
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    // obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.Location = row["Branch"].ToString();
                    obj.status = row["AssetStatus"].ToString();
                    obj.NewAssetId = row["assetsplitmerge_new_assetid"].ToString();
                    //obj.FileName = row["pv_assetdet_assetfilename"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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


        public List<AssetParentModel> GetAssetMergemakersummary(string Generateid)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "Assetmergemakerviewsummary";
                cmd.Parameters.Add("@AssetGenerateId", SqlDbType.VarChar).Value = Generateid;

                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["Branch"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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



        public List<AssetParentModel> GetAssetMergemakersummarynew(string Generateid, string status)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "Assetmergemakerviewsummarynew";
                cmd.Parameters.Add("@AssetGenerateId", SqlDbType.VarChar).Value = Generateid;
                cmd.Parameters.AddWithValue("@ActionAutoStatus", status);
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["Branch"].ToString();
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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





        //Muthu 
        public List<AssetParentModel> GetAssetMergecheckersummarynew(string Generateid, string status)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "Assetmergecheckerviewsummarynew";
                cmd.Parameters.Add("@AssetGenerateId", SqlDbType.VarChar).Value = Generateid;
                cmd.Parameters.AddWithValue("@ActionAutoStatus", status);
                //  cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["Branch"].ToString();
                    obj.rectifAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString());
                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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



        public List<AssetParentModel> GetAssetMergecheckersummary(string Generateid)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "Assetmergecheckerviewsummary";
                cmd.Parameters.Add("@AssetGenerateId", SqlDbType.VarChar).Value = Generateid;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                    obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                    obj.AssetCatName = row["assetcategory_name"].ToString();
                    obj.AssetSubCode = row["AssetSubcategory"].ToString();
                    obj.AssetDes = row["AssetSubCatName"].ToString();
                    obj.Location = row["Branch"].ToString();

                    Model.Add(obj);
                }
                //objCmnFunctions.generateAssetid("Branch","Assetcode");

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
        public string GetAssetMerging1(string assetid, string[] assetgid, string remrks)
        {
            string Result = string.Empty;
            try
            {
                string oldid = string.Join(",", assetgid.ToArray());
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fa_set_MergechkrApprove", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Oldassetid", SqlDbType.VarChar).Value = oldid;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = remrks.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "APPROVED";
                cmd.Parameters.Add("@Userid", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message.ToString();
            }
            finally
            {

                GetOffConnection();

            }
            return Result;
        }
        #region
        /*
        public string GetAssetMerging1(string assetid, string[] assetgid, string remrks)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            string Msg = "";
            string BranchCode = "";
            string AssetCode = "";
            string NewAssetId = "";
            string Groupid = "";
            string Assetcodegid = "";
            string AssetDes = "";
            string Branchgid = "";
            string Capdate = "";
            string pono = "";
            string ponumber = "";
            string ecfnumber = "";
            string cbfnumber = "";
            double MergedValues = 0;
            string branchlegacycode = "";
            string oracleCCcode = "";
            string oracleBScode = "";
            string ecfgid = "";
            string cbfgid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "AssetApprove";
                cmd.Parameters.Add("@term", SqlDbType.VarChar).Value = assetid;
                dt.Load(cmd.ExecuteReader());
                for (int i = 0; i < assetgid.Length; i++)
                {
                    if (assetgid[i].ToString() != "")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            obj = new AssetParentModel();
                            if (Convert.ToInt32(row["assetdetails_gid"].ToString()) == Convert.ToInt32(assetgid[i]))
                            {
                                obj.AssetDetGid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                                obj.AssetId = row["assetdetails_assetdet_id"].ToString();
                                obj.MAssetValue = Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                                obj.AssetCatName = row["assetcategory_name"].ToString();
                                obj.AssetSubCode = row["AssetSubCategoryCode"].ToString();
                                obj.AssetDes = row["AssetSubCatName"].ToString();
                                obj.Location = row["branch_code"].ToString();
                                BranchCode = row["branch_code"].ToString();
                                AssetCode = row["AssetSubCategoryCode"].ToString();
                                Groupid = row["assetdetails_asset_groupid"].ToString();
                                Assetcodegid = row["assetdetails_asset_code"].ToString();
                                AssetDes = row["assetdetails_asset_description"].ToString();
                                Branchgid = row["assetdetails_branch_gid"].ToString();
                                Capdate = row["assetdetails_cap_date"].ToString();
                                pono = row["assetdetails_po_number"].ToString();
                                MergedValues = MergedValues + Convert.ToDouble(row["assetdetails_asset_value"].ToString());
                                ecfnumber = row["assetdetails_ecfnum"].ToString();
                                ponumber = row["assetdetails_ponum"].ToString();
                                cbfnumber = row["assetdetails_cbfnum"].ToString();
                                ecfgid = row["assetdetails_ecf_gid"].ToString();
                                cbfgid = row["assetdetails_cbf_gid"].ToString();
                                Model.Add(obj);
                            }
                        }
                    }
                }
                for (int i = 0; i < assetgid.Length; i++)
                {
                    if (assetgid[i].ToString() != "")
                    {
                        string[,] code = new string[,]
                    {                             
                             {"assetdetails_isremoved","N"},
                             {"assetdetails_active_status","Y"},
                             {"assetdetails_takenby",AssetMergeIdApprove()}  
                    };
                        string[,] wher = new string[,]
                    {                                                     
                             {"assetdetails_gid", assetgid[i].ToString()}
                    };
                        objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                    }
                }
                NewAssetId = objCmnFunctions.generateAssetid(BranchCode, AssetCode);
                HttpContext.Current.Session["NewAssetId"] = NewAssetId;


                decimal a = 0;
                a = Convert.ToDecimal(MergedValues.ToString());
                string b = "";

                string GL = string.Empty;

                //Muthu 07-12-2016
                DataTable dta = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetsubgid", Assetcodegid.ToString());
                cmd.Parameters.AddWithValue("@action", "SelectGLassetid");
                da = new SqlDataAdapter(cmd);
                da.Fill(dta);
                for (int j = 0; j < dta.Rows.Count; j++)
                {
                    GL = dta.Rows[0]["asset_glcode"].ToString();
                }


                if (a <= 5000 || (a > 5000 && GL == "121010002"))
                {
                    b = "N";
                }
                else { b = "Y"; }


                string[,] codes = new string[,]
                            {
                                {"assetdetails_assetdet_id",NewAssetId.ToString()},
                                {"assetdetails_entity_gid","1"},
                                {"assetdetails_asset_groupid",Groupid.ToString()},
                                {"assetdetails_asset_code ",Assetcodegid.ToString()},
                                {"assetdetails_asset_value",MergedValues.ToString()},
                                {"assetdetails_asset_description",AssetDes.ToString()},
                                {"assetdetails_active_status","Y"},
                                {"assetdetails_upld_status","N"},
                                {"assetdetails_dep_onhold","N"},
                                {"assetdetails_spoke_asset","N"},
                                {"assetdetails_impair_asset","N"},
                                {"assetdetails_not_5kcase", b.ToString()},                               
                                {"assetdetails_isremoved","N"},
                                {"assetdetails_branch_gid",Branchgid.ToString()},
                                {"assetdetails_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"assetdetails_insert_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                                {"assetdetails_takenby","14"},
                                {"assetdetails_assetid_source",GetSourceid()},
                                {"assetdetails_physical_assetid","0"},
                                {"assetdetails_cap_date",convertoDate(Capdate.ToString())},
                                {"assetdetails_qty","1"},
                                {"assetdetails_po_number",pono},                           
                                {"assetdetails_sale_status","N"},
                                {"assetdetails_ponum",ponumber},                                  
                                {"assetdetails_cbfnum",cbfnumber},
                                {"assetdetails_cbf_gid",cbfgid},
                                {"assetdetails_ecfnum",ecfnumber},
                                {"assetdetails_ecf_gid",ecfgid},
                                {"assetdetails_trf_value",MergedValues.ToString()},
                                {"assetdetails_asset_owner","F"},                           
                                {"assetdetails_reduced_value","0"},                           
                                {"assetdetails_dep_rate","0"},
                                {"assetdetails_asset_serialno","0"},
                                {"assetdetails_addate",convertoDate(objCommonIUD.GetCurrentDate().ToString())}
                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                ,{"assetdetails_assetcode_changeid","0"}
                                ,{"assetdetails_assetcode_changestatus","N"}
                                ,{"assetdetails_barcode",string.Empty}
                                ,{"assetdetails_capnew_date",string.Empty}
                                ,{"assetdetails_capold_date",string.Empty}
                                ,{"assetdetails_imp_date",string.Empty}
                                ,{"assetdetails_lease_enddate",string.Empty}
                                ,{"assetdetails_lease_startdate",string.Empty}
                                ,{"assetdetails_physical_idrelease","0"}
                                ,{"assetdetails_recon_reference",string.Empty}
                                ,{"assetdetails_sale_date",string.Empty}
                                ,{"assetdetails_sale_id","0"}
                                ,{"assetdetails_status","0"}
                                ,{"assetdetails_tfr_date",string.Empty}
                                ,{"assetdetails_tfr_id","0"}
                                ,{"assetdetails_tfr_status","N"}
                                ,{"assetdetails_trf_from","0"}
                                ,{"assetdetails_trf_reason",string.Empty}
                                ,{"assetdetails_trf_to","0"}
                                ,{"assetdetails_trn_date",string.Empty}
                                ,{"assetdetails_update_by","0"}
                                ,{"assetdetails_update_date",string.Empty}
                                ,{"assetdetails_upld_ref",string.Empty}
                                ,{"assetdetails_wrt_date",string.Empty}
                                ,{"assetdetails_wrt_id","0"}
                                ,{"assetdetails_wrt_status","N"}
                            };
                string tblname = "fa_trn_tassetdetails";
                Msg = objCommonIUD.InsertCommon(codes, tblname);
                string Assetgid = DepAssetgid(NewAssetId);






                //Location table entry

                DataTable dtloc = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branchgid", Branchgid.ToString());
                cmd.Parameters.AddWithValue("@assetdetid", NewAssetId.ToString());
                cmd.Parameters.AddWithValue("@action", "Branchlegacy");
                da = new SqlDataAdapter(cmd);
                da.Fill(dtloc);
                for (int l = 0; l < dtloc.Rows.Count; l++)
                {
                    branchlegacycode = dtloc.Rows[l]["branch_legacy_code"].ToString();
                    oracleBScode = dtloc.Rows[l]["branch_businesssegement"].ToString();
                }


                DataTable dtcc = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetdetid", NewAssetId.ToString());
                cmd.Parameters.AddWithValue("@action", "transfercc");
                da = new SqlDataAdapter(cmd);
                da.Fill(dtcc);
                for (int l = 0; l < dtcc.Rows.Count; l++)
                {
                    oracleCCcode = dtcc.Rows[l]["cc_code"].ToString();
                }

                string oraclebscccode = "";
                oraclebscccode = oracleCCcode + oracleBScode;


                string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",NewAssetId.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",oraclebscccode.ToString()},
                                {"assetlocation_location_fc",oracleBScode.ToString()},
                                {"assetlocation_location_cc",oracleCCcode.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",Convert.ToString("SYSDATETIME()")}
                            };

                string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");









                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = Assetgid.ToString();
                da = new SqlDataAdapter(cmd);
                string glCode = "";
                string branch = "";
                string cat = "";
                string subcat = "";
                string asset_id = "";
                string BS = "";
                string CC = "";
                DataTable UPLOADDATATBL = new DataTable();
                da.Fill(UPLOADDATATBL);
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
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = 42;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                da = new SqlDataAdapter(cmd);
                UPLOADDATATBL = new DataTable();
                da.Fill(UPLOADDATATBL);
                if (UPLOADDATATBL.Rows.Count > 0)
                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                    {
                        BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                        CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                    }
                //string[,] glCoulmsD = new string[,]
                //            {                                
                //                {"tran_date",convertoDate(objCommonIUD.GetCurrentDate())},
                //                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //                {"tran_desc","ASSET MERGE- " + asset_id.ToString()  },
                //                {"tran_gl_no",glCode},
                //                {"tran_amount",MergedValues.ToString().ToString()},
                //                {"tran_acc_mode","D".ToString()},
                //                {"tran_mult","-1"},  
                //                {"tran_fc_code",BS},
                //                {"tran_cc_code",CC},                              
                //                {"tran_ou_code",branch},
                //                {"tran_product_code","999".ToString()},
                //                {"tran_ref_flag",GetRef("MOA")},
                //                {"tran_ref_gid", Assetgid },
                //                {"tran_upload_gid","0".ToString()},
                //                {"tran_isremoved","N"},
                //                {"tran_main_cat","1"},
                //                {"tran_sub_cat",subcat},
                //                {"tran_expense_category",cat},
                //                {"tran_primary_branch_code",branch.ToString()},                               
                //                {"tran_invoice_no",""},                               
                //                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //                {"tran_maker_id","".ToString()},
                //                {"tran_checker_id",objCmnFunctions.GetLoginUserGid().ToString()}
                //            };
                //string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "iem_trn_ttran");
                //string[,] glCoulmsC = new string[,]
                //            {                                
                //                {"tran_date",convertoDate(objCommonIUD.GetCurrentDate())},
                //                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //                {"tran_desc","ASSET MERGE- " + asset_id.ToString()  },
                //                {"tran_gl_no",glCode},
                //                {"tran_amount",MergedValues.ToString()},
                //                {"tran_acc_mode","C".ToString()},
                //                {"tran_mult","1"},  
                //                {"tran_fc_code",BS},
                //                {"tran_cc_code",CC},  
                //                {"tran_product_code","999".ToString()},
                //                {"tran_ou_code",branch},
                //                {"tran_ref_flag",GetRef("MOA")},
                //                {"tran_ref_gid", Assetgid},
                //                {"tran_upload_gid","0".ToString()},
                //                {"tran_isremoved","N"},
                //                {"tran_main_cat","1"},
                //                {"tran_sub_cat",subcat},
                //                {"tran_expense_category","".ToString()},
                //                {"tran_primary_branch_code",branch.ToString()},                               
                //                {"tran_invoice_no",""},                               
                //                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //                {"tran_maker_id",""},
                //                {"tran_checker_id",objCmnFunctions.GetLoginUserGid().ToString()}
                //            };
                //string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "iem_trn_ttran");
                string splimergestatus = System.Configuration.ConfigurationManager.AppSettings["WFA"].ToString();
                string approveassetmerge = System.Configuration.ConfigurationManager.AppSettings["Approvedassetmerge"].ToString();
                for (int i = 0; i < assetgid.Length; i++)
                {
                    if (assetgid[i].ToString() != "")
                    {
                        string[,] code = new string[,]
                    {                             
                             {"assetsplitmerge_new_assetid",NewAssetId.ToString()},                
                             {"assetsplitmerge_status",approveassetmerge}                                                                      
                    };
                        string[,] wher = new string[,]
                    {                             
                             {"assetsplitmerge_assetdet_gid", assetgid[i].ToString()},
                             {"assetsplitmerge_status",splimergestatus},
                    };
                        Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");
                    }
                    //Que Entry for approve
                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"queue_action_status","1"},
                        {"queue_action_date",convertoDate(objCommonIUD.GetCurrentDate().ToString())},
                        {"queue_isremoved","Y"},
                        {"queue_action_remark",remrks}
                    };
                    string[,] whercolu = new string[,]
                    {
                        {"queue_ref_gid", assetgid[i].ToString ()},
                        {"queue_ref_flag",objidm.GetRef("MOA").ToString()},                       
                        {"queue_action_status","0"}
                    };
                    Msg = objCommonIUD.UpdateCommon(colu, whercolu, "iem_trn_tqueue");
                }

                int[] DepSum = new int[500];
                string[] Depdate = new string[500];
                string[] Depgroup = new string[300];
                // int[] DepSum;
                for (int i = 0; i < assetgid.Length; i++)
                {
                    if (assetgid[i].ToString() != "")
                    {
                        int j = 0;
                        DataTable dt1 = GetDepreciation(Convert.ToInt32(assetgid[i].ToString()));
                        foreach (DataRow row in dt1.Rows)
                        {
                            DepSum[j] = DepSum[j] + Convert.ToInt32(row["depreciation_amount"]);
                            Depdate[j] = row["depreciation_month"].ToString();
                            Depgroup[j] = row["depreciation_asset_groupid"].ToString();
                            j++;
                        }
                    }
                }
                //Insert Depreciation
                for (int i = 0; i < DepSum.Length; i++)
                {
                    if (Depdate[i] == null)
                    {
                        Depdate[i] = "";
                    }
                    if (Depgroup[i] == null)
                    {
                        Depgroup[i] = "";
                    }
                    if (Depdate[i] != null && Depgroup[i] != null && Depgroup[i] != "0" && Depdate[i] != "" && Depgroup[i] != "")
                    {
                        string[,] cod = new string[,]
                             {
                             {"depreciation_assetdet_gid",Assetgid},
                             {"depreciation_amount",DepSum[i].ToString()},
                             {"depreciation_month ",Depdate[i].ToString()},
                             {"depreciation_asset_groupid",Depgroup[i]},
                             {"depreciation_asset_owner","F"},
                             {"depreciation_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"depreciation_insert_date",convertoDate(objCommonIUD.GetCurrentDate())},
                             };
                        string tblnam = "fa_trn_tdepreciation";

                        Msg = objCommonIUD.InsertCommon(cod, tblnam);
                    }
                    //ENTRY FOR GL UPLOAD

                    //if (DepSum[i].ToString() != "0")
                    //{
                    //    string[,] glCoulmsDepD = new string[,]
                    //        {                             
                    //            {"tran_date",convertoDate(objCommonIUD.GetCurrentDate())},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_gl_no",glCode},
                    //            {"tran_desc","DEP FOR THE MONTH OF " + Format(convertoDate(Depdate[i]).ToString()) + " - FOR THE ASSET :  " + asset_id.ToString()},
                    //            {"tran_amount",DepSum[i].ToString()},
                    //            {"tran_acc_mode","D".ToString()},
                    //            {"tran_mult","-1"},  
                    //            {"tran_product_code","999".ToString()},
                    //            {"tran_fc_code",BS},
                    //            {"tran_cc_code",CC},
                    //            {"tran_ou_code",branch},
                    //            {"tran_ref_flag",GetRef("DEP")},
                    //            {"tran_ref_gid",Assetgid},
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //            {"tran_expense_category",cat},
                    //            {"tran_primary_branch_code",branch.ToString()},                                
                    //            {"tran_invoice_no",""},                                
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                    //            {"tran_checker_id",objCmnFunctions.GetLoginUserGid().ToString()}
                    //        };
                    //    string insertforglDepD = objCommonIUD.InsertCommon(glCoulmsDepD, "iem_trn_ttran");
                    //    string[,] glCoulmsDepC = new string[,]
                    //        {                                
                    //            {"tran_date",convertoDate(objCommonIUD.GetCurrentDate())},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_desc","DEP FOR THE MONTH OF " + Format(convertoDate(Depdate[i]).ToString()) + " - FOR THE ASSET :  " + asset_id.ToString()},
                    //            {"tran_gl_no",glCode},
                    //            {"tran_amount",DepSum[i].ToString()},
                    //            {"tran_acc_mode","C".ToString()},
                    //            {"tran_mult","1"},  
                    //            {"tran_fc_code",BS},
                    //            {"tran_product_code","999".ToString()},
                    //            {"tran_cc_code",CC},                                
                    //            {"tran_ou_code",branch},
                    //            {"tran_ref_flag",GetRef("DEP")},
                    //            {"tran_ref_gid",Assetgid},
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //            {"tran_expense_category",cat},
                    //            {"tran_primary_branch_code",branch.ToString()},
                    //            {"tran_invoice_no",""},                               
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                    //            {"tran_checker_id",objCmnFunctions.GetLoginUserGid().ToString()}
                    //        };
                    //    string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "iem_trn_ttran");
                    //}
                }
                //Delete Depreciation
                //for (int i = 0; i < assetgid.Length; i++)
                //{
                //    if (assetgid[i].ToString() != "")
                //    {
                //        Depdelete(Convert.ToInt32(assetgid[i].ToString()));
                //    }
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Msg;
        }
        */
        #endregion

        public string RejectMerge(string[] Assetgid, string remarks)
        {
            string Result = string.Empty;
            try
            {
                string oldid = string.Join(",", Assetgid.ToArray());
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fa_set_MergechkrApprove", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Oldassetid", SqlDbType.VarChar).Value = oldid;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = remarks.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "REJECTED";
                cmd.Parameters.Add("@Userid", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message.ToString();
            }
            finally
            {

                GetOffConnection();

            }
            return Result;
        }

        #region
        /*
        public string RejectMerge(string[] Assetgid, string remarks)
        {
            string Msg = string.Empty;
            try
            {
                for (int i = 0; i < Assetgid.Length; i++)
                {
                    string approvevalue = System.Configuration.ConfigurationManager.AppSettings["WFA"].ToString();
                    string rejectvalue = System.Configuration.ConfigurationManager.AppSettings["RejectAssetmrge"].ToString();
                    if (Assetgid[i].ToString() != "")
                    {
                        string[,] code = new string[,]
                    {                             
                             
                             {"assetsplitmerge_status",rejectvalue}                     
                    
                            
                    };
                        string[,] wher = new string[,]
                    { 
                            
                             {"assetsplitmerge_assetdet_gid", Assetgid[i].ToString()},
                             {"assetsplitmerge_status",approvevalue}
                    };

                        Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetsplitmerge");
                    }
                }
                for (int i = 0; i < Assetgid.Length; i++)
                {
                    // string approvevalue = System.Configuration.ConfigurationManager.AppSettings["Approvedassetmerge"].ToString();
                    // string rejectvalue = System.Configuration.ConfigurationManager.AppSettings["RejectAssetmrge"].ToString();
                    if (Assetgid[i].ToString() != "")
                    {
                        string[,] code = new string[,]
                    {                             
                             
                             {"assetdetails_takenby","14"} ,
                             {"assetdetails_active_status","Y"}
                    
                            
                    };

                        string[,] wher = new string[,]
                    { 
                            
                             {"assetdetails_gid", Assetgid[i].ToString()},
                             {"assetdetails_takenby","15"}
                             
                    };

                        Msg = objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                    }


                    //que entry
                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"queue_action_status","2"},
                        {"queue_action_date",convertoDate(objCommonIUD.GetCurrentDate())},
                        {"queue_isremoved","Y"},
                        {"queue_action_remark",remarks}
                    };
                    string[,] Whercolu = new string[,]
                    {
                        {"queue_ref_gid", Assetgid[i]},
                        {"queue_ref_flag",objidm.GetRef("MOA").ToString()},
                        {"queue_action_status","0"}
                    };
                    Msg = objCommonIUD.UpdateCommon(colu, Whercolu, "iem_trn_tqueue");
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }

            return Msg;
        }
        */
        #endregion
        public string Format(string date)
        {
            try
            {
                GetConnection();
                string date1 = objCmnFunctions.convertoDateTimeString(date);
                string[] array = date1.Split('-');
                date = array[1] + "-" + array[2];
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return date;
        }
        public List<AssetParentModel> AutoGroupid(string term)
        {
            List<AssetParentModel> Model = new List<AssetParentModel>();
            AssetParentModel obj = new AssetParentModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_assetmerging", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AssetIdInGroup", SqlDbType.VarChar).Value = "autoGroupid";
                cmd.Parameters.Add("@term", SqlDbType.VarChar).Value = term;

                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetParentModel();
                    obj.Assetgrpid = row["assetdetails_asset_groupid"].ToString();
                    Model.Add(obj);
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

            return Model;
        }

    }
}