//Version 1.0 23/07/2015
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Excel;
using System.Collections;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetTransferController : Controller
    {
        private IRepository ifr;
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private FileServer Cmnfs = new FileServer();
        public AssetTransferController()
            : this(new DataModel()) { }

        public AssetTransferController(IRepository objModel)
        {
            ifr = objModel;
        }

        //
        // GET: /TransferMakerSummary/
        public ActionResult index() { return View(); }

        [HttpGet]
        public ActionResult TMSummary(string toagid = "")
        {
            try
            {
                //UAT
                TransferMakerModel records = new TransferMakerModel();
                Session["TransferNo"] = null;
                Session["Status"] = null;
                Session["Errdatatable"] = null;
                Session["maindatatable"] = null;
                Session["RAISERattmtfileexcel"] = null;
                Session["orginal"] = null;
                Session["err"] = null;
                Session["Tempdataset"] = null;
                Session["filename"] = null;
                Session["Tempdatatable"] = null;
                Session["maindatatable"] = null;
                Session["gid"] = null;
                string status = Request.QueryString["reqstatus"];
                //For FACR TOA

                ViewBag.Status = "DRAFT";
                ViewBag.stsrejected = "REJECTED";
                string success = ifr.getTheUser("FAMMKR");
                ViewBag.success = success;
                if (success == "success")
                {
                    records.TModel = ifr.GetTrfDetailWFAP(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if (records.TModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["Trfexceldownload"] = records.TModel;
                    return View(records);
                }

                else if (status == ViewBag.Status)
                {
                    //records.TModel = ifr.GetTrfDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    records.TModel = ifr.GetTrfDetailDraft(Convert.ToString(toagid)).ToList();
                    if (records.TModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["Trfexceldownload"] = records.TModel;
                    return View(records);
                }
                else if (status == ViewBag.stsrejected)
                {
                    //records.TModel = ifr.GetTrfDetailRejected(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    records.TModel = ifr.GetTrfDetailRejected(Convert.ToString(toagid)).ToList();
                    if (records.TModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["Trfexceldownload"] = records.TModel;
                    return View(records);
                }
                else
                {
                    // records.TModel = ifr.GetTrfDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    records.TModel = ifr.GetTrfDetailDraft(Convert.ToString(toagid)).ToList();
                    if (records.TModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["Trfexceldownload"] = records.TModel;
                    return View(records);
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
        }

        [HttpPost]
        public ActionResult TMSummary(string status, string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                TransferMakerModel details = new TransferMakerModel();
                details.TModel = ifr.GetTrfDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                if (command == "SEARCH")
                {

                    details.TModel = ifr.GetTrfDetailFull(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                    {
                        details.TModel = ifr.GetTrfDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        details.TModel = details.TModel.Where(x => filter.ToUpper() == null || (x._toa_number.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.filter1 = filter1;
                        details.TModel = details.TModel.Where(x => filter1 == null || (x._tfr_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (status != "--Select Status--")
                    {
                        ViewBag.Status = status;
                        details.TModel = details.TModel.Where(x => status == null || (x._tfr_status.ToUpper().Contains(status.ToUpper()))).ToList();
                    }
                    if (details.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["Trfexceldownload"] = details.TModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("TMSummary", "AssetTransfer"); }
                else if (command == "TRANSFER")
                {
                    return RedirectToAction("TMSummary");
                }
                return View();
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
        //For FACR TOA
        public PartialViewResult TransferMakerView(string id, string viewfor, FormCollection collections, TransferMakerModel status)
        {
            try
            {

                string raiserBranchName = string.Empty;

                string userGroup = string.Empty;
                userGroup = ifr.GetUserTypeTransfer(id);
                ViewBag.success = userGroup;
                string toa_number = ifr.GetTrfNo(id);
                Session["TransferNo"] = toa_number;
                Session["toanumber"] = toa_number;
                Session["gid"] = id;
                string stat = ifr.Getststus(status, id);
                Session["Status"] = stat;
                TransferMakerModel llist = new TransferMakerModel();
                llist = ifr.GetRaiserDetail(Convert.ToString(objCmnFunctions.GetLoginUserGid()), toa_number);
                llist.TModel = ifr.GetTransferDetail(toa_number).ToList();
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
                if (llist.TModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(llist);
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

        //For query screen view //For FACR TOA
        public PartialViewResult QueryTransferView(string id, string viewfor, FormCollection collections, TransferMakerModel status)
        {
            try
            {
                string raiserId, raiserCode, raiserName, raiserBranchCode, raiserBranchName = string.Empty;

                string userGroup = string.Empty;
                userGroup = ifr.GetUserTypeTransfer(id);
                ViewBag.success = userGroup;

                string toa_number = ifr.GetTrfNo(id);
                Session["TransferNo"] = toa_number;
                Session["toanumber"] = toa_number;
                Session["gid"] = id;
                string stat = ifr.Getststus(status, id);
                Session["Status"] = stat;
                TransferMakerModel llist = new TransferMakerModel();
                llist = ifr.GetRaiserDetail(Convert.ToString(objCmnFunctions.GetLoginUserGid()), toa_number);
                llist.TModel = ifr.GetTransferDetail(toa_number).ToList();

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
                if (llist.TModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(llist);
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

        // For query screen inprocess view

        public PartialViewResult QueryInprocessView(string id, string viewfor, FormCollection collections, TransferMakerModel status)
        {
            try
            {
                string raiserId, raiserCode, raiserName, raiserBranchCode, raiserBranchName = string.Empty;

                string userGroup = string.Empty;
                userGroup = ifr.GetUserTypeTransfer(id);
                ViewBag.success = userGroup;

                string toa_number = ifr.GetTrfNo(id);
                Session["TransferNo"] = toa_number;
                Session["toanumber"] = toa_number;
                Session["gid"] = id;
                string stat = ifr.Getststus(status, id);
                Session["Status"] = stat;
                TransferMakerModel llist = new TransferMakerModel();
                llist = ifr.GetRaiserDetail(Convert.ToString(objCmnFunctions.GetLoginUserGid()), toa_number);
                llist.TModel = ifr.GetTransferDetail(toa_number).ToList();

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
                if (llist.TModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(llist);
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




        //For FACR TOA
        [HttpGet]
        public ActionResult TransferedSummary()
        {
            TransferMakerModel records = new TransferMakerModel();
            try
            {

                records.TModel = ifr.GetTrfDetailSummary("").ToList();
                if (records.TModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["Trfexceldownload"] = records.TModel;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View(records);
        }

        //For FACR TOA
        [HttpPost]
        public ActionResult TransferedSummary(string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                TransferMakerModel details = new TransferMakerModel();

                //details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                details.TModel = ifr.GetTrfDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                if (command == "SEARCH")
                {
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)))
                    {
                        //  details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                        details.TModel = ifr.GetTrfDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    }
                    if (filter != null)
                    {
                        ViewBag.checkerfilter = filter;
                        details.TModel = details.TModel.Where(x => filter.ToUpper() == null || (x._toa_number.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.checkerfilter1 = filter1;
                        details.TModel = details.TModel.Where(x => filter1.ToUpper() == null || (x._tfr_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (details.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["Trfexceldownload"] = details.TModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("TransferedSummary", "AssetTransfer"); }
                else if (command == "TRANSFER")
                {
                    return RedirectToAction("TransferedSummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View();
        }



        public JsonResult ImpairStatus()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            Ifams_Modelx Model = new Ifams_Modelx();
            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel details = new IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
            try
            {
                return Json(Model.GetImpairStatus(), JsonRequestBehavior.AllowGet);
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
        public JsonResult SaveData(FormCollection collections, Models.TransferMakerModel json)
        {
            String result = String.Empty;
            try
            {
                if (json._fccc != "")
                {
                    ifr.updateFCCC(json);
                    result = "1";
                }
                else
                {
                    result = "0";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }


        //Submit and  Approve Click  //For FACR TOA
        [HttpPost]
        public JsonResult TransferMakerView(TransferMakerModel assetTransferDetails)
        {
            string Checker = string.Empty;
            string Maker = string.Empty;
            int Result = 0;
            string Msg = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (assetTransferDetails._toa_number != "")
                {
                    Session["Transferno"] = assetTransferDetails._toa_number;
                    //Result=ifr.updateTrfStatus(assetTransferDetails);
                    dt = ifr.updateTrfStatus(assetTransferDetails);
                    Msg = dt.Rows[0][0].ToString();
                    Result = Convert.ToInt32(dt.Rows[0][1].ToString());
                }
                return Json(new { assetTransferDetails.commandType, Msg, Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
        }


        [HttpPost]
        public JsonResult VerifyTransferDetailSummary(TransferMakerModel objk)
        {
            List<TransferMakerModel> obkl = new List<TransferMakerModel>();
            int Result = 0;
            try
            {

                obkl = ifr.verifylogedindetails(objk._gid.ToString()).ToList();
                if (obkl.Count == 0)
                {
                    Result = ifr.UpdatelogedinStatus(objk._gid.ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(obkl, JsonRequestBehavior.AllowGet);
        }




        //[HttpPost]
        //public JsonResult TransferMakerView(string command, string viewfor, string remarks, string id, TransferMakerModel status, FormCollection collections)
        //{
        //    try
        //    {
        //        string Result = string.Empty;
        //        string Maker = objCmnFunctions.authorize("276");
        //        string Checker = objCmnFunctions.authorize("277");
        //        if (Maker == "Success" && (command == "abort" || command == "submit"))
        //        {
        //            if (command == "abort")
        //            {
        //                // id = status._toa_number;
        //                ifr.AbortTransfer(status, id);
        //                ViewBag.viewfor = "abort";

        //            }
        //            if (command == "submit")
        //            {
        //                if (id != "")
        //                {
        //                    Session["Transferno"] = id;
        //                    ifr.updateTrfStatus(status, id);
        //                }
        //            }
        //        }
        //        if (Checker == "Success" && (command == "Approve" || command == "Reject"))
        //        {
        //            if (command == "Approve")
        //            {
        //                if (id != "")
        //                {
        //                    Session["Transferno"] = id;
        //                    Result = ifr.ApprovalStatus(id, "APPROVED", remarks);
        //                    command = Result;
        //                }

        //            }
        //            if (command == "Reject")
        //            {
        //                if (id != "")
        //                {
        //                    Session["Transferno"] = id;
        //                    Result = ifr.ApprovalStatus(id, "REJECTED", remarks);
        //                    command = Result;
        //                }
        //            }
        //        }

        //        if (command == "cancel")
        //        {

        //        }
        //        return Json(new { command, Checker, Maker }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json(ex, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //    }
        //}

        private void datasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string duplicateasset = "";
            string notduplicatenewbranch = "";
            string grpId1 = "";
            string grpId2 = "";
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string assetdata = "";
                    string assetbranch = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    assetbranch = dataTable.Rows[0][3].ToString();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();

                        if (duplicateasset.Contains(assetdata) == true)
                        {
                            errs = "Duplicate Asset id Found";
                        }

                        if (assetbranch != "" && dataTable.Rows[i][3].ToString() != assetbranch)
                        {
                            errs = "New branch Should be same";
                        }
                        else
                        {
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty ";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = ifr.GetStatusexcel(assetdata, exceltext, "AssetIdCheck");
                                if (status == "Free")
                                {
                                    status = ifr.GetStatusexcel(assetdata, exceltext, "AssetIdProcessCheck");
                                    if (status == "Free")
                                    {
                                        duplicateasset += duplicateasset == "" ? assetdata : ("," + assetdata);
                                        grpId1 = ifr.GetGroupGid(dataTable.Rows[0][1].ToString());
                                        grpId2 = ifr.GetGroupGid(assetdata);


                                        if (grpId2 == grpId1 || grpId2 == "")
                                        {
                                            grpId1 = ifr.GetGrpCount(grpId1);
                                            if (grpId1 != "0")
                                                if (Convert.ToInt32(grpId1) != dataTable.Rows.Count)
                                                {
                                                    if (errs == "")
                                                    {
                                                        errs = "All assets in the Group are not Provided.";
                                                    }
                                                    else
                                                    {
                                                        errs = errs + "," + "All assets in the Group are not Provided.";
                                                    }
                                                }
                                        }
                                        else
                                        {
                                            if (errs == "")
                                            {
                                                errs = " All Asset ID must be of Same Group.";
                                            }
                                            else
                                            {
                                                errs = errs + "," + "All Asset ID must be of Same Group.";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (errs == "")
                                        {
                                            errs = status;
                                        }
                                        else
                                        {
                                            errs = errs + "," + status;
                                        }
                                    }
                                }
                                else
                                {
                                    if (errs == "")
                                    {
                                        errs = status;
                                    }
                                    else
                                    {
                                        errs = errs + "," + status;
                                    }
                                }
                            }
                        }
                        if (dataTable.Rows[i][2].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "From location Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "From location Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "AssetLoc");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Asset belongs to Diffrent Branch.";
                                }
                                else
                                {
                                    errs = errs + "," + "Asset belongs to Diffrent Branch.";
                                }
                            }
                        }
                        if (dataTable.Rows[i][3].ToString() == dataTable.Rows[i][2].ToString() && dataTable.Rows[i][3].ToString() != "" && dataTable.Rows[i][2].ToString() != "")
                        {
                            if (errs == "")
                            {
                                errs = "New Branch and Current Branch should not be same.";
                            }
                            else
                            {
                                errs = errs + "," + "New Branch and Current Branch should not be same.";
                            }

                        }
                        if (dataTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "New Branch should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "New Branch should not be Empty.";
                            }
                        }



                        else
                        {
                            exceltext = dataTable.Rows[i][3].ToString();

                            status = ifr.GetStatusexcel(assetdata, exceltext, "Loc");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "New Branch is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "New Branch is invalid.";
                                }
                            }

                            else
                            {
                                status = ifr.GetStatusexcel(assetdata, exceltext, "Locdate");
                                if (status == "notexists") // branch SLM(notvaolidate-branch lease_start date end date not checked) --lPM(validate both are checked) 
                                {
                                    if (errs == "")
                                    {
                                        errs = "New Branch start/end date or BranchLease start/end is invalid.";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "New Branch start/end date or BranchLease start/end is invalid.";
                                    }
                                }
                            }
                        }
                        string format = "dd-mm-yyyy";
                        DateTime dateTime;

                        if (dataTable.Rows[i][4].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Date of Transfer should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Date of Transfer should not be Empty.";
                            }
                        }

                        else if (DateTime.TryParse(dataTable.Rows[i][4].ToString(),
                            // format,
                            //   System.Globalization.CultureInfo.InvariantCulture,
                            //System.Globalization.DateTimeStyles.None,
                                 out dateTime))
                        {
                            exceltext = dataTable.Rows[i][4].ToString();
                            DateTime date = Convert.ToDateTime(exceltext);
                            DateTime year = DateTime.Today;
                            string finyear = ifr.toGetFincialyear(date);
                            string fintodayyear = ifr.toGetFincialyear();
                            string[] arrYer = finyear.Split('-');
                            if (finyear != fintodayyear)
                            {
                                if (date.Year == year.Year || date.Year == (year.Year - 1))

                                    if (errs == "")
                                    {
                                        errs = "Date does not belong to this financial year.";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Date does not belong to this financial year.";
                                    }
                            }
                        }
                        else
                        {
                            if (errs == "")
                            {
                                errs = "Invalid date format.";
                            }
                            else
                            {
                                errs = errs + "," + "Invalid date format.";
                            }
                        }
                        if (dataTable.Rows[i][5].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "CC should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "CC should not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][5].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "fccc");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "CC is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "CC is invalid.";
                                }
                            }
                        }
                        if (dataTable.Rows[i][6].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Inward No. Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Inward No. Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][6].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "InwardNo");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Inward No. is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "Inward No. is invalid.";
                                }
                            }
                        }
                        if (dataTable.Rows[i][8].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "HSN Code. Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "HSN Code. Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][8].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "HSN");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "HSN is not mapped for asset.";
                                }
                                else
                                {
                                    errs = errs + "," + "HSN is not mapped for asset";
                                }
                            }
                        }


                        if (errs != "")
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, RowNo, errs);
                        }

                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please select a valid sheet");
                }
                duplicateasset = "";
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record(s) in Excel File :" + totalrecord;
                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
        }
        public void UploadFilesnew()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["RAISERattmtfileexcel"] = hpf;
                        Session["orginal"] = hpf.FileName;
                    }
                }
                //return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return Json("Upload failed");
            }
            finally
            {
            }
        }

        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {

            string filename = "";
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["_upld_fname"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string; ;

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        filename = objCmnFunctions.GetSequenceNoFam("TOAA");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        //  var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}

                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);



                        filename = filename + fileextension;
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        Session["filename"] = filename;
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        message = filename;
                    }

                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["RAISERattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["RAISERattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("TOA{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["filename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                    //   path1 = @"C:\temp\" + n + " _ " + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SheetData> objparent = new List<SheetData>();
                SheetData objModel;
                int count = 0;
                string sheets = "";
                FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                DataSet result1 = new DataSet();
                if (Extension1 == ".xlsx")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else if (Extension1 == ".xls")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = error.ToString();
                    objparent.Add(objModel);
                }


                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = sheets.ToString();
                    objparent.Add(objModel);
                }

                Session["Tempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }


        public ActionResult downloadsexcel()
        {
            try
            {
                Session["err"] = "";
                DataTable dtnew = (DataTable)Session["Errdatatable"];
                DataTable dtmainnew = (DataTable)Session["maindatatable"];
                dtmainnew.Columns.Add("Error Description");
                for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                        if (i == line)
                            dtmainnew.Rows[line - 1]["Error Description"] = dtnew.Rows[j]["Error Description"];
                    }
                DataTable dt = dtmainnew;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Transfer_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Transfer_ErrorLog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Transfer_ErrorLog.xls"));
                //Response.ContentType = "application/ms-excel";
                // string str = string.Empty;   
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write(str + dtcol.ColumnName);
                //    str = "\t";
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + Convert.ToString(dr[j]));
                //        str = "\t";
                //    }
                //    Response.Write("\n");
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

        public PartialViewResult Uploadstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["Tempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatable"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["Errdatatable"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(string.Empty);
        }

        public ActionResult samdownloadsexcel() //Download Template Click
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Current Branch");
                dtnew.Columns.Add("New Branch");
                // dtnew.Columns.Add("Date of Transfer(dd-mm-yyyy)");
                dtnew.Columns.Add("Date(dd-mm-yyyy)");
                dtnew.Columns.Add("CC");
                dtnew.Columns.Add("Inward No");
                dtnew.Columns.Add("Reason");
                dtnew.Columns.Add("Hsn Code");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "Transfer_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Transfer_Template.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return View();
        }

        //public ActionResult samdownloadsexcel()
        //{
        //    try
        //    {
        //        DataTable dtnew = new DataTable();
        //        dtnew.Columns.Add("SNo");
        //        dtnew.Columns.Add("Asset Id");
        //        dtnew.Columns.Add("Current Branch");
        //        dtnew.Columns.Add("New Branch");
        //        dtnew.Columns.Add("Date of Transfer(dd-mm-yyyy)");
        //        dtnew.Columns.Add("FCCC");
        //        dtnew.Columns.Add("Inward No");
        //        dtnew.Columns.Add("Reason");
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Transfer_Template.xls"));
        //        Response.ContentType = "application/ms-excel";
        //        DataTable dt = dtnew;
        //        string str = string.Empty;
        //        foreach (DataColumn dtcol in dt.Columns)
        //        {
        //            Response.Write(str + dtcol.ColumnName);
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            str = "";
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                Response.Write(str + Convert.ToString(dr[j]));
        //                str = "\t";
        //            }
        //            Response.Write("\n");
        //        }
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return View();
        //}

        //For FACR TOA
        public JsonResult Makerupdate(TransferMakerModel obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdataset"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[obj.SheetName.ToString()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Current Branch"
                    && strColArr[3].ToString() == "New Branch"
                    //&& strColArr[4].ToString() == "Date of Transfer(dd-mm-yyyy)"
                    && strColArr[4].ToString() == "Date(dd-mm-yyyy)"
                    && strColArr[5].ToString() == "CC"
                    && strColArr[6].ToString() == "Inward No"
                    && strColArr[7].ToString() == "Reason"
                    && strColArr[8].ToString() == "Hsn Code"
                    && strColArr.Count().ToString() == "9")
                {
                    mag = "Success";
                }
                else
                {
                    mag = "Faild";
                }
                if (mag == "Success")
                {
                    int count = dataTable.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataTable.Rows[i]["SNo"].ToString() == "" && dataTable.Rows[i]["Asset Id"].ToString() == ""
                            && dataTable.Rows[i]["Asset Id"].ToString() == ""
                            && dataTable.Rows[i]["Current Branch"].ToString() == ""
                            && dataTable.Rows[i]["New Branch"].ToString() == ""
                            && dataTable.Rows[i]["Date of Transfer(dd-mm-yyyy)"].ToString() == ""
                            && dataTable.Rows[i]["CC"].ToString() == ""
                             && dataTable.Rows[i]["Inward No"].ToString() == ""
                            && dataTable.Rows[i]["Reason"].ToString() == ""
                            && dataTable.Rows[i]["Hsn Code"].ToString() == "")
                        {
                            dataTable.Rows[i].Delete();
                            dataTable.AcceptChanges();
                            count--;
                            i--;
                        }
                    }
                    Session["Tempdatatable"] = dataTable;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult localdetails(string ddlSelectsheet)
        {
            try { Session["err"] = "flag"; }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return PartialView();
        }
        //Fileupload Save ok  button click  //For FACR TOA
        public JsonResult localdetails() // For File Upload 
        {
            try
            {
                //UAT SOLUTION
                Session["err"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["maindatatable"];
                string check;
                string assettype = "TOA";
                string filename = Session["filename"].ToString();
                //check = ifr.InsertTransfer(uploadedData, filename).ToString();
                check = ifr.ImportAssetDetails(uploadedData, filename, assettype).ToString();

                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public void ViewAttachment(string id)
        {
            string filename = ifr.getfilename(id);
            DownloadDocument(filename);
        }

        [HttpPost]
        public JsonResult CheckFileExists(string id)
        {
            string result = "0";
            try
            {
                //WebReference.Service1 ObjService = new WebReference.Service1();
                //var isExists = ObjService.CheckFileExists(id);
                //if (isExists == 1)
                //{
                //    result = "1";
                //}
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var localpath = Path.Combine(CmnFilePath, id);
                // var localpath = Path.Combine(@"C:\temp\", id);
                //if (System.IO.File.Exists(localpath))
                //{
                //    result = "1";
                //}
                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult != "Fail")
                {
                    result = "1";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DownloadDocument(string id)
        {

            try
            {
                string filename = id;
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var path = Path.Combine(CmnFilePath, filename);
                //  var path = Path.Combine(@"C:\Temp\", filename);
                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult == "Fail")
                {
                    return File("", "");
                }
                else
                {
                    byte[] filebyte = Convert.FromBase64String(apiresult);
                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return File("", "");
            }

        }
        [HttpPost]
        public JsonResult fcccsearchtext(string lsFccc)
        {
            string data = "";
            try
            {
                data = ifr.GetFcccSerachText(lsFccc);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult FCCCSearch()
        {
            ViewBag.NoRecordsFound = "";
            TransferMakerModel objDetails = new TransferMakerModel();
            try
            {
                string lsId = Convert.ToString(Request.QueryString["id"]);
                objDetails = ifr.GetFccc();
                if (objDetails.fcccList.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                objDetails._fccc = lsId;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(objDetails);

        }

        [HttpPost]
        public PartialViewResult Fccc(FcccMaster objFcccModel)
        {
            ViewBag.NoRecordsFound = "";
            TransferMakerModel objDetails = new TransferMakerModel();
            try
            {
                ViewBag.code = objFcccModel.fcccGid;
                ViewBag.name = objFcccModel.fcccName;
                objDetails = ifr.GetFcccSerach(objFcccModel);
                if (objDetails.fcccList.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView("FCCCSearch", objDetails);
        }


        [HttpPost]
        public JsonResult Save_attach(TransferMakerModel model)  // FA Attachenment File save  Transfer (s)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.save_attachment(model);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_attach(int id)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.Delete_attach(id);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public PartialViewResult toaAttach(int id, string viewfor)
        {
            TransferMakerModel tmm = new TransferMakerModel();
            try
            {

                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";
                }
                if (id != 0)
                {
                    tmm._gid = id;
                    tmm._attach_list = ifr.Getattachment(id.ToString(), ifr.GetRef("TOA").ToString(), ifr.GetAttachType().ToString());
                }
                if (tmm._attach_list.Count == 0)
                    ViewBag.Message = "No records found";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(tmm);
        }

        [HttpGet]
        public PartialViewResult BulkTransfer()
        {
            try
            {
                Session["TransferNo"] = null;
                Session["Status"] = null;
                Session["Errdatatable"] = null;
                Session["maindatatable"] = null;
                Session["RAISERattmtfileexcel"] = null;
                Session["orginal"] = null;
                Session["err"] = null;
                Session["Tempdataset"] = null;
                Session["filename"] = null;
                Session["Tempdatatable"] = null;
                Session["maindatatable"] = null;
                Session["gid"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult toaAttachgrid(int id, string viewfor)
        {
            TransferMakerModel tmm = new TransferMakerModel();
            try
            {

                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";
                }
                if (id != 0)
                {
                    tmm._gid = id;
                    tmm._attach_list = ifr.Getattachment(id.ToString(), ifr.GetRef("TOA").ToString(), ifr.GetAttachType().ToString());
                }
                if (tmm._attach_list.Count == 0)
                    ViewBag.Message = "No records found";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(tmm);
        }


        //************************************************************
        // GET: /TransferCheckerSummary/

        [HttpGet]
        public ActionResult TCSummary()
        {
            TransferMakerModel records = new TransferMakerModel();
            try
            {
                string success = ifr.getTheUser("TOACHK");
                ViewBag.success = success;
                if (success == "success")
                {
                    records.TModel = ifr.GetTrfDetailforChecker("").ToList();
                    if (records.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                }
                else
                {
                    records.TModel = ifr.GetTrfDetailforChecker("").ToList();
                    if (records.TModel.Count == 0)
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
            return View(records);
        }

        [HttpPost]
        public ActionResult TCSummary(string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                if (command == "SEARCH")
                {
                    TransferMakerModel details = new TransferMakerModel();
                    details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)))
                    {
                        details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    }
                    if (filter != null)
                    {
                        ViewBag.checkerfilter = filter;
                        details.TModel = details.TModel.Where(x => filter.ToUpper() == null || (x._toa_number.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.checkerfilter1 = filter1;
                        details.TModel = details.TModel.Where(x => filter1.ToUpper() == null || (x._tfr_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (details.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("TCSummary", "AssetTransfer"); }
                else if (command == "TRANSFER")
                {
                    return RedirectToAction("TCSummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View();
        }

        [HttpPost]
        public JsonResult verifytransfer(TransferMakerModel objk)
        {
            List<TransferMakerModel> obkl = new List<TransferMakerModel>();
            string Result = string.Empty;
            try
            {

                obkl = ifr.verifylogeddetails(objk._gid.ToString()).ToList();
                if (obkl.Count == 0)
                {
                    Result = ifr.Updatelogedstatus(objk._gid.ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(obkl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updatelogeddetials(TransferMakerModel uobj)
        {
            string changelogstatus = string.Empty;
            try
            {
                changelogstatus = ifr.changlogedstatus(uobj._gid.ToString());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(changelogstatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Trfexceldownload()
        {
            string mt = null;
            List<TransferMakerModel> cList;
            cList = (List<TransferMakerModel>)Session["Trfexceldownload"];


            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Transfer Number");
            dt.Columns.Add("Transfer Date");
            dt.Columns.Add("No of Records");
            dt.Columns.Add("Transfer Status");
            dt.Columns.Add("Uploded File");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i]._toa_number.ToString()
                    , cList[i]._tfr_date.ToString()
                    , cList[i]._no_records.ToString()
                    , cList[i]._tfr_status.ToString()
                    , cList[i]._upld_fname.ToString());
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Asset Transfer Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }


        // Dashboard link Inprocess //For FACR TOA
        [HttpGet]
        public ActionResult TrfDasboardInprocessSummary(string toagid = "")
        {
            TransferMakerModel records = new TransferMakerModel();
            try
            {
               //records.TModel = ifr.GetTrfDtlInprocessSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
               records.TModel = ifr.GetTrfDtlInprocessSummary(Convert.ToString(toagid)).ToList();
                if (records.TModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["Trfexceldownload"] = records.TModel;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View(records);
        }

        //SELVA
        public ActionResult TrfDasboardInprocessSummary(string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                TransferMakerModel details = new TransferMakerModel();

                //details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                details.TModel = ifr.GetTrfDtlInprocessSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                if (command == "SEARCH")
                {
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)))
                    {
                        //  details.TModel = ifr.GetTrfDetailforChecker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                        details.TModel = ifr.GetTrfDtlInprocessSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    }
                    if (filter != null)
                    {
                        ViewBag.checkerfilter = filter;
                        details.TModel = details.TModel.Where(x => filter.ToUpper() == null || (x._toa_number.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.checkerfilter1 = filter1;
                        details.TModel = details.TModel.Where(x => filter1.ToUpper() == null || (x._tfr_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (details.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["Trfexceldownload"] = details.TModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("TrfDasboardInprocessSummary", "AssetTransfer"); }
                else if (command == "TRANSFER")
                {
                    return RedirectToAction("TrfDasboardInprocessSummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View();
        }


    }
}
