using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class WoRaiserController : Controller
    {
        //
        // GET: /FLEXIBUY/WoRaiser/
        ErrorLog objErrorLog = new ErrorLog();
        ForMailController mailsender = new ForMailController();
        private Irepositorypr objModel;
        int count = 0;
        public WoRaiserController()
            : this(new fb_DataModelpr())
        {

        }
        public WoRaiserController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult WoRaiser()
        {
            Session["podetail"] = null;
            Session["details"] = null;
            Session["overTotal"] = null;
            return View();
        }

        [HttpGet]
        public PartialViewResult WoDetails(string function_param)
        {
            woraiser obj = new woraiser();
            try
            {
                //Session["poTempdetail"] = null;
                //Session["quant"] = null;

                DataTable objtable = new DataTable();
                //if (Session["podetail"] != null)
                //{
                //    objtable = (DataTable)Session["podetail"];
                //    obj.objlist = objModel.getpolist(objtable);
                //}

                string[] words = function_param.Split(',');
                foreach (string detgid in words)
                {
                    DataTable dt = objModel.getobfdetails(detgid);
                    objtable.Merge(dt);
                }
                Session["wodetail"] = objtable;
                obj.obflist = objModel.GetWoList(objtable);
                obj.wodate = DateTime.Now.ToShortDateString();
                //obj.cbfheadAmount = cbfamount;
                // obj.uomgid = obj.obflist[0].uomgid;
                obj.vendorgid = obj.obflist[0].vendorgid;
                obj.prodservgid = obj.obflist[0].prodservgid;
                obj.total = obj.obflist[0].total;
                obj.requestforgid = obj.obflist[0].requestforgid;
                obj.department = obj.obflist[0].department;
                obj.vendorName = obj.obflist[0].vendorName;
                //obj.devamount = obj.objlist[0].devamount;
                obj.templateList = new SelectList(objModel.GetTemplateListWO(), "templateGid", "tempName");
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName");
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1");
                // obj.projmanagerList = new SelectList(objModel.GetProjectManagerList(), "projmanagergid", "projmanagername");
                obj.raisedby = objModel.GetLoginUser();
                return PartialView("WoRaiser", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoRaiser", obj);
            }
        }

        [HttpGet]
        public PartialViewResult CreateWoTemplate()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult SaveWoTemplate(PoTemplate objTemplate)
        {
            try
            {
                objTemplate.result = objModel.InsertPoTemplate(objTemplate);
                if (objTemplate.result != null)
                {
                    return Json(objTemplate, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TemplateDetails(int id)
        {
            PoTemplate objTemplate = new PoTemplate();
            try
            {
                if (id != null)
                {
                    objTemplate.templateContent = objModel.GetTemplateContent(id);
                }
                return Json(objTemplate.templateContent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objTemplate.templateContent, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetTemplateList()
        {
            return Json(objModel.GetTemplateList(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetWoSplit()
        {
            woraiser obj = new woraiser();
            try
            {

                if (Session["objflist"] != null)
                    obj.obflist = (List<woraiser>)Session["objflist"];
                obj.overTotal = obj.obflist[0].overTotal;
                Session["overTotal"] = obj.overTotal;
                //obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                //obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName");
                //obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1");
                return PartialView("WoDetails", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoDetails", obj);
            }
        }
        [HttpPost]
        public PartialViewResult GetWoSplit(woraiser objraiser)
        {
            try { 
            DataTable dt = new DataTable();
            objraiser.freqId = objModel.GetFreqMonth(objraiser.freqId);
            // int freqMonth =0;5
           
                int actualMonth = (Convert.ToInt32(objraiser.monthId1) - Convert.ToInt32(objraiser.monthId)) + 1;
                int monthQ = (actualMonth / objraiser.freqId);
                int monthRem = (actualMonth % objraiser.freqId);
                if (monthRem > 0)
                {
                    monthQ = monthQ + 1;
                }
                //decimal monthSeparator = Math.Ceiling(monthRem);

                //if (objraiser.freqId != 0)
                //{
                //freqMonth = objModel.GetFreqMonth(objraiser.freqId);
                //int remMonth=Convert.ToInt32(objraiser.monthId1) - Convert.ToInt32(objraiser.monthId);
                // int freqMonth1=Convert.ToInt32(freqMonth-1);
                //objraiser.monthId1 = 0;
                //  objraiser.monthId1 = (Convert.ToInt32(objraiser.monthId) + Convert.ToInt32(freqMonth - 1));
                //if(remMonth<freqMonth)
                //{
                //    //string msg="Please Select Valid Month";
                //    objraiser.monthName = "Please Select Valid Month";
                //    return PartialView("WoDetails", objraiser.monthName);
                //}

                //}
                objraiser.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", Convert.ToInt32(objraiser.monthId1));
                string month = string.Empty;
                // woraiser obj = new woraiser();

                if (Session["wodetail"] != null)
                    dt = (DataTable)Session["wodetail"];
                if (!dt.Columns.Contains("service_month"))
                    dt.Columns.Add("service_month");
                // objraiser.obflist = objModel.GetWoSplitList(dt, objraiser.monthId, objraiser.monthId1);
                objraiser.obflist = objModel.GetWoSplitList(dt, monthQ, objraiser.monthId, objraiser.monthId1, objraiser.freqId);
                objraiser.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                objraiser.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName");
                Session["objflist"] = objraiser.obflist;
                return PartialView("WoDetails", objraiser);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoDetails", objraiser);
            }
           
        }
        [HttpGet]
        public PartialViewResult WoRaiserTempSave()
        {
            woraiser obj = new woraiser();
            try
            {
               
                if (Session["wodetailList"] != null)
                {
                    obj.obflist = (List<woraiser>)Session["wodetailList"];
                }
                if (Session["freqId"] != null)
                    obj.freqId = (int)Session["freqId"];
                if (Session["monthId"] != null)
                    obj.monthId = (int)Session["monthId"];
                if (Session["monthId1"] != null)
                    obj.monthId1 = (int)Session["monthId1"];
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName", obj.freqId);
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", obj.monthId);
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", obj.monthId1);
                return PartialView("WoDetails", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoDetails", obj);
            }
        }
        [HttpPost]
        public PartialViewResult WoRaiserTempSave(woraiser objraiser)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                if (Session["WoSplitTable"] != null)
                {
                    dt1 = (DataTable)Session["WoSplitTable"];
                    if (!dt1.Columns.Contains("percentage") && !dt1.Columns.Contains("total"))
                    {
                        dt1.Columns.Add("percentage");
                        dt1.Columns.Add("total");
                    }
                    // dt1.Columns.Add("grandTotal");
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        if (dt1.Rows[i]["cbfdetails_gid"].ToString() == objraiser.obfdetailsgid.ToString() && dt1.Rows[i]["serviceMonth"].ToString() == objraiser.serviceMonth.ToString())
                        {
                            dt1.Rows[i]["percentage"] = objraiser.percentage;
                            dt1.Rows[i]["total"] = objraiser.total;
                            dt1.Rows[i]["cbfdetails_totalamt"] = objraiser.grandTotal;
                            dt1.Rows[i]["prodservice_description"] = objraiser.serviceDesc;
                        }
                    }
                }
                Session["wodetailtemp"] = dt1;
                objraiser.obflist = objModel.GetWoTempList(dt1);
                Session["wodetailList"] = objraiser.obflist;
                // obj.department = obj.objlist[0].department;
                objraiser.vendorName = objraiser.obflist[0].vendorName;
                // obj.devamount = obj.objlist[0].devamount;
                // Session["details"] = obj;
                // Session["cbfprdeta"] = objgetprdetails;
                //return json(1, jsonrequestbehavior.allowget);
                Session["freqId"] = objraiser.freqId;
                Session["monthId"] = objraiser.monthId;
                Session["monthId1"] = objraiser.monthId1;
                objraiser.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName", objraiser.freqId);
                objraiser.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", objraiser.monthId);
                objraiser.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", objraiser.monthId1);
                return PartialView("WoDetails", objraiser);
                //return Json(obj.objlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoDetails", objraiser);
            }
        }
        public JsonResult SaveWoDetails(woraiser objwoheader)
        {
            try
            {
                DataTable objTable = new DataTable();
                if (Session["wodetailtemp"] != null)
                {
                    objTable = (DataTable)Session["wodetailtemp"];
                }
                else
                {
                    if (Session["wodetail"] != null)
                        objTable = (DataTable)Session["wodetail"];
                }
                objwoheader.result = objModel.InsertWo(objwoheader, objTable);

                if (objwoheader.result == "Inserted Successfully")
                {
                    if (objwoheader.actionName == "Submit")
                    {
                        CbfSumModel objforMail = new CbfSumModel();
                        int refgid = objforMail.GetRefGidForMail("WO");
                        string reqstatus = objforMail.GetRequestStatus(refgid, "WO");
                        int queuegid = objforMail.GetQueueGidForMail(refgid, "WO");
                        string curapprovalstage = "0";
                        if (reqstatus == "S")
                        {
                            curapprovalstage = "0";
                        }
                        else
                        {
                            curapprovalstage = "1";
                        }
                        mailsender.sendusermail("FB", "PO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                    }
                    return Json(objwoheader.result, JsonRequestBehavior.AllowGet);
                }
                else if (objwoheader.result == "")
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
