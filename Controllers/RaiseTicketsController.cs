using IEM.Common;
using IEM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Controllers
{
    public class RaiseTicketsController : Controller
    {
         private IEMRepository objModel;
        ErrorLog objErrorLog = new ErrorLog();
        public RaiseTicketsController()
            :this(new IEMDataModel()) 
        {

         }
        public RaiseTicketsController(IEMRepository objM)
        {
            objModel = objM; 
        }
        //
        // GET: /RaiseTickets/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTicketSummary()
        {
            string data0 = string.Empty; 
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetTicketSummary();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { 
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InsertTicketComments(RaiseTicketEntity objRaiseTic)  
        {
            try
            {
                objModel.InsertTicketComments(objRaiseTic);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SingleTicket(string refname = null,string itemrefno = null)  
        {
            ViewBag.refname = refname;
            ViewBag.itemrefno = itemrefno;
            return View();
        }
       [HttpPost]
        public JsonResult InsertTicketCommentsNew(RaiseTicketEntity objRaiseTic) 
        {
            try
            {
                objModel.InsertTicketCommentsNew(objRaiseTic); 
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTicketSummarySingle(string refflag = null, string itemrefno = null)
        { 
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetTicketSummarySingle(refflag,itemrefno);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }

                }
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SingleTicketNormal(string refname = null, string itemrefno = null) 
        {
            ViewBag.refname = refname;
            ViewBag.itemrefno = itemrefno;
            string data0 = string.Empty;
            string data1 = string.Empty;
            DataSet ds = new DataSet();
            NewRaiseTicketList objRaiseTicket = new NewRaiseTicketList();
            DataTable dt;
            ds = objModel.GetTicketSummarySingle(refname, itemrefno);
            if (ds != null && ds.Tables.Count != 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                objRaiseTicket.lstTickets = JsonConvert.DeserializeObject<List<RaiseTicketEntity>>(data0);
                ViewBag.lstTickets = objRaiseTicket.lstTickets;
                objRaiseTicket.lstReplies = JsonConvert.DeserializeObject<List<RaiseTicketEntity>>(data1);
                ViewBag.lstReplies = objRaiseTicket.lstReplies;
            }
            return View();
        }
    }
}
