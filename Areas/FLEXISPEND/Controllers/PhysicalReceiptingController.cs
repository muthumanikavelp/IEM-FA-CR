using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Areas.MASTERS.Controllers;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PhysicalReceiptingController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        ForMailController mail = new ForMailController();
        #endregion

        //
        // GET: /FLEXISPEND/PhysicalReceipting/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPouchInward(PInward pInward)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPouchInward("", pl.ConvertDate(pInward.DateFrom == null ? "" : pInward.DateFrom), pl.ConvertDate(pInward.DateTo == null ? "" : pInward.DateTo), pInward.CourierId == null ? "" : pInward.CourierId, pInward.AWBNo == null ? "" : pInward.AWBNo, "", pl.LoginUserId);
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

        //get Physical receipt details
        [HttpPost]
        public JsonResult GetPhysicalReceipt(string PouchId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetPhysicalReceipt(PouchId, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 2)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
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

        //Save Physical receipting Details
        public JsonResult SavePhysicalReceipt(string PRGId, string Pouch_GId, string DocType, string DocRefNo, string Phy_Verification, string Remarks, string IsRemoved)
        {
            string Clear = "", Data1 = "", Data2 = "";
            DataSet ds = db.SetPhysicalReceipt(PRGId, Pouch_GId, DocType, DocRefNo, Phy_Verification, Remarks, IsRemoved, pl.LoginUserId);
            if (ds != null)
            {
                Clear= ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();   
                                
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                
                if (Clear == "1" || Clear == "true")
                { 
                    // DataSet dts = db.getuploaddata("1", "123");
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    string[] data = { };
                    if (DocRefNo != "") //Pandi 
                    {
                        DataSet ecf_gid = db.GetEcfGidByDocRefNo(DocRefNo);
                        // dts = db.getuploaddata("2", "123");
                        string ecfid = ecf_gid.Tables[0].Rows[0]["docinward_ecf_gid"].ToString();
                        if (ecf_gid != null)
                        {
                            // dts = db.getuploaddata("3", ecfid);
                            mail.sendusermailfs("FS", "Physical Receipting", ecfid, "S", data, "0");

                            //mail.sendusermailfs("","",ecf_gid,"",data,"");
                        }
                    }
                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
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

        public ActionResult DocumentDetails(string id = "", string subId="")
        {
            //checker View
            @ViewBag.PREcfId = "";
            @ViewBag.PREcfdet = "";

            @ViewBag.PREcfId = id;
            @ViewBag.PREcfdet = string.Format("{0}-{1}-{2}", id, subId, "Y~0~Y");
            return View();
        }
    }
}
