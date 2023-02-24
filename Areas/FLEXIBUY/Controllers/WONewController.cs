using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;
using Newtonsoft.Json;
using IEM.Areas.MASTERS.Controllers;
using System.Collections;
using IEM.Helper;
using System.Net;
using System.IO;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class WONewController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
        private WORepository objModel;
        proLib plib = new proLib();
        dbLib db = new dbLib();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public WONewController()
            : this(new WODataModel())
        {

        }
        public WONewController(WORepository objM)
        {
            objModel = objM;
        }
        //
        // GET: /FLEXIBUY/WONew/

        public ActionResult Index(string cbfdetailsgid = null,string wogid = null,string viewtype = null)
        {
            Session["objlist"] = null;
            ViewBag.isapproval = "";
            ViewBag.isapprovalMode = "0";
            ViewBag.deleteflag = "0";
            if (cbfdetailsgid != null)
               ViewBag.cbfdetailsgid = cbfdetailsgid;
            else
               ViewBag.cbfdetailsgid = "0";
            if (wogid != null){
                Session["WOHeaderGid"] = Convert.ToInt32(wogid);
                ViewBag.wogid = wogid;
            }
            else{
                ViewBag.wogid = "0";
            }
            if (viewtype == "view"){
                 ViewBag.viewtype = "1";  
            }
            else if (viewtype == "delmat" || viewtype == "checker")
            {
               ViewBag.viewtype = "1";
               ViewBag.isapprovalMode = "1";
               ViewBag.isapproval = viewtype;
            }
            else if (viewtype == "delete")
            {
                ViewBag.viewtype = "1";
                ViewBag.deleteflag = "1";
            }
            else{
               ViewBag.viewtype = "0";
            }
            if (Session["WOHeaderGid"] != null)
            {
                if (ViewBag.wogid != "0") {
                    ViewBag.wogid = Convert.ToString((int)Session["WOHeaderGid"]); 
                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetWOHeaderDetails(string cbfdetid = "0")
        {
            //string cbfdetid = "19";
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty;
            string ErrorMsg = string.Empty;
           int wogid = 0;
            try
            {
                string[] cbfdetarray = cbfdetid.Split(',');
                if (Session["WOHeaderGid"] != null)
                {
                    wogid = (int)Session["WOHeaderGid"];
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsParent(wogid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[3];
                    if (dt.Rows.Count > 0) { data3 = JsonConvert.SerializeObject(dt); }
                }
                else
                {
                    if (cbfdetarray.Length > 0)
                    {
                        for (int i = 0; i < cbfdetarray.Length; i++)
                        {
                            int cbfgid = Convert.ToInt32(string.IsNullOrEmpty(cbfdetarray[i].ToString()) ? "0" : cbfdetarray[i].ToString());
                            if (Session["WOHeaderGid"] != null)
                            {
                              //  wogid = (int)Session["WOHeaderGid"];
                                objModel.InsertWODetails(cbfgid, wogid);
                            }
                            else
                            {
                                wogid = objModel.InsertWOHeader(cbfgid);
                                Session["WOHeaderGid"] = wogid;
                                ViewBag.wogid = wogid;
                                objModel.InsertWODetails(cbfgid, wogid);
                            }
                        }
                        objModel.UpdateWOHeaderAmount(wogid);
                        DataSet ds = new DataSet();
                        DataTable dt;
                        ds = objModel.GetWODetailsParent(wogid);
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { data3 = JsonConvert.SerializeObject(dt); }
                    }
                    else
                    {
                        ErrorMsg = "1";
                    }
                }
                
            
                //int cbfgid = Convert.ToInt32(string.IsNullOrEmpty(cbfdetid) ? "0" : cbfdetid);
                //DataSet ds = new DataSet();
                //DataTable dt;
                //ds = objModel.GetWODetails(cbfgid); 
                //dt = ds.Tables[0];
                //if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                //dt = ds.Tables[1];
                //if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                //dt = ds.Tables[2];
                //if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1, data2,data3, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "2";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2,data3, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetWOHeaderDetails1(string wogid = "0")  
        {
            //string cbfdetid = "19";
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty; 
            int woheadergid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid); 
            try 
            { 
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetWODetailsParent(woheadergid); 
                dt = ds.Tables[0]; 
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { data3 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1, data2, data3 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, data3 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddWOParentDetails(WOEntity WoParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty; 
            string ErrorMsg = string.Empty; 
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddParentWODetails(WoParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsParent(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddWOParentDetails1(WOEntity WoParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty; 
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddParentWODetails1(WoParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsParent(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetWODetailParent(string wodetailgid = null) 
        {
            string data0 = string.Empty;
            int wogid = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetWODetailsParentForEdit(wogid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetWODetailChild(string wodetailgid = null)
        { 
            string data0 = string.Empty;
            string data1 = string.Empty;
            int wogid = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetWODetailsChild(wogid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                if (ds.Tables.Count > 1)
                {
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
        [HttpPost]
        public JsonResult UpdateWOParentDetails(WOEntity WoParentDetailsInsert)
        { 
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.UpdateParentWODetails(WoParentDetailsInsert);
                if (result == 0)
                { 
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsParent(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetProductGroup(string flag = null)
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetProductGroup(flag);
                dt = ds.Tables[0];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetProductGroupById(string flag = null,string ProdGroupId = null) 
        {
            string data0 = string.Empty;
            int ParentProduct = Convert.ToInt32(string.IsNullOrEmpty(ProdGroupId) ? "0" : ProdGroupId);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetProductByGroup(flag,ParentProduct);
                dt = ds.Tables[0];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetProductDesc(string prodgid = null) 
        { 
            string data0 = string.Empty;
            int ParentProduct = Convert.ToInt32(string.IsNullOrEmpty(prodgid) ? "0" : prodgid);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetProductDesc(ParentProduct);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ADDWOChildDetails(WOEntity WoParentDetailsInsert)
        {  
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddChildWODetails(WoParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1"; 
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsChild(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateWOChildDetails(WOEntity WoParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.UpdateChildWODetails(WoParentDetailsInsert); 
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWODetailsChild(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetBranchAutoComplete(string searchtext = null) 
        {
            List<string> result = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                ds = objModel.GetBranchAutoComplete(searchtext);
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetEmployeeAutoComplete(string searchtext = null)
        {
            List<string> result = new List<string>(); 
            try
            {
                DataSet ds = new DataSet();
                ds = objModel.GetEmployeeComplete(searchtext);
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ADDWOShipmentDetails(WOEntity WoShipmentInsert) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.ADDWOShipmentDetails(WoShipmentInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetWOShipmentDetails(result); 
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetWOShipment(string wodetailgid = null, string vendorid = null)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty;
            string data4=string.Empty;
            int wogid = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid);
            int Vdid = 0;
            if (vendorid == "undefined")
            {
                Vdid = 0;
            }
            else
            {
                Vdid = Convert.ToInt32(string.IsNullOrEmpty(vendorid) ? "0" : vendorid);
            }
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetWOShipmentDetails(wogid, Vdid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); } //wo shipment details
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); } // wo gst gride details
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); } // Wo details
                dt = ds.Tables[3];
                DataRow row3 = dt.NewRow();
                dt.Rows.InsertAt(row3, 0);
                dt.Rows[0][0] = "0";
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data3 = JsonConvert.SerializeObject(dt); } // provider location

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { data4 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0, data1, data2, data3, data4 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, data3 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteWOParentDetails(WOEntity WoDetailsForDelete) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int itemgid = Convert.ToInt32(string.IsNullOrEmpty(WoDetailsForDelete.WODetailParentGid.ToString()) ? "0" : WoDetailsForDelete.WODetailParentGid.ToString());
                DataSet ds = objModel.DeleteWoParentDetails(itemgid, string.IsNullOrEmpty(WoDetailsForDelete.WORequestFor) ? "" : WoDetailsForDelete.WORequestFor); 
                DataTable dt; 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                ErrorMsg = "0";
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveAsDraft(WOEntity ObjWOEntity)
        { 
            string ErrorMsg = string.Empty;
            try
            {
                
                ErrorMsg = "0";
                int result = objModel.SaveAsDraft(ObjWOEntity);
                if (result == 0)
                {
                    ErrorMsg = "";
                }
                else
                {
                    ErrorMsg = "1";
                }
                if (ErrorMsg == "1")
                    Session["WOHeaderGid"] = null;
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SubmitRaiser(WOEntity ObjWOEntity)  
        { 
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                string result = objModel.SubmitRaiser(ObjWOEntity);
                if (result == "1")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    ForMailController mailsender = new ForMailController();
                    //int refgid = objforMail.GetRefGidForMail("WO");
                    string reqstatus = objforMail.GetRequestStatus(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                    int queuegid = objforMail.GetQueueGidForMail(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                    string curapprovalstage = "0";
                    if (reqstatus == "S")
                    {
                        curapprovalstage = "0";
                    }
                    else
                    {
                        curapprovalstage = "1";
                    }
                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = Convert.ToString(result);
                }
                if (ErrorMsg == "1")
                    Session["WOHeaderGid"] = null;
              
                return Json(new {  ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new {  ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SubmitApprover(WOEntity ObjWOEntity)  
        {
            string ErrorMsg = string.Empty;
            string result = string.Empty;
            string Data2 = "0";
            try
            {
                ErrorMsg = "0";
                DataTable dt = new DataTable();
                DataSet ds1 = db.CheckAuthorize(Convert.ToString(ObjWOEntity.WOGid), Convert.ToString(plib.LoginUserId), "WO");

                dt = ds1.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["Result"]) == "Y")
                    {
                        result = Convert.ToString(dt.Rows[0]["Result"]);
                    }
                    else
                    {

                        result = JsonConvert.SerializeObject(dt);
                    }
                }
                else
                {

                    result = JsonConvert.SerializeObject(dt);
                }

                if (result == "Y")
                {

                    ErrorMsg = objModel.GetDelmatemployeeForWo(ObjWOEntity);
                    if (ErrorMsg == "success" || ErrorMsg == "Success")
                    {
                        CbfSumModel objforMail = new CbfSumModel();
                        // int refgid = ObjWOEntity.WOGid;
                        string reqstatus = objforMail.GetRequestStatus(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                        int queuegid = objforMail.GetQueueGidForMail(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                        string curapprovalstage = "0";
                        if (reqstatus == "S")
                        {
                            curapprovalstage = "0";
                        }
                        else
                        {
                            curapprovalstage = "1";
                        }
                        ForMailController mailsender = new ForMailController();
                        mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);

                        ErrorMsg = "1";
                        DataSet ds2 = new DataSet();
                        DataTable dt2 = new DataTable();
                        
                        prsummodel prmodel = new prsummodel();
                        ds2 = prmodel.Getapprovaldetails("WO");
                        dt2 = ds2.Tables[0];


                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            Data2 = dt2.Rows[0]["queue_ref_gid"].ToString() + ":" + dt2.Rows[0]["doc_status_gid"].ToString();
                        }
                        ErrorMsg = ErrorMsg + "-" + Data2;
                        return Json(new { ErrorMsg}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ErrorMsg = ErrorMsg + "-" + Data2;
                        return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ErrorMsg = "2";
                    ErrorMsg = ErrorMsg + "-" + Data2;
                    return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ErrorMsg = ex + "-" + Data2;
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RejectApprover(WOEntity ObjWOEntity)
        { 
            string ErrorMsg = string.Empty;
            string result = string.Empty;
            string Data2 = "0";
            try
            {
                ErrorMsg = "0";
                ErrorMsg = objModel.RejectWO(ObjWOEntity);
                if (ErrorMsg == "1")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    //int refgid = ObjWOEntity.WOGid;
                    string reqstatus = objforMail.GetRequestStatus(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                    int queuegid = objforMail.GetQueueGidForMail(Convert.ToInt32(ObjWOEntity.WOGid), "WO");
                    string curapprovalstage = "0";
                    if (reqstatus == "S")
                    {
                        curapprovalstage = "0";
                    }
                    else
                    {
                        curapprovalstage = "1";
                    }
                    ForMailController mailsender = new ForMailController();
                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                }
                ErrorMsg = "1";
                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();

                prsummodel prmodel = new prsummodel();
                ds2 = prmodel.Getapprovaldetails("WO");
                dt2 = ds2.Tables[0];


                if (ds2.Tables[0].Rows.Count > 0)
                {
                    Data2 = dt2.Rows[0]["queue_ref_gid"].ToString() + ":" + dt2.Rows[0]["doc_status_gid"].ToString();
                }
                ErrorMsg = ErrorMsg + "-" + Data2;
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetAuditTrail(string wogid = null,string refname = null) 
        {
            List<AuditTrailWO> lst = new List<Models.AuditTrailWO>();
            int woheadergid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid); 
            try
            {
              
                lst = objModel.PopulateAuditTrail(woheadergid, refname);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(lst, JsonRequestBehavior.AllowGet); 
            }
        }
        [HttpPost]
        public JsonResult ChkWOShipment(string wodetailgid = null)  
        {
            int wodetgid = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid); 
            try
            {
                string ErrorMsg = objModel.ChkWOShipment(wodetgid);
                return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("404", JsonRequestBehavior.AllowGet); 
            }
        }
         [HttpPost]
        public JsonResult ValidateWOShipment(string wogid = null)    
        {
            string data0 = string.Empty;
            ArrayList result = new ArrayList(); 
            int woheadgid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid);  
            try
            {
                DataSet ds = new DataSet();
                ds = objModel.ValidateWOShipment(woheadgid);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                  result.Add(dr.ItemArray.Select(item => item.ToString()));
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
         [HttpPost]
         public JsonResult GetVendorNameAutoComplete(string searchtext = null) 
         {
             List<string> result = new List<string>();
             try
             {
                 DataSet ds = new DataSet();
                 ds = objModel.GetVendorNameAutoComplete(searchtext); 
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
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { result }, JsonRequestBehavior.AllowGet);
             }
         }
         [HttpPost]
         public JsonResult GetTermCondContent(string TermsAndCodGid = null)  
         {
             string data0 = string.Empty;
             int TermsGid = Convert.ToInt32(string.IsNullOrEmpty(TermsAndCodGid) ? "0" : TermsAndCodGid); 
             try
             {
                 DataSet ds = new DataSet();
                 DataTable dt;
                 ds = objModel.GetTermCondContent(TermsGid);
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                 return Json(new { data0 }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { data0 }, JsonRequestBehavior.AllowGet);
             }
         }
         [HttpPost]
         public JsonResult DeleteWO(string wogid = null) 
         {
             string ErrorMsg = string.Empty;
             int woheadergid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid); 
             try 
             {
                 ErrorMsg = "0";
                 int result = objModel.DeleteWo(woheadergid);
                 if (result == 0)
                     ErrorMsg = "";
                 else
                     ErrorMsg = "1";
                 return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
         }
         [HttpPost]
         public JsonResult ChkWOSplitExists(string wodetailgid = null)
         {
             int wodetgid = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid);
             try
             {
                 string ErrorMsg = objModel.ChkWOSplitExists(wodetgid); 
                 return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json("404", JsonRequestBehavior.AllowGet);
             }
         }
         public ActionResult WOAmendment() 
         {
             return View();
         }
         [HttpGet]
         public JsonResult GetWOAmendment() 
         {
             string data0 = string.Empty;
             try
             {
                 DataSet ds = new DataSet();
                 DataTable dt;
                 ds = objModel.GetWOAmendment();
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                 //  return Json(new { data0 }, JsonRequestBehavior.AllowGet);
                 var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                // jsonResult.MaxJsonLength = 2147483647;
                 jsonResult.MaxJsonLength = int.MaxValue;
                 return jsonResult;
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { data0 }, JsonRequestBehavior.AllowGet);
             }
         }
         [HttpGet]
         public JsonResult GetWOAmendmentnew(int wogid=0)
         {
             string data0 = string.Empty;
             try
             {
                 DataSet ds = new DataSet();
                 DataTable dt;
                 ds = objModel.GetWOAmendment();
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                 //  return Json(new { data0 }, JsonRequestBehavior.AllowGet);
                 var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                 // jsonResult.MaxJsonLength = 2147483647;
                 jsonResult.MaxJsonLength = int.MaxValue;
                 return jsonResult;
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { data0 }, JsonRequestBehavior.AllowGet);
             }
         }
         [HttpPost]
         public JsonResult DoAmend(string wogid = null)    
         {
             string ErrorMsg = "0";
             string NewWOGid = "0";
             int woheadergid = Convert.ToInt32(string.IsNullOrEmpty(wogid) ? "0" : wogid); 
             try
             {
                 DataTable dt = objModel.DoAmend(woheadergid);
                 if (dt.Rows.Count == 0)
                 {
                     ErrorMsg = "1";
                 }
                 else
                 {
                     NewWOGid = dt.Rows[0]["POHeaderGId"].ToString();
                     ErrorMsg = dt.Rows[0]["ErrMessage"].ToString();
                 }
                 return Json(new { ErrorMsg, NewWOGid }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 ErrorMsg = "1";
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 return Json(new { ErrorMsg, NewWOGid }, JsonRequestBehavior.AllowGet);
             }
         }

         [HttpPost]
         public JsonResult SetWoTemplate(string TemplateGId,string TemplateName,string Content,string IsRemoved)
         {
             try
             {
                
                 string Data1 = "";
                 DataSet ds = db.SetPOTemplate(TemplateGId, TemplateName, Content, IsRemoved, plib.LoginUserId);
                 DataTable dt = new DataTable();
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                 return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
             }
             catch
             {
                 return null;
             }
         }

         //Dhasarathan 
         #region
         [HttpPost]
         public void UploadedFiles(string uploadfor, string fname = null, string attach = null, string attaching_format = null) //Ramya 18-11-2019 
         {
             try
             {
                 Session["filenameWO"] = null;
                 Session["uploadforWO"] = null;
                 //Session["fileSaved"] = null;
                 foreach (string file in Request.Files)
                 {
                     var fileContent = Request.Files[file];
                     if (fileContent != null && fileContent.ContentLength > 0)
                     {
                         string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                         string[] attaching_fileformat = attaching_format.Split(',');
                         if (attaching_fileformat.Contains(fileextension.ToLower()))
                         {
                             var stream = fileContent.InputStream;
                             byte[] bytFile = null;
                             using (var memoryStream = new MemoryStream())
                             {
                                 stream.CopyTo(memoryStream);
                                 bytFile = memoryStream.ToArray();
                             }
                             bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                             if (isEXE == false)
                             {
                                 HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                                 Session["filenameWO"] = hpfBase;
                                 Session["uploadforWO"] = uploadfor;
                                 //Session["fileSaved"] = "Success";
                             }
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 Response.StatusCode = (int)HttpStatusCode.BadRequest;
                 Json("Upload failed");
             }
         }

         [HttpPost]
         public void UploadedFilesold(string uploadfor, string fname = null)
         {
             try
             {
                 Session["filenameWO"] = null;
                 string filename = "";

                 foreach (string file in Request.Files)
                 {
                     var fileContent = Request.Files[file];

                     if (fileContent != null && fileContent.ContentLength > 0)
                     {

                         if (fname != null && fname.Trim() != "")
                         {
                             filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
                         }
                         else
                         {
                             if (uploadfor != null)
                             {
                                 filename = objCmnFunctions.GetSequenceNo(uploadfor);
                             }
                         }
                         var fileextension = Path.GetExtension(fileContent.FileName);
                         var stream = fileContent.InputStream;
                         byte[] bytFile = null;
                         using (var memoryStream = new MemoryStream())
                         {
                             stream.CopyTo(memoryStream);
                             bytFile = memoryStream.ToArray();
                         }
                         string result = "";
                         if (!string.IsNullOrEmpty(filename))
                         {
                             filename = filename + fileextension;
                             Session["filenameWO"] = filename;
                             var FileString = Convert.ToBase64String(bytFile);
                             FileServer objserver = new FileServer();
                             // string result = objserver.SaveFiles(bytFile, filename);
                             result = objserver.SaveFileToServer(FileString, filename).Result;
                         }

                         if (result == "Success")
                         {
                             Session["fileSaved"] = "Success";
                         }
                         else
                         {
                             Session["fileSaved"] = "Failed";
                         }
                     }
                 }
                 //  return Json(filename);
             }
             catch (Exception ex)
             {
                 Response.StatusCode = (int)HttpStatusCode.BadRequest;
                 Json("Upload failed");
             }
         }

         [HttpPost]
         public JsonResult CreateWOAttachment(Attachments WOAttachmentInsert)
         {
             string data0 = string.Empty;
             string ErrorMsg = string.Empty;
             string filename = "";
             try
             {


                 ErrorMsg = "1";
                 if (Session["filenameWO"] != null && Session["uploadforWO"] != null)
                 {
                     filename = objCmnFunctions.GetSequenceNo(Session["uploadforWO"].ToString());

                     HttpPostedFileBase _attFile = Session["filenameWO"] as HttpPostedFileBase;
                     var fileextension = "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1];
                     var stream = _attFile.InputStream;
                     var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                     var path = Path.Combine(CmnFilePath, filename + fileextension);
                     using (var fileStream = System.IO.File.Create(path))
                     {
                         stream.CopyTo(fileStream);
                     }
                     filename = filename + fileextension;
                     //Session["filenamePR"] = filename;
                     byte[] bytFile = System.IO.File.ReadAllBytes(path);
                     string Filestring = Convert.ToBase64String(bytFile);
                     FileServer objserver = new FileServer();
                     string result = objserver.SaveFileToServer(Filestring, filename).Result;
                     if (result == "Success")
                     {
                         WOAttachmentInsert.TempFileName = filename;
                         int data = objModel.CreateWOAttachment(WOAttachmentInsert);
                         if (data > 0)
                         {
                             ErrorMsg = "0";
                         }
                     }
                 }
                 else
                 {
                     ErrorMsg = "2";
                 }
                 return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 ErrorMsg = "1";
                 return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
         }

         public JsonResult CreateWOAttachmentold(Attachments WOAttachmentInsert)
         {
             string data0 = string.Empty;
             string ErrorMsg = string.Empty;
             try
             {


                 ErrorMsg = "1";
                 if (Session["filenameWO"] != null)
                 {
                     if (Session["fileSaved"] == "Success")
                     {
                         WOAttachmentInsert.TempFileName = (string)Session["filenameWO"];
                         objModel.CreateWOAttachment(WOAttachmentInsert);
                         ErrorMsg = "0";
                     }
                     else
                     {
                         ErrorMsg = "1";
                     }
                 }
                 return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 ErrorMsg = "1";
                 return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
         }

         public JsonResult GetWOAttachments(int WOHeaderGId = 0)
         {
             string Data1 = string.Empty;
             string ErrorMsg = string.Empty;
             try
             {
                 ErrorMsg = "0";
                 DataSet ds = new DataSet();
                 DataTable dt;
                 ds = objModel.GetWOAttachments(WOHeaderGId);

                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                 return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);


             }
             catch (Exception ex)
             {
                 ErrorMsg = "1";
                 return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
         }

         public FileResult DownloadDocument(string filename = "", string actualfilename = "")
         {
             string ErrorMsg = string.Empty;
             try
             {

                 //string filename = filename;
                 byte[] fileBytes = null;
                 FileServer fileserver = new FileServer();

                 var result = fileserver.DownloadFile(filename).Result;
                 if (result != "Fail")
                 {
                     fileBytes = Convert.FromBase64String(result);
                     return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, actualfilename);
                 }
                 else
                 {
                     return File("", "");
                 }
             }
             catch (Exception ex)
             {
                 objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                 throw ex;
             }
         }

         public JsonResult DeleteAttachment(string id)
         {
             string Data1 = string.Empty;
             string ErrorMsg = string.Empty;
             ErrorMsg = "1";
             int result = objModel.DeleteWOAttachment(id);
             if (result > 0)
             {
                 ErrorMsg = "0";
                 return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
             else
             {
                 return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
             }
         }


        //Dhasarathan
         #endregion
        // Correction By Sugumar for GST
        #region
         [HttpPost]
         public JsonResult GetVendorAddress(string VendorId)
         {
             try
             {
                 string Data1 = "";
                 DataSet ds = db.GetVendorAddressDetails(VendorId);
                 DataTable dt = new DataTable();
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                 return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
             }
             catch
             {
                 return null;
             }
         }
        #endregion
        #region Get PO GstTax Details
         public JsonResult GetTaxdetails(string PODetailGId, string ProvLocId, string Baseamt, string ReciverLocId, string HSN_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.Getgsttax(PODetailGId, ProvLocId, Baseamt, ReciverLocId, HSN_ID);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region SetPoGstdetails
        public JsonResult SetPoGstDet(string POHeaderGId, string PODetilsId, string ProvLocationId, string ReceiverLocationId, string HsnId, string TaxsubTypeId, string TaxablAmt, string TaxRate, string NetAmt, string Vendorid, string TaxAmt)
        {
            DataTable dt = new DataTable();
            try
            {
                string Data1 = "";
                DataSet ds = db.SavepoGstdetails(POHeaderGId, PODetilsId, ProvLocationId, ReceiverLocationId, HsnId, TaxsubTypeId, TaxablAmt, TaxRate, NetAmt, Vendorid, TaxAmt);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
