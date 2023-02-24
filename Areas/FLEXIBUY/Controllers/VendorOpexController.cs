using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class VendorOpexController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        public VendorOpexController()
            : this(new CbfSumModel())
        {

        }
        public VendorOpexController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }

        public ActionResult Index()
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            try
            {
                Session["opex_parcbfdetails"] = null;
                Session["opex_da"] = null;
                Session["opex_parheadersession"] = null;
                Session["opex_objowner"] = null;
                Session["opex_Prdetailsfor"] = null;
                Session["opex_prheadersession"] = null;
                Session["opexparheaderra"] = null;
                Session["opex_prd"] = null;
                Session["opex_idprdetails"] = null;
                Session["opex_cbfprdeta"] = null;
                Session["opex_pardetails_gid"] = null;
                Session["Requestvalue"] = null;
                Session["vendorbudget"] = null;
                Session["prheadersessionvv"] = null;
                Session["parheadersessionvv"] = null;
                objDetails = new CbfSumEntity()

               {
                   requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname"),
                   projectOwner1 = new SelectList(objRep.GetList(), "projectownergid", "projectownername"),
                   branchCode1 = new SelectList(objRep.GetBranchCode(), "branchcodegid", "branchcodename"),
               };
                objDetails.result = objRep.GetReqGroup();
                string levelreq = objDetails.result;
                string reqidvl = objRep.Rquestvalue(levelreq);
                //ViewBag.Requestvalue = reqidvl;
                Session["Requestvalue"] = reqidvl;
                if (Convert.ToString(Session["vendorbudget"]) == "")
                {
                    Session["vendorbudget"] = "Y";
                }
                else
                {
                    Session["vendorbudget"] = "N";
                }
                return View(objDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(objDetails);
            }
        }
        [HttpGet]
        public PartialViewResult vendorparheader(string id, string viewfor)
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfPrHeader cbfhead = new CbfPrHeader();
                cbfhead.branch = Session["Requestvalue"].ToString();
                cbfhead.dept = id;
                Session["vendorbudget"] = id;
                //string jj = Convert.ToString(Session["vendorbudget"]);
                //if (Convert.ToString(Session["vendorbudget"]) == "" )
                //{
                //    Session["vendorbudget"] = "Y";
                //    cbfhead.dept = Session["vendorbudget"].ToString();
                //}
                //else
                //{
                //    Session["vendorbudget"] = "N";
                //    cbfhead.dept = Session["vendorbudget"].ToString();
                //}
                objDetails = objRep.GetParHeaderopex(cbfhead);
                Session["opex_parheadersession"] = objDetails;
                if (objDetails.cbfParheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(objDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objDetails);
            }
        }
        [HttpPost]
        public JsonResult vendorparheadersearch(CbfParHeader prheader)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                CbfPrHeader cbfhead = new CbfPrHeader();
                cbfhead.branch = Session["Requestvalue"].ToString();
                cbfhead.dept = Session["vendorbudget"].ToString();
                objSumEntity = objRep.GetParHeaderopex(cbfhead);
                int lsRequest1 = 0;
                if (prheader.parAmt != 0)
                    lsRequest1 = Convert.ToInt32(prheader.parAmt);
                if ((string.IsNullOrEmpty(prheader.parNo)) == false)
                {
                    ViewBag.txtparno = prheader.parNo;
                    objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => prheader.parNo == null ||
                        (x.parNo.ToUpper().Contains(prheader.parNo.ToUpper()))).ToList();
                }
                if (!(string.IsNullOrEmpty(prheader.parDate)))
                {
                    ViewBag.txtpardate = prheader.parDate;
                    objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => prheader.parDate == null ||
                        (x.parDate.ToUpper().Contains(prheader.parDate.ToUpper()))).ToList();
                }
                if (lsRequest1.ToString() != "0")
                {
                    ViewBag.txtparamount = lsRequest1;
                    objSumEntity.cbfParheader = objSumEntity.cbfParheader.Where(x => lsRequest1 == null
                        || (x.parAmt.Equals(lsRequest1))).ToList();
                }
                if (objSumEntity.cbfParheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No records found";
                }

                Session["parheadersessionvv"] = objSumEntity;
                return Json(objSumEntity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objSumEntity, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult vendorprheadersearch(CbfPrHeader prheader)
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                CbfPrHeader cbfhead = new CbfPrHeader();
                int prreq = Convert.ToInt32(Session["Requestvalue"].ToString());
                objSumEntity = objRep.GetPrHeaderopex(prreq);

                if ((string.IsNullOrEmpty(prheader.prNo)) == false)
                {
                    ViewBag.txtprno = prheader.prNo;
                    objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => prheader.prNo == null ||
                        (x.prNo.ToUpper().Contains(prheader.prNo.ToUpper()))).ToList();
                }
                if (!(string.IsNullOrEmpty(prheader.prDate)))
                {
                    ViewBag.txtprdate = prheader.prDate;
                    objSumEntity.cbfPrheader = objSumEntity.cbfPrheader.Where(x => prheader.prDate == null ||
                        (x.prDate.ToUpper().Contains(prheader.prDate.ToUpper()))).ToList();
                }

                if (objSumEntity.cbfPrheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No records found";
                }

                Session["prheadersessionvv"] = objSumEntity;
                return Json(objSumEntity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objSumEntity, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult CbfSearchPR(string listfor = null)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                if (listfor == "search")
                {
                    if (Session["prheadersessionvv"] != null)
                    {
                        objSumEntity = (CbfSumEntity)Session["prheadersessionvv"];
                    }
                }
                else
                {
                    Session["prheadersessionvv"] = "";
                    //  objSumEntity = objRep.GetParHeader();
                }
                if (objSumEntity.cbfPrheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorprheader", objSumEntity);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorprheader", objSumEntity);
            }
        }
        [HttpGet]
        public PartialViewResult CbfSearchPAR(string listfor = null)
        {
            ViewBag.NoRecordsFound = "";
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                if (listfor == "search")
                {
                    if (Session["parheadersessionvv"] != null)
                    {
                        objSumEntity = (CbfSumEntity)Session["parheadersessionvv"];
                    }
                }
                else
                {
                    Session["parheadersessionvv"] = "";
                    //  objSumEntity = objRep.GetParHeader();
                }
                if (objSumEntity.cbfParheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorparheader", objSumEntity);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorparheader", objSumEntity);
            }
        }
        [HttpGet]
        public PartialViewResult vendorprheader(string lsListFor = null)
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                if (lsListFor == "search")
                {
                    if (Session["opex_objowner"] != null)
                        objDetails = (CbfSumEntity)Session["opex_objowner"];
                    if (objDetails.cbfPrheader.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                else if (lsListFor == "refresh")
                {
                    if (Session["opex_objowner"] != null)
                        objDetails = (CbfSumEntity)Session["opex_objowner"];
                    if (objDetails.cbfPrheader.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                else
                {
                    Session["opex_objowner"] = "";
                    objDetails = (CbfSumEntity)Session["opex_prheadersession"];
                    if (objDetails.cbfPrheader.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                return PartialView(objDetails);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objDetails);
            }

        }


        [HttpPost]
        public JsonResult vendorprheader(CbfPrHeader objPrheader)
        {
            CbfSumEntity objDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                Session["opex_Prdetailsfor"] = objPrheader.prRequestFor;
                objDetails = objRep.GetPrHeaderopex(Convert.ToInt16(Session["opex_Prdetailsfor"]));
                Session["opex_prheadersession"] = objDetails;
                if (objDetails.cbfPrheader.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }

                return Json(objDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objDetails, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult vendorpardetails()
        {
            CbfSumEntity sumentity = new CbfSumEntity();
            try
            {

                ViewBag.NoRecordsFound = "";

                sumentity = (CbfSumEntity)Session["opexparheaderra"];
                Session["opexparheaderra"] = sumentity;
                if (sumentity.cbfPardetailslst.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(sumentity);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(sumentity);
            }
        }
        [HttpPost]
        public PartialViewResult vendorpardetails(CbfParHeader objpar)
        {
            CbfSumEntity sumentity = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                ViewBag.parheadgid = objpar.parGid;
                sumentity = objRep.VendorPardetails(objpar);
                Session["opexparheaderra"] = sumentity;
                if (sumentity.cbfPardetailslst.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView(sumentity);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(sumentity);
            }

        }

        [HttpGet]
        public PartialViewResult vendorprdetails()
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                objGetPrDetails = (CbfSumEntity)Session["opex_prd"];
                if (objGetPrDetails.cbfPrdetarils.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorprdetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorprdetails", objGetPrDetails);
            }
        }
        [HttpPost]
        public PartialViewResult vendorprdetails(CbfPrHeader objPrHeGid)
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                Session["opex_idprdetails"] = objPrHeGid.prhed_Gid;

                objGetPrDetails = objRep.GetPrDetailopex(objPrHeGid);
                Session["opex_prd"] = objGetPrDetails;
                if (objGetPrDetails.cbfPrdetarils.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }

                return PartialView("vendorprdetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorprdetails", objGetPrDetails);
            }
        }
        [HttpGet]
        public PartialViewResult vendorcbfdetails()
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                objGetPrDetails = (CbfSumEntity)Session["opex_cbfprdeta"];
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorcbfdetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorcbfdetails", objGetPrDetails);
            }
        }
        [HttpPost]
        public PartialViewResult vendorcbfdetails(CbfDetails prdelgid)
        {
            CbfSumEntity objgetprdetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();

                if (Session["opex_parcbfdetails"] != null)
                {
                    objdt1 = (DataTable)Session["opex_parcbfdetails"];
                }
                else
                {
                    objdt = objRep.DtTable(prdelgid);
                }
                if (objdt != null)
                    objdt1.Merge(objdt);
                if (Session["opex_parcbfdetails"] != null)
                {
                    objgetprdetails = objRep.GetCbfParDetails(objdt1);
                }
                else
                {
                    objgetprdetails = objRep.GetCbfDetails(objdt1);
                }
                Session["opex_cbfprdeta"] = objgetprdetails;
                objgetprdetails.productGroupList = new SelectList(objRep.GetServicelist(),
                    "productGroupId", "productGroupIdName");
                Session["opex_pardetails_gid"] = prdelgid.productCode.ToString();
                ViewBag.pardetGid = prdelgid.prdetailsGid.ToString();
                ViewBag.lnamt = objRep.VendorPardetails_id(prdelgid.productCode.ToString());
                if (objgetprdetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorcbfdetails", objgetprdetails);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorcbfdetails", objgetprdetails);
            }
        }
        [HttpPost]
        public JsonResult productdetails(CbfDetails Parproduct)
        {
            try
            {
                int data = 0;
                DataTable ObjDtpar = new DataTable();
                ObjDtpar = (DataTable)Session["opex_parcbfdetails"];
                if (ObjDtpar != null)
                {
                    DataRow[] matches = ObjDtpar.Select("productgid='" + Parproduct.productgid + "'");
                    data = matches.Count();
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult CbfParSave(CbfDetails pardetails)
        {
            CbfSumEntity objcbf = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                DataTable objDt = new DataTable();
                if (Session["opex_parcbfdetails"] != null)
                    objDt = (DataTable)Session["opex_parcbfdetails"];
                if (Session["opex_parcbfdetails"] == null)
                {
                    objDt.Columns.Add("sno", typeof(int));
                    objDt.Columns.Add("prdetails_gid", typeof(decimal));
                    objDt.Columns.Add("productgroup", typeof(string));
                    objDt.Columns.Add("productgid", typeof(string));
                    objDt.Columns.Add("productcode", typeof(string));
                    objDt.Columns.Add("productname", typeof(string));
                    objDt.Columns.Add("productdescription", typeof(string));
                    objDt.Columns.Add("qty", typeof(int));
                    objDt.Columns.Add("unitamount", typeof(decimal));
                    objDt.Columns.Add("total", typeof(decimal));
                    objDt.Columns.Add("Vendorname", typeof(string));
                    objDt.Columns.Add("Vendorgid", typeof(int));
                }
                if (pardetails.unChecked12.ToString() == null || pardetails.unChecked12 == 0)
                {
                    int sno = Convert.ToInt32(Session["sno"]) + 1;
                    Session["sno"] = sno;
                    objDt.Rows.Add(sno, pardetails.prdetailsGid, pardetails.productGroupId, pardetails.productgid,
                     pardetails.productCode, pardetails.productName, pardetails.description,
                     pardetails.qty, pardetails.unitAmt, pardetails.total, pardetails.vendorselection, pardetails.vendorgid);
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
                            objDt.Rows[i]["qty"] = pardetails.qty;
                            objDt.Rows[i]["unitamount"] = pardetails.unitAmt;
                            objDt.Rows[i]["total"] = pardetails.total;
                            objDt.Rows[i]["Vendorname"] = pardetails.vendorselection;
                            objDt.Rows[i]["Vendorgid"] = pardetails.vendorgid;

                        }
                    }

                }

                Session["opex_parcbfdetails"] = objDt;
                objcbf = objRep.GetCbfParDetailsOpex(objDt);
                objcbf.selectedValue = pardetails.productGroupId.ToString();
                objcbf.productGroupList = new SelectList(objRep.GetServicelist(), "productGroupId", "productGroupIdName");

                if (objcbf.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("vendorcbfdetails", objcbf);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorcbfdetails", objcbf);
            }
        }
        [HttpPost]
        //public PartialViewResult Vendorcbfprdetails(CbfDetails objPrDelGid = null, CbfDetails objUnchecked1=null)
        public PartialViewResult Vendorcbfprdetails(CbfDetails objPrDelGid = null, CbfDetails objUnchecked1 = null)
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objDt = new DataTable();
                DataTable objDt1 = new DataTable();

                if (Session["opex_da"] != null)
                    objDt1 = (DataTable)Session["opex_da"];

                if (objPrDelGid.prdetailsGid != 0)
                {
                    if (objDt1.Columns.Count > 0)
                    {

                        DataView dv = new DataView();
                        dv = objDt1.DefaultView;
                        dv.RowFilter = "prdetails_gid =" + objUnchecked1.prdetailsGid + "";
                        objDt = dv.ToTable();
                        if (objDt1.Columns.Contains("Vendorname"))
                        {

                            DataRow[] matches = objDt1.Select("Vendorname is null");
                            foreach (DataRow row in matches)
                            {
                                objDt1.Rows.Remove(row);
                            }
                        }
                    }
                    if (objDt.Rows.Count == 0)
                    {
                        objDt = null;
                        objDt = objRep.DtTableOPex(objPrDelGid);
                        if (objDt1.Columns.Contains("Vendorname"))
                        {
                            for (int i = 0; i < objDt1.Rows.Count; i++)
                            {
                                if (objDt1.Rows[i]["Vendorname"].ToString() == "")
                                {
                                    objDt1.Rows.RemoveAt(i);
                                }

                            }
                            objDt1.Merge(objDt);
                            Session["opex_da"] = objDt1;
                        }
                        else
                        {
                            Session["opex_da"] = objDt;
                            objDt1 = objDt;
                        }


                        Session["opex_prdetails_gid"] = objUnchecked1.prdetailsGid;
                    }
                    objGetPrDetails = objRep.GetCbfDetailsOpex(objDt1);
                }
                else if (objUnchecked1.unChecked12 != 0)
                {
                    DataView dv = new DataView();
                    dv = objDt1.DefaultView;

                    dv.RowFilter = "prdetails_gid =" + objUnchecked1.unChecked12 + "";
                    objDt = dv.ToTable();

                    if (objDt.Columns.Contains("Vendorname"))
                    {
                        if (objDt.Rows[0]["Vendorname"].ToString() != "")
                        {
                            objGetPrDetails = objRep.GetCbfDetailsOpex(objDt1);
                        }
                        else
                        {
                            Session["opex_da"] = objDt1;
                            dv = objDt1.DefaultView;
                            dv.RowFilter = "prdetails_gid <>" + objUnchecked1.unChecked12 + "";
                            DataRow[] matches = objDt1.Select("prdetails_gid='" + objUnchecked1.unChecked12 + "'");
                            foreach (DataRow row in matches)
                            {
                                objDt1.Rows.Remove(row);
                            }

                            objGetPrDetails = objRep.GetCbfDetailsOpex(objDt1);
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
                        objGetPrDetails = objRep.GetCbfDetailsOpex(objDt1);
                    }


                }
                Session["opex_cbfprdeta"] = objGetPrDetails;
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                //  return Json(objGetPrDetails, JsonRequestBehavior.AllowGet);
                return PartialView("vendorcbfprdetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorcbfprdetails", objGetPrDetails);
            }
        }
        [HttpPost]
        public PartialViewResult CbfPrSave(CbfDetails objUserModel)
        {
            CbfSumEntity objGetPrdetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();

                if (Session["opex_da"] != null)
                    objdt1 = (DataTable)Session["opex_da"];
                if (objdt1.Columns.Count <= 13)
                {
                    objdt1.Columns.Add("fccc", typeof(int));
                    objdt1.Columns.Add("budgetline", typeof(int));
                    objdt1.Columns.Add("myref", typeof(int));
                    objdt1.Columns.Add("Vendorname", typeof(string));
                    objdt1.Columns.Add("Vendorgid", typeof(int));

                }
                for (int i = 0; i < objdt1.Rows.Count; i++)
                {
                    if (objdt1.Rows[i]["prdetails_gid"].ToString() == objUserModel.unChecked12.ToString())
                    {
                        // objdt1.Rows[i]["remarks"] = objUserModel.remarks;
                        objdt1.Rows[i]["prdetails_qty"] = objUserModel.qty;
                        objdt1.Rows[i]["pipinput_rate"] = objUserModel.unitAmt;
                        objdt1.Rows[i]["pipinput_costestimation"] = objUserModel.total;
                        objdt1.Rows[i]["fccc"] = objUserModel.fccc;
                        objdt1.Rows[i]["budgetline"] = objUserModel.budgetLine;
                        objdt1.Rows[i]["Vendorname"] = objUserModel.vendorselection;
                        objdt1.Rows[i]["Vendorgid"] = objUserModel.vendorgid;
                        //  objdt1.Rows[i]["myref"] = 1;
                    }
                }
                Session["opex_da"] = objdt1;
                objGetPrdetails = objRep.GetCbfDetailsOpex(objdt1);
                Session["cbfprdeta"] = objGetPrdetails;
                if (objGetPrdetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("Vendorcbfprdetails", objGetPrdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("Vendorcbfprdetails", objGetPrdetails);
            }
        }
        [HttpGet]
        public PartialViewResult Vendorcbfprdetails()
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                objGetPrDetails = (CbfSumEntity)Session["opex_cbfprdeta"];
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("Vendorcbfprdetails", objGetPrDetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("Vendorcbfprdetails", objGetPrDetails);
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
                    objdt = (DataTable)Session["opex_da"];
                }
                else if (objClassificationModel.cbfMode == "PAR")
                {
                    objdt = (DataTable)Session["opex_parcbfdetails"];
                }
                string data = objRep.InsertCbfHeaderopex(objClassificationModel, objdt);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult CbfSave(CbfDetails objUserModel)
        {
            CbfSumEntity objGetPrdetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();

                if (Session["opex_da"] != null)
                    objdt1 = (DataTable)Session["opex_da"];
                if (objdt1.Columns.Count <= 10)
                {
                    objdt1.Columns.Add("Vendorname", typeof(string));
                    objdt1.Columns.Add("Vendorgid", typeof(int));
                }
                for (int i = 0; i < objdt1.Rows.Count; i++)
                {
                    if (objdt1.Rows[i]["prdetails_gid"].ToString() == objUserModel.unChecked12.ToString())
                    {

                        objdt1.Rows[i]["Vendorname"] = objUserModel.vendorselection;
                        objdt1.Rows[i]["Vendorgid"] = objUserModel.vendorgid;
                    }
                }
                Session["opex_da"] = objdt1;
                objGetPrdetails = objRep.GetCbfDetailsOpex(objdt1);
                Session["opex_cbfprdeta"] = objGetPrdetails;
                if (objGetPrdetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("Vendorcbfprdetails", objGetPrdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("Vendorcbfprdetails", objGetPrdetails);
            }
        }

        [HttpPost]
        public PartialViewResult CbfDelete(CbfDetails objUserModel)
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                DataTable objDt = new DataTable();
                DataTable AttachDelete = new DataTable();
                if (Session["opex_da"] != null)
                    objDt = (DataTable)Session["opex_da"];

                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    if (objDt.Rows[i]["prdetails_gid"].ToString() == objUserModel.prdetailsGid.ToString())
                        objDt.Rows.RemoveAt(i);

                }
                objGetPrDetails = objRep.GetCbfDetails(objDt);
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                Session["opex_cbfprdeta"] = objGetPrDetails;
                return PartialView("Vendorcbfprdetails", objGetPrDetails);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("Vendorcbfprdetails", objGetPrDetails);
            }

        }
        [HttpPost]
        public PartialViewResult CbfDeletePar(CbfDetails objUserModel)
        {
            CbfSumEntity objGetPrDetails = new CbfSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                DataTable objDt = new DataTable();
                if (Session["opex_parcbfdetails"] != null)
                    objDt = (DataTable)Session["opex_parcbfdetails"];
                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    if (objDt.Rows[i]["sno"].ToString() == objUserModel.attch_Gid.ToString())
                        objDt.Rows.RemoveAt(i);

                }
                objGetPrDetails = objRep.GetCbfParDetails(objDt);
                Session["opex_parcbfdetails"] = objDt;
                objGetPrDetails.productGroupList = new SelectList(objRep.Getproductlist(), "productGroupId", "productGroupIdName");
                objGetPrDetails.uomList = new SelectList(objRep.GetUomList(), "uomGid", "uomCode");
                if (objGetPrDetails.cbfDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                Session["opex_parcbfdetails_list"] = objGetPrDetails;
                return PartialView("vendorcbfdetails", objGetPrDetails);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("vendorcbfdetails", objGetPrDetails);
            }

        }

        [HttpGet]
        public PartialViewResult VendorSelection(string listfor)
        {
            List<vendorselection> lstVendor = new List<vendorselection>();
            try
            {
                if (listfor == "search")
                {
                    if (Session["objvendoropexpar"] != null)
                        lstVendor = (List<vendorselection>)Session["objvendoropexpar"];
                    if (TempData["Norecords"] != null)
                        TempData["records"] = TempData["Norecords"];
                    if (TempData["vendorcode"] != null)
                        TempData["code"] = TempData["vendorcode"];
                    if (TempData["vendorname"] != null)
                        TempData["name"] = TempData["vendorname"];
                }
                else
                {
                    Session["objvendoropexpar"] = null;
                    lstVendor = objRep.getvendorselection();
                }
                return PartialView("VendorSelection", lstVendor);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("VendorSelection", lstVendor);
            }
        }

        [HttpGet]
        public PartialViewResult VendorSelectionForPr(string listfor)
        {
            List<vendorselection> lstVendor = new List<vendorselection>();
            try
            {
                if (listfor == "search")
                {
                    if (Session["objvendoropexpr"] != null)
                        lstVendor = (List<vendorselection>)Session["objvendoropexpr"];
                    if (TempData["Norecords"] != null)
                        TempData["records"] = TempData["Norecords"];
                    if (TempData["vendorcode"] != null)
                        TempData["code"] = TempData["vendorcode"];
                    if (TempData["vendorname"] != null)
                        TempData["name"] = TempData["vendorname"];
                }
                else
                {
                    Session["objvendoropexpr"] = null;
                    lstVendor = objRep.getvendorselection();
                }
                return PartialView("VendorSelectionForPr", lstVendor);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("VendorSelectionForPr", lstVendor);
            }
        }

        [HttpPost]
        public PartialViewResult searchvendor(vendorselection objsearchfilter)
        {
            List<vendorselection> objvendorsearch = new List<vendorselection>();
            try
            {
                objvendorsearch = objRep.getvendorselection();
                if ((string.IsNullOrEmpty(objsearchfilter.vendorcode)) == false)
                {
                    TempData["vendorcode"] = objsearchfilter.vendorcode;
                    objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorcode == null ||
                        (x.vendorcode.ToUpper().Contains(objsearchfilter.vendorcode.ToUpper()))).ToList();
                    Session["objvendoropexpr"] = objvendorsearch;
                }
                if ((string.IsNullOrEmpty(objsearchfilter.vendorname)) == false)
                {
                    TempData["vendorname"] = objsearchfilter.vendorname;
                    objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorname == null ||
                        (x.vendorname.ToUpper().Contains(objsearchfilter.vendorname.ToUpper()))).ToList();
                    Session["objvendoropexpr"] = objvendorsearch;
                }
                if (objvendorsearch.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
                return PartialView("VendorSelectionForPr", objvendorsearch);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("VendorSelectionForPr", objvendorsearch);
            }
        }

        [HttpPost]
        public PartialViewResult searchvendorforpar(vendorselection objsearchfilter)
        {
            List<vendorselection> objvendorsearch = new List<vendorselection>();
            try
            {
                objvendorsearch = objRep.getvendorselection();
                if ((string.IsNullOrEmpty(objsearchfilter.vendorcode)) == false)
                {
                    TempData["vendorcode"] = objsearchfilter.vendorcode;
                    objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorcode == null ||
                        (x.vendorcode.ToUpper().Contains(objsearchfilter.vendorcode.ToUpper()))).ToList();
                    Session["objvendoropexpar"] = objvendorsearch;
                }
                if ((string.IsNullOrEmpty(objsearchfilter.vendorname)) == false)
                {
                    TempData["vendorname"] = objsearchfilter.vendorname;
                    objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorname == null ||
                        (x.vendorname.ToUpper().Contains(objsearchfilter.vendorname.ToUpper()))).ToList();
                    Session["objvendoropexpar"] = objvendorsearch;
                }
                if (objvendorsearch.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
                return PartialView("VendorSelection", objvendorsearch);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("VendorSelection", objvendorsearch);
            }
        }

        [HttpPost]
        public JsonResult GetProductGroup()
        {
            return Json(objRep.GetServicelist(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetProductList(int lnid)
        {

            CbfSumEntity objM = new CbfSumEntity();
            try
            {
                objM = objRep.GetProdServDetails(lnid);
                HttpContext.Session["opex_ProductGroupgid"] = lnid;

                return PartialView(objM);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objM);
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
            ViewBag.NoRecordsFound = "";
            DataTable AttachDelete;
            CbfSumEntity objAttachment1 = new CbfSumEntity();
            try
            {
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult PROpexAttachmentDetails(int prdetid = 0) 
        {
            ViewBag.prdetid = prdetid;
            return PartialView();
        }

    }
}
