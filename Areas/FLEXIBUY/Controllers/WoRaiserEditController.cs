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
    public class WoRaiserEditController : Controller
    {
        //
        // GET: /FLEXIBUY/WoRaiser/
        ErrorLog objErrorLog = new ErrorLog();
        ForMailController mailsender = new ForMailController();
        private Irepositorypr objModel;
        int count = 0;
        public WoRaiserEditController()
            : this(new fb_DataModelpr())
        {

        }
        public WoRaiserEditController(Irepositorypr objM)
        {
            objModel = objM;
        }

        public ActionResult WoEdit()
        {
            woraiser obj = new woraiser();
            try
            {
                if (Session["objwoList"] != null)
                {
                    obj = (woraiser)Session["objwoList"];
                }
                obj.templateList = new SelectList(objModel.GetTemplateListWO(), "templateGid", "tempName");
                obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername");
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", obj.monthId);
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", obj.monthId1);
                return View("WoEdit", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View("WoEdit", obj);
            }
        }
        [HttpGet]
        public PartialViewResult WoEditDetails(woraiser objraiser)
        {
            woraiser obj = new woraiser();
            try
            {
                Session["viewfor"] = null;
                Session["wodetail"] = null;
                Session["wodetailtemp"] = null;
                if (objraiser.viewfor == "edit")
                {
                    Session["viewfor"] = "edit";
                    // ViewBag.viewfor = "edit";
                }
                else if (objraiser.viewfor == "view")
                {
                    Session["viewfor"] = "view";
                    //ViewBag.viewfor = "view";
                }
                else if (objraiser.viewfor == "cancel")
                {
                    Session["viewfor"] = "cancel";
                    //ViewBag.viewfor = "cancel";
                }
                else if (objraiser.viewfor == "delete")
                {
                    Session["viewfor"] = "delete";
                    //ViewBag.viewfor = "delete";
                }
                else if (objraiser.viewfor == "checkercancel")
                {
                    Session["viewfor"] = "checkercancel";
                }
                else if (objraiser.viewfor == "checker")
                {
                    Session["viewfor"] = "checker";
                }
                else if (objraiser.viewfor == "closure")
                {
                    Session["viewfor"] = "closure";
                }
                else if (objraiser.viewfor == "closurechecker")
                {
                    Session["viewfor"] = "closurechecker";
                }
                else if (objraiser.viewfor == "amend")
                {
                    Session["viewfor"] = "amend";
                }
                else if (objraiser.viewfor == "delmat")
                {
                    Session["viewfor"] = "delmat";
                }


                DataTable objtable = objModel.GetWoDetails(objraiser.woheadGid);
                Session["wodetail"] = objtable;
                obj.obflist = objModel.GetWoEditList(objtable);
                obj.worefno = obj.obflist[0].worefno;
                obj.wodate = obj.obflist[0].wodate;
                //obj.woenddate = obj.obflist[0].woenddate;
                obj.woheadGid = objraiser.woheadGid;
                obj.woCancelGid = objraiser.woCancelGid;
                obj.woClosureGid = objraiser.woClosureGid;
                obj.remarks = objraiser.remarks;
                //obj.cbfheadAmount = cbfamount;
                //obj.uomgid = obj.obflist[0].uomgid;
                obj.vendorgid = obj.obflist[0].vendorgid;
                obj.prodservgid = obj.obflist[0].prodservgid;
                obj.total = obj.obflist[0].total;
                obj.requestforgid = obj.obflist[0].requestforgid;
                obj.department = obj.obflist[0].department;
                obj.vendorName = obj.obflist[0].vendorName;
                obj.vendorNote = obj.obflist[0].vendorNote;
                obj.freqId = obj.obflist[0].freqId;
                obj.monthName = obj.obflist[0].monthName;
                obj.monthId = objModel.GetMonthId(obj.monthName);
                obj.monthName1 = obj.obflist[0].monthName1;
                obj.monthId1 = objModel.GetMonthId(obj.monthName1);
                // obj.projmanagergid = obj.obflist[0].projmanagergid;
                obj.templateGid = obj.obflist[0].templateGid;
                obj.tempName = obj.obflist[0].tempName;
                obj.templname = obj.obflist[0].templname;
                obj.additionTemplate = obj.obflist[0].additionTemplate;
                obj.overTotal = obj.obflist[0].overTotal;
                obj.itType = obj.obflist[0].itType;
                //obj.devamount = obj.objlist[0].devamount;
                obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", obj.monthId);
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", obj.monthId1);
                // obj.projmanagerList = new SelectList(objModel.GetProjectManagerList(), "projmanagergid", "projmanagername",obj.projmanagergid);
                obj.raisedby = objModel.GetLoginUser();
                Session["objwoList"] = obj;
                return PartialView("WoEdit", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoEdit", obj);
            }
        }

        [HttpGet]
        public PartialViewResult CreatePoTemplate()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult SavePoTemplate(PoTemplate objTemplate)
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
        //[HttpGet]
        //public PartialViewResult GetWoSplit()
        //{
        //    woraiser obj = new woraiser();
        //    if (Session["objflist"] != null)
        //        obj.obflist = (List<woraiser>)Session["objflist"];
        //    obj.overTotal = obj.obflist[0].overTotal;
        //    Session["overTotal"] = obj.overTotal;
        //    //obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
        //    //obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName");
        //    //obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1");
        //    return PartialView("WoDetails", obj);
        //}
        //[HttpPost]
        //public PartialViewResult GetWoSplit(woraiser objraiser)
        //{
        //    DataTable dt = new DataTable();
        //    int freqMonth = 0;
        //    try
        //    {
        //        if (objraiser.freqId != 0)
        //        {
        //            freqMonth = objModel.GetFreqMonth(objraiser.freqId);
        //            objraiser.monthId1 = 0;
        //            objraiser.monthId1 = (Convert.ToInt32(objraiser.monthId) + Convert.ToInt32(freqMonth - 1));
        //            objraiser.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", Convert.ToInt32(objraiser.monthId1));
        //        }

        //        string month = string.Empty;
        //        // woraiser obj = new woraiser();

        //        if (Session["wodetail"] != null)
        //            dt = (DataTable)Session["wodetail"];
        //        if (!dt.Columns.Contains("service_month"))
        //            dt.Columns.Add("service_month");
        //        objraiser.obflist = objModel.GetWoSplitList(dt, objraiser.monthId, objraiser.monthId1);
        //        objraiser.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
        //        objraiser.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName");
        //        Session["objflist"] = objraiser.obflist;
        //        return PartialView("WoEditDetails", objraiser);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                return PartialView("WoEditDetails", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoEditDetails", obj);
            }
        }
        [HttpPost]
        public PartialViewResult WoRaiserTempSave(woraiser objraiser)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                if (Session["wodetail"] != null)
                {
                    dt1 = (DataTable)Session["wodetail"];
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["podetails_gid"].ToString() == objraiser.podetailsgid.ToString() && dt1.Rows[i]["podetails_serv_month1"].ToString() == objraiser.serviceMonth.ToString())
                        {
                            dt1.Rows[i]["podetails_perce"] = objraiser.percentage;
                            dt1.Rows[i]["podetails_total"] = objraiser.total;
                            dt1.Rows[i]["cbfdetails_totalamt"] = objraiser.grandTotal;
                            dt1.Rows[i]["podetails_desc"] = objraiser.serviceDesc;
                        }
                    }
                }
                Session["wodetailtemp"] = dt1;
                objraiser.obflist = objModel.GetWoEditTempList(dt1);
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
                return PartialView("WoEditDetails", objraiser);
                //return Json(obj.objlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoEditDetails", objraiser);
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
                objwoheader.result = objModel.UpdateWo(objwoheader, objTable);

                if (objwoheader.result == "Updated Successfully")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    int refgid = objforMail.GetRefGidForMail();
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
                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                    return Json(objwoheader.result, JsonRequestBehavior.AllowGet);
                }
                else if (objwoheader.result == "" || objwoheader.result==null)
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
        public JsonResult WoSummaryDelete(woraiser objdel)
        {
            try
            {
                if (objdel.woheadGid != null)
                    objdel.result = objModel.DeleteWoSummaryDetails(objdel);
                if (objdel.result != null)
                {
                    return Json(objdel, JsonRequestBehavior.AllowGet);
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
        public JsonResult WoSummaryCancel(woraiser objdel)
        {
            try
            {
                if (objdel.woheadGid != null)
                    objdel.result = objModel.CancelWoSummaryDetails(objdel);
                if (objdel.result != null)
                {
                    return Json(objdel, JsonRequestBehavior.AllowGet);
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

        public ActionResult WoEditRT() 
        {
            woraiser obj = new woraiser();
            try
            {
                if (Session["objwoList"] != null)
                {
                    obj = (woraiser)Session["objwoList"];
                }
                obj.templateList = new SelectList(objModel.GetTemplateListWO(), "templateGid", "tempName");
                obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername");
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", obj.monthId);
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", obj.monthId1);
                return View("WoEditRT", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View("WoEditRT", obj);
            }
        } 
        [HttpGet]
        public PartialViewResult WoEditDetailsRT(string id)
        {
            woraiser objraiser = new woraiser();
            objraiser.woheadGid = Convert.ToInt32(id);
            woraiser obj = new woraiser();
            try
            {
                Session["viewfor"] = null;
                Session["wodetail"] = null;
                Session["wodetailtemp"] = null;
              
                Session["viewfor"] = "view";

                DataTable objtable = objModel.GetWoDetails(objraiser.woheadGid);
                Session["wodetail"] = objtable;
                obj.obflist = objModel.GetWoEditList(objtable);
                obj.worefno = obj.obflist[0].worefno;
                obj.wodate = obj.obflist[0].wodate;
                //obj.woenddate = obj.obflist[0].woenddate;
                obj.woheadGid = objraiser.woheadGid;
                obj.woCancelGid = objraiser.woCancelGid;
                obj.woClosureGid = objraiser.woClosureGid;
                obj.remarks = objraiser.remarks;
                //obj.cbfheadAmount = cbfamount;
                //obj.uomgid = obj.obflist[0].uomgid;
                obj.vendorgid = obj.obflist[0].vendorgid;
                obj.prodservgid = obj.obflist[0].prodservgid;
                obj.total = obj.obflist[0].total;
                obj.requestforgid = obj.obflist[0].requestforgid;
                obj.department = obj.obflist[0].department;
                obj.vendorName = obj.obflist[0].vendorName;
                obj.vendorNote = obj.obflist[0].vendorNote;
                obj.freqId = obj.obflist[0].freqId;
                obj.monthName = obj.obflist[0].monthName;
                obj.monthId = objModel.GetMonthId(obj.monthName);
                obj.monthName1 = obj.obflist[0].monthName1;
                obj.monthId1 = objModel.GetMonthId(obj.monthName1);
                // obj.projmanagergid = obj.obflist[0].projmanagergid;
                obj.templateGid = obj.obflist[0].templateGid;
                obj.tempName = obj.obflist[0].tempName;
                obj.templname = obj.obflist[0].templname;
                obj.additionTemplate = obj.obflist[0].additionTemplate;
                obj.overTotal = obj.obflist[0].overTotal;
                obj.itType = obj.obflist[0].itType;
                //obj.devamount = obj.objlist[0].devamount;
                obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
                obj.freqList = new SelectList(objModel.GetFreqList(), "freqId", "freqName");
                obj.monthList = new SelectList(objModel.GetMonth(), "monthId", "monthName", obj.monthId);
                obj.monthList1 = new SelectList(objModel.GetMonth1(), "monthId1", "monthName1", obj.monthId1);
                // obj.projmanagerList = new SelectList(objModel.GetProjectManagerList(), "projmanagergid", "projmanagername",obj.projmanagergid);
                obj.raisedby = objModel.GetLoginUser();
                Session["objwoList"] = obj;
                return PartialView("WoEditRT", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("WoEditRT", obj);
            }
        }
    }
}
