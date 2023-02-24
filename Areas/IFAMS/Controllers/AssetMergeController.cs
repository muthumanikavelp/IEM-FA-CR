using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data;
using System.Web.UI.WebControls;

namespace IEM.Areas.IFAMS.Controllers
{
    public class AssetMergeController : Controller
    {
        //
        // GET: /IFAMS/AssetMerge/
        private IfamsAssetMergeRepository AssetMergeR;
        public AssetMergeController() : this(new IfamsAssetMergeDataModel()) { }
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();

        public AssetMergeController(IfamsAssetMergeRepository objModel)
        {
            AssetMergeR = objModel;
        }


        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AssetMerge()
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();


            try
            {

                Records.AssetParentList = Model.GetAssetParent();
                if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                return View(Records);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }



        }
        [HttpGet]
        public ActionResult AssetMergeSummary()
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                Records.AssetParentSummary = Model.GetAssetParentSummary();
                if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }
                ViewBag.UserRole = "Maker";






            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View("AssetMergeSummary", Records);

        }

        public ActionResult Assetcheckersummary()
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> get = new List<AssetParentModel>();
            try
            {
                get = AssetMergeR.GetAssettmergeCheckerDetails().ToList();
                //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                // return View(Records);
                if (get.Count == 0) { ViewBag.Message = "No records found"; }
                return View(get);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult Assetcheckersummary(string txtgroupid, string txtbranchid, string command)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> get = new List<AssetParentModel>();
            try
            {
                if (command == "SEARCH")
                {
                    ViewBag.txtgroupid = txtgroupid;
                    ViewBag.txtbranchid = txtbranchid;
                    get = AssetMergeR.getsearchckeckerdetails(txtgroupid, txtbranchid).ToList();
                    //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                    // return View(Records);
                    if (get.Count == 0) { ViewBag.Message = "No records found"; }
                }
                else
                {
                    ViewBag.txtgroupid = "";
                    ViewBag.txtbranchid = "";
                    get = AssetMergeR.getsearchckeckerdetails(txtgroupid, txtbranchid).ToList();
                    //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                    // return View(Records);
                    if (get.Count == 0) { ViewBag.Message = "No records found"; }
                }
                return View(get);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return View();
        }

        [HttpGet]
        public PartialViewResult ViewAssetmergesummary(string id, string name)
        {
            if (name == "DRAFT")
            {
                name = "1";
            }
            else if (name == "WAITING")
            {
                name = "2";

            }
            else if (name == "APPROVED")
            {
                name = "3";
            }
            else if (name == "REJECTED")
            {
                name = "4";
            }
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> get = new List<AssetParentModel>();
            try
            {
                get = AssetMergeR.GetAssetMergemakersummarynew(id, name).ToList();
                //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                // return View(Records);
                if (get.Count == 0) { ViewBag.Message = "No records found"; }
                // return PartialView(get);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return PartialView(get);

        }

        public PartialViewResult ApproveMerge(string id)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> get = new List<AssetParentModel>();
            try
            {
                ViewBag.approveid = id;
                //get = AssetMergeR.GetAssetMergemakersummary(id).ToList();

                //********* MUTHU ************
                string status = "2";
                get = AssetMergeR.GetAssetMergecheckersummarynew(id, status);

                //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                // return View(Records);
                if (get.Count == 0) { ViewBag.Message = "No records found"; }
                // return PartialView(get);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return PartialView(get);
        }
        public ActionResult AssetMergeSummaryy()
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                Records.AssetParentSummary = Model.CheckerSummary();
                if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }
                Session["UserRole"] = "Checker";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View(Records);

        }

        [HttpPost]
        public ActionResult AssetMergeSummary(string filter, string command)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                //Records.AssetParentSummary = Model.GetAssetParentSummary();
                //if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }



                if (command == "SEARCH")
                {

                    Records.AssetParentSummary = Model.GetAssetParentSummary();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)))
                    {
                        Records.AssetParentSummary = Model.GetAssetParentSummary();
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        Records.AssetParentSummary = Records.AssetParentSummary.Where(x => filter == null || (x.AssetId.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }

                    if (Records.AssetParentSummary.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View(Records);

        }


        [HttpPost]
        public ActionResult AssetMergeSummaryy(string filter, string command)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                //Records.AssetParentSummary = Model.GetAssetParentSummary();
                //if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }

                if (command == "SEARCH")
                {

                    Records.AssetParentSummary = Model.GetAssetParentSummary();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)))
                    {
                        Records.AssetParentSummary = Model.GetAssetParentSummary();
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        Records.AssetParentSummary = Records.AssetParentSummary.Where(x => filter == null || (x.AssetId.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }

                    if (Records.AssetParentSummary.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View(Records);

        }

        public JsonResult Approve(string Approve)
        {
            string Msg = "";
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();

            try
            {
                int ParentId = (int)Session["ParentId"];
                if (Approve.ToString() == "Approve")
                {

                    Msg = Model.AppChecker(ParentId);

                }


            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Reject(string Reject)
        {
            string Msg = "";
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();

            try
            {
                int ParentId = (int)Session["ParentId"];
                if (Reject.ToString() == "Reject")
                {

                    Msg = Model.RejectChecker(ParentId);

                }
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public PartialViewResult AssetParent()
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();

            try
            {

                Records.AssetParentList = Model.GetAssetParent();

                Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return PartialView("AssetParent", Records);
        }




        [HttpGet]
        public PartialViewResult ViewMerge(string viewfor, string id)
        {

            string Status = "";

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            Session["ParentId"] = Convert.ToInt32(id.ToString());

            //if (status.ToString() == "WAITING")
            //{
            //    Status = "WAITING FOR APPROVAL";

            //}
            //else if (status.ToString() == "APPROVED")
            //{
            //    Status = "APPROVED";

            //}
            //  Session["AuditStatus"] = Status.ToString();

            try
            {


                Status = Model.GetViewMergeStatus(Convert.ToInt32(id));
                if (viewfor == "Maker")
                {
                    ViewBag.UserRole = "Maker";
                    Records.AssetParentSummary = Model.ViewMergedDet(Convert.ToInt32(id));
                    if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }
                }
                else if (viewfor == "Checker")
                {
                    ViewBag.UserRole = "Checker";
                    Records.AssetParentSummary = Model.ViewMergedDet(Convert.ToInt32(id));
                    if (Records.AssetParentSummary.Count == 0) { ViewBag.Message = "No records found"; }
                    if (Status.ToString() == "REJECTED" || Status.ToString() == "APPROVED")
                    {
                        ViewBag.Status = Status.ToString();
                    }
                    else if (Status.ToString() == "WAITING FOR APPROVAL")
                    {

                        ViewBag.Status = "WAITING FOR APPROVAL";

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
            return PartialView(Records);
        }



        [HttpPost]
        public ActionResult AssetParent(AssetParentModel Model)
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel obj = new IfamsAssetMergeDataModel();

            try
            {



                if (Model.command == "SEARCH")
                {

                    Records.AssetParentList = AssetMergeR.GetAssetIdParent(Model.AssetId);
                    if ((string.IsNullOrEmpty(Model.AssetId) || string.IsNullOrWhiteSpace(Model.AssetId)))
                    {
                        Records.AssetParentList = AssetMergeR.GetAssetIdParent(Model.AssetId);
                    }
                    if (Model.AssetId != null)
                    {


                        Records.AssetParentList = AssetMergeR.GetAssetIdParent(Model.AssetId.Trim());
                        Records.AssetParentList = Records.AssetParentList.Where(x => Model.AssetId == null || (x.AssetId.ToUpper().Contains(Model.AssetId.ToUpper()))).ToList();

                        Session["ParentID"] = obj.ParentId(Model.AssetId);
                    }

                    if (Records.AssetParentList.Count == 0)
                    {
                        ViewBag.Message = "No records found";
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

            return PartialView("AssetParent", Records);
        }




        [HttpGet]
        public PartialViewResult AssetChild()
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();


            try
            {


                Records.AssetParentList = Model.GetAssetParent();
                if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return PartialView(Records);
            // return PartialView("FCCCSearch", objGetPrDetails);

        }

        [HttpPost]
        public JsonResult GetCheckedId(int[] ids)
        {

            var Msg = "Success";

            return Json(Msg, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetCheckValues(string ids)
        {
            string Msg = "";

            string CheckedIds = "";
            string[] CheckedValues;
            IfamsAssetMergeDataModel obj = new IfamsAssetMergeDataModel();
            string ChildAssetValues = "";
            int index = ids.IndexOf("undefined");
            if (index != -1)
            {
                CheckedIds = ids.Remove(index);
            }
            int ParentId = 0;
            if (CheckedIds.ToString() != "")
            {

                CheckedValues = CheckedIds.Split(',').ToArray();

                if (Session["ParentID"] != null)
                {

                    ParentId = (int)Session["ParentID"];
                    for (int i = 0; i < CheckedValues.Length; i++)
                    {

                        if (CheckedValues[i].ToString() == Convert.ToString(ParentId.ToString()))
                        {
                            Msg = "Parent can not be a Child";
                        }

                    }

                    if (Msg != "Parent can not be a Child")
                    {
                        for (int i = 0; i < CheckedValues.Length; i++)
                        {
                            if (CheckedValues[i].ToString() != "")
                                if (CheckedValues[i].ToString() != Convert.ToString(ParentId.ToString()))
                                    ChildAssetValues += obj.GetChildValues(Convert.ToInt32(CheckedValues[i])) + ",";
                        }
                        Msg = obj.InsertMerge(ChildAssetValues, ParentId, CheckedValues);

                    }

                }
                else
                {
                    Msg = "There is no Parent to Merge";

                }
            }
            else
            {

                Msg = "Please Select Child to Merge";

            }
            return Json(Msg, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult AssetChild(AssetParentModel Model)
        {

            AssetParentModel Records = new AssetParentModel();
            try
            {

                //ViewBag.Childsearch = Model.AssetId;
                //ViewBag.command = "SEARCH";

                if (Model.command == "SEARCH")
                {
                    Records.AssetParentList = AssetMergeR.GetAssetIdChild();
                    if ((string.IsNullOrEmpty(Model.AssetId) || string.IsNullOrWhiteSpace(Model.AssetId)))
                    {
                        Records.AssetParentList = AssetMergeR.GetAssetIdChild();
                    }
                    if (Model.AssetId != null)
                    {
                        ViewBag.filter = Model.AssetId;
                        Records.AssetParentList = AssetMergeR.GetAssetIdChild();
                        Records.AssetParentList = Records.AssetParentList.Where(x => Model.AssetId == null || (x.AssetId.ToUpper().Contains(Model.AssetId.ToUpper()))).ToList();
                        //Session["Childlist"] = Records.AssetParentList.ToList();
                    }

                    if (Records.AssetParentList.Count == 0)
                    {
                        ViewBag.Message = "No records found";
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

            return PartialView("AssetChild", Records);
        }

        public PartialViewResult MergeAuditTrial()
        {

            return PartialView();
        }

        public JsonResult BacktoSummary()
        {
            string Msg = "Success";
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Merging
        /// </summary>
        /// <returns></returns>


        public ActionResult AssetMerging()
        {

            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> get = new List<AssetParentModel>();
            try
            {

                // Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                get = AssetMergeR.GetAssettmergeDetails().ToList();
                //if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                // return View(Records);
                if (get.Count == 0) { ViewBag.Message = "No records found"; }
                Session["AssetMergeExceldownload"] = get;
                return View(get);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }

        }

        public PartialViewResult MergeSearch()
        {
            return PartialView();
        }
        public PartialViewResult Mergecheckersummary()
        {
            return PartialView();
        }
        public PartialViewResult MergeSer()
        {
            return PartialView();
        }

        public ActionResult MergeSearch1()
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            //List<AssetParentModel> get = new List<AssetParentModel>();


            try
            {

                Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                //get = AssetMergeR.GetAssettmergeDetails().ToList();
                if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }
                return View(Records);
                //if (get.Count == 0) { ViewBag.Message = "No records found"; }
                // return View(get);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
        }

        [HttpGet]
        public PartialViewResult MergerSearchClear()
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();


            try
            {

                Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                if (Records.AssetParentList.Count == 0) { ViewBag.Message = "No records found"; }



            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return PartialView("MergeSearch", Records);
        }



        [HttpGet]
        public PartialViewResult MergeSearchGroupId(AssetParentModel Mod)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();


            try
            {
                if (!string.IsNullOrEmpty(Mod.AssetId))
                {
                    Records.AssetParentList = Model.GetAssetInGroup(Mod.AssetId.ToString(), "");
                    Session["AssetId"] = Mod.AssetId.ToString();
                }
                else if (!string.IsNullOrEmpty(Mod.Assetgrpid))
                {
                    Records.AssetParentList = Model.GetAssetInGroup("", Mod.Assetgrpid.ToString());
                    Session["AssetId"] = Mod.Assetgrpid.ToString();
                }

                //else {
                //    Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                //}
                // if (Records.AssetParentList.Count == 0 || Convert.ToString(Records.AssetParentList.Count)=="" ) {
                else
                {
                    ViewBag.Message = "No records found";
                    Records.AssetParentList = Enumerable.Empty<AssetParentModel>().ToList<AssetParentModel>();
                }


                //return PartialView("MergeSearch", Records);
                return PartialView("MergeSer", Records);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return PartialView("MergeSearch");
                return PartialView("MergeSer");
            }
            finally
            {
            }
            //return PartialView("MergeSearch");
        }

        [HttpPost]
        public ActionResult AssetMerging(string txtgroupid, string txtbranchlocation, string command = null)
        {
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            List<AssetParentModel> Records1 = new List<AssetParentModel>();

            try
            {
                if (command == "search")
                {
                    ViewBag.txtgroupid = txtgroupid;
                    ViewBag.txtbranchlocation = txtbranchlocation;
                    Records1 = AssetMergeR.Getsearchmakersummary(txtgroupid, txtbranchlocation);
                    Session["AssetId"] = txtgroupid;
                    if (Records1.Count == 0) { ViewBag.Message = "No records found"; }
                }
                else
                {
                    ViewBag.txtgroupid = "";
                    ViewBag.txtbranchlocation = "";
                    Records1 = AssetMergeR.Getsearchmakersummary(txtgroupid, txtbranchlocation);
                    Session["AssetId"] = txtgroupid;
                    if (Records1.Count == 0) { ViewBag.Message = "No records found"; }
                }
                Session["AssetMergeExceldownload"] = Records1;
                return View(Records1);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(Records1);
            }
            finally
            {
            }
            //return PartialView("MergeSearch");
        }

        [HttpPost]
        public JsonResult AutoAssetId(string term)
        {

            List<AssetParentModel> AssetList = new List<AssetParentModel>();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();

            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                AssetList = Model.AutoAssetId(term).ToList();
                return Json(AssetList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpPost]
        public JsonResult AssetGroupMerging(string ids)
        {
            string Msg = "Success";
            List<AssetParentModel> AssetList = new List<AssetParentModel>();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            string CheckedIds = "";
            string[] CheckedValues;
            IfamsAssetMergeDataModel obj = new IfamsAssetMergeDataModel();
            //string ChildAssetValues = "";

            int index = ids.IndexOf("undefined");

            try
            {

                if (index != -1)
                {
                    CheckedIds = ids.Remove(index);
                }
                if (CheckedIds.ToString() != "")
                {
                    CheckedValues = CheckedIds.Split(',').ToArray();
                    string AssetId = Session["AssetId"].ToString();
                    // Msg = Model.CheckPhyAsset(AssetId.ToString());

                    var GroupList = Model.ValidateGroup(CheckedValues);

                    var GroupList1 = GroupList;

                    foreach (var g in GroupList)
                    {

                        foreach (var g1 in GroupList1)
                        {

                            if (g.Location != g1.Location)
                            {

                                Msg = "Location Mismatch";
                            }

                            if (g.AssetSubCode != g1.AssetSubCode)
                            {

                                Msg = "Assetcode Mismatch";
                            }

                            if (g.Assetgrpid != g1.Assetgrpid)
                            {
                                Msg = "AssetGroupid Mismatch";
                            }

                            if (g.Physicalid != "0" && g.Physicalid != "")
                            {
                                if (g.Physicalid != g1.Physicalid)
                                {

                                    Msg = "Physical id Mismatch";
                                }
                            }

                            if (g.date != g1.date)
                            {

                                Msg = "Capitalization Date Mismatch";
                            }
                        }

                    }


                }
                else
                {
                    Msg = "Please Check Assets to Merge";

                }

                return Json(Msg, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }


        public ActionResult Assetmergi()
        {

            return View();
        }


        [HttpGet]
        public PartialViewResult MergedAsset()
        {


            return PartialView();
        }

        [HttpPost]
        public JsonResult GetMergedAsset(string ids = null)
        {
            string Msg = "";
            string Maker = string.Empty;
            Maker = objCmnFunctions.authorize("242");
            List<AssetParentModel> AssetList = new List<AssetParentModel>();
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            string CheckedIds = "";
            string[] CheckedValues;
            IfamsAssetMergeDataModel obj = new IfamsAssetMergeDataModel();
            //string ChildAssetValues = "";
            int index = ids.IndexOf("undefined");

            try
            {

                if (index != -1)
                {
                    CheckedIds = ids.Remove(index);
                }

                if (CheckedIds.ToString() != "")
                {
                    CheckedValues = CheckedIds.Split(',').ToArray();
                    Session["CheckedIds"] = CheckedValues;
                    string AssetId = Session["AssetId"].ToString();
                    if (Maker == "Success")
                    {
                        Msg = Model.GetAssetMerging(AssetId, CheckedValues);
                        if (Msg == "success")
                        {

                            Msg = "Successfully Merged";
                        }
                    }
                    else
                    {
                        Msg = "Unauthorized User!";
                    }
                }
                else
                {
                    Msg = "Please Check Assets to Merge";

                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AssetcheckerMergedetails(string getassetsubcode, string[] getIds, string Assetremarks)
        {

            string Msg = string.Empty;
            string Checker = objCmnFunctions.authorize("243");
            List<AssetParentModel> AssetList = new List<AssetParentModel>();
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            if (Checker == "Success")
            {
                if (getIds.ToString() != "")
                {

                    // CheckedValues = CheckedIds.Split(',').ToArray();
                    Session["CheckedIds"] = getIds;
                    //string AssetId = Session["AssetId"].ToString();
                    Msg = Model.GetAssetMerging1(getassetsubcode, getIds, Assetremarks);
                    if (Msg == "success")
                    {

                        Msg = "Successfully Approved Merge";
                    }


                }
                else
                {
                    Msg = "Please Check Assets to Merge";

                }
            }
            else
            {
                Msg = "U";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AssetrejectMergedetails(string[] getIds, string Assetremarks)
        {

            string Msg = string.Empty;
            string Checker = objCmnFunctions.authorize("243");
            List<AssetParentModel> AssetList = new List<AssetParentModel>();
            AssetParentModel Records = new AssetParentModel();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            if (Checker == "Success")
            {
                if (getIds.ToString() != "")
                {

                    Msg = Model.RejectMerge(getIds, Assetremarks);
                    if (Msg == "Success")
                    {

                        Msg = "Successfully Reject Merge";
                    }


                }
                else
                {
                    Msg = "Please Check Assets to Merge";

                }
            }
            else { Msg = "U"; }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AutoPopGroupid(string term)
        {
            try
            {
                List<AssetParentModel> AssetList = new List<AssetParentModel>();
                IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
                AssetList = Model.AutoGroupid(term).ToList();
                return Json(AssetList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }
        public ActionResult AssetMergeExceldownload()
        {

            List<AssetParentModel> cList;
            cList = (List<AssetParentModel>)Session["AssetMergeExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Asset Sub-Category Code");
            dt.Columns.Add("Location");
            dt.Columns.Add("Status");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].AssetSubCode.ToString()
                , cList[i].Location.ToString()
                , cList[i].status.ToString()
                );
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Asset Merge Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
