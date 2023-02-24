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
    public class ChequeNoUpdationController : Controller
    {

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion
        //
        // GET: /FLEXISPEND/ChequeNoUpdation/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetChequeNoUpdation(string PaymentDate, string BatchNo, string SuppNameId, string EmpNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetChequeNoUpdation(plib.ConvertDate(PaymentDate), BatchNo, SuppNameId, EmpNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetChequeNoUpdation(string ChqDate, string BankId, string ChqBookNo, string ChqNoFrom, string ChqNoTo,
                            string PVNoFrom, string PVNoTo, string Reason, string ViewType)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetChequeNoUpdation(plib.ConvertDate(ChqDate), BankId, ChqBookNo, ChqNoFrom, ChqNoTo, PVNoFrom, PVNoTo, Reason, ViewType, plib.LoginUserId);
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
