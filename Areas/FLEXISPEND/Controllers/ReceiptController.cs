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
    public class ReceiptController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        CmnFunctions cmnfun = new CmnFunctions();
        #endregion

        //
        // GET: /FLEXISPEND/Receipt/
        public ActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReceiptEntry(string ReceiptDateFrom, string ReceiptDateTo, string ReceiptNo, string ReceiptFrom, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetReceiptEntry(plib.ConvertDate(ReceiptDateFrom), plib.ConvertDate(ReceiptDateTo), ReceiptNo, ReceiptFrom, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetReceiptEntry(string ReceiptId, string ReceiptDate, string ReceiptFrom, string Source, string Raiser, string EmpSuppNameId,
                                        string ReceiptMode, string Amount, string PayRefNo, string TranDate, string BankId, string Remarks)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfun.authorize("319");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetReceiptEntry(ReceiptId, plib.ConvertDate(ReceiptDate), ReceiptFrom, Source, Raiser, EmpSuppNameId, ReceiptMode, Amount == null || Amount == "" ? "0" : Amount, PayRefNo, plib.ConvertDate(TranDate), BankId, Remarks, plib.LoginUserId);
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

        [HttpPost]
        public JsonResult SetGLUploads(string ReceiptGLId, string ReceiptId, string CRFrom, string DocNo, string CRGlNo, string Description, string Amount, string IsRemoved)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetGLUploads(ReceiptGLId, ReceiptId, CRFrom, DocNo, CRGlNo, Description, Amount == null || Amount == "" ? "0" : Amount, IsRemoved, plib.LoginUserId);
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

        //
        // GET: /FLEXISPEND/Receipt/
        public ActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReceiptChecker(string ReceiptDateFrom, string ReceiptDateTo, string ReceiptNo, string ReceiptFrom, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId,
            string PayDateFrom, string PayDateTo, string BankId, string ReceiptMode, string PayMode, string Amount, string PayRefNo)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetReceiptChecker(plib.ConvertDate(ReceiptDateFrom), plib.ConvertDate(ReceiptDateTo), ReceiptNo, ReceiptFrom, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId,
                    plib.ConvertDate(PayDateFrom), plib.ConvertDate(PayDateTo), BankId, ReceiptMode, PayMode, Amount == null || Amount == "" ? "0" : Amount, PayRefNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UpdateReceiptStatus(string ReceiptId, string Status, string Remarks)
        {
            try
            {
                string Data1 = "";
                string Checker = string.Empty;
                
                if (Status == "IsMakerRaised")
                {
                    Checker = cmnfun.authorize("319");
                }
                else
                {
                    Checker = cmnfun.authorize("320");
                }
                if (Checker == "Success")
                {
                    DataSet ds = db.UpdateReceiptStatus(ReceiptId, Status, Remarks, plib.LoginUserId);
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
