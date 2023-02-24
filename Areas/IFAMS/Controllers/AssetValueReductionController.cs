using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class assetvaluereductionController : Controller
    {
        private IfamsAssetVRRepository_M ivr;
        private CmnFunctions objcmnAssetVR = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public assetvaluereductionController() : this(new IfamsAssetVRDataModel_M()) { }
        public assetvaluereductionController(IfamsAssetVRRepository_M objAssetVRModel)
        {
            ivr = objAssetVRModel;
        }
        //
        // GET: /IFAMS/assetvaluereduction/

        public ActionResult ValueReductionSummary()
        {
            try
            {
                Session["values"] = null;
                AssetVRModel VRrecord = new AssetVRModel();
                VRrecord.VRModel = ivr.GetMakerReduction(Convert.ToString(objcmnAssetVR.GetLoginUserGid())).ToList();
                if (VRrecord.VRModel.Count == 0) { ViewBag.Message = "No records found"; ViewBag.status = "WAITING FOR APPROVAL"; }
                else
                {
                    ViewBag.status = "WAITING FOR APPROVAL";
                }
                Session["AVRExceldownload"] = VRrecord.VRModel;
                return View(VRrecord);
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

        [HttpPost]
        public ActionResult ValueReductionSummary(string vrfilterid, string status)
        {
            AssetVRModel vrdetails = new AssetVRModel();
            List<AssetVRModel> vrdetail = new List<AssetVRModel>();
            try
            {
                vrdetails.VRModel = ivr.GetVRDetailsearch(vrfilterid).ToList();
                if (vrfilterid != null)
                {
                    ViewBag.vrfilterid = vrfilterid;
                    vrdetails.VRModel = vrdetails.VRModel.Where(x => vrfilterid.ToUpper() == null || (x.assetdetDetid.Contains(vrfilterid.ToUpper()))).ToList();
                }                
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    vrdetails.VRModel = vrdetails.VRModel.Where(x => status.ToUpper() == null || (x.avrstatus.Equals(status.ToUpper()))).ToList();
                }                
                if (vrdetails.VRModel.Count == 0)
                {
                    vrdetails.VRModel = Enumerable.Empty<AssetVRModel>().ToList<AssetVRModel>();
                    ViewBag.Message = "No Records Found";
                }
                Session["AVRExceldownload"] = vrdetails.VRModel;
                return View(vrdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(vrdetails);
            }
            finally
            {

            }

        }

        public ActionResult ValueReduction()
        {
            try
            {
                AssetVRModel VRrecord = new AssetVRModel();
                if (Session["values"] != null)
                {
                    VRrecord.VRModel = (List<AssetVRModel>)Session["values"];
                }
                else
                {
                    VRrecord.VRModel = Enumerable.Empty<AssetVRModel>().ToList<AssetVRModel>();
                    ViewBag.Message = "No records found";
                }
                return View(VRrecord);
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

        [HttpPost]
        public ActionResult ValueReduction(string command, string vrlocation, string vrfilter1, FormCollection collections)
        {
            AssetVRModel vrdetails = new AssetVRModel();
            try
            {
                if (command == "SEARCH")
                {
                    vrdetails.VRModel = ivr.GetVRDetailsearch(vrfilter1, vrlocation).ToList();
                   
                    ViewBag.vrlocation = vrlocation;
                    ViewBag.vrfilter1 = vrfilter1;
                    if (vrdetails.VRModel.Count == 0)
                    {
                        vrdetails.VRModel = Enumerable.Empty<AssetVRModel>().ToList<AssetVRModel>();
                        ViewBag.Message = "No Records Found";
                    }
                }
                Session["values"] = vrdetails.VRModel;

                if (command == "CLEAR")
                {
                    Session["values"] = null;
                    return RedirectToAction("ValueReduction", "AssetValueReduction");
                }
                return View(vrdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return View(vrdetails);
            }
            finally
            {

            }
        }

        [HttpPost]
        public JsonResult ReductionSave(Models.AssetVRModel status)
        {
            string Result = string.Empty;
            string Maker = objcmnAssetVR.authorize("383");
            try
            {
                if (Maker == "Success")
                {
                    ivr.Updateasset(status);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else { return Json(Maker, JsonRequestBehavior.AllowGet); }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
        }

        [HttpPost]
        public JsonResult locautosearch(string term)
        {
            try
            {
                List<AssetVRModel> autoloc = new List<AssetVRModel>();
                autoloc = ivr.VRAutoBranch(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
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
        public JsonResult locautoassetid(string term)
        {
            try
            {
                List<AssetVRModel> autoloc = new List<AssetVRModel>();
                autoloc = ivr.VRAutoAsset(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
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



        // [HttpGet]
        public PartialViewResult ValueReductionViewdetails(string VRassetGid, string viewfor, FormCollection collections, SaleMakerModel status)
        {
            try
            {

                AssetVRModel vrlist = new AssetVRModel();
                vrlist.VRModel = ivr.GetVRDetail(VRassetGid).ToList();

                if (viewfor == "checkerView")
                {
                    ViewBag.viewfor = "checkerView";
                }
                if (viewfor == "abort")
                {
                    ViewBag.viewfor = "abort";
                }
                if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                if (vrlist.VRModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(vrlist);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }
        }


        [HttpPost]
        public JsonResult ValueReductionView(string command, string id, string remarks)
        {
            string result = "";
            try
            {

                if (command == "abort")
                {
                    result = ivr.AbortVR(id);
                    ViewBag.viewfor = "abort";
                    if (result == "success")
                    {
                        result = "success";
                    }
                    else
                    {
                        result = "Fail";
                    }

                }
                
                if (command == "APPROVE")
                {
                    if (id != "")
                    {
                        // Session[""] = id;
                        result = ivr.VRChkApprovalStatus(id, "APPROVED", remarks);
                        if (result == "success")
                            if (result == "success")
                            {
                                result = "success";
                            }
                            else
                            {
                                result = "Fail";
                            }

                    }
                }
                if (command == "REJECT")
                {
                    if (id != "")
                    {
                        // Session["Saleno"] = id;
                        result = ivr.VRChkApprovalStatus(id, "REJECTED", remarks);
                        if (result == "success")
                        {
                            result = "success";
                        }
                        else
                        {
                            result = "Fail";
                        }

                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        //-------------------------CHECKER----------------------

        public ActionResult ValueReductionChkSummary()
        {
            try
            {
                AssetVRModel VRrecord = new AssetVRModel();
                VRrecord.VRModel = ivr.GetCheckerReduction(Convert.ToString(objcmnAssetVR.GetLoginUserGid())).ToList();
                if (VRrecord.VRModel.Count == 0) { ViewBag.Mesage = "No records found"; }

                return View(VRrecord);
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

        [HttpPost]
        public ActionResult ValueReductionChkSummary(string vrchkfilterid)
        {
            AssetVRModel vrdetails = new AssetVRModel();
            List<AssetVRModel> vrdetail = new List<AssetVRModel>();
            try
            {

                vrdetails.VRModel = ivr.GetVRchkDetailsearch(vrchkfilterid.Trim()).ToList();
                
                if (vrdetails.VRModel.Count == 0)
                {
                    vrdetails.VRModel = Enumerable.Empty<AssetVRModel>().ToList<AssetVRModel>();
                    ViewBag.Message = "No Records Found";
                }
                return View(vrdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(vrdetails);

            }
            finally
            {

            }

        }


        public JsonResult ReductionStatus()
        {

            IfamsAssetVRDataModel_M Model = new IfamsAssetVRDataModel_M();

            try
            {
                return Json(Model.GetVRStatus(), JsonRequestBehavior.AllowGet);
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
        public ActionResult AVRExceldownload()
        {

            List<AssetVRModel> cList;
            cList = (List<AssetVRModel>)Session["AVRExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Reduction Number");
            dt.Columns.Add("Asset Id");
            dt.Columns.Add("Reduced Amount");
            dt.Columns.Add("Old Asset Value");
            dt.Columns.Add("New Asset Value");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].assetrefno.ToString()
                , cList[i].assetdetDetid.ToString()
                , cList[i].assetreducedamt.ToString()
                , cList[i].assetreduval.ToString()
                , cList[i].assetnewval.ToString());
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Asset Value Reduction Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
