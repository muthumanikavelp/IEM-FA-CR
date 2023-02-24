using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PhysicalPulloutController : Controller
    {

        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXISPEND/PhysicalPullout/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchPullout(Pullout pullout)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchPhysicalPullout(pl.ConvertDate(pullout.DateFrom == null ? "" : pullout.DateFrom), pl.ConvertDate(pullout.DateTo == null ? "" : pullout.DateTo), pullout.DocInwNo == null ? "" : pullout.DocInwNo, pullout.DocRefNo == null ? "" : pullout.DocRefNo, "", "0", "", pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetPullout(PPullout pPullout)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetPhysicalPullout(pPullout.PloId, pl.ConvertDate(pPullout.RequestDate), pPullout.RequestBy, pPullout.Reason, pl.ConvertDate(pPullout.PulloutDate), pPullout.InwardNo, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
    }
}
