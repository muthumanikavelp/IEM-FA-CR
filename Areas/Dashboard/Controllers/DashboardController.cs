using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM;
using System.Data;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Common;
using System.IO;
using Rotativa.Options;
using IEM.Areas.MASTERS.Controllers;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.Dashboard.Models;
namespace IEM.Areas.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        DataTable dt = new DataTable();
        #endregion

        // GET: /Dashboard/Dashboard/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Dashboard/VendorManagement/
        public ActionResult VendorManagement()
        {
            return View();
        }

        // GET: /Dashboard/EProcure/
        public ActionResult EProcure()
        {
            return View();
        }

        // GET: /Dashboard/EClaims/
        public ActionResult EClaims()
        {
            return View();
        }

        #region Dashboard
        public JsonResult GetOverAllDashboard()  // Dashboard bind 
        {
            try
            {
              

                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                string raisermode = "S";
                Int32 RaierGid = 0;
                try
                {
                    if (objCmnFunctions.GetLoginProxyUserGid() != null && objCmnFunctions.GetLoginProxyUserGid() != 0)
                    {
                        raisermode = "P";
                        RaierGid = objCmnFunctions.GetLoginProxyUserGid();
                    }
                }
                catch
                {
                    raisermode = "S";
                    RaierGid = 0;
                }

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();



                DataSet ds = db.GetOverAllDashboard(objCmnFunctions.GetLoginUserGid().ToString(), RaierGid.ToString(), raisermode);
                if (ds != null && ds.Tables.Count > 3)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];


                        DataTable dtEProcure = new DataTable();
                        dtEProcure.Columns.Add("CATEGORY", typeof(string));
                        dtEProcure.Columns.Add("DRAFT", typeof(string));
                        dtEProcure.Columns.Add("INPROCESS", typeof(string));
                        dtEProcure.Columns.Add("APPROVED", typeof(string));
                        dtEProcure.Columns.Add("REJECTED", typeof(string));
                        dtEProcure.Columns.Add("WAITINGFORAPPROVAL", typeof(string));

                        if (dt1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if ((string.IsNullOrEmpty(dt1.Rows[i]["CATEGORY"].ToString()) ? "" : dt1.Rows[i]["CATEGORY"].ToString()) != "GRN")
                                {
                                    DataRow dr = dtEProcure.NewRow();
                                    dr["CATEGORY"] = dt1.Rows[i]["CATEGORY"].ToString();
                                    dr["DRAFT"] = dt1.Rows[i]["DRAFT"].ToString();
                                    dr["INPROCESS"] = dt1.Rows[i]["INPROCESS"].ToString();
                                    dr["APPROVED"] = dt1.Rows[i]["APPROVED"].ToString();
                                    dr["REJECTED"] = dt1.Rows[i]["REJECTED"].ToString();
                                    dr["WAITINGFORAPPROVAL"] = dt2.Rows[i]["FORMYAPPROVAL"].ToString();
                                    dtEProcure.Rows.Add(dr);
                                }
                            }
                        }

                        dt = dtEProcure;
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }


                    dt = ds.Tables[3];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["ForMyApproval"] = GetCountOfForMyApproval(objCmnFunctions.GetLoginUserGid().ToString(), dr["ctype"].ToString());
                        }
                        Data3 = JsonConvert.SerializeObject(dt);
                    }

                    dt = ds.Tables[4];
                    if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                    //For Fixed Asset 13-11-2019

                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        dt3 = ds.Tables[5];
                        dt4 = ds.Tables[6];

                        DataTable dtFixedAsset = new DataTable();
                        dtFixedAsset.Columns.Add("CATEGORY", typeof(string));
                        dtFixedAsset.Columns.Add("DRAFT", typeof(string));
                        dtFixedAsset.Columns.Add("INPROCESS", typeof(string));
                        dtFixedAsset.Columns.Add("APPROVED", typeof(string));
                        dtFixedAsset.Columns.Add("REJECTED", typeof(string));
                        dtFixedAsset.Columns.Add("FORMYAPPROVAL", typeof(string));

                        if (dt3.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                if ((string.IsNullOrEmpty(dt3.Rows[i]["CATEGORY"].ToString()) ? "" : dt3.Rows[i]["CATEGORY"].ToString()) != "GRN")
                                {
                                    DataRow dr = dtFixedAsset.NewRow();
                                    dr["CATEGORY"] = dt3.Rows[i]["CATEGORY"].ToString();
                                    dr["DRAFT"] = dt3.Rows[i]["DRAFT"].ToString();
                                    dr["INPROCESS"] = dt3.Rows[i]["INPROCESS"].ToString();
                                    dr["APPROVED"] = dt3.Rows[i]["APPROVED"].ToString();
                                    dr["REJECTED"] = dt3.Rows[i]["REJECTED"].ToString();
                                    dr["FORMYAPPROVAL"] = dt4.Rows[i]["FORMYAPPROVAL"].ToString();
                                    dtFixedAsset.Rows.Add(dr);
                                }
                            }
                        }
                        dt = dtFixedAsset;
                        if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }
                    }

                    //dt = ds.Tables[5];
                    //if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                }
                return Json(new { Data1, Data2, Data3, Data4, Data5 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetCountOfForMyApproval(string employee_gid, string doctype)
        {
            try
            {
                DataTable dt;
                string Data1 = "0";
                DataSet ds = db.GetCountOfForMyApproval(employee_gid, doctype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = ds.Tables[0].Rows[0][0].ToString(); }
                }
                return Data1;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        #endregion

        #region Summary loading
        [HttpPost]
        public JsonResult GetDashboardSummary(string action, string requesttype, string requeststatus)
        {
            try
            {
                string proxytype = "";
                if (action == "ECLAIMS")
                {
                    try
                    {
                        if (objCmnFunctions.GetLoginProxyUserGid() != null && objCmnFunctions.GetLoginProxyUserGid() != 0)
                        {
                            proxytype = "Proxy";
                        }
                        else
                        {
                            proxytype = "Self";
                        }
                    }
                    catch
                    {
                        proxytype = "Self";
                    }
                }

                string Data1 = "";
                DataSet ds = db.GetDashboardSummary(objCmnFunctions.GetLoginUserGid().ToString(), action, requesttype, requeststatus, proxytype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Individual Record Details
        [HttpPost]
        public JsonResult GetASMSSupplierHeaderDetails(string Suppliergid, string Action)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetASMSSupplierHeaderDetails(Suppliergid, Action);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetFBDetails(string Refgid, string Action)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetFBDetails(Refgid, Action);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetEClaimsDetails(string ECFId, string DocSubtype)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetEClaimsDetails(ECFId, DocSubtype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Approve/Reject
        //E-Procure Approve (or) Reject
        [HttpPost]
        public JsonResult ApproveCBF(string CBFHeaderGId, string Type, string IsReject, string Remarks)
        {
            try
            {
                string Data1 = "";
                bool IsSaved = false;
                DataSet ds = db.SubmitCBF(CBFHeaderGId, Type, IsReject, Remarks, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);
                        if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString().ToLower() == "1")
                        {
                            try
                            {

                                //Create PDF File For Email Attachment
                                EProcurePDFModel _tmpModel = new EProcurePDFModel();
                                _tmpModel.DocType = "CBF";
                                _tmpModel.CBFDoc = CreateCBFAttachment(CBFHeaderGId);

                                var root = plib.ApprovalSummaryDownloadUrl;
                                var pdfname = String.Format("{0}_{1}_{2}.pdf", "CBF", CBFHeaderGId, DateTime.Now.Ticks.ToString());
                                var path = Path.Combine(root, pdfname);
                                path = Path.GetFullPath(path);

                                var actionPDF = new Rotativa.ViewAsPdf("EProcure", _tmpModel)
                                {
                                    PageSize = Size.A4,
                                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                                    PageMargins = { Left = 1, Right = 1 }
                                };
                                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                                fileStream.Close();

                                IsSaved = SaveAttachmentDoc("2", "CBF", CBFHeaderGId, pdfname, IsReject == "0" ? "Approved" : "Rejected");
                            }
                            catch { }
                            try
                            {
                                if (IsSaved)
                                {
                                    //Send Mail
                                    ForMailController mailsender = new ForMailController();
                                    CbfSumModel objMail = new CbfSumModel();
                                    string reqstatus = objMail.GetRequestStatus(Convert.ToInt32(CBFHeaderGId), "CBF");
                                    int queuegid = objMail.GetQueueGidForMail(Convert.ToInt32(CBFHeaderGId), "CBF");
                                    mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
                                }
                            }
                            catch { }
                        }


                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult ApprovePO(string POHeaderGId, string ViewType, string IsReject, string Remarks)
        {
            try
            {
                string Data1 = "";
                bool IsSaved = false;
                DataSet ds = db.SubmitPO(POHeaderGId, "", "", "", "", "", "", "", "", ViewType, IsReject, Remarks, plib.LoginUserId, "0");
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);
                        if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString().ToLower() == "1")
                        {
                            try
                            {
                                //Create PDF File For Email Attachment
                                EProcurePDFModel _tmpModel = new EProcurePDFModel();
                                _tmpModel.DocType = "PO";
                                _tmpModel.PODoc = CreatePOAttachment(POHeaderGId, "PO");

                                var root = plib.ApprovalSummaryDownloadUrl;
                                var pdfname = String.Format("{0}_{1}_{2}.pdf", "PO", POHeaderGId, DateTime.Now.Ticks.ToString());
                                var path = Path.Combine(root, pdfname);
                                path = Path.GetFullPath(path);

                                var actionPDF = new Rotativa.ViewAsPdf("EProcure", _tmpModel)
                                {
                                    PageSize = Size.A4,
                                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                                    PageMargins = { Left = 1, Right = 1 }
                                };
                                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                                fileStream.Close();

                                IsSaved = SaveAttachmentDoc("8", "PO", POHeaderGId, pdfname, IsReject == "0" ? "Approved" : "Rejected");
                            }
                            catch { }
                            try
                            {
                                if (IsSaved)
                                {
                                    ForMailController mailsender = new ForMailController();
                                    CbfSumModel objforMail = new CbfSumModel();
                                    int refgid = objforMail.GetRefGidForMail("PO");
                                    string reqstatus = objforMail.GetRequestStatus(refgid, "PO");
                                    int queuegid = objforMail.GetQueueGidForMail(refgid, "PO");
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
                            }
                            catch { }
                        }


                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SubmitEprocure(string RefGId, string Action, string Submit, string Remarks)
        {
            try
            {
                string Data1 = "";
                string _tmpRefFlag = "";
                bool IsSaved = false;
                DataSet ds = db.SubmitEprocure(RefGId, Action, Submit, Remarks, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);
                        if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString().ToLower() == "1")
                        {
                            try
                            {
                                //Create PDF File For Email Attachment
                                EProcurePDFModel _tmpModel = new EProcurePDFModel();
                                _tmpModel.DocType = Action;
                                if (Action == "WO")
                                {
                                    _tmpRefFlag = "10";
                                    _tmpModel.PODoc = CreatePOAttachment(RefGId, "WO");
                                }
                                if (Action == "PAR")
                                {
                                    _tmpRefFlag = "9";
                                    _tmpModel.PARDoc = CreatePARAttachment(RefGId);
                                }
                                if (Action == "PR")
                                {
                                    _tmpRefFlag = "5";
                                    _tmpModel.PRDoc = CreatePRAttachment(RefGId);
                                }

                                var root = plib.ApprovalSummaryDownloadUrl;
                                var pdfname = String.Format("{0}_{1}_{2}.pdf", Action, RefGId, DateTime.Now.Ticks.ToString());
                                var path = Path.Combine(root, pdfname);
                                path = Path.GetFullPath(path);

                                var actionPDF = new Rotativa.ViewAsPdf("EProcure", _tmpModel)
                                {
                                    PageSize = Size.A4,
                                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                                    PageMargins = { Left = 1, Right = 1 }
                                };
                                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                                fileStream.Close();

                                IsSaved = SaveAttachmentDoc(_tmpRefFlag, Action, RefGId, pdfname, Submit == "1" ? "Approved" : "Rejected");
                            }
                            catch { }

                            //SEND EMAIL
                            try
                            {
                                if (Action == "PAR" && IsSaved)
                                {
                                    ForMailController mailsender = new ForMailController();
                                    CbfSumModel objMail = new CbfSumModel();
                                    int refgid = objMail.GetRefGidForMail("PAR");
                                    string reqstatus = objMail.GetRequestStatus(refgid, "PAR");
                                    int queuegid = objMail.GetQueueGidForMail(refgid, "PAR");
                                    mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), reqstatus, "0");
                                }
                                if (Action == "PR" && IsSaved)
                                {
                                    ForMailController mailsender = new ForMailController();
                                    CbfSumModel objMail = new CbfSumModel();
                                    int refgid = objMail.GetRefGidForMail("PR");
                                    string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                                    int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                                    mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
                                }
                                if (Action == "WO" && IsSaved)
                                {
                                    CbfSumModel objforMail = new CbfSumModel();
                                    ForMailController mailsender = new ForMailController();
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
                                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                                }
                            }
                            catch { }
                        }

                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //EClaim Approve (or) Reject
        [HttpPost]
        public JsonResult SubmitEClaim(string EcfId, string DocType, string Submit, string Remarks, string ConcurrentTo)
        {
            try
            {
                bool IsSaved = false;
                string Data1 = "";
                DataSet ds = db.SubmitEClaim(EcfId, DocType, Submit, Remarks, ConcurrentTo, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);
                        if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString().ToLower() == "1")
                        {
                            try
                            {
                                //Create PDF File For Email Attachment
                                EClaimsPDFModel _tmpModel = new EClaimsPDFModel();
                                _tmpModel = CreateEClaimsAttachment(EcfId, DocType);

                                var root = plib.ApprovalSummaryDownloadUrl;
                                var pdfname = String.Format("EClaims_{0}_{1}_{2}.pdf", DocType, EcfId, DateTime.Now.Ticks.ToString());
                                var path = Path.Combine(root, pdfname);
                                path = Path.GetFullPath(path);

                                var actionPDF = new Rotativa.ViewAsPdf("EClaims", _tmpModel)
                                {
                                    PageSize = Size.A4,
                                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                                    PageMargins = { Left = 1, Right = 1 }
                                };
                                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                                fileStream.Close();

                                IsSaved = SaveAttachmentDoc("1", DocType, EcfId, pdfname, Submit == "1" ? "Approved" : "Rejected");
                            }
                            catch { }
                            try
                            {
                                if (IsSaved && Submit != "3" && Submit != "4")
                                {
                                    //Send Mail

                                    string _QueueGId = GetMaxQueueGid(EcfId);
                                    string _DocType = GetDocTypeGIDEClaim(_QueueGId);
                                    _DocType = objCmnFunctions.GetSubDocType(_DocType);

                                    ForMailController mailsender = new ForMailController();
                                    string s = mailsender.sendusermail("EOW", _DocType, _QueueGId, Submit == "1" ? "A" : "R", "0", EcfId);
                                    //mailsender.sendusermail("EOW", DocType, EcfId, Submit == "1" ? "A" : "R", "0");
                                }
                            }
                            catch (Exception ex) { throw ex; }
                        }

                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }

        public string GetMaxQueueGid(string _ECFId)
        {
            try
            {
                string Data1 = "0";
                DataSet ds = db.GetMaxQueueGidEClaim(_ECFId, objCmnFunctions.GetLoginUserGid().ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = dt.Rows[0]["queue_gid"].ToString(); }
                }
                return Data1;
            }
            catch
            {
                return "0";
            }
        }
        public string GetDocTypeGIDEClaim(string queue_gid)
        {
            try
            {
                string Data1 = "0";
                DataSet ds = db.GetDocTypeGIDEClaim(queue_gid);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = dt.Rows[0]["docsubtype_gid"].ToString(); }
                }
                return Data1;
            }
            catch
            {
                return "0";
            }
        }


        //ASMS Approve (or) Reject
        [HttpPost]
        public JsonResult SubmitASMS(string refgid, string requesttype, string actionremark, string queueto, string actionstatus, string skipflag, string action)
        {
            bool IsSaved = false;
            try
            {
                DataTable dttemp = new DataTable();
                dttemp.Columns.Add("Message", typeof(string));
                dttemp.Columns.Add("Clear", typeof(Boolean));

                string Data1 = "";
                DataSet ds = db.SubmitASMS(refgid, plib.LoginUserId, requesttype, actionremark, queueto, actionstatus, skipflag, action);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dttemp.Rows.Add(new object[] { dt.Rows[0]["Message"].ToString(), false });
                    }
                }
                else
                {
                    dttemp.Rows.Add(new object[] { "Successfully Submitted", true });
                }

                if (dttemp != null && dttemp.Rows.Count > 0)
                {
                    Data1 = JsonConvert.SerializeObject(dttemp);
                    if (dttemp.Rows[0]["Clear"].ToString().ToLower() == "true" || dttemp.Rows[0]["Clear"].ToString().ToLower() == "1")
                    {
                        try
                        {
                            //Create PDF File For Email Attachment
                            ASMSPDFModel _tmpModel = new ASMSPDFModel();
                            _tmpModel = CreateASMSAttachment(refgid, requesttype);

                            var root = plib.ApprovalSummaryDownloadUrl;
                            var pdfname = String.Format("{0}_{1}_{2}.pdf", requesttype, refgid, DateTime.Now.Ticks.ToString());
                            var path = Path.Combine(root, pdfname);
                            path = Path.GetFullPath(path);

                            var actionPDF = new Rotativa.ViewAsPdf("VendorManagement", _tmpModel)
                            {
                                PageSize = Size.A4,
                                PageOrientation = Rotativa.Options.Orientation.Landscape,
                                PageMargins = { Left = 1, Right = 1 }
                            };
                            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                            fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                            fileStream.Close();

                            IsSaved = SaveAttachmentDoc("6", requesttype.ToUpper().Trim(), refgid, pdfname, actionstatus == "1" ? "Approved" : "Rejected");
                        }
                        catch { }
                        try
                        {
                            if (IsSaved)
                            {
                                string curapprovalstage = "0";
                                ForMailController mailsender = new ForMailController();
                                DataSet dstmp = db.GetASMSMailDetails(refgid);
                                if (dstmp != null && dstmp.Tables.Count > 0)
                                {
                                    if (dstmp.Tables[0].Rows.Count > 0)
                                    {
                                        curapprovalstage = string.IsNullOrEmpty(dstmp.Tables[0].Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dstmp.Tables[0].Rows[0]["supplierheader_currentapprovalstage"].ToString();
                                    }
                                }

                                mailsender.sendusermail("ASMS", requesttype.ToUpper().Trim(), refgid, actionstatus == "1" ? "A" : "R", curapprovalstage);
                            }
                        }
                        catch { }
                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region EProcure Attachment Template
        //(CBF)
        public CBFPDFModel CreateCBFAttachment(string CBFHeaderGId)
        {
            CBFPDFModel cbf = new CBFPDFModel();
            DataSet dsCBF = db.GetFBDetails(CBFHeaderGId, "CBF");
            if (dsCBF != null && dsCBF.Tables.Count > 0)
            {
                if (dsCBF.Tables[0].Rows.Count > 0)
                {
                    cbf.CBFNo = dsCBF.Tables[0].Rows[0]["RefNo"].ToString();
                    cbf.CBFDate = dsCBF.Tables[0].Rows[0]["RefDate"].ToString();
                    cbf.Raiser = dsCBF.Tables[0].Rows[0]["Raiser"].ToString();
                    cbf.RequestFor = dsCBF.Tables[0].Rows[0]["RequestFor"].ToString();
                    cbf.CBFAmount = dsCBF.Tables[0].Rows[0]["Amount"].ToString();
                    cbf.CBFMode = dsCBF.Tables[0].Rows[0]["Mode"].ToString();
                    cbf.PARPRNo = dsCBF.Tables[0].Rows[0]["PARPRRefNo"].ToString();
                    cbf.PARPRAmt = dsCBF.Tables[0].Rows[0]["PARPRAmount"].ToString();
                    cbf.CBFApproval = dsCBF.Tables[0].Rows[0]["Approvaltype"].ToString();
                    cbf.Budgeted = dsCBF.Tables[0].Rows[0]["IsBudgeted"].ToString();
                    cbf.Branch = dsCBF.Tables[0].Rows[0]["BranchName"].ToString();
                    cbf.CBFJustification = dsCBF.Tables[0].Rows[0]["Justification"].ToString();
                    cbf.PreviousApprover = dsCBF.Tables[0].Rows[0]["PreviousApprover"].ToString();
                    cbf.PreviousApproverComment = dsCBF.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();

                    List<CBFInnerDetails> SMarray = new List<CBFInnerDetails>();
                    if (dsCBF.Tables.Count > 1)
                    {
                        if (dsCBF.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsCBF.Tables[1].Rows.Count; j++)
                            {
                                CBFInnerDetails pd = new CBFInnerDetails();
                                pd.Type = dsCBF.Tables[1].Rows[j]["ProdServiceFlag"].ToString();
                                pd.ProductServiceGroup = dsCBF.Tables[1].Rows[j]["ProductGroup"].ToString();
                                pd.ProductServiceName = dsCBF.Tables[1].Rows[j]["ProductCode"].ToString() + dsCBF.Tables[1].Rows[j]["ProductName"].ToString();
                                pd.ProductServiceDesc = dsCBF.Tables[1].Rows[j]["ProductDescription"].ToString();
                                pd.UOM = dsCBF.Tables[1].Rows[j]["UOM"].ToString();
                                pd.Amount = dsCBF.Tables[1].Rows[j]["TotalAmount"].ToString();
                                pd.Qty = dsCBF.Tables[1].Rows[j]["Qty"].ToString();
                                pd.GLCode = dsCBF.Tables[1].Rows[j]["GLCode"].ToString();
                                pd.CC = dsCBF.Tables[1].Rows[j]["FccName"].ToString();
                                pd.BudgetedLine = dsCBF.Tables[1].Rows[j]["BudgetLine"].ToString();
                                SMarray.Add(pd);
                            }
                        }
                    }
                    cbf.ProductList = SMarray;
                }
            }
            return cbf;
        }

        //PO(or)WO
        public POPDFModel CreatePOAttachment(string POHeaderGId, string Type)
        {
            POPDFModel po = new POPDFModel();
            DataSet ds = db.GetFBDetails(POHeaderGId, Type);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    po.PONo = ds.Tables[0].Rows[0]["RefNo"].ToString();
                    po.PODate = ds.Tables[0].Rows[0]["RefDate"].ToString();
                    po.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                    po.Supplier = ds.Tables[0].Rows[0]["Supplier"].ToString();
                    po.CBFNo = ds.Tables[0].Rows[0]["CBFNo"].ToString();
                    po.CBFLineAmount = ds.Tables[0].Rows[0]["CBFLineAmount"].ToString();
                    po.Mode = ds.Tables[0].Rows[0]["Mode"].ToString();
                    po.PARPRNo = ds.Tables[0].Rows[0]["PARPRRefNo"].ToString();
                    po.BranchType = ds.Tables[0].Rows[0]["BranchSingle"].ToString();
                    po.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                    po.CC = ds.Tables[0].Rows[0]["CC"].ToString();
                    po.POAmount = ds.Tables[0].Rows[0]["Amount"].ToString();
                    po.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                    po.PreviousApproverComment = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();

                    List<POInnerDetails> SMarray = new List<POInnerDetails>();
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                POInnerDetails pd = new POInnerDetails();
                                pd.Product = ds.Tables[1].Rows[j]["ProductCode"].ToString() + ds.Tables[1].Rows[j]["ProductName"].ToString();
                                pd.Description = ds.Tables[1].Rows[j]["ProductDescription"].ToString();
                                pd.UOM = ds.Tables[1].Rows[j]["UOM"].ToString();
                                pd.UnitPrice = ds.Tables[1].Rows[j]["UnitPrice"].ToString();
                                pd.Qty = ds.Tables[1].Rows[j]["Qty"].ToString();
                                pd.Discount = ds.Tables[1].Rows[j]["Discount"].ToString();
                                pd.Tax = ds.Tables[1].Rows[j]["Tax"].ToString();
                                pd.Others = ds.Tables[1].Rows[j]["Others"].ToString();
                                pd.TotalAmount = ds.Tables[1].Rows[j]["TotalAmount"].ToString();
                                SMarray.Add(pd);
                            }
                        }
                    }
                    po.ProductList = SMarray;
                }
            }
            return po;
        }

        //PAR
        public PARPDFModel CreatePARAttachment(string RefGId)
        {
            PARPDFModel par = new PARPDFModel();
            DataSet ds = db.GetFBDetails(RefGId, "PAR");
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    par.PARNo = ds.Tables[0].Rows[0]["RefNo"].ToString();
                    par.PARDate = ds.Tables[0].Rows[0]["RefDate"].ToString();
                    par.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                    par.Budgeted = ds.Tables[0].Rows[0]["IsBudgeted"].ToString();
                    par.PARAmount = ds.Tables[0].Rows[0]["Amount"].ToString();
                    par.RaiserComment = ds.Tables[0].Rows[0]["RaiserComments"].ToString();

                    List<PARInnerDetails0> SMarray0 = new List<PARInnerDetails0>();
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                PARInnerDetails0 pd = new PARInnerDetails0();
                                pd.Type = ds.Tables[1].Rows[j]["ExpenseType"].ToString();
                                pd.FYFrom = ds.Tables[1].Rows[j]["FYFrom"].ToString();
                                pd.FYTo = ds.Tables[1].Rows[j]["FYTo"].ToString();
                                pd.Amount = ds.Tables[1].Rows[j]["Amount"].ToString();
                                SMarray0.Add(pd);
                            }
                        }
                    }
                    par.ProductList0 = SMarray0;

                    List<PARInnerDetails1> SMarray1 = new List<PARInnerDetails1>();
                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                PARInnerDetails1 pd = new PARInnerDetails1();
                                pd.Name = ds.Tables[2].Rows[j]["ApproverCode"].ToString();
                                pd.Code = ds.Tables[2].Rows[j]["ApproverName"].ToString();
                                pd.Date = ds.Tables[2].Rows[j]["ApprovalDate"].ToString();
                                SMarray1.Add(pd);
                            }
                        }
                    }
                    par.ProductList1 = SMarray1;
                }
            }
            return par;
        }

        //PR
        public PRPDFModel CreatePRAttachment(string RefGId)
        {
            PRPDFModel pr = new PRPDFModel();
            DataSet ds = db.GetFBDetails(RefGId, "PR");
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pr.RefNo = ds.Tables[0].Rows[0]["RefNo"].ToString();
                    pr.RefDate = ds.Tables[0].Rows[0]["RefDate"].ToString();
                    pr.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                    pr.IsBudgeted = ds.Tables[0].Rows[0]["IsBudgeted"].ToString();
                    pr.Amount = ds.Tables[0].Rows[0]["Amount"].ToString();
                    pr.RequestFor = ds.Tables[0].Rows[0]["RequestFor"].ToString();
                    pr.ExpenseType = ds.Tables[0].Rows[0]["ExpenseType"].ToString();
                    pr.BSCC = ds.Tables[0].Rows[0]["BSCC"].ToString();
                    pr.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                    pr.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                    pr.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                    pr.PreviousApproverComment = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                    List<PRInnerDetails> SMarray = new List<PRInnerDetails>();
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                PRInnerDetails pd = new PRInnerDetails();
                                pd.Product = ds.Tables[1].Rows[j]["Product"].ToString();
                                pd.Description = ds.Tables[1].Rows[j]["ProductDescription"].ToString();
                                pd.UOM = ds.Tables[1].Rows[j]["UOM"].ToString();
                                pd.Qty = ds.Tables[1].Rows[j]["Qty"].ToString();
                                pd.Amount = ds.Tables[1].Rows[j]["TotalAmount"].ToString();
                                SMarray.Add(pd);
                            }
                        }
                    }
                    pr.ProductList = SMarray;
                }
            }
            return pr;
        }
        #endregion

        #region Vendor Management Template
        public ASMSPDFModel CreateASMSAttachment(string RefGId, string Type)
        {
            ASMSPDFModel asms = new ASMSPDFModel();
            DataSet ds = db.GetASMSSupplierHeaderDetails(RefGId, Type);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    asms.ASMSType = Type;

                    asms.Supplier = ds.Tables[0].Rows[0]["Supplier"].ToString();
                    asms.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                    asms.ContractFrom = ds.Tables[0].Rows[0]["ContractFrom"].ToString();
                    asms.ContractTo = ds.Tables[0].Rows[0]["ContractTo"].ToString();

                    if (Type.ToLower() == "activation")
                    {
                        asms.ActivationFrom = ds.Tables[0].Rows[0]["ActivationFrom"].ToString();
                        asms.ActivationTo = ds.Tables[0].Rows[0]["ActivationTo"].ToString();
                        asms.ActivateReason = ds.Tables[0].Rows[0]["ActivateReason"].ToString();
                    }
                    if (Type.ToLower() == "deactivation")
                    {
                        asms.DeActivateReason = ds.Tables[0].Rows[0]["DeActivateReason"].ToString();
                    }

                    if (Type.ToLower() == "onboarding" || Type.ToLower() == "renewal")
                    {
                        asms.ApproxContractValue = ds.Tables[0].Rows[0]["ApproxContractValue"].ToString();
                        asms.GSTINProvided = ds.Tables[0].Rows[0]["GSTINProvided"].ToString();
                    }
                    if (Type.ToLower() == "renewal")
                    {
                        asms.PreviousContractFrom = ds.Tables[0].Rows[0]["PreviousContractFrom"].ToString();
                        asms.PreviousContractTo = ds.Tables[0].Rows[0]["PreviousContractTo"].ToString();
                    }

                    asms.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                    asms.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                    asms.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                }
            }
            return asms;
        }
        #endregion

        #region Eclaims Template
        public EClaimsPDFModel CreateEClaimsAttachment(string ECFId, string Type)
        {
            EClaimsPDFModel claim = new EClaimsPDFModel();
            DataSet ds = db.GetEClaimsDetails(ECFId, Type);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables.Count > 0)
                {
                    claim.DocType = Type;
                    if (Type == "1" || Type == "3")
                    {
                        ECFEmployeeClaim cl = new ECFEmployeeClaim();
                        cl.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        cl.ECFNo = ds.Tables[0].Rows[0]["ECFNo"].ToString();
                        cl.ECFAmount = ds.Tables[0].Rows[0]["ECFAmount"].ToString();
                        cl.PeriodFrom = ds.Tables[0].Rows[0]["PeriodFrom"].ToString();
                        cl.PeriodTo = ds.Tables[0].Rows[0]["PeriodTo"].ToString();
                        cl.ServiceMonth = ds.Tables[0].Rows[0]["ServiceMonth"].ToString();
                        cl.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                        cl.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        cl.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        cl.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();

                        List<EmployeeClaimInnerDetails> SMarray = new List<EmployeeClaimInnerDetails>();
                        if (ds.Tables.Count > 1)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {
                                    EmployeeClaimInnerDetails inner = new EmployeeClaimInnerDetails();
                                    inner.Category = ds.Tables[1].Rows[j]["Category"].ToString();
                                    inner.SubCategory = ds.Tables[1].Rows[j]["SubCategory"].ToString();
                                    inner.Amount = ds.Tables[1].Rows[j]["Amount"].ToString();
                                    SMarray.Add(inner);
                                }
                            }
                        }
                        cl.ExpenseList = SMarray;
                        claim.EmployeeClaimDoc = cl;
                    }
                    if (Type == "2")
                    {
                        LocalConveyanceType locc = new LocalConveyanceType();
                        locc.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        locc.ECFNo = ds.Tables[0].Rows[0]["ECFNo"].ToString();
                        locc.ECFAmount = ds.Tables[0].Rows[0]["ECFAmount"].ToString();
                        locc.NoOfPersons = ds.Tables[0].Rows[0]["NoOfPersons"].ToString();
                        locc.ServiceMonth = ds.Tables[0].Rows[0]["ServiceMonth"].ToString();
                        locc.HighestClaimAmount = ds.Tables[0].Rows[0]["HighestClaimAmount"].ToString();
                        locc.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                        locc.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        locc.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        locc.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                        claim.locConveyanceDoc = locc;
                    }
                    if (Type == "4")
                    {
                        ECFSIClaim ecfClaim = new ECFSIClaim();
                        ecfClaim.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        ecfClaim.Supplier = ds.Tables[0].Rows[0]["Supplier"].ToString();
                        ecfClaim.ECFNo = ds.Tables[0].Rows[0]["ECFNo"].ToString();
                        ecfClaim.ECFAmount = ds.Tables[0].Rows[0]["ECFAmount"].ToString();
                        ecfClaim.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        ecfClaim.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        ecfClaim.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                        ecfClaim.POType = ds.Tables[0].Rows[0]["POType"].ToString();

                        List<ECFSIClaimInnerDetails> SMarray = new List<ECFSIClaimInnerDetails>();
                        if (ds.Tables.Count > 1)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {
                                    ECFSIClaimInnerDetails inner = new ECFSIClaimInnerDetails();
                                    inner.InvoiceNo = ds.Tables[1].Rows[j]["InvoiceNo"].ToString();
                                    inner.InvoiceDate = ds.Tables[1].Rows[j]["InvoiceDate"].ToString();
                                    inner.InvoiceAmount = ds.Tables[1].Rows[j]["InvoiceAmount"].ToString();
                                    inner.POWONo = ds.Tables[0].Rows[j]["POWONo"].ToString();
                                    inner.GSTApplicable = ds.Tables[1].Rows[j]["GSTApplicable"].ToString();
                                    inner.TaxableAmount = ds.Tables[1].Rows[j]["TaxableAmount"].ToString();
                                    inner.Taxtype = ds.Tables[1].Rows[j]["Taxtype"].ToString();
                                    inner.TaxAmount = ds.Tables[1].Rows[j]["TaxAmount"].ToString();
                                    SMarray.Add(inner);
                                }
                            }
                        }
                        ecfClaim.InvoiceList = SMarray;
                        claim.SIClaimDoc = ecfClaim;
                    }
                    if (Type == "5")
                    {
                        DSASupplierType dsa = new DSASupplierType();
                        dsa.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        dsa.Supplier = ds.Tables[0].Rows[0]["Supplier"].ToString();
                        dsa.ECFNo = ds.Tables[0].Rows[0]["ECFNo"].ToString();
                        dsa.ECFAmount = ds.Tables[0].Rows[0]["ECFAmount"].ToString();
                        dsa.NoOfPersons = ds.Tables[0].Rows[0]["NoOfPersons"].ToString();
                        dsa.ServiceMonth = ds.Tables[0].Rows[0]["ServiceMonth"].ToString();
                        dsa.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        dsa.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        dsa.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                        claim.DSAClaimDoc = dsa;
                    }
                    if (Type == "6" || Type == "8")
                    {
                        ARFEmployeeType arfemp = new ARFEmployeeType();
                        arfemp.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        arfemp.ARFNo = ds.Tables[0].Rows[0]["ARFNo"].ToString();
                        arfemp.ARFAmount = ds.Tables[0].Rows[0]["ARFAmount"].ToString();
                        arfemp.ARFType = ds.Tables[0].Rows[0]["ARFType"].ToString();
                        arfemp.Advancetype = ds.Tables[0].Rows[0]["Advancetype"].ToString();
                        arfemp.TargetLiqDate = ds.Tables[0].Rows[0]["TargetLiqDate"].ToString();
                        arfemp.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                        arfemp.ARFDescription = ds.Tables[0].Rows[0]["ARFDescription"].ToString();

                        arfemp.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        arfemp.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        arfemp.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                        claim.ARFEmpClaimDoc = arfemp;
                    }
                    if (Type == "7")
                    {
                        ARFSupplierType arfsup = new ARFSupplierType();
                        arfsup.Raiser = ds.Tables[0].Rows[0]["Raiser"].ToString();
                        arfsup.SupplierEmployeeName = ds.Tables[0].Rows[0]["SupplierEmployeeName"].ToString();
                        arfsup.ARFNo = ds.Tables[0].Rows[0]["ARFNo"].ToString();
                        arfsup.ARFAmount = ds.Tables[0].Rows[0]["ARFAmount"].ToString();
                        arfsup.ARFType = ds.Tables[0].Rows[0]["ARFType"].ToString();
                        arfsup.Advancetype = ds.Tables[0].Rows[0]["Advancetype"].ToString();
                        arfsup.TargetLiqDate = ds.Tables[0].Rows[0]["TargetLiqDate"].ToString();
                        arfsup.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                        arfsup.ARFDescription = ds.Tables[0].Rows[0]["ARFDescription"].ToString();
                        arfsup.PromoInvoice = ds.Tables[0].Rows[0]["PromoInvoice"].ToString();
                        arfsup.PONo = ds.Tables[0].Rows[0]["PONo"].ToString();
                        arfsup.RaiserComments = ds.Tables[0].Rows[0]["RaiserComments"].ToString();
                        arfsup.PreviousApprover = ds.Tables[0].Rows[0]["PreviousApprover"].ToString();
                        arfsup.PreviousApproverRemarks = ds.Tables[0].Rows[0]["PreviousApproverRemarks"].ToString();
                        claim.ARFSupClaimDoc = arfsup;
                    }
                }
            }
            return claim;
        }
        #endregion

        #region Save Attachment FileName
        public bool SaveAttachmentDoc(string RefFlag, string RefType, string RefGid, string AttachmentName, string AttachmentDesc)
        {
            DataSet ds = db.SetAttachmentNew(RefFlag, RefType, RefGid, AttachmentName, AttachmentDesc, plib.LoginUserId);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return (ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() == "1") ? true : false;
                }
            }
            return false;
        }
        #endregion

        #region My Request
        [HttpPost]
        public JsonResult GetMyRequestTabDetails()  // For Work bench grid bind
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", proxytype = "", Data4 = "";
                try
                {
                    if (objCmnFunctions.GetLoginProxyUserGid() != null && objCmnFunctions.GetLoginProxyUserGid() != 0)
                    {
                        proxytype = "Proxy";
                    }
                    else
                    {
                        proxytype = "Self";
                    }
                }
                catch
                {
                    proxytype = "Self";
                }
                DataSet ds = db.GetMyRequestTabDetails(objCmnFunctions.GetLoginUserGid().ToString(), objCmnFunctions.GetLoginProxyUserGid().ToString(), proxytype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }

                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region My Approval
        [HttpPost]
        public JsonResult GetMyApprovalTabDetails()
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", proxytype = "";
                try
                {
                    if (objCmnFunctions.GetLoginProxyUserGid() != null && objCmnFunctions.GetLoginProxyUserGid() != 0)
                    {
                        proxytype = "Proxy";
                    }
                    else
                    {
                        proxytype = "Self";
                    }
                }
                catch
                {
                    proxytype = "Self";
                }

                DataSet ds = db.GetMyApprovalTabDetails(objCmnFunctions.GetLoginUserGid().ToString(), proxytype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }

                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Hold Tab
        [HttpPost]
        public JsonResult GetHoldTabDetails()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetHoldTabDetails(objCmnFunctions.GetLoginUserGid().ToString(), "HOLDREPORTSEARCH");
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UpdateHoldData(string ecfgid, string remarks)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.UpdateReleaseHold(ecfgid, remarks, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Testing Code
        [HttpPost]
        public JsonResult TestSavePDF()
        {
            try
            {
                string Data1 = "";
                var root = plib.ApprovalSummaryDownloadUrl;
                var pdfname = String.Format("{0}_{1}_{2}.pdf", "Test", "123", DateTime.Now.Ticks.ToString());
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);
                var actionPDF = new Rotativa.ViewAsPdf("VendorManagement")
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageMargins = { Left = 1, Right = 1 }
                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                fileStream.Close();
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Data1 = ex.ToString();
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
