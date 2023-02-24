using ClosedXML.Excel;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CBFUtilizationSummaryController : Controller
    {

        //
        // GET: /FLEXIBUY/CBFUtilizationSummary/
        private IRepositoryKIR objrep;

        public CBFUtilizationSummaryController()
            : this(new prsummodel())
        { }
        public CBFUtilizationSummaryController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(string command, string CBFDateFrom, string CBFDateTo)
        {
            if (command == "CBFSearch")
            {
                ViewBag.CBFDateFrom = CBFDateFrom;
                ViewBag.CBFDateTo = CBFDateTo;
            }
            return View();
        }
 
        public ActionResult downloadexcelold(string CBFDateFrom = null, string CBFDateTo = null)
        {


            DataTable dtnew = new DataTable();
            if (!string.IsNullOrEmpty(CBFDateFrom) && !string.IsNullOrEmpty(CBFDateTo))
            {
                //dtnew = objrep.GetCBFUtilReports(CBFDateFrom, CBFDateTo);
            }
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CBF Summary.xls"));
                Response.ContentType = "application/vnd.ms-excel";
                DataTable dt = dtnew;

                string str = string.Empty;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write(str + dtcol.ColumnName);
                    str = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string content = dr[j].ToString();
                        content.Replace(System.Environment.NewLine, " ");
                        Response.Write(str + Convert.ToString(dr[j]));
                        str = "\t";
                    }

                    Response.Write("\n");
                    //GGG
                }
                Response.End();
            }
            return View();
        }

        public ActionResult DownloadExcel(string CBFDateFrom, string CBFDateTo, string CBFNO, string PONO)
        {
            try
            {
                DataTable dt = new DataTable("Table1");
                if (!string.IsNullOrEmpty(CBFDateFrom) && !string.IsNullOrEmpty(CBFDateTo))
                {
                    DataSet ds = new DataSet();
                    ds = objrep.GetCBFUtilReports(CBFDateFrom, CBFDateTo, CBFNO, PONO);
                    //ds = objrep.GetCBFUtilReports(CBFDateFrom, CBFDateTo);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt);
                                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                wb.Style.Font.Bold = true;

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename= CBF Summary.xlsx");
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
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            //return Json("", JsonRequestBehavior.AllowGet);
            // return View("~/Areas/Flexibuy/Views/CBFUtilizationSummary/Index.cshtml");
            return View();

        }

    }
}
