using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class SCNInwardController : Controller
    {
         ErrorLog objErrorLog = new ErrorLog();
        private WORepository objModel;
        public SCNInwardController()
            : this(new WODataModel())
        {

        }
        public SCNInwardController(WORepository objM)
        {
            objModel = objM;
        }

        //
        // GET: /FLEXIBUY/SCNInward/
        public ActionResult SummaryForSCNInward()
        {
            return View();
        }
        [HttpGet]
        public JsonResult SummaryForSCNInwardData()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetWOSummary();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index(string WOGid = null,string scngid = null,string viewfor = null)
        {
            ViewBag.viewfor = "0";
            if (WOGid != null)
                ViewBag.wogid = WOGid;
            else
                ViewBag.wogid = "0";
            if (scngid != null)
                ViewBag.scngid = scngid;
            else
                ViewBag.cbfdetailsgid = "0";
            if (viewfor == "view")
            {
                ViewBag.viewfor = "1";
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetSCNHeaderDetails(string wogid = "0")  
        {
            //string cbfdetid = "19";
            string data0 = string.Empty;
            string data1 = string.Empty;
            int woheadergid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid); 
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetSCNInwardDetails(woheadergid); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddInwardDetails(SCNInward objSCNInward)  
        {
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = objModel.AddInwardDetails(objSCNInward);
                if (ErrorMsg == "")
                {
                    ErrorMsg = "1";
                }
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSCNHeaderDetailsView(string scngid = "0")   
        {
            //string cbfdetid = "19";
            string data0 = string.Empty;
            string data1 = string.Empty;
            int scnheadergid = Convert.ToInt32(string.IsNullOrEmpty(scngid) ? "0" : scngid);  
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetSCNInwardDetailsView(scnheadergid);  
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
