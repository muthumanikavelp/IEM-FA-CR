using IEM.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class AmortController : Controller
    {
        #region Global Variables
        proLib plib = new proLib();
        dbLib db = new dbLib();
        Message msg = new Message();
        DataTable dt;
        #endregion

        #region AmortRun
        // Load the amort run page.
        public ActionResult Index()
        {
            return View();
        }

        // Get the amort search details based on the parameter passed based on the UI and returns the Dataset.
        public JsonResult AmortRunSearchDetails(string Month, string SupplierCodeId, string SupplierNameId, string AmortAmount, string ECFFrom, string ECFTo, string ECFNumber, string ECFAmount)
        {
            try
            {
                string strMonth = "0";
                string strYear = "0";
                if (Month != "")
                {
                    string[] strSplitMonth = Month.Split('-');
                    strMonth = plib.GetMonth(strSplitMonth[0]);
                    strYear = strSplitMonth[1];
                }

                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortRunSearchDetails(plib.ConvertDate(ECFFrom), plib.ConvertDate(ECFTo), ECFNumber, ECFAmount == "" ? "0" : ECFAmount, AmortAmount == "" ? "0" : AmortAmount, SupplierCodeId == "0" ? "" : SupplierCodeId, SupplierNameId == "0" ? "" : SupplierNameId, strMonth, strYear, plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult SaveAmortRunDetails(string Month, string AmortData)
        {
            string strMonth = "0";
            string strYear = "0";
            if (Month != "")
            {
                string[] strSplitMonth = Month.Split('-');
                strMonth = plib.GetMonth(strSplitMonth[0]);
                strYear = strSplitMonth[1];
            }

            DataSet ds = db.SetAmortRunDetails(AmortData, strMonth, strYear, plib.LoginUserId);
            if (ds != null)
            {
                msg.Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();
                msg.MessageText = ds.Tables[0].Rows[0]["Msg"].ToString();
                if (msg.Clear == "1" || msg.Clear == "true")
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Amort Edit
        // Load the Amort Edit Page 
        public ActionResult AmortEdit()
        {
            return View();
        }

        // Get the Search Details and bind the gird.
        public JsonResult AmortSearchDetails(string ECFFrom, string ECFTo, string ECFNo, string ECFAmount, string SupplierCodeId, string SupplierNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortDetails(plib.ConvertDate(ECFFrom), plib.ConvertDate(ECFTo), ECFNo, ECFAmount == "" ? "0" : ECFAmount, SupplierCodeId == "0" ? "" : SupplierCodeId, SupplierNameId == "0" ? "" : SupplierNameId, plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Amort Debit line Details.
        public JsonResult AmortDebitLineDetails(string ECFFrom, string ECFTo, string ECFNo, string ECFAmount, string SupplierCodeId, string SupplierNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortDetails(plib.ConvertDate(ECFFrom), plib.ConvertDate(ECFTo), ECFNo, ECFAmount == "" ? "0" : ECFAmount, SupplierCodeId == "0" ? "" : SupplierCodeId, SupplierNameId == "0" ? "" : SupplierNameId, plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        // Get the Search Details and bind the gird.
        public JsonResult AmortSplitDetails(string startdate, string enddate, string frequencygid)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetAmortSplitDetails(plib.ConvertDate(startdate), plib.ConvertDate(enddate), frequencygid, plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        // Amort Reschedule Details.
        public JsonResult AmortReScheduleDetails(string InvId, string Amount, string AmortCycle, string AmortStartDate, string AmortEndDate, string AmortSplit)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortReScheduleDetails(InvId, Amount, AmortCycle, plib.ConvertDate(AmortStartDate), plib.ConvertDate(AmortEndDate), AmortSplit, plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                if (ds.Tables[1].Rows.Count == 0)
                {
                    DataTable dts = new DataTable();
                    dts.Columns.Add("Msg");
                    dts.Rows.Add(new object[] { plib.NoRecordFound });

                    if (dts.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dts); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }

        // Save the Amort edit details.        
        public JsonResult SaveAmortEditDetails(string AmortId, string ECFId, string InvId, string Amount, string CurrentAmount, string AmortCycle, string AmortStartDate, string AmortEndDate, string AmortSplit, string SupplierId, string DebitlinePercent)
        {
            DataTable _tmpdt = new DataTable();
            try
            {   
                _tmpdt.Columns.Add("GId", typeof(string));
                _tmpdt.Columns.Add("DPercent", typeof(string));

                DataTable dtDebitlinePercent = null;
                dtDebitlinePercent = (DataTable)JsonConvert.DeserializeObject(DebitlinePercent, (typeof(DataTable)));

                foreach (DataRow row in dtDebitlinePercent.Rows)
                {
                    _tmpdt.Rows.Add(row["GId"].ToString(), row["DPercent"].ToString());
                }
            }
            catch
            {
                _tmpdt = null;
            }

            DataSet ds = db.SaveAmortDetailsBasedOnReSchedule(AmortId, ECFId, InvId, Amount, CurrentAmount, AmortCycle, plib.ConvertDate(AmortStartDate), plib.ConvertDate(AmortEndDate), AmortSplit, SupplierId, _tmpdt, plib.LoginUserId);
            if (ds != null)
            {
                msg.Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();
                msg.MessageText = ds.Tables[0].Rows[0]["Msg"].ToString();
                if (msg.Clear == "1" || msg.Clear == "true")
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }

    public class _temp
    {
        public string GId { get; set; }
        public string Slno { get; set; }
    }
}
