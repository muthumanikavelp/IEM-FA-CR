using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PaymentReversalsController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        CmnFunctions cmnfunc = new CmnFunctions();
        #endregion

        //
        // GET: /FLEXISPEND/PaymentReversals/
        public ActionResult Maker()
        {
            return View();
        }

        //Get Payment Reversals Maker
        public JsonResult GetPaymentReversalsMaker(string PVDateFrom, string PVDateTo, string PVNo, string Status, string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPaymentReversals(pl.ConvertDate(PVDateFrom), pl.ConvertDate(PVDateTo), PVNo, Status, ViewType, pl.LoginUserId);
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
            catch { return null; }
        }

        //Set Payment Reversals Maker
        public JsonResult SetPaymentReversalsMaker(string PvId, string ReversalDate, string Reason)
        {
            try
            {
                DataTable dtErrmsg = new DataTable();
                dtErrmsg.Columns.Add("Message");
                dtErrmsg.Columns.Add("Clear");

                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("314");
                if (Maker == "Success")
                {

                    DataSet ds = db.SetPaymentReversals(PvId, pl.ConvertDate(ReversalDate), Reason, pl.LoginUserId);
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
                else
                {
                    DataRow dr = dtErrmsg.NewRow();
                    dr["Message"] = "Unauthorized User!";
                    dr["Clear"] = "0";
                    dtErrmsg.Rows.Add(dr);
                    Data1 = JsonConvert.SerializeObject(dtErrmsg);
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch { return null; }
        }
        //
        // GET: /FLEXISPEND/PaymentReversals/
        public ActionResult Checker()
        {
            return View();
        }

        //Get Payment Reversals Checker
        public JsonResult GetPaymentReversalsChecker(string PVDateFrom, string PVDateTo, string PVNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCheckerPaymentReversals(pl.ConvertDate(PVDateFrom), pl.ConvertDate(PVDateTo), PVNo, pl.LoginUserId);
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
            catch { return null; }
        }

        //Set Payment Reversals Checker
        public JsonResult SetPaymentReversalsChecker(string ReversalGId, string Status, string Remarks)
        {
            try
            {
                string Data1 = "";
                string checker = string.Empty;
                checker = cmnfunc.authorize("315");
                if (checker == "Success")
                {
                    DataSet ds = db.SetCheckerPaymentReversals(ReversalGId, Status, Remarks, pl.LoginUserId);
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
                else
                {
                    Data1 = "Unauthorized User!";
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch { return null; }
        }

    }
}
