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
    public class PaymentController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXISPEND/Payment/
        public ActionResult chequeInventory()
        {
            return View();
        }

        //Get Check List Details
        public JsonResult GetChequeListDetails(string BankId, string ChqBookNo, string ViewType)
        {
            string Data1 = "";
            DataSet ds = db.GetChequeBookDetails(BankId, ChqBookNo, ViewType, pl.LoginUserId);
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

        //Save Check Inventory Details
        public JsonResult SaveChequeInventory( string BankId, string Date, string ChqBookNo, string ChqNoFrom,
            string ChqNoTo, string Reason, string ViewType)
        {
            string Data1 = "";
            DataSet ds = db.SetChequeInventory( BankId, pl.ConvertDate(Date), ChqBookNo, ChqNoFrom, ChqNoTo, Reason, ViewType, pl.LoginUserId);
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

    }
}
