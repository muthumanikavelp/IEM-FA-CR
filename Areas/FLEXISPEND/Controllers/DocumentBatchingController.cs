using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class DocumentBatchingController : Controller
    {

        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //GET: /FLEXISPEND/DocumentBatching/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDocumentBatching(DocumentBatching DBatching)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetDocumentBatching(pl.ConvertDate(DBatching.InwardDate==null?"":DBatching.InwardDate), pl.ConvertDate(DBatching.DateFrom == null ? "" : DBatching.DateFrom), pl.ConvertDate(DBatching.DateTo == null ? "" : DBatching.DateTo), DBatching.BatchNo == null ? "" : DBatching.BatchNo, DBatching.ViewType, pl.LoginUserId);
                if (DBatching.ViewType == "0")
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else if (DBatching.ViewType == "1")
                {
                    
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

          //Save Document Batching Details
        public JsonResult SetDocumentBatching(string BatchId, string InwardDate, string InwardFrom, string InwardTo,string IsRemoved, string UId)
        {
            try 
            { 
            string Clear = "", Data1 = "", Data2 = "";
            DataSet ds = db.SetDocumentBatching(BatchId, pl.ConvertDate(InwardDate == null ? "" : InwardDate), InwardFrom, InwardTo,  IsRemoved, pl.LoginUserId);
            if (ds != null)
            {
                Clear= ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();   
                                
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                if (Clear == "1" || Clear == "true")
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1,Data2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
            }
            catch
            {
                return null;
            }
        }
    }
}
