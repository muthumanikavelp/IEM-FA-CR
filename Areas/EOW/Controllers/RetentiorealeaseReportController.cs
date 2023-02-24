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
    public class RetentiorealeaseReportController : Controller
    {
        //
        // GET: /EOW/RetentiorealeaseReport/
         private RetentiorealeaseReportepository retentionrelease;
        CmnFunctions com = new CmnFunctions();
        public RetentiorealeaseReportController()
            : this(new RetentiorealeaseReport())
        {

        }
        public RetentiorealeaseReportController(RetentiorealeaseReportepository objM)
        {
            retentionrelease = objM;
        }
        public ActionResult Index()
        {
            List<Eow_RetentionRealeaseReport> list = new List<Eow_RetentionRealeaseReport>();
            RetentiorealeaseReport retentionreleasereport = new RetentiorealeaseReport();
            @ViewBag.Option = "Amount Released";
            list = retentionreleasereport.RetentioRealeaseReport().ToList();
            return View(list);
        }
       
        
         [HttpPost]
        public ActionResult Index(string ReportMode, string datefrom, string dateto, string suppliercode, string suppliername, string invoiceno, string ecfno, string command = null)
        {   
            List<Eow_RetentionRealeaseReport> list = new List<Eow_RetentionRealeaseReport>();
            RetentiorealeaseReport retentionreleasereport = new RetentiorealeaseReport();
            if(command=="Search" || command=="Refresh")
            {
               
                @ViewBag.Option = "Amount Released";
                list = retentionreleasereport.RetentioRealeaseReport(datefrom, dateto, suppliercode, suppliername, invoiceno, ecfno).ToList();
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
                //}
               
            }
            if (command == "Clear")
            {
                @ViewBag.Option = "Amount Released";
                list = retentionreleasereport.RetentioRealeaseReport().ToList();
            }
            return View(list);
        }
         public PartialViewResult ReportView(int id)
         {
             List<Eow_RetentionRealeaseReport> listview = new List<Eow_RetentionRealeaseReport>();
             
             listview = retentionrelease.RetentioRealeaseView(id).ToList();
             return PartialView(listview);
         }
    }
}
