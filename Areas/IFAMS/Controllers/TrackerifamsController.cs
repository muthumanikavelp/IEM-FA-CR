using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using IEM.Models;
using IEM.Areas.IFAMS.Models;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{
      [NoDirectAccess]
    public class TrackerifamsController : Controller
    {

        private IfamsQueryRepository ObjQuery;
        ErrorLog objErrorLog = new ErrorLog();

        public TrackerifamsController()
            : this(new IfamsQueryDataModel())
        {

        }
        public TrackerifamsController(IfamsQueryRepository Objist)
        {
            ObjQuery = Objist;
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
                ds = ObjQuery.GetTrackerScreenData();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                var js = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                js.MaxJsonLength = 2147483647;
                return js;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
