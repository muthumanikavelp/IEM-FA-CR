using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.Web.Script.Serialization;
using System.Web.Helpers;
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class ReportController : Controller
    {
        private ReportRepository_M Rr;
        private Ifams_RptRepository R;
        private CmnFunctions objcmnr = new CmnFunctions();
        ErrorLog objerrlog = new ErrorLog();

        public ReportController() : this(new ReportDataModel_M(), new Ifams_RptDataModel()) { }

        public ReportController(ReportDataModel_M objRModel,Ifams_RptDataModel objreport)
        {
            Rr = objRModel;
            R = objreport;
        }


        //[HttpPost]
        public ActionResult Sales(string command, string sano, string Chequeno, string sainvoiceno, string spfilter1, string fDate, string tDate)
        {
            AssetReportModel salereport = new AssetReportModel();
            List<AssetReportModel> salereport1 = new List<AssetReportModel>();
            try
            {
                if (command == "SEARCH")
                {
                    salereport.ReportModel = Rr.GetSalereport().ToList();
                    if (sano != null && sano != "")
                    {
                        ViewBag.sano = sano;
                        salereport.ReportModel = salereport.ReportModel.Where(x => sano.ToUpper() == null || (x.soaSalenumber.ToUpper().Contains(sano.ToUpper()))).ToList();
                    }
                    if (Chequeno != null && Chequeno != "")
                    {
                        ViewBag.Chequeno = Chequeno;
                        salereport.ReportModel = salereport.ReportModel.Where(x => Chequeno.ToUpper() == null || (x.soapayChqno.ToUpper().Contains(Chequeno.ToUpper()))).ToList();
                    }
                    if (sainvoiceno != null && sainvoiceno != "")
                    {
                        ViewBag.sainvoiceno = sainvoiceno;
                        salereport.ReportModel = salereport.ReportModel.Where(x => sainvoiceno.ToUpper() == null || (x.soainvno.ToUpper().Contains(sainvoiceno.ToUpper()))).ToList();
                    }
                    if (spfilter1 != null && spfilter1 != "")
                    {
                        ViewBag.spfilter1 = spfilter1;
                        salereport.ReportModel = salereport.ReportModel.Where(x => spfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(spfilter1.ToUpper()))).ToList();
                    }
                    if (fDate != null && fDate != "" && fDate != null && fDate != "")
                    {
                        ViewBag.fDate = fDate;
                        ViewBag.tDate = tDate;
                        //salereport.ReportModel = salereport.ReportModel.Where(x => Convert.ToDateTime(fDate) == null || Convert.ToDateTime(x.soaProcessdate) >= Convert.ToDateTime(fDate) && (Convert.ToDateTime(tDate) == null || Convert.ToDateTime(x.soaProcessdate) <= Convert.ToDateTime(tDate))).ToList();
                        salereport.ReportModel = salereport.ReportModel.Where(x => Convert.ToDateTime(fDate) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.soaProcessdate) ? "01-01-1880" : x.soaProcessdate) >= Convert.ToDateTime(fDate) && (Convert.ToDateTime(tDate) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.soaProcessdate) ? "01-01-1880" : x.soaProcessdate) <= Convert.ToDateTime(tDate))).ToList();

                    }

                    if (salereport.ReportModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["saexcel"] = salereport.ReportModel;

                    return View(salereport);
                }
                else
                {
                    salereport.ReportModel = Rr.GetSalereport().ToList();
                    if (salereport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["saexcel"] = salereport.ReportModel;
                    return View(salereport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //  return Json("", JsonRequestBehavior.AllowGet);
                return View(salereport);
            }
            finally
            {

            }
        }
        //// [HttpPost]
        //  public PartialViewResult Sales()
        //  {
        //      AssetReportModel salereport = new AssetReportModel();
        //      try
        //      {
        //          salereport.ReportModel = Rr.GetSalereport().ToList();
        //          if (salereport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }


        //      }
        //      catch (Exception ex)
        //      {
        //          objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());


        //      }
        //      finally
        //      {
        //      }
        //      return PartialView(salereport);
        //  }

        //  [HttpPost]
        //  public ActionResult Sales(AssetReportModel status)
        //  {
        //     AssetReportModel salereport = new AssetReportModel();
        //     List<AssetReportModel> salereport1 = new List<AssetReportModel>();
        //      try
        //      {
        //          salereport.ReportModel = Rr.GetSalereport().ToList();
        //          if (status.soaSalenumber != null && status.soaSalenumber != "")
        //          {
        //              ViewBag.sano = status.soaSalenumber;
        //              salereport.ReportModel = salereport.ReportModel.Where(x => status.soaSalenumber.ToUpper() == null || (x.soaSalenumber.ToUpper().Contains(status.soaSalenumber.ToUpper()))).ToList(); 
        //          }
        //          if (status.soapayChqno != null && status.soapayChqno != "")
        //          {
        //              ViewBag.Chequeno = status.soapayChqno;
        //              salereport.ReportModel = salereport.ReportModel.Where(x => status.soapayChqno.ToUpper() == null || (x.soapayChqno.ToUpper().Contains(status.soapayChqno.ToUpper()))).ToList();
        //          }
        //          if (status.soainvno != null && status.soainvno != "")
        //          {
        //              ViewBag.sainvoiceno = status.soainvno;
        //              salereport.ReportModel = salereport.ReportModel.Where(x => status.soainvno.ToUpper() == null || (x.soainvno.ToUpper().Contains(status.soainvno.ToUpper()))).ToList();
        //          }
        //          if (status.assetdetDetid != null && status.assetdetDetid != "")
        //          {
        //              ViewBag.spfilter1 = status.assetdetDetid;
        //              salereport.ReportModel = salereport.ReportModel.Where(x => status.assetdetDetid.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(status.assetdetDetid.ToUpper()))).ToList();
        //          }
        //          if (status.fromdate != null && status.fromdate != "" && status.todate != null && status.todate != "")
        //          {
        //              ViewBag.fDate = status.fromdate;
        //              ViewBag.tDate = status.todate;
        //              //WAreport.ReportModel = WAreport.ReportModel.Where(x => fDateWO.ToUpper() == null || (x.woatfrdate.ToUpper().Contains(fDateWO.ToUpper())) || (x.woatfrdate.ToUpper().Contains(tDateWO.ToUpper()))).ToList();
        //              salereport.ReportModel = salereport.ReportModel.Where(x => Convert.ToDateTime(status.fromdate) == null || Convert.ToDateTime(x.soaProcessdate) >= Convert.ToDateTime(status.fromdate) && (Convert.ToDateTime(status.todate) == null || Convert.ToDateTime(x.soaProcessdate) <= Convert.ToDateTime(status.todate))).ToList();
        //          }
        //          //if (status.fromdate != null && status.fromdate != "" && status.todate != null && status.todate != "")
        //          //{
        //          //    ViewBag.fDate = status.fromdate;
        //          //    salereport1 = salereport.ReportModel.Where(x => status.fromdate.ToUpper() == null || (x.fromdate.ToUpper().Contains(status.fromdate.ToUpper()))).ToList();
        //          //}
        //          //if (status.todate != null && status.todate != "")
        //          //{
        //          //    ViewBag.tDate = status.todate;
        //          //    salereport.ReportModel = salereport.ReportModel.Where(x => status.todate.ToUpper() == null || (x.todate.ToUpper().Contains(status.todate.ToUpper()))).ToList();
        //          //}
        //          if (salereport.ReportModel.Count == 0)
        //          {
        //              ViewBag.Message = "No records found";
        //          }
        //          Session["saexcel"] = salereport.ReportModel;
        //         // return Json(salereport.ReportModel, JsonRequestBehavior.AllowGet);
        //          return View(salereport);
        //      }
        //      catch (Exception ex)
        //      {
        //          objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //          return Json("", JsonRequestBehavior.AllowGet);
        //      }
        //      finally
        //      {

        //      }
        //  }

        public ActionResult SAExport()
        {
            // // string sa = null;
            List<AssetReportModel> saList = new List<AssetReportModel>();
            if (Session["saexcel"] == null)
            {
                // saList = ifr.GetSADetail(Convert.ToString(objcmnr.GetLoginUserGid())).ToList();
            }
            else
            {
                saList = (List<AssetReportModel>)Session["saexcel"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Process Date");
            dt.Columns.Add("Sale NO");
            dt.Columns.Add("Sale Invoice No");
            dt.Columns.Add("Total Sale Value");
            dt.Columns.Add("VAT %");
            dt.Columns.Add("Total VAT Amount");
            dt.Columns.Add("Sale Date");
            dt.Columns.Add("Process Status");
            dt.Columns.Add("Maker ID");
            dt.Columns.Add("Checker ID");
            dt.Columns.Add("Job Code");
            dt.Columns.Add("Trans Date");
            dt.Columns.Add("Value Date");
            dt.Columns.Add("Asset ID");
            dt.Columns.Add("Asset Value");
            dt.Columns.Add("Sale Value");
            dt.Columns.Add("VAT");
            dt.Columns.Add("PL Value");
            dt.Columns.Add("Dep. Rect. Amount");
            dt.Columns.Add("Reason");
            dt.Columns.Add("Bank GL");
            dt.Columns.Add("Pay Mode");
            dt.Columns.Add("Cheque No");
            dt.Columns.Add("Cheque Amount");
            for (int i = 0; i < saList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    saList[i].soaProcessdate.ToString(),
                    saList[i].soaSalenumber.ToString(),
                    saList[i].soainvno.ToString(),
                    saList[i].soaSalevalue.ToString(),
                    saList[i].soaVatper.ToString(),
                    saList[i].soaVatamt.ToString(),
                    saList[i].soaSaledate.ToString(),
                    saList[i].soaStatus.ToString(),
                    saList[i].soaMakerid.ToString(),
                    saList[i].soaCheckerid.ToString(),
                    saList[i].soaJobcode.ToString(),
                    saList[i].assetdettrndate.ToString(),
                    saList[i].assetdetDetid.ToString(),
                    saList[i].assetdetAssetvalue.ToString(),
                    saList[i].soadetVatvalue.ToString(),
                    saList[i].soadetProfitloss.ToString(),
                    saList[i].soadetrectamt.ToString(),
                    saList[i].assetdettaken.ToString(),
                    saList[i].soapayGL.ToString(),
                    saList[i].soapaymode.ToString(),
                    saList[i].soapayChqno.ToString(),
                    saList[i].soapayAmount.ToString()

                    );
            }

            GridView GV = new GridView();
            GV.DataSource = dt;
            GV.DataBind();
            Session["Summary"] = GV;
            if (GV.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["Summary"], "Excelexport.xls");
            }
            else
            {
                ViewBag.Message = "No Records Found";
            }
            return RedirectToAction("Sales");
        }

        //Transfer Report
        public ActionResult Transfer(string tano, string trnfilter1, string fDateta, string tDatetrn, string command)
        {
            AssetReportModel trnreport = new AssetReportModel();
            try
            {
                if (command == "SEARCH")
                {
                    trnreport.ReportModel = Rr.GetTransreport().ToList();
                    if (!String.IsNullOrEmpty(tano))
                    {
                        ViewBag.tano = tano;
                        trnreport.ReportModel = trnreport.ReportModel.Where(x => tano.ToUpper() == null || (x.toaTrannumber.ToUpper().Contains(tano.ToUpper()))).ToList();
                    }

                    if (trnfilter1 != null && trnfilter1 != "")
                    {
                        ViewBag.trnfilter1 = trnfilter1;
                        trnreport.ReportModel = trnreport.ReportModel.Where(x => trnfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(trnfilter1.ToUpper()))).ToList();
                    }
                    if (fDateta != null && fDateta != "" && tDatetrn != null && tDatetrn != "")
                    {
                        ViewBag.fDateta = fDateta;
                        ViewBag.tDatetrn = tDatetrn;
                        trnreport.ReportModel = trnreport.ReportModel.Where(x => Convert.ToDateTime(fDateta) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.toaProcessdate) ? "01-01-1880" : x.toaProcessdate) >= Convert.ToDateTime(fDateta) && (Convert.ToDateTime(tDatetrn) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.toaProcessdate) ? "01-01-1880" : x.toaProcessdate) <= Convert.ToDateTime(tDatetrn))).ToList();
                    }

                    if (trnreport.ReportModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    // Session["taexcel"] = trnreport.ReportModel;
                    return View(trnreport);
                }
                else
                {
                    trnreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
                    if (trnreport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
                    // Session["taexcel"] = trnreport.ReportModel;
                    return View(trnreport);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return View(trnreport);

            }
            finally
            {
            }

        }

        [HttpGet]
        public ActionResult Transferangular()
        {
            AssetReportModel trnreport = new AssetReportModel();
            trnreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
            if (trnreport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
            // Session["taexcel"] = trnreport.ReportModel;
            return View(trnreport);
        }

        // [HttpPost]
        public JsonResult Transferangularjson()
        {
            var trndata = "";
            //var jsonResult = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
            var jsonResult = Json(new { trndata = JsonConvert.SerializeObject(0) }, JsonRequestBehavior.AllowGet);
            DataTable trnreport = new DataTable();
            trnreport = Rr.GetTransferangular();
            Session["taexcela"] = trnreport;
            if (trnreport.Rows.Count > 0)
            {// trndata = JsonConvert.SerializeObject(trnreport); 
                jsonResult = Json(new { trndata = JsonConvert.SerializeObject(trnreport) }, JsonRequestBehavior.AllowGet);
            }
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        //[HttpPost]
        // public ActionResult Transfer(AssetReportModel status)
        // {
        //     AssetReportModel Trnreport = new AssetReportModel();
        //     try
        //     {
        //         Trnreport.ReportModel = Rr.GetTransreport().ToList();
        //         if (!String.IsNullOrEmpty(status.toaTrannumber))
        //         {
        //             ViewBag.tano = status.toaTrannumber;
        //             Trnreport.ReportModel = Trnreport.ReportModel.Where(x => status.toaTrannumber.ToUpper() == null || (x.toaTrannumber.ToUpper().Contains(status.toaTrannumber.ToUpper()))).ToList();
        //         }

        //         if (status.assetdetDetid != null && status.assetdetDetid != "")
        //         {
        //             ViewBag.trnfilter1 = status.assetdetDetid;
        //             Trnreport.ReportModel = Trnreport.ReportModel.Where(x => status.assetdetDetid.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(status.assetdetDetid.ToUpper()))).ToList();
        //         }
        //         if (status.fromdate != null && status.fromdate != "" && status.todate != null && status.todate != "")
        //         {
        //             ViewBag.fDateta = status.fromdate;
        //             Trnreport.ReportModel = Trnreport.ReportModel.Where(x => status.fromdate.ToUpper() == null || (x.fromdate.ToUpper().Contains(status.fromdate.ToUpper()))).ToList();
        //         }
        //         if (status.todate != null && status.todate != "")
        //         {
        //             ViewBag.tDatetrn = status.todate;
        //             Trnreport.ReportModel = Trnreport.ReportModel.Where(x => status.todate.ToUpper() == null || (x.todate.ToUpper().Contains(status.todate.ToUpper()))).ToList();
        //         }
        //         if (Trnreport.ReportModel.Count == 0)
        //         {
        //             ViewBag.Message = "No records found";
        //         }
        //         Session["taexcel"] = Trnreport.ReportModel;
        //         return View(Trnreport);
        //     }
        //     catch (Exception ex)
        //     {
        //         objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //         return View(Trnreport);
        //     }
        //     finally
        //     {

        //     }
        // }

        public ActionResult TAExport()
        {


            DataTable dt = new DataTable();
            // dt.Columns.Add("S.No");
            dt.Columns.Add("Process Date");
            dt.Columns.Add("TA Number");
            dt.Columns.Add("Group ID");
            dt.Columns.Add("Asset Det ID");
            dt.Columns.Add("Asset Value");
            dt.Columns.Add("Transfer Value");
            dt.Columns.Add("New Asset ID");
            dt.Columns.Add("Transfer Date");
            dt.Columns.Add("Rectification Amount");
            dt.Columns.Add("Status");
            dt.Columns.Add("Job Code");
            dt.Columns.Add("Upload Date");
            dt.Columns.Add("Trans Date");
            dt.Columns.Add("Value Date");
            dt.Columns.Add("Maker ID");
            dt.Columns.Add("Checker ID");


            DataTable _downloadingData = Session["taexcel"] as DataTable;//Session["taexcela"]

            string attachment = "attachment; filename=TOAReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            if (_downloadingData.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int k;
                foreach (DataRow dr in _downloadingData.Rows)
                {
                    tab = "";
                    for (k = 0; k < _downloadingData.Columns.Count; k++)
                    {
                        Response.Write(tab + dr[k].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            return RedirectToAction("Transfer");
        }

        //Writeoff report
        public ActionResult Writeoff(string WOno, string WOfilter1, string fDateWO, string tDateWO, string command)
        {
            AssetReportModel WAreport = new AssetReportModel();
            try
            {
                if (command == "SEARCH")
                {
                    WAreport.ReportModel = Rr.GetWAreport().ToList();
                    if (!String.IsNullOrEmpty(WOno))
                    {
                        ViewBag.tano = WOno;
                        WAreport.ReportModel = WAreport.ReportModel.Where(x => WOno.ToUpper() == null || (x.woaWOnumber.ToUpper().Contains(WOno.ToUpper()))).ToList();
                    }

                    if (WOfilter1 != null && WOfilter1 != "")
                    {
                        ViewBag.trnfilter1 = WOfilter1;
                        WAreport.ReportModel = WAreport.ReportModel.Where(x => WOfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(WOfilter1.ToUpper()))).ToList();
                    }
                    if (fDateWO != null && fDateWO != "" && tDateWO != null && tDateWO != "")
                    {
                        ViewBag.fDateWO = fDateWO;
                        ViewBag.tDateWO = tDateWO;
                        //WAreport.ReportModel = WAreport.ReportModel.Where(x => fDateWO.ToUpper() == null || (x.woatfrdate.ToUpper().Contains(fDateWO.ToUpper())) || (x.woatfrdate.ToUpper().Contains(tDateWO.ToUpper()))).ToList();
                        WAreport.ReportModel = WAreport.ReportModel.Where(x => Convert.ToDateTime(fDateWO) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.woaProcessdate) ? "01-01-1880" : x.woaProcessdate) >= Convert.ToDateTime(fDateWO) && (Convert.ToDateTime(tDateWO) == null || Convert.ToDateTime(string.IsNullOrEmpty(x.woaProcessdate) ? "01-01-1880" : x.woaProcessdate) <= Convert.ToDateTime(tDateWO))).ToList();
                    }
                    //.Where(row => startTime == null || row.A_Date > startDate || (row.A_Date == startDate && row.A_Time >= startTime))
                    //if (tDateWO != null && tDateWO != "")
                    //{
                    //    ViewBag.tDatetrn = tDateWO;
                    //    WAreport.ReportModel = WAreport.ReportModel.Where(x => tDateWO.ToUpper() == null || (x.woatfrdate.ToUpper().Contains(tDateWO.ToUpper()))).ToList();
                    //}
                    if (WAreport.ReportModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["waexcel"] = WAreport.ReportModel;
                    return View(WAreport);
                }
                else
                {
                    WAreport.ReportModel = Rr.GetWAreport().ToList();
                    if (WAreport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["waexcel"] = WAreport.ReportModel;
                    return View(WAreport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return View(WAreport);
        }

        public ActionResult WAExport()
        {
            // string sa = null;
            List<AssetReportModel> waList = new List<AssetReportModel>();
            if (Session["waexcel"] == null)
            {
                // saList = ifr.GetSADetail(Convert.ToString(objcmnr.GetLoginUserGid())).ToList();
            }
            else
            {
                waList = (List<AssetReportModel>)Session["waexcel"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Process Date");
            dt.Columns.Add("WA Number");
            dt.Columns.Add("Status");
            dt.Columns.Add("Job Code");
            dt.Columns.Add("Trans Date");
            dt.Columns.Add("Value Date");
            dt.Columns.Add("Maker ID");
            dt.Columns.Add("Checker ID");
            dt.Columns.Add("Asset ID");
            dt.Columns.Add("Asset Value");
            dt.Columns.Add("Dep.Till Date");
            dt.Columns.Add("Reason");
            dt.Columns.Add("Writte Off Date");
            dt.Columns.Add("Rectification Amount");
            for (int i = 0; i < waList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    waList[i].woaProcessdate.ToString(),
                    waList[i].woaWOnumber.ToString(),
                    waList[i].woaStatus.ToString(),
                    waList[i].woaJobcode.ToString(),
                    waList[i].woatfrdate.ToString(),
                    waList[i].woatfrvaluedate.ToString(),
                    waList[i].woaMakerid.ToString(),
                    waList[i].woaCheckerid.ToString(),
                    waList[i].assetdetDetid.ToString(),
                    waList[i].assetdetAssetvalue.ToString(),
                    waList[i].woadeptilldate.ToString(),
                    waList[i].assetdettaken.ToString(),
                    waList[i].woaWOdate.ToString(),
                    waList[i].woadetrectamt.ToString()
                                       );
            }

            GridView GV = new GridView();
            GV.DataSource = dt;
            GV.DataBind();
            Session["Summary"] = GV;
            if (GV.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["Summary"], "Excelexport.xls");
            }
            else
            {
                ViewBag.Message = "No Records Found";
            }
            return RedirectToAction("Writeoff");
        }

        //AssetCC report
        public ActionResult AssetCodeChange(string oacc, string nacc, string Grpid, string accfilter1, string fDateacc, string tDateacc, string command)
        {
            AssetReportModel ACCreport = new AssetReportModel();
            try
            {
                if (command == "SEARCH")
                {
                    ACCreport.ReportModel = Rr.GetACCreport().ToList();
                    if (!String.IsNullOrEmpty(oacc))
                    {
                        ViewBag.oacc = oacc;
                        ACCreport.ReportModel = ACCreport.ReportModel.Where(x => oacc.ToUpper() == null || (x.assetdetCode.ToUpper().Contains(oacc.ToUpper()))).ToList();
                    }

                    if (!String.IsNullOrEmpty(nacc))
                    {
                        ViewBag.nacc = nacc;
                        ACCreport.ReportModel = ACCreport.ReportModel.Where(x => nacc.ToUpper() == null || (x.accnewassetcode.ToUpper().Contains(nacc.ToUpper()))).ToList();
                    }

                    if (!String.IsNullOrEmpty(Grpid))
                    {
                        ViewBag.Grpid = Grpid;
                        ACCreport.ReportModel = ACCreport.ReportModel.Where(x => Grpid.ToUpper() == null || (x.assetdetgroupid.ToUpper().Contains(Grpid.ToUpper()))).ToList();
                    }

                    if (accfilter1 != null && accfilter1 != "")
                    {
                        ViewBag.trnfilter1 = accfilter1;
                        ACCreport.ReportModel = ACCreport.ReportModel.Where(x => accfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(accfilter1.ToUpper()))).ToList();
                    }
                    if (fDateacc != null && fDateacc != "" && tDateacc != null && tDateacc != "")
                    {
                        ViewBag.fDateacc = fDateacc;
                        ViewBag.tDateacc = tDateacc;
                        ACCreport.ReportModel = ACCreport.ReportModel.Where(x => Convert.ToDateTime(fDateacc) == null || Convert.ToDateTime(x.accupdatedate) >= Convert.ToDateTime(fDateacc) && (Convert.ToDateTime(tDateacc) == null || Convert.ToDateTime(x.accupdatedate) <= Convert.ToDateTime(tDateacc))).ToList();
                    }
                    //if (fDateacc != null && fDateacc != "" )
                    //{
                    //    ViewBag.fDateta = fDateacc;
                    //    ACCreport.ReportModel = ACCreport.ReportModel.Where(x => fDateacc.ToUpper() == null || (x.fromdate.ToUpper().Contains(fDateacc.ToUpper()))).ToList();
                    //}
                    //if (tDateacc != null && tDateacc != "")
                    //{
                    //    ViewBag.tDatetrn = tDateacc;
                    //    ACCreport.ReportModel = ACCreport.ReportModel.Where(x => tDateacc.ToUpper() == null || (x.todate.ToUpper().Contains(tDateacc.ToUpper()))).ToList();
                    //}
                    if (ACCreport.ReportModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["accexcel"] = ACCreport.ReportModel;
                    return View(ACCreport);
                }
                else
                {
                    ACCreport.ReportModel = Rr.GetACCreport().ToList();
                    if (ACCreport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["accexcel"] = ACCreport.ReportModel;
                    return View(ACCreport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return View(ACCreport);

            }
            finally
            {
            }

        }

        public ActionResult ACCExport()
        {
            // string sa = null;
            List<AssetReportModel> accList = new List<AssetReportModel>();
            if (Session["accexcel"] == null)
            {
                // saList = ifr.GetSADetail(Convert.ToString(objcmnr.GetLoginUserGid())).ToList();
            }
            else
            {
                accList = (List<AssetReportModel>)Session["accexcel"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("ASSET GROUP ID");
            dt.Columns.Add("ASSET ID");
            dt.Columns.Add("OLD ASSET CODE");
            dt.Columns.Add("NEW ASSET ID");
            dt.Columns.Add("NEW ASSET CODE");
            dt.Columns.Add("ASSET VALUE");
            dt.Columns.Add("UPDATED ON");
            dt.Columns.Add("UPDATED BY");
            dt.Columns.Add("REMARKS");

            for (int i = 0; i < accList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    accList[i].assetdetgroupid.ToString(),
                    accList[i].assetdetDetid.ToString(),
                    accList[i].assetdetCode.ToString(),
                    accList[i].accnewassetdetid.ToString(),
                    accList[i].accnewassetcode.ToString(),
                    accList[i].assetdetAssetvalue.ToString(),
                    accList[i].accupdateby.ToString(),
                    accList[i].accupdatedate.ToString(),
                    accList[i].Reason.ToString()

                                       );
            }

            GridView GV = new GridView();
            GV.DataSource = dt;
            GV.DataBind();
            Session["Summary"] = GV;
            if (GV.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["Summary"], "Excelexport.xls");
            }
            else
            {
                ViewBag.Message = "No Records Found";
            }
            return RedirectToAction("AssetCodeChange");
        }

        //physical asset report
        public ActionResult physicalassetid(string pid, string pGrpid, string pidfilter1, string fDatepid, string tDatepid, string command)
        {
            AssetReportModel PIDreport = new AssetReportModel();
            try
            {
                if (command == "SEARCH")
                {
                    PIDreport.ReportModel = Rr.GetPIDreport().ToList();
                    //if (!String.IsNullOrEmpty(oacc))
                    //{
                    //    ViewBag.oacc = oacc;
                    //    ACCreport.ReportModel = ACCreport.ReportModel.Where(x => oacc.ToUpper() == null || (x.assetdetCode.ToUpper().Contains(oacc.ToUpper()))).ToList();
                    //}

                    if (!String.IsNullOrEmpty(pid))
                    {
                        ViewBag.pid = pid;
                        PIDreport.ReportModel = PIDreport.ReportModel.Where(x => pid.ToUpper() == null || (x.goaphysicalID.ToUpper().Contains(pid.ToUpper()))).ToList();
                    }

                    if (!String.IsNullOrEmpty(pGrpid))
                    {
                        ViewBag.pGrpid = pGrpid;
                        PIDreport.ReportModel = PIDreport.ReportModel.Where(x => pGrpid.ToUpper() == null || (x.assetdetgroupid.ToUpper().Contains(pGrpid.ToUpper()))).ToList();
                    }

                    if (pidfilter1 != null && pidfilter1 != "")
                    {
                        ViewBag.trnfilter1 = pidfilter1;
                        PIDreport.ReportModel = PIDreport.ReportModel.Where(x => pidfilter1.ToUpper() == null || (x.assetdetDetid.ToUpper().Contains(pidfilter1.ToUpper()))).ToList();
                    }
                    if (fDatepid != null && fDatepid != "" && tDatepid != null && tDatepid != "")
                    {
                        ViewBag.fDatepid = fDatepid;
                        ViewBag.tDatepid = tDatepid;
                        PIDreport.ReportModel = PIDreport.ReportModel.Where(x => Convert.ToDateTime(fDatepid) == null || Convert.ToDateTime(x.goaupdatedate) >= Convert.ToDateTime(fDatepid) && (Convert.ToDateTime(tDatepid) == null || Convert.ToDateTime(x.goaupdatedate) <= Convert.ToDateTime(tDatepid))).ToList();
                    }
                    //if (fDatepid != null && fDatepid != "")
                    //{
                    //    ViewBag.fDatepid = fDatepid;
                    //    PIDreport.ReportModel = PIDreport.ReportModel.Where(x => fDatepid.ToUpper() == null || (x.fromdate.ToUpper().Contains(fDatepid.ToUpper()))).ToList();
                    //}
                    //if (tDatepid != null && tDatepid != "")
                    //{
                    //    ViewBag.tDatepid = tDatepid;
                    //    PIDreport.ReportModel = PIDreport.ReportModel.Where(x => tDatepid.ToUpper() == null || (x.todate.ToUpper().Contains(tDatepid.ToUpper()))).ToList();
                    //}
                    if (PIDreport.ReportModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["pidexcel"] = PIDreport.ReportModel;
                    return View(PIDreport);
                }
                else
                {
                    PIDreport.ReportModel = Rr.GetACCreport().ToList();
                    if (PIDreport.ReportModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["pidexcel"] = PIDreport.ReportModel;
                    return View(PIDreport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return View(PIDreport);

            }
            finally
            {
            }

        }

        public ActionResult PIDExport()
        {
            // string sa = null;
            List<AssetReportModel> pidList = new List<AssetReportModel>();
            if (Session["pidexcel"] == null)
            {
                // saList = ifr.GetSADetail(Convert.ToString(objcmnr.GetLoginUserGid())).ToList();
            }
            else
            {
                pidList = (List<AssetReportModel>)Session["pidexcel"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("ASSET GROUP ID");
            dt.Columns.Add("ASSET ID");
            dt.Columns.Add("PHYSICAL ASSET ID");
            dt.Columns.Add("UPDATED ON");
            dt.Columns.Add("UPDATED BY");
            dt.Columns.Add("STATUS");

            for (int i = 0; i < pidList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    pidList[i].assetdetgroupid.ToString(),
                    pidList[i].assetdetDetid.ToString(),
                    pidList[i].goaphysicalID.ToString(),
                    pidList[i].accupdatedate.ToString(),
                    pidList[i].accupdateby.ToString(),
                    pidList[i].goastatus.ToString()

                                       );
            }

            GridView GV = new GridView();
            GV.DataSource = dt;
            GV.DataBind();
            Session["Summary"] = GV;
            if (GV.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["Summary"], "Excelexport.xls");
            }
            else
            {
                ViewBag.Message = "No Records Found";
            }
            return RedirectToAction("physicalassetid");
        }
        //[HttpGet]
        //public ActionResult FAreport(string depfd, string deptd)
        //{
        //    AssetReportModel FAreport = new AssetReportModel();
        //    try
        //    {

        //            Session["faexcel"] = null;
        //            // FAreport.ReportModel = Rr.GetFAreport(depfd, deptd).ToList();
        //            // Muthu DataTable fatable = new DataTable();
        //            DataTable Mfatable = new DataTable();
        //            // Muthu  Rr.deletefatemp();
        //            // Muthu fatable = Rr.GetFAreport(depfd, deptd, "");
        //            Mfatable = Rr.getfadep(depfd, deptd);
        //            ViewBag.depfd = depfd;
        //            ViewBag.deptd = deptd;
        //            FAreport.ReportModel = Rr.getFATableNew(Mfatable).ToList();
        //            // Muthu FAreport.ReportModel = Rr.getfatable(fatable).ToList();
        //            // FAreport.ReportModel = Rr.GetFAreportDetails(depfd).ToList();                    

        //            Session["faexcel"] = FAreport.ReportModel;
        //            return View(FAreport);

        //    }
        //    catch (Exception ex)
        //    {
        //        objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return View();
        //    }
        //    finally
        //    {

        //    }
        //}
        ////FA report
        //[HttpPost]
        public ActionResult FAreport(string Command, string inwfd, string inwtd, string depfd, string deptd, string history, string bcode, string glcode)
        {
            AssetReportModel FAreport = new AssetReportModel();
            try
            {
                //Session["faexcel"] = null;
                //if(Command != "SEARCH")
                //{
                //    Session["faexcel"] = null;
                //}
                // Command = "SEARCH";
                FAreport.history = history;
                if (!string.IsNullOrEmpty(depfd) && !string.IsNullOrEmpty(deptd))
                {
                    if (depfd != "1" && deptd != "1")
                    {
                        Session["depfdt"] = depfd;
                        Session["deptdt"] = deptd;
                    }
                    else
                    {
                        depfd = "";
                        deptd = "";
                        Session["depfdt"] = "";
                        Session["deptdt"] = "";
                    }
                }
                else
                {
                    if (Convert.ToString(Session["depfdt"]) != "" && Convert.ToString(Session["deptdt"]) != "")
                    {
                        depfd = Session["depfdt"].ToString();
                        deptd = Session["deptdt"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(depfd) && !string.IsNullOrEmpty(deptd))
                {
                    Session["faexcel"] = null;
                    // FAreport.ReportModel = Rr.GetFAreport(depfd, deptd).ToList();
                    // Muthu DataTable fatable = new DataTable();
                    DataTable Mfatable = new DataTable();
                    // Muthu  Rr.deletefatemp();
                    // Muthu fatable = Rr.GetFAreport(depfd, deptd, "");
                    Mfatable = Rr.getfadep(depfd, deptd, history, bcode, glcode);
                    ViewBag.depfd = depfd;
                    ViewBag.deptd = deptd;
                    FAreport.ReportModel = Rr.getFATableNew(Mfatable).ToList();
                    // Muthu FAreport.ReportModel = Rr.getfatable(fatable).ToList();
                    // FAreport.ReportModel = Rr.GetFAreportDetails(depfd).ToList();  
                    Session["FAtable"] = Mfatable;
                    if (!String.IsNullOrEmpty(inwfd) && !String.IsNullOrEmpty(inwtd))
                    {
                        ViewBag.inwfd = inwfd;
                        ViewBag.inwtd = inwtd;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => Convert.ToDateTime(inwfd) == null || Convert.ToDateTime(String.IsNullOrEmpty(x.inwarddate) ? "1990-01-01" : x.inwarddate.ToString()) >= Convert.ToDateTime(inwfd) && (Convert.ToDateTime(inwtd) == null || Convert.ToDateTime(String.IsNullOrEmpty(x.inwarddate) ? "1990-01-01" : x.inwarddate.ToString()) <= Convert.ToDateTime(inwtd))).ToList();
                    }
                    if (!String.IsNullOrEmpty(history))
                    {
                        ViewBag.history = history;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => history.ToUpper() == null || (x.assetdetgroupid.ToUpper().Contains(history.ToUpper()))).ToList();
                    }
                    if (!String.IsNullOrEmpty(bcode))
                    {
                        ViewBag.bcode = bcode;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => bcode.ToUpper() == null || (x.assetdetLocationcode.Equals(bcode.ToUpper()))).ToList();
                    }
                    if (!String.IsNullOrEmpty(glcode))
                    {
                        ViewBag.glcode = glcode;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => glcode.ToUpper() == null || (x.assetdetglcode.Equals(glcode.ToUpper()))).ToList();
                    }
                    Session["faexcel"] = FAreport.ReportModel;
                    return View(FAreport);
                }
                else
                {
                    Session["faexcel"] = null;
                    FAreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
                    if (Session["faexcel"] != null)
                    {
                        FAreport.ReportModel = (List<AssetReportModel>)Session["faexcel"];
                    }
                    else
                    {
                        ViewBag.Message = "No Records Found";
                    }
                    return View(FAreport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {

            }
        }

        //public ActionResult FAreportjs(string Command, string depfd, string deptd)
        //{
        //    AssetReportModel FAreport = new AssetReportModel();
        //    try
        //    {
        //        FAreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
        //        //    if (Session["faexcel"] != null)
        //        //    {
        //        //        FAreport.ReportModel = (List<AssetReportModel>)Session["faexcel"];
        //        //    }
        //        //    else
        //        //    {
        //        //        ViewBag.Message = "No Records Found";
        //        //    }
        //        return View(FAreport);
        //    }
        //    catch (Exception ex)
        //    {
        //        objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return View();
        //    }
        //    finally
        //    {

        //    }
        //}

        //public ActionResult FAreportj(string Command, string depfd, string deptd)
        //{
        //    AssetReportModel FAreport = new AssetReportModel();
        //    try
        //    {

        //        //FAreport.history = history;
        //        if (Command == "SEARCH")
        //        {
        //            Session["faexcel"] = null;
        //            // FAreport.ReportModel = Rr.GetFAreport(depfd, deptd).ToList();
        //            DataTable fatable = new DataTable();
        //            fatable = Rr.GetFAreport(depfd, deptd, "");

        //            FAreport.ReportModel = Rr.getfatable(fatable).ToList();



        //            Session["faexceltable"] = fatable;

        //            //if (!String.IsNullOrEmpty(inwfd) && !String.IsNullOrEmpty(inwtd))
        //            //{
        //            //    ViewBag.inwfd = inwfd;
        //            //    ViewBag.inwtd = inwtd;
        //            //    FAreport.ReportModel = FAreport.ReportModel.Where(x => Convert.ToDateTime(inwfd) == null || Convert.ToDateTime(x.inwarddate) >= Convert.ToDateTime(inwfd) && (Convert.ToDateTime(inwtd) == null || Convert.ToDateTime(x.inwarddate) <= Convert.ToDateTime(inwtd))).ToList();
        //            //}

        //            //if (depfd != null && depfd != "" && deptd != null && deptd != "")
        //            //{
        //            //    ViewBag.depfd = depfd;
        //            //    ViewBag.deptd = deptd;
        //            //    FAreport.ReportModel = FAreport.ReportModel.Where(x => Convert.ToDateTime(depfd) == null || Convert.ToDateTime(x.goaupdatedate) >= Convert.ToDateTime(depfd) && (Convert.ToDateTime(deptd) == null || Convert.ToDateTime(x.goaupdatedate) <= Convert.ToDateTime(deptd))).ToList();
        //            //}

        //            //if (!String.IsNullOrEmpty(history))
        //            //{
        //            //    ViewBag.history = history;
        //            //    FAreport.ReportModel = FAreport.ReportModel.Where(x => history.ToUpper() == null || (x.history.ToUpper().Contains(history.ToUpper()))).ToList();
        //            //}

        //            //if (!String.IsNullOrEmpty(bcode))
        //            //{
        //            //    ViewBag.bcode = bcode;
        //            //    FAreport.ReportModel = FAreport.ReportModel.Where(x => bcode.ToUpper() == null || (x.assetdetgroupid.Equals(bcode.ToUpper()))).ToList();
        //            //}
        //            Session["faexcel"] = FAreport.ReportModel;

        //        }
        //        //else
        //        //{
        //        //    //Rr.GroupIDupdate();
        //        //    Session["faexcel"] = null;
        //        //    FAreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
        //        //    if (Session["faexcel"] != null)
        //        //    {
        //        //        FAreport.ReportModel = (List<AssetReportModel>)Session["faexcel"];
        //        //    }
        //        //    else
        //        //    {
        //        //        ViewBag.Message = "No Records Found";
        //        //    }
        //        //    return View(FAreport);
        //        //}
        //        return Json(FAreport, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {

        //    }
        //}

        //public ActionResult FAExport()
        //{
        //    // string sa = null;
        //    string duplicateasset = "";
        //    DataTable dtn = new DataTable();
        //    List<AssetReportModel> FAList = new List<AssetReportModel>();
        //    if (Session["faexcel"] != null)
        //    {

        //        FAList = (List<AssetReportModel>)Session["faexcel"];

        //        dtn = (DataTable)Session["FAtable"];

        //    }

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("S.No");
        //    //dt.Columns.Add("ASSET GROUP ID");
        //    dt.Columns.Add("ACF_NUMBER");
        //    dt.Columns.Add("GROUP ID");
        //    dt.Columns.Add("ASSET_ID");
        //    dt.Columns.Add("QUANTITY");
        //    dt.Columns.Add("ASSET CODE");
        //    dt.Columns.Add("DESCRIPTION");
        //    dt.Columns.Add("GLCODE");
        //    dt.Columns.Add("DEPR TYPE");
        //    dt.Columns.Add("DEPR RATE");
        //    dt.Columns.Add("DEPR GL");
        //    dt.Columns.Add("DEPR PV GL");
        //    dt.Columns.Add("BRANCH CODE");
        //    dt.Columns.Add("BRANCH LAUNCH_DATE");
        //    dt.Columns.Add("LEASE START DATE");
        //    dt.Columns.Add("LEASE END DATE");
        //    dt.Columns.Add("CAPITALISATION DATE");
        //    // dt.Columns.Add("QUANTITY");
        //    dt.Columns.Add("ASSET VALUE");
        //    // dt.Columns.Add("YEARS");
        //    dt.Columns.Add("APR");
        //    dt.Columns.Add("MAY");
        //    dt.Columns.Add("JUN");
        //    dt.Columns.Add("JUL");
        //    dt.Columns.Add("AUG");
        //    dt.Columns.Add("SEP");
        //    dt.Columns.Add("OCT");
        //    dt.Columns.Add("NOV");
        //    dt.Columns.Add("DEC");
        //    dt.Columns.Add("JAN");
        //    dt.Columns.Add("FEB");
        //    dt.Columns.Add("MAR");
        //    dt.Columns.Add("ACCUMULATED_DEP");
        //    dt.Columns.Add("CUMMULATIVE_DEP");
        //    dt.Columns.Add("TOTAL DEPR");
        //    dt.Columns.Add("WDV");
        //    dt.Columns.Add("TRANSFER FROM");
        //    dt.Columns.Add("TRANSFER VALUE");
        //    dt.Columns.Add("ASSET STATUS");
        //    dt.Columns.Add("Active Status");
        //    dt.Columns.Add("TRANSFER DATE");
        //    dt.Columns.Add("SALE DATE");
        //    dt.Columns.Add("DEPARTMENT");
        //    //dt.Columns.Add("SPOKE");
        //    dt.Columns.Add("ASSET BSCC");
        //    //dt.Columns.Add("UPLOAD BSCC");
        //    dt.Columns.Add("PO NUMBER");
        //    dt.Columns.Add("CBF NUMBER");
        //    dt.Columns.Add("VENDOR NAME");
        //    dt.Columns.Add("BRANCH_NAME");
        //    dt.Columns.Add("OFFICE");
        //    dt.Columns.Add("BRANCH_STATUS");
        //    dt.Columns.Add("SALE_VALUE");
        //    dt.Columns.Add("VAT_VALUE");
        //    dt.Columns.Add("NET_LOSS");
        //    dt.Columns.Add("NARRATION");
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        int i1 = 1;
        //        //for (int i = 0; i < FAList.Count; i++)
        //        //{

        //        //    //if (FAList[i].assetdetDetid.ToString() == null)
        //        //    //    FAList[i].assetdetDetid = "";
        //        //    dt.Rows.Add(i + 1,
        //        //        // FAList[i].inwarddate.ToString(),
        //        //        FAList[i].acfnumber.ToString(),
        //        //        FAList[i].assetdetgroupid.ToString(),

        //        //          FAList[i].assetdetDetid.ToString(),
        //        //        FAList[i].assetdetqty.ToString(),
        //        //        FAList[i].assetdetCode.ToString(),
        //        //        FAList[i].assetdetDescription.ToString(),
        //        //        FAList[i].assetdetglcode.ToString(),
        //        //        FAList[i].deptype.ToString(),
        //        //        FAList[i].deprate.ToString(),
        //        //        FAList[i].depgl.ToString(),
        //        //        FAList[i].deppvgl.ToString(),
        //        //        FAList[i].assetdetLocationcode.ToString(),
        //        //        FAList[i].branchlaunchdat.ToString(),
        //        //        FAList[i].assetleasedat.ToString(),
        //        //        FAList[i].assetleaseenddat.ToString(),
        //        //        FAList[i].assetcapdate.ToString(),
        //        //        //  FAList[i].assetdetqty.ToString(),
        //        //        FAList[i].assetdetAssetvalue.ToString(),
        //        //        // FAList[i].Year.ToString(),
        //        //        FAList[i].depapr.ToString(),
        //        //        FAList[i].depmay.ToString(),
        //        //        FAList[i].depjun.ToString(),
        //        //        FAList[i].depjul.ToString(),
        //        //        FAList[i].depaug.ToString(),
        //        //        FAList[i].depsep.ToString(),
        //        //        FAList[i].depoct.ToString(),
        //        //        FAList[i].depnov.ToString(),
        //        //        FAList[i].depdec.ToString(),
        //        //        FAList[i].depjan.ToString(),
        //        //        FAList[i].depfeb.ToString(),
        //        //        FAList[i].depmar.ToString(),
        //        //        FAList[i].ACCUMULATED_DEP.ToString(),
        //        //        FAList[i].CUMMULATIVE_DEP.ToString(),
        //        //        FAList[i].deptot.ToString(),
        //        //        FAList[i].wdv.ToString(),
        //        //        FAList[i].assettrffrm.ToString(),
        //        //        FAList[i].assetdettrfval.ToString(),
        //        //        FAList[i].assetdetstaus.ToString(),
        //        //        FAList[i].assetdetstatus1.ToString(),
        //        //        FAList[i].assetdettrfdat.ToString(),
        //        //        FAList[i].assetdetsaledat.ToString(),
        //        //        FAList[i].department.ToString(),
        //        //        //FAList[i].assetdetspoke.ToString(),
        //        //        FAList[i].bscc.ToString(),
        //        //        //FAList[i].upbscc.ToString(),
        //        //        FAList[i].ponumb.ToString(),
        //        //        FAList[i].cbfnumb.ToString(),
        //        //        FAList[i].vendornam.ToString(),
        //        //        FAList[i].BRName.ToString(),
        //        //        FAList[i].BRAddress.ToString(),
        //        //        FAList[i].Branch_Status.ToString(),
        //        //        FAList[i].SALE_VALUE.ToString(),
        //        //        FAList[i].VAT_VALUE.ToString(),
        //        //        FAList[i].NET_LOSS.ToString(),
        //        //        FAList[i].Naration.ToString()
        //        //        //FAList[i].goastatus.ToString()

        //        //      );


        //        //    //if (dt.Rows.Count == 60000)
        //        //    //{
        //        //    //    if (dt != null)
        //        //    //    {

        //        //    //        var workSheet = wb.Worksheets.Add(dt, "Report" + i1);
        //        //    //        dt = new DataTable();
        //        //    //        i1 += 1;
        //        //    //        dt.Columns.Add("S.No");                            
        //        //    //        dt.Columns.Add("ACF_NUMBER");
        //        //    //        dt.Columns.Add("GROUP ID");
        //        //    //        dt.Columns.Add("ASSET_ID");
        //        //    //        dt.Columns.Add("QUANTITY");
        //        //    //        dt.Columns.Add("ASSET CODE");
        //        //    //        dt.Columns.Add("DESCRIPTION");
        //        //    //        dt.Columns.Add("GLCODE");
        //        //    //        dt.Columns.Add("DEPR TYPE");
        //        //    //        dt.Columns.Add("DEPR RATE");
        //        //    //        dt.Columns.Add("DEPR GL");
        //        //    //        dt.Columns.Add("DEPR PV GL");
        //        //    //        dt.Columns.Add("BRANCH CODE");
        //        //    //        dt.Columns.Add("BRANCH LAUNCH_DATE");
        //        //    //        dt.Columns.Add("LEASE START DATE");
        //        //    //        dt.Columns.Add("LEASE END DATE");
        //        //    //        dt.Columns.Add("CAPITALISATION DATE");
        //        //    //        dt.Columns.Add("ASSET VALUE");
        //        //    //        dt.Columns.Add("APR");
        //        //    //        dt.Columns.Add("MAY");
        //        //    //        dt.Columns.Add("JUN");
        //        //    //        dt.Columns.Add("JUL");
        //        //    //        dt.Columns.Add("AUG");
        //        //    //        dt.Columns.Add("SEP");
        //        //    //        dt.Columns.Add("OCT");
        //        //    //        dt.Columns.Add("NOV");
        //        //    //        dt.Columns.Add("DEC");
        //        //    //        dt.Columns.Add("JAN");
        //        //    //        dt.Columns.Add("FEB");
        //        //    //        dt.Columns.Add("MAR");
        //        //    //        dt.Columns.Add("ACCUMULATED_DEP");
        //        //    //        dt.Columns.Add("CUMMULATIVE_DEP");
        //        //    //        dt.Columns.Add("TOTAL DEPR");
        //        //    //        dt.Columns.Add("WDV");
        //        //    //        dt.Columns.Add("TRANSFER FROM");
        //        //    //        dt.Columns.Add("TRANSFER VALUE");
        //        //    //        dt.Columns.Add("ASSET STATUS");
        //        //    //        dt.Columns.Add("Active Status");
        //        //    //        dt.Columns.Add("TRANSFER DATE");
        //        //    //        dt.Columns.Add("SALE DATE");
        //        //    //        dt.Columns.Add("DEPARTMENT");
        //        //    //        dt.Columns.Add("ASSET BSCC");
        //        //    //        dt.Columns.Add("PO NUMBER");
        //        //    //        dt.Columns.Add("CBF NUMBER");
        //        //    //        dt.Columns.Add("VENDOR NAME");
        //        //    //        dt.Columns.Add("BRANCH_NAME");
        //        //    //        dt.Columns.Add("OFFICE");
        //        //    //        dt.Columns.Add("BRANCH_STATUS");
        //        //    //        dt.Columns.Add("SALE_VALUE");
        //        //    //        dt.Columns.Add("VAT_VALUE");
        //        //    //        dt.Columns.Add("NET_LOSS");
        //        //    //        dt.Columns.Add("NARRATION");
        //        //    //    }
        //        //    //}

        //        //}



        //        // ADD A WORKSHEET.

        //        wb.Worksheets.Add(dtn);
        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        wb.Style.Font.Bold = true;
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //        Response.AddHeader("content-disposition", "attachment;filename= FA_Report.xlsx");

        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }

        //    }

        //    //   DataTable _downloadingData = Session["GLDownlinkFile"] as DataTable;







        //    return RedirectToAction("FAreport");
        //}



        public ActionResult FAExporttxt()
        {
            // string sa = null;
            //string duplicateasset = "";
            //DataTable dtn = new DataTable();
            //List<AssetReportModel> FAList = new List<AssetReportModel>();
            //if (Session["faexcel"] != null)
            //{

            //    FAList = (List<AssetReportModel>)Session["faexcel"];

            //    dtn = (DataTable)Session["FAtable"];

            //}
            //string txt = string.Empty;

            //foreach (DataColumn column in dtn.Columns)
            //{
            //    //Add the Header row for Text file.
            //    txt += column.ColumnName + "\t\t";
            //}

            ////Add new line.
            //txt += "\r\n";

            //foreach (DataRow row in dtn.Rows)
            //{
            //    foreach (DataColumn column in dtn.Columns)
            //    {
            //        //Add the Data rows.
            //        txt += row[column.ColumnName].ToString() + "\t\t";
            //    }

            //    //Add new line.
            //    txt += "\r\n";
            //}

            ////Download the Text file.
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=SqlExport.txt");
            //Response.Charset = "";
            //Response.ContentType = "application/text";
            //Response.Output.Write(txt);
            //Response.Flush();
            //Response.End();
            List<AssetReportModel> FAList = new List<AssetReportModel>();
            FAList = (List<AssetReportModel>)Session["faexcel"];



            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            //dt.Columns.Add("ASSET GROUP ID");
            dt.Columns.Add("INWARD DATE");
            dt.Columns.Add("ACF_NUMBER");
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
            // dt.Columns.Add("QUANTITY");
            dt.Columns.Add("ASSET VALUE");
            // dt.Columns.Add("YEARS");
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
            dt.Columns.Add("TOTAL DEPR");
            dt.Columns.Add("WDV");
            dt.Columns.Add("TRANSFER FROM");
            dt.Columns.Add("TRANSFER VALUE");
            dt.Columns.Add("ASSET STATUS");
            dt.Columns.Add("Active Status");
            dt.Columns.Add("TRANSFER DATE");
            dt.Columns.Add("SALE DATE");
            dt.Columns.Add("DEPARTMENT");
            //dt.Columns.Add("SPOKE");
            dt.Columns.Add("ASSET BSCC");
            //dt.Columns.Add("UPLOAD BSCC");
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
            dt.Columns.Add("Taxable Amount");
            dt.Columns.Add("CGST Amount");
            dt.Columns.Add("SGST Amount");
            dt.Columns.Add("IGST Amount");
            dt.Columns.Add("CGCESS Amount");
            dt.Columns.Add("SGCESS Amount");
            dt.Columns.Add("IGCESS Amount");


            for (int i = 0; i < FAList.Count; i++)
            {

                //if (FAList[i].assetdetDetid.ToString() == null)
                //    FAList[i].assetdetDetid = "";
                dt.Rows.Add(i + 1,
                    // FAList[i].inwarddate.ToString(),
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
                    //  FAList[i].assetdetqty.ToString(),
                    FAList[i].assetdetAssetvalue.ToString(),
                    // FAList[i].Year.ToString(),
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
                    //FAList[i].assetdetspoke.ToString(),
                    FAList[i].bscc.ToString(),
                    //FAList[i].upbscc.ToString(),
                    FAList[i].ponumb.ToString(),
                    FAList[i].cbfnumb.ToString(),
                    FAList[i].vendornam.ToString(),
                    FAList[i].BRName.ToString(),
                    FAList[i].BRAddress.ToString(),
                    FAList[i].Branch_Status.ToString(),
                    FAList[i].SALE_VALUE.ToString(),
                    FAList[i].VAT_VALUE.ToString(),
                    FAList[i].NET_LOSS.ToString(),
                    FAList[i].Naration.ToString(),
                    FAList[i].taxable_amount.ToString(),
                    FAList[i].cgst_amount.ToString(),
                    FAList[i].sgst_amount.ToString(),
                    FAList[i].igst_amount.ToString(),
                    FAList[i].cgcess_amount.ToString(),
                    FAList[i].sgcess_amount.ToString(),
                    FAList[i].igcess_amount.ToString()
                    //FAList[i].goastatus.ToString()

                  );

            }

            DataTable _downloadingData = Session["faexcel"] as DataTable;
            //if (_downloadingData != null)
            string attachment = "attachment; filename=FAReport.xls";
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


                // DataTable _downloadingData = Session["FAtable"] as DataTable;
                //if (_downloadingData != null)
                //{



                //    using (XLWorkbook wb = new XLWorkbook())
                //    { 
                //        int i1 = 1;

                //        wb.Worksheets.Add(_downloadingData, "Report");    
                //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //        wb.Style.Font.Bold = true;

                //        Response.Clear();
                //        Response.Buffer = true;
                //        Response.Charset = "";
                //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //        Response.AddHeader("content-disposition", "attachment;filename= FA Advance Report.xlsx");

                //        using (MemoryStream MyMemoryStream = new MemoryStream())
                //        {
                //            wb.SaveAs(MyMemoryStream);
                //            MyMemoryStream.WriteTo(Response.OutputStream);
                //            Response.Flush();
                //            Response.End();
                //        }
                //    }
                //}
            }
            return RedirectToAction("FAreport");

        }

        [HttpPost]
        public JsonResult FAautobcode(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.FAAutobranch(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
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
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.Autoassetid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult WAAutoserialno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.WOAAutoserialno(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult TOAautoassetid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.TOAAutoassetid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult TAAutoserialno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.TOAAutoserialno(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult soaautoassetid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.SOAAutoassetid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult soaautoserialno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.SOAAutoserialno(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult soaautocheqno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.SOAAutocheqno(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult soaautoinvoiceno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.SOAAutoinvoiceno(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult ACCautoserialno(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.ACCAutoassetid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult ACCautooldassetcode(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.ACCAutoOassetcode(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult ACCautonewassetcode(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.ACCAutoNassetcode(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult ACCautoGrpid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.ACCAutoGrpid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult PHYautoassetid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.PHYAutoassetid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult PHYAutophyid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.Autophyid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        [HttpPost]
        public JsonResult PHYautoGrpid(string term)
        {
            try
            {
                List<AssetReportModel> autoloc = new List<AssetReportModel>();
                autoloc = Rr.PHYAutogrpid(term).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        //Asset Clearing Report
        [HttpGet]
        public ActionResult AssetClearing()
        {
            try
            {

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View();
        }
        [HttpGet]
        public ActionResult AssetClearingNew()
        {
            try
            {

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetAssetClearing()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = Rr.GetAssetClearingDetl();
                dt = ds.Tables[0];
                Session["AssetClearing"] = dt;
                if (dt.Rows.Count > 0)
                {
                    data0 = JsonConvert.SerializeObject(dt);
                }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportAssetClearing()
        {

            try
            {
                DataTable dt = Session["AssetClearing"] as DataTable;
                if (dt != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var workSheet = wb.Worksheets.Add(dt, "Report");
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = false;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename= Asset_Clearing_Account_Recon_Report.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return View();

        }

        //FA Summary Report
        [HttpGet]
        public ActionResult FASummary()
        {
            try
            {

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetFASummary()
        {
            string data0 = string.Empty, data1 = string.Empty, data2 = string.Empty, data3 = string.Empty, data4 = string.Empty, data5 = string.Empty, data6 = string.Empty, data7 = string.Empty, data8 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt0, dt1, dt2;
                ds = Rr.GetFASummaryDetail();
                dt0 = ds.Tables[0];
                dt0.Columns.Add("tb_g");
                dt0.Columns.Add("tb_d");
                dt0.Columns.Add("diff_g");
                dt0.Columns.Add("diff_d");
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                Session["d0"] = dt0;
                Session["d1"] = dt1;
                Session["d2"] = dt2;
                if (dt0.Rows.Count > 0)
                {
                    data0 = JsonConvert.SerializeObject(dt0);
                }
                if (dt1.Rows.Count > 0)
                {
                    data1 = JsonConvert.SerializeObject(dt1);
                }
                if (dt2.Rows.Count > 0)
                {
                    data2 = JsonConvert.SerializeObject(dt2);
                }
                return Json(new { data0, data1, data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2 }, JsonRequestBehavior.AllowGet);
            }
        }


        // [HttpPost]
        public ActionResult ExportFASummary(AssetReportModel model)
        {
            string data0 = string.Empty, data1 = string.Empty, data2 = string.Empty;
            try
            {
                //var datq = ViewData["TestPar"];
                DataModel mod = new DataModel();
                string finyer = mod.toGetFincialyear();
                string[] fin = finyer.Split('-');
                DataSet ds = new DataSet();
                DataTable dt0 = new DataTable("Overall"), dt1 = new DataTable("Branch"), dt2 = new DataTable("NonBranch"), dt = new DataTable("NonBranch");
                dt0 = Session["d0"] as DataTable;
                dt1 = Session["d1"] as DataTable;
                dt2 = Session["d2"] as DataTable;
                // model.ReportModel = Session["listget"] as List<AssetReportModel>;
                //dt0.Columns.Add("Particulars");
                //dt0.Columns.Add("Oracle GL");
                //dt0.Columns.Add("Opening");
                //dt0.Columns.Add("Additions");
                //dt0.Columns.Add("Deletions");
                //dt0.Columns.Add("Impairment");
                //dt0.Columns.Add("Closing");
                //dt0.Columns.Add("Oracle Dep GL");
                //dt0.Columns.Add("Dep Opening");
                //dt0.Columns.Add("Dep Additions");
                //dt0.Columns.Add("Dep Deletions");
                //dt0.Columns.Add("Dep Impairment");
                //dt0.Columns.Add("Dep Closing");
                //dt0.Columns.Add("AS on " + fin[0]);
                //dt0.Columns.Add("AS on " + fin[1]);
                //dt0.Columns.Add("tb Gross Block");
                //dt0.Columns.Add("tb Depreciation");
                //dt0.Columns.Add("Diff Gross Block");
                //dt0.Columns.Add("Diff Depreciation");
                //for (int i = 0; i < model.ReportModel.Count; i++)
                //{
                //    if (string.IsNullOrEmpty(model.ReportModel[i].OracleGL_g))
                //    {
                //        model.ReportModel[i].OracleGL_g = "-";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].OracleGL_d))
                //    {
                //        model.ReportModel[i].OracleGL_d = "-";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Opening_g))
                //    {
                //        model.ReportModel[i].Opening_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Opening_d))
                //    {
                //        model.ReportModel[i].Opening_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Additions_g))
                //    {
                //        model.ReportModel[i].Additions_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Additions_d))
                //    {
                //        model.ReportModel[i].Additions_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Deletions_d))
                //    {
                //        model.ReportModel[i].Deletions_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Deletions_g))
                //    {
                //        model.ReportModel[i].Deletions_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Impairment_g))
                //    {
                //        model.ReportModel[i].Impairment_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Impairment_d))
                //    {
                //        model.ReportModel[i].Impairment_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].diff_g))
                //    {
                //        model.ReportModel[i].diff_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].diff_d))
                //    {
                //        model.ReportModel[i].diff_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].tb_d))
                //    {
                //        model.ReportModel[i].tb_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].tb_g))
                //    {
                //        model.ReportModel[i].tb_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Closing_g))
                //    {
                //        model.ReportModel[i].Closing_g = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Closing_d))
                //    {
                //        model.ReportModel[i].Closing_d = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Opening_n))
                //    {
                //        model.ReportModel[i].Opening_n = "0";
                //    }
                //    if (string.IsNullOrEmpty(model.ReportModel[i].Closing_n))
                //    {
                //        model.ReportModel[i].Closing_n = "0";
                //    }

                //    dt0.Rows.Add(
                //            string.IsNullOrEmpty(model.ReportModel[i].Particulars.ToString()) ? " 0 " : model.ReportModel[i].Particulars.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].OracleGL_g.ToString()) ? " 0 " : model.ReportModel[i].OracleGL_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Opening_g.ToString()) ? " 0 " : model.ReportModel[i].Opening_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Additions_g.ToString()) ? " 0 " : model.ReportModel[i].Additions_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Deletions_g.ToString()) ? " 0 " : model.ReportModel[i].Deletions_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Impairment_g.ToString()) ? " 0 " : model.ReportModel[i].Impairment_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Closing_g.ToString()) ? " 0 " : model.ReportModel[i].Closing_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].OracleGL_d.ToString()) ? " 0 " : model.ReportModel[i].OracleGL_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Opening_d.ToString()) ? " 0 " : model.ReportModel[i].Opening_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Additions_d.ToString()) ? " 0 " : model.ReportModel[i].Additions_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Deletions_d.ToString()) ? " 0 " : model.ReportModel[i].Deletions_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Impairment_d.ToString()) ? " 0 " : model.ReportModel[i].Impairment_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Closing_d.ToString()) ? " 0 " : model.ReportModel[i].Closing_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Opening_n.ToString()) ? " 0 " : model.ReportModel[i].Opening_n.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].Closing_n.ToString()) ? " 0 " : model.ReportModel[i].Closing_n.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].tb_g.ToString()) ? " 0 " : model.ReportModel[i].tb_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].tb_d.ToString()) ? " 0 " : model.ReportModel[i].tb_d.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].diff_g.ToString()) ? " 0 " : model.ReportModel[i].diff_g.ToString(),
                //            string.IsNullOrEmpty(model.ReportModel[i].diff_d.ToString()) ? " 0 " : model.ReportModel[i].diff_d.ToString());
                //}

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workSheet = wb.Worksheets.Add(dt0, "Overall Summary");
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = false;
                    int dt0cnt = dt0.Rows.Count + 1;
                    for (int i = 2; i <= dt0cnt; i++)
                    {
                        workSheet.Cell("g" + i).FormulaA1 = "=c" + i + "+d" + i + "-e" + i + "-f" + i;
                        workSheet.Cell("m" + i).FormulaA1 = "=i" + i + "+j" + i + "-k" + i + "-l" + i;
                        workSheet.Cell("n" + i).FormulaA1 = "=c" + i + "-i" + i;
                        workSheet.Cell("o" + i).FormulaA1 = "=g" + i + "-m" + i;
                        workSheet.Cell("r" + i).FormulaA1 = "=n" + i + "-p" + i;
                        workSheet.Cell("s" + i).FormulaA1 = "=o" + i + "-q" + i;
                    }

                    var workSheet1 = wb.Worksheets.Add(dt1, "Branch");
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = false;
                    int dt1cnt = dt1.Rows.Count + 1;
                    //workSheet1.Range(1, 1, 1, 7).Merge().Value = "GROSS BLOCK";
                    //workSheet1.Range(1, 7, 1, 14).Merge().Value = "DEPRECIATION BLOCK";
                    //workSheet1.Range(1, 14, 1, 15).Merge().Value = "NET BLOCK";
                    for (int i = 2; i <= dt1cnt; i++)
                    {

                        workSheet1.Cell("g" + i).FormulaA1 = "=c" + i + "+d" + i + "-e" + i + "-f" + i;
                        workSheet1.Cell("m" + i).FormulaA1 = "=i" + i + "+j" + i + "-k" + i + "-l" + i;
                        workSheet1.Cell("n" + i).FormulaA1 = "=c" + i + "-i" + i;
                        workSheet1.Cell("o" + i).FormulaA1 = "=g" + i + "-m" + i;

                    }
                    var workSheet2 = wb.Worksheets.Add(dt2, "Non-Branch");
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = false;
                    int dt2cnt = dt2.Rows.Count + 1;
                    for (int i = 2; i <= dt2cnt; i++)
                    {
                        //workSheet1.Range(1, 1, 1, 7).Merge().Value = "GROSS BLOCK";
                        //workSheet1.Range(1, 7, 1, 14).Merge().Value = "DEPRECIATION BLOCK";
                        //workSheet1.Range(1, 14, 1, 15).Merge().Value = "NET BLOCK";
                        workSheet2.Cell("g" + i).FormulaA1 = "=c" + i + "+d" + i + "-e" + i + "-f" + i;
                        workSheet2.Cell("m" + i).FormulaA1 = "=i" + i + "+j" + i + "-k" + i + "-l" + i;
                        workSheet2.Cell("n" + i).FormulaA1 = "=c" + i + "-i" + i;
                        workSheet2.Cell("o" + i).FormulaA1 = "=g" + i + "-m" + i;

                    }
                    Response.Clear();
                    Response.Buffer = true;

                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=FA_Summary.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }


        public ActionResult DownloadExcel()
        {
            DataTable _downloadingData = Session["GLDownlinkFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workSheet = wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    for (int i = 1; i <= _downloadingData.Rows.Count; i++)
                    {
                        workSheet.Cell("g" + i).FormulaA1 = "=c" + i + "+d" + i + "-e" + i + "-f" + i;
                        workSheet.Cell("m" + i).FormulaA1 = "=i" + i + "+j" + i + "-k" + i + "-l" + i;
                        workSheet.Cell("n" + i).FormulaA1 = "=c" + i + "-i" + i;
                        workSheet.Cell("o" + i).FormulaA1 = "=g" + i + "-m" + i;
                        workSheet.Cell("r" + i).FormulaA1 = "=n" + i + "-p" + i;
                        workSheet.Cell("s" + i).FormulaA1 = "=o" + i + "-q" + i;
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= GLUpload.xlsx");

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
        [HttpGet]
        public JsonResult FinancialYearDrop()
        {
            PhysicalVerificationAsset Model = new PhysicalVerificationAsset();
            List<PhysicalVerificationAsset> list = new List<PhysicalVerificationAsset>();
            try
            {
                DataModel DM = new DataModel();
                string Currentyear = DM.toGetFincialyear();
                Model.FinancialYear = Currentyear;
                list.Add(Model);

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            } return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FARDetailed(string Command, string inwfd, string inwtd, string depfd, string deptd, string history, string bcode, string glcode)
        {
            AssetReportModel FAreport = new AssetReportModel();
            try
            {
                if (!string.IsNullOrEmpty(depfd) && !string.IsNullOrEmpty(deptd))
                {
                    if (depfd != "1" && deptd != "1")
                    {
                        Session["depfdtDetail"] = depfd;
                        Session["deptdtDetail"] = deptd;
                    }
                    else
                    {
                        depfd = "";
                        deptd = "";
                        Session["depfdtDetail"] = "";
                        Session["deptdtDetail"] = "";
                    }
                }
                else
                {
                    if (Convert.ToString(Session["depfdtDetail"]) != "" && Convert.ToString(Session["deptdtDetail"]) != "")
                    {
                        depfd = Session["depfdtDetail"].ToString();
                        deptd = Session["deptdtDetail"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(depfd) && !string.IsNullOrEmpty(deptd))
                {
                    Session["faexcelDetail"] = null;
                    DataTable Mfatable = new DataTable();
                    Mfatable = Rr.getfadepDetailed(depfd, deptd, history, bcode, glcode);
                    ViewBag.depfd = depfd;
                    ViewBag.deptd = deptd;
                    FAreport.ReportModel = Rr.getFATableDetailed(Mfatable).ToList();
                    Session["FAtableDetail"] = Mfatable;
                    if (!String.IsNullOrEmpty(inwfd) && !String.IsNullOrEmpty(inwtd))
                    {
                        ViewBag.inwfd = inwfd;
                        ViewBag.inwtd = inwtd;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => Convert.ToDateTime(inwfd) == null || Convert.ToDateTime(String.IsNullOrEmpty(x.inwarddate) ? "1990-01-01" : x.inwarddate.ToString()) >= Convert.ToDateTime(inwfd) && (Convert.ToDateTime(inwtd) == null || Convert.ToDateTime(String.IsNullOrEmpty(x.inwarddate) ? "1990-01-01" : x.inwarddate.ToString()) <= Convert.ToDateTime(inwtd))).ToList();
                    }
                    if (!String.IsNullOrEmpty(history))
                    {
                        ViewBag.history = history;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => history.ToUpper() == null || (x.assetdetgroupid.ToUpper().Contains(history.ToUpper()))).ToList();
                    }
                    if (!String.IsNullOrEmpty(bcode))
                    {
                        ViewBag.bcode = bcode;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => bcode.ToUpper() == null || (x.assetdetLocationcode.Equals(bcode.ToUpper()))).ToList();
                    }
                    if (!String.IsNullOrEmpty(glcode))
                    {
                        ViewBag.glcode = glcode;
                        FAreport.ReportModel = FAreport.ReportModel.Where(x => glcode.ToUpper() == null || (x.assetdetglcode.Equals(glcode.ToUpper()))).ToList();
                    }

                    Session["faexcelDetail"] = FAreport.ReportModel;
                    return View(FAreport);
                }
                else
                {
                    //Rr.GroupIDupdate();
                    Session["faexcelDetail"] = null;
                    FAreport.ReportModel = Enumerable.Empty<AssetReportModel>().ToList<AssetReportModel>();
                    if (Session["faexcelDetail"] != null)
                    {
                        FAreport.ReportModel = (List<AssetReportModel>)Session["faexcel"];
                    }
                    else
                    {
                        ViewBag.Message = "No Records Found";
                    }
                    return View(FAreport);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {

            }
        }

        public ActionResult FARDetailedExport()
        {
            List<AssetReportModel> FAList = new List<AssetReportModel>();
            // FAList = (List<AssetReportModel>)Session["faexcel"];
            FAList = (List<AssetReportModel>)Session["faexcelDetail"];
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("INWARD DATE");
            dt.Columns.Add("ACF_NUMBER");
            dt.Columns.Add("GROUP ID");
            dt.Columns.Add("ASSET_ID");
            dt.Columns.Add("BARCODE");
            dt.Columns.Add("ASSET SERIAL NO");
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
            dt.Columns.Add("TOTAL DEPR");
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
            for (int i = 0; i < FAList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    FAList[i].inwarddate.ToString(),
                    FAList[i].acfnumber.ToString(),
                    FAList[i].assetdetgroupid.ToString(),
                    FAList[i].assetdetDetid.ToString(),
                    FAList[i].barCode.ToString(),
                    FAList[i].assetSerialNo.ToString(),
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

            DataTable _downloadingData = Session["faexcel"] as DataTable;

            string attachment = "attachment; filename=FAReport.xls";
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
            return RedirectToAction("FAreport");

        }

        [HttpPost]
        public JsonResult SearchAssetClear(string DateFrom, string DateTo)
        {
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {

                DataTable dt = new DataTable();
                dt = Rr.SearchAssetClear(DateFrom, DateTo);
                Session["AssetClearing"] = dt;
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                //return Json(new { data1 }, JsonRequestBehavior.AllowGet);
                var jsonResult = Json(new { data1 }, JsonRequestBehavior.AllowGet);
                // return Json(Data2 , JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {

                ErrorMsg = "1";
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public ActionResult Batchtally()
        { return View(); }

       // [HttpPost]
        public ActionResult Batchtallyget(string frmdate)
        {
            ViewBag.fdate = frmdate;
            DataTable dt = new DataTable();
            dt = R.GetBatchtally(frmdate);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "BatchTally_Report");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Batchtally.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View();
               // return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ControllReport()
        {
            return View();
        }

        public ActionResult GetControllReport(string DateFrom, string todate)
        {
            ViewBag.fdate = DateFrom;
            ViewBag.tdate = todate;
            DataSet ds = R.TrantoOgl(DateFrom, todate);
            DataSet ds1 = R.BasetoTran(DateFrom, todate);
            System.Data.DataTable dt1 = new System.Data.DataTable();
            System.Data.DataTable dt2 = new System.Data.DataTable();
            System.Data.DataTable dt3 = new System.Data.DataTable();
            System.Data.DataTable dt4 = new System.Data.DataTable();
            System.Data.DataTable dt5 = new System.Data.DataTable();
            System.Data.DataTable dt6 = new System.Data.DataTable();
            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            dt3 = ds1.Tables[0];
            dt4 = ds1.Tables[1];
            dt5 = ds1.Tables[2];
            dt6 = ds1.Tables[3];
            int i = dt1.Rows.Count + 4;
            int j = dt3.Rows.Count + 4;
            int k = j + dt4.Rows.Count + 4;
            int l = k + dt5.Rows.Count + 4;
            int m = l + dt6.Rows.Count + 4;

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt1, "TRAN TO OGL");
                wb.Worksheet(1).Cell(i, 1).InsertTable(dt2);
                wb.Worksheets.Add(dt3, "BASET TO TRAN");
                wb.Worksheet(2).Cell(j, 1).InsertTable(dt4);
                wb.Worksheet(2).Cell(k, 1).InsertTable(dt5);
                wb.Worksheet(2).Cell(l, 1).InsertTable(dt6);

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ControllReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View();
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FAControlldetailREport()
        {
            return View();
        }

        public ActionResult FAOverallControlRptExport(string datefrom, string dateto)
        {
            DataTable dt = new DataTable();
            Ifams_RptDataModel export = new Ifams_RptDataModel();
            dt = export.FAExport(datefrom, dateto);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "FAControl_Report");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=FAControlRpt.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View("FAControlldetailREport");
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }


        #region //FAControl Report for Tran To Ogl
        public ActionResult FAControlRptTrantoOGL()
        {
            return View();
        }

        public ActionResult GetFAControlReport(string DateFrom, string todate)
        {
            ViewBag.fdate = DateFrom;
            ViewBag.tdate = todate;
            DataSet ds = R.FATrantoOgl(DateFrom, todate);
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt1 = ds.Tables[0];
            //  dt2 = ds.Tables[1];
            //  int i = dt1.Rows.Count + 4;

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt1, "FA TRAN TO OGL");
                // wb.Worksheet(1).Cell(i, 1).InsertTable(dt2);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=FAControllReport TrantoOGL.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View();
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion



        #region //FAControl Report for Tran To Ogl
        public ActionResult CentralECFRpt(string DateFrom = null, string todate = null, string dep = null, string command = null)
        {
            Ifams_RptEntity vrlist = new Ifams_RptEntity();
            //command = "Search";
            if (command == "Search")
            {
                ViewBag.fdate = DateFrom;
                ViewBag.tdate = todate;
                ViewBag.dept = dep;
                //Ifams_RptEntity vrlist = new Ifams_RptEntity();
                vrlist.RptModel = R.GetCTECFReport(DateFrom, todate, dep, objcmnr.GetLoginUserGid()).ToList();
            }
            else
            {
                ViewBag.fdate = "";
                ViewBag.tdate = "";
                ViewBag.dept = "";
                vrlist.RptModel = Enumerable.Empty<Ifams_RptEntity>().ToList<Ifams_RptEntity>();
                ViewBag.Message = "No Records Found";
            }
            return View(vrlist);
        }

        public ActionResult GetCentralECFReport(string DateFrom, string todate, string dep)
        {
            ViewBag.fdate = DateFrom;
            ViewBag.tdate = todate;
            ViewBag.dept = dep;
            DataSet ds = R.CentralECFReport(DateFrom, todate, dep, objcmnr.GetLoginUserGid());
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt1 = ds.Tables[0];
            //  dt2 = ds.Tables[1];
            //  int i = dt1.Rows.Count + 4;

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt1, "Central ECF");
                // wb.Worksheet(1).Cell(i, 1).InsertTable(dt2);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CentralECFReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View();
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CentralECFReport(string DateFrom, string todate, string dep)
        {
            ViewBag.fdate = DateFrom;
            ViewBag.tdate = todate;
            ViewBag.dept = dep;
            Ifams_RptEntity vrlist = new Ifams_RptEntity();
            vrlist.RptModel = R.GetCTECFReport(DateFrom, todate, dep, objcmnr.GetLoginUserGid()).ToList();
            return View("CentralECFRpt", vrlist);
        }

        [HttpPost]
        public JsonResult autodepartment(string term)
        {
            try
            {
                List<Ifams_RptEntity> autoloc = new List<Ifams_RptEntity>();
                autoloc = R.Autodepsummary(term, objcmnr.GetLoginUserGid()).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }
        #endregion

    }
}

