using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.EOW.Models;
using System.Data;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class RetentionInvoiceReportController : Controller
    {
        //
        // GET: /EOW/RetentionInvoiceReport/
        private RetentionInvoiceReportRepository retentioninvoice;
        CmnFunctions com = new CmnFunctions();
        public RetentionInvoiceReportController()
            : this(new RetentionInvoiceReport())
        {

        }
        public RetentionInvoiceReportController(RetentionInvoiceReportRepository objM)
        {
            retentioninvoice = objM;
        }
        public ActionResult Index()
        {
            List<EOW_RetentionInvoiceReport> list = new List<EOW_RetentionInvoiceReport>();            
            list = retentioninvoice.RetentioinInvoiceReport().ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Index(string datefrom, string dateto, string suppliercode, string suppliername, string invoiceno, string ecfno, string command = null)
        {
            List<EOW_RetentionInvoiceReport> list = new List<EOW_RetentionInvoiceReport>();
            EOW_RetentionInvoiceReport retentionreleasereport = new EOW_RetentionInvoiceReport();
            if (command == "Search" || command == "Refresh")
            {
                list = retentioninvoice.RetentioinInvoiceReport(datefrom, dateto, suppliercode, suppliername, invoiceno, ecfno).ToList();
                @ViewBag.datefrom = datefrom;
                @ViewBag.dateto = dateto;
                @ViewBag.suppliercode = suppliercode;
                @ViewBag.suppliername = suppliername;
                @ViewBag.invoiceno = invoiceno;
                @ViewBag.ecfno = ecfno;
                if (list.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                }
            }
            if (command == "Clear")
            {
                list = retentioninvoice.RetentioinInvoiceReport().ToList();
            }
            return View(list);
        }
        //
        // GET: /EOW/RetentionInvoiceReport/Details/5

        
    }
}
