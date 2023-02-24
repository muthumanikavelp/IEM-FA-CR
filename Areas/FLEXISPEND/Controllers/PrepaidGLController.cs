using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PrepaidGLController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        CmnFunctions cmnfunc = new CmnFunctions();
        #endregion

        //
        // GET: /FLEXISPEND/PrepaidGL/
        public ActionResult Maker()
        {
            return View();
        }

        //
        // GET: /FLEXISPEND/PrepaidGL/
        public ActionResult Checker()
        {
            return View();
        }

        #region Maker
        [HttpPost]
        public JsonResult FetchPrepaidMaker(string ARFFrom, string ARFTo, string ARFNo, string ARFAmt, string liqdatefrom, string liqdateto, string Type, string Paymode, string SCode, string SName,
                    string Empcode, string Empname, string Raisercode, string Raisername, string Advancetype, string Advancegl, string rejected)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPrepaidGLMaker(plib.ConvertDate(ARFFrom), plib.ConvertDate(ARFTo), ARFNo, ARFAmt == "" || ARFAmt == null ? "0" : ARFAmt, plib.ConvertDate(liqdatefrom), plib.ConvertDate(liqdateto), Type, Paymode, SCode, SName, Empcode, Empname, Raisercode, Raisername, Advancetype, Advancegl, rejected, plib.LoginUserId);
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

        public JsonResult SetPrepaidMaker(string ecfid, string ecfarfid, string newadvancedate, string newadvencetype, string newadvancegl, string remarks, string isremoved)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("296");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetPrepaidGLMaker("0", ecfid, ecfarfid, plib.ConvertDate(newadvancedate), newadvencetype, newadvancegl, remarks, isremoved, plib.LoginUserId);
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
        #endregion

        #region Checker
        [HttpPost]
        public JsonResult FetchPrepaidChecker(string RequestFrom, string RequestTo, string RequestCode, string RequestName, string Paymode, string SCode, string SName, string Empcode, string Empname, string Type, string ARFNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPrepaidGLChecker(plib.ConvertDate(RequestFrom), plib.ConvertDate(RequestTo), RequestCode, RequestName, Paymode, SCode, SName, Empcode, Empname, Type, ARFNo, plib.LoginUserId);
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

        public JsonResult SetPrepaidChecker(string gltransfergid, string Status, string remarks)
        {
            try
            {
                string Data1 = "";
                string checker = string.Empty;
                checker = cmnfunc.authorize("297");
                if (checker == "Success")
                {
                    DataSet ds = db.SetPrepaidGLChecker(gltransfergid, Status, remarks, plib.LoginUserId);
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
        #endregion
    }
}
