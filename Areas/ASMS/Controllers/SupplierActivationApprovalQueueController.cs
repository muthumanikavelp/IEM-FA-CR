using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using System.Text;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

    [NoDirectAccess]
    public class SupplierActivationApprovalQueueController : Controller
    {
        IRepositoryRenewal irAq;
        ErrorLog objErrorLog = new ErrorLog();
        public SupplierActivationApprovalQueueController()
           :this  (new SupRenwalModel())
    {

    }

        public SupplierActivationApprovalQueueController(IRepositoryRenewal qirAq)
        {
            irAq = qirAq;
        }

        public ActionResult ActivationApprovalQueue(string queuefor = null, string requesttype = null, string reqstatus = null)
        {
            DataTable dtsc = new DataTable();
            SupplierActivation objActi = new SupplierActivation();
            List<SupplierActivation> laobj = new List<SupplierActivation>();
            List<SupplierActivation> losc = new List<SupplierActivation>();
                try
                {
                    //if (requesttype != null && reqstatus != null)
                    //{
                        laobj = irAq.GetSupHeaderDetailsDashboard(requesttype, reqstatus);
                  //  }
                  
                    //  laobj = irAq.GetSupHeaderDetailsDashboard("activation", "draft");
                    // laobj = irAq.GetSupHeaderDetailsDashboard("activation", "WAITINGFORAPPROVAL");
                    Session["objlist"] = laobj;
                    losc = irAq.GetSuppliercatogory(string.Empty);
                    //if(dtsc.Rows.Count>0)
                    //{
                    //    for (int j = 0; j < dtsc.Rows.Count;j++ )
                    //    {
                    //        objActi = new SupplierActivation();
                    //        objActi.SupplierClassificationID = Convert.ToInt32(dtsc.Rows[j]["SupplierCategorygid"].ToString());
                    //        objActi.SupplierClassificationName = dtsc.Rows[j]["SupplierCategoryName"].ToString();
                    //        losc.Add(objActi);
                    //    }
                    Session["reqstatus"] = reqstatus;
                    Session["category"] = losc;
                    objActi.SupplierClassification = new SelectList(losc, "SupplierClassificationID", "SupplierClassificationName");
                    objActi.SupplierActiveList = laobj.ToList();
                    ViewBag.status = "yes";
                    // }
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                }
            


            return View(objActi);
        }

        [HttpPost]
        public ActionResult ActivationApprovalQueue(string txtSuppliercode, string txtSupplierName, string Common, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            SupplierActivation objActi = new SupplierActivation();
            DataSet getdt = new DataSet();
            List<SupplierActivation> objvar = new List<SupplierActivation>();
            List<SupplierActivation> obj = new List<SupplierActivation>();
            List<SupplierActivation> aobj = new List<SupplierActivation>();
            try
            {
                objActi.SupplierActiveList = (List<SupplierActivation>)Session["objlist"];
                obj = irAq.GetSuppliercatogory(ddlSup_Clasification);
                aobj = irAq.GetEmployeelist();
                if (ddlSup_Clasification != "0")
                {
                    objActi.SupplierClassificationName = obj[0].SupplierClassificationName;
                }

                if (Common == "Search")
                {

                    if ((string.IsNullOrEmpty(txtSuppliercode)) == false)
                    {
                        ViewBag.txtSuppliercode = txtSuppliercode;
                        objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => txtSuppliercode == null ||
                           (x.SupplierCode.ToUpper().Contains(txtSuppliercode.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierActiveList;
                    }
                    if ((string.IsNullOrEmpty(txtSupplierName)) == false)
                    {
                        ViewBag.txtSupplierName = txtSupplierName;
                        objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => txtSupplierName == null ||
                           (x.SupplierName.ToUpper().Contains(txtSupplierName.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierActiveList;
                    }
                    if (ddlSup_Clasification != "0")
                    {
                        ViewBag.ddlSup_Clasification = ddlSup_Clasification;
                        objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => ddlSup_Clasification == null ||
                          (x.SupplierClassificationName.ToUpper().Contains(objActi.SupplierClassificationName.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierActiveList;
                    }
                    if (ddlRequestType != "0")
                    {
                        ViewBag.ddlRequestType = ddlRequestType;
                        objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => ddlRequestType == null ||
                           (x.RequestTypeName.ToUpper().Contains(ddlRequestType.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierActiveList;
                    }
                    if (ddlRequestStatus != "0")
                    {
                        ViewBag.ddlRequestStatus = ddlRequestStatus;
                        objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => ddlRequestStatus == null ||
                           (x.RequestStatusName.ToUpper().Contains(ddlRequestStatus.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierActiveList;
                    }
                }
                else
                {
                    objActi.SupplierActiveList = (List<SupplierActivation>)Session["objlist"];
                }

                objActi.SupplierClassification = new SelectList((List<SupplierActivation>)Session["category"], "SupplierClassificationID", "SupplierClassificationName");
                objActi.ApproverList = new SelectList(aobj, "employee_gid", "employee_name");
                ViewBag.status = "yes";
                
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
               
            
            return View(objActi);
        }

        public PartialViewResult SupplierApproveActivation(string id)
        {
            List<SupplierActivation> obj = new List<SupplierActivation>();
            List<SupplierActivation> objA = new List<SupplierActivation>();
            List<SupplierActivation> objF = new List<SupplierActivation>();
            SupplierActivation objAct = new SupplierActivation();
            DataSet getds = new DataSet();
           // int currentStage=0;
            string Alogedstatus = string.Empty;
            CmnFunctions objCmnFunctions = new CmnFunctions();
            CommonIUD objCommonIUD = new CommonIUD();
            string statsus = "";
            if (Session["reqstatus"] != null)
            {
                statsus = Session["reqstatus"].ToString();
            }
           
         
            //if (statsus != "rejected")
            //{
          
                try
                {
                    string user = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                  //  Alogedstatus = objCommonIUD.SupplierLocked(id, user); 
                    getds = irAq.GetActive_emptemp(id, "A");
                    if (getds.Tables[0].Rows.Count > 0)
                    {

                        //for (int j = 0; j < getds.Tables[1].Rows.Count; j++)
                        //{
                        objAct = new SupplierActivation();
                        // objAct.Approverid = Convert.ToInt32(getds.Tables[1].Rows[j]["employee_gid"].ToString());
                        // objAct.ApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                        objAct.Supplierheadergid = getds.Tables[0].Rows[0]["Suppliergid"].ToString();
                        objAct.SupplierCode = getds.Tables[0].Rows[0]["SupplierCode"].ToString();
                        objAct.SupplierName = getds.Tables[0].Rows[0]["SupplierName"].ToString();
                        objAct.Fdate = getds.Tables[0].Rows[0]["ActivationFrom"].ToString();
                        objAct.Todate = getds.Tables[0].Rows[0]["ActivationTo"].ToString();
                        objAct.ActiveReason = getds.Tables[0].Rows[0]["ActivateReson"].ToString();
                        objAct.comments = getds.Tables[0].Rows[0]["ActiveComments"].ToString();
                        obj.Add(objAct);
                        // }
                        Session["SupplierHeaderGid"] = objAct.Supplierheadergid;
                        SupDataModel dm = new SupDataModel();
                        objAct._IsExistsFlagApprover = dm.IsExistsFlagApprover();
                        DataTable dtIsExistsFlagApprover = new DataTable();
                        dtIsExistsFlagApprover = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                        objAct._IsExistsApproverName = (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString().ToUpper()) + "-" + (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString().ToUpper());
                        if (getds.Tables[2].Rows.Count > 0)
                        {
                            for (int k = 0; k < getds.Tables[2].Rows.Count; k++)
                            {
                                //  Session["supplierattmtfile"] = getds.Tables[2];
                                SupplierActivation objActN = new SupplierActivation();
                                objActN.Attachid = Convert.ToInt32(getds.Tables[2].Rows[k]["saattachment_gid"].ToString());
                                objActN.AttachFileName = getds.Tables[2].Rows[k]["saattachment_filename"].ToString();
                                objActN.Attachdate = getds.Tables[2].Rows[k]["saattachment_insert_date"].ToString();
                                objA.Add(objActN);
                                Session["supplierattmtlist"] = objA;
                            }
                        }
                        else
                        {
                            Session["supplierattmtlist"] = objA;
                        }
                       
                        // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
                        objAct.AttchList = objA.ToList();
                        // objAct.ApproverList = new SelectList(obj, "Approverid", "ApproverName");
                        objF = irAq.GetEmployeeListForApproval(id);
                        objAct.ApproverList = new SelectList(objF, "Approverid", "ApproverName");
                        Session["listobj"] = obj;
                        objAct.CurrenStrage = Convert.ToInt32(getds.Tables[0].Rows[0]["supplierheader_currentapprovalstage"].ToString());
                        objAct.RequestTypeName = getds.Tables[0].Rows[0]["supplierheader_requesttype"].ToString();
                        Session["stage"] = string.Empty;
                        if (objAct.CurrenStrage == 1)
                        {
                            //ViewBag.Message = "Supervisory Approval";
                            objAct.QueueStatus = "Supervisory Approval";
                        }
                        else if (objAct.CurrenStrage == 2)
                        {
                           // ViewBag.Message = "VMU Checker";
                            objAct.QueueStatus = "Supervisory Approval";
                        }
                        else if (objAct.CurrenStrage == 3)
                        {
                            //ViewBag.Message = "Funcational Head Approval";
                            objAct.QueueStatus = "Supervisory Approval";
                        }
                        else if (objAct.CurrenStrage == 4)
                        {
                            //ViewBag.Message = "VMU Head Approval";
                            objAct.QueueStatus = "Supervisory Approval";
                            Session["stage"] = "VMU Head Approval";
                        }

                    }
                    if (Alogedstatus == "Success")
                    {
                        objAct.Alogstatus = "N";
                    }
                    else
                    {
                        objAct.Alogstatus = Alogedstatus;
                    }
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                }
                  return PartialView(objAct);
      //      }
      //else
      //     {
      //          //return RedirectToAction("../SupplierActivationQueue/SupplierActivation");
      //    }
          
         
        }
      
        

        [HttpPost]
        public ActionResult SupplierApproveActivation(SupplierActivation hobj)
        {
            List<SupervisoryApproval> objApr = new List<SupervisoryApproval>();
            int Result = 0;
            int status = 0;
            string queto = string.Empty;
            string result = string.Empty;
            string Areleselog = string.Empty;
           // DataTable dtgetfp = new DataTable();
            List<SupplierActivation> dtgetfp = new List<SupplierActivation>();
            try
            {
                Session["SupplierHeaderGid"] = hobj.Supplierheadergid;
                SupDataModel supmodel = new SupDataModel();
                DataTable dtForMail = supmodel.GetMailDetails();
                var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                var curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                status = Convert.ToInt32(string.IsNullOrEmpty(hobj.Approval) ? hobj.Reject : hobj.Approval);
                Session["SupplierHeaderGid"] = hobj.Supplierheadergid;
                //skipFlag =Convert.ToInt16(hobj.skipFlag);
                if (Session["stage"].ToString() == string.Empty)
                {
                    // objApr = irAq.GetNextApprovalStage(hobj.RequestTypeName, hobj.CurrenStrage);
                    //   Result = irAq.SubmitToNextQueue(string.Empty, hobj.RequestTypeName, hobj.ActiveReason);
                    if (hobj.Approverid != 0)
                    {
                        queto = hobj.Approverid.ToString();
                    }
                    Result = irAq.ASubmitToNextQueue(queto, hobj.RequestTypeName, hobj.Remarks, status, 0);
                }
                else
                {
                    if (Session["supplierattmtlist"] != null)
                    {
                        dtgetfp = (List< SupplierActivation>)Session["supplierattmtlist"];
                        hobj.arrAttachfile = new string[dtgetfp.Count];
                        for (int i = 0; i < dtgetfp.Count; i++)
                        {
                            hobj.arrAttachfile[i] = dtgetfp[i].AttachFileName.ToString();
                            // array[i] = dtgetfp.Rows[i]["AttachName"].ToString();Asms_ActivationQueue
                        }
                    }
                    Result = irAq.ASubmitToNextQueue(queto, hobj.RequestTypeName, hobj.Remarks, status, 0);
                    result = irAq.Update_SupplierActive(hobj);
                    Session["stage"] = string.Empty;
                }
                TempData["msg"] = "Successfully moved";
                if (status == 0 && Result!=0)
                {
                    Session["Message"] = "Rejected";
                }
                else if (Result == 0)
                {
                    Session["Message"] = "Please check contracttodate";
                }
                else
                {
                    Session["Message"] = "Successfully Moved Approval";
                }

                //Areleselog = irAq.ReleaseLog(hobj.Supplierheadergid);
                //string Module = "ASMS";
                //string TransactionType = "ACTIVATION";
                //string gid = irAq.GetSupIdForMail(Convert.ToInt32(hobj.Supplierheadergid));
                //string request = (status == 0 ? "R" : "A");
                //MailAlertController sendusermail = new MailAlertController();
                //sendusermail.sendusermail(Module, TransactionType, gid, request, string.Empty);
                string gid = supmodel.GetSupIdForMail(Convert.ToInt32(Session["SupplierHeaderGid"]));
                ForMailController mailsender = new ForMailController();
                if (requestfor != "S")
                {
                    if (status == 1)
                    {
                        requestfor = "A";
                    }
                    else if (status == 0)
                    {
                        requestfor = "R";
                    }
                    if (curapprovalstage == "0")
                    {
                        requestfor = "S";
                    }
                }
                Session["SupplierHeaderGid"] = null;
                mailsender.sendusermail("ASMS", reqType.ToUpper().Trim(), gid, requestfor, curapprovalstage);
            }
            catch(Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return RedirectToAction("../AsmsMain/DashBoard");
        }

        [HttpPost]
        public JsonResult SupplierActivationApproval(SupplierActivation hobj)
        {
            List<SupervisoryApproval> objApr = new List<SupervisoryApproval>();
            int Result = 0;
            int status = 0;
            string queto = string.Empty;
            string result = string.Empty;
            string Areleselog = string.Empty;
            int skipFlag = 1; //Ramya
            string msg = "0";
            // DataTable dtgetfp = new DataTable();
            List<SupplierActivation> dtgetfp = new List<SupplierActivation>();
            try
            {
                //Session["SupplierHeaderGid"] = hobj.Supplierheadergid;
                SupDataModel supmodel = new SupDataModel();
                DataTable dtForMail = supmodel.GetMailDetails();
                var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                var curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                //status = Convert.ToInt32(string.IsNullOrEmpty(hobj.Approval) ? hobj.Reject : hobj.Approval);
                status = Convert.ToInt32(hobj.Approval);
                Session["SupplierHeaderGid"] = hobj.Supplierheadergid;
                if (Session["stage"].ToString() == string.Empty)
                {
                    // objApr = irAq.GetNextApprovalStage(hobj.RequestTypeName, hobj.CurrenStrage);
                    //   Result = irAq.SubmitToNextQueue(string.Empty, hobj.RequestTypeName, hobj.ActiveReason);
                    if (hobj.Approverid != 0)
                    {
                        queto = hobj.Approverid.ToString();
                    }
                    ////Ramya
                    if (hobj.skipFlag == 0)
                    {
                        skipFlag = 0;
                    }
                    //Commented by Ramya
                    //Result = irAq.ASubmitToNextQueue(queto, hobj.RequestTypeName, hobj.Remarks, status, 0);
                    Result = irAq.ASubmitToNextQueue(queto, hobj.RequestTypeName, hobj.Remarks, status, skipFlag);//Ramya                    
                }
                else
                {
                    if (Session["supplierattmtlist"] != null)
                    {
                        dtgetfp = (List<SupplierActivation>)Session["supplierattmtlist"];
                        hobj.arrAttachfile = new string[dtgetfp.Count];
                        for (int i = 0; i < dtgetfp.Count; i++)
                        {
                            hobj.arrAttachfile[i] = dtgetfp[i].AttachFileName.ToString();
                            // array[i] = dtgetfp.Rows[i]["AttachName"].ToString();Asms_ActivationQueue
                        }
                    }
                    Result = irAq.ASubmitToNextQueue(queto, hobj.RequestTypeName, hobj.Remarks, status, 0);
                    result = irAq.Update_SupplierActive(hobj);
                    Session["stage"] = string.Empty;
                }
                TempData["msg"] = "Successfully moved";
                if (status == 0 && Result != 0)
                {
                    Session["Message"] = "Rejected";
                    msg = "Rejected";
                }
                else if (Result == 0)
                {
                    Session["Message"] = "Please check contracttodate";
                    msg = "Please check contracttodate";
                }
                else
                {
                    Session["Message"] = "Successfully Moved Approval";
                    msg = "success";
                }
                                 
                string gid = supmodel.GetSupIdForMail(Convert.ToInt32(Session["SupplierHeaderGid"]));
                ForMailController mailsender = new ForMailController();
                if (requestfor != "S")
                {
                    if (status == 1)
                    {
                        requestfor = "A";
                    }
                    else if (status == 0)
                    {
                        requestfor = "R";
                    }
                    if (curapprovalstage == "0")
                    {
                        requestfor = "S";
                    }
                }
                Session["SupplierHeaderGid"] = null;
                mailsender.sendusermail("ASMS", reqType.ToUpper().Trim(), gid, requestfor, curapprovalstage);
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("error", JsonRequestBehavior.AllowGet);            
            }            
        }


        public PartialViewResult SupplierActivationHistory(string id)
        {
            SupplierActivation objh = new SupplierActivation();
            
            try
            {
                List<SupplierActivation> objhl = new List<SupplierActivation>();
                objhl = irAq.GetSupplierApprovalHistory(id, "getrequesthistory");
                DataTable dt = new DataTable();
                SupDataModel objModel = new SupDataModel();
                dt = objModel.GetSupplierName(Convert.ToInt32(id));
                ViewBag.SupName = dt.Rows[0]["supplierheader_name"].ToString().ToUpper();
                ViewBag.SupCode = dt.Rows[0]["supplierheader_suppliercode"].ToString();
               
                if (objhl.Count > 0)
                {
                    ViewBag.Supnextapprover = dt.Rows[0]["nextapprover"].ToString().ToUpper();
                }
                objh.SupplierHeaderList = objhl;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
          
            return PartialView(objh);
        }

        //public PartialViewResult RequestHistory(string SupHeadGid)
        //{
        //    DataTable dt = new DataTable();
        //    List<SupplierHeader> lst = new List<SupplierHeader>();
        //    lst = objModel.GetRequestHistory(Convert.ToInt32(SupHeadGid)).ToList();
        //    dt = objModel.GetSupplierName(Convert.ToInt32(SupHeadGid));
        //    ViewBag.Code = dt.Rows[0]["supplierheader_suppliercode"].ToString();
        //    ViewBag.Name = dt.Rows[0]["supplierheader_name"].ToString().ToUpper();
        //    if (lst.Count > 0)
        //    {
        //        ViewBag.nextapprover = dt.Rows[0]["nextapprover"].ToString().ToUpper();
        //    }
        //    return PartialView(lst);
        //}

    }
}
