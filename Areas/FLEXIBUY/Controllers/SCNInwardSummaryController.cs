using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using IEM.Areas.FLEXIBUY.Models;
using System.Text;
using IEM.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class SCNInwardSummaryController : Controller
    {
        //
        // GET: /FLEXIBUY/SCNInwardSummary/
          ErrorLog objErrorLog = new ErrorLog();
          private Irepositorypr objrep;
        public SCNInwardSummaryController()
            : this(new fb_DataModelpr())
        {

        }
        public SCNInwardSummaryController(Irepositorypr objM)
        {
            objrep = objM;
        }
        public ActionResult SCNInwardSummaryIndex()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        public JsonResult GetSCNInward()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objrep.GetSCNSummaryNew();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SCNInwardSummaryWO()
        {
            List<GRNInward> obj = new List<GRNInward>();
            try
            {
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        public JsonResult GetSCNInwardWO()
        {
            List<GRNInward> obj = new List<GRNInward>();
            try
            {
                obj = objrep.GetSelectionForWo();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
