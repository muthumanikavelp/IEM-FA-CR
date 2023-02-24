using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM;
using System.Data;
using Newtonsoft.Json;
using IEM.Common;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.Dashboard.Controllers
{
    [NoDirectAccess]
    public class HistoryController : Controller
    {
        proLib plib = new proLib();
        dbLib db = new dbLib();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        DataTable dt = new DataTable();
        //
        // GET: /Dashboard/History/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Dashboard/History/ASMS
        public ActionResult ASMS()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetASMSHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetASMSHistory(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), RequestType, RequestStatus, objCmnFunctions.GetLoginUserGid().ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Dashboard/History/Eprocure
        public ActionResult EProcure()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetEProcureHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetEProcureHistory(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), RequestType, RequestStatus, objCmnFunctions.GetLoginUserGid().ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Dashboard/History/EClaim
        public ActionResult EClaim()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetEClaimHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetEClaimHistory(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), RequestType, RequestStatus, objCmnFunctions.GetLoginUserGid().ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
    }
}
