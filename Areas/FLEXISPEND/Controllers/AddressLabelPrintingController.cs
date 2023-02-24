
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class AddressLabelPrintingController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        // GET: /FLEXISPEND/AddressLabelPrinting/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAddressLabelPrint(string PVDateFrom, string PVDateTo, string PVNo, string PVAmount, string EmpCodeId, string EmpNameId, string SuppCodeId,
            string SuppNameId, string PayMode, string ClaimType, string BatchNo, string Location)
        {
            try
            {
                string Data1 = "", Data2 = "";

                DataSet ds = db.GetAddressLabelPrint(plib.ConvertDate(PVDateFrom), plib.ConvertDate(PVDateTo), PVNo, PVAmount, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, PayMode, ClaimType, BatchNo, Location, plib.LoginUserId);
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
        public JsonResult PrintAddressLable(string PvIds, string Status)
        {
            try
            {
                TempData["AddressPrint"] = null;
                TempData["AddressPrint"] = string.Format("{0}-{1}", PvIds == "" ? "0" : PvIds, Status);
                TempData.Keep("AddressPrint");
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        
        public ActionResult Print()
        {
            List<AddressDetail> result = new List<AddressDetail>();
            string det = "";
            det = TempData["AddressPrint"] != null ? TempData["AddressPrint"].ToString() : "";
            DataSet ds = db.PrintAddressLabelPrint(det.Split('-')[0], det.Split('-')[1], plib.LoginUserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    result = FillAddressDetails(ds.Tables[1]);
                }
            }
            return View(result);
        }

        public List<AddressDetail> FillAddressDetails(DataTable dtDet)
        {
            List<AddressDetail> result = new List<AddressDetail>();
            if (dtDet != null)
            {
                for (int i = 0; i < dtDet.Rows.Count; i++)
                {
                    AddressDetail rec = new AddressDetail();
                    rec.Type = dtDet.Rows[i]["Type"].ToString();
                    rec.Address = dtDet.Rows[i]["address"].ToString();
                    if (dtDet.Rows[i]["Type"].ToString().ToLower() == "1")
                    {
                        rec.CompanyName = dtDet.Rows[i]["CompanyName"].ToString();
                        rec.LocationCode = dtDet.Rows[i]["LocationCode"].ToString();
                    }

                    result.Add(rec);
                }
            }
            return result;
        }
    }

    public class AddressDetail
    {
        public string Type { get; set; }
        public string CompanyName { get; set; }
        public string LocationCode { get; set; }
        public string Address { get; set; }
        
    }
}
