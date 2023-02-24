using IEM.Areas.FLEXISPEND.Models;
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
    public class UrgentTaggingController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXISPEND/UrgentTagging/
        public ActionResult Index()
        {
            return View();
        }

        //
        // UrgentTagging/FetchUrgentTagDetails
        [HttpPost]
        public JsonResult FetchUrgentTagDetails(string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string DocTypeId, string DocRefNo, string ApprDate, string Amount, string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetUrgentTagging(EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, DocTypeId, DocRefNo, plib.ConvertDate(ApprDate), Amount == "" ? "0" : Amount, ViewType, "0", plib.LoginUserId);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                
                if (ds.Tables[1].Rows.Count == 0)
                {
                    DataTable dts = new DataTable();
                    dts.Columns.Add("Message");
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

        //Update Urgent Tag Details
        public JsonResult SetUrgentTag(string UTagId, string EcfId, string ReqBy, string ReqDate, string Remarks, string IsRemoved)
        {
            string Data1 = "";
            DataSet ds = db.SetUrgentTag(UTagId, EcfId, ReqBy, plib.ConvertDate(ReqDate), Remarks, IsRemoved, plib.LoginUserId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult DocumentDetails(string id = "", string subId = "")
        {
            //checker view
            @ViewBag.PREcfId = "";
            @ViewBag.PREcfdet = "";

            @ViewBag.PREcfId = id;
            @ViewBag.PREcfdet = string.Format("{0}-{1}-{2}", id, subId, "Y");
            return View();
        }
    }
}
