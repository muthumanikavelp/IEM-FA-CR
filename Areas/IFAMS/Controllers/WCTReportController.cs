using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.Globalization;


namespace IEM.Areas.IFAMS.Controllers
{
    public class WCTReportController : Controller
    {
        private sectorrep objrep;
        private CmnFunctions objcmnr = new CmnFunctions();
        ErrorLog objerrlog = new ErrorLog();
        DataTable dt = new DataTable();

        public WCTReportController()
            : this(new sectormodel())
        {
        }
        public WCTReportController(sectorrep objRModel)
        {
            objrep = objRModel;

        }

        [HttpGet]
        public ActionResult WCTreports(string depfd, string deptd)
        {
            wctreport wtcreps = new wctreport();
            //List<wctreport> tdsreports = new List<wctreport>();
            if (Session["wctpaging"] == "" || Session["wctpaging"] == null)
            {
                wtcreps.wctrep = Enumerable.Empty<wctreport>().ToList<wctreport>();
                ViewBag.Message = "No Records Found";
            }
            else
            {

                wtcreps.wctrep = Session["wctpaging"] as List<wctreport>;
                //wtcreps = (wctreport)Session["wctexcel"];
                // wtcreps.wctrep = objrep.getwctrep(depfd, deptd).ToList();
            }
            return View(wtcreps);
        }



        [HttpPost]
        public ActionResult WCTreports(string depfd, string deptd, string command)
        {
            wctreport wtcreps = new wctreport();

            try
            {
                ViewBag.depfd = depfd;
                ViewBag.deptd = deptd;
                string sdate = "";
                string tdate = "";
                if (command != "Clear")
                {
                    DateTime datefrom = Convert.ToDateTime(depfd.ToString());
                    sdate = datefrom.ToString("yyyy-MM-dd");
                    DateTime dateto = Convert.ToDateTime(deptd.ToString());
                    tdate = dateto.ToString("yyyy-MM-dd");
                }

                else
                {
                    sdate = "";
                    tdate = "";
                }
                wtcreps.wctrep = objrep.getwctrep(sdate, tdate).ToList();
                Session["wctpaging"] = objrep.getwctrep(sdate, tdate).ToList();
                if (wtcreps.wctrep.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                    Session["wctpaging"] = null;
                }
            }
            catch
            {
            }
            return View(wtcreps);
        }

        public ActionResult WCTExport()
        {
            DataTable dt = (DataTable)Session["wctexcel"];
            string attachment = "attachment; filename=WCTReport.xls";
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
            return RedirectToAction("WCTreports");
        }

        [HttpGet]
        public ActionResult Servicereports()
        {
            servicetareport servicereps = new servicetareport();
            //List<wctreport> tdsreports = new List<wctreport>();

            // servicereps.stax  = Enumerable.Empty<servicetareport>().ToList<servicetareport>();
            var list = Session["serrptpaging"] as List<servicetareport>;
            if (list == null)
            {
                servicereps.stax = Enumerable.Empty<servicetareport>().ToList<servicetareport>();
                ViewBag.Message = "No Records Found";
            }
            else
            {
                servicereps.stax = (List<servicetareport>)Session["serrptpaging"];
            }
            return View(servicereps);
        }

        [HttpPost]
        public ActionResult Servicereports(string depfd, string deptd, string command)
        {
            servicetareport servicereps = new servicetareport();
            try
            {
                ViewBag.depfd = depfd;
                ViewBag.deptd = deptd;
                string sdate = "";
                string tdate = "";
                if (command != "Clear")
                {
                    DateTime datefrom = Convert.ToDateTime(depfd.ToString());
                    sdate = datefrom.ToString("yyyy-MM-dd");
                    DateTime dateto = Convert.ToDateTime(deptd.ToString());
                    tdate = dateto.ToString("yyyy-MM-dd");
                }

                else
                {
                    sdate = "";
                    tdate = "";
                }
                servicereps.stax = objrep.getservicerep(sdate, tdate).ToList();
                Session["serrptpaging"] = objrep.getservicerep(sdate, tdate).ToList();
                if (servicereps.stax.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                    Session["serrptpaging"] = null;
                }
            }
            catch
            {
            }
            return View(servicereps);
        }

        public ActionResult STaxExport()
        {
            DataTable dt = (DataTable)Session["serviceexcel"];
            string attachment = "attachment; filename=STAXReport.xls";
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
            return RedirectToAction("Servicereports");
        }

        [HttpGet]
        public ActionResult inwardreports()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [HttpGet]
        public ActionResult VALreports()
        {
            valreport valreps = new valreport();

            if (Session["valpaging"] == "" || Session["valpaging"] == null)
            {

                //List<wctreport> tdsreports = new List<wctreport>();

                valreps.valrep = Enumerable.Empty<valreport>().ToList<valreport>();
                ViewBag.Message = "No Records Found";
            }
            else
            {
                valreps.valrep = Session["valpaging"] as List<valreport>;
            }

            return View(valreps);
        }

        [HttpPost]
        public ActionResult VALreports(string depfd, string deptd, string command)
        {
            valreport valreps = new valreport();
            try
            {

                ViewBag.depfd = depfd;
                ViewBag.deptd = deptd;
                string sdate = "";
                string tdate = "";
                if (command != "Clear")
                {
                    DateTime datefrom = Convert.ToDateTime(depfd.ToString());
                    sdate = datefrom.ToString("yyyy-MM-dd");
                    DateTime dateto = Convert.ToDateTime(deptd.ToString());
                    tdate = dateto.ToString("yyyy-MM-dd");
                }

                else
                {
                    sdate = "";
                    tdate = "";
                }

                valreps.valrep = objrep.getvalrep(sdate, tdate).ToList();
                Session["valpaging"] = objrep.getvalrep(sdate, tdate).ToList();
                if (valreps.valrep.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                    Session["valpaging"] = null;
                }
            }
            catch
            {

            }


            return View(valreps);
        }


        public ActionResult VALExport()
        {
            DataTable dt = (DataTable)Session["valexcel"];
            string attachment = "attachment; filename=VALReport.xls";
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
            return RedirectToAction("VALreports");
        }
    }
}
