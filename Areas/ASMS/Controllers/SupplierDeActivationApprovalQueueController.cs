using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierDeActivationApprovalQueueController : Controller
    {
       IRepositoryRenewal irAeq;
       ErrorLog objErrorLog = new ErrorLog();
        public SupplierDeActivationApprovalQueueController()
           :this  (new SupRenwalModel())
    {

    }

        public SupplierDeActivationApprovalQueueController(IRepositoryRenewal qirAeq)
        {
            irAeq = qirAeq;
        }

        public ActionResult SupplierDeActivationApproval(string queuefor = null, string requesttype = null, string reqstatus = null)
        {
            DataTable dtsc = new DataTable();
            SupplierDeActvation objActi = new SupplierDeActvation();
            List<SupplierDeActvation> laobj = new List<SupplierDeActvation>();
            List<SupplierDeActvation> losc = new List<SupplierDeActvation>();
            try
            {

                laobj = irAeq.GetSupHeaderDeDetailsDashboard(requesttype, reqstatus);
                Session["objlist"] = laobj;
                losc = irAeq.GetDeSuppliercatogory(string.Empty);
                Session["category"] = losc;
                objActi.DSupplierClassification = new SelectList(losc, "DSupplierClassificationID", "DSupplierClassificationName");
                objActi.SupplierDeActiveList = laobj.ToList();
                ViewBag.status = "yes";
                Session["Dreqstatus"] = reqstatus;
                // }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View(objActi);
        }

        [HttpPost]
        public ActionResult SupplierDeActivationApproval(string txtDeSuppliercode, string txtDeSupplierName, string Common, string ddlDeSup_Clasification, string ddlDeRequestType, string ddlDeRequestStatus)
        {
            SupplierDeActvation objActi = new SupplierDeActvation();
            DataSet getdt = new DataSet();
            List<SupplierDeActvation> objvar = new List<SupplierDeActvation>();
            List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            List<SupplierDeActvation> aobj = new List<SupplierDeActvation>();

            try
            {
                objActi.SupplierDeActiveList = (List<SupplierDeActvation>)Session["objlist"];
                obj = irAeq.GetDeSuppliercatogory(ddlDeSup_Clasification);
                aobj = irAeq.GetDeEmployeelist();
                if (ddlDeSup_Clasification != "0")
                {
                    objActi.DSupplierClassificationName = obj[0].DSupplierClassificationName;
                }

                if (Common == "Search")
                {

                    if ((string.IsNullOrEmpty(txtDeSuppliercode)) == false)
                    {
                        ViewBag.txtDeSuppliercode = txtDeSuppliercode;
                        objActi.SupplierDeActiveList = objActi.SupplierDeActiveList.Where(x => txtDeSuppliercode == null ||
                           (x.DSupplierCode.ToUpper().Contains(txtDeSuppliercode.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierDeActiveList;
                    }
                    if ((string.IsNullOrEmpty(txtDeSupplierName)) == false)
                    {
                        ViewBag.txtDeSupplierName = txtDeSupplierName;
                        objActi.SupplierDeActiveList = objActi.SupplierDeActiveList.Where(x => txtDeSupplierName == null ||
                           (x.DSupplierName.ToUpper().Contains(txtDeSupplierName.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierDeActiveList;
                    }
                    if (ddlDeSup_Clasification != "0")
                    {
                        ViewBag.ddlDeSup_Clasification = ddlDeSup_Clasification;
                        objActi.SupplierDeActiveList = objActi.SupplierDeActiveList.Where(x => ddlDeSup_Clasification == null ||
                          (x.DSupplierClassificationName.ToUpper().Contains(objActi.DSupplierClassificationName.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierDeActiveList;
                    }
                    if (ddlDeRequestType != "0")
                    {
                        ViewBag.ddlDeRequestType = ddlDeRequestType;
                        objActi.SupplierDeActiveList = objActi.SupplierDeActiveList.Where(x => ddlDeRequestType == null ||
                           (x.DRequestTypeName.ToUpper().Contains(ddlDeRequestType.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierDeActiveList;
                    }
                    if (ddlDeRequestStatus != "0")
                    {
                        ViewBag.ddlDeRequestStatus = ddlDeRequestStatus;
                        objActi.SupplierDeActiveList = objActi.SupplierDeActiveList.Where(x => ddlDeRequestStatus == null ||
                           (x.DRequestStatusName.ToUpper().Contains(ddlDeRequestStatus.ToUpper()))).ToList();
                        Session["objlist1"] = objActi.SupplierDeActiveList;
                    }
                }
                else
                {
                    objActi.SupplierDeActiveList = (List<SupplierDeActvation>)Session["objlist"];
                }

                objActi.DSupplierClassification = new SelectList((List<SupplierDeActvation>)Session["category"], "DSupplierClassificationID", "DSupplierClassificationName");
                objActi.DApproverList = new SelectList(aobj, "employee_gid", "employee_name");
                ViewBag.status = "yes";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return View(objActi);
        }



        public PartialViewResult SupplierApproveDeActivation(string id)
        {
            List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            List<SupplierDeActvation> objA = new List<SupplierDeActvation>();
            List<SupplierDeActvation> objF = new List<SupplierDeActvation>();
            SupplierDeActvation objAct = new SupplierDeActvation();
            DataSet getds = new DataSet();
            string logedstatus = string.Empty;
            CmnFunctions objCmnFunctions = new CmnFunctions();
            CommonIUD objCommonIUD = new CommonIUD();
            try
            {
                string user=Convert.ToString(objCmnFunctions.GetLoginUserGid());
            //    logedstatus = objCommonIUD.SupplierLocked(id, user); // SupplierLocked
               
                    getds = irAeq.GetActive_emptemp(id, "D");
                    if (getds.Tables[0].Rows.Count > 0)
                    {

                        //  for (int j = 0; j < getds.Tables[1].Rows.Count; j++)
                        //  {
                        objAct = new SupplierDeActvation();
                        //   objAct.DApproverid = getds.Tables[1].Rows[j]["employee_gid"].ToString();
                        // objAct.DApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                        objAct.DSupplierheadergid = getds.Tables[0].Rows[0]["Suppliergid"].ToString();
                        objAct.DSupplierCode = getds.Tables[0].Rows[0]["SupplierCode"].ToString();
                        objAct.DSupplierName = getds.Tables[0].Rows[0]["SupplierName"].ToString();
                        // objAct.DSupplierStatusName = getds.Tables[0].Rows[0]["Status"].ToString();
                        objAct.DFdate = getds.Tables[0].Rows[0]["ActivationFrom"].ToString();
                        objAct.DTodate = getds.Tables[0].Rows[0]["ActivationTo"].ToString();
                        objAct.DeActiveReason = getds.Tables[0].Rows[0]["ActivateReson"].ToString();
                        objAct.Decomments = getds.Tables[0].Rows[0]["ActiveComments"].ToString();
                        obj.Add(objAct);
                        // }
                        Session["SupplierHeaderGid"] = objAct.DSupplierheadergid;
                        SupDataModel dm = new SupDataModel();
                        objAct._IsExistsFlagApprover = dm.IsExistsFlagApprover();
                        DataTable dtIsExistsFlagApprover = new DataTable();
                        dtIsExistsFlagApprover = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                        objAct._IsExistsApproverName = (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString().ToUpper()) + "-" + (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString().ToUpper());
                        if (getds.Tables[2].Rows.Count > 0)
                        {
                            for (int k = 0; k < getds.Tables[2].Rows.Count; k++)
                            {
                                SupplierDeActvation objActN = new SupplierDeActvation();
                                objActN.DAttachid = Convert.ToInt32(getds.Tables[2].Rows[k]["saattachment_gid"].ToString());
                                objActN.DAttachFileName = getds.Tables[2].Rows[k]["saattachment_filename"].ToString();
                                objActN.DAttachdate = getds.Tables[2].Rows[k]["saattachment_insert_date"].ToString();
                                objA.Add(objActN);
                                Session["Dsupplierattmtlist"] = objA;
                            }
                        }
                        else
                        {
                            Session["Dsupplierattmtlist"] = objA;
                        }
                       
                        // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
                        objAct.DAttchList = objA.ToList();
                        objF = irAeq.GetEmployeeDeListForApproval(id);
                        objAct.DApproverList = new SelectList(objF, "DApproverid", "DApproverName");
                        //  objAct.DApproverList = new SelectList(obj, "DApproverid", "DApproverName");

                        Session["listobj"] = obj;
                        objAct.DCurrenStrage = Convert.ToInt32(getds.Tables[0].Rows[0]["supplierheader_currentapprovalstage"].ToString());
                        objAct.DRequestTypeName = getds.Tables[0].Rows[0]["supplierheader_requesttype"].ToString();
                        Session["Dstage"] = string.Empty;
                        if (objAct.DCurrenStrage == 1)
                        {
                            //  ViewBag.DMessage = "Supervisory Approval";
                            objAct.DQueueStatus = "Supervisory Approval";
                        }
                        else if (objAct.DCurrenStrage == 2)
                        {
                            objAct.DQueueStatus = "VMU Checker";
                            // ViewBag.DMessage = "VMU Checker";
                        }
                        else if (objAct.DCurrenStrage == 3)
                        {
                            objAct.DQueueStatus = "Funcational Head Approval";
                            //ViewBag.DMessage = "Funcational Head Approval";
                        }
                        else if (objAct.DCurrenStrage == 4)
                        {
                            // ViewBag.DMessage = "VMU Head Approval";
                            objAct.DQueueStatus = "VMU Head Approval";
                            Session["Dstage"] = "VMU Head Approval";
                        }
                    }
                    if (logedstatus == "Success")
                    {
                        objAct.logstatus = "N";
                    }
                    else
                    {
                        objAct.logstatus = logedstatus;
                    }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(objAct);
        }

        [HttpPost]
        public ActionResult SupplierApproveDeActivation(SupplierDeActvation hobj)
        {
            List<SupervisoryApproval> objApr = new List<SupervisoryApproval>();
            int Result = 0;
            string data = "";
            int status = 0;
            string queto = string.Empty;
            string result = string.Empty;
            string releselog = string.Empty;
            Int16 skipFlag = 0; //Ramya
            // DataTable dtgetfp = new DataTable();
            List<SupplierDeActvation> dtgetfp = new List<SupplierDeActvation>();
            try
            {
                Session["SupplierHeaderGid"] = hobj.DSupplierheadergid;
                SupDataModel supmodel = new SupDataModel();
                DataTable dtForMail = supmodel.GetMailDetails();
                var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                var curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                status = Convert.ToInt32(string.IsNullOrEmpty(hobj.DApproval) ? hobj.DReject : hobj.DApproval);
                Session["DSupplierHeaderGid"] = hobj.DSupplierheadergid;
                skipFlag = Convert.ToInt16(hobj.DskipFlag);

                if (Session["Dstage"].ToString() == string.Empty)
                {
                    // objApr = irAq.GetNextApprovalStage(hobj.RequestTypeName, hobj.CurrenStrage);
                    //   Result = irAq.SubmitToNextQueue(string.Empty, hobj.RequestTypeName, hobj.ActiveReason);
                    if (hobj.DApproverid != 0)
                    {
                        queto = hobj.DApproverid.ToString();
                    }
                    Result = irAeq.SubmitToNextQueue(queto, hobj.DRequestTypeName, hobj.DRemarks, status, skipFlag);
                }
                else
                {
                    if (Session["Dsupplierattmtlist"] != null)
                    {
                        dtgetfp = (List<SupplierDeActvation>)Session["Dsupplierattmtlist"];
                        hobj.arrDeAttachfile = new string[dtgetfp.Count];
                        for (int i = 0; i < dtgetfp.Count; i++)
                        {
                            hobj.arrDeAttachfile[i] = dtgetfp[i].DAttachFileName.ToString();
                            // array[i] = dtgetfp.Rows[i]["AttachName"].ToString();Asms_ActivationQueue
                        }
                    }
                    result = irAeq.DeUpdate_SupplierActive(hobj);
                    Result = irAeq.SubmitToNextQueue(queto, hobj.DRequestTypeName, hobj.DRemarks, status, skipFlag);
                 
                    Session["Dstage"] = string.Empty;
                }
                if(status==0)
                {
                    Session["Message"] = "Rejected";
                    data = "Rejected";
                }
                else
                {
                    Session["Message"] = "Successfully Submitted";
                    data = "success";
                }
             //   releselog = irAeq.ReleaseLog(hobj.DSupplierheadergid);
                //string Module = "ASMS";
                //string TransactionType = "DEACTIVATION";
                //string gid = irAeq.GetSupIdForMail(Convert.ToInt32(hobj.DSupplierheadergid));
                //string request = (status== 0 ? "R" :"A");
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                data = ex.Message.ToString();
            }
            //return RedirectToAction("../AsmsMain/DashBoard");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SupplierDeactivationHistory(string id)
        {
            SupplierDeActvation objh = new SupplierDeActvation();
            try
            {
                List<SupplierDeActvation> objhl = new List<SupplierDeActvation>();
                objhl = irAeq.GetDSupplierApprovalHistory(id, "getrequesthistory");
                objh.DSupplierHeaderList = objhl;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
          
            return PartialView(objh);
        }
    }
}
