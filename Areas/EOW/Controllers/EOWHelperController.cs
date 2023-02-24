using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using IEM.Areas.FLEXISPEND.Models;
using System.Data;
using Newtonsoft.Json;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class EOWHelperController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        DataTable dt = new DataTable();
        FSRepository _fsRep = new FSRepository();
        #endregion
        //
        // GET: /EOW/EOWHelper/

        public ActionResult Index()
        {
            return View();
        }
        //Supplier Code & Name Combained Auto Complete
        public JsonResult GetAutoCompleteSupplierAll(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteSupplierAll(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetGSTINArray(string ViewType, string RefId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetGSTMaster(ViewType, RefId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (ViewType != "4")
                    {
                        dt.Rows[0][2] = "";
                    }
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SetAdhocVendor(string Name, string GSTIN, string LocationId, string Suppliergid, string IsGST)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetAdhocVendor(Name, GSTIN, LocationId, Suppliergid, IsGST, plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetGSTNVendorDetails(string ViewType, string GSTIN, string SubrefId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetGSTNVendorDetails(ViewType, GSTIN, SubrefId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Tax Type
        [HttpPost]
        public JsonResult GetTaxType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("52", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Tax Rate
        [HttpPost]
        public JsonResult GetTaxRate()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster1("26", "1", "0");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "0";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //With Holding Tax Sub Type
        [HttpPost]
        public JsonResult GetWHTaxSubType(string CatId)
        {
            try
            {
                string Data1 = "";
                //DataSet ds = db.GetMaster("24", CatId, "1");
                DataSet ds = db.GetMaster1("51", "1", CatId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult SetCessTaxDetails(InvoiceTax iTax)
        {
            try
            {
                string Data1 = "";
                string inv = "0";
                if (!string.IsNullOrEmpty(Convert.ToString(Session["invoiceGid"])))
                {
                    inv = Convert.ToString(Session["invoiceGid"]);
                }
                else
                {
                    inv = iTax.InvId;
                }
                DataSet ds = db.SetCessTaxDetails(iTax.action, iTax.InvoiceTaxgid, inv, iTax.TaxId, iTax.SubTaxId, iTax.TaxRate, iTax.TaxableAmt, iTax.TaxAmount, plib.LoginUserId);

                DataTable dt = ds.Tables[0];
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
