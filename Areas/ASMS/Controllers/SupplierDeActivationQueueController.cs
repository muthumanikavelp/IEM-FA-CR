using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using IEM.Areas.EOW.Models;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierDeActivationQueueController : Controller
    {
        private IRepositoryRenewal IrDeA;
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        public SupplierDeActivationQueueController()
            :this(new SupRenwalModel())
        {

        }
        public SupplierDeActivationQueueController(IRepositoryRenewal IrDeAc)
        {
            IrDeA = IrDeAc;
        }

        public ActionResult SupplierDeActivation()
        {

            SupplierDeActvation objActi = new SupplierDeActvation();
            DataSet getdt = new DataSet();
            List<SupplierDeActvation> objvar = new List<SupplierDeActvation>();
            List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            try
            {
                getdt = IrDeA.GetDeActivatequeue(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                if (getdt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                    {
                        objActi = new SupplierDeActvation();
                        objActi.DSupplierheadergid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                        objActi.DSupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                        objActi.DSupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                        objActi.DSupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                        objActi.DSupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                        objActi.DRequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                        objActi.DRequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                        objActi.DActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                        objvar.Add(objActi);
                    }
                    Session["Desearchresult"] = objvar;
                }
                if (getdt.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                    {
                        objActi = new SupplierDeActvation();
                        objActi.DSupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                        objActi.DSupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                        obj.Add(objActi);
                    }
                    Session["Declassfication"] = obj;
                }

                objActi.DSupplierClassification = new SelectList(obj, "DSupplierClassificationID", "DSupplierClassificationName");
                ViewBag.status = "yes";

                objActi.SupplierDeActiveList = objvar.ToList();
                Session["supplierattmtfileDe"] = null;
                Session["Dreqstatus"] = string.Empty;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            
            return View(objActi);
        }

        [HttpPost]
        public ActionResult SupplierDeActivation(string txtDeSuppliercode, string txtDeSupplierName, string ddlDeSupplierStatus, string ddlDeSup_Clasification, string ddlDeRequestType, string ddlDeRequestStatus)
        {
            SupplierDeActvation objActi = new SupplierDeActvation();
            DataSet getdt = new DataSet();
            List<SupplierDeActvation> objvar = new List<SupplierDeActvation>();
            List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            try
            {
                getdt = IrDeA.GetDeActivatequeue(txtDeSuppliercode, txtDeSupplierName, ddlDeSupplierStatus, ddlDeSup_Clasification, ddlDeRequestType, ddlDeRequestStatus);
                if (getdt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                    {
                        objActi = new SupplierDeActvation();
                        objActi.DSupplierheadergid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                        objActi.DSupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                        objActi.DSupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                        objActi.DSupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                        objActi.DSupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                        objActi.DRequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                        objActi.DRequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                        objActi.DActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                        objvar.Add(objActi);
                    }

                }
                else
                {
                    ViewBag.Dvalue = "RecordNtFound";
                }

                if (getdt.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                    {
                        objActi = new SupplierDeActvation();
                        objActi.DSupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                        objActi.DSupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                        obj.Add(objActi);
                    }
                    objActi.DSupplierClassification = new SelectList(obj, "DSupplierClassificationID", "DSupplierClassificationName");
                }

                objActi.SupplierDeActiveList = objvar.ToList();
                ViewBag.status = "yes";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
          
            return View(objActi);
        }

        public PartialViewResult SpplierDeActive(string id)
        {
           DataSet dsgetsd = new DataSet();
            List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            SupplierDeActvation objAct = new SupplierDeActvation();
            try
            {

                objAct.Dr_status = string.IsNullOrEmpty((string)Session["Dreqstatus"]) ? string.Empty : (string)Session["Dreqstatus"];
                DataSet getds = new DataSet();
                dsgetsd = IrDeA.GetpreSupplierDetails(id, "D");
                if (dsgetsd.Tables[0].Rows.Count > 0)
                {
                    objAct.Dr_status = string.IsNullOrEmpty(dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString()) ? string.Empty : dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString();
                }
                if (dsgetsd.Tables[0].Rows.Count > 0 && (dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString() == "REJECTED" || objAct.Dr_status.ToUpper() == "INPROCESS" || objAct.Dr_status.ToUpper() == "APPROVED" || objAct.Dr_status.ToUpper() == "REJECTED" || objAct.Dr_status.ToUpper() == "WAITINGFORAPPROVAL"))
                // if (dsgetsd.Tables[0].Rows.Count > 0 && dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString() == "REJECTED" )
                {


                    objAct.DSupplierheadergid = dsgetsd.Tables[0].Rows[0]["supplierheader_gid"].ToString();
                    objAct.DSupplierCode = dsgetsd.Tables[0].Rows[0]["supplierheader_suppliercode"].ToString();
                    objAct.DSupplierName = dsgetsd.Tables[0].Rows[0]["supplierheader_name"].ToString();
                    if (dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString() == "REJECTED" || objAct.Dr_status.ToUpper() == "INPROCESS" || objAct.Dr_status.ToUpper() == "REJECTED" || objAct.Dr_status.ToUpper() == "WAITINGFORAPPROVAL")
                    {
                        if (dsgetsd.Tables[1].Rows.Count > 0)
                        {
                            objAct.DeActiveReason = dsgetsd.Tables[1].Rows[0]["supplieractivation_activationreason"].ToString();
                            objAct.Decomments = dsgetsd.Tables[1].Rows[0]["supplieractivation_comments"].ToString();
                        }
                        if (dsgetsd.Tables[2].Rows.Count > 0)
                        {
                            SupplierDeActvation objtemp = new SupplierDeActvation();
                            objtemp.DAttachFileName = dsgetsd.Tables[2].Rows[0]["AttachName"].ToString();
                            objtemp.DAttachdate = dsgetsd.Tables[2].Rows[0]["saattachment_insert_date"].ToString();
                            obj.Add(objtemp);
                            Session["supplierattmtfileDe"] = dsgetsd.Tables[2];
                        }
                        else
                        {
                            Session["supplierattmtfileDe"] = null;
                        }
                    }
                    else
                    {
                        Session["supplierattmtfileDe"] = null;
                    }

                }
                else
                {


                    getds = IrDeA.GetActive_emp(id);
                    if (getds.Tables[0].Rows.Count > 0)
                    {

                        //for (int j = 0; j < getds.Tables[1].Rows.Count; j++)
                        //{
                        //    objAct = new SupplierDeActvation();
                        //objAct.Approverid = Convert.ToInt32(getds.Tables[1].Rows[j]["employee_gid"].ToString());
                        //objAct.ApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                        objAct.DSupplierheadergid = getds.Tables[0].Rows[0][0].ToString();
                        objAct.DSupplierCode = getds.Tables[0].Rows[0][1].ToString();
                        objAct.DSupplierName = getds.Tables[0].Rows[0][2].ToString();
                        // obj.Add(objAct);
                        // }
                        // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
                        //   objAct.ApproverList = new SelectList(obj, "Approverid", "ApproverName");
                        //Session["listobj"] = obj;
                    }
                    objAct.Dr_status = string.Empty;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            if (dsgetsd.Tables[0].Rows.Count > 0)
            {
                Session["Dreqstatus"] = string.IsNullOrEmpty(dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString()) ? string.Empty : dsgetsd.Tables[0].Rows[0]["supplierheader_requeststatus"].ToString();
            }
            return PartialView(objAct);
        }

        [HttpPost]
        public JsonResult SpplierDeActive(SupplierDeActvation objDe)
        {
            string Result = string.Empty;
            DataTable dtgetfpp = new DataTable();

            //   int i = 0;
            List<SupplierDeActvation> objj = new List<SupplierDeActvation>();
            List<SupplierDeActvation> objP = new List<SupplierDeActvation>();
            try
            {
                Session["SupplierHeaderGid"] = objDe.DSupplierheadergid;
                IEM.Areas.ASMS.Models.SupDataModel supmodel = new IEM.Areas.ASMS.Models.SupDataModel();
                DataTable dtForMail = supmodel.GetMailDetails();
                var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                var curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                if (Session["supplierattmtfileDe"] != null)
                {
                    dtgetfpp = (DataTable)Session["supplierattmtfileDe"];
                    if (dtgetfpp.Rows.Count > 0)
                    {
                        objDe.arrDeAttachfile = new string[dtgetfpp.Rows.Count];
                        for (int i = 0; i < dtgetfpp.Rows.Count; i++)
                        {
                          //  if (dtgetfpp.Rows[i]["flag"].ToString() != "1" )
                           // {
                                objDe.arrDeAttachfile[i] = dtgetfpp.Rows[i]["AttachName"].ToString();
                          //  }
                            // array[i] = dtgetfp.Rows[i]["AttachName"].ToString();Asms_DeActivationQueue
                        }
                    }
                }
                // Result = IrDeA.DeUpdate_SupplierActive(objDe);
                Result = IrDeA.Asms_DeActivationQueue(objDe);


               Session["DSupplierHeaderGid"] = objDe.DSupplierheadergid;
                IrDeA.SubmitToNextQueue(string.Empty, "DEACTIVATION", objDe.DRemarks, 1, 0);
               // objj = (List<SupplierDeActvation>)Session["listobj"];
            objDe.DSupplierCode = string.Empty;
            objDe.DSupplierName = string.Empty;
 
            objDe.DTodate = string.Empty;
           // objDe.DApproverList = new SelectList(objj, "DApproverid", "DApproverName");
            Session["result"] = Result;
            //string Module = "ASMS";
            //string TransactionType = "DEACTIVATION";
            //string gid =IrDeA.GetSupIdForMail(Convert.ToInt32(objDe.DSupplierheadergid));
            //string request = "S";
            //MailAlertController sendusermail = new MailAlertController();
            //sendusermail.sendusermail(Module, TransactionType, gid, request, string.Empty);
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
                
            }
            reqType = "DEACTIVATION";
            Session["SupplierHeaderGid"] = null;
            mailsender.sendusermail("ASMS", reqType.ToUpper().Trim(), gid, requestfor, curapprovalstage);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objDe,JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SupplierAttachmentsDe(SupplierDeActvation obj, List<HttpPostedFileBase> fileUpload)
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
            SupplierDeActvation objActt = new SupplierDeActvation();

            return PartialView(objActt);
        }


     //   [HttpPost]
    
        //public  JsonResult SupplierAttachmentsDe()
        //{
        //    //   public async Task<JsonResult> SupplierAttachmentsDe()
        //    DataTable dtac = new DataTable();
        //    dtac.Columns.Add("Attachid");
        //    dtac.Columns.Add("AttachName");
        //    dtac.Columns.Add("AttachFileType");
        //    dtac.Columns.Add("Attachlength");
        //    dtac.Columns.Add("flag");
        //    int j = 1;
        //    try
        //    {
        //        string filename = "";
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //               // string path = "//192.168.0.130/images/";
        //                string path = HttpContext.Application["path"] as string;
        //                filename = Path.GetFileName(Request.Files[file].FileName);
        //                Session["path"] = path;
        //                Request.Files[file].SaveAs(Path.Combine(path, filename));
        //                DataRow dr = dtac.NewRow();
        //                dr["Attachid"] = j.ToString();
        //             //   dr["AttachName"] = Request.Files[file].FileName.ToString();
        //                dr["AttachName"] = filename;
        //                dr["AttachFileType"] = Request.Files[file].ContentType.ToString();
        //                dr["Attachlength"] = Request.Files[file].ContentLength.ToString();
        //                dr["flag"] = 0;
        //                //SupplierActivation objj = new SupplierActivation();
        //                //objj.AttachFileName = Request.Files[file].FileName.ToString();
        //                //objj.AttachFileType = Request.Files[file].ContentType.ToString();
        //                // HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
        //                //Session["Postfile"] = hpf;
        //                dtac.Rows.Add(dr);
        //            }
        //          //  Session["supplierattmtfileDe"] = dtac;
        //            j++;

        //        }
        //        if (Session["supplierattmtfileDe"] !=null)
        //        {
        //            int a = 1;
        //            DataTable dtprevalue = new DataTable();
        //            dtprevalue = (DataTable)Session["supplierattmtfileDe"];
        //            for(int k=0; dtprevalue.Rows.Count > k;k++)
        //            {
        //                DataRow dr = dtac.NewRow();
        //                dr["Attachid"] = dtac.Rows.Count+1;
        //                dr["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
        //                dr["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
        //                dr["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
        //                dr["flag"] = 1;
        //                dtac.Rows.Add(dr);
        //                a++;
        //            }
                  
        //        }
        //        Session["supplierattmtfileDe"] = dtac;

        //        return Json(filename);
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> removeDe1(SupplierDeActvation obj)
        {

            string filename = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["supplierattmtfileDe"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Attachid"].ToString() == obj.FilesToBeUploaded)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    Session["supplierattmtfileD"] = null;
                }
                else
                {
                    Session["supplierattmtfileD"] = dt;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
          
            
            return Json(filename);
        }


           #region Actions

    /*    /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult SupplierAttachmentsDe()
        {

            DataTable dtac = new DataTable();
            dtac.Columns.Add("Attachid");
            dtac.Columns.Add("AttachName");
            dtac.Columns.Add("AttachFileType");
            dtac.Columns.Add("Attachlength");
            dtac.Columns.Add("flag");
            int j = 1;
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving =   HttpContext.Application["path"] as string;;
                //if (this.CreateFolderIfNeeded(pathForSaving))
                //{
                  
                    try
                    {
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                      //  myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                       // myFile.SaveAs(Path.Combine(@"C:\temp\", myFile.FileName));
                        myFile.SaveAs(Path.Combine(CmnFilePath, myFile.FileName));
                        var path = Path.Combine(CmnFilePath, myFile.FileName);

                        //Muthu 19-10-2016
                        //byte[] bytFile = System.IO.File.ReadAllBytes(path);
                        //FileServer objserver = new FileServer();
                        //string result = objserver.SaveFiles(bytFile, myFile.FileName);
                        var stream = myFile.InputStream;
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        var fileName = Path.GetFileName(myFile.FileName);
                        var result = Cmnfs.SaveFileToServer(FileString, fileName).Result;
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        DataRow dr = dtac.NewRow();
                        dr["Attachid"] = j.ToString();
                        //   dr["AttachName"] = Request.Files[file].FileName.ToString();
                        dr["AttachName"] =Path.GetFileName( Request.Files["MyFile"].FileName);
                        dr["AttachFileType"] = Request.Files["MyFile"].ContentType.ToString();
                        dr["Attachlength"] = Request.Files["MyFile"].ContentLength.ToString();
                        dr["flag"] = 0;
                        //SupplierActivation objj = new SupplierActivation();
                        //objj.AttachFileName = Request.Files[file].FileName.ToString();
                        //objj.AttachFileType = Request.Files[file].ContentType.ToString();
                        // HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        //Session["Postfile"] = hpf;
                        dtac.Rows.Add(dr);



                                         isUploaded = true;
                        message = "File uploaded successfully!";
                        if (Session["supplierattmtfileDe"] != null)
                        {
                            int a = 1;
                            DataTable dtprevalue = new DataTable();
                            dtprevalue = (DataTable)Session["supplierattmtfileDe"];
                            for (int k = 0; dtprevalue.Rows.Count > k; k++)
                            {
                                DataRow dr1 = dtac.NewRow();
                                dr1["Attachid"] = dtac.Rows.Count + 1;
                                dr1["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                                dr1["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                                dr1["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                                dr1["flag"] = 1;
                                dtac.Rows.Add(dr1);
                                a++;
                            }

                        }
                        Session["supplierattmtfileDe"] = dtac;
                     
                    }
                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                //}
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }*/



        /// <summary>
       /// Uploads the file.
       /// </summary>
       /// <returns></returns>
       [HttpPost]
        public virtual ActionResult SupplierAttachmentsDe(string uploadfor, string attach = null, string attaching_format = null)
       {

           DataTable dtac = new DataTable();
           dtac.Columns.Add("Attachid");
           dtac.Columns.Add("AttachName");
           dtac.Columns.Add("AttachFileType");
           dtac.Columns.Add("Attachlength");
           dtac.Columns.Add("flag");
           int j = 1;
           //  Get all files from Request object  
           HttpFileCollectionBase mFile = Request.Files;
           HttpPostedFileBase myFile = mFile[0];
         
           bool isUploaded = false;
           string message = "File upload failed";

           if (myFile != null && myFile.ContentLength != 0)
           {
               string pathForSaving = HttpContext.Application["path"] as string;

               try
               {
                   

                     string fileextension = Path.GetExtension(Path.GetFileName(myFile.FileName)); 
                    var fileName = Path.GetFileName(myFile.FileName);
                     string []attaching_fileformat=attaching_format.Split(',');
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
                             DataRow dr = dtac.NewRow();
                             dr["Attachid"] = j.ToString();
                             dr["AttachName"] = myFile.FileName;//Path.GetFileName( Request.Files["MyFile"].FileName);
                             dr["AttachFileType"] = myFile.ContentType.ToString();//Request.Files["MyFile"].ContentType.ToString();
                             dr["Attachlength"] = myFile.ContentLength.ToString();//Request.Files["MyFile"].ContentLength.ToString();
                             dr["flag"] = 0;
                             dtac.Rows.Add(dr);

                       isUploaded = true;
                       message = "File uploaded successfully!";
                       if (Session["supplierattmtfileDe"] != null)
                       {
                           int a = 1;
                           DataTable dtprevalue = new DataTable();
                           dtprevalue = (DataTable)Session["supplierattmtfileDe"];
                           for (int k = 0; dtprevalue.Rows.Count > k; k++)
                           {
                               DataRow dr1 = dtac.NewRow();
                               dr1["Attachid"] = dtac.Rows.Count + 1;
                               dr1["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                               dr1["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                               dr1["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                               dr1["flag"] = 1;
                               dtac.Rows.Add(dr1);
                               a++;
                           }

                             }
                             Session["supplierattmtfileDe"] = dtac;
                         }
                     }
                   

               }
               catch (Exception ex)
               {
                   objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                   message = string.Format("File upload failed: {0}", ex.Message);
               } 
           }
           return Json(new { isUploaded = isUploaded, message = message }, "text/html");
       }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
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

        #endregion
    }

    }

