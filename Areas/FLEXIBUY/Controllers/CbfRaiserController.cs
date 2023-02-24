using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CbfRaiserController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private IrepositoryAn objRep;


        public CbfRaiserController()
            : this(new CbfSumModel())
        {
        }

        public CbfRaiserController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }

        public ActionResult CbfRaiser(string lsIda)
        {
            try
            {
                Session["tempbranchser"] = null;
                Session["tempparheaderra"] = null;
                Session["cbfAttachment"] = null;
                Session["cbfAttachmentDetails"] = null;
                Session["count"] = null;
                Session["count1"] = null;
                Session["prdetails_gid"] = null;
                Session["AccessTokenheader"] = null;
                Session["AccessToken"] = null;
                Session["AccessTokeninline"] = null;
                Session["parcbfdetails"] = null;
                Session["da"] = null;
                Session["attachment"] = null;
                Session["objowner"] = null;
                Session["parheadersession"] = null;
                Session["dasavedate"] = null;
                Session["attcnt"] = null;
                Session["cbfid"] = null;
                Session["attach_date_val"] = null;
                Session["cbfprdeta"] = null;
                Session["cbfprdetails"] = null;

                //objDetails = objrep.getprheader();
                CbfRaiseHeader objCrHr = new CbfRaiseHeader();
                int getbranchid = objRep.Getbranchgid();

                objCrHr.cbfDate = DateTime.Now.ToString("dd-MM-yyyy");
                objCrHr.cbfNo = lsIda;

                CbfSumEntity objDetails = new CbfSumEntity()
                {
                    requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname"),
                    projectOwner1 = new SelectList(objRep.GetList(), "projectownergid", "projectownername"),
                    //branchCode1 = new SelectList(objRep.GetBranchCode(), "branchcodegid", "branchcodename", selectedValue: getbranchid),
                    BudgetOwner = new SelectList(objRep.GetBudgetowner(), "BudgetownerGid", "BudgetOwner_Name"),
                    raiser = objCrHr,
                };
                objDetails.result = objRep.GetReqGroup();
                objRep.deletetempstoredattach("22270");
                return View(objDetails);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult PrHeader(string lsListFor = null,int BranchDetailsGid = 0)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objDetails = new CbfSumEntity();
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

                else
                {
                    Session["objowner"] = "";
                    objDetails = (CbfSumEntity)Session["prheadersession"];
                    if (objDetails.cbfPrheader.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                //if (BranchDetailsGid != 0)
                //{
                //    Session["branchgidforpr"] = BranchDetailsGid;
                //    objDetails.cbfPrheader = objDetails.cbfPrheader.Where(n => n.branch_Gid.Equals(BranchDetailsGid)).ToList();
                //}
              
                return PartialView(objDetails);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult PrHeader(CbfPrHeader objPrheader)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objDetails = new CbfSumEntity();
            Session["Prdetailsfor"] = objPrheader.prRequestFor;
            objDetails = objRep.GetPrHeader(Convert.ToInt16(Session["Prdetailsfor"]));
            Session["prheadersession"] = objDetails;
            if (objDetails.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }

            return Json(objDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult ParHeader(string isBudgeted = null)
        {
            try
            {
                if (isBudgeted == "yes")
                {
                    TempData["isBudgeted"] = "yes";
                    Session["parheadersession"] = null;
                }
                else if (isBudgeted == "no")
                {
                    TempData["isBudgeted"] = "no";
                    Session["parheadersession"] = null;
                }
                else
                {
                    TempData["isBudgeted"] = "pr";
                }
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objDetails = new CbfSumEntity();
                //objDetails = objRep.GetParHeader();
                //Session["parheadersession"] = objDetails;
                //if (objDetails.cbfParheader.Count == 0)
                //{
                //    ViewBag.NoRecordsFound = "No Records Found";
                //}
                return PartialView(objDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult PrDetails()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objGetPrDetails = (CbfSumEntity)Session["prd"];
                if (objGetPrDetails.cbfPrdetarils.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("PrDetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpPost]
        public PartialViewResult PrDetails(CbfPrHeader objPrHeGid)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                Session["idprdetails"] = objPrHeGid.prhed_Gid;
                CbfSumEntity objGetPrDetails = new CbfSumEntity();
                objGetPrDetails = objRep.GetPrDetail(objPrHeGid);
                Session["prd"] = objGetPrDetails;
                if (objGetPrDetails.cbfPrdetarils.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }

                return PartialView("PrDetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpGet]
        public PartialViewResult CbfDetails()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objGetPrDetails = (CbfSumEntity)Session["cbfprdeta"];
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("CbfDetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [HttpPost]
        public PartialViewResult CbfDetails(CbfDetails objPrDelGid, CbfDetails objUnchecked1)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objDt = new DataTable();
                DataTable objDt1 = new DataTable();
                CbfSumEntity objGetPrDetails = new CbfSumEntity();
                if (Session["da"] != null)
                    objDt1 = (DataTable)Session["da"];

                if (objPrDelGid.prdetailsGid != 0)
                {
                    if (objDt1.Columns.Count > 0)
                    {

                        DataView dv = new DataView();
                        dv = objDt1.DefaultView;
                        dv.RowFilter = "prdetails_gid =" + objUnchecked1.prdetailsGid + "";
                        objDt = dv.ToTable();
                        //if (objDt1.Columns.Contains("remarks"))
                        //{

                        //    DataRow[] matches = objDt1.Select("remarks is null");
                        //    foreach (DataRow row in matches)
                        //    {
                        //        objDt1.Rows.Remove(row);
                        //    }
                        //}
                    }
                    if (objDt.Rows.Count == 0)
                    {
                        objDt = null;
                        objDt = objRep.DtTable(objPrDelGid);
                        if (objDt1.Columns.Contains("remarks"))
                        {
                            for (int i = 0; i < objDt1.Rows.Count; i++)
                            {
                                if (objDt1.Rows[i]["fccc"].ToString() == "")
                                {
                                    objDt1.Rows.RemoveAt(i);
                                }

                            }
                            objDt1.Merge(objDt);
                            Session["da"] = objDt1;
                        }
                        else
                        {
                            
                            objDt1.Merge(objDt);
                            Session["da"] = objDt1;
                            //objDt1 = objDt;
                        }


                        Session["prdetails_gid"] = objUnchecked1.prdetailsGid;
                    }
                    objGetPrDetails = objRep.GetCbfDetails(objDt1);
                }
                else if (objUnchecked1.unChecked12 != 0)
                {
                    DataView dv = new DataView();
                    dv = objDt1.DefaultView;

                    dv.RowFilter = "prdetails_gid =" + objUnchecked1.unChecked12 + "";
                    objDt = dv.ToTable();

                    if (objDt.Columns.Contains("remarks"))
                    {
                        if (objDt.Rows[0]["remarks"].ToString() != "")
                        {
                            objGetPrDetails = objRep.GetCbfDetails(objDt1);
                        }
                        else
                        {
                            Session["da"] = objDt1;
                            dv = objDt1.DefaultView;
                            dv.RowFilter = "prdetails_gid <>" + objUnchecked1.unChecked12 + "";
                            DataRow[] matches = objDt1.Select("prdetails_gid='" + objUnchecked1.unChecked12 + "'");
                            foreach (DataRow row in matches)
                            {
                                objDt1.Rows.Remove(row);
                            }

                            objGetPrDetails = objRep.GetCbfDetails(objDt1);
                        }

                    }
                    else
                    {
                        Session["da"] = objDt1;
                        DataRow[] matches = objDt1.Select("prdetails_gid='" + objUnchecked1.unChecked12 + "'");
                        foreach (DataRow row in matches)
                        {
                            objDt1.Rows.Remove(row);
                        }
                        objGetPrDetails = objRep.GetCbfDetails(objDt1);
                    }


                }
                Session["cbfprdeta"] = objGetPrDetails;
                Session["cbfprdetails"] = objGetPrDetails.cbfDetails;
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("CbfDetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpPost]
        public PartialViewResult CbfSave(CbfDetails objUserModel)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();
                CbfSumEntity objGetPrdetails = new CbfSumEntity();
                if (Session["da"] != null)
                    objdt1 = (DataTable)Session["da"];
                if (objdt1.Columns.Count <= 13)
                {
                    objdt1.Columns.Add("remarks", typeof(string));
                    objdt1.Columns.Add("fccc", typeof(int));
                    objdt1.Columns.Add("budgetline", typeof(int));
                    objdt1.Columns.Add("myref", typeof(int));
                }
                for (int i = 0; i < objdt1.Rows.Count; i++)
                {
                    if (objdt1.Rows[i]["prdetails_gid"].ToString() == objUserModel.unChecked12.ToString())
                    {
                        objdt1.Rows[i]["remarks"] = objUserModel.remarks;
                        objdt1.Rows[i]["prdetails_qty"] = objUserModel.qty;
                        objdt1.Rows[i]["pipinput_rate"] = objUserModel.unitAmt;
                        objdt1.Rows[i]["pipinput_costestimation"] = objUserModel.total;
                        objdt1.Rows[i]["fccc"] = objUserModel.fccc;
                        objdt1.Rows[i]["budgetline"] = objUserModel.budgetLine;
                        objdt1.Rows[i]["myref"] = 1;
                    }
                }
                Session["da"] = objdt1;
                objGetPrdetails = objRep.GetCbfDetails(objdt1);
                Session["cbfprdeta"] = objGetPrdetails;
                if (objGetPrdetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("CbfDetails", objGetPrdetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult CbfDelete(CbfDetails objUserModel)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objGetPrDetails = new CbfSumEntity();
                DataTable objDt = new DataTable();
                DataTable AttachDelete = new DataTable();
                if (Session["da"] != null)
                    objDt = (DataTable)Session["da"];

                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    if (objDt.Rows[i]["prdetails_gid"].ToString() == objUserModel.prdetailsGid.ToString())
                        objDt.Rows.RemoveAt(i);
                    AttachDelete = (DataTable)Session["AccessTokenheader"];
                    if (AttachDelete != null)
                    {
                        if (AttachDelete.Rows.Count > 0)
                        {
                            for (int j = 0; j < AttachDelete.Rows.Count; j++)
                            {
                                if (AttachDelete.Rows[j]["prdetails"].ToString() == objUserModel.prdetailsGid.ToString())
                                {
                                    AttachDelete.Rows.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                Session["AccessTokenheader"] = AttachDelete;
                objGetPrDetails = objRep.GetCbfDetails(objDt);
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                Session["cbfprdeta"] = objGetPrDetails;
                return PartialView("CbfDetails", objGetPrDetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public PartialViewResult CbfDeletePar(CbfDetails objUserModel)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objGetPrDetails = new CbfSumEntity();
                DataTable objDt = new DataTable();
                if (Session["parcbfdetails"] != null)
                    objDt = (DataTable)Session["parcbfdetails"];
                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    if (objDt.Rows[i]["sno"].ToString() == objUserModel.attch_Gid.ToString())
                        objDt.Rows.RemoveAt(i);

                }
                objGetPrDetails = objRep.GetCbfParDetails(objDt);
                Session["parcbfdetails"] = objDt;
                objGetPrDetails.productGroupList = new SelectList(objRep.Getproductlist(), "productGroupId", "productGroupIdName");
                objGetPrDetails.uomList = new SelectList(objRep.GetUomList(), "uomGid", "uomCode");
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                Session["parcbfdetails_list"] = objGetPrDetails;
                return PartialView("CbfParDetails", objGetPrDetails);

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
                    objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");
                }
            }
            else
            {
                Session["prheadersession"] = "";
                objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Prdetailsfor"]));
            }
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("prHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfRefresh()
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Prdetailsfor"]));
            objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");
            Session["prheadersession"] = objSumEntity;
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return Json(objSumEntity, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult CbfSearch(string listfor = null)
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
                objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Prdetailsfor"]));
            }
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("prHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfSearch(CbfPrHeader objPrHeader)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();

            string lsPrNo = objPrHeader.prNo;
            string lsPrDate = objPrHeader.prDate;

            //    objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");

            if (objPrHeader.branch_Gid != 0)
                objPrHeader.description = objRep.GetStatusName(objPrHeader.branch_Gid);
            if (objPrHeader.prDate != null)
                lsPrDate = objPrHeader.prDate.ToString();
            string lsRequest1 = objPrHeader.description;
            objSumEntity = objRep.GetPrHeader(Convert.ToInt32(Session["Prdetailsfor"]));


            if (!(string.IsNullOrEmpty(lsPrNo)))
                ViewBag.txtcbfno = lsPrNo.ToUpper();
            objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsPrNo == null
                || (x.prNo.Contains(lsPrNo))).ToList();
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            Session["objowner"] = objSumEntity;
            if (!(string.IsNullOrEmpty(lsPrDate)))
                ViewBag.txtcbfdate = lsPrDate;

            objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsPrDate == null
                || (x.prDate.Contains(lsPrDate))).ToList();
            if (objSumEntity.cbfPrheader.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            Session["objowner"] = objSumEntity;
            if (!(string.IsNullOrEmpty(lsRequest1)))
                ViewBag.txtcbfmode = lsRequest1;

            objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => lsRequest1 == null
                || (x.prRequestFor.Equals(lsRequest1))).ToList();
            if (objSumEntity.cbfPrheader.Count == 0)
                ViewBag.Message = "No records found";
            Session["objowner"] = objSumEntity;
            if ((string.IsNullOrEmpty(lsPrNo)) &&
                (string.IsNullOrEmpty(lsPrDate)) &&
                (string.IsNullOrEmpty(lsRequest1)))
                ViewBag.Error = "Please fill search Criteria";
            objSumEntity.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname");
            Session["prheadersession"] = objSumEntity;

            return Json(objSumEntity, JsonRequestBehavior.AllowGet);
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
            return PartialView("ParHeader", objSumEntity);
        }
        [HttpPost]
        public JsonResult CbfSearchPAR(CbfParHeader prheader)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            Session["parheadersession"] = null;
            string lsPrNo = "";
            string lsPrDate = "";
            string lsParBudget = "";
            int lsRequest1 = 0;
            if (prheader.parNo != null)
                lsPrNo = prheader.parNo.ToString();
            if (prheader.parDate != null)
                lsPrDate = prheader.parDate.ToString();
            if (prheader.parAmt != 0)
                lsRequest1 = Convert.ToInt32(prheader.parAmt);
            if (prheader.par_Budget != null)
                lsParBudget = prheader.par_Budget.ToString();
            objSumEntity = objRep.GetParHeader(0,lsParBudget);


            if (!(string.IsNullOrEmpty(lsPrNo)))
            {
                ViewBag.txtcbfno = lsPrNo.ToUpper();
                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsPrNo == null
                    || (x.parNo.Contains(lsPrNo))).ToList();
            }


            Session["objowner"] = objSumEntity;

            if (!(string.IsNullOrEmpty(lsPrDate)))
            {
                ViewBag.txtcbfdate = lsPrDate;
                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsPrDate == null
                    || (x.parDate.Contains(lsPrDate))).ToList();
            }

            //Session["objowner"] = objSumEntity;

            if (lsRequest1.ToString() != "0")
            {
                ViewBag.txtcbfmode = lsRequest1;
                objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsRequest1 == null
                    || (x.parAmt.Equals(lsRequest1))).ToList();
            }

            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No records found";
            }

            //if (prheader.par_Budget != null && prheader.par_Budget != "")
            //{
            //    objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => prheader.par_Budget == null
            //       || (x.par_Budget.Contains(prheader.par_Budget))).ToList();
            //}
            //Session["objowner"] = objSumEntity;

            //if ((string.IsNullOrEmpty(lsPrNo)) &&
            //    (string.IsNullOrEmpty(lsPrDate)) &&
            //    (lsRequest1==0))
            //    ViewBag.Error = "Please fill search Criteria";

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
                Session["parheadersession"] = null;
                objSumEntity = objRep.GetParHeader();
            }
            if (objSumEntity.cbfParheader.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("ParHeader", objSumEntity);
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
        public JsonResult Create(CbfRaiseHeader objClassificationModel)
        {
            try
            {
                CbfSumEntity objSumEntity;
                DataTable objDt = new DataTable();
                if (objClassificationModel.cbfMode == "PR")
                {
                    objDt = (DataTable)Session["da"];
                }
                else if (objClassificationModel.cbfMode == "PAR")
                {
                    objDt = (DataTable)Session["parcbfdetails"];
                }


                string data = objRep.InsertCbfHeader(objClassificationModel, objDt);
                RedirectToAction("Index");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BoqAttach()
        {
            try
            {
                Session["cbfdetails"] = "";
                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                CbfSumEntity objMAttachment = new CbfSumEntity();

                if (cbfattchemnet == "")
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //PRAVEEN
        [HttpGet]
        public PartialViewResult BoqAttached(string viewfor=null)
        {
            try
            {
                //Session["cbfdetails"] = "";
                //string cbfattchemnet = Request.QueryString["id"].ToString();
                //Session["cbfdetails"] = cbfattchemnet;
                //CbfSumEntity objMAttachment = new CbfSumEntity();
                //Session["inline"] = "3".ToString();
                //if (cbfattchemnet == "")
                //{
                //    objMAttachment.amoun = cbfattchemnet;
                //    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                //}
                //else
                //{
                //    objMAttachment.amoun = cbfattchemnet;
                //    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                //}
                //objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                //return PartialView(objMAttachment);

                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                CbfSumEntity objMAttachment = new CbfSumEntity();
                string dateval = Convert.ToString(Session["attach_date_val"]);
                string cnt = Convert.ToString(Session["attcnt"]);

                if (cbfattchemnet == "Attach_1" || cbfattchemnet == "undefined")
                {
                    if (cnt == "")
                    {

                        Session["attcnt"] = "31";
                    }
                    else
                    {
                        int count = Convert.ToInt32(cnt);
                        count++;
                        Session["attcnt"] = Convert.ToString(count);
                    }
                    if (cbfattchemnet == "")
                    {
                        objMAttachment.amoun = cbfattchemnet;
                        objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                    }
                    else
                    {
                        objMAttachment.amoun = cbfattchemnet;
                        objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                    }



                    objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                }
                else
                {
                    Session["attcnt"] = cbfattchemnet;
                    CbfSumEntity objAttachment = new CbfSumEntity();
                    objAttachment.attachmentDate = dateval;
                    objAttachment.boqfile = cbfattchemnet;
                    objAttachment.cbfGid = "22270";
                    objAttachment.cbfref = "2";
                    objMAttachment.attachment = objRep.Getattachment_val(objAttachment);

                }
                if (Session["ViewBag"] == "" || Session["ViewBag"] == "diasbled" || viewfor=="view")
                {
                    //if (cnt == "")
                    //{

                    //    Session["attcnt"] = "31";
                    //}
                    //else
                    //{
                    //    int count = Convert.ToInt32(cnt);
                    //    count++;
                    //    Session["attcnt"] = Convert.ToString(count);
                    //}
                    CbfSumEntity objAttachment = new CbfSumEntity();
                    objAttachment.attachmentDate = dateval;
                    objAttachment.boqfile = cbfattchemnet;
                    if (cbfattchemnet != "Attach_1")
                    {
                        objMAttachment.attachment = objRep.Getattachment(cbfattchemnet, "2", "2");
                    }
                   
                }
                return PartialView(objMAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public JsonResult save_attach(CbfSumEntity parModel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["ViewBag"] == "")
                    {
                        Session["cbfid"] = "1";
                        parModel.Budgeted = Session["attcnt"].ToString();
                        result = objRep.attach_cbfinlineEdit(parModel);
                        Session["attach_date_val"] = parModel.Attachment_date;
                    }
                    else
                    { //string[] attach_cnt = Session["cbfdetails"].ToString().Split('_');
                        parModel.Budgeted = Session["attcnt"].ToString();
                        result = objRep.attach_parinline_cbf(parModel);
                        Session["attach_date_val"] = parModel.Attachment_date;
                        if (result == "success") RedirectToAction("Index");
                        
                    }

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BoqAttachGrid()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment = new CbfSumEntity();
                if (Session["cbfdetails"].ToString() == "")
                {
                    objAttachment = (CbfSumEntity)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                else
                {
                    if (Session["attachmentdetails"] != null)
                    {
                        objAttachment = (CbfSumEntity)Session["attachmentdetails"];
                        if (objAttachment.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //praveen
        [HttpGet]
        public PartialViewResult BoqAttachgridinline()
        {
            try
            {
                string cbfattchemnet = Convert.ToString(Request.QueryString["id"]);
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment = new CbfSumEntity();
                //if (Session["cbfdetails"].ToString() == "")
                //{
                //    objAttachment = (CbfSumEntity)Session["attachment"];
                //    if (objAttachment.attachment.Count == 0)
                //    {c
                //        ViewBag.NoRecordsFound = "No Records Found";
                //    }
                //}
                //else
                //{
                //    objAttachment = (CbfSumEntity)Session["attachmentdetails"];
                //    if (objAttachment.attachment.Count == 0)
                //    {
                //        ViewBag.NoRecordsFound = "No Records Found";
                //    }
                //}

                string ji = Convert.ToString(Session["cbfid"]);
                if (Convert.ToString(Session["cbfid"]) == "22270" || Convert.ToString(Session["cbfid"]) == "")
                {
                    string dateval = Session["attach_date_val"].ToString();
                    objAttachment.attachmentDate = dateval;
                    objAttachment.boqfile = Session["attcnt"].ToString();
                    objAttachment.cbfGid = "22270";
                    objAttachment.cbfref = "2";
                    objAttachment.attachment = objRep.Getattachment_val(objAttachment);

                }
                else
                {
                    //   objAttachment.attachmentDate = dateval;
                    cbfattchemnet = Convert.ToString(Session["cbfdetails"]);
                    Session["attcnt"] = cbfattchemnet;
                    objAttachment.attachment = objRep.Getattachment(cbfattchemnet, "2", "2");
                }
                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult BoqAttachgridinline(CbfSumEntity objAttachment)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment1 = new CbfSumEntity();
                if (ModelState.IsValid)
                {
                    objAttachment.amoun = Session["cbfdetails"].ToString();
                    if (objAttachment.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || objAttachment.amoun == "Attach_1")
                    {
                        objAttachment.amoun = "";
                    }
                    objAttachment1 = objRep.AttachmentnameInline(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachmentdetails"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }



            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult BoqAttachGrid(CbfSumEntity objAttachment)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment1 = new CbfSumEntity();
                if (ModelState.IsValid)
                {
                    objAttachment.amoun = Session["cbfdetails"].ToString();
                    if (objAttachment.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
                    {
                        objAttachment.amoun = "";
                    }
                    objAttachment1 = objRep.Attachmentname(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachmentdetails"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Submit(CbfRaiseHeader objClassificationModel)
        {
            try
            {
                CbfSumEntity obj;
                DataTable objdt = new DataTable();
                if (objClassificationModel.cbfMode == "PR")
                {
                    objdt = (DataTable)Session["da"];
                }
                else if (objClassificationModel.cbfMode == "PAR")
                {
                    objdt = (DataTable)Session["parcbfdetails"];
                }
                string data = objRep.InsertCbfHeader1(objClassificationModel, objdt);
                RedirectToAction("Index");
                //ForMailController mailsender = new ForMailController();

                //string mail = Session["queuegid"].ToString();
                //mailsender.sendusermail("FB", "CBF", mail, "S", "0");
                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                int refgid = objMail.GetRefGidForMail("CBF");
                string reqstatus = objMail.GetRequestStatus(refgid, "CBF");
                int queuegid = objMail.GetQueueGidForMail(refgid, "CBF");
                mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult CreateSearch()
        {
            ViewBag.NoRecordsFound = "";
            string lsId = Convert.ToString(Request.QueryString["id"]);
            CbfSumEntity objgetprdetails = new CbfSumEntity();

            objgetprdetails = objRep.GetFccc();
            if (objgetprdetails.searchFccc.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            objgetprdetails.des = lsId;
            Session["des"] = lsId;
            return PartialView(objgetprdetails);

        }

        [HttpPost]
        public PartialViewResult FcccSearch(FcccMaster objFcccModel)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            objGetPrDetails = objRep.GetFcccSerach(objFcccModel);
            if (objGetPrDetails.searchFccc.Count == 0)
            {
                ViewBag.NoRecordsFound = "No Records Found";
            }
            return PartialView("CreateSearch", objGetPrDetails);
        }

        //[HttpPost]
        //public async Task<JsonResult> UploadedFiles(string lsfname)
        //{
        //    try
        //    {
        //        string lsFilename = "";
        //        foreach (string lsfile in Request.Files)

        //        {
        //            var fileContent = Request.Files[lsfile];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //                if (lsfname != null && lsfname.Trim() != "")
        //                {
        //                    lsFilename = lsfname.Substring(0, lsfname.LastIndexOf(".") + 0);
        //                }
        //                else
        //                {
        //                    lsFilename = objCmnFunctions.GetSequenceNo("CBF");
        //                }
        //            var fileextension = Path.GetExtension(fileContent.FileName);
        //            var stream = fileContent.InputStream;
        //            var path = Path.Combine(@"C:/temp/", lsFilename + fileextension);
        //            //var fileStream = System.IO.File.Create(path);
        //            //stream.CopyTo(fileStream);
        //            using (var fileStream = System.IO.File.Create(path))
        //            {
        //                stream.CopyTo(fileStream);

        //            }
        //            lsFilename = lsFilename + fileextension;
        //        }
        //        return Json(lsFilename);
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}
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
        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {
            //try
            //{
            //    string filename = "";
            //    foreach (string file in Request.Files)
            //    {
            //        var fileContent = Request.Files[file];

            //        if (fileContent != null && fileContent.ContentLength > 0)
            //        {

            //            if (fname != null && fname.Trim() != "")
            //            {
            //                filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
            //            }
            //            else
            //            {
            //                filename = objCmnFunctions.GetSequenceNo("PR");
            //            }



            //            var fileextension = Path.GetExtension(fileContent.FileName);
            //            var stream = fileContent.InputStream;
            //            var path = Path.Combine(@"C:/temp/", filename + fileextension);
            //            using (var fileStream = System.IO.File.Create(path))
            //            {
            //                stream.CopyTo(fileStream);
            //            }
            //            filename = filename + fileextension;
            //        }
            //    }
            //    return Json(filename);
            //}
            //catch (Exception)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("Upload failed");
            //}
            string filename = "";
            //DataTable dtac = new DataTable();
            //dtac.Columns.Add("Attachid");
            //dtac.Columns.Add("AttachName");
            //dtac.Columns.Add("AttachFileType");
            //dtac.Columns.Add("Attachlength");
            //dtac.Columns.Add("flag");
            //int j = 1;
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["boqfile"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string; ;
                    //if (this.CreateFolderIfNeeded(pathForSaving))
                    //{

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        // filename = filename.Substring(0, filename.LastIndexOf(".") + 0);
                        filename = objCmnFunctions.GetSequenceNo("CBF");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var path = Path.Combine(HoldFileUploadUrlDSA(), filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filename"] = filename;

                        //myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        // myFile.SaveAs(Path.Combine(@"C:\temp\", myFile.FileName));

                        //DataRow dr = dtac.NewRow();
                        //dr["Attachid"] = j.ToString();
                        ////   dr["AttachName"] = Request.Files[file].FileName.ToString();
                        //dr["AttachName"] = Path.GetFileName(Request.Files["boqfile"].FileName);
                        //dr["AttachFileType"] = Request.Files["boqfile"].ContentType.ToString();
                        //dr["Attachlength"] = Request.Files["boqfile"].ContentLength.ToString();
                        //dr["flag"] = 0;

                        //dtac.Rows.Add(dr);



                        isUploaded = true;
                        //message = "File uploaded successfully!";
                        //message = Path.GetFileName(Request.Files["boqfile"].FileName);
                        message = filename;
                        //if (Session["supplierattmtfileDe"] != null)
                        //{
                        //    int a = 1;
                        //    DataTable dtprevalue = new DataTable();
                        //    dtprevalue = (DataTable)Session["supplierattmtfileDe"];
                        //    for (int k = 0; dtprevalue.Rows.Count > k; k++)
                        //    {
                        //        DataRow dr1 = dtac.NewRow();
                        //        dr1["Attachid"] = dtac.Rows.Count + 1;
                        //        dr1["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                        //        dr1["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                        //        dr1["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                        //        dr1["flag"] = 1;
                        //        dtac.Rows.Add(dr1);
                        //        a++;
                        //    }

                        //}
                        //Session["supplierattmtfileDe"] = dtac;

                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                    //}
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        [HttpGet]
        public PartialViewResult ParDetails()
        {
            try
            {

                ViewBag.NoRecordsFound = "";
                CbfSumEntity sumentity = new CbfSumEntity();
                sumentity = (CbfSumEntity)Session["tempparheaderra"];
                Session["tempparheaderra"] = sumentity;
                if (sumentity.cbfPardetailslst.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(sumentity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult ParDetails(CbfParHeader objpar)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity sumentity = new CbfSumEntity();
                sumentity = objRep.CbfPardetails(objpar);
                Session["tempparheaderra"] = sumentity;
                if (sumentity.cbfPardetailslst.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(sumentity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public PartialViewResult CbfParDetails(CbfDetails prdelgid)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();
                CbfSumEntity objgetprdetails = new CbfSumEntity();
                if (Session["parcbfdetails"] != null)
                {
                    objdt1 = (DataTable)Session["parcbfdetails"];
                }
                else
                {
                    objdt = objRep.DtTable(prdelgid);
                }
                if (objdt != null)
                    objdt1.Merge(objdt);
                if (Session["parcbfdetails"] != null)
                {
                    objgetprdetails = objRep.GetCbfParDetails(objdt1);
                }
                else
                {
                    objgetprdetails = objRep.GetCbfDetails(objdt1);
                }
                Session["cbfprdeta"] = objgetprdetails;
                objgetprdetails.productGroupList = new SelectList(objRep.Getproductlist(),
                    "productGroupId", "productGroupIdName");
                objgetprdetails.uomList = new SelectList(objRep.GetUomList(), "uomGid", "uomCode");
                Session["pardetails_gid"] = prdelgid.productCode.ToString();
                if (objgetprdetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("CbfParDetails", objgetprdetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult CbfParSave(CbfDetails pardetails)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objcbf = new CbfSumEntity();
                DataTable objDt = new DataTable();
                if (Session["parcbfdetails"] != null)
                    objDt = (DataTable)Session["parcbfdetails"];
                if (Session["parcbfdetails"] == null)
                {
                    objDt.Columns.Add("sno", typeof(int));
                    objDt.Columns.Add("prdetails_gid", typeof(decimal));
                    objDt.Columns.Add("productgroup", typeof(string));
                    objDt.Columns.Add("productgid", typeof(string));
                    objDt.Columns.Add("productcode", typeof(string));
                    objDt.Columns.Add("productname", typeof(string));
                    objDt.Columns.Add("productdescription", typeof(string));
                    objDt.Columns.Add("uom", typeof(string));
                    objDt.Columns.Add("qty", typeof(int));
                    objDt.Columns.Add("unitamount", typeof(decimal));
                    objDt.Columns.Add("total", typeof(decimal));
                    objDt.Columns.Add("remarks", typeof(string));
                    objDt.Columns.Add("chartofaccount", typeof(int));
                    objDt.Columns.Add("fccc", typeof(string));
                    objDt.Columns.Add("budgetline", typeof(int));
                    objDt.Columns.Add("myref", typeof(int));
                    objDt.Columns.Add("BOQ", typeof(int));
                }
                if (pardetails.unChecked12.ToString() == null || pardetails.unChecked12 == 0)
                {
                    int sno = Convert.ToInt32(Session["sno"]) + 1;
                    if (sno == 1)
                    {
                        Session["attcont_vl"] = "31";
                        pardetails.att_identify = Session["attcont_vl"].ToString();
                        ViewBag.id_valatt = pardetails.att_identify;
                    }
                    else
                    {
                        //int cnmt = Convert.ToInt32(Session["attcont_vl"].ToString());
                        //cnmt++;
                        //Session["attcont_vl"] = Convert.ToString(cnmt);
                        //pardetails.att_identify = Session["attcont_vl"].ToString();
                        //ViewBag.id_valatt = pardetails.att_identify;
                    }
                    Session["sno"] = sno;
                    objDt.Rows.Add(sno, pardetails.prdetailsGid, pardetails.productGroupId, pardetails.productgid,
                     pardetails.productCode, pardetails.productName, pardetails.description, pardetails.uomGid,
                     pardetails.qty, pardetails.unitAmt, pardetails.total, pardetails.remarks, pardetails.chartOfAccount,
                     pardetails.fccc, pardetails.budgetLine, 1, pardetails.att_identify);
                }
                else
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["sno"].ToString() == pardetails.unChecked12.ToString())
                        {
                            // Dt.Rows[i]["prdetails_gid"] =  pardetails.productgroupid;
                            objDt.Rows[i]["productgid"] = pardetails.productgid;
                            objDt.Rows[i]["productgroup"] = pardetails.productGroupId;
                            objDt.Rows[i]["productcode"] = pardetails.productCode;
                            objDt.Rows[i]["productname"] = pardetails.productName;
                            objDt.Rows[i]["productdescription"] = pardetails.description;
                            objDt.Rows[i]["uom"] = pardetails.uomGid;
                            objDt.Rows[i]["qty"] = pardetails.qty;
                            objDt.Rows[i]["unitamount"] = pardetails.unitAmt;
                            objDt.Rows[i]["total"] = pardetails.total;
                            objDt.Rows[i]["remarks"] = pardetails.remarks;
                            objDt.Rows[i]["chartofaccount"] = pardetails.chartOfAccount;
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
                if (objcbf.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("CbfParDetails", objcbf);

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
        //[HttpGet]
        //public PartialViewResult GetProductList(int lnid)
        //{

        //    CbfSumEntity objM = new CbfSumEntity();
        //    objM = objRep.GetProdServDetails(lnid);
        //    HttpContext.Session["ProductGroupgid"] = lnid;

        //    return PartialView(objM);
        //}
        [HttpGet]
        public PartialViewResult GetProductList(int lnid,string listfor=null)
        {
            CbfSumEntity objM = new CbfSumEntity();
            if (listfor == "search")
            {
                if (Session["SearchcbfProd"] != null)
                    objM.cbfDetails = (List<CbfDetails>)Session["SearchcbfProd"];
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
            return PartialView("GetProductlist", objM);
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
                        Session["SearchcbfProd"] = objM.cbfDetails;
                    }
                    if ((string.IsNullOrEmpty(objSearchProd.productName)) == false)
                    {
                        TempData["prodname"] = objSearchProd.productName;
                        objM.cbfDetails = objM.cbfDetails.Where(x => objSearchProd.productName == null ||
                            (x.productName.ToUpper().Contains(objSearchProd.productName.ToUpper()))).ToList();
                        Session["SearchcbfProd"] = objM.cbfDetails;
                    }
                    if (objM.cbfDetails.Count == 0)
                    {
                        TempData["Norecords"] = "No records Found";
                    }
                }
                return PartialView("GetProductlist", objM);
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
        public FileResult Download(CbfSumEntity obj)
        {

            try
            {
                string txt1 = Session["downfile"].ToString();
                string directory = HoldFileUploadUrlDSA() + txt1.ToString();
                FileServer ObjService = new FileServer();
                var Filenamecont = ObjService.DownloadFile(txt1).Result;
                if (Filenamecont != "Fail")
                {
                    if (Filenamecont != "error")
                    {
                        string fileName = "Download" + txt1.ToString();
                        byte[] fileBytes = Convert.FromBase64String(Filenamecont);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                    else
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                        string fileName = "Download" + txt1.ToString();
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                }
                else
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                    string fileName = "Download" + txt1.ToString();
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteInlineAttachment(CbfSumEntity obj)
        {
            //ViewBag.NoRecordsFound = "";
            //DataTable AttachDelete;
            //CbfSumEntity objAttachment1 = new CbfSumEntity();
            //if (Session["cbfdetails"].ToString() == "")
            //{
            //    AttachDelete = (DataTable)Session["AccessTokeninline"];
            //}
            //else
            //{
            //    AttachDelete = (DataTable)Session["AccessTokenheaderinline"];

            //}
            //if (AttachDelete.Rows.Count > 0)
            //{
            //    for (int i = 0; i < AttachDelete.Rows.Count; i++)
            //    {
            //        if (AttachDelete.Rows[i]["Sno"].ToString() == obj.attachment1)
            //        {
            //            AttachDelete.Rows.RemoveAt(i);
            //            objAttachment1.amoun = Session["cbfdetails"].ToString();
            //        }
            //    }
            //}


            //objAttachment1 = objRep.Attachmentname(objAttachment1);
            //objAttachment1.amoun = Session["cbfdetails"].ToString();

            //if (Session["cbfdetails"].ToString() == "")
            //{
            //    Session["attachment"] = objAttachment1;
            //    if (objAttachment1.attachment.Count == 0)
            //    {
            //        ViewBag.NoRecordsFound = "No Records Found";
            //    }
            //}
            //else
            //{
            //    Session["attachmentdetails"] = objAttachment1;
            //    if (objAttachment1.attachment.Count == 0)
            //    {
            //        ViewBag.NoRecordsFound = "No Records Found";
            //    }
            //}
            //return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            string result = string.Empty;
            try
            {
                if (Session["ViewBag"] == "")
                {
                    Session["cbfid"] = "1";
                }
                int attid = Convert.ToInt32(obj.attachment1);
                result = objRep.Deleteattach(attid);

                if (result == "success") RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAttachment(CbfSumEntity obj)
        {
            ViewBag.NoRecordsFound = "";
            DataTable AttachDelete;
            CbfSumEntity objAttachment1 = new CbfSumEntity();
            string s = Session["cbfdetails"].ToString();
            if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
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
                        AttachDelete.Rows.RemoveAt(i);
                        objAttachment1.amoun = Session["cbfdetails"].ToString();
                        if (AttachDelete.Rows.Count == 0)
                        {
                            Session["attachmentdetails"] = null;
                        }
                    }
                }
            }


            objAttachment1 = objRep.Attachmentname(objAttachment1);
            objAttachment1.amoun = Session["cbfdetails"].ToString();

            if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
            {
                Session["attachment"] = objAttachment1;
                if (objAttachment1.attachment.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            else
            {
                Session["attachmentdetails"] = objAttachment1;
                if (objAttachment1.attachmentCbfdetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
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
        public JsonResult productdetails(CbfDetails Parproduct)
        {
            try
            {
                int data = 0;
                DataTable ObjDtpar = new DataTable();
                ObjDtpar = (DataTable)Session["parcbfdetails"];
                if (ObjDtpar != null)
                {
                    DataRow[] matches = ObjDtpar.Select("productgid='" + Parproduct.productgid + "'");
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
        public PartialViewResult BranchDetailsForcbf(string listfor = null)
        {
            Session["tempbranchser"] = "First";
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
            if (objbanklist.Count > 0)
            {
                Session["branchgidforpr"] = objbanklist[0].branchgid;
            }
            
            return PartialView("BranchDetailsForcbf", objbanklist);
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
            //return Json(objowner, JsonRequestBehavior.AllowGet);
            return PartialView("BranchDetailsForcbf", objowner);
        }
        [HttpPost]
        public JsonResult GetEmployeeBranchGid()   
        {
            try
            {
                return Json(objRep.getbranchForLoginUser(), JsonRequestBehavior.AllowGet);
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
                data.attachment = objRep.Getattachment(lnParDetail.ToString(), "12", "2");
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
        public PartialViewResult CBFAttachmentDetails() 
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult CBFAttachmentFields() 
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult CBFAttachmentIndex(string rowum = null,string cbfdetid = null, string viewtype = null) 
        {
            try 
            {
                TempData["cbfattachmentviewmode"] = null;
                Session["rownum"] = null;
                Session["cbfdetidforattachment"] = null;
                if (rowum != null && rowum !="")
                {
                    ViewBag.InlineAttachmentGid = rowum;
                    Session["rownum"] = Convert.ToInt32(rowum);
                }
                if (cbfdetid != null && cbfdetid !="")
                {
                    ViewBag.InlineAttachmentGid = cbfdetid;
                    Session["cbfdetidforattachment"] = Convert.ToInt32(cbfdetid);
                }
                if (viewtype == "view")
                {
                    ViewBag.cbfattachmentviewmode = "view";
                }
                else if (viewtype == "edit")
                {
                    ViewBag.cbfattachmentviewmode = "edit";
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            CbfSumEntity mod = new CbfSumEntity();
            return PartialView(mod);
        }
        [HttpPost]
        public JsonResult DeleteCBFAttachment(CbfSumEntity objCBFAttachment)  
        {
            try
            {
                int cbfattachmentId = (int)objCBFAttachment._CBFAttachmentID;
                objRep.DeleteCBFAttachment(cbfattachmentId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
          
        }

        [HttpPost]
        public JsonResult CreateCBFAttachment(CbfSumEntity objCBFAttachment) 
        {
            try
            {
                if (Session["cbfdetidforattachment"] != null)
                {
                    objCBFAttachment._CBFAttachmentID = (int)Session["cbfdetidforattachment"];
                    objCBFAttachment._CBFAttachmentFor = "CBFDET";
                }
                else
                {
                    objCBFAttachment._CBFAttachmentID = (int)Session["rownum"];
                    objCBFAttachment._CBFAttachmentFor = "CBFTEMP";
                }
                objRep.InsertCBFAttachment(objCBFAttachment);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }


}
