using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Reflection;
using IEM.Helper;
using IEM.Areas.FLEXISPEND.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PouchInwardController : Controller
    {

        #region Declaration
        PouchInward objModel = new PouchInward();
        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        proLib plib = new proLib();
        Message msg = new Message();
        DataTable dt = new DataTable();
        #endregion

        // GET: /FLEXISPEND/PouchInward/
        public ActionResult Index()
        {
            return View();
        }

        // PouchInward/GetDetails  
        [HttpPost]
        public JsonResult FetchInwardDetails(string frmDate, string toDate, string courId, string AWBNo)
        {
            DataSet ds = new DataSet();
            try
            {
                List<PouchInward> dept = new List<PouchInward>();
                ds = db.GetPouchInward("", plib.ConvertDate(frmDate), plib.ConvertDate(toDate), courId, AWBNo, "", plib.LoginUserId);
                if (ds != null)
                {
                    msg.TotalCount = ds.Tables[0].Rows[0]["TotCount"].ToString();
                    msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();
                    if (msg.MessageText.Trim().Length == 0)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { dept = ConvertDataTable<PouchInward>(dt); }

                        msg.Clear = "true";
                        return Json(new { msg, dept }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg.Clear = "false";
                        return Json(new { msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                return null;
            }
            catch
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetInwardDetails(string PouchId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<PouchInward> dept = new List<PouchInward>();
                ds = db.GetPouchInward(PouchId, "", "", "", "", "", plib.LoginUserId);
                if (ds != null)
                {
                    msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();
                    if (msg.MessageText.Trim().Length == 0)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { dept = ConvertDataTable<PouchInward>(dt); }

                        msg.Clear = "true";
                        return Json(new { msg, dept }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg.Clear = "false";
                        return Json(new { msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                return null;
            }
            catch
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //Save Inward Pouch Details
        public JsonResult SavePouchDetails(PouchInward piw)
        {
            DataSet ds = db.SetPouchInward(Convert.ToString(piw.Id), piw.SenderTypeCode, Convert.ToString(piw.SenderId), plib.ConvertDate(piw.ReceivedDate), piw.Sender, Convert.ToString(piw.Noofdocs), Convert.ToString(piw.CourierId), piw.AWBNo, piw.Remarks, "0", plib.LoginUserId);
            if (ds != null)
            {
                msg.Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();
                msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();
                if (msg.Clear == "1" || msg.Clear == "true")
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }

        //Delete Inward Pouch Details
        public JsonResult DeletePouchDetails(string Id)
        {
            DataSet ds = db.SetPouchInward(Id, "", "0", "", "", "0", "0", "", "", "1", plib.LoginUserId);
            if (ds != null)
            {
                msg.Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();
                msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();
                if (msg.Clear == "1" || msg.Clear == "true")
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        #region Convert Datatable to List
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion
    }

    public class Message
    {
        public string MessageText { get; set; }
        public string Clear { get; set; }
        public string TotalCount { get; set; }
    }
}
