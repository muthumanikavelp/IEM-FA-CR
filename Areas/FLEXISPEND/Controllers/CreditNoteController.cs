using IEM.Common;
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
    public class CreditNoteController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        CmnFunctions cmnfunc = new CmnFunctions();
        #endregion

        //
        // GET: /FLEXISPEND/CreditNote/
        public ActionResult Maker()
        {
            return View();
        }

        //
        // GET: /FLEXISPEND/CreditNote/
        public ActionResult Checker()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult GetCreditNoteMaker(string Rejected, string DateFrom, string DateTo, string SupplierId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCreditNoteMaker(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), SupplierId, Rejected, "1", plib.LoginUserId);
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
        public JsonResult LoadCreditNoteInfo()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetCreditNoteMaker("", "", "", "0", "0", plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[1];
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
        public JsonResult LoadInvoiceDetails(string ECFNo, string InvNo)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetInvoiceAmountDetails(ECFNo, InvNo, "1", plib.LoginUserId);
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
        public JsonResult SetCreditNoteMaker(string Creditnotegid, string supplierId, string ecfid, string invid, string creditnoteno,
                            string creditnoteamt, string remarks, string isremoved)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("215");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetCreditNoteMaker(Creditnotegid, supplierId, ecfid, invid, creditnoteno, (creditnoteamt == "" || creditnoteamt == null) ? "0" : creditnoteamt, remarks, isremoved, plib.LoginUserId);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                    else { return null; }
                }
                else
                {
                    Data1 = "Unauthorized User!";
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
            }
        }


        //Credit Note Checker
        [HttpPost]
        public JsonResult GetCreditNoteChecker(string DateFrom, string DateTo, string SupplierId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCreditNoteChecker(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), SupplierId, plib.LoginUserId);
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
        public JsonResult SetCreditNoteChecker(string Creditnotegid, string Status, string remarks)
        {
            try
            {
                string Data1 = "";
                 string checker = string.Empty;
                checker = cmnfunc.authorize("216");
                if (checker == "Success")
                {
                    DataSet ds = db.SetCreditNoteChecker(Creditnotegid, Status, remarks, plib.LoginUserId);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                    else { return null; }
                }
                else
                {
                    Data1 = "Unauthorized User!";
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
