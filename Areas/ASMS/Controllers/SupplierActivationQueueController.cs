using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.ASMS.Controllers
{
    [NoDirectAccess]
    public class SupplierActivationQueueController : Controller
    {
        //
        // GET: /SupplierActivationQueue/
        private IRepositoryRenewal IreAQ;
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        public SupplierActivationQueueController()
            : this(new SupRenwalModel())
        {

        }

        public SupplierActivationQueueController(IRepositoryRenewal IreAQS)
        {
            IreAQ = IreAQS;
        }
        public ActionResult ActivationQueue()
        {
            SupplierActivation objActi = new SupplierActivation();
            DataSet getdt = new DataSet();
            List<SupplierActivation> objvar = new List<SupplierActivation>();
            List<SupplierActivation> obj = new List<SupplierActivation>();
            try
            {
                if (Session["searchresult"] != null && Session["classfication"] != null)
                {
                    objActi.SupplierClassification = new SelectList((List<SupplierActivation>)Session["classfication"], "SupplierClassificationID", "SupplierClassificationName");

                    objActi.SupplierActiveList = (List<SupplierActivation>)Session["searchresult"];
                    ViewBag.status = "yes";
                    ViewBag.Result = "success";
                }
                else
                {
                    getdt = IreAQ.GetActivatequeue(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    if (getdt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                        {
                            objActi = new SupplierActivation();
                            objActi.ActiheaderGid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                            objActi.SupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                            objActi.SupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                            objActi.SupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                            objActi.SupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                            objActi.RequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                            objActi.RequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                            objActi.ActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                            objvar.Add(objActi);
                        }
                        // ViewBag.status = "yes";
                        // if(Session["Result"] != null)
                        // {
                        //    ViewBag.Result = "success";
                        // }
                    }
                    if (getdt.Tables[1].Rows.Count > 0)
                    {
                        for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                        {
                            objActi = new SupplierActivation();
                            objActi.SupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                            objActi.SupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                            obj.Add(objActi);
                        }
                    }
                    objActi.SupplierClassification = new SelectList(obj, "SupplierClassificationID", "SupplierClassificationName");


                    objActi.SupplierActiveList = objvar.ToList();
                }

                ViewBag.status = "yes";
                Session["supplierattmtfile"] = null;
                Session["reqstatus"] = string.Empty;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View(objActi);
        }

        [HttpPost]
        public ActionResult ActivationQueue(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            SupplierActivation objActi = new SupplierActivation();
            DataSet getdt = new DataSet();
            List<SupplierActivation> objvar = new List<SupplierActivation>();
            List<SupplierActivation> obj = new List<SupplierActivation>();
            try
            {
                getdt = IreAQ.GetActivatequeue(txtSuppliercode, txtSupplierName, ddlSupplierStatus, ddlSup_Clasification, ddlRequestType, ddlRequestStatus);
                if (getdt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                    {
                        objActi = new SupplierActivation();
                        objActi.ActiheaderGid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                        objActi.SupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                        objActi.SupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                        objActi.SupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                        objActi.SupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                        objActi.RequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                        objActi.RequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                        objActi.ActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                        objvar.Add(objActi);
                    }
                    Session["searchresult"] = objvar;
                }
                else
                {
                    ViewBag.value = "RecordNtFound";
                }
                if (getdt.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                    {
                        objActi = new SupplierActivation();
                        objActi.SupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                        objActi.SupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                        obj.Add(objActi);
                    }
                    Session["classfication"] = obj;
                }


                objActi.SupplierClassification = new SelectList(obj, "SupplierClassificationID", "SupplierClassificationName");
                ViewBag.status = "yes";

                objActi.SupplierActiveList = objvar.ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View(objActi);
        }

        public PartialViewResult SupplierActivation(string id)
        {
            List<SupplierActivation> obj = new List<SupplierActivation>();
            SupplierActivation objAct = new SupplierActivation();
            DataSet getds = new DataSet();
            DataSet dsgetsd = new DataSet();
            try
            {
                objAct.r_status = string.IsNullOrEmpty((string)Session["reqstatus"]) ? string.Empty : (string)Session["reqstatus"];

                List<SupplierActivation> objN = new List<SupplierActivation>();
                //   SupplierDeActvation objAct = new SupplierDeActvation();

                // DataSet getds = new DataSet();
                dsgetsd = IreAQ.GetpreSupplierDetails(id, "A");
                if (dsgetsd.Tables[0].Rows[0]["supplierheader_currentapprovalstage"] != null)
                {
                    objAct.CurrenStrage = Convert.ToInt32(dsgetsd.Tables[0].Rows[0]["supplierheader_currentapprovalstage"]);
                }
                if (dsgetsd.Tables[0].Rows.Count > 0)
                {
                    objAct.r_status = string.IsNullOrEmpty(dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString()) ? string.Empty : dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString();
                }
                if (dsgetsd.Tables[0].Rows.Count > 0 && (dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString() == "REJECTED" || objAct.r_status.ToUpper() == "INPROCESS" || objAct.r_status.ToUpper() == "APPROVED" || objAct.r_status.ToUpper() == "REJECTED" || objAct.r_status.ToUpper() == "WAITINGFORAPPROVAL"))
                {
                    objAct.Supplierheadergid = dsgetsd.Tables[0].Rows[0]["supplierheader_gid"].ToString();
                    objAct.SupplierCode = dsgetsd.Tables[0].Rows[0]["supplierheader_suppliercode"].ToString();
                    objAct.SupplierName = dsgetsd.Tables[0].Rows[0]["supplierheader_name"].ToString();
                    if (dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString() == "REJECTED" || objAct.r_status.ToUpper() == "INPROCESS" || objAct.r_status.ToUpper() == "REJECTED" || objAct.r_status.ToUpper() == "WAITINGFORAPPROVAL")
                    {
                        if (dsgetsd.Tables[1].Rows.Count > 0)
                        {
                            objAct.ActiveReason = dsgetsd.Tables[1].Rows[0]["supplieractivation_activationreason"].ToString();
                            objAct.Fdate = dsgetsd.Tables[1].Rows[0]["supplieractivation_activationfrom"].ToString();
                            objAct.Todate = dsgetsd.Tables[1].Rows[0]["supplieractivation_activationto"].ToString();
                            objAct.comments = dsgetsd.Tables[1].Rows[0]["supplieractivation_comments"].ToString();
                        }
                        if (dsgetsd.Tables[2].Rows.Count > 0)
                        {
                            SupplierActivation objtemp = new SupplierActivation();
                            objtemp.AttachFileName = dsgetsd.Tables[2].Rows[0]["AttachName"].ToString();
                            objtemp.Attachdate = dsgetsd.Tables[2].Rows[0]["saattachment_insert_date"].ToString();
                            objN.Add(objtemp);
                            Session["supplierattmtfile"] = dsgetsd.Tables[2];
                        }
                        else
                        {
                            Session["supplierattmtfile"] = null;
                        }
                    }
                    else
                    {
                        Session["supplierattmtfile"] = null;
                    }
                }

                else
                {


                    getds = IreAQ.GetActive_emp(id);
                    if (getds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < getds.Tables[0].Rows.Count; i++)
                        {
                            objAct = new SupplierActivation();

                        }

                        for (int j = 0; j < getds.Tables[1].Rows.Count; j++)
                        {
                            objAct = new SupplierActivation();
                            objAct.Approverid = Convert.ToInt32(getds.Tables[1].Rows[j]["employee_gid"].ToString());
                            objAct.ApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                            objAct.Supplierheadergid = getds.Tables[0].Rows[0][0].ToString();
                            objAct.SupplierCode = getds.Tables[0].Rows[0][1].ToString();
                            objAct.SupplierName = getds.Tables[0].Rows[0][2].ToString();
                            obj.Add(objAct);
                        }
                        // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
                        objAct.ApproverList = new SelectList(obj, "Approverid", "ApproverName");
                        objAct.r_status = string.Empty;
                        Session["listobj"] = obj;
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            if (dsgetsd.Tables[0].Rows.Count > 0)
            {
                Session["reqstatus"] = string.IsNullOrEmpty(dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString()) ? string.Empty : dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString();
            }
            return PartialView(objAct);
        }

        [HttpPost]
        public ActionResult SupplierActivation(SupplierActivation obj)
        {
            try
            {
                int result1 = 0;
                Session["SupplierHeaderGid"] = obj.Supplierheadergid;
                SupDataModel supmodel = new SupDataModel();
                var reqType = "";
                var requestfor = "S";
                var curapprovalstage = "0";
                DataTable dtForMail = supmodel.GetMailDetails();
                try
                { 
                     reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                     requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                     curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                }
                catch (Exception ex)
                {
                    
                }

                DataTable dtgetfp = new DataTable();
                //   int i = 0;
                List<SupplierActivation> objj = new List<SupplierActivation>();
                List<SupplierActivation> objP = new List<SupplierActivation>();

                if (Session["supplierattmtfile"] != null)
                {
                    dtgetfp = (DataTable)Session["supplierattmtfile"];
                    obj.arrAttachfile = new string[dtgetfp.Rows.Count];
                    for (int i = 0; i < dtgetfp.Rows.Count; i++)
                    {
                        obj.arrAttachfile[i] = dtgetfp.Rows[i]["AttachName"].ToString();
                        // array[i] = dtgetfp.Rows[i]["AttachName"].ToString();Asms_ActivationQueue
                    }
                }
                //string Result = IreAQ.Update_SupplierActive(obj);

                string Result = IreAQ.Asms_ActivationQueue(obj);
                Session["SupplierHeaderGid"] = string.IsNullOrEmpty(obj.Supplierheadergid) ? obj.ActiheaderGid : obj.Supplierheadergid;
                result1 = IreAQ.ASubmitToNextQueue(string.Empty, "ACTIVATION", obj.Remarks, 1, 0);
                objj = (List<SupplierActivation>)Session["listobj"];
                obj.SupplierCode = string.Empty;
                obj.SupplierName = string.Empty;

                obj.Todate = string.Empty;
                //  obj.ApproverList = new SelectList(objj, "Approverid", "ApproverName");
                if (Result == "success" && result1 != 0)
                {
                    Session["Active"] = "yes";
                }

                string gid = supmodel.GetSupIdForMail(Convert.ToInt32(Session["SupplierHeaderGid"]));
                ForMailController mailsender = new ForMailController();
                int status = 1;
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
                    //  curapprovalstage = (Convert.ToInt32(curapprovalstage) - 1).ToString();
                }
                reqType = "ACTIVATION";
                Session["SupplierHeaderGid"] = null;
                mailsender.sendusermail("ASMS", reqType.ToUpper().Trim(), gid, requestfor, curapprovalstage);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return RedirectToAction("../SupplierActivationQueue/ActivationQueue");
        }


        public PartialViewResult SupplierAttachments(SupplierActivation obj, List<HttpPostedFileBase> fileUpload)
        {
            //  string filename = string.Empty;
            //  List<SupplierActivation> lobj = new List<SupplierActivation>();
            //  foreach (string upload in Request.Files)
            //  {
            //      if (Request.Files[upload].ContentLength > 0)
            //      {
            //          string path = "//192.168.0.130/images/";
            //           filename = Path.GetFileName(Request.Files[upload].FileName);
            //          Request.Files[upload].SaveAs(Path.Combine(path, filename));
            //      }


            //  }
            ////  string file = obj.AttachFileName.ToString();
            //  string file = obj.FilesToBeUploaded.ToString();
            // // string filename;
            //  //FileUpload fu = new FileUpload();
            //  //fu.FileName(obj.AttachFileName);
            //  //FileInfo fi = new FileInfo(obj.AttachFileName);
            //  FileInfo fi = new FileInfo(obj.FilesToBeUploaded);
            //  string fnam = fi.Name;
            //  obj.AttachFileName = fnam;
            //  obj.Today = DateTime.Now.ToString("dd-MM-yyyy");
            //  lobj.Add(obj);

            //  obj.AttchList = lobj.ToList();
            //  ViewBag.status = "yes";
            SupplierActivation objActt = new SupplierActivation();

            return PartialView(objActt);
        }


        [HttpPost]
        public virtual ActionResult SupplierAttachments(string uploadfor, string attach = null, string attaching_format = null)
        {
            // public async Task<JsonResult> SupplierAttachments()
            DataTable dtac = new DataTable();
            dtac.Columns.Add("Attachid");
            dtac.Columns.Add("AttachName");
            dtac.Columns.Add("AttachFileType");
            dtac.Columns.Add("Attachlength");
            dtac.Columns.Add("flag");
            int j = 1;
            HttpPostedFileBase myFile = Request.Files["AMyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            try
            {
                if (myFile != null && myFile.ContentLength != 0)
                {
                    message = "Enter into if";
                    string pathForSaving = HttpContext.Application["path"] as string;

                    string fileextension = Path.GetExtension(Path.GetFileName(myFile.FileName));
                    var fileName = Path.GetFileName(myFile.FileName);
                    string[] attaching_fileformat = attaching_format.Split(',');
                    if (attaching_fileformat.Contains(fileextension.ToLower()))
                    {
                        var stream = myFile.InputStream;
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                        if (isEXE == false)
                        {
                            var FileString = Convert.ToBase64String(bytFile);
                            var result = Cmnfs.SaveFileToServer(FileString, fileName).Result;
                            isUploaded = true;
                            DataRow dr = dtac.NewRow();
                            dr["Attachid"] = j.ToString();
                            // dr["AttachName"] = Request.Files[file].FileName.ToString();
                            dr["AttachName"] = Path.GetFileName(Request.Files["AMyFile"].FileName);
                            dr["AttachFileType"] = Request.Files["AMyFile"].ContentType.ToString();
                            dr["Attachlength"] = Request.Files["AMyFile"].ContentLength.ToString();
                            dr["flag"] = 0;
                            dtac.Rows.Add(dr);

                            if (Session["supplierattmtfile"] != null)
                            {
                                int a = 1;
                                DataTable dtprevalue = new DataTable();
                                dtprevalue = (DataTable)Session["supplierattmtfile"];
                                for (int k = 0; dtprevalue.Rows.Count > k; k++)
                                {
                                    DataRow dratt = dtac.NewRow();
                                    dratt["Attachid"] = dtac.Rows.Count + 1;
                                    dratt["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                                    dratt["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                                    dratt["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                                    dratt["flag"] = 1;
                                    dtac.Rows.Add(dratt);
                                    a++;
                                }

                            }
                            Session["supplierattmtfile"] = dtac;

                        }


                    }

                }


                j++;

                /* if (Session["supplierattmtfile"] != null)
                 {
                     //int a = 1;
                    // DataTable dtprevalue = new DataTable();
                    // dtprevalue = (DataTable)Session["supplierattmtfile"];
                     //for (int k = 0; dtprevalue.Rows.Count > k; k++)
                     //{
                         //DataRow dr = dtac.NewRow();
                         //dr["Attachid"] = dtac.Rows.Count + 1;
                         //dr["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                         //dr["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                         //dr["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                         //dr["flag"] = 1;
                         //dtac.Rows.Add(dr);
                     //    a++;
                     //}
               
                 } */
                //Session["supplierattmtfile"] = dtac;


                return Json(new { isUploaded = isUploaded, message = message }, "text/html");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }


        }

        [HttpPost]
        public async Task<JsonResult> remove1(SupplierActivation obj)
        {
            string filename = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["supplierattmtfile"];
                //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                //{
                //    int j = Convert.ToInt32(obj.FilesToBeUploaded);
                //    DataRow dr = dt.Rows[i];
                //    if (dr["Attachid"] == obj.FilesToBeUploaded)
                //        dr.Delete();
                //    dt.Rows[j].Delete();
                //}

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Attachid"].ToString() == obj.FilesToBeUploaded)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();
                Session["supplierattmtfile"] = dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }

            return Json(filename);
        }

        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
    }
}
