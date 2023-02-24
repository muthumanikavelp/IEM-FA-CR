using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Helper;
using IEM.Areas.FLEXISPEND.Models;
using Newtonsoft.Json;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class FBHelperController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        #endregion

        //CBF Project Owner
        public JsonResult GetProjectOwner()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("33", "1");
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

        //GBF Request By
        public JsonResult GetRequestBy()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("32", "1");
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

        //CBF Budget SPOC
        public JsonResult GetBudgetSPOC()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("34", "1");
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

        //CBF Product Group
        public JsonResult GetProductGroup(string prodservice = null)
        {
            try
            {
                string Data1 = "";
                int prodservflag = Convert.ToInt32(string.IsNullOrEmpty(prodservice) ? "0" : prodservice);
                DataSet ds = db.GetMaster("35", "1", prodservflag);
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

        //CBF Product Name
        public JsonResult GetProductName(string CatId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("36", CatId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    dt.Rows[0][2] = "";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //CBF Product UOM
        public JsonResult GetUOM()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("37", "1");
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

        //PO Template
        public JsonResult GetPOTemplate()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("38", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    dt.Rows[0][2] = "";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //PO Shipment Type
        public JsonResult GetShipmentType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("39", "1");
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

        //Branch Auto Complete
        public JsonResult GetAutoCompleteBranch(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteBranch(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //FCCC Auto Complete
        public JsonResult GetAutoCompleteFCCC(string txt)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "16", plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch { return null; }
        }

        //Vendor Auto Complete
        public JsonResult GetAutoCompleteVendor(string txt)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "4", plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}~{2}~{3}", dr[0], dr[1],dr[2],dr[3]));
                    }
                }
                ds.Dispose();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch { return null; }
        }

        //Employee Auto Complete
        public JsonResult GetAutoCompleteEmployee(string txt)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "13", plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch { return null; }
        }

        //Employee Auto Complete
        public JsonResult GetAutoCompleteBranchPO(string txt)
        {
            try
            {
               List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "17", plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}~{2}~{3}~{4}~{5}", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5]));
                    }
                }
                ds.Dispose();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch { return null; }
        }

       //add gst start
        public JsonResult Getgstcbodetls()
        {
            DataTable dt = new DataTable();
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                DataSet ds = db.GetGstCbolist();
                if (ds != null && ds.Tables.Count > 0)
                {
                     dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    DataRow row1 = dt.NewRow();
                    dt.Rows.InsertAt(row1, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    DataRow row2 = dt.NewRow();
                    dt.Rows.InsertAt(row2, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[3];
                    DataRow row3 = dt.NewRow();
                    dt.Rows.InsertAt(row3, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                }
                return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

    }
}
