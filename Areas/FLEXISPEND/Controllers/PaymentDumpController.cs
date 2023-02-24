using ClosedXML.Excel;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PaymentDumpController : Controller
    {
        #region Decalartion
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/PaymentDump/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPaymentDump(string PaymentBatchNo, string PaymentDate)
        {
            try
            {
                Session["PDumpDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.GetPaymentDump(PaymentBatchNo, plib.ConvertDate(PaymentDate), plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);

                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.TableName = "Payment Dump";
                            Session["PDumpDownlinkFile"] = _tmpdata;
                        }
                    }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult IsDownloadAvailable()
        {
            try
            {   
                DataTable _downloadingData = Session["PDumpDownlinkFile"] as DataTable;
                if (_downloadingData != null)
                    return Json("OK", JsonRequestBehavior.AllowGet);
                else
                    return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //downloading Excel File
        public JsonResult DownloadExcel()
        {
            DataTable _downloadingData = Session["PDumpDownlinkFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Payment Dump.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
