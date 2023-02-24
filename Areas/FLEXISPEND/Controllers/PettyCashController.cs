using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Controllers;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PettyCashController : Controller
    {
        #region Decalartion
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        ForMailController MailController = new ForMailController();
        #endregion

        //
        // GET: /FLEXISPEND/PettyCash/
        public ActionResult Alert()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPettyCashAlert(string ARFDateFrom, string ARFDateTo, string ARFNo, string ARFAmount, string EmpCodeId, string EmpNameId, string BranchCodeId, string BranchNameId, string RaiserCodeId, string RaiserNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPettyCashAlert(plib.ConvertDate(ARFDateFrom), plib.ConvertDate(ARFDateTo), ARFNo, ARFAmount == null || ARFAmount == "" ? "0" : ARFAmount, EmpCodeId, EmpNameId, BranchCodeId, BranchNameId, RaiserCodeId, RaiserNameId, plib.LoginUserId);
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

        //Email
        [HttpPost]
        public JsonResult GetPettyCashEmailAlert(string ARFDateFrom, string ARFDateTo, string ARFNo, string ARFAmount, string EmpCodeId, string EmpNameId, string BranchCodeId, string BranchNameId, string RaiserCodeId, string RaiserNameId)
        {
            string MailRes = "", MailTempbit = "", Data1 = "", Msg = "", Clear = "";
            int SuccCount = 0, FailCount = 0;      
            try
            {
                DataSet ds = db.GetPettyCashAlert(plib.ConvertDate(ARFDateFrom), plib.ConvertDate(ARFDateTo), ARFNo, ARFAmount == null || ARFAmount == "" ? "0" : ARFAmount, EmpCodeId, EmpNameId, BranchCodeId, BranchNameId, RaiserCodeId, RaiserNameId, plib.LoginUserId);
                if (ds != null)
                {
                    //Table
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count ; i++)
                        {
                            if (ds.Tables[1].Rows[i]["emailId"].ToString() != "")
                            {
                                MailRes = MailController.sendusermailfs("FS", "Petty Cash Email Alert", ds.Tables[1].Rows[i]["EcfId"].ToString(), "s", new[] { ds.Tables[1].Rows[i]["emailId"].ToString() }, "0");
                                if (MailRes == "Sucess")
                                {
                                    SuccCount++;
                                    MailTempbit = "1";
                                    Msg = "Mail Sent Successfully!";
                                    Clear = "true";

                                }
                                else
                                {
                                    FailCount++;
                                    if (MailTempbit == "1") { }
                                    else
                                    {
                                        MailTempbit = "0";
                                        Msg = "Mail has not been sent!";
                                        Clear = "false";
                                    }
                                };
                            }
                            else
                            {

                                FailCount++;
                                if (MailTempbit == "")   //for email id not available
                                {
                                    MailTempbit = "0";
                                    Msg = "Mail has not been sent!";
                                    Clear = "false";
                                }
                            }
                        }
                    }
                    else
                    {
                        Msg = "Data not available for mail sending!";
                        Clear = "false";
                    }

                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Message", typeof(string));
                    dt2.Columns.Add("Clear", typeof(string));
                    dt2.Rows.Add(new object[] { Msg + "  Total Record:" + ds.Tables[1].Rows.Count + ", Success Mail:" + SuccCount + ", Failure Mail:" + FailCount, Clear });
                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

                }
                else
                { //TempData["PaymentAdvicePrint"] = null;
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

    }
}
