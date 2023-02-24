using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PaymodeConversionController : Controller
    {

        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        CmnFunctions cmnfunc = new CmnFunctions();
        #endregion
        //
        // GET: /FLEXISPEND/PaymodeConversion/
        public ActionResult Maker()
        {
            return View();
        }

        //
        // GET: /FLEXISPEND/PaymodeConversion/
        public ActionResult Checker()
        {
            return View();
        }
       
        //Maker
        [HttpPost]
        public JsonResult GetPaymodeConversion(string PVNoFrom,string PVNoTo,string PVNo,string Amount,string EmployeeCode,string EmployeeName,string SupplierCode,string SupplierName,string BounceFrom,string BounceTo,string ChequeNo, string MemoNo,string PayMode,string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPaymodeConversionMaker(pl.ConvertDate(PVNoFrom), pl.ConvertDate(PVNoTo), PVNo,Amount==""?"0":Amount, EmployeeCode, EmployeeName,SupplierCode, SupplierName, pl.ConvertDate(BounceFrom), pl.ConvertDate(BounceTo), ChequeNo, MemoNo, PayMode, ViewType, pl.LoginUserId);

                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetPaymodeConversion(string PVId, string PayMode, string Beneficiary,
            string AccNo, string IfscCode, string BankId, string Desc, string Remarks)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("213");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetPaymodeConversionMaker(PVId, PayMode, Beneficiary, AccNo, IfscCode, BankId, Desc, Remarks, pl.LoginUserId);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                else
                {
                    Data1 = "Unauthorized User!";
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);


            }
            catch
            {
                return null;
            }
        }

       //Checker
        [HttpPost]
        public JsonResult GetPaymodeConversionChecker(string PVNo, string ReqDateFrom, string ReqDateTo, string ReqCodeId,
            string ReqNameId, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string PayMode,
             string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPaymodeConversionChecker(PVNo, pl.ConvertDate(ReqDateFrom), pl.ConvertDate(ReqDateTo),ReqCodeId,ReqNameId,EmpCodeId,EmpNameId,SuppCodeId,SuppNameId,PayMode,ViewType, pl.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult SetPaymodeConversionChecker(string PaymentReIssueGId, string Status, string Remarks, string chqno)
        {
            try
            {
                string Data1 = "";
                string checker = string.Empty;
                checker = cmnfunc.authorize("214");
                if (checker == "Success")
                {
                    DataSet ds = db.SetPaymodeConversionChecker(PaymentReIssueGId, Status, Remarks, pl.LoginUserId, chqno);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                else
                {
                    Data1 = "Unauntorized User!";
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);


            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult FetchBenificiaryDetails(string PayMode, string SupplierId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetSupplierBeneficiary(SupplierId, PayMode);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
     
    }
}
