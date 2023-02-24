using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Data;
using IEM.Areas.ASMS.Models;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{
      [NoDirectAccess]
    public class SupplierEvaluationController : Controller
    {
        //
        // GET: /asms_trn_supevaluation/
        private ASMSIRepository ist;
        private Asms_IRepository mst;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public SupplierEvaluationController() :
            this(new ASMSDataModel(), new Asms_DataModel()) { }

        public SupplierEvaluationController(ASMSIRepository Objist, Asms_IRepository od)
        {
            ist = Objist;
            mst = od;
        }
        //public PartialViewResult Pending(string str = null)
        //{

        //    ViewBag.search = str;
        //    string mt = string.Empty;
        //    SEModel revirew = new SEModel();
        //    if (Session["cDownload"] == null)
        //    {
        //        revirew.modelPList = ist.GetSEModelPending(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //    }
        //    else
        //    {
        //        revirew.modelPList = (List<SEModel>)Session["pDownload"];
        //    }
        //    if (revirew.modelPList.Count() == 0)
        //    {
        //        ViewBag.Message = "No records found";
        //    }
        //    return PartialView(revirew);
        //}
        //[HttpPost]
        //public PartialViewResult Pending(string txtPenFilter, string txtCodePenFilter, int txtYear, string pending)
        //{
        //    Session["pDownload"] = null;
        //    SEModel revirew = new SEModel();
        //    if (string.IsNullOrEmpty(txtPenFilter) || string.IsNullOrWhiteSpace(txtPenFilter) || txtYear == 0)
        //    {
        //        revirew.modelPList = ist.GetSEModelPending(txtPenFilter, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //    }

        //    if (string.IsNullOrEmpty(txtPenFilter) == false || string.IsNullOrWhiteSpace(txtPenFilter) == false)
        //    {
        //        revirew.modelPList = ist.GetSEModelPending(txtPenFilter, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //        ViewBag.txtPenFilter = txtPenFilter.ToString();
        //    }
        //    if (string.IsNullOrEmpty(txtCodePenFilter) == false || string.IsNullOrWhiteSpace(txtCodePenFilter) == false)
        //    {
        //        revirew.modelPList = revirew.modelPList.Where(x => txtCodePenFilter.ToUpper() == null ||
        //               (x.seSupCode.ToUpper().Contains(txtCodePenFilter.ToUpper()))).ToList();
        //        ViewBag.txtCodePenFilter = txtCodePenFilter.ToString();
        //    }
        //    if (txtYear != 0)
        //    {
        //        revirew.modelPList = revirew.modelPList.Where(x => txtYear == 0 ||
        //               (x.seYear.ToString().Contains(txtYear.ToString()))).ToList();
        //        ViewBag.txtYear = txtYear.ToString();
        //    }

        //    if (revirew.modelPList.Count() == 0)
        //    {
        //        ViewBag.search = "";
        //        ViewBag.PendMessage = "No records found";
        //    }
        //    Session["pDownload"] = revirew.modelPList;
        //    return PartialView(revirew);
        //}
        //public PartialViewResult Completed(string str = null)
        //{
        //    ViewBag.search = str;
        //    string mt = string.Empty;
        //    SEModel revirew = new SEModel();
        //    if (Session["cDownload"] == null)
        //    {
        //        revirew.modelCList = ist.GetSEModelCompleted(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //    }
        //    else
        //    {
        //        revirew.modelCList = (List<SEModel>)Session["cDownload"];
        //    }
        //    if (revirew.modelCList.Count() == 0)
        //    {
        //        ViewBag.Message = "No records found";
        //    }
        //    return PartialView(revirew);
        //}
        //[HttpPost]
        //public ActionResult Completed(string txtCompFilter, string command, string txtCompCodeFilter,int txtYear,  int period, string pending)
        //{
        //    SEModel revirew = new SEModel();
        //    if (string.IsNullOrEmpty(txtCompFilter) || string.IsNullOrWhiteSpace(txtCompFilter))
        //    {
        //        revirew.modelCList = ist.GetSEModelCompleted(txtCompFilter, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //    }
        //    if (string.IsNullOrEmpty(txtCompFilter) == false || string.IsNullOrWhiteSpace(txtCompFilter) == false)
        //    {
        //        revirew.modelCList = ist.GetSEModelCompleted(txtCompFilter, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
        //        ViewBag.txtCompFilter = txtCompFilter.ToString();
        //    }
        //    if (string.IsNullOrEmpty(txtCompCodeFilter) == false || string.IsNullOrWhiteSpace(txtCompCodeFilter) == false)
        //    {
        //        revirew.modelCList = revirew.modelCList.Where(x => txtCompCodeFilter.ToUpper() == null ||
        //               (x.seSupCode.ToUpper().Contains(txtCompCodeFilter.ToUpper()))).ToList();
        //        ViewBag.txtCompCodeFilter = txtCompCodeFilter.ToString();
        //    }
        //    if (txtYear != 0)
        //    {
        //        revirew.modelPList = revirew.modelPList.Where(x => txtYear == 0 ||
        //               (x.seYear.ToString().Contains(txtYear.ToString()))).ToList();
        //        ViewBag.txtYear = txtYear.ToString();
        //    }
        //    if (period != 0)
        //    {
        //        revirew.modelCList = revirew.modelCList.Where(x => period == 0 ||
        //               (x.seYear.ToString().Contains(period.ToString()))).ToList();
        //        ViewBag.period = period.ToString();
        //    }
        //    if (revirew.modelCList.Count() == 0)
        //    {
        //        ViewBag.Message = "No records found";
        //    }
        //    if (pending == "Completed") { ViewBag.search = "Completed"; }
        //    ViewBag.search = "Completed";
        //    Session["cDownload"] = revirew.modelCList;
        //    return PartialView(revirew);
        //}

        public ActionResult Index(string pending = null, string command = null)
        {
            SEModel revirew = new SEModel();
            try
            {
                if (pending == "Pending") { ViewBag.search = ""; }
                if (pending == "Completed") { ViewBag.search = "Completed"; }
                Session["cYEAR"] = "0";
                Session["pYEAR"] = "0";
                Session["txtPenFilter"] = "";
                Session["searchthing"] = "";
                Session["txtCodePenFilter"] = "";
                string mt = string.Empty;

                revirew.modelCList = ist.GetSEModelCompleted(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                if (revirew.modelCList.Count() == 0)
                {
                    ViewBag.Message = "No records found";
                }
                revirew.modelPList = ist.GetSEModelPending(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                if (revirew.modelPList.Count() == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            
            return View(revirew);
        }

        [HttpPost]
        public ActionResult Index(SEModel modelComp=null, SEModel modelPen=null)
        {
            SEModel revirew = new SEModel();
            try
            {
                if (modelPen.seSupCode == null && Session["txtCodePenFilter"] != null)
                {
                    modelPen.seSupCode = Session["txtCodePenFilter"].ToString();
                }
                else if (modelPen.seSupName == null && Session["txtPenFilter"] != null)
                {
                    modelPen.seSupName = Session["txtPenFilter"].ToString();
                }
                else if (modelPen.seYear == 0 && Session["pYEAR"] != "0")
                {
                    modelPen.seYear = Convert.ToInt32(Session["pYEAR"]);
                }
                else if (modelComp.SheetName == null && Session["PendSearch"] != null)
                {
                    modelComp.SheetName = Session["PendSearch"].ToString();
                }
                else
                {
                    Session["txtCodePenFilter"] = modelPen.seSupCode;
                    Session["txtPenFilter"] = modelPen.seSupName;
                    Session["pYEAR"] = modelPen.seYear;
                    Session["PendSearch"] = modelComp.SheetName;
                }
                revirew.modelCList = ist.GetSEModelCompleted(modelComp.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                revirew.modelPList = ist.GetSEModelPending(modelPen.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                if (modelPen.SheetName == null)
                {
                    modelPen.SheetName = Convert.ToString(Session["searchthing"]);
                }
                else
                {
                    Session["searchthing"] = modelPen.SheetName;
                }
                if (modelPen.SheetName == "Pending")
                {
                    ViewBag.search = "";

                    if (string.IsNullOrEmpty(modelPen.seSupName) || string.IsNullOrWhiteSpace(modelPen.seSupName) || string.IsNullOrEmpty(modelComp.seSupName) || string.IsNullOrWhiteSpace(modelComp.seSupName))
                    {
                        revirew.modelCList = ist.GetSEModelCompleted(modelComp.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                        revirew.modelPList = ist.GetSEModelPending(modelPen.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                    }

                    if (string.IsNullOrEmpty(modelPen.seSupName) == false || string.IsNullOrWhiteSpace(modelPen.seSupName) == false)
                    {
                        revirew.modelPList = ist.GetSEModelPending(modelPen.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                        Session["txtPenFilter"] = modelPen.seSupName.ToString();
                    }

                    if (string.IsNullOrEmpty(modelPen.seSupCode) == false || string.IsNullOrWhiteSpace(modelPen.seSupCode) == false)
                    {
                        revirew.modelPList = revirew.modelPList.Where(x => modelPen.seSupCode.ToUpper() == null ||
                               (x.seSupCode.ToUpper().Contains(modelPen.seSupCode.ToUpper()))).ToList();
                        Session["txtCodePenFilter"] = modelPen.seSupCode.ToString();
                    }

                    if (Session["pYEAR"] == null || Session["pYEAR"] == "0")
                    {
                        revirew.modelPList = revirew.modelPList.Where(x => modelPen.seYear == 0 ||
                            (x.seYear.ToString().Contains(modelPen.seYear.ToString()))).ToList();
                        ViewBag.pYear = modelPen.seYear.ToString();
                        Session["pYEAR"] = modelPen.seYear.ToString();
                    }
                    else
                    {
                        if (modelPen.seYear != 0)
                        {
                            revirew.modelPList = revirew.modelPList.Where(x => modelPen.seYear == 0 ||
                                   (x.seYear.ToString().Contains(modelPen.seYear.ToString()))).ToList();
                            ViewBag.pYear = modelPen.seYear.ToString();
                            Session["pYEAR"] = modelPen.seYear.ToString();
                        }
                        if (Session["pYEAR"] != null || Session["pYEAR"] != "0")
                        {
                            revirew.modelPList = revirew.modelPList.Where(x => Session["pYEAR"] == "0" ||
                                (x.seYear.ToString().Contains(Session["pYEAR"].ToString()))).ToList();
                            ViewBag.pYear = Session["pYEAR"].ToString();

                        }
                    }
                    Session["pDownload"] = revirew.modelPList;
                }
                if (modelPen.SheetName == "Completed")
                {
                    ViewBag.search = "Completed";

                    if (string.IsNullOrEmpty(modelPen.seSupName) || string.IsNullOrWhiteSpace(modelPen.seSupName) || string.IsNullOrEmpty(modelComp.seSupName) || string.IsNullOrWhiteSpace(modelComp.seSupName))
                    {
                        revirew.modelCList = ist.GetSEModelCompleted(modelComp.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                        revirew.modelPList = ist.GetSEModelPending(modelPen.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                    }

                    if (string.IsNullOrEmpty(modelComp.seSupName) == false || string.IsNullOrWhiteSpace(modelComp.seSupName) == false)
                    {
                        revirew.modelCList = ist.GetSEModelCompleted(modelComp.seSupName, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                        ViewBag.txtCompFilter = modelComp.seSupName.ToString();
                        ViewBag.search = "Completed";
                    }
                    if (string.IsNullOrEmpty(modelPen.seSupCode) == false || string.IsNullOrWhiteSpace(modelPen.seSupCode) == false)
                    {
                        ViewBag.search = "Completed";
                        revirew.modelCList = revirew.modelCList.Where(x => modelPen.seSupCode.ToUpper() == null ||
                               (x.seSupCode.ToUpper().Contains(modelPen.seSupCode.ToUpper()))).ToList();
                        ViewBag.txtCompCodeFilter = modelPen.seSupCode.ToString();
                    }


                    if (Session["cYEAR"] == null || Session["cYEAR"] == "0")
                    {
                        ViewBag.search = "Completed";
                        revirew.modelCList = revirew.modelCList.Where(x => modelPen.seYear == 0 ||
                               (x.seYear.ToString().Contains(modelPen.seYear.ToString()))).ToList();
                        ViewBag.cYear = modelPen.seYear.ToString();
                        Session["cYEAR"] = modelPen.seYear.ToString();
                    }
                    else
                    {
                        if (modelPen.seYear != 0)
                        {
                            ViewBag.search = "Completed";
                            revirew.modelCList = revirew.modelCList.Where(x => modelPen.seYear == 0 ||
                                   (x.seYear.ToString().Contains(modelPen.seYear.ToString()))).ToList();
                            ViewBag.cYear = modelPen.seYear.ToString();
                            Session["cYEAR"] = modelPen.seYear.ToString();
                        }
                        if (Session["cYEAR"] != null || Session["cYEAR"] != "0")
                        {
                            revirew.modelCList = revirew.modelCList.Where(x => Session["cYEAR"] == "0" ||
                                (x.seYear.ToString().Contains(Session["cYEAR"].ToString()))).ToList();
                            ViewBag.pYear = Session["cYEAR"].ToString();

                        }
                    }
                }
                Session["cDownload"] = revirew.modelCList;
                if (revirew.modelCList.Count() == 0)
                {
                    ViewBag.search = "Completed";
                    ViewBag.Message = "No records found";
                }
                if (revirew.modelPList.Count() == 0)
                {
                    ViewBag.search = "";
                    ViewBag.PendMessage = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return View(revirew);
        }

        [HttpGet]
        public ActionResult Evaluation(int year, int id)
        {
            string mt = null;
            SEModel revirew = new SEModel();
            try
            {
                DataTable dt = new DataTable();
                revirew.seSupID = Convert.ToInt32(id);
                revirew.seYear = Convert.ToInt32(year);
                revirew.seSupName = ist.GetSupbyId(Convert.ToInt32(id)).ToString();
                revirew.seCatModelList = mst.GetSECategoryWithQues().ToList();
                revirew.seSubcatModelList = mst.GetSESubCategory(mt).ToList();
                revirew.seRatList1 = ist.Getscorebygrp(1).ToList();
                revirew.seRateList6 = ist.Getscorebygrp(6).ToList();
                revirew.seRateList6 = ist.Getscorebygrp(6).ToList();
                ViewBag.list1 = ist.Getscorebygrp(1).ToList();
                ViewBag.list6 = ist.Getscorebygrp(6).ToList();
                dt = objCmnFunctions.GetLoginUserDetails(objCmnFunctions.GetLoginUserGid());
                revirew.seReviewerCode = dt.Rows[0]["employee_code"].ToString();
                revirew.seReviewerName = dt.Rows[0]["employee_name"].ToString();
                dt = ist.GetReviwerDetails(Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                revirew.seProcurementID = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                revirew.seProcurementCode = dt.Rows[0]["employee_code"].ToString();
                revirew.seProcurementName = dt.Rows[0]["employee_name"].ToString();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
                      
            return View(revirew);
        }
        [HttpPost]
        public JsonResult Evaluation(SEModel model)
        {
            try
            {
                //model.seSupID = Convert.ToInt32(ist.GetSupIDbyCode(model.seSupCode, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())));
                string check = ist.InsertReview(model);
                if (check == null)
                {
                    RedirectToAction("Index");
                }
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet); 
            }


        }

        public ActionResult PDownload()
        {
            try
            {
                string mt = null;

                List<SEModel> pList;
                if (Session["pDownload"] == null)
                {
                    pList = ist.GetSEModelPending(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                else
                {
                    pList = (List<SEModel>)Session["pDownload"];
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Supplier Name");
                dt.Columns.Add("Review Year");
                dt.Columns.Add("Supplier Status");
                for (int i = 0; i < pList.Count; i++)
                {
                    dt.Rows.Add(
                        i + 1
                        , pList[i].seSupName.ToString()
                        , pList[i].seYear.ToString()
                        , pList[i].seStatus.ToString());
                }
                //export to excel from gridview            
                GridView gv1 = new GridView();
                gv1.DataSource = dt;
                gv1.DataBind();
                Session["SupplierReview1"] = gv1;
                if (gv1.Rows.Count != 0)
                {
                    return new DownloadFileActionResult((GridView)Session["SupplierReview1"], "SupplierReviewPending.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return RedirectToAction("Index");
        }

        public ActionResult CDownload()
        {
            
            string mt = null;
            List<SEModel> cList;
            try
            {
                if (Session["cDownload"] == null)
                {
                    cList = ist.GetSEModelCompleted(mt, Convert.ToInt32(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                else
                {
                    cList = (List<SEModel>)Session["cDownload"];
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Supplier Code");
                dt.Columns.Add("Supplier Name");
                dt.Columns.Add("Overall Rating");
                dt.Columns.Add("Reviewed Year");
                dt.Columns.Add("Supplier Status");
                for (int i = 0; i < cList.Count; i++)
                {
                    dt.Rows.Add(
                        i + 1
                        , cList[i].seSupCode.ToString()
                        , cList[i].seSupName.ToString()
                        , cList[i].seOverAllRating.ToString()
                        , cList[i].seReviewYear.ToString()
                        , cList[i].seStatus.ToString());
                }
                //export to excel from gridview
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Session["SupplierReview"] = gv;
                if (gv.Rows.Count != 0)
                {
                    return new DownloadFileActionResult((GridView)Session["SupplierReview"], "SupplierReviewCompleted.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return RedirectToAction("Index");
        }


    }
}

public class DownloadFileActionResult : ActionResult
{
    public GridView ExcelGridView { get; set; }
    public string fileName { get; set; }

    public DownloadFileActionResult(GridView gv, string pFileName)
    {
        ExcelGridView = gv;
        fileName = pFileName;
    }

    public override void ExecuteResult(ControllerContext context)
    {
        HttpContext curContext = HttpContext.Current;
        curContext.Response.Clear();
        curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        curContext.Response.Charset = "";
        curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        curContext.Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        ExcelGridView.RenderControl(htw);
        byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
        MemoryStream s = new MemoryStream(byteArray);
        StreamReader sr = new StreamReader(s, Encoding.ASCII);
        curContext.Response.Write(sr.ReadToEnd());
        curContext.Response.End();
    }

}
