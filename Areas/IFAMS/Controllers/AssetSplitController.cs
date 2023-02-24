using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetSplitController : Controller
    {
        private IfamsSplitRepository_M isr;
        private CmnFunctions objcmnSplit = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        ErrorLog objErrorLog = new ErrorLog();
        public AssetSplitController() : this(new IfamsSplitDataModel_M()) { }
        public AssetSplitController(IfamsSplitDataModel_M objsplitModel)
        {
            isr = objsplitModel;
        }

        ////public ActionResult Index()
        ////{
        ////    return View();
        ////}



        public ActionResult Split()
        {
            try
            {
                AssetSplitModel splitrecord = new AssetSplitModel();
                splitrecord.SplitModel = isr.GetSplit(Convert.ToString(objcmnSplit.GetLoginUserGid())).ToList();
                if (splitrecord.SplitModel.Count == 0) { ViewBag.Mesage = "No records found"; }
                Session["SplitExceldownload"] = splitrecord.SplitModel;
                return View(splitrecord);

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
        public ActionResult Split(string spfilter, string spfilter1, FormCollection collections, string command)
        {
            AssetSplitModel splitrec = new AssetSplitModel();
            try
            {
                splitrec.SplitModel = Enumerable.Empty<AssetSplitModel>().ToList<AssetSplitModel>();

                if (command == "SEARCH")
                {
                    //splitrec.SplitModel = isr.GetAssetDetailChk().ToList();
                    splitrec.SplitModel = isr.GetSplit(Convert.ToString(objcmnSplit.GetLoginUserGid())).ToList();
                    if (spfilter != null && spfilter != "")
                    {
                        ViewBag.spfilter = spfilter;
                        splitrec.SplitModel = splitrec.SplitModel.Where(x => spfilter.ToUpper() == null || (x.assetdetDescription.ToUpper().Contains(spfilter.ToUpper()))).ToList();

                    }

                    if (spfilter1 != null && spfilter1 != "")
                    {
                        ViewBag.spfilter1 = spfilter1;
                        splitrec.SplitModel = splitrec.SplitModel.Where(x => spfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(spfilter1.ToUpper()))).ToList();
                    }
                    if (splitrec.SplitModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }


                }

                if (command == "Clear")
                {
                    return RedirectToAction("Split", "AssetSplit");
                }
                Session["SplitExceldownload"] = splitrec.SplitModel;
                return View(splitrec);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return View(splitrec);
            }
            finally
            {

            }
        }



        // [HttpPost]
        public ActionResult SplitSummary()
        {
            try
            {
                AssetSplitModel splitrecords = new AssetSplitModel();
                //splitrecords.SplitModel = isr.GetSplitDetail(Convert.ToString(objcmnSplit.GetLoginUserGid())).ToList();
                // (splitrecords.SplitModel.Count == 0) { ViewBag.Message = "No records found"; }
                splitrecords.SplitModel = Enumerable.Empty<AssetSplitModel>().ToList<AssetSplitModel>();
                ViewBag.Message = "No records found";
                return View(splitrecords);
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
        public ActionResult SplitSummary(string splitfilter, string splitfilter1, FormCollection collections, string command)
        {
            AssetSplitModel assetdeta = new AssetSplitModel();

            try
            {
                if (command == "SEARCH")
                {

                    assetdeta.SplitModel = isr.GetAssetDetail(splitfilter1.Trim()).ToList();
                    if (assetdeta.SplitModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    else
                    {
                        assetdeta.assetdetAssetvalue = assetdeta.SplitModel[0].assetdetAssetvalue;
                        assetdeta.assetdetCode = assetdeta.SplitModel[0].assetdetCode;
                        assetdeta.assetdetLocationcode = assetdeta.SplitModel[0].assetdetLocationcode;
                        assetdeta.assetdetId = assetdeta.SplitModel[0].assetdetId;
                        assetdeta.locationId = assetdeta.SplitModel[0].locationId;
                        assetdeta.Tdpreciation = assetdeta.SplitModel[0].Tdpreciation;
                    }

                    if (!string.IsNullOrEmpty(splitfilter))
                    {
                        ViewBag.splitfilter = splitfilter.Trim();
                        assetdeta.SplitModel = assetdeta.SplitModel.Where(x => splitfilter.Trim().ToUpper() == null || (x.assetdetDescription.Contains(splitfilter.Trim().ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(splitfilter1))
                    {
                        ViewBag.splitfilter1 = splitfilter1.Trim();
                        assetdeta.SplitModel = assetdeta.SplitModel.Where(x => splitfilter1.Trim().ToUpper() == null || (x.assetdetDetid.Contains(splitfilter1.Trim().ToUpper()))).ToList();
                    }



                }
                if (command == "Clear")
                {
                    return RedirectToAction("SplitSummary", "AssetSplit");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            // return View();
            return View(assetdeta);

        }




        [HttpPost]
        public JsonResult SplitAsset(Models.AssetSplitModel status, string command, string splitamount)
        {
            try
            {
                string[] splitamounts = splitamount.Split(',');
                string value = "";
                string Maker = objcmnSplit.authorize("239");
                for (int i = 0; i < splitamounts.Length; i++)
                {
                    if (Convert.ToDecimal(splitamounts[i]) == 0)
                    {
                        value = "Zero";
                        return Json(value, JsonRequestBehavior.AllowGet);
                    }
                }

                try
                {
                    if (Maker == "Success")
                    {
                        if (value != "Zero")
                        {
                            string locid = status.locationId.ToString();
                            string acode = status.assetdetCode.ToString();
                            if (command == "split")
                            {
                                isr.GetAssetDetIDNew(locid, acode, status, splitamount);
                            }

                        }
                    }
                    return Json(new { command, Maker }, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }




        //Checker
        [HttpGet]
        public ActionResult SplitChkSummary()
        {
            try
            {
                AssetSplitModel splitrecord = new AssetSplitModel();
                splitrecord.SplitModel = isr.GetAssetDetailChk().ToList();
                if (splitrecord.SplitModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                return View(splitrecord);
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
        public ActionResult SplitChkSummary(string splitfilterchk, string splitfilter1chk, FormCollection collections, string command)
        {
            AssetSplitModel SplitChkdet = new AssetSplitModel();
            try
            {

                if (command == "SEARCH")
                {
                    SplitChkdet.SplitModel = isr.GetAssetDetailChk().ToList();
                    if (splitfilterchk != null && splitfilterchk != "")
                    {
                        ViewBag.schkfilter = splitfilterchk;
                        SplitChkdet.SplitModel = SplitChkdet.SplitModel.Where(x => splitfilterchk.ToUpper() == null || (x.assetdetDescription.Contains(splitfilterchk.ToUpper()))).ToList();
                    }
                    if (splitfilter1chk != null && splitfilter1chk != "")
                    {
                        ViewBag.schkfilter1 = splitfilter1chk;
                        SplitChkdet.SplitModel = SplitChkdet.SplitModel.Where(x => splitfilter1chk.ToUpper() == null || (x.assetdetDetid.Contains(splitfilter1chk.ToUpper()))).ToList();
                    }
                    if (SplitChkdet.SplitModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                }
                if (command == "Clear")
                {
                    return RedirectToAction("SplitChkSummary", "AssetSplit");
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View(SplitChkdet);
        }


        public PartialViewResult splitChkView(string id, string viewfor, FormCollection collections)
        {
            try
            {
                AssetSplitModel Split = new AssetSplitModel();


                if (viewfor == "view")
                {
                    Split.SplitModel = isr.GetMakSplitDetail(id).ToList();
                    // Split.assetdetDetid = "";
                    ViewBag.viewfor = "view";
                }
                if (viewfor == "Checkerview")
                {
                    Split.SplitModel = isr.GetSplitDetail(id).ToList();
                    ViewBag.viewfor = "Checkerview";
                }
                if (Split.SplitModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                return PartialView(Split);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return PartialView();
            }
            finally
            {

            }

        }

        [HttpPost]
        public JsonResult splitChkView(string command, string id, string remark)
        {
            try
            {
                string Checker = objcmnSplit.authorize("240");
                if (Checker == "Success")
                {
                    if (command == "Approve")
                    {
                        if (id != "" && id != null)
                        {
                            command = isr.AppRejAsset(id, "APPROVED", remark);
                        }
                    }
                    if (command == "Reject")
                    {
                        if (id != "" && id != null)
                        {
                            command = isr.AppRejAsset(id, "REJECTED", remark);
                        }
                    }
                }
                else
                {
                    command = "Unauthorized User";
                }
                return Json(command, JsonRequestBehavior.AllowGet);
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

        // [HttpPost]
        public PartialViewResult SplitHelp()
        {
            AssetSplitModel splithelp = new AssetSplitModel();
            try
            {
                splithelp.SplitModel = isr.GetAssetDetailHp().ToList();
                if (splithelp.SplitModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return PartialView();
            }
            finally
            {

            }
            return PartialView(splithelp);
        }



        [HttpPost]
        public PartialViewResult SplitHelp(string splitfilter1hp, string splitfilterhp, string command)
        {
            AssetSplitModel splithelps = new AssetSplitModel();
            try
            {
                if (command == "SEARCH")
                {
                    splithelps.SplitModel = isr.GetAssetDetailHp().ToList();
                    if (splitfilterhp != null && splitfilterhp != "")
                    {
                        ViewBag.spfilter = splitfilterhp;
                        splithelps.SplitModel = splithelps.SplitModel.Where(x => splitfilterhp.ToUpper() == null || (x.assetdetDescription.Contains(splitfilterhp.ToUpper()))).ToList();

                    }

                    if (splitfilter1hp != null && splitfilter1hp != "")
                    {
                        ViewBag.spfilter1 = splitfilter1hp;
                        splithelps.SplitModel = splithelps.SplitModel.Where(x => splitfilter1hp.ToUpper() == null || (x.assetdetDetid.Contains(splitfilter1hp.ToUpper()))).ToList();
                    }
                    if (splithelps.SplitModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }


                }

                if (command == "Clear")
                {
                    // return RedirectToAction("SplitHelp", "AssetSplit");

                }
                return PartialView(splithelps);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return PartialView(splithelps);
            }
            finally
            {

            }
        }

        [HttpPost]
        public JsonResult autoassetid(string term)
        {
            try
            {
                List<AssetSplitModel> autoloc = new List<AssetSplitModel>();
                autoloc = isr.SPAutoAssetsummary(term).ToList();
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
        public JsonResult splitautoassetid(string term)
        {
            try
            {
                List<AssetSplitModel> autoloc = new List<AssetSplitModel>();
                autoloc = isr.SPAutoAsset(term, Convert.ToString(objcmnSplit.GetLoginUserGid())).ToList();
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
        public JsonResult splitchkautoassetid(string term)
        {
            try
            {
                List<AssetSplitModel> autoloc = new List<AssetSplitModel>();
                autoloc = isr.SPAutoAssetChk(term).ToList();
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
        public ActionResult SplitExceldownload()
        {

            List<AssetSplitModel> cList;
            cList = (List<AssetSplitModel>)Session["SplitExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Asset Id");
            dt.Columns.Add("Category");
            dt.Columns.Add("Sub-Category Name");
            dt.Columns.Add("Sub-Category Code");
            dt.Columns.Add("Asset Value");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].assetdetDetid.ToString()
                , cList[i].assetcategory.ToString()
                , cList[i].assetsubcategory.ToString()
                , cList[i].assetdetCode.ToString()
                , cList[i].assetdetAssetvalue.ToString());
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Split Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
