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
using IEM.Areas.MASTERS.Controllers;
using IEM.Areas.MASTERS.Models;
using IEM.Areas.FLEXISPEND.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PaymentRunController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        ForMailController mail = new ForMailController();
        #endregion

        //
        // GET: /FLEXISPEND/PaymentRun/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchPaymentRun(string Priorit, string DocNo, string DocFrom, string DocTo,
                            string SuppCodeId, string SuppNameId, string RaiserCodeId, string RaiserNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPaymentRun(Priorit, DocNo, plib.ConvertDate(DocFrom), plib.ConvertDate(DocTo), SuppCodeId, SuppNameId, RaiserCodeId, RaiserNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }


                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //Save Payment Run
        public JsonResult SetPaymentRun(string ecfId)
        {
            string Data1 = "", Data2 = "", Data3 = "";
            string[] data = { };
            DataSet ds = db.SetPaymentRun(ecfId, plib.LoginUserId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                //to send mail
                if(ds.Tables[0].Rows[0]["Clear"].ToString() == "True")
                {
                    var ecf = ecfId.Split('|');
                    for (var i = 0; i < ecf.Length - 1; i++)
                    {
                        DataSet PrepaidValidationCount = db.IsPrepaidLiquidation(ecf[i]);
                        if (PrepaidValidationCount.Tables[0].Rows[0]["ECFCount"].ToString() != "0")
                        {
                            DataSet claimType = db.GetSupplierOREmployeeClaimType(ecf[i]);
                            if (claimType.Tables[0].Rows[0]["ecf_supplier_employee"].ToString() == "S")
                            {
                                mail.sendusermailfs("FS", "Prepaid Adjustment - Vendor Claim", ecf[i], "S", data, "0");
                            }
                            else
                            {
                                mail.sendusermailfs("FS", "Prepaid Adjustment - Employee Claim", ecf[i], "S", data, "0");
                            }
                        }
                    }        
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        //Save Payment Run Dedup
        public JsonResult PaymentRedupRun(string ecfId)
        {
            string Data1 = "", Data2 = "";
            DataSet ds = db.SetPaymentRunDedup(ecfId, plib.LoginUserId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        //Maintain dedup details
        public JsonResult tmpDedupKeep(string ecfId)
        {
            string msg = "";
            Session["PRDedup"] = null;
            DataSet ds = db.SetPaymentRunDedup(ecfId, plib.LoginUserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    DataTable _tmpdata = ds.Tables[2];
                    _tmpdata.TableName="Payment Run Dedup details";
                    Session["PRDedup"] = _tmpdata;
                }
                else
                {
                    msg = "problem on downloading..please try again";
                }
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }

        //Download the dedup details
        public JsonResult DownloadDedupData()
        {
            DataTable _downloadingData = Session["PRDedup"] as DataTable;
            if (_downloadingData != null)
            {
                _downloadingData.TableName = "Payment Dedup Details";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Payment Run Dedup Details.xlsx");

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

        //Authorization Reverse
        public JsonResult PaymentAuthorizationReverse(string EcfId, string Reason)
        {
            string Data1 = "";
            DataSet ds = db.SetPaymentRunAutReverse(EcfId, Reason, plib.LoginUserId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        //Authorization Reverse with GL
        public JsonResult PaymentAuthorizationReverseWithGL(string EcfId, string Reason)
        {
            string Data1 = "";
            DataSet ds = db.SetPaymentRunAutReverseWithGL(EcfId, Reason, plib.LoginUserId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

    }
}
