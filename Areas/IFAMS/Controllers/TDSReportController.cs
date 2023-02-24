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
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{
      [NoDirectAccess]
    public class TDSReportController : Controller
    {
        //
        // GET: /IFAMS/TDSReport/

        private sectorrep objrep;
        private CmnFunctions objcmnr = new CmnFunctions();
        ErrorLog objerrlog = new ErrorLog();
        DataTable dt = new DataTable();


        public TDSReportController()
            : this(new sectormodel())
        {
        }
        public TDSReportController(sectorrep objRModel)
        {
            objrep = objRModel;
        }
        [HttpPost]

        public ActionResult Index(string depfd, string deptd, string command)
        {
            tdsreport tdsreps = new tdsreport();
            List<tdsreport> tdsreports = new List<tdsreport>();
            string sdate = "";
            string tdate = "";
            try
            {
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
                Session["tdsrptpaging"] = objrep.Gettdsreport(sdate, tdate).ToList();
                tdsreps.tdsrep = objrep.Gettdsreport(sdate, tdate).ToList();
                
                if (tdsreps.tdsrep.Count == 0 || command == "Clear")
                {
                    Session["tdsrptpaging"] = null;
                    ViewBag.message = "No Records Found";
                    ViewBag.alert = "No Records";
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(tdsreps);
        }


        public ActionResult Index(string depfd, string deptd)
        {
            tdsreport tdsreps = new tdsreport();

            var list = Session["tdsrptpaging"] as List<tdsreport>;
            //tdsreps.tdsrep = Enumerable.Empty<tdsreport>().ToList<tdsreport>();
            if (list == null)
            {
                tdsreps.tdsrep = Enumerable.Empty<tdsreport>().ToList<tdsreport>();
                ViewBag.Message = "No Records Found";
            }
            else
            {
                tdsreps.tdsrep = (List<tdsreport>)Session["tdsrptpaging"];
            }

           
            if (tdsreps.tdsrep.Count == 0 )
            {
                ViewBag.message = "No Records Found";
                ViewBag.alert = "No Records";
            }
            return View(tdsreps);

        }

        public ActionResult TDSExport()
        {
            DataTable dt = (DataTable)Session["tdsexcel"];
            string attachment = "attachment; filename=TDSReport.xls";
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
            return RedirectToAction("TDSreports");
        }

    }
}
