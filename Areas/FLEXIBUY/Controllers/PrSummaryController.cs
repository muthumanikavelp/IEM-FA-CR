using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;
//using IEM.Areas.FLEXIBUY.Helpers;
//using IEM.Filters;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PrSummaryController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objuid = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        // GET: /PrSummary/
        private IRepositoryKIR objrep;


        public PrSummaryController()
            : this(new prsummodel())

        { }
        public PrSummaryController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        [HttpGet]
        public ActionResult PrSummary1()
        {
            Session["editprotemp"] = null;
            Session["cloneprotemp"] = null;
            Session["PRHeaderGid"] = null;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                Session["prsummary1"] = null;
                Session["rblProductService"] = null;
                Session["AccessToken1"] = null;
                Session["AccessTokenheader1"] = null;
                obj = objrep.GetPrSumry();

                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName");
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");

                // Session["prsummary"] = obj;

                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        //[HttpGet]
        //public ActionResult PrSummary1(string docnum = null)
        //{
        //    Session["editprotemp"] = null;
        //    Session["cloneprotemp"] = null;
        //    PrSumEntity obj = new PrSumEntity();
        //    Session["prsummary1"] = null;
        //    Session["rblProductService"] = null;
        //    obj = objrep.GetPrSumry();
        //    if (docnum != null)
        //    {
        //        if ((string.IsNullOrEmpty(docnum)) == false)
        //        {
        //            ViewBag.txtprrefno = docnum;
        //            obj = objrep.CheckExistancyPrRefNo(docnum);
        //            obj.lstprSum = obj.lstprSum.Where(x => docnum == null || (x.prRefNo.ToUpper().Contains(docnum.ToUpper()))).ToList();
        //            if (obj.lstprSum.Count == 0)
        //            {
        //                ViewBag.Message = "No records found";
        //            }
        //        }
        //    }
        //    obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
        //    obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName");
        //    obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
        //    obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");

        //    // Session["prsummary"] = obj;

        //    return View(obj);
        //}

        [HttpPost]
        public ActionResult PrSummary1(string txtprrefno, string txtprdate, string command, string dropRequestfor, string dropStatus, string dropBranch)
        {
            PrSumEntity obj;
            obj = objrep.GetPrSumry();
            try
            {
                Session["PRHeaderGid"] = null;
                //Session["prsummary1"] = obj.lstprSum;
                if (command == "SEARCH")
                {
                    if ((string.IsNullOrEmpty(txtprrefno)) == false)
                    {

                        obj = objrep.CheckExistancyPrRefNo(txtprrefno);
                        obj.lstprSum = obj.lstprSum.Where(x => txtprrefno == null || (x.prRefNo.ToUpper().Contains(txtprrefno.ToUpper()))).ToList();
                        if (obj.lstprSum.Count == 0)
                        {
                            ViewBag.Message = "No records found";
                        }
                    }
                    if ((string.IsNullOrEmpty(txtprdate)) == false)
                    {
                        ViewBag.txtprdate = txtprdate;
                        obj.lstprSum = obj.lstprSum.Where(x => txtprdate == null || (x.prDate.Contains(txtprdate))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    if ((string.IsNullOrEmpty(dropRequestfor)) == false)
                    {
                        obj.requestForGid = Convert.ToInt32(dropRequestfor);

                        obj.requestForName = objrep.GetRequestforName(obj.requestForGid);

                        ViewBag.txtprreqfor = obj.requestForName;
                        obj.lstprSum = obj.lstprSum.Where(x => obj.requestForName == null || (x.prReqFor.Contains(obj.requestForName))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    if ((string.IsNullOrEmpty(dropStatus)) == false)
                    {
                        obj.statusGid = Convert.ToInt32(dropStatus);
                        obj.statusName = objrep.GetStatusName(obj.statusGid);
                        ViewBag.txtstatus = obj.statusName;
                        obj.lstprSum = obj.lstprSum.Where(x => obj.statusName == null || (x.prStatus == obj.statusName)).ToList();
                        //if (dropStatus == "2")
                        //{
                        //    for (int statusGid = 2; statusGid <= 4; statusGid++)
                        //    {
                        //        obj.statusGid = Convert.ToInt32(statusGid);


                        //        obj.statusName = "Inprocess";
                        //        obj.lstprSum = obj.lstprSum.Where(x => obj.statusName == null || (x.prStatus == obj.statusName)).ToList();
                        //        // (x.prStatus.Contains(obj.statusName))).ToList();
                        //    }
                        //}

                        // (x.prStatus.Contains(obj.statusName))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    if ((string.IsNullOrEmpty(dropBranch)) == false)
                    {
                        obj.branchGid = Convert.ToInt32(dropBranch);
                        obj.branchName = objrep.GetBranchName(obj.branchGid);
                        ViewBag.txtbranch = obj.branchName;
                        obj.lstprSum = obj.lstprSum.Where(x => obj.branchName == null ||
                            (x.prBranch.Contains(obj.branchName))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    //if ((string.IsNullOrEmpty(txtprrefno)) == true && (string.IsNullOrEmpty(txtprdate)) == true && (string.IsNullOrEmpty(dropRequestfor)) == true && (string.IsNullOrEmpty(dropStatus)) == true && (string.IsNullOrEmpty(dropBranch)) == true)
                    //{
                    //    ViewBag.Error = "Please fill search Criteria";
                    //}
                    if (obj.lstprSum.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                }
                else if (command == "refresh")
                {
                    obj = objrep.GetPrSumry();
                }
                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");
                //if (command != "SEARCH")
                //{
                //    if (Session["prsummary1"] != null)
                //    {
                //        obj.lstprSum = (List<PrsummaryModel>)Session["prsummary1"];
                //    }

                //}
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        public ActionResult ViewPRDetails(string id, PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();

            try
            {
                string ss = "View";
                if (id != null && id != "")
                {
                    pr.prGid = id;
                    obj = objrep.ViewPrDetails(pr, ss);
                }

                Session["rblProductService"] = obj.product.prodserv_Type;
                Session["rblBranchType"] = obj.prHead.prBranchType;
                Session["rblExpenses"] = obj.prHead.prExpense;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
            return View(obj);
        }


        public ActionResult DeletePRDetails(string id)
        {
            string ss = "View";
            PrSumEntity obj = new PrSumEntity();
            try
            {
                PrHeader pr = new PrHeader();

                if (id != null && id != "")
                {
                    pr.prGid = id;
                    obj = objrep.ViewPrDetails(pr, ss);
                }
                if (obj.product.prodserv_Type != null)
                {
                    Session["rblProductService"] = obj.product.prodserv_Type;
                }
                if (obj.prHead.prBranchType != null)
                {
                    Session["rblBranchType"] = obj.prHead.prBranchType;
                }
                if (obj.prHead.prExpense != null)
                {
                    Session["rblExpenses"] = obj.prHead.prExpense;
                }


                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        [HttpPost]
        public JsonResult DeletePRDetails1(string l)
        {
            string ss = "Delete";
            PrSumEntity obj = new PrSumEntity();
            try
            {
                PrHeader pr = new PrHeader();

                if (l != null && l != "")
                {
                    pr.prRefNo = l;
                    obj = objrep.ViewPrDetails(pr, ss);

                }

                //obj = objrep.GetPrSumry();

                //obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                //obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName");
                //obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                //obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");
                return Json(obj.lstproduct.Count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj.lstproduct.Count, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CancelPRDetails(string id)
        {
            string ss = "View";
            PrSumEntity obj = new PrSumEntity();
            try
            {
                PrHeader pr = new PrHeader();

                if (id != null)
                {
                    pr.prGid = id;
                    obj = objrep.ViewPrDetails(pr, ss);
                }
                if (obj.product.prodserv_Type != null)
                {
                    Session["rblProductService"] = obj.product.prodserv_Type;
                }
                if (obj.prHead.prBranchType != null)
                {
                    Session["rblBranchType"] = obj.prHead.prBranchType;
                }
                if (obj.prHead.prExpense != null)
                {
                    Session["rblExpenses"] = obj.prHead.prExpense;
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        [HttpPost]
        public JsonResult CancelPRDetails(PrHeader pr, string id = null)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                string ss = "Cancel";

                //if (l != null && l != "")
                //{
                obj = objrep.ViewPrDetails(pr, ss);
                //}

                obj = objrep.GetPrSumry();

                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName");
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }



        public PartialViewResult viewprattachment(string id)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                obj.attachment = objrep.getattachmentdetails(id, "PR");

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        public JsonResult Downloaded(PrSumEntity prattachmentmodel)
        {
            Session["downfile"] = prattachmentmodel.attachment1;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
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
        public FileResult Download(string ff)
        {

            string txt1 = Session["downfile"].ToString();
            string directory = HoldFileUploadUrlDSA() + txt1.ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            string fileName = "Download" + txt1.ToString();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }



        // [HttpPost]
        //// public async Task<JsonResult> UploadedFiles(string fname)
        // public virtual ActionResult UploadedFiles()
        // {

        //     try
        //     {
        //         string filename = "";
        //         //foreach (string file in Request.Files)
        //         //{
        //         //    var fileContent = Request.Files[file];

        //         //    if (fileContent != null && fileContent.ContentLength > 0)
        //         //    {

        //         //        if (fname != null && fname.Trim() != "")
        //         //        {
        //         //            filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
        //         //        }
        //         //        else
        //         //        {
        //         //            filename = objCmnFunctions.GetSequenceNo("PR");
        //         //        }



        //         //        var fileextension = Path.GetExtension(fileContent.FileName);
        //         //        var stream = fileContent.InputStream;
        //         //        var path = Path.Combine(@"C:/temp/", filename + fileextension);
        //         //        using (var fileStream = System.IO.File.Create(path))
        //         //        {
        //         //            stream.CopyTo(fileStream);
        //         //        }
        //         //        filename = filename + fileextension;
        //         //    }
        //         //}
        //         return View(filename);
        //     }
        //     catch (Exception)
        //     {
        //         Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //         return Json("Upload failed");
        //     }
        // }

        [HttpPost]
        public virtual ActionResult UploadedFilesNew()
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

            //DataTable dtac = new DataTable();
            //dtac.Columns.Add("Attachid");
            //dtac.Columns.Add("AttachName");
            //dtac.Columns.Add("AttachFileType");
            //dtac.Columns.Add("Attachlength");
            //dtac.Columns.Add("flag");
            //int j = 1;
            //HttpPostedFileBase myFile = Request.Files["MyFile"];
            //bool isUploaded = false;
            //string message = "File upload failed";

            //if (myFile != null && myFile.ContentLength != 0)
            //{
            //    string pathForSaving = HttpContext.Application["path"] as string; ;
            //    //if (this.CreateFolderIfNeeded(pathForSaving))
            //    //{

            //    try
            //    {

            //        //myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
            //        myFile.SaveAs(Path.Combine(@"C:\temp\", myFile.FileName));

            //        DataRow dr = dtac.NewRow();
            //        dr["Attachid"] = j.ToString();
            //        //   dr["AttachName"] = Request.Files[file].FileName.ToString();
            //        dr["AttachName"] = Path.GetFileName(Request.Files["MyFile"].FileName);
            //        dr["AttachFileType"] = Request.Files["MyFile"].ContentType.ToString();
            //        dr["Attachlength"] = Request.Files["MyFile"].ContentLength.ToString();
            //        dr["flag"] = 0;

            //        dtac.Rows.Add(dr);



            //        isUploaded = true;
            //        // message = "File uploaded successfully!";
            //        message = Path.GetFileName(Request.Files["MyFile"].FileName);
            //        if (Session["supplierattmtfileDe"] != null)
            //        {
            //            int a = 1;
            //            DataTable dtprevalue = new DataTable();
            //            dtprevalue = (DataTable)Session["supplierattmtfileDe"];
            //            for (int k = 0; dtprevalue.Rows.Count > k; k++)
            //            {
            //                DataRow dr1 = dtac.NewRow();
            //                dr1["Attachid"] = dtac.Rows.Count + 1;
            //                dr1["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
            //                dr1["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
            //                dr1["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
            //                dr1["flag"] = 1;
            //                dtac.Rows.Add(dr1);
            //                a++;
            //            }

            //        }
            //        Session["supplierattmtfileDe"] = dtac;
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
                HttpPostedFileBase myFile = Request.Files["MyFile"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string; ;
                    //if (this.CreateFolderIfNeeded(pathForSaving))
                    //{

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        // filename = filename.Substring(0, filename.LastIndexOf(".") + 0);
                        filename = objCmnFunctions.GetSequenceNo("PR");
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
                        message = "File uploaded successfully!";
                        //message = Path.GetFileName(Request.Files["boqfile"].FileName);
                        message = filename;

                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }

                }

            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        //[HttpPost]
        //public void UploadedFiles()
        //{
        //    try
        //    {
        //        string filename = "";

        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                filename = Path.GetFileNameWithoutExtension(fileContent.FileName);
        //                var fileextension = Path.GetExtension(fileContent.FileName);
        //                var stream = fileContent.InputStream;
        //                var path = Path.Combine(@"C:\temp\", filename + fileextension);
        //                using (var fileStream = System.IO.File.Create(path))
        //                {
        //                    stream.CopyTo(fileStream);
        //                }
        //                filename = filename + fileextension;
        //                Session["filename"] = filename;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}



        public PartialViewResult editprattachment(string id)
        {

            PrSumEntity obj = new PrSumEntity();

            Session["editattachment"] = null;
            Session["AccessTokenheader"] = null;

            PrSumEntity obj1 = new PrSumEntity();
            try
            {
                List<Attachment> lstattach = new List<Attachment>();



                obj1.attachment = objrep.getattachmentdetails(id, "PR");


                //for (int i = 0; i < lstattach.Count; i++)
                //{
                //    obj1 = new PrSumEntity();
                //    obj1.attachName = lstattach[i].fileName;
                //    obj1.attachmentDate = lstattach[i].attachedDate;
                //    obj1.attachmentDesc = lstattach[i].description;
                //    obj = objrep.PRAttachmentname(obj1);

                //}
                Session["prheader_AccessTokenheader"] = null;
                Session["prheader_AccessToken"] = null;
                Session["editattachmentNew"] = null;
                Session["editattachmentNew"] = obj1.attachment;

                obj1.attachmentDate = DateTime.Now.ToShortDateString();
                //  return PartialView("editprattachmentnew", obj1.attachment);
                return PartialView(obj1);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj1);
            }
        }

        [HttpPost]
        public JsonResult editPRattachgrid(PrSumEntity attachmodel)
        {
            PrSumEntity attachment1 = new PrSumEntity();
            try
            {
                DataTable dt = new DataTable();




                if (ModelState.IsValid)
                {
                    attachment1 = objrep.PRAttachmentname(attachmodel);


                    if (Session["editattachmentNew"] != null)
                    {


                        List<Attachment> oldatt = new List<Attachment>();
                        oldatt = (List<Attachment>)Session["editattachmentNew"];
                        if (oldatt.Count > 0)
                        {

                            attachment1.attachment.AddRange(oldatt);
                            Session["editattachmentNew"] = null;
                        }




                    }
                    Session["editattachment"] = attachment1;
                }



                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public PartialViewResult editPRattachgrid()
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {


                obj = (PrSumEntity)Session["editattachment"];

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        public JsonResult EditDeleteAttachment(PrSumEntity obj)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            PrSumEntity attachment1 = new PrSumEntity();
            try
            {
                PrSumEntity rr = new PrSumEntity();

                rr = (PrSumEntity)Session["editattachment"];



                if (Session["editattachment"] != null)
                {
                    int rowcount = 1;
                    dt.Columns.Add("Srno");
                    //dt.Columns.Add("Filename");Documnet_Name
                    //dt.Columns.Add("AttachmentDate");Attachment_Date
                    //dt.Columns.Add("AttachmentDescription");Document_des

                    dt.Columns.Add("Documnet_Name");
                    dt.Columns.Add("Attachment_Date");
                    dt.Columns.Add("Document_des");

                    for (int i = 0; i < rr.attachment.Count; i++)
                    {
                        dt.Rows.Add(rowcount, rr.attachment[i].fileName, rr.attachment[i].attachedDate, rr.attachment[i].description);
                        rowcount = rowcount + 1;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Documnet_Name"].ToString() == obj.attachment1)
                        {
                            dt.Rows.RemoveAt(i);

                        }
                        dt.AcceptChanges();
                    }

                    int rowcount1 = 1;
                    dt1.Columns.Add("Srno");
                    dt1.Columns.Add("Documnet_Name");

                    if (Session["Removedrows"] == null)
                    {
                        dt1.Rows.Add(rowcount1, obj.attachment1);

                    }
                    else
                    {
                        dt1 = (DataTable)Session["Removedrows"];
                        rowcount1 = rowcount1 + 1;
                        dt1.Rows.Add(rowcount1, obj.attachment1);

                    }

                    Session["Removedrows"] = dt1;

                    List<Attachment> lst = objrep.EditPRAttachmentList(dt);
                    attachment1.attachment = lst;

                    Session["editattachment"] = attachment1;

                }

                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult EditPRDetails(string id, string ss)
        {
            Session["prgid"] = id;
            Session["editservtemp"] = null;
            Session["editattachment"] = null;
            Session["Removedrowsproduct"] = null;
            Session["Removedrowsservice"] = null;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempserv = new DataTable();
                PrHeader pr = new PrHeader();

                pr.prGid = id;

                ss = "View";

                obj = objrep.ViewPrDetails(pr, ss);


                Session["editdetails"] = obj;

                if (obj.product.prodserv_Type != null)
                {
                    Session["rblProductService"] = obj.product.prodserv_Type;
                }
                if (obj.prHead.prBranchType != null)
                {
                    Session["rblBranchType"] = obj.prHead.prBranchType;
                }
                if (obj.prHead.prExpense != null)
                {
                    Session["rblExpenses"] = obj.prHead.prExpense;
                }

                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName", obj.branchGid);
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName", obj.fcccGid);
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName", obj.requestForGid);
                obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName", obj.statusGid);

                if (obj.product.prodserv_Type == "P")
                {
                    if (Session["editprotemp"] == null)
                    {
                        temppro.Columns.Add("Srno");
                        temppro.Columns.Add("product_Group");
                        temppro.Columns.Add("product_Code");
                        temppro.Columns.Add("product_Name");
                        temppro.Columns.Add("product_Description");
                        temppro.Columns.Add("product_Qty");
                        temppro.Columns.Add("product_Unit");
                        temppro.Columns.Add("product_gid");
                        for (int i = 0; i < obj.lstproduct.Count; i++)
                        {
                            int rowcount = i + 1;


                            temppro.Rows.Add(obj.lstproduct[i].srNo, obj.lstproduct[i].prodservgrp_Gid, obj.lstproduct[i].product_Code, obj.lstproduct[i].product_Name, obj.lstproduct[i].product_Description, obj.lstproduct[i].product_Qty, obj.lstproduct[i].productUnit_Gid, obj.lstproduct[i].product_gid);


                        }
                        Session["editprotemp"] = temppro;

                    }
                }
                if (obj.product.prodserv_Type == "S")
                {
                    if (Session["editservtemp"] == null)
                    {
                        tempserv.Columns.Add("Srno");
                        tempserv.Columns.Add("service_Group");
                        tempserv.Columns.Add("service_Code");
                        tempserv.Columns.Add("service_Name");
                        tempserv.Columns.Add("service_Description");
                        tempserv.Columns.Add("product_gid");

                        for (int i = 0; i < obj.lstproduct.Count; i++)
                        {
                            string productid = obj.lstproduct[i].prodservgrp_Gid;
                            string code = obj.lstproduct[i].service_Code;
                            string name = obj.lstproduct[i].service_Name;
                            string desc = obj.lstproduct[i].service_Description;
                            string product = obj.lstproduct[i].product_gid;
                            int rowcount = i + 1;


                            tempserv.Rows.Add(rowcount, obj.lstproduct[i].prodservgrp_Gid, obj.lstproduct[i].service_Code, obj.lstproduct[i].service_Name, obj.lstproduct[i].service_Description, obj.lstproduct[i].product_gid);


                        }
                        Session["editservtemp"] = tempserv;

                    }
                }


                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        [HttpPost]
        public PartialViewResult EditPro(string id, Product id1)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                temppro = (DataTable)Session["editprotemp"];

                for (int i = 0; i < temppro.Rows.Count; i++)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = obj.lstproduct[i].prodservgrp_Gid;
                    obj.selectedValue1 = obj.lstproduct[i].productUnit_Gid;

                }


                obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList("Product"), "productGroupGid", "productGroupName");
                obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }
        [HttpPost]
        public JsonResult editsavepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";

                if (Session["editprotemp"] == null && Session["cloneprotemp"] == null)
                {
                    int rowcount = 1;

                    temppro.Columns.Add("Srno");
                    temppro.Columns.Add("product_Group");
                    temppro.Columns.Add("product_Code");
                    temppro.Columns.Add("product_Name");
                    temppro.Columns.Add("product_Description");
                    temppro.Columns.Add("product_Qty");
                    temppro.Columns.Add("product_Unit");
                    temppro.Columns.Add("product_gid");
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid, product.product_gid);
                    Session["editprotemp"] = temppro;
                    Session["cloneprotemp"] = temppro;

                }
                else
                {
                    if (Session["cloneprotemp"] == null)
                    {
                        temppro = (DataTable)Session["editprotemp"];
                    }
                    else
                    {
                        temppro = (DataTable)Session["cloneprotemp"];
                    }
                    int rowcount = temppro.Rows.Count + 1;
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid, product.product_gid);
                    Session["editprotemp"] = temppro;
                    Session["cloneprotemp"] = temppro;
                }

                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");


                }
                Session["editdetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult editupdatepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";
                if (Session["editprotemp"] != null)
                {
                    temppro = (DataTable)Session["editprotemp"];

                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_Code"].ToString() == product.product_Code.ToString().Trim())
                        {
                            // temppro.Rows[i]["Srno"] = product.srNo;
                            temppro.Rows[i]["product_Unit"] = product.productUnit_Gid;
                            temppro.Rows[i]["product_Qty"] = product.product_Qty;

                        }
                    }

                }


                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                    obj.lstproduct = objrep.GetProduct(temppro);
                }
                Session["editdetails"] = obj;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult editdeletepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                PrSumEntity pr = new PrSumEntity();

                DataTable temppro = new DataTable();
                DataTable dt1 = new DataTable();

                string tax = "Product";
                if (Session["editprotemp"] != null)
                {
                    temppro = (DataTable)Session["editprotemp"];

                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_Code"].ToString() == product.product_Code.ToString().Trim())
                        {
                            temppro.Rows.RemoveAt(i);
                        }
                        temppro.AcceptChanges();
                    }

                }

                pr = (PrSumEntity)Session["editdetails"];

                int rowcount1 = 0, detailgid = 0;
                dt1.Columns.Add("Srno");
                dt1.Columns.Add("detail_gid");

                for (int i = 0; i < pr.lstproduct.Count; i++)
                {
                    if (pr.lstproduct[i].product_Code == product.product_Code.ToString().Trim())
                    {
                        detailgid = pr.lstproduct[i].srNo;
                        // detailgid = product.srNo;
                    }
                }

                if (Session["Removedrowsproduct"] == null)
                {
                    rowcount1 = 1;
                    dt1.Rows.Add(rowcount1, detailgid);

                }
                else
                {
                    dt1 = (DataTable)Session["Removedrowsproduct"];

                    rowcount1 = rowcount1 + 1;

                    dt1.Rows.Add(rowcount1, detailgid);

                }

                Session["Removedrowsproduct"] = dt1;


                if (temppro.Rows.Count > 0)
                {

                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                    obj.lstproduct = objrep.GetProduct(temppro);
                }

                Session["editdetails"] = obj;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public PartialViewResult EditPro()
        {
            PrSumEntity objda = new PrSumEntity();
            try
            {
                objda = (PrSumEntity)Session["editdetails"];
                return PartialView(objda);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objda);
            }
        }

        [HttpPost]
        public PartialViewResult EditSer(string id, Product id1)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                tempserv = (DataTable)Session["editservtemp"];

                for (int i = 0; i < tempserv.Rows.Count; i++)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.selectedValue = obj.lstproduct[i].prodservgrp_Gid;
                }

                obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList("Service"), "productGroupGid", "productGroupName");


                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        [HttpPost]
        public JsonResult editdeletese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                PrSumEntity pr = new PrSumEntity();
                DataTable dt1 = new DataTable();

                string tax = "Service";
                if (Session["editservtemp"] != null)
                {
                    tempserv = (DataTable)Session["editservtemp"];

                    for (int i = 0; i < tempserv.Rows.Count; i++)
                    {
                        if (tempserv.Rows[i]["service_Code"].ToString() == service.service_Code.ToString().Trim())
                        {
                            tempserv.Rows.RemoveAt(i);
                        }
                        tempserv.AcceptChanges();
                    }
                    pr = (PrSumEntity)Session["editdetails"];

                    int rowcount1 = 0, detailgid = 0;
                    dt1.Columns.Add("Srno");
                    dt1.Columns.Add("detail_gid");

                    for (int i = 0; i < pr.lstproduct.Count; i++)
                    {
                        if (pr.lstproduct[i].service_Code == service.service_Code.ToString().Trim())
                        {
                            detailgid = pr.lstproduct[i].srNo;
                        }
                    }

                    if (Session["Removedrowsservice"] == null)
                    {
                        rowcount1 = 1;
                        dt1.Rows.Add(rowcount1, detailgid);

                    }
                    else
                    {
                        dt1 = (DataTable)Session["Removedrowsservice"];

                        rowcount1 = rowcount1 + 1;

                        dt1.Rows.Add(rowcount1, detailgid);

                    }

                    Session["Removedrowsservice"] = dt1;
                }


                if (tempserv.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                    obj.lstproduct = objrep.GetProduct(tempserv);
                }

                Session["editsedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult editsavese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                string tax = "Service";

                if (Session["editservtemp"] == null)
                {
                    int rowcount = 1;

                    tempserv.Columns.Add("Srno");
                    tempserv.Columns.Add("service_Group");
                    tempserv.Columns.Add("service_Code");
                    tempserv.Columns.Add("service_Name");
                    tempserv.Columns.Add("service_Description");
                    tempserv.Columns.Add("product_gid");
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);
                    Session["editservtemp"] = tempserv;

                }
                else
                {
                    tempserv = (DataTable)Session["editservtemp"];
                    int rowcount = tempserv.Rows.Count + 1;
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);

                }

                if (tempserv.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.selectedValue = service.prodservgrp_Gid;


                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                }

                Session["editsedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);

                //  return RedirectToAction("../PrSummary/EditSer", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult EditSer()
        {
            PrSumEntity objda = new PrSumEntity();
            try
            {
                objda = (PrSumEntity)Session["editsedetails"];
                return PartialView(objda);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objda);
            }
        }


        [HttpPost]
        public JsonResult saveeditprraiser(PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();
                DataTable dt = new DataTable();
                string Result = string.Empty;
                int headegid = 0;
                if (Session["editprotemp"] != null)
                {
                    temppro = (DataTable)Session["editprotemp"];

                    lstpro = objrep.GetProduct(temppro);
                    obj = objrep.EditPrheader(pr);
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.EditPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);
                    Result = objrep.Insertqueue(headegid, pr.requestForName, pr);
                }
                if (Session["editservtemp"] != null)
                {
                    tempser = (DataTable)Session["editservtemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.EditPrheader(pr);
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.EditPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);
                    Result = objrep.Insertqueue(headegid, pr.requestForName, pr);

                }
                //if (Session["editattachment"] != null)
                //{
                //    PrSumEntity obj1 = new PrSumEntity();
                //    obj1 = (PrSumEntity)Session["editattachment"];

                //    obj = objrep.InsertAttachment(pr.prRefNo, "PR", obj1.attachment);

                //}
                //if (Session["Removedrows"] != null)
                //{

                //    dt = (DataTable)Session["Removedrows"];
                //    obj = objrep.DeleteAttachment(dt);

                //}

                if (Session["Removedrowsproduct"] != null)
                {

                    dt = (DataTable)Session["Removedrowsproduct"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }
                if (Session["Removedrowsservice"] != null)
                {
                    dt = (DataTable)Session["Removedrowsservice"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }
                ForMailController mailsender = new ForMailController();

                string mail = Session["PRqueuegid"].ToString();
                mailsender.sendusermail("FB", "PR", mail, "S", "0");
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult saveeditprraiser1(PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();
                DataTable dt = new DataTable();
                string Result = string.Empty;
                int headegid = 0;
                if (Session["editprotemp"] != null)
                {
                    temppro = (DataTable)Session["editprotemp"];

                    lstpro = objrep.GetProduct(temppro);
                    obj = objrep.EditPrheader1(pr);
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.EditPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);
                    //Result = objrep.Insertqueue(headegid, pr.requestForName);
                }
                if (Session["editservtemp"] != null)
                {
                    tempser = (DataTable)Session["editservtemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.EditPrheader1(pr);
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.EditPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);
                    // Result = objrep.Insertqueue(headegid, pr.requestForName);
                }
                //if (Session["editattachment"] != null)
                //{
                //    PrSumEntity obj1 = new PrSumEntity();
                //    obj1 = (PrSumEntity)Session["editattachment"];

                //    obj = objrep.InsertAttachment(pr.prRefNo, "PR", obj1.attachment);

                //}
                //if (Session["Removedrows"] != null)
                //{

                //    dt = (DataTable)Session["Removedrows"];
                //    obj = objrep.DeleteAttachment(dt);

                //}

                if (Session["Removedrowsproduct"] != null)
                {

                    dt = (DataTable)Session["Removedrowsproduct"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }
                if (Session["Removedrowsservice"] != null)
                {
                    dt = (DataTable)Session["Removedrowsservice"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult cloneprattachment(string id)
        {

            PrSumEntity obj = new PrSumEntity();
            try
            {
                List<Attachment> lstattach = new List<Attachment>();
                //Session["cloneattachment"] = null;
                Session["AccessTokenheader"] = null;
                if (Session["cloneattachment"] != null)
                {
                    obj = (PrSumEntity)Session["cloneattachment"];
                }
                else
                {
                    obj.attachment = lstattach;
                }





                //lstattach = objrep.getattachmentdetails(id, "PR");

                //for (int i = 0; i < lstattach.Count; i++)
                //{
                //    obj1 = new PrSumEntity();
                //    obj1.attachName = lstattach[i].fileName;
                //    obj1.attachmentDate = lstattach[i].attachedDate;
                //    obj1.attachmentDesc = lstattach[i].description;
                //    obj = objrep.PRAttachmentname(obj1);

                //}
                // Session["cloneattachment"] = obj;
                obj.attachmentDate = DateTime.Now.ToShortDateString();
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        [HttpPost]
        public JsonResult clonePRattachgrid(PrSumEntity attachmodel)
        {
            try
            {
                DataTable dt = new DataTable();
                PrSumEntity attachment1 = new PrSumEntity();

                try
                {

                    if (ModelState.IsValid)
                    {
                        attachment1 = objrep.PRAttachmentname(attachmodel);
                        Session["cloneattachment"] = attachment1;
                    }



                    return Json(attachment1, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    return Json(attachment1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult clonePRattachgrid()
        {
            try
            {
                PrSumEntity obj = new PrSumEntity();
                try
                {
                    obj = (PrSumEntity)Session["cloneattachment"];

                    return PartialView(obj);
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    return PartialView(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult CloneDeleteAttachment(PrSumEntity obj)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            PrSumEntity attachment1 = new PrSumEntity();
            try
            {
                PrSumEntity rr = new PrSumEntity();

                rr = (PrSumEntity)Session["cloneattachment"];


                if (Session["cloneattachment"] != null)
                {
                    int rowcount = 1;
                    dt.Columns.Add("Srno");
                    dt.Columns.Add("Filename");
                    dt.Columns.Add("AttachmentDate");
                    dt.Columns.Add("AttachmentDescription");
                    for (int i = 0; i < rr.attachment.Count; i++)
                    {
                        dt.Rows.Add(rowcount, rr.attachment[i].fileName, rr.attachment[i].attachedDate, rr.attachment[i].description);
                        rowcount = rowcount + 1;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Filename"].ToString() == obj.attachment1)
                        {
                            dt.Rows.RemoveAt(i);

                        }
                    }

                    //int rowcount1 = 1;
                    //dt1.Columns.Add("Srno");
                    //dt1.Columns.Add("Filename");

                    //if (Session["Removedrowsclone"] == null)
                    //{
                    //    dt1.Rows.Add(rowcount1, obj.attachment1);

                    //}
                    //else
                    //{
                    //    dt1 = (DataTable)Session["Removedrowsclone"];
                    //    rowcount1 = rowcount1 + 1;
                    //    dt1.Rows.Add(rowcount1, obj.attachment1);

                    //}

                    //Session["Removedrowsclone"] = dt1;

                    List<Attachment> lst = objrep.EditPRAttachmentList(dt);
                    attachment1.attachment = lst;

                    Session["cloneattachment"] = attachment1;

                }

                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ClonePRDetails(string id, string ss)
        {
            Session["prgid"] = id;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempserv = new DataTable();
                PrHeader pr = new PrHeader();
                ss = "Clone";
                pr.prGid = id;


                obj = objrep.ViewPrDetails(pr, ss);
                if (obj.product != null)
                {
                    if (obj.product.prodserv_Type != null)
                    {
                        Session["rblProductService"] = obj.product.prodserv_Type;
                    }
                    if (obj.prHead.prBranchType != null)
                    {
                        Session["rblBranchType"] = obj.prHead.prBranchType;
                    }
                    if (obj.prHead.prExpense != null)
                    {
                        Session["rblExpenses"] = obj.prHead.prExpense;
                    }
                }

                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName", obj.branchGid);
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName", obj.fcccGid);
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName", obj.requestForGid);
                obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName", obj.statusGid);

                if (obj.product.prodserv_Type == "P")
                {
                    if (Session["cloneprotemp"] == null)
                    {
                        temppro.Columns.Add("Srno");
                        temppro.Columns.Add("product_Group");
                        temppro.Columns.Add("product_Code");
                        temppro.Columns.Add("product_Name");
                        temppro.Columns.Add("product_Description");
                        temppro.Columns.Add("product_Qty");
                        temppro.Columns.Add("product_Unit");
                        temppro.Columns.Add("product_gid");
                        for (int i = 0; i < obj.lstproduct.Count; i++)
                        {
                            int rowcount = i + 1;


                            temppro.Rows.Add(rowcount, obj.lstproduct[i].prodservgrp_Gid, obj.lstproduct[i].product_Code, obj.lstproduct[i].product_Name, obj.lstproduct[i].product_Description, obj.lstproduct[i].product_Qty, obj.lstproduct[i].productUnit_Gid, obj.lstproduct[i].product_gid);


                        }
                        Session["cloneprotemp"] = temppro;
                    }

                }

                if (obj.product.prodserv_Type == "S")
                {
                    if (Session["cloneservtemp"] == null)
                    {
                        tempserv.Columns.Add("Srno");
                        tempserv.Columns.Add("service_Group");
                        tempserv.Columns.Add("service_Code");
                        tempserv.Columns.Add("service_Name");
                        tempserv.Columns.Add("service_Description");
                        tempserv.Columns.Add("product_gid");
                        for (int i = 0; i < obj.lstproduct.Count; i++)
                        {
                            int rowcount = i + 1;


                            tempserv.Rows.Add(rowcount, obj.lstproduct[i].prodservgrp_Gid, obj.lstproduct[i].service_Code, obj.lstproduct[i].service_Name, obj.lstproduct[i].service_Description, obj.lstproduct[i].product_gid);


                        }
                        Session["cloneservtemp"] = tempserv;

                    }
                }

                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }



        [HttpPost]
        public PartialViewResult ClonePro(string id, Product id1)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                temppro = (DataTable)Session["cloneprotemp"];

                for (int i = 0; i < temppro.Rows.Count; i++)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = obj.lstproduct[i].prodservgrp_Gid;
                    obj.selectedValue1 = obj.lstproduct[i].productUnit_Gid;

                }

                obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList("Product"), "productGroupGid", "productGroupName");
                obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        [HttpPost]
        public JsonResult clonesavepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";

                if (Session["cloneprotemp"] == null)
                {
                    int rowcount = 1;

                    temppro.Columns.Add("Srno");
                    temppro.Columns.Add("product_Group");
                    temppro.Columns.Add("product_Code");
                    temppro.Columns.Add("product_Name");
                    temppro.Columns.Add("product_Description");
                    temppro.Columns.Add("product_Qty");
                    temppro.Columns.Add("product_Unit");
                    temppro.Columns.Add("product_gid");
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid, product.product_gid);
                    Session["cloneprotemp"] = temppro;

                }
                else
                {
                    temppro = (DataTable)Session["cloneprotemp"];
                    int rowcount = temppro.Rows.Count + 1;
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid, product.product_gid);

                }

                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");


                }
                Session["clonedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult cloneupdatepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";
                if (Session["cloneprotemp"] != null)
                {
                    temppro = (DataTable)Session["cloneprotemp"];

                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_Code"].ToString() == product.product_Code.ToString().Trim())
                        {
                            temppro.Rows[i]["product_Unit"] = product.productUnit_Gid;
                            temppro.Rows[i]["product_Qty"] = product.product_Qty;

                        }
                    }

                }


                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                    obj.lstproduct = objrep.GetProduct(temppro);
                }

                Session["clonedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            //return PartialView("ClonePro", obj);
        }

        [HttpPost]
        public JsonResult clonedeletepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";
                if (Session["cloneprotemp"] != null)
                {
                    temppro = (DataTable)Session["cloneprotemp"];
                    for (int j = 0; j < temppro.Rows.Count; j++)
                    {
                        temppro.Rows[j]["Srno"] = j + 1;
                    }
                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_Code"].ToString() == product.product_Code.ToString().Trim() && temppro.Rows[i]["Srno"].ToString() == product.srNo.ToString())
                        {
                            temppro.Rows.RemoveAt(i);
                        }
                    }

                }


                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                    obj.lstproduct = objrep.GetProduct(temppro);
                }
                Session["clonedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult ClonePro()
        {
            PrSumEntity objda = new PrSumEntity();
            try
            {
                objda = (PrSumEntity)Session["clonedetails"];
                return PartialView(objda);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objda);
            }
        }



        [HttpPost]
        public PartialViewResult CloneSer(string id, Product id1)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                tempserv = (DataTable)Session["cloneservtemp"];

                for (int i = 0; i < tempserv.Rows.Count; i++)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.selectedValue = obj.lstproduct[i].prodservgrp_Gid;
                }

                obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList("Service"), "productGroupGid", "productGroupName");


                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        [HttpPost]
        public JsonResult clonedeletese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                string tax = "Service";
                if (Session["cloneservtemp"] != null)
                {
                    tempserv = (DataTable)Session["cloneservtemp"];

                    for (int i = 0; i < tempserv.Rows.Count; i++)
                    {
                        if (tempserv.Rows[i]["service_Code"].ToString() == service.service_Code.ToString().Trim())
                        {
                            tempserv.Rows.RemoveAt(i);
                        }
                    }

                }


                if (tempserv.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                    obj.lstproduct = objrep.GetProduct(tempserv);
                }

                Session["clonesedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult clonesavese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                string tax = "Service";

                if (Session["cloneservtemp"] == null)
                {
                    int rowcount = 1;

                    tempserv.Columns.Add("Srno");
                    tempserv.Columns.Add("service_Group");
                    tempserv.Columns.Add("service_Code");
                    tempserv.Columns.Add("service_Name");
                    tempserv.Columns.Add("service_Description");
                    tempserv.Columns.Add("product_gid");
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);
                    Session["cloneservtemp"] = tempserv;

                }
                else
                {
                    tempserv = (DataTable)Session["cloneservtemp"];
                    int rowcount = tempserv.Rows.Count + 1;
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);

                }

                if (tempserv.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.selectedValue = service.prodservgrp_Gid;


                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                }
                Session["clonesedetails"] = obj;

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public PartialViewResult CloneSer()
        {
            PrSumEntity objda = new PrSumEntity();
            try
            {
                objda = (PrSumEntity)Session["clonesedetails"];
                return PartialView(objda);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objda);
            }
        }

        [HttpPost]
        public JsonResult savecloneprraiser(PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();

                if (Session["cloneprotemp"] != null)
                {
                    temppro = (DataTable)Session["cloneprotemp"];

                    lstpro = objrep.GetProduct(temppro);
                    obj = objrep.InsertPrheader(pr, "Save");
                    obj = objrep.InsertPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);

                }
                if (Session["cloneservtemp"] != null)
                {
                    tempser = (DataTable)Session["cloneservtemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.InsertPrheader(pr, "Save");
                    obj = objrep.InsertPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);
                    //line insert kulothungan 29-07-2015
                    Session["cloneservtemp"] = null;
                }
                if (Session["cloneattachment"] != null)
                {
                    PrSumEntity obj1 = new PrSumEntity();
                    obj1 = (PrSumEntity)Session["cloneattachment"];

                    obj = objrep.InsertAttachment(pr.prRefNo, "PR", obj1.attachment);

                }
                //if (Session["Removedrowsclone"] != null)
                //{
                //    DataTable dt = new DataTable();
                //    dt = (DataTable)Session["Removedrowsclone"];
                //    obj = objrep.DeleteAttachment(dt);

                //}

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult GetProductGroup1()
        {
            PrSumEntity obj = new PrSumEntity();

            return Json(objrep.GetProductGroupList("Product"), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetServiceGroup1()
        {
            PrSumEntity obj = new PrSumEntity();

            return Json(objrep.GetProductGroupList("Service"), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult getuom1()
        {
            PrSumEntity obj = new PrSumEntity();

            return Json(objrep.GetUomList(), JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult Attachments(string id)
        {
            PrSumEntity obj = new PrSumEntity();
            return PartialView(obj);
        }

        [HttpGet]
        public PartialViewResult BoqAttachgrid() 
        {
            PrSumEntity obj = new PrSumEntity();
            return PartialView(obj);
        }

        //[HttpPost]
        //public JsonResult Getrowcount()
        //{
        //    PrSumEntity obj = new PrSumEntity();


        //    return Json(procount, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult EditPRAttachment(PrSumEntity objAttachment)  
        {
            try
            {
                objrep.InsertPRAttachmentEditNew(objAttachment);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
           

        }
        [HttpPost]
        public JsonResult CheckFileExists(string id)
        {
            string result = "0";
            try
            {
                var localpath = Path.Combine(HoldFileUploadUrlDSA(), id);
                if (System.IO.File.Exists(localpath))
                {
                    result = "1";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CheckFileExists1(string id) 
        {
            string result = "0";
            try
            {
                var localpath = Path.Combine(HoldFileUploadUrlDSA(), id);
                if (System.IO.File.Exists(localpath))
                {
                    result = "1";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult DownloadAttachedFilesNew(string filename="") 
        {

            try
            {
                string directory = Path.Combine(HoldFileUploadUrlDSA(), filename);
              //  byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                return File(directory, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeletePREditAttachment(string AttachmentGid)  
        {
            try
            {
                int id = Convert.ToInt32(AttachmentGid); 
                objrep.DeletePREditAttachment(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
         
        }


        public ActionResult ViewPRDetailsRT(string id) 
        {
            PrSumEntity obj = new PrSumEntity();

            try
            {
                PrHeader pr = new PrHeader();
                string ss = "View";
                if (id != null && id != "")
                {
                    pr.prGid = id;
                    obj = objrep.ViewPrDetails(pr, ss);
                }

                Session["rblProductService"] = obj.product.prodserv_Type;
                Session["rblBranchType"] = obj.prHead.prBranchType;
                Session["rblExpenses"] = obj.prHead.prExpense;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
            return View(obj);
        }
    }



}

