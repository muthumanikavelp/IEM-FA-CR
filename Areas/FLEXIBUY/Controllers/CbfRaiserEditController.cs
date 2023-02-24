using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using IEM.Common;
using System.Net;
using System.Data;
using IEM.Areas.FLEXIBUY.Models;
using System.Net.Mime;
using System.Collections;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CbfRaiserEditController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        ArrayList lalCbfdetails = new ArrayList();
        public CbfRaiserEditController()
            : this(new CbfSumModel())
        {

        }
        public CbfRaiserEditController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        [HttpGet]
        public ActionResult CbfRaiserEdit(int lnid, string lsviewfor)
        {
           
            Session["pardetailstemp"] = null;
            Session["pardetails_gid"] = null;
            Session["parcbfdetails"] = null;
            Session["cbfdeailsedit"] = null;
            Session["cbfAttachment"] = null;
            Session["cbfAttachmentDetails"] = null;
            Session["prdetails_gid"] = null;
            Session["Arraylistattachment"] = null;
            Session["objowner"] = null;
            Session["parheadersession"] = null;
            Session["ArrayList"] = null;
            Session["count"] = null;
            Session["count1"] = null;
            Session["cbfprdetailcbf"] = null;
            Session["attachmentdetails"] = null;
            Session["attachment"] = null;
            Session["AccessTokenheader"] = null;
            Session["AccessToken"] = null;
            Session["cbfgid"] = null;
            Session["cbfprdeta"] = null;
            Session["prheader"] = null;
            Session["da"] = null;
            Session["cbfprdetails"] = null;
            Session["cbfgidforpar"] = null;
            if (lsviewfor == "edit")
            {
                Session["viewfor"] = "edit";
                ViewBag.viewfor = "edit";
            }
            else if (lsviewfor == "view")
            {
                Session["viewfor"] = "view";
                ViewBag.viewfor = "view";
            }
            else if (lsviewfor == "delete")
            {
                Session["viewfor"] = "delete";
                ViewBag.viewfor = "delete";
            }
            else if (lsviewfor == "cancel")
            {
                Session["viewfor"] = "cancel";
                ViewBag.viewfor = "cancel";
            }
            //else if (lsviewfor=="delmat")
            //{
            //    Session["viewfor"] = "delmat";
            //    ViewBag.viewfor = "delmat";
            //}
            CbfSumEntity objDetails = new CbfSumEntity();
            string lsNumber = lnid.ToString();
            Session["cbfgid"] = lsNumber;
            if(lsNumber !=null && lsNumber !="")
            {
                Session["cbfgidforpar"] = lsNumber;
            }
            CbfRaiseHeader objCrheader = new CbfRaiseHeader();
            objDetails = objRep.Getcbfaiseredit(lsNumber);
            if (objDetails.cbdRasieobj != null)
            {
                if (objDetails.cbdRasieobj.branchcodeGid != 0)
                {
                    objDetails.cbfbranchName = objRep.GetBranchNameForEdit(objDetails.cbdRasieobj.branchcodeGid);
                }
            }
          
            objDetails.productGroupList = new SelectList(objRep.Getproductlist(), "productgroupid", "productgroupidname");
            objDetails.uomList = new SelectList(objRep.GetUomList(), "uomgid", "uomcode");
            objDetails.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname", objDetails.cbdRasieobj.requeestForGid);
            objDetails.projectOwner1 = new SelectList(objRep.GetList(), "projectownergid", "projectownername", objDetails.cbdRasieobj.projectOwnerGid);
            //objDetails.branchCode1 = new SelectList(objRep.GetBranchCode(), "branchcodegid", "branchcodename", objDetails.cbdRasieobj.branchcodeGid);
            objCrheader.BudgetownerGid = objRep.GetBudgetownerGid(objDetails.cbdRasieobj.BudgetownerGid.ToString());
            objDetails.BudgetOwner = new SelectList(objRep.GetBudgetowner(), "BudgetownerGid", "BudgetOwner_Name", objCrheader.BudgetownerGid);
            Session["prheader"] = objDetails.cbfPrheader;
            Session["cbfraiseredit"] = objDetails;
            Session["Details"] = objDetails.cbdRasieobj.requeestForGid;
            return View(objDetails);
        }
        public PartialViewResult EditPrHeader(string lsListFor)
        {
            try
            {
                CbfSumEntity objDetails = new CbfSumEntity();
                if (Session["viewfor"].ToString() == "edit")
                {
                    objDetails = (CbfSumEntity)Session["Details"];
                    if (lsListFor == "search")
                    {
                        if (Session["objowner"] != null)
                            objDetails = (CbfSumEntity)Session["objowner"];
                        if (objDetails.cbfPrheader.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                    else if (lsListFor == "refresh")
                    {
                        if (Session["objowner"] != null)
                            objDetails = (CbfSumEntity)Session["objowner"];
                        if (objDetails.cbfPrheader.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }


                }
                else
                {
                    objDetails = (CbfSumEntity)Session["prheader"];

                }
                return PartialView(objDetails);
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpGet]
        public PartialViewResult CbfRefresh(string listfor)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["prheadersession"] != null)
                {
                    objSumEntity = (CbfSumEntity)Session["prheadersession"];

                }
            }
            else
            {
                Session["prheadersession"] = "";
                objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Details"]));
            }
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("EditPrHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfRefresh()
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Details"]));
            Session["prheadersession"] = objSumEntity;
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return Json(objSumEntity, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult CbfSearch(string listfor)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["prheadersession"] != null)
                {
                    objSumEntity = (CbfSumEntity)Session["prheadersession"];
                }
            }
            else
            {
                Session["prheadersession"] = "";
                objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Details"]));
            }
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("EditPrHeader", objSumEntity);
        }
        [HttpGet]
        public PartialViewResult EditParHeader(string isBudgeted=null)
        {
            try
            {
                if (isBudgeted == "yes")
                {
                    TempData["isBudgeted"] = "yes";
                    Session["objowner"] = null;
                }
                else if (isBudgeted == "no")
                {
                    TempData["isBudgeted"] = "no";
                    Session["objowner"] = null;
                }
                else
                {
                    TempData["isBudgeted"] = "pr";
                }
                CbfSumEntity objDetails = new CbfSumEntity();
                //objDetails = (CbfSumEntity)Session["prheader"];
              
                return PartialView(objDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult EditPrDetails()
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            if (Session["viewfor"].ToString() == "edit")
            {
                objDetails = (CbfSumEntity)Session["prd"];
            }
            else
            {
                objDetails = (CbfSumEntity)Session["prheader"];
            }
            return PartialView(objDetails);

        }
        [HttpPost]
        public PartialViewResult EditPrDetails(CbfPrHeader objPrhegid)
        {
            try
            {

                CbfSumEntity objDetails = new CbfSumEntity();
                if (Session["viewfor"].ToString() == "edit")
                {
                    Session["idprdetails"] = objPrhegid.prhed_Gid;
                    objDetails = objRep.GetPrDetailEdit(objPrhegid);
                    Session["prd"] = objDetails;

                }
                else
                {
                    objDetails = (CbfSumEntity)Session["prheader"];
                }

                return PartialView("EditPrDetails", objDetails);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        [HttpGet]
        public PartialViewResult EditCbfDetails()
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            objGetPrDetails = (CbfSumEntity)Session["cbfprdeta"];

            return PartialView("EditCbfDetails", objGetPrDetails);
        }


        [HttpPost]
        public PartialViewResult EditCbfDetails(CbfDetails objPrDelGid, CbfDetails objUnchecked1)
        {
            try
            {
                Session["prdetails_gid"] = null;
                DataTable objDtcbfdetails = new DataTable();
                DataTable objDtsession = new DataTable();
                CbfSumEntity objgetprdetails = new CbfSumEntity();
                Session["da"] = Session["cbfdeailsdt"];
                if (Session["da"] != null)
                    objDtsession = (DataTable)Session["da"];

                if (objPrDelGid.prdetailsGid != 0)
                {
                    if (objDtsession.Columns.Count > 0)
                    {
                        DataView dv = new DataView();
                        dv = objDtsession.DefaultView;
                        dv.RowFilter = "prdetails_gid =" + objUnchecked1.prdetailsGid + "";
                        objDtcbfdetails = dv.ToTable();
                        //if (objDtsession.Columns.Contains("remarks"))
                        //{

                        //    DataRow[] matches = objDtsession.Select("remarks is null");
                        //    foreach (DataRow row in matches)
                        //    {
                        //        objDtsession.Rows.Remove(row);
                        //    }
                        //}
                    }
                    if (objDtcbfdetails.Rows.Count == 0)
                    {
                        objDtcbfdetails = null;
                        objDtcbfdetails = objRep.DtTable(objPrDelGid);
                        if (objDtsession.Columns.Contains("remarks"))
                        {
                            for (int i = 0; i < objDtsession.Rows.Count; i++)
                            {
                                if (objDtsession.Rows[i]["fccc"].ToString() == "")
                                {
                                    objDtsession.Rows.RemoveAt(i);
                                }

                            }
                            if (objDtcbfdetails.Rows.Count > 0)
                            {

                                objDtsession.Merge(objDtcbfdetails);
                            }
                            Session["da"] = objDtsession;
                        }
                        else
                        {
                            //Session["da"] = objDtcbfdetails;
                            //objDtsession = objDtcbfdetails;
                          
                            objDtsession.Merge(objDtcbfdetails);
                            Session["da"] = objDtsession;
                        }


                    }

                    Session["prdetails_gid"] = objUnchecked1.prdetailsGid;
                    objgetprdetails = objRep.GetCbfDetails(objDtsession);
                }
                else if (objUnchecked1.unChecked12 != 0)
                {
                    DataView dv = new DataView();
                    dv = objDtsession.DefaultView;

                    dv.RowFilter = "prdetails_gid =" + objUnchecked1.unChecked12 + "";
                    objDtcbfdetails = dv.ToTable();

                    if (objDtcbfdetails.Rows[0]["remarks"].ToString() != null && objDtcbfdetails.Rows[0]["remarks"].ToString() != "")
                    {
                        objgetprdetails = objRep.GetCbfDetails(objDtsession);
                    }
                    else
                    {
                        Session["da"] = objDtsession;
                        DataRow[] matches = objDtsession.Select("prdetails_gid='" + objUnchecked1.unChecked12 + "'");
                        foreach (DataRow row in matches)
                        {
                            objDtsession.Rows.Remove(row);
                        }
                        objgetprdetails = objRep.GetCbfDetails(objDtsession);
                    }


                }
                Session["da"] = objDtsession;
                Session["cbfprdeta"] = objgetprdetails;
                return PartialView("EditCbfDetails", objgetprdetails);

            }

            catch (Exception ex)
            {
                throw ex;

            }
        }
        public PartialViewResult CbfUpdate(CbfDetails Usermodel)
        {
            try
            {

                CbfSumEntity objGetPrdetails = new CbfSumEntity();
                objGetPrdetails.cbfDetails = new List<CbfDetails>();
                CbfDetails Cbfdetails;
                //  CbfSumEntity cbf = (CbfSumEntity)Session["cbfprdetails"];
                if (Session["da"] == null)
                {
                    Session["da"] = Session["cbfdeailsdt"];
                }
                DataTable dtCbfdetails = (DataTable)Session["da"];
                if (dtCbfdetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCbfdetails.Rows.Count; i++)
                    {
                        if (Usermodel.cbfDetGid.ToString() == dtCbfdetails.Rows[i]["cbfdetails_gid"].ToString())
                        {
                            dtCbfdetails.Rows[i]["prdetails_qty"] = Usermodel.qty;
                            dtCbfdetails.Rows[i]["pipinput_rate"] = Usermodel.unitAmt;
                            dtCbfdetails.Rows[i]["pipinput_costestimation"] = Usermodel.total;
                            dtCbfdetails.Rows[i]["remarks"] = Usermodel.remarks;
                            dtCbfdetails.Rows[i]["fccc"] = Usermodel.fccc;
                            dtCbfdetails.Rows[i]["budgetline"] = Usermodel.budgetLine;

                        }
                    }
                }
                objGetPrdetails = objRep.GetCbfDetails(dtCbfdetails);
                Session["cbfprdeta"] = objGetPrdetails;
                return PartialView("EditCbfDetails", objGetPrdetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CreateSerachEdit()
        {
            string lsId = Convert.ToString(Request.QueryString["id"]);
            CbfSumEntity objGetPrdetails = new CbfSumEntity();

            objGetPrdetails = objRep.GetFccc();
            objGetPrdetails.des = lsId;
            Session["des"] = lsId;
            return PartialView(objGetPrdetails);

        }
        [HttpPost]
        public JsonResult Create(CbfRaiseHeader objCbfRaiser)
        {
            try
            {
                string data = objRep.UpdateCfHeaderSave(objCbfRaiser);
                RedirectToAction("Index");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Submit(CbfRaiseHeader objCbfRaiser)
        {
            try
            {
                string data = objRep.UpdateCfHeaderSubmit(objCbfRaiser);
                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                if (Session["cbfgid"] != null)
                {
                    int refgid = Convert.ToInt32(Session["cbfgid"]);
                    string reqstatus = objMail.GetRequestStatus(refgid, "CBF");
                    int queuegid = objMail.GetQueueGidForMail(refgid, "CBF");
                    mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
                }
                
                RedirectToAction("Index");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CbfSearchPAR(string listfor = null)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["parheadersession"] != null)
                {
                    objSumEntity = (CbfSumEntity)Session["parheadersession"];
                }
            }
            else
            {
                Session["parheadersession"] = "";
                objSumEntity = objRep.GetParHeader();
            }
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("EditParHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfSearchPAR(CbfParHeader objParHeader,string listfor=null)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();

            string lsPrNo = objParHeader.parNo;
            string lsPrDate = objParHeader.parDate;
            string lsRequest1 = objParHeader.parAmt.ToString();
            string lsParBudget = objParHeader.par_Budget.ToString();
            //    objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");
            
            if (objParHeader.parDate != null)
                lsPrDate = objParHeader.parDate.ToString();
            if (objParHeader.parAmt != null)
                lsRequest1 = objParHeader.parAmt.ToString();
            if (objParHeader.par_Budget != null)
                lsParBudget = objParHeader.par_Budget.ToString();
            int cbfgid = 0;
            if (Session["cbfgid"] != null)
            {
                cbfgid = (Int32)Session["cbfgid"];
            }
            objSumEntity = objRep.GetParHeader(cbfgid,lsParBudget); 


            if (!(string.IsNullOrEmpty(lsPrNo)))
            {
                ViewBag.txtcbfno = lsPrNo.ToUpper();
                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsPrNo == null
                    || (x.parNo.Contains(lsPrNo))).ToList();
            }
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            Session["objowner"] = objSumEntity;
            if (!(string.IsNullOrEmpty(lsPrDate)))
            {
                ViewBag.txtcbfdate = lsPrDate;

                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsPrDate == null
                    || (x.parDate.Contains(lsPrDate))).ToList();
            }
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            Session["objowner"] = objSumEntity;
            if (!(string.IsNullOrEmpty(lsRequest1)) && lsRequest1.ToString() != "0")
            {
                ViewBag.txtcbfmode = lsRequest1;

                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsRequest1 == null
                    || (x.parAmt.Equals(lsRequest1))).ToList();
            }
            if (objSumEntity.cbfParheader.Count == 0)
                ViewBag.Message = "No records found";
            Session["objowner"] = objSumEntity;
            if ((string.IsNullOrEmpty(lsPrNo)) &&
                (string.IsNullOrEmpty(lsPrDate)) &&
                (string.IsNullOrEmpty(lsRequest1)))
                ViewBag.Error = "Please fill search Criteria";
            Session["parheadersession"] = objSumEntity;
            return Json(objSumEntity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult CbfRefreshPAR(string listfor)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["parheadersession"] != null)
                {
                    objSumEntity = (CbfSumEntity)Session["parheadersession"];
                }
            }
            else
            {
                Session["parheadersession"] = "";
                objSumEntity = objRep.GetParHeader();
            }
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("EditParHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfRefreshPAR()
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            objSumEntity = objRep.GetParHeader();

            Session["parheadersession"] = objSumEntity;
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return Json(objSumEntity, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult FcccSearchEdit(FcccMaster fcccheader)
        {
            CbfSumEntity objGetPrdetails = new CbfSumEntity();
            objGetPrdetails = objRep.GetFcccSerach(fcccheader);

            return PartialView("CreateSerachEdit", objGetPrdetails);
        }
        [HttpGet]
        public PartialViewResult CbfSaveEdit()
        {
            CbfSumEntity objGetPrdetails = new CbfSumEntity();
            objGetPrdetails = (CbfSumEntity)Session["cbfprdeta"];
            return PartialView(objGetPrdetails);
        }
        [HttpPost]
        public PartialViewResult CbfSaveEdit(CbfDetails UserModel)
        {
            try
            {
                DataTable objDtforCbfsave = new DataTable();
                DataTable objDtforCbfSavedata = new DataTable();
                CbfSumEntity objGetPrdetails = new CbfSumEntity();
                CbfSumEntity objGetPrdetailsTemp = new CbfSumEntity();
                if (Session["da"] != null)
                {
                    objDtforCbfSavedata = (DataTable)Session["da"];
                    if (objDtforCbfSavedata.Columns.Count <= 12)
                    {
                        objDtforCbfSavedata.Columns.Add("remarks", typeof(string));
                        objDtforCbfSavedata.Columns.Add("fccc", typeof(int));
                        objDtforCbfSavedata.Columns.Add("budgetline", typeof(int));
                        objDtforCbfSavedata.Columns.Add("myref", typeof(int));
                    }
                    for (int i = 0; i < objDtforCbfSavedata.Rows.Count; i++)
                    {
                        if (objDtforCbfSavedata.Rows[i]["prdetails_gid"].ToString() == UserModel.unChecked12.ToString())
                        {
                            objDtforCbfSavedata.Rows[i]["remarks"] = UserModel.remarks;
                            objDtforCbfSavedata.Rows[i]["prdetails_qty"] = UserModel.qty;
                            objDtforCbfSavedata.Rows[i]["pipinput_rate"] = UserModel.unitAmt;
                            objDtforCbfSavedata.Rows[i]["pipinput_costestimation"] = UserModel.total;
                            objDtforCbfSavedata.Rows[i]["fccc"] = UserModel.fccc;
                            objDtforCbfSavedata.Rows[i]["budgetline"] = UserModel.budgetLine;
                            //   objDtforCbfSavedata.Rows[i]["myref"] = Session["cbfgid"];

                        }
                    }
                }
                objGetPrdetailsTemp = objRep.GetCbfDetails(objDtforCbfSavedata);
                CbfSumEntity obj = new CbfSumEntity();
                Session["da"] = objDtforCbfSavedata;
                Session["cbfprdeta"] = objGetPrdetailsTemp;
                return PartialView("EditCbfDetails", objGetPrdetailsTemp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult CbfDelete(CbfDetails UserModel)
        {
            try
            {
                CbfSumEntity objgetcbfdetails = new CbfSumEntity();
                DataTable objDtCbfDetails = new DataTable();
                DataTable objDtCbfDetails1 = new DataTable();
                if (Session["da"] == null)
                {
                    Session["da"] = Session["cbfdeailsdt"];
                    objDtCbfDetails = (DataTable)Session["da"];
                }
                else
                {
                    objDtCbfDetails = (DataTable)Session["da"];
                }


                if (UserModel.cbfDetGid.ToString() != "" && UserModel.cbfDetGid.ToString() != "0" && UserModel.cbfDetGid.ToString() != null)
                {
                    for (int i = 0; i < objDtCbfDetails.Rows.Count; i++)
                    {
                        if (objDtCbfDetails.Rows[i]["cbfdetails_gid"].ToString() == UserModel.cbfDetGid.ToString())
                        {
                            objDtCbfDetails.Rows.RemoveAt(i);
                            if (Session["ArrayList"] != null)
                                lalCbfdetails = (ArrayList)Session["ArrayList"];
                            lalCbfdetails.Add(UserModel.cbfDetGid.ToString());
                            Session["ArrayList"] = lalCbfdetails;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < objDtCbfDetails.Rows.Count; i++)
                    {
                        if (objDtCbfDetails.Rows[i]["prdetails_gid"].ToString() == UserModel.unChecked12.ToString())
                            objDtCbfDetails.Rows.RemoveAt(i);
                    }

                }
                Session["da"] = objDtCbfDetails;
                objgetcbfdetails = objRep.GetCbfDetails(objDtCbfDetails);
                Session["cbfprdeta"] = objgetcbfdetails;

                return PartialView("EditCbfDetails", objgetcbfdetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpGet]
        public PartialViewResult EditCbfParDetails()
        {
            CbfSumEntity objgetprdetails = new CbfSumEntity();
            objgetprdetails = (CbfSumEntity)Session["cbfprdeta"];
            return PartialView("EditCbfParDetails", objgetprdetails);
        }


        [HttpPost]
        public PartialViewResult EditCbfParDetails(CbfDetails prdelgid)
        {
            CbfSumEntity objgetprdetails = new CbfSumEntity();
            if (Session["viewfor"].ToString() == "edit")
            {
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();

                if (Session["parcbfdetails"] != null)
                {
                    objdt1 = (DataTable)Session["parcbfdetails"];
                }
                else
                {
                    objdt1 = (DataTable)Session["cbfdeailsdt"];
                }
                if (objdt != null)
                    objdt1.Merge(objdt);
                if (Session["parcbfdetails"] != null)
                {
                    objgetprdetails = objRep.GetCbfParDetails(objdt1);
                }
                else
                {
                    objgetprdetails = objRep.GetCbfParDetails(objdt1);
                }
                Session["cbfprdeta"] = objgetprdetails;
                objgetprdetails.productGroupList = new SelectList(objRep.Getproductlist(),
                    "productGroupId", "productGroupIdName");
                objgetprdetails.uomList = new SelectList(objRep.GetUomList(), "uomGid", "uomCode");
                Session["pardetails_gid"] = prdelgid.productCode.ToString();

            }
            else
            {
                objgetprdetails = (CbfSumEntity)Session["prheader"];

            }

            return PartialView("EditCbfParDetails", objgetprdetails);
        }
        [HttpGet]
        public PartialViewResult EditParDetails()
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            if (Session["viewfor"].ToString() == "edit")
            {
                objDetails = (CbfSumEntity)Session["pardetailstemp"];
                return PartialView(objDetails);

            }
            else
            {
                objDetails = (CbfSumEntity)Session["prheader"];

            }
            return PartialView(objDetails);

        }

        [HttpPost]
        public PartialViewResult EditParDetails(CbfParHeader objpar)
        {
            try
            {
                CbfSumEntity objDetails = new CbfSumEntity();
                string viewfor = "";
                if (Session["viewfor"] != null)
                {
                    viewfor = (string)Session["viewfor"];
                }
                if (viewfor == "edit")
                {
                    objDetails = objRep.CbfPardetailsedit(objpar);
                    if (objDetails != null)
                    {
                        Session["pardetailstemp"] = objDetails;
                    }

                    return PartialView(objDetails);
                }
                else
                {
                    objDetails = (CbfSumEntity)Session["cbfraiseredit"];
                }
               
                return PartialView(objDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetProductGroup()
        {
            return Json(objRep.Getproductlist(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUom()
        {
            return Json(objRep.GetUomList(), JsonRequestBehavior.AllowGet);
        }

        //public PartialViewResult EditGetProductList(int lnid)
        //{

        //    CbfSumEntity objM = new CbfSumEntity();
        //    objM = objRep.GetProdServDetails(lnid);
        //    HttpContext.Session["ProductGroupgid"] = lnid;

        //    return PartialView(objM);
        //}
        [HttpGet]
        public PartialViewResult EditGetProductList(int lnid, string listfor = null)
        {
            CbfSumEntity objM = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["SearchcbfeditProd"] != null)
                    objM.cbfDetails = (List<CbfDetails>)Session["SearchcbfeditProd"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["prodcode"] != null)
                    TempData["code"] = TempData["prodcode"];
                if (TempData["prodname"] != null)
                    TempData["name"] = TempData["prodname"];
            }
            else
            {
                Session["SearchcbfProd"] = "";
                objM = objRep.GetProdServDetails(lnid);
            }
            return PartialView("EditGetProductList", objM);
        }

        [HttpPost]
        public PartialViewResult SearchCbfProd(CbfDetails objSearchProd)
        {
            try
            {
                int id = Convert.ToInt32(objSearchProd.productgid);
                // List<Product> objEmp = new List<Product>();
                CbfSumEntity objM = new CbfSumEntity();
                objM = objRep.GetProdServDetails(id);
                if (objM != null)
                {
                    if ((string.IsNullOrEmpty(objSearchProd.productCode)) == false)
                    {
                        TempData["prodcode"] = objSearchProd.productCode;
                        objM.cbfDetails = objM.cbfDetails.Where(x => objSearchProd.productCode == null ||
                            (x.productCode.ToUpper().Contains(objSearchProd.productCode.ToUpper()))).ToList();
                        Session["SearchcbfeditProd"] = objM.cbfDetails;
                    }
                    if ((string.IsNullOrEmpty(objSearchProd.productName)) == false)
                    {
                        TempData["prodname"] = objSearchProd.productName;
                        objM.cbfDetails = objM.cbfDetails.Where(x => objSearchProd.productName == null ||
                            (x.productName.ToUpper().Contains(objSearchProd.productName.ToUpper()))).ToList();
                        Session["SearchcbfeditProd"] = objM.cbfDetails;
                    }
                    if (objM.cbfDetails.Count == 0)
                    {
                        TempData["Norecords"] = "No records Found";
                    }
                }
                return PartialView("EditGetProductList", objM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult Editsave(CbfDetails pardetails)
        {

            try
            {
                CbfSumEntity objcbf = new CbfSumEntity();
                DataTable objDt = new DataTable();
                if (Session["parcbfdetails"] != null)
                {
                    objDt = (DataTable)Session["parcbfdetails"];
                }
                else
                {

                    objDt = (DataTable)Session["cbfdeailsdt"];

                }
                if (!objDt.Columns.Contains("sno"))
                {

                    objDt.Columns.Add("sno", typeof(int));
                    objDt.Columns.Add("productgid", typeof(string));
                    objDt.Columns.Add("productgroup", typeof(string));
                    objDt.Columns.Add("productcode", typeof(string));
                    objDt.Columns.Add("productname", typeof(string));
                    objDt.Columns.Add("uom", typeof(int));
                    objDt.Columns.Add("qty", typeof(int));
                    objDt.Columns.Add("unitamount", typeof(decimal));
                    objDt.Columns.Add("total", typeof(decimal));

                    objDt.Columns.Add("productdescription", typeof(string));
                    objDt.Columns.Add("chartofaccount", typeof(int));
                    objDt.Columns.Add("myref", typeof(int));

                }
                if (pardetails.unChecked12.ToString() == null || pardetails.unChecked12 == 0)
                {
                    int sno = Convert.ToInt32(Session["sno"]) + 1;
                    Session["sno"] = sno;
                    var row = objDt.NewRow();
                    row["sno"] = sno;
                    row["productgid"] = pardetails.productgid;
                    row["prdetails_gid"] = pardetails.prdetailsGid;
                    row["productgroup"] = pardetails.productGroupId;
                    row["productcode"] = pardetails.productCode;
                    row["productname"] = pardetails.productName;
                    row["productdescription"] = pardetails.description;
                    row["uom"] = pardetails.uomGid;
                    row["qty"] = pardetails.qty;
                    row["unitamount"] = pardetails.unitAmt;
                    row["total"] = pardetails.total;
                    row["remarks"] = pardetails.remarks;
                    row["chartofaccount"] = pardetails.chartOfAccount;
                    row["fccc"] = pardetails.fccc;
                    row["budgetline"] = pardetails.budgetLine;
                    row["myref"] = 1;
                    objDt.Rows.Add(row);
                }
                else
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["sno"].ToString() == pardetails.unChecked12.ToString())
                        {
                            // Dt.Rows[i]["prdetails_gid"] =  pardetails.productgroupid;

                            objDt.Rows[i]["uom"] = pardetails.uomGid;
                            objDt.Rows[i]["qty"] = pardetails.qty;
                            objDt.Rows[i]["unitamount"] = pardetails.unitAmt;
                            objDt.Rows[i]["total"] = pardetails.total;
                            objDt.Rows[i]["remarks"] = pardetails.remarks;

                            objDt.Rows[i]["fccc"] = pardetails.fccc;
                            objDt.Rows[i]["budgetline"] = pardetails.budgetLine;
                        }
                    }

                }

                Session["parcbfdetails"] = objDt;
                objcbf = objRep.GetCbfParDetails(objDt);

                objcbf.selectedValue = pardetails.productGroupId.ToString();
                objcbf.selectedValue1 = pardetails.uomGid.ToString();
                objcbf.productGroupList = new SelectList(objRep.Getproductlist(), "productGroupId", "productGroupIdName");
                objcbf.uomList = new SelectList(objRep.GetUomList(), "uomGid", "uomCode");
                Session["cbfprdeta"] = objcbf;
                return PartialView("EditCbfParDetails", objcbf);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult editupdate(CbfDetails UserModel)
        {
            try
            {
                CbfSumEntity objGetPrdetails = new CbfSumEntity();
                objGetPrdetails.cbfDetails = new List<CbfDetails>();
                CbfDetails Cbfdetails;
                if (Session["parcbfdetails"] == null)
                {
                    Session["parcbfdetails"] = Session["cbfdeailsdt"];
                }
                DataTable dtCbfdetails = (DataTable)Session["parcbfdetails"];
                if (dtCbfdetails.Columns.Contains("BOQ"))
                {
                    
                }
                else
                {
                    dtCbfdetails.Columns.Add("BOQ", typeof(int));
                }
                
                if (dtCbfdetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCbfdetails.Rows.Count; i++)
                    {
                        string sa = dtCbfdetails.Rows[i]["cbfdetails_gid"].ToString();
                        if (UserModel.cbfDetGid.ToString() == dtCbfdetails.Rows[i]["cbfdetails_gid"].ToString())
                        {
                            dtCbfdetails.Rows[i]["prdetails_qty"] = UserModel.qty;
                            dtCbfdetails.Rows[i]["pipinput_rate"] = UserModel.unitAmt;
                            dtCbfdetails.Rows[i]["pipinput_costestimation"] = UserModel.total;
                            dtCbfdetails.Rows[i]["remarks"] = UserModel.remarks;
                            dtCbfdetails.Rows[i]["fccc"] = UserModel.fccc;
                            dtCbfdetails.Rows[i]["budgetline"] = UserModel.budgetLine;
                            dtCbfdetails.Rows[i]["uom_gid"] = UserModel.uomGid;
                            dtCbfdetails.Rows[i]["BOQ"] = sa;
                        }
                    }
                }
                Session["parcbfdetails"] = dtCbfdetails;
                objGetPrdetails = objRep.GetCbfParDetails(dtCbfdetails);
                objGetPrdetails.productGroupList = new SelectList(objRep.Getproductlist(), "productgroupid", "productgroupidname");
                objGetPrdetails.uomList = new SelectList(objRep.GetUomList(), "uomgid", "uomcode");
                Session["cbfprdeta"] = objGetPrdetails;
                return PartialView("EditCbfParDetails", objGetPrdetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult CbfDeletePar(CbfDetails UserModel)
        {
            try
            {
                CbfSumEntity objgetcbfdetails = new CbfSumEntity();
                DataTable objDtCbfDetails = new DataTable();
                DataTable objDtCbfDetails1 = new DataTable();
                if (Session["parcbfdetails"] == null)
                {
                    Session["parcbfdetails"] = Session["cbfdeailsdt"];
                    objDtCbfDetails = (DataTable)Session["parcbfdetails"];
                }
                else
                {
                    objDtCbfDetails = (DataTable)Session["parcbfdetails"];
                }


                if (UserModel.cbfDetGid.ToString() != "" && UserModel.cbfDetGid.ToString() != "0" && UserModel.cbfDetGid.ToString() != null)
                {
                    for (int i = 0; i < objDtCbfDetails.Rows.Count; i++)
                    {
                        if (objDtCbfDetails.Rows[i]["cbfdetails_gid"].ToString() == UserModel.cbfDetGid.ToString())
                        {
                            objDtCbfDetails.Rows.RemoveAt(i);
                            if (Session["ArrayList"] != null)
                                lalCbfdetails = (ArrayList)Session["ArrayList"];
                            lalCbfdetails.Add(UserModel.cbfDetGid.ToString());
                            Session["ArrayList"] = lalCbfdetails;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < objDtCbfDetails.Rows.Count; i++)
                    {
                        if (objDtCbfDetails.Rows[i]["sno"].ToString() == UserModel.unChecked12.ToString())
                            objDtCbfDetails.Rows.RemoveAt(i);
                    }

                }
                Session["parcbfdetails"] = objDtCbfDetails;
                objgetcbfdetails = objRep.GetCbfParDetails(objDtCbfDetails);
                Session["cbfprdeta"] = objgetcbfdetails;
                objgetcbfdetails.productGroupList = new SelectList(objRep.Getproductlist(), "productgroupid", "productgroupidname");
                objgetcbfdetails.uomList = new SelectList(objRep.GetUomList(), "uomgid", "uomcode");
                return PartialView("EditCbfParDetails", objgetcbfdetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult cbfeditsearch(CbfPrHeader objPrHeader)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();

            string lsPrNo = objPrHeader.prNo;
            string lsPrDate = objPrHeader.prDate;

            //    objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");

            if (objPrHeader.branch_Gid != 0)
                objPrHeader.description = objRep.GetStatusName(objPrHeader.branch_Gid);

            string lsRequest1 = objPrHeader.description;
            objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Details"]));

            if (!(string.IsNullOrEmpty(lsPrNo)))
            {
                ViewBag.txtcbfno = lsPrNo.ToUpper();
                objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsPrNo.ToUpper() == null
                    || (x.prNo.Contains(lsPrNo.ToUpper()))).ToList();

                if (objSumEntity.cbfPrheader.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["objowner"] = objSumEntity;
            }
            if (!(string.IsNullOrEmpty(lsPrDate)))
            {
                ViewBag.txtcbfdate = lsPrDate;

                objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsPrDate == null
                    || (x.prDate.Contains(lsPrDate))).ToList();
                if (objSumEntity.cbfPrheader.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["objowner"] = objSumEntity;

            }
            if (!(string.IsNullOrEmpty(lsRequest1)))
            {
                ViewBag.txtcbfmode = lsRequest1;

                objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsRequest1 == null
                    || (x.prRequestFor.Equals(lsRequest1))).ToList();
                if (objSumEntity.cbfPrheader.Count == 0)
                    ViewBag.Message = "No records found";
                Session["objowner"] = objSumEntity;

            }
            if ((string.IsNullOrEmpty(lsPrNo)) &&
          (string.IsNullOrEmpty(lsPrDate)) &&
          (string.IsNullOrEmpty(lsRequest1)))
                ViewBag.Error = "Please fill search Criteria";
            objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");
            Session["prheadersession"] = objSumEntity;
            return Json(objSumEntity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Viewboqattachment()
        {
            ViewBag.viewfor = Session["viewfor"];
            string cbfattchemnet = Request.QueryString["id"].ToString();
            Session["cbfdetails"] = cbfattchemnet;
            CbfSumEntity objMAttachment = new CbfSumEntity();

            if (Session["cbfdetails"].ToString() == "")
            {
                objMAttachment.amoun = cbfattchemnet;
                objMAttachment = objRep.Attachmentname(objMAttachment);
            }
            else
            {
                objMAttachment.amoun = cbfattchemnet;
                objMAttachment = objRep.Attachmentname(objMAttachment);
            }
            objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
            return PartialView(objMAttachment);
        }
        [HttpPost]
        public JsonResult Viewboqattachment(CbfSumEntity objAttachment)
        {
            try
            {
                CbfSumEntity objAttachment1 = new CbfSumEntity();
                if (ModelState.IsValid)
                {
                    objAttachment.amoun = Session["cbfdetails"].ToString();
                    objAttachment1 = objRep.Attachmentname(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                    }
                    else
                    {
                        Session["attachmentdetails"] = objAttachment1;
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DownloadDocument(CbfSumEntity obj)
        {
            Session["downfile"] = obj.attachment1;
            CbfSumEntity obj1 = new CbfSumEntity();
            return Json(obj1, JsonRequestBehavior.AllowGet);
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        public FileResult Download(CbfSumEntity obj)
        {

            try
            {
                string txt1 = Session["downfile"].ToString();
                string directory = HoldFileUploadUrlDSA() + txt1.ToString();
                byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                string fileName = "Download" + txt1.ToString();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAttachment(CbfSumEntity obj)
        {
            ArrayList arraylist = new ArrayList();
            DataTable AttachDelete;
            CbfSumEntity objAttachment1 = new CbfSumEntity();
            if (Session["cbfdetails"].ToString() == "")
            {
                AttachDelete = (DataTable)Session["AccessToken"];
            }
            else
            {
                AttachDelete = (DataTable)Session["AccessTokenheader"];

            }
            if (AttachDelete.Rows.Count > 0)
            {
                for (int i = 0; i < AttachDelete.Rows.Count; i++)
                {
                    if (AttachDelete.Rows[i]["Sno"].ToString() == obj.attachment1)
                    {
                        if (AttachDelete.Rows[i]["cbfheader_gid"].ToString() != "")
                        {
                            if (Session["Arraylistattachment"] != null)
                            {
                                arraylist = (ArrayList)Session["Arraylistattachment"];

                            }
                            arraylist.Add(AttachDelete.Rows[i]["Sno"].ToString());
                            Session["Arraylistattachment"] = arraylist;
                        }
                        else if (AttachDelete.Rows[i]["cbfdetails_gid"].ToString() != "")
                        {
                            if (Session["Arraylistattachment"] != null)
                            {
                                arraylist = (ArrayList)Session["Arraylistattachment"];

                            }
                            arraylist.Add(AttachDelete.Rows[i]["Sno"].ToString());
                            Session["Arraylistattachment"] = arraylist;
                        }
                        AttachDelete.Rows.RemoveAt(i);
                        objAttachment1.amoun = Session["cbfdetails"].ToString();
                    }
                }
            }


            objAttachment1 = objRep.Attachmentname(objAttachment1);
            objAttachment1.amoun = Session["cbfdetails"].ToString();

            if (Session["cbfdetails"].ToString() == "")
            {
                Session["attachment"] = objAttachment1;
            }
            else
            {
                Session["attachmentdetails"] = objAttachment1;
            }
            return Json(objAttachment1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult fcccsearchtext(string lsFccc)
        {
            string data = objRep.GetFcccSerachText(lsFccc);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel(CbfRaiseHeader objCbfRaiser)
        {
            string data = objRep.Cancel(objCbfRaiser);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(CbfRaiseHeader objCbfRaiser)
        {
            string data = objRep.Delete(objCbfRaiser);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult productdetails(CbfDetails Parproduct)
        {
            try
            {
                int data = 0;
                DataTable ObjDtpar = new DataTable();
                ObjDtpar = (DataTable)Session["parcbfdetails"];
                if (ObjDtpar != null)
                {
                    DataRow[] matches = ObjDtpar.Select("cbfdetails_prod_gid='" + Parproduct.productgid + "'");
                    data = matches.Count();
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult viewattachforpar(int lnParDetail)
        {
            try
            {
                string par = (string)Session["pardetails_gid"];
                ViewBag.NoRecordsFound = "";
                CbfSumEntity data = new CbfSumEntity();
                data.attachment = objRep.Getattachment(par.ToString(), "12", "2");
                if (data.attachment.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult BranchDetailsForcbfEdit(string listfor)
        {
            //Session["tempbranchseredit"] = "First";
            List<shipment> objbanklist = new List<shipment>();
            if (listfor == "search")
            {
                if (Session["objbranch"] != null)
                    objbanklist = (List<shipment>)Session["objbranch"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["branchCode"] != null)
                    TempData["code"] = TempData["branchCode"];
                if (TempData["branchName"] != null)
                    TempData["name"] = TempData["branchName"];
            }
            else
            {
                Session["objowner"] = "";
                objbanklist = objRep.getbranchdetails();
            }
            return PartialView("BranchDetailsForcbfEdit", objbanklist);
        }
        [HttpPost]
        public PartialViewResult searchbranch(shipment objbranchsearch)
        {
            List<shipment> objowner = new List<shipment>();
            objowner = objRep.getbranchdetails();
            if (objbranchsearch != null)
            {
                if ((string.IsNullOrEmpty(objbranchsearch.branchCode)) == false)
                {
                    TempData["branchCode"] = objbranchsearch.branchCode;
                    objowner = objowner.Where(x => objbranchsearch.branchCode == null ||
                        (x.branchCode.ToUpper().Contains(objbranchsearch.branchCode.ToUpper()))).ToList();
                    Session["objbranch"] = objowner;
                }
                if ((string.IsNullOrEmpty(objbranchsearch.branchName)) == false)
                {
                    TempData["branchName"] = objbranchsearch.branchName;
                    objowner = objowner.Where(x => objbranchsearch.branchName == null ||
                        (x.branchName.ToUpper().Contains(objbranchsearch.branchName.ToUpper()))).ToList();
                    Session["objbranch"] = objowner;
                }
            }
           // return Json(objowner, JsonRequestBehavior.AllowGet);
            return PartialView("BranchDetailsForcbfEdit", objowner);
        }
        //For Delmat

        //public JsonResult DelmatApproveForCbf(CbfSummarymodel objcbf)
        //{
        //    try
        //    {
        //        string GetEmployeeDetails = objRep.GetDelmatemployee(objcbf.cbfGid.ToString());
        //        return Json(GetEmployeeDetails, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
