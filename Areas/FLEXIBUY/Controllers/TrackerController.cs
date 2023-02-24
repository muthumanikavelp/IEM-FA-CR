using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using Newtonsoft.Json;
using System.Data;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class TrackerController : Controller
    {
        //
        // GET: /FLEXIBUY/Tracker/
         ErrorLog objErrorLog = new ErrorLog();
         private TrackerRepository objRep;
        public TrackerController()
            : this(new TrackerDataModel())
        {

        }
        public TrackerController(TrackerRepository objModel)
        {
            objRep = objModel;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTrackerScreenData()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetTrackerScreenData();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                var js= Json(new { data0 }, JsonRequestBehavior.AllowGet);
                js.MaxJsonLength = 2147483647;
                return js;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult POSummary(string docgid = null,string frompage = null)  
        {
            if (docgid != null)
            {
                ViewBag.docgid = docgid;
            }
            if (frompage != null)
            {
                ViewBag.frompage = frompage;
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetTrackerScreenDataPO(string docgid = null) 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetTrackerScreenDataPO(docgid); 
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
     
        public ActionResult GRNSummary(string docgid = null, string frompage = null) 
        {
            if (docgid != null)
            {
                ViewBag.docgid = docgid;
            }
            if (frompage != null)
            {
                ViewBag.frompage = frompage;
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetTrackerScreenDataGRN(string docgid = null) 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetTrackerScreenDataGRN(docgid);
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
        [HttpPost]
        public JsonResult GetTrackerGRNConfirm(string docgid = null)  
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetTrackerGRNConfirm(docgid);
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
        public ActionResult CBFSummary(string docgid = null,string doctype = null, string frompage = null) 
        {
            if (docgid != null)
            {
                ViewBag.docgid = docgid;
            }
            if (docgid != null)
            {
                ViewBag.doctype = doctype;
            }
            if (frompage != null)
            {
                ViewBag.frompage = frompage;
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetTrackerScreenDataCBF(string docgid = null, string doctype = null) 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetTrackerScreenDataCBF(docgid,doctype);
               
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
    }
}
