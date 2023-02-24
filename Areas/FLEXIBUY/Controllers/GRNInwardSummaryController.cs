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
    public class GRNInwardSummaryController : Controller
    {
        //
        // GET: /FLEXIBUY/GRNInwardSummary/

        ErrorLog objErrorLog = new ErrorLog();
          private Irepositorypr objrep;
        public GRNInwardSummaryController()
            : this(new fb_DataModelpr())
        {

        }
        public GRNInwardSummaryController(Irepositorypr objM)
        {
            objrep = objM;
        }
        public ActionResult GRNInwardSummaryIndex() 
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
        public JsonResult GetGRNInward()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objrep.GetGRNSummaryNew();
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
    }
}
