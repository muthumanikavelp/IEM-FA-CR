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
    public class StatusController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/Status/
        public ActionResult Shop()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetShopStatus(string DateFrom, string DateTo)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetShopStatus(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    
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
        public JsonResult GetShopStatusSummary(string DateFrom, string DateTo, string Activity)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetShopStatusSummary(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), Activity, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
    }
}
