using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using System.Data;
using System.Data.Sql;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class FlexiQueryReportController : Controller
    {
        private IQueryReport Res;

        public FlexiQueryReportController()
            : this(new DALQueryReport())
        {

        }

        public FlexiQueryReportController(IQueryReport objlist)
        {

            Res = objlist;
        }


        [HttpGet]
        public ActionResult Index()
        {
            List<EFlexiQuery> records = new List<EFlexiQuery>();
            EFlexiQuery obj = new EFlexiQuery();
            records.Clear();

            records = Res.GetFlexiQuery().ToList();
            obj.DocList = new SelectList(Res.GetDocList(), "docname", "docname");
            ViewBag.DoctypeList = obj;
            if (records.Count == 0)
            {
                ViewBag.query = "No Records Found";
            }

            return View(records);
        }


        [HttpPost]
        public ActionResult Index(string FromDate = null, string ToDate = null, string RefNo = null, string DocList = null, string command = null)
        {
            List<EFlexiQuery> records = new List<EFlexiQuery>();
            EFlexiQuery obj = new EFlexiQuery();
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = Res.GetFlexiQuery_List(FromDate, ToDate, RefNo, DocList).ToList();
                //@ViewBag.filter = pincode;
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                ViewBag.fdate = FromDate;
                ViewBag.tdate = ToDate;
                ViewBag.Refno = RefNo;
                ViewBag.DocList = DocList;
            }
            else if (command == "Clear")
            {
                records = Res.GetFlexiQuery().ToList();
                ViewBag.fdate = "";
                ViewBag.tdate = "";
                ViewBag.Refno = "";
                 
            }

           

            obj.DocList = new SelectList(Res.GetDocList(), "docname", "docname");
            ViewBag.DoctypeList = obj;
            return View(records);
        }
         
        
        public ActionResult GetFlexiQueryReportexport(string datefrom, string dateto, string RefNo, string DocList)
        {
             
       
              DataTable dt1 = new DataTable();
              dt1 = Res.GetQueryExport(datefrom, dateto, RefNo, DocList);
            
             
            //  dt2 = ds.Tables[1];
            //  int i = dt1.Rows.Count + 4;

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt1, "FlexiQueryReport");
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




         
    }
}
