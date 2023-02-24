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
    public class PhysicalPulloutReceiptingController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        FSRepository _fsRep = new FSRepository();
        #endregion

        //
        // GET: /FLEXISPEND/PhysicalPulloutReceipting/

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
                DataSet ds = db.FetchPhysicalPullout(pl.ConvertDate(pullout.DateFrom == null ? "" : pullout.DateFrom), pl.ConvertDate(pullout.DateTo == null ? "" : pullout.DateTo), pullout.DocInwNo == null ? "" : pullout.DocInwNo, pullout.DocRefNo == null ? "" : pullout.DocRefNo, pullout.DocType == null ? "" : pullout.DocType, pullout.DocAmount == null || pullout.DocAmount == "" ? "0" : pullout.DocAmount, pullout.ReqBy == null ? "" : pullout.ReqBy, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
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
                DataSet ds = db.SetPhysicalPulloutReceipting(pPullout.PloRId, pPullout.PloId, pl.ConvertDate(pPullout.ReturnDate), pPullout.ReturnBy, pPullout.Remarks, pl.LoginUserId);
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

        [HttpPost]
        public JsonResult GetDocType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("1", pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select All --";
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
