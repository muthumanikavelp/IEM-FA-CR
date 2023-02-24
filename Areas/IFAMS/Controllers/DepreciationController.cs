using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class DepreciationController : Controller
    {
        //
        // GET: /IFAMS/Depreciationrun/
        private IRepository ifr;
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public DepreciationController()
            : this(new DataModel()) { }

        public DepreciationController(IRepository objModel)
        {
            ifr = objModel;
        }

        [HttpGet]
        public ActionResult DepreciationRun()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DepreciationRun(string txtmonthpicker, string command, Depreciation depr)
        {
            string RAN = "";
            decimal totalDepAmt = 0;
            decimal CurrentDepAmt = 0;
            decimal SameDepAmount = 0;
            string sAssetGroupId = "";
            string lsPreviousCapDate = "";
            decimal dAsssetValue = 0;
            string sTransferID = "";
            decimal dTransferValue = 0;
            try
            {

                Depreciation dep = new Depreciation();
                dep._Till_date = "01-" + ifr.Format(depr._date);
                DateTime date = Convert.ToDateTime(dep._Till_date);
                var thisYear = date.AddMonths(-1);
                Session["depdate"] = Convert.ToString(thisYear);

                if (command == "FinalRun")
                {
                    if (!ifr.DataSynchronise())
                    {
                        return Json("Already initiated _ Data Synchronised", JsonRequestBehavior.AllowGet);

                    }
                    if (!ifr.StartProcess("DEPR_RUN"))
                    {
                        return Json("Already initiated _ Run", JsonRequestBehavior.AllowGet);
                    }
                    Session["date"] = dep._Till_date;
                    string finyear = ifr.toGetFincialyear();
                    string[] yeararr = finyear.Split('-');
                    DateTime dtcheck = Convert.ToDateTime("01-04-" + yeararr[0]);
                    if (Convert.ToDateTime(dep._Till_date) < dtcheck)
                    {
                        string finyear1 = ifr.toGetFincialyear(date);
                        return Json("Already initiated for the finacial year " + finyear1, JsonRequestBehavior.AllowGet);
                    }
                    DateTime date1 = dtcheck;
                    DateTime date2 = Convert.ToDateTime(dep._Till_date);
                    int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                    if (iMonthCount.ToString().Contains("-") == true)
                    {
                        iMonthCount = (iMonthCount * -1) + 1;
                    }
                    else
                    {
                        iMonthCount = (iMonthCount + 1);
                    }
               //     ifr.UpdateReversals();
                    for (int i = 1; i <= iMonthCount; i++)
                    {
                        if (!ifr.DepreciationDone(dtcheck.ToString(), "Final"))
                        {
                            dep._Dep_list = ifr.SelectDepDetails();

                            for (int loop = 0; loop < dep._Dep_list.Count; loop++)
                            {
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._cap_date))
                                {
                                    dep._Dep_list[loop]._cap_date = "01-01-1900";
                                }
                                int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                                DateTime daycount1 = Convert.ToDateTime(lastday + ifr.Format(dtcheck.ToString()));
                                dep._Till_date = daycount1.ToString();
                                DateTime datecheckend = Convert.ToDateTime("01-" + ifr.Format(depr._date));
                                int lastday2 = DateTime.DaysInMonth(datecheckend.Year, datecheckend.Month);
                                DateTime daycount2 = Convert.ToDateTime(lastday2 + ifr.Format(depr._date));
                                if (daycount1 > daycount2)
                                {
                                    dep._Till_date = depr._date.ToString();
                                }
                                if (dep._Dep_list[loop]._Trf_status == "Y")
                                {
                                    dep._Dep_list[loop]._Trf_date = daycount2.AddDays(-1).ToString();
                                }
                                if (dep._Dep_list[loop]._Sale_status == "Y")
                                {
                                    dep._Dep_list[loop]._Sale_date = daycount2.AddDays(-1).ToString();
                                }
                                DateTime depratechangedate;
                                if (dep._Dep_list[loop]._assetdet_dep_rate != 0)
                                {
                                    depratechangedate = ifr.getDepRateChangeDetail(dep._Dep_list[loop]._gid.ToString());
                                    if (dtcheck >= depratechangedate)
                                    {
                                        dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;

                                    }
                                    if (dtcheck.Year == depratechangedate.Year && dtcheck.Month == depratechangedate.Month && dtcheck >= depratechangedate)
                                    {
                                        dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;
                                    }
                                }
                                //DateTime.TryParse(dep._Dep_list[loop]._leasedate_start,out dt==false
                                //if (DateTime.TryParse(dep._Dep_list[loop]._leasedate_start,out dt==false)
                                //{
                                //    dep._Dep_list[loop]._leasedate_start = "01-01-1900";
                                //}
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._leasedate_end))
                                {
                                    dep._Dep_list[loop]._leasedate_end = "01-01-1900";
                                }

                                if ((dep._Dep_list[loop]._leasedate_end == "01-01-1900" || dep._Dep_list[loop]._leasedate_end == "01-01-1900") && dep._Dep_list[loop]._dep_type == "LPM")
                                {
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_end))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_end = "01-01-1900";
                                    }
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_start))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_start = "01-01-1900";
                                    }
                                    if (!(dep._Dep_list[loop]._branch_leasedate_start == "01-01-1900" || dep._Dep_list[loop]._branch_leasedate_end == "01-01-1900"))
                                    {
                                        ifr.UpdateLeaseDates(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._branch_leasedate_start, dep._Dep_list[loop]._branch_leasedate_end);
                                    }
                                }
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._branchdate_start))
                                {
                                    dep._Dep_list[loop]._branchdate_start = "01-01-1900";
                                }

                                // if (!(sAssetGroupId == dep._Dep_list[loop]._group_id &&
                                //   lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                                //&& sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value)
                                //     || dep._Dep_list[loop]._group_id == "")
                                if (!(lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                                 && sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value) && dep._Dep_list[loop]._asset_value != 0
                               || dep._Dep_list[loop]._group_id == "" && dep._Dep_list[loop]._dep_amt != dep._Dep_list[loop]._asset_value)
                                {
                                    totalDepAmt = ifr.GetTotalDep(Convert.ToDateTime(dep._Dep_list[loop]._cap_date), Convert.ToDateTime(dep._Till_date),
                                                            dep._Dep_list[loop]._asset_value, dep._Dep_list[loop]._asset_code, dep._Dep_list[loop]._is_5k,
                                                            dep._Dep_list[loop]._loc, "", Convert.ToDateTime(dep._Dep_list[loop]._branchdate_start),
                                                            Convert.ToDateTime(dep._Dep_list[loop]._leasedate_start), Convert.ToDateTime(dep._Dep_list[loop]._leasedate_end)
                                                            , dep._Dep_list[loop]._dep_type, dep._Dep_list[loop]._dep_rate, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._po_number
                                                            , dep._Dep_list[loop]._recon_ref, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._Trf_value,
                                                            dep._Dep_list[loop]._dep_full, 0);
                                }
                                else
                                {
                                    totalDepAmt = SameDepAmount;
                                    if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                    {
                                        totalDepAmt = dep._Dep_list[loop]._asset_value;
                                    }
                                }
                                CurrentDepAmt = totalDepAmt - dep._Dep_list[loop]._dep_amt;
                                if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                {
                                    CurrentDepAmt = dep._Dep_list[loop]._asset_value;
                                }

                                SameDepAmount = totalDepAmt;
                                sAssetGroupId = dep._Dep_list[loop]._group_id;
                                dAsssetValue = dep._Dep_list[loop]._asset_value;
                                sTransferID = dep._Dep_list[loop]._Trf_id;
                                dTransferValue = dep._Dep_list[loop]._Trf_value;
                                lsPreviousCapDate = dep._Dep_list[loop]._cap_date;
                                //if (Math.Round(CurrentDepAmt) > 0)
                                //{
                                CurrentDepAmt = Math.Round(CurrentDepAmt, 2);
                                string dte = objCmnFunctions.convertoDateTimeString(dtcheck.ToString());
                                //if (dep._Dep_list[loop]._dep_type == "SLM")
                                //{
                                //    dep._Dep_list[loop]._dep_type = "S";
                                //}
                                //if (dep._Dep_list[loop]._dep_type == "LPM")
                                //{
                                //    dep._Dep_list[loop]._dep_type = "L";
                                //}
                                ifr.InsertDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, dte, "M");
                                //ifr.InsertDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, ifr.Format("01-12-2014"));
                                //  }
                            }
                        }
                        dtcheck = dtcheck.AddMonths(1);
                    }
                    ifr.EndProcess("DEPR_RUN");
                    RAN = "RAN";
                }
                if (command == "RunDepreciation")
                {
                    //if (!ifr.DataSynchronise())
                    //{
                    //    return Json("Already initiated _ Data Synchronised", JsonRequestBehavior.AllowGet);

                    //}
                    //if (!ifr.StartProcess("DEPR_RUN"))
                    //{
                    //    return Json("Already initiated _ Run", JsonRequestBehavior.AllowGet);
                    //}
                    //ifr.TruncatePreDep();
                    //ifr.InsertFromDepToPreDep();
                    //ifr.insertdepmonths(date.ToString(), thisYear.ToString(), "1");
                    //dep._Till_date = "01-" + ifr.Format(depr._date);
                    //Session["date"] = dep._Till_date;
                    //string finyear = ifr.toGetFincialyear();
                    //string[] yeararr = finyear.Split('-');
                    //DateTime dtcheck = Convert.ToDateTime("01-04-" + yeararr[0]);
                    //if (Convert.ToDateTime(dep._Till_date) < dtcheck)
                    //{
                    //    string finyear1 = ifr.toGetFincialyear(date);
                    //    return Json("Already initiated for the finacial year " + finyear1, JsonRequestBehavior.AllowGet);
                    //}
                    //DateTime date1 = dtcheck;
                    //DateTime date2 = Convert.ToDateTime(dep._Till_date);
                    //int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                    //if (iMonthCount.ToString().Contains("-") == true)
                    //{
                    //    iMonthCount = (iMonthCount * -1) + 1;
                    //}
                    //else
                    //{
                    //    iMonthCount = (iMonthCount + 1);
                    //}
                    //ifr.UpdateReversals();
                    //for (int i = 0; i < iMonthCount; i++)
                    //{
                    //    if (!ifr.DepreciationDone(dtcheck.ToString(), "Final"))
                    //    {
                    //        dep._Dep_list = ifr.SelectPreRunDetails();

                    //        for (int loop = 0; loop < dep._Dep_list.Count; loop++)
                    //        {
                    //            if (dep._Dep_list[loop]._cap_date == null)
                    //            {
                    //                dep._Dep_list[loop]._cap_date = "01-01-1900";
                    //            }
                    //            int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                    //            DateTime daycount1 = Convert.ToDateTime(lastday + ifr.Format(dtcheck.ToString()));
                    //            dep._Till_date = daycount1.ToString();
                    //            DateTime daycount2 = DateTime.Today.Date;
                    //            if (daycount1 > daycount2)
                    //            {
                    //                dep._Till_date = DateTime.Today.ToShortDateString();
                    //            }
                    //            //if (dep._Dep_list[loop]._Trf_status == "Y" || dep._Dep_list[loop]._asset_status == "Asset Transfered")
                    //            //{
                    //            //    dep._Till_date = dep._Dep_list[loop]._Trf_date;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //MSG Invalid Transfer date for " & rsRecords("asset_id")
                    //            //}
                    //            //if (dep._Dep_list[loop]._Sale_status == "Y" || dep._Dep_list[loop]._asset_status == "Asset Sold")
                    //            //{
                    //            //    dep._Till_date = dep._Dep_list[loop]._Sale_date;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //MSG Invalid Sale/disacard date for " & rsRecords("asset_id")
                    //            //}
                    //            //if (dep._Dep_list[loop]._asset_status == "Asset Written-Off")
                    //            //{
                    //            //    dep._Till_date = dep._Dep_list[loop]._Wrt_date;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //MSG Invalid Sale/disacard date for " & rsRecords("asset_id")
                    //            //}
                    //            //if (dep._Dep_list[loop]._asset_status == "Asset Impaired")
                    //            //{
                    //            //    dep._Till_date = dep._Dep_list[loop]._Imp_date;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //MSG Invalid Sale/disacard date for " & rsRecords("asset_id")
                    //            //}
                    //            /////////////////////////////////////////
                    //            DateTime depratechangedate;
                    //            if (dep._Dep_list[loop]._assetdet_dep_rate != 0)
                    //            {
                    //                depratechangedate = ifr.getDepRateChangeDetail(dep._Dep_list[loop]._asset_id);
                    //                if (dtcheck >= depratechangedate)
                    //                {
                    //                    dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Transfered")
                    //            {
                    //                if (!string.IsNullOrEmpty(dep._Dep_list[loop]._Trf_date))
                    //                if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Trf_date) ||
                    //                    Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Trf_date))
                    //                {
                    //                    dep._Till_date = dep._Dep_list[loop]._Trf_date;
                    //                }
                    //                else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Trf_date))
                    //                {
                    //                    dep._Dep_list[loop]._asset_status = "Free";
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Written-Off")
                    //            {
                    //                if (!string.IsNullOrEmpty(dep._Dep_list[loop]._Wrt_date))
                    //                if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Wrt_date) ||
                    //                    Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Wrt_date))
                    //                {
                    //                    dep._Till_date = dep._Dep_list[loop]._Wrt_date;
                    //                }
                    //                else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Wrt_date))
                    //                {
                    //                    dep._Dep_list[loop]._asset_status = "Free";
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Sold")
                    //            {
                    //                if (!string.IsNullOrEmpty(dep._Dep_list[loop]._Sale_date))
                    //                if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Sale_date) ||
                    //                    Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Sale_date))
                    //                {
                    //                    dep._Till_date = dep._Dep_list[loop]._Sale_date;
                    //                }
                    //                else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Sale_date))
                    //                {
                    //                    dep._Dep_list[loop]._asset_status = "Free";
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Impaired")
                    //            {
                    //                if (!string.IsNullOrEmpty(dep._Dep_list[loop]._Imp_date))                               
                    //                if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Imp_date) ||
                    //                Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Imp_date))
                    //                {
                    //                    dep._Till_date = dep._Dep_list[loop]._Imp_date.ToString();
                    //                }
                    //                else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Imp_date))
                    //                {
                    //                    dep._Dep_list[loop]._asset_status = "Free";
                    //                }
                    //            }
                    //            //if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Merge")
                    //            //{
                    //            //    if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Imp_date) ||
                    //            //    Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Imp_date))
                    //            //    {
                    //            //        dep._Till_date = dep._Dep_list[loop]._Imp_date.ToString();
                    //            //    }
                    //            //    else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Imp_date))
                    //            //    {
                    //            //        dep._Dep_list[loop]._asset_status = "Free";
                    //            //    }
                    //            //}
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Asset Split")
                    //            {
                    //                if (!string.IsNullOrEmpty(dep._Dep_list[loop]._Split_date))   
                    //                if (Convert.ToDateTime(dep._Till_date) == Convert.ToDateTime(dep._Dep_list[loop]._Split_date) ||
                    //                Convert.ToDateTime(dep._Till_date) > Convert.ToDateTime(dep._Dep_list[loop]._Split_date))
                    //                {
                    //                    dep._Till_date = dep._Dep_list[loop]._Split_date.ToString();
                    //                }
                    //                else if (Convert.ToDateTime(dep._Till_date) < Convert.ToDateTime(dep._Dep_list[loop]._Split_date))
                    //                {
                    //                    dep._Dep_list[loop]._asset_status = "Free";
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._asset_status.ToString() == "Impairment in Process" ||
                    //                dep._Dep_list[loop]._asset_status.ToString() == "Sale in Process" ||
                    //                dep._Dep_list[loop]._asset_status.ToString() == "Write-off in Process" ||
                    //                dep._Dep_list[loop]._asset_status.ToString() == "Code Change in Process" ||
                    //                dep._Dep_list[loop]._asset_status.ToString() == "Asset Merge in Process" ||
                    //                dep._Dep_list[loop]._asset_status.ToString() == "Asset Split in Process")
                    //            {
                    //                dep._Dep_list[loop]._asset_status = "Free";
                    //            }
                    //            ////////////////////////////////////////
                    //            if (dep._Till_date == "" || (dep._Dep_list[loop]._asset_status == "Free" && Convert.ToDateTime(dep._Till_date).Month == DateTime.Today.Month))
                    //            {
                    //                dep._Till_date = daycount1.ToString();
                    //                if (Convert.ToDateTime(dep._Till_date).Month == DateTime.Today.Month)
                    //                {
                    //                    dep._Till_date = DateTime.Today.ToShortDateString();
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._leasedate_end == null)
                    //            {
                    //                dep._Dep_list[loop]._leasedate_end = "01-01-1900";
                    //            }

                    //            if ((dep._Dep_list[loop]._leasedate_end == "01-01-1900" || dep._Dep_list[loop]._leasedate_end == "01-01-1900") && dep._Dep_list[loop]._dep_type == "LEA")
                    //            {
                    //                if (dep._Dep_list[loop]._branch_leasedate_end == null)
                    //                {
                    //                    dep._Dep_list[loop]._branch_leasedate_end = "01-01-1900";
                    //                }
                    //                if (dep._Dep_list[loop]._branch_leasedate_start == null)
                    //                {
                    //                    dep._Dep_list[loop]._branch_leasedate_start = "01-01-1900";
                    //                }

                    //                if (!(dep._Dep_list[loop]._branch_leasedate_start == "01-01-1900" || dep._Dep_list[loop]._branch_leasedate_end == "01-01-1900"))
                    //                {
                    //                    ifr.UpdateLeaseDates(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._branch_leasedate_start, dep._Dep_list[loop]._branch_leasedate_end);
                    //                }
                    //            }
                    //            if (dep._Dep_list[loop]._branchdate_start == null)
                    //            {
                    //                dep._Dep_list[loop]._branchdate_start = "01-01-1900";
                    //            }

                    //            if (!(lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                    //             && sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value)
                    //           || dep._Dep_list[loop]._group_id == "")
                    //            {
                    //                totalDepAmt = ifr.GetTotalDep(Convert.ToDateTime(dep._Dep_list[loop]._cap_date), Convert.ToDateTime(dep._Till_date),
                    //                                        dep._Dep_list[loop]._asset_value, dep._Dep_list[loop]._asset_code, dep._Dep_list[loop]._is_5k,
                    //                                        dep._Dep_list[loop]._loc, "", Convert.ToDateTime(dep._Dep_list[loop]._branchdate_start),
                    //                                        Convert.ToDateTime(dep._Dep_list[loop]._leasedate_start), Convert.ToDateTime(dep._Dep_list[loop]._leasedate_end)
                    //                                        , dep._Dep_list[loop]._dep_type, dep._Dep_list[loop]._dep_rate, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._po_number
                    //                                        , dep._Dep_list[loop]._recon_ref, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._Trf_value,
                    //                                        dep._Dep_list[loop]._dep_full, 0);
                    //            }
                    //            else
                    //            {
                    //                totalDepAmt = SameDepAmount;
                    //            }
                    //            CurrentDepAmt = totalDepAmt - dep._Dep_list[loop]._dep_amt;
                    //            SameDepAmount = totalDepAmt;
                    //            sAssetGroupId = dep._Dep_list[loop]._group_id;
                    //            dAsssetValue = dep._Dep_list[loop]._asset_value;
                    //            sTransferID = dep._Dep_list[loop]._Trf_id;
                    //            dTransferValue = dep._Dep_list[loop]._Trf_value;
                    //            lsPreviousCapDate = dep._Dep_list[loop]._cap_date;
                    //            if (Math.Round(CurrentDepAmt) > 0)
                    //            {
                    //                CurrentDepAmt = Math.Round(CurrentDepAmt, 2);
                    //                string dte = objCmnFunctions.convertoDateTimeString(dtcheck.ToString());

                    //                ifr.InsertPreDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, dte, "M");
                    //            }
                    //        }
                    //    }
                    //    dtcheck = dtcheck.AddMonths(1);
                    //}
                    //ifr.EndProcess("DEPR_RUN");
                    //RAN = "RAN";
                    if (!ifr.DataSynchronise())
                    {
                        return Json("Already initiated _ Data Synchronised", JsonRequestBehavior.AllowGet);

                    }
                    if (!ifr.StartProcess("DEPR_RUN"))
                    {
                        return Json("Already initiated _ Run", JsonRequestBehavior.AllowGet);
                    }

                    ifr.InsertFromDepToPreDep();

                    dep._Till_date = "01-" + ifr.Format(depr._date);
                    Session["date"] = dep._Till_date;
                    string finyear = ifr.toGetFincialyear();
                    string[] yeararr = finyear.Split('-');
                    DateTime dtcheck = Convert.ToDateTime("01-04-" + yeararr[0]);
                    if (Convert.ToDateTime(dep._Till_date) < dtcheck)
                    {
                        string finyear1 = ifr.toGetFincialyear(date);
                        return Json("Already initiated for the finacial year " + finyear1, JsonRequestBehavior.AllowGet);
                    }
                    DateTime date1 = dtcheck;
                    DateTime date2 = Convert.ToDateTime(dep._Till_date);
                    int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                    if (iMonthCount.ToString().Contains("-") == true)
                    {
                        iMonthCount = (iMonthCount * -1) + 1;
                    }
                    else
                    {
                        iMonthCount = (iMonthCount + 1);
                    }
                   // ifr.UpdateReversals();
                    for (int i = 0; i < iMonthCount; i++)
                    {
                        if (!ifr.DepreciationDone(dtcheck.ToString(), "Final"))
                        {
                            dep._Dep_list = ifr.SelectPreRunDetails();

                            for (int loop = 0; loop < dep._Dep_list.Count; loop++)
                            {
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._cap_date))
                                {
                                    dep._Dep_list[loop]._cap_date = "01-01-1900";
                                }
                                int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                                DateTime daycount1 = Convert.ToDateTime(lastday + ifr.Format(dtcheck.ToString()));
                                dep._Till_date = daycount1.ToString();
                                DateTime datecheckend = Convert.ToDateTime("01-" + ifr.Format(depr._date));
                                int lastday2 = DateTime.DaysInMonth(datecheckend.Year, datecheckend.Month);
                                DateTime daycount2 = Convert.ToDateTime(lastday2 + ifr.Format(depr._date));

                                if (daycount1 > daycount2)
                                {
                                    dep._Till_date = depr._date.ToString();
                                }
                                ifr.InsertDepMonths(date.ToString(), thisYear.ToString(), "1", dep._Till_date.ToString());
                                if (dep._Dep_list[loop]._Trf_status == "Y")
                                {
                                    dep._Dep_list[loop]._Trf_date = daycount2.AddDays(-1).ToString();
                                }
                                if (dep._Dep_list[loop]._Sale_status == "Y")
                                {
                                    dep._Dep_list[loop]._Sale_date = daycount2.AddDays(-1).ToString();
                                }
                                DateTime depratechangedate;
                                if (dep._Dep_list[loop]._assetdet_dep_rate != 0)
                                {
                                    depratechangedate = ifr.getDepRateChangeDetail(dep._Dep_list[loop]._gid.ToString());
                                    if (dtcheck >= depratechangedate)
                                    {
                                        dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;

                                    }
                                    if (dtcheck.Year == depratechangedate.Year && dtcheck.Month == depratechangedate.Month && dtcheck >= depratechangedate)
                                    {
                                        dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;
                                    }
                                }
                                //DateTime.TryParse(dep._Dep_list[loop]._leasedate_start,out dt==false
                                //if (DateTime.TryParse(dep._Dep_list[loop]._leasedate_start,out dt==false)
                                //{
                                //    dep._Dep_list[loop]._leasedate_start = "01-01-1900";
                                //}
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._leasedate_end))
                                {
                                    dep._Dep_list[loop]._leasedate_end = "01-01-1900";
                                }

                                if ((dep._Dep_list[loop]._leasedate_end == "01-01-1900" || dep._Dep_list[loop]._leasedate_end == "01-01-1900") && dep._Dep_list[loop]._dep_type == "LPM")
                                {
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_end))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_end = "01-01-1900";
                                    }
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_start))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_start = "01-01-1900";
                                    }
                                    if (!(dep._Dep_list[loop]._branch_leasedate_start == "01-01-1900" || dep._Dep_list[loop]._branch_leasedate_end == "01-01-1900"))
                                    {
                                        ifr.UpdateLeaseDates(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._branch_leasedate_start, dep._Dep_list[loop]._branch_leasedate_end);
                                    }
                                }


                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._branchdate_start))
                                {
                                    dep._Dep_list[loop]._branchdate_start = "01-01-1900";
                                }
                                // if (!(sAssetGroupId == dep._Dep_list[loop]._group_id &&
                                //   lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                                //&& sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value)
                                //     || dep._Dep_list[loop]._group_id == "")
                                if (!(lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                                 && sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value) && dep._Dep_list[loop]._asset_value != 0
                               || dep._Dep_list[loop]._group_id == "")
                                {
                                    totalDepAmt = ifr.GetTotalDep(Convert.ToDateTime(dep._Dep_list[loop]._cap_date), Convert.ToDateTime(dep._Till_date),
                                                            dep._Dep_list[loop]._asset_value, dep._Dep_list[loop]._asset_code, dep._Dep_list[loop]._is_5k,
                                                            dep._Dep_list[loop]._loc, "", Convert.ToDateTime(dep._Dep_list[loop]._branchdate_start),
                                                            Convert.ToDateTime(dep._Dep_list[loop]._leasedate_start), Convert.ToDateTime(dep._Dep_list[loop]._leasedate_end)
                                                            , dep._Dep_list[loop]._dep_type, dep._Dep_list[loop]._dep_rate, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._po_number
                                                            , dep._Dep_list[loop]._recon_ref, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._Trf_value,
                                                            dep._Dep_list[loop]._dep_full, 0);
                                }
                                else
                                {
                                    totalDepAmt = SameDepAmount;
                                    if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                    {
                                        totalDepAmt = dep._Dep_list[loop]._asset_value;
                                    }
                                }
                                CurrentDepAmt = totalDepAmt - dep._Dep_list[loop]._dep_amt;
                                if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                {
                                    CurrentDepAmt = dep._Dep_list[loop]._asset_value;
                                }

                                SameDepAmount = totalDepAmt;
                                sAssetGroupId = dep._Dep_list[loop]._group_id;
                                dAsssetValue = dep._Dep_list[loop]._asset_value;
                                sTransferID = dep._Dep_list[loop]._Trf_id;
                                dTransferValue = dep._Dep_list[loop]._Trf_value;
                                lsPreviousCapDate = dep._Dep_list[loop]._cap_date;
                                //if (Math.Round(CurrentDepAmt) > 0)
                                //{
                                CurrentDepAmt = Math.Round(CurrentDepAmt, 2);
                                string dte = objCmnFunctions.convertoDateTimeString(dtcheck.ToString());
                                if (dep._Dep_list[loop]._dep_type == "SLM")
                                {
                                    dep._Dep_list[loop]._dep_type = "S";
                                }
                                if (dep._Dep_list[loop]._dep_type == "LPM")
                                {
                                    dep._Dep_list[loop]._dep_type = "L";
                                }
                                ifr.InsertPreDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, dte, "M");
                                //ifr.InsertDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, ifr.Format("01-12-2014"));
                                //}
                            }
                        }
                        dtcheck = dtcheck.AddMonths(1);
                    }
                    ifr.EndProcess("DEPR_RUN");
                    RAN = "RAN";
                }
                if (command == "ForecasteRun")
                {
                    ifr.InsertFromDepToPreDep();
                    if (!ifr.DataSynchronise())
                    {
                        return Json("Already initiated _ Data Synchronised", JsonRequestBehavior.AllowGet);

                    }
                    if (!ifr.StartProcess("DEPR_RUN"))
                    {
                        return Json("Already initiated _ Run", JsonRequestBehavior.AllowGet);
                    }

                    dep._Till_date = "01-" + ifr.Format(depr._date);
                    ifr.InsertDepMonths(dep._Till_date.ToString(), thisYear.ToString(), "2", dep._Till_date.ToString());
                    Session["date"] = dep._Till_date;
                    string finyear = ifr.toGetFincialyear();
                    string[] yeararr = finyear.Split('-');
                    DateTime dtcheck = DateTime.Today.AddDays(1);
                    DateTime date1 = dtcheck;
                    DateTime date2 = Convert.ToDateTime(dep._Till_date);
                    if (date2.Year <= dtcheck.Year)
                    {
                        if (date2.Month <= dtcheck.Month)
                        {
                            ifr.EndProcess("DEPR_RUN");
                            return Json("Only Depreciation Forecast allowed, Select later months", JsonRequestBehavior.AllowGet);
                        }
                    }

                    int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                    if (iMonthCount.ToString().Contains("-") == true)
                    {
                        iMonthCount = (iMonthCount * -1) + 1;
                    }
                    else
                    {
                        iMonthCount = (iMonthCount + 1);
                    }
              //      ifr.UpdateReversals();
                    for (int i = 1; i < iMonthCount; i++)
                    {
                        if (!ifr.DepreciationDone(dtcheck.ToString(), "Final"))
                        {
                            dep._Dep_list = ifr.SelectDepDetails();

                            for (int loop = 0; loop < dep._Dep_list.Count; loop++)
                            {
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._cap_date))
                                {
                                    dep._Dep_list[loop]._cap_date = "01-01-1900";
                                }
                                int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                                DateTime daycount1 = Convert.ToDateTime(lastday + ifr.Format(dtcheck.ToString()));
                                dep._Till_date = daycount1.ToString();
                                DateTime datecheckend = Convert.ToDateTime("01-" + ifr.Format(depr._date));
                                int lastday2 = DateTime.DaysInMonth(datecheckend.Year, datecheckend.Month);
                                DateTime daycount2 = Convert.ToDateTime(lastday2 + ifr.Format(depr._date));
                                if (daycount1 > daycount2)
                                {
                                    dep._Till_date = depr._date.ToString();
                                }
                                if (dep._Dep_list[loop]._Trf_status == "Y")
                                {
                                    dep._Dep_list[loop]._Trf_date = daycount2.AddDays(-1).ToString();
                                }
                                if (dep._Dep_list[loop]._Sale_status == "Y")
                                {
                                    dep._Dep_list[loop]._Sale_date = daycount2.AddDays(-1).ToString();
                                }

                                DateTime depratechangedate;
                                if (dep._Dep_list[loop]._assetdet_dep_rate != 0)
                                {
                                    depratechangedate = ifr.getDepRateChangeDetail(dep._Dep_list[loop]._asset_id);
                                    if (dtcheck >= depratechangedate)
                                    {
                                        dep._Dep_list[loop]._dep_rate = dep._Dep_list[loop]._assetdet_dep_rate;
                                    }
                                }
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._leasedate_end))
                                {
                                    dep._Dep_list[loop]._leasedate_end = "01-01-1900";
                                }

                                if ((dep._Dep_list[loop]._leasedate_end == "01-01-1900" || dep._Dep_list[loop]._leasedate_end == "01-01-1900") && dep._Dep_list[loop]._dep_type == "LPM")
                                {
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_end))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_end = "01-01-1900";
                                    }
                                    if (string.IsNullOrEmpty(dep._Dep_list[loop]._branch_leasedate_start))
                                    {
                                        dep._Dep_list[loop]._branch_leasedate_start = "01-01-1900";
                                    }
                                    if (!(dep._Dep_list[loop]._branch_leasedate_start == "01-01-1900" || dep._Dep_list[loop]._branch_leasedate_end == "01-01-1900"))
                                    {
                                        ifr.UpdateLeaseDates(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._branch_leasedate_start, dep._Dep_list[loop]._branch_leasedate_end);
                                    }
                                }
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._branchdate_start))
                                {
                                    dep._Dep_list[loop]._branchdate_start = "01-01-1900";
                                }
                                if (string.IsNullOrEmpty(dep._Dep_list[loop]._loc))
                                {
                                    dep._Dep_list[loop]._loc = "";
                                }
                                if (!(lsPreviousCapDate == dep._Dep_list[loop]._cap_date && dAsssetValue == dep._Dep_list[loop]._asset_value
                                 && sTransferID == dep._Dep_list[loop]._Trf_id && dTransferValue == dep._Dep_list[loop]._Trf_value) && dep._Dep_list[loop]._asset_value != 0
                               || dep._Dep_list[loop]._group_id == "")
                                {
                                    totalDepAmt = ifr.GetTotalDep(Convert.ToDateTime(dep._Dep_list[loop]._cap_date), Convert.ToDateTime(dep._Till_date),
                                                            dep._Dep_list[loop]._asset_value, dep._Dep_list[loop]._asset_code, dep._Dep_list[loop]._is_5k,
                                                            dep._Dep_list[loop]._loc, "", Convert.ToDateTime(dep._Dep_list[loop]._branchdate_start),
                                                            Convert.ToDateTime(dep._Dep_list[loop]._leasedate_start), Convert.ToDateTime(dep._Dep_list[loop]._leasedate_end)
                                                            , dep._Dep_list[loop]._dep_type, dep._Dep_list[loop]._dep_rate, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._po_number
                                                            , dep._Dep_list[loop]._recon_ref, dep._Dep_list[loop]._group_id, dep._Dep_list[loop]._Trf_value,
                                                            dep._Dep_list[loop]._dep_full, 0);
                                }
                                else
                                {
                                    {
                                        totalDepAmt = SameDepAmount;
                                        if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                        {
                                            totalDepAmt = dep._Dep_list[loop]._asset_value;
                                        }
                                    }
                                    CurrentDepAmt = totalDepAmt - dep._Dep_list[loop]._dep_amt;
                                    if (totalDepAmt > dep._Dep_list[loop]._asset_value)
                                    {
                                        CurrentDepAmt = dep._Dep_list[loop]._asset_value;
                                    }

                                    SameDepAmount = totalDepAmt;
                                    sAssetGroupId = dep._Dep_list[loop]._group_id;
                                    dAsssetValue = dep._Dep_list[loop]._asset_value;
                                    sTransferID = dep._Dep_list[loop]._Trf_id;
                                    dTransferValue = dep._Dep_list[loop]._Trf_value;
                                    lsPreviousCapDate = dep._Dep_list[loop]._cap_date;
                                    //if (Math.Round(CurrentDepAmt) > 0)
                                    //{
                                    CurrentDepAmt = Math.Round(CurrentDepAmt, 2);
                                    string dte = objCmnFunctions.convertoDateTimeString(dtcheck.ToString());
                                    //if (dep._Dep_list[loop]._dep_type == "SLM")
                                    //{
                                    //    dep._Dep_list[loop]._dep_type = "S";
                                    //}
                                    //if (dep._Dep_list[loop]._dep_type == "LPM")
                                    //{
                                    //    dep._Dep_list[loop]._dep_type = "L";
                                    //}
                                    ifr.InsertForecasteDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, dte, dep._Dep_list[loop]._dep_type);
                                }
                            }
                        }
                        dtcheck = dtcheck.AddMonths(1);
                    }
                    ifr.EndProcess("DEPR_RUN");
                    RAN = "RAN";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ifr.EndProcess("DEPR_RUN");
                RAN = "Error occured , Please checkdata.";
            }
            finally
            {
            }
            return Json(RAN, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DepreciationForecast()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DepreciationHold(string listfor)
        {
            Depreciation DepList = new Depreciation();
            DepList._Dep_list = ifr.GetdetailsforDepHold();
            DepList._Dep_listHeld = ifr.GetdetailsforDepRelease();
            if (listfor == "SEARCHfree")
            {
                if (Session["Searchforfree"] != null)
                {
                    DepList._Dep_list = (List<Depreciation>)Session["Searchforfree"];
                    Session["Searchforfree"] = null;
                }
            }
            if (listfor == "SEARCHheld")
            {
                if (Session["SearchforHeld"] != null)
                {
                    DepList._Dep_listHeld = (List<Depreciation>)Session["SearchforHeld"];
                    Session["SearchforHeld"] = null;
                }
            }
            if (DepList._Dep_listHeld.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            if (DepList._Dep_list.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            ViewBag.Source = "";
            return View(DepList);

            //  return View();
        }

        [HttpPost]
        public ActionResult DepreciationHold(string assetgid, string command, Depreciation model)
        {
            try
            {
                string subcat = model._asset_code;
                string branch = model._branch;
                string capdate = model._cap_date;
                Depreciation details = new Depreciation();
                List<Depreciation> details_list = new List<Depreciation>();
                details._Dep_list = ifr.GetdetailsforDepHold();
                details._Dep_listHeld = ifr.GetdetailsforDepRelease();
                Session["Searchforfree"] = null;
                if (command == "SEARCHfree")
                {

                    if ((string.IsNullOrEmpty(subcat) || string.IsNullOrWhiteSpace(subcat)) && (string.IsNullOrEmpty(branch) || string.IsNullOrWhiteSpace(branch)) && (string.IsNullOrEmpty(capdate) || string.IsNullOrWhiteSpace(capdate)))
                    {
                        details._Dep_list = ifr.GetdetailsforDepHold();
                    }
                    if (subcat != null)
                    {
                        ViewBag.subcat = subcat;
                        details._Dep_list = details._Dep_list.Where(x => subcat.ToUpper() == null || (x._asset_code.Contains(subcat.ToUpper()))).ToList();
                    }
                    if (branch != null)
                    {
                        ViewBag.branch = branch;
                        details._Dep_list = details._Dep_list.Where(x => branch == null || (x._branch.ToUpper().Contains(branch.ToUpper()))).ToList();
                    }
                    if (capdate != null)
                    {
                        ViewBag.capdate = capdate;
                        details._Dep_list = details._Dep_list.Where(x => capdate == null || (x._cap_date.ToUpper().Contains(capdate.ToUpper()))).ToList();
                    }
                    Session["Searchforfree"] = details._Dep_list;
                    if (details._Dep_list.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return Json(details, JsonRequestBehavior.AllowGet);
                }

                Session["SearchforHeld"] = null;
                if (command == "SEARCHheld")
                {

                    if ((string.IsNullOrEmpty(subcat) || string.IsNullOrWhiteSpace(subcat)) && (string.IsNullOrEmpty(branch) || string.IsNullOrWhiteSpace(branch)) && (string.IsNullOrEmpty(capdate) || string.IsNullOrWhiteSpace(capdate)))
                    {
                        details._Dep_listHeld = ifr.GetdetailsforDepRelease();
                    }
                    if (subcat != null)
                    {
                        ViewBag.subcat = subcat;
                        details._Dep_listHeld = details._Dep_listHeld.Where(x => subcat.ToUpper() == null || (x._asset_code.Contains(subcat.ToUpper()))).ToList();
                    }
                    if (branch != null)
                    {
                        ViewBag.branch = branch;
                        details._Dep_listHeld = details._Dep_listHeld.Where(x => branch == null || (x._branch.ToUpper().Contains(branch.ToUpper()))).ToList();
                    }
                    if (branch != null)
                    {
                        ViewBag.capdate = capdate;
                        details._Dep_listHeld = details._Dep_listHeld.Where(x => capdate == null || (x._cap_date.ToUpper().Contains(capdate.ToUpper()))).ToList();
                    }
                    Session["SearchforHeld"] = details._Dep_listHeld;
                    if (details._Dep_listHeld.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return Json(details, JsonRequestBehavior.AllowGet);
                }

                // else 
                //string data0 = string.Empty;
                if (command == "hold")
                {
                    ifr.Insert_deponhold(assetgid);
                    details._Dep_list = ifr.GetdetailsforDepHold();
                    details._Dep_listHeld = ifr.GetdetailsforDepRelease();
                    return View(details);
                    //    DataSet ds = new DataSet();
                    //    DataTable dt;
                    //    ds = ifr.GetdetailsforDepHold();
                    //    dt = ds.Tables[0];
                    //    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    //    return Json(new { data0 }, JsonRequestBehavior.AllowGet);
                }
                else if (command == "release")
                {
                    ifr.Update_deponhold(assetgid);
                    details._Dep_list = ifr.GetdetailsforDepHold();
                    details._Dep_listHeld = ifr.GetdetailsforDepRelease();
                    return View(details);
                    //    DataSet ds = new DataSet();
                    //    DataTable dt;
                    //    ds = ifr.GetdetailsforDepHold();
                    //    dt = ds.Tables[0];
                    //    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    //    return Json(new { data0 }, JsonRequestBehavior.AllowGet);
                }
                return Json(details, JsonRequestBehavior.AllowGet);
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


        public string convertoDate(string date1)
        {
            try
            {
                string date2 = date1;
                string convdate = string.Empty;
                DateTime convdate2 = Convert.ToDateTime(date2);
                string format = "dd/MMM/yyyy";
                convdate = convdate2.ToString(format);
                return convdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        //public string HoldFileUploadUrl()
        //{
        //    string x = "";
        //    try
        //    {
        //        x = System.Configuration.ConfigurationManager.AppSettings["HoldFileUpload"].ToString();
        //    }
        //    catch { x = ""; }
        //    return (x == null || x.Trim() == "") ? "" : x;
        //}
        //[HttpGet]
        //public ActionResult GetReportFileDownload()
        //{
        //    DataModel ifr = new DataModel();
        //    List<Depreciation> DepList = new List<Depreciation>();
        //    try
        //    {
        //        string extension = System.Configuration.ConfigurationManager.AppSettings["FileType"].ToString();
        //        string saveFileName = "TR" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + extension.ToString();
        //        string directory = HoldFileUploadUrl() + saveFileName.ToString();
        //        DataTable dt = ifr.GetSelectreportDetails();
        //        using (StreamWriter writer = new StreamWriter(directory))
        //        {
        //            WriteDataTable(dt, writer, true);
        //        }

        //        return File(directory, "application/text", "Depreciation_PreRun" + "." + extension.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json("Error occurred While Downloading..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        //{
        //    if (includeHeaders)
        //    {
        //        IEnumerable<String> headerValues = sourceTable.Columns
        //            .OfType<DataColumn>()
        //            .Select(column => QuoteValue(column.ColumnName));

        //        writer.WriteLine(String.Join(",", headerValues));
        //    }

        //    IEnumerable<String> items = null;

        //    foreach (DataRow row in sourceTable.Rows)
        //    {
        //        items = row.ItemArray.Select(o => QuoteValue(o.ToString()));
        //        writer.WriteLine(String.Join(",", items));
        //    }

        //    writer.Flush();
        //}

        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }


        public ActionResult downloadsexcel()
        {
            try
            {
                List<Depreciation> DepList;
                DepList = ifr.SelectreportDetails("report", "2016-02-01");
                Depreciation dep = new Depreciation();
                DataTable dt = new DataTable();
                dep = ifr.selcetdates("1");
                int lstdate = DateTime.DaysInMonth(Convert.ToDateTime(dep._current_month).Year, Convert.ToDateTime(dep._current_month).Month);
                string lastMonth = ifr.Format(dep._date.ToString());

                string stratDate = Convert.ToDateTime("01" + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                string endDate = Convert.ToDateTime(lstdate + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                if (Convert.ToDateTime(dep._current_month).Month == DateTime.Today.Month)
                {
                    endDate = DateTime.Today.ToShortDateString();
                }
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Asset Id");
                dt.Columns.Add("Group Id");
                dt.Columns.Add("Category");
                dt.Columns.Add("Sub category");
                dt.Columns.Add("Asset GL");
                dt.Columns.Add("Capitalization Date");
                dt.Columns.Add("Asset Value");
                dt.Columns.Add("Is 5K");
                dt.Columns.Add("Asset Detail dep rate");
                dt.Columns.Add("Cumulative Depreciation for " + lastMonth.ToString());
                dt.Columns.Add("WDV for " + lastMonth.ToString());
                dt.Columns.Add("Asset Status");
                dt.Columns.Add("Depreciation Type");
                dt.Columns.Add("Lease start date");
                dt.Columns.Add("Lease end dat");
                dt.Columns.Add("Depreciation Rate");
                dt.Columns.Add("Depreciation Start Date");
                dt.Columns.Add("Depreciation End Date");
                dt.Columns.Add("Depreciation For " + ifr.Format(dep._current_month.ToString()));
                dt.Columns.Add("Cumulative Depreciation as of " + ifr.Format(dep._current_month.ToString()));
                dt.Columns.Add("WDV as of " + ifr.Format(dep._current_month.ToString()));
                for (int i = 0; i < DepList.Count; i++)
                {

                    if (Convert.ToDateTime(DepList[i]._cap_date).Year == DateTime.Today.Year && Convert.ToDateTime(DepList[i]._cap_date).Month == DateTime.Today.Month)
                    {
                        DepList[i]._Sale_status = "Purchased";
                    }
                    if (DepList[i]._Sale_status == "Free")
                    {
                        endDate = Convert.ToDateTime(lstdate + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                        if (Convert.ToDateTime(dep._current_month).Month == DateTime.Today.Month)
                        {
                            endDate = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month).ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;
                        }
                    }




                    decimal depamount = Convert.ToDecimal(DepList[i]._dep_amt) - Convert.ToDecimal(DepList[i]._dep_amtpre);
                    decimal cumdep2 = depamount + Convert.ToDecimal(DepList[i]._dep_amtpre);
                    decimal wdv1 = (DepList[i]._asset_value - depamount);
                    decimal wdv2 = wdv1 - Convert.ToDecimal(DepList[i]._dep_amtpre);
                    dt.Rows.Add(
                        i + 1
                    , DepList[i]._asset_id.ToString()
                    , DepList[i]._group_id.ToString()
                    , DepList[i]._asset_code.ToString()
                    , DepList[i]._AssetDesp.ToString()
                    , DepList[i]._asset_glcode.ToString()
                    , DepList[i]._cap_date.ToString()
                    , DepList[i]._asset_value.ToString()
                    , DepList[i]._is_5k.ToString()
                     , DepList[i]._assetdet_dep_rate.ToString()
                    , depamount
                    , wdv1.ToString()
                    , DepList[i]._Sale_status.ToString()
                    , DepList[i]._dep_type.ToString()
                    , DepList[i]._leasedate_start.ToString()
                    , DepList[i]._leasedate_end.ToString()
                    , DepList[i]._dep_rate.ToString()
                    , stratDate
                    , endDate
                    , DepList[i]._dep_amtpre.ToString()
                    , cumdep2.ToString()
                    , wdv2.ToString()
                   );
                }
                ////export to excel from gridview
                ////GridView gv = new GridView();
                ////gv.DataSource = dt;
                ////gv.DataBind();
                ////Session["exceldownload"] = gv;
                ////if (gv.Rows.Count != 0)
                ////{
                ////    return new DownloadFileActionResult((GridView)Session["exceldownload"], "DepreciationReport.xls");
                ////}
                ////else
                ////{
                ////    ViewBag.Message = "No records found";
                ////}
                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    wb.Worksheets.Add(dt, "DepreciationReport");

                //    Response.Clear();
                //    Response.Buffer = true;
                //    Response.Charset = "";
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.AddHeader("content-disposition", "attachment;filename=DepreciationReport.xlsx");
                //    using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //        wb.SaveAs(MyMemoryStream);
                //        MyMemoryStream.WriteTo(Response.OutputStream);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}

                string attachment = "attachment; filename=Depreciation_PreRun.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                string strquery = "";

                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int k;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (k = 0; k < dt.Columns.Count; k++)
                        {
                            Response.Write(tab + dr[k].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
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
            return RedirectToAction("DepreciationRun");
        }

        public ActionResult downloadexcel()
        {
            try
            {
                List<Depreciation> DepList;
                DepList = ifr.SelectreportDetails("forecastereport", "");
                Depreciation dep = new Depreciation();
                DataTable dt = new DataTable();
                dep = ifr.selcetdates("2");
                int lstdate = DateTime.DaysInMonth(Convert.ToDateTime(dep._current_month).Year, Convert.ToDateTime(dep._current_month).Month);
                string lastMonth = ifr.Format(dep._date.ToString());
                string stratDate = Convert.ToDateTime("01" + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                string endDate = Convert.ToDateTime(lstdate + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                if (Convert.ToDateTime(dep._current_month).Month == DateTime.Today.Month)
                {
                    endDate = DateTime.Today.ToShortDateString();
                }
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Asset Id");
                dt.Columns.Add("Group Id");
                dt.Columns.Add("Category");
                dt.Columns.Add("Sub category");
                dt.Columns.Add("Asset GL");
                dt.Columns.Add("Capitalization Date");
                dt.Columns.Add("Asset Value");
                dt.Columns.Add("Is 5K");
                dt.Columns.Add("Original Value");
                dt.Columns.Add("Cumulative Depreciation for " + lastMonth.ToString());
                dt.Columns.Add("WDV for " + lastMonth.ToString());
                dt.Columns.Add("Asset Status");
                dt.Columns.Add("Depreciation Type");
                dt.Columns.Add("Depreciation Rate");
                dt.Columns.Add("Depreciation Start Date");
                dt.Columns.Add("Depreciation End Date");
                dt.Columns.Add("Depreciation For " + ifr.Format(dep._current_month.ToString()));
                dt.Columns.Add("Cumulative Depreciation as of " + ifr.Format(dep._current_month.ToString()));
                dt.Columns.Add("WDV as of " + ifr.Format(dep._current_month.ToString()));
                for (int i = 0; i < DepList.Count; i++)
                {
                    if (DepList[i]._Sale_status.ToString() == "Asset Transfered")
                    {
                        if (Convert.ToDateTime(endDate) == Convert.ToDateTime(DepList[i]._Trf_date) ||
                            Convert.ToDateTime(endDate) > Convert.ToDateTime(DepList[i]._Trf_date))
                        {
                            endDate = DepList[i]._Trf_date;
                        }
                        else if (Convert.ToDateTime(endDate) < Convert.ToDateTime(DepList[i]._Trf_date))
                        {
                            DepList[i]._Sale_status = "Free";
                        }
                    }
                    if (DepList[i]._Sale_status.ToString() == "Asset Written-Off")
                    {
                        if (Convert.ToDateTime(endDate) == Convert.ToDateTime(DepList[i]._Wrt_date) ||
                            Convert.ToDateTime(endDate) > Convert.ToDateTime(DepList[i]._Wrt_date))
                        {
                            endDate = DepList[i]._Wrt_date;
                        }
                        else if (Convert.ToDateTime(endDate) < Convert.ToDateTime(DepList[i]._Wrt_date))
                        {
                            DepList[i]._Sale_status = "Free";
                        }
                    }
                    if (DepList[i]._Sale_status.ToString() == "Asset Sold")
                    {
                        if (Convert.ToDateTime(endDate) == Convert.ToDateTime(DepList[i]._Sale_date) ||
                            Convert.ToDateTime(endDate) > Convert.ToDateTime(DepList[i]._Sale_date))
                        {
                            endDate = DepList[i]._Sale_date;
                        }
                        else if (Convert.ToDateTime(endDate) < Convert.ToDateTime(DepList[i]._Sale_date))
                        {
                            DepList[i]._Sale_status = "Free";
                        }
                    }
                    if (DepList[i]._Sale_status.ToString() == "Asset Impaired")
                    {
                        if (Convert.ToDateTime(endDate) == Convert.ToDateTime(DepList[i]._Imp_date) ||
                        Convert.ToDateTime(endDate) > Convert.ToDateTime(DepList[i]._Imp_date))
                        {
                            endDate = DepList[i]._Imp_date.ToString();
                        }
                        else if (Convert.ToDateTime(endDate) < Convert.ToDateTime(DepList[i]._Imp_date))
                        {
                            DepList[i]._Sale_status = "Free";
                        }
                    }
                    if (DepList[i]._Sale_status.ToString() == "Asset Split")
                    {
                        if (Convert.ToDateTime(endDate) == Convert.ToDateTime(DepList[i]._Split_date) ||
                        Convert.ToDateTime(endDate) > Convert.ToDateTime(DepList[i]._Split_date))
                        {
                            endDate = DepList[i]._Split_date.ToString();
                        }
                        else if (Convert.ToDateTime(endDate) < Convert.ToDateTime(DepList[i]._Split_date))
                        {
                            DepList[i]._Sale_status = "Free";
                        }
                    }

                    if (DepList[i]._Sale_status.ToString() == "Impairment in Process" ||
                        DepList[i]._Sale_status.ToString() == "Sale in Process" ||
                        DepList[i]._Sale_status.ToString() == "Write-off in Process" ||
                        DepList[i]._Sale_status.ToString() == "Code Change in Process" ||
                        DepList[i]._Sale_status.ToString() == "Asset Merge in Process" ||
                        DepList[i]._Sale_status.ToString() == "Asset Split in Process")
                    {
                        DepList[i]._Sale_status = "Free";
                    }

                    //if (Convert.ToDateTime(DepList[i]._cap_date).Month == DateTime.Today.Month)
                    //{
                    //    DepList[i]._Sale_status = "Purchased";
                    //}
                    if (DepList[i]._Sale_status == "Free")
                    {
                        endDate = Convert.ToDateTime(lstdate + ifr.Format(dep._current_month.ToString())).ToShortDateString();
                        if (Convert.ToDateTime(dep._current_month).Month == DateTime.Today.Month)
                        {
                            endDate = DateTime.Today.ToShortDateString();
                        }
                    }
                    if (Convert.ToDateTime(endDate).Month == DateTime.Today.Month)
                    {

                        DepList[i]._current_month = (ifr.GetTotalDep(Convert.ToDateTime(stratDate), Convert.ToDateTime(endDate),
                                                            DepList[i]._asset_value, DepList[i]._asset_code, DepList[i]._is_5k,
                                                           DepList[i]._loc, "", Convert.ToDateTime(DepList[i]._branchdate_start),
                                                            Convert.ToDateTime(DepList[i]._leasedate_start), Convert.ToDateTime(DepList[i]._leasedate_end)
                                                            , DepList[i]._dep_type, DepList[i]._dep_rate, DepList[i]._group_id, DepList[i]._po_number
                                                            , DepList[i]._recon_ref, DepList[i]._group_id, DepList[i]._Trf_value,
                                                           DepList[i]._dep_full, 0)).ToString();
                    }
                    else
                    {
                        DepList[i]._current_month = "0";
                    }
                    decimal depamount = Convert.ToDecimal(DepList[i]._dep_amt) - Convert.ToDecimal(DepList[i]._current_month);
                    decimal cumdep2 = depamount + Convert.ToDecimal(DepList[i]._current_month);
                    decimal wdv1 = (DepList[i]._asset_value - depamount);
                    decimal wdv2 = wdv1 - Convert.ToDecimal(DepList[i]._current_month);
                    dt.Rows.Add(
                        i + 1
                    , DepList[i]._asset_id.ToString()
                    , DepList[i]._group_id.ToString()
                    , DepList[i]._asset_code.ToString()
                    , DepList[i]._AssetDesp.ToString()
                    , DepList[i]._asset_glcode.ToString()
                    , DepList[i]._cap_date.ToString()
                    , DepList[i]._asset_value.ToString()
                    , DepList[i]._is_5k.ToString()
                        //, DepList[i]._dep_amt.ToString()
                    , depamount
                    , wdv1.ToString()
                    , DepList[i]._Sale_status.ToString()
                    , DepList[i]._dep_type.ToString()
                    , DepList[i]._dep_rate.ToString()
                    , stratDate
                    , endDate
                    , DepList[i]._current_month.ToString()
                    , cumdep2.ToString()
                    , wdv2.ToString()
                   );
                }
                //export to excel from gridview
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Session["exceldownload"] = gv;
                if (gv.Rows.Count != 0)
                {
                    return new DownloadFileActionResult((GridView)Session["exceldownload"], "DepreciationReport.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
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
            return RedirectToAction("DepreciationForecast");
        }

        //[HttpGet]
        //public JsonResult GetForecasteList()
        //{
        //    List<Depreciation> lstforHeld = new List<Depreciation>();
        //    lstforHeld = (List<Depreciation>)Session["SearchforHeld"];
        //    if (lstforHeld == null)
        //    {
        //        lstforHeld = ifr.GetdetailsforDepHold();
        //        Session["SearchforHeld"] = lstforHeld;
        //    }
        //    return Json(lstforHeld, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult ToGetforHeld()
        //{
        //    List<Depreciation> lstforHeld = new List<Depreciation>();
        //    lstforHeld = (List<Depreciation>)Session["SearchforHeld"];
        //    if (lstforHeld == null)
        //    {
        //        lstforHeld = ifr.GetdetailsforDepHold();
        //        Session["SearchforHeld"] = lstforHeld;
        //    }
        //    return Json(lstforHeld, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult ToGetforHeldIndex()
        //{
        //    string data0 = string.Empty, data1 = string.Empty;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt;
        //        ds = ifr.GetdetailsforDepHold();
        //        dt = ds.Tables[0];
        //        //DataSet ds1 = new DataSet();
        //        //DataTable dt1;
        //        //ds1 = ifr.GetdetailsforDepRelease();
        //        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
        //        dt = ds.Tables[1];
        //        if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
        //        return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult ToGetforfree()
        //{
        //    List<Depreciation> lstforfree = new List<Depreciation>();
        //    lstforfree = (List<Depreciation>)Session["Searchforfree"];
        //    if (lstforfree == null)
        //    {
        //        lstforfree = ifr.GetdetailsforDepRelease();
        //        Session["Searchforfree"] = lstforfree;
        //    }
        //    return Json(lstforfree, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult ToGetforfree()
        //{
        //    string data0 = string.Empty;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt;
        //        ds = ifr.GetdetailsforDepRelease();
        //        dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
        //        return Json(new { data0 }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json(new { data0 }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult DepreciationRunNew(string txtmonthpicker, string command, Depreciation depr)
        {
            string RAN = "";
            try
            {
                Depreciation dep = new Depreciation();
                dep._Till_date = "01-" + ifr.Format(depr._date);
                DateTime date = Convert.ToDateTime(dep._Till_date);
                var thisYear = date.AddMonths(-1);
                Session["depdate"] = Convert.ToString(thisYear);
                dep._Till_date = "01-" + ifr.Format(depr._date);
                Session["date"] = dep._Till_date;
                string finyear = ifr.toGetFincialyear();
                string[] yeararr = finyear.Split('-');
                DateTime dtcheck = Convert.ToDateTime("01-04-" + yeararr[0]);
                if (ifr.DepreciationDone("01-" + ifr.Format(depr._date).ToString(), "Final"))
                {
                    return Json("Already initiated for 01-" + ifr.Format(depr._date).ToString(), JsonRequestBehavior.AllowGet);
                }
                if (Convert.ToDateTime(dep._Till_date) < dtcheck)
                {
                    string finyear1 = ifr.toGetFincialyear(date);
                    return Json("Already initiated for the finacial year " + finyear1, JsonRequestBehavior.AllowGet);
                }
                DateTime date1 = dtcheck;
                DateTime date2 = Convert.ToDateTime(dep._Till_date);
                int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                if (iMonthCount.ToString().Contains("-") == true)
                {
                    iMonthCount = (iMonthCount * -1) + 1;
                }
                else
                {
                    iMonthCount = (iMonthCount + 1);
                }
                //ifr.UpdateReversals(); //Muthu(23-01-2017 (Empty line insert in fa_trn_tassetdetails so hide.))
                if (command == "RunDepreciation")
                {
                    ifr.InsertFromDepToPreDep();
                }
                if (command == "ForecasteRun")
                {
                    ifr.InsertFromDepToForeDep();
                }
                for (int i = 1; i <= iMonthCount; i++)
                {
                    if (!ifr.DepreciationDone(dtcheck.ToString(), "Final"))
                    {
                        int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                        DateTime daycount1 = Convert.ToDateTime(lastday + ifr.Format(dtcheck.ToString()));
                        dep._Till_date = daycount1.ToString();
                        DateTime datecheckend = Convert.ToDateTime("01-" + ifr.Format(depr._date));
                        int lastday2 = DateTime.DaysInMonth(datecheckend.Year, datecheckend.Month);
                        DateTime daycount2 = Convert.ToDateTime(lastday2 + ifr.Format(depr._date));
                        if (daycount1 > daycount2)
                        {
                            dep._Till_date = depr._date.ToString();
                        }
                        if (command == "RunDepreciation")
                        {
                            ifr.InsertDepMonths(date.ToString(), thisYear.ToString(), "1", dep._Till_date.ToString());
                            RAN = ifr.SelectDepDetailsnInsert(Convert.ToDateTime("01-" + ifr.Format(dtcheck.ToShortDateString())), "prerun");
                        }
                        if (command == "ForecasteRun")
                        {
                            ifr.InsertDepMonths(date.ToString(), thisYear.ToString(), "2", dep._Till_date.ToString());
                            RAN = ifr.SelectDepDetailsnInsert(Convert.ToDateTime("01-" + ifr.Format(dtcheck.ToShortDateString())), "forerun");
                        }
                        else if (command == "FinalRun")
                        {
                            ifr.InsertDepMonths(date.ToString(), thisYear.ToString(), "3", dep._Till_date.ToString());
                            RAN = ifr.SelectDepDetailsnInsert(Convert.ToDateTime("01-" + ifr.Format(dtcheck.ToShortDateString())), "run");
                        }
                    }
                    dtcheck = dtcheck.AddMonths(1);
                }
                if (RAN == "success")
                    RAN = "RAN";
                else
                    RAN = "Error , Please checkdata.";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                RAN = "Error occured , Please checkdata.";
            }
            finally
            {
                ifr.EndProcess("DEPR_RUN");
            }
            return Json(RAN, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FARDetailedExport()
        {
            Depreciation depfore = new Depreciation();
            depfore = ifr.selcetdates("2");
            string selecteddepdate = depfore._current_month;
            List<AssetReportModel> FAList = new List<AssetReportModel>();
            FAList = ifr.SelectFARreportDetails(selecteddepdate, DateTime.Today.ToShortDateString());
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("INWARD DATE");
            dt.Columns.Add("ECF NUMBER");
            dt.Columns.Add("GROUP ID");
            dt.Columns.Add("ASSET_ID");
            dt.Columns.Add("QUANTITY");
            dt.Columns.Add("ASSET CODE");
            dt.Columns.Add("DESCRIPTION");
            dt.Columns.Add("GLCODE");
            dt.Columns.Add("DEPR TYPE");
            dt.Columns.Add("DEPR RATE");
            dt.Columns.Add("DEPR GL");
            dt.Columns.Add("DEPR PV GL");
            dt.Columns.Add("BRANCH CODE");
            dt.Columns.Add("BRANCH LAUNCH_DATE");
            dt.Columns.Add("LEASE START DATE");
            dt.Columns.Add("LEASE END DATE");
            dt.Columns.Add("CAPITALISATION DATE");
            dt.Columns.Add("ASSET VALUE");
            dt.Columns.Add("APR");
            dt.Columns.Add("MAY");
            dt.Columns.Add("JUN");
            dt.Columns.Add("JUL");
            dt.Columns.Add("AUG");
            dt.Columns.Add("SEP");
            dt.Columns.Add("OCT");
            dt.Columns.Add("NOV");
            dt.Columns.Add("DEC");
            dt.Columns.Add("JAN");
            dt.Columns.Add("FEB");
            dt.Columns.Add("MAR");
            dt.Columns.Add("ACCUMULATED_DEP");
            dt.Columns.Add("CUMMULATIVE_DEP");
            dt.Columns.Add("TOTAL DEP");
            dt.Columns.Add("WDV");
            dt.Columns.Add("TRANSFER FROM");
            dt.Columns.Add("TRANSFER VALUE");
            dt.Columns.Add("ASSET STATUS");
            dt.Columns.Add("Active Status");
            dt.Columns.Add("TRANSFER DATE");
            dt.Columns.Add("SALE DATE");
            dt.Columns.Add("DEPARTMENT");
            dt.Columns.Add("ASSET BSCC");
            dt.Columns.Add("PO NUMBER");
            dt.Columns.Add("CBF NUMBER");
            dt.Columns.Add("VENDOR NAME");
            dt.Columns.Add("BRANCH_NAME");
            dt.Columns.Add("OFFICE");
            dt.Columns.Add("BRANCH_STATUS");
            dt.Columns.Add("SALE_VALUE");
            dt.Columns.Add("VAT_VALUE");
            dt.Columns.Add("NET_LOSS");
            dt.Columns.Add("NARRATION");
            //GridView gv = new GridView();
            //gv.DataSource = dt;
            //gv.DataBind();
            for (int i = 0; i < FAList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    FAList[i].inwarddate.ToString(),
                    FAList[i].acfnumber.ToString(),
                    FAList[i].assetdetgroupid.ToString(),
                    FAList[i].assetdetDetid.ToString(),
                    FAList[i].assetdetqty.ToString(),
                    FAList[i].assetdetCode.ToString(),
                    FAList[i].assetdetDescription.ToString(),
                    FAList[i].assetdetglcode.ToString(),
                    FAList[i].deptype.ToString(),
                    FAList[i].deprate.ToString(),
                    FAList[i].depgl.ToString(),
                    FAList[i].deppvgl.ToString(),
                    FAList[i].assetdetLocationcode.ToString(),
                    FAList[i].branchlaunchdat.ToString(),
                    FAList[i].assetleasedat.ToString(),
                    FAList[i].assetleaseenddat.ToString(),
                    FAList[i].assetcapdate.ToString(),
                    FAList[i].assetdetAssetvalue.ToString(),
                    FAList[i].depapr.ToString(),
                    FAList[i].depmay.ToString(),
                    FAList[i].depjun.ToString(),
                    FAList[i].depjul.ToString(),
                    FAList[i].depaug.ToString(),
                    FAList[i].depsep.ToString(),
                    FAList[i].depoct.ToString(),
                    FAList[i].depnov.ToString(),
                    FAList[i].depdec.ToString(),
                    FAList[i].depjan.ToString(),
                    FAList[i].depfeb.ToString(),
                    FAList[i].depmar.ToString(),
                    FAList[i].ACCUMULATED_DEP.ToString(),
                    FAList[i].CUMMULATIVE_DEP.ToString(),
                    FAList[i].deptot.ToString(),
                    FAList[i].wdv.ToString(),
                    FAList[i].assettrffrm.ToString(),
                    FAList[i].assetdettrfval.ToString(),
                    FAList[i].assetdetstaus.ToString(),
                    FAList[i].assetdetstatus1.ToString(),
                    FAList[i].assetdettrfdat.ToString(),
                    FAList[i].assetdetsaledat.ToString(),
                    FAList[i].department.ToString(),
                    FAList[i].bscc.ToString(),
                    FAList[i].ponumb.ToString(),
                    FAList[i].cbfnumb.ToString(),
                    FAList[i].vendornam.ToString(),
                    FAList[i].BRName.ToString(),
                    FAList[i].BRAddress.ToString(),
                    FAList[i].Branch_Status.ToString(),
                    FAList[i].SALE_VALUE.ToString(),
                    FAList[i].VAT_VALUE.ToString(),
                    FAList[i].NET_LOSS.ToString(),
                    FAList[i].Naration.ToString()
                  );

            }


            string attachment = "attachment; filename=FAR Forecast Report.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int k;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (k = 0; k < dt.Columns.Count; k++)
                    {
                        Response.Write(tab + dr[k].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "FAR Forecast Report.xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            //gv.AllowPaging = false;
            //gv.DataSource = dt;
            //gv.DataBind();
            ////Change the Header Row back to white color
            //gv.HeaderRow.Style.Add("background-color", "#FFFFFF");

            ////Applying stlye to gridview header cells
            //for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
            //{
            //    gv.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
            //    gv.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
            //}
            //gv.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();
            return RedirectToAction("DepreciationForecast");

        }

    }


}
