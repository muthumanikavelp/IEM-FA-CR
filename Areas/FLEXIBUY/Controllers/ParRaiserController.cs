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
    public class ParRaiserController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private IrepositoryAn objRep;
        public ParRaiserController()
            : this(new CbfSumModel())
        {

        }
        public ParRaiserController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }

        public ActionResult ParRaiserIndex()
        {
            Session["ViewBag"] = null;
            Session["att_gid"] = null;
            Session["cbfid"] = null;
            Session["attach_date_val"] = null;
            Session["attcnt"] = null;
            Session["Approval"] = null;
            Session["parcbfdetails"] = null;
            Session["sno"] = null;
            Session["sno1"] = null;
            Session["attachment"] = null;
            Session["attachmentdetails"] = null;
            Session["AccessTokenheader"] = null;
            Session["AccessToken"] = null;
            Session["AccessTokenheader1"] = null;
            Session["AccessToken1"] = null;
            Session["AccessTokenheaderinline"] = null;
            Session["AccessTokeninline"] = null;
            CbfSumEntity objPardetails = new CbfSumEntity();
            objRep.deletetempstoredattach("932600");
            return View(objPardetails);
        }
        [HttpGet]
        public PartialViewResult ParDetailsForPar()
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult ParDetailsForParPost()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetRequestfor()
        {
            return Json(objRep.GetList1(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult ParSave(ParDetails pardetails)
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
                    objDt.Columns.Add("Expense", typeof(string));
                    objDt.Columns.Add("department", typeof(string));
                    objDt.Columns.Add("bugdect", typeof(string));
                    objDt.Columns.Add("Description", typeof(string));
                    objDt.Columns.Add("fromyear", typeof(string));
                    objDt.Columns.Add("toyear", typeof(string));
                    objDt.Columns.Add("Amount", typeof(decimal));
                    objDt.Columns.Add("remarks", typeof(string));
                    objDt.Columns.Add("BOQ", typeof(int));

                }
                if (pardetails.ParDetails_Gid.ToString() == null || pardetails.ParDetails_Gid == 0)
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
                        int cnmt = Convert.ToInt32(Session["attcont_vl"].ToString());
                        cnmt++;
                        Session["attcont_vl"] = Convert.ToString(cnmt);
                        pardetails.att_identify = Session["attcont_vl"].ToString();
                        ViewBag.id_valatt = pardetails.att_identify;
                    }
                    //  obj_getvalues.att_identify = Convert.ToString(cntiden);
                    Session["sno"] = sno;
                    objDt.Rows.Add(sno, pardetails.Expense, pardetails.Department, pardetails.Budgeted, pardetails.Description
                        , pardetails.FromYear, pardetails.ToYear,
                        pardetails.ParAmount, pardetails.Remarks, pardetails.att_identify);
                }
                else
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["sno"].ToString() == pardetails.ParDetails_Gid.ToString())
                        {
                            // Dt.Rows[i]["prdetails_gid"] =  pardetails.productgroupid;
                            objDt.Rows[i]["Expense"] = pardetails.Expense;
                            objDt.Rows[i]["department"] = pardetails.Department;
                            objDt.Rows[i]["bugdect"] = pardetails.Budgeted;
                            objDt.Rows[i]["Description"] = pardetails.Description;
                            objDt.Rows[i]["fromyear"] = pardetails.FromYear;
                            objDt.Rows[i]["toyear"] = pardetails.ToYear;
                            objDt.Rows[i]["Amount"] = pardetails.ParAmount;
                            objDt.Rows[i]["remarks"] = pardetails.Remarks;

                        }
                    }

                }

                Session["parcbfdetails"] = objDt;
                objcbf = objRep.GetParDetailsSave(objDt);

              //  objcbf.selectedValue = pardetails.Department.ToString();
                objcbf.requestFor = new SelectList(objRep.GetList1(), "requeestForGid", "requestForName");
                objcbf.capexlist = new SelectList(objRep.GetCapex(), "capexId", "capexname");
                objcbf.budjectedlist = new SelectList(objRep.GetBudjected(), "budjectedid", "budjectedname");
                if (objcbf.lListParDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("ParDetailsForPar", objcbf);

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
        [HttpGet]
        public PartialViewResult BoqAttached( int a=0,string b="No")
        {
            try
            {
                string cbfattchemnet = "";
                if (b == "yes")
                {
                    cbfattchemnet = Convert.ToString(a);
                }
                else
                {
                     cbfattchemnet = Request.QueryString["id"].ToString();
                }
                if (cbfattchemnet != "0")
                {
                    Session["cbfdetails"] = cbfattchemnet;
                }
                else
                {
                    cbfattchemnet =Convert.ToString(Session["cbfdetails"]);
                     if (cbfattchemnet != "Attach_1" || cbfattchemnet!="undefined" )
                     {
                         cbfattchemnet = "Attach_1";
                     }
                }
                CbfSumEntity objMAttachment = new CbfSumEntity();
                string dateval = Convert.ToString(Session["attach_date_val"]);
                string cnt = Convert.ToString(Session["attcnt"]);

                if (cbfattchemnet == "Attach_1" || cbfattchemnet=="undefined" )
                {
                    if (cnt == "")
                    {

                        Session["attcnt"] = "31";
                    }
                    else
                    {
                        if (cnt != "Attach_1")
                        {
                            int count = Convert.ToInt32(cnt);
                            count++;
                            Session["attcnt"] = Convert.ToString(count);
                        }
                        else
                        {
                            Session["attcnt"] = "31";
                        }
                    }
                   
                    objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                    if (cbfattchemnet == "undefined")
                    {
                        objMAttachment.amoun = "Attach_1";
                    }
                    else
                    {
                        objMAttachment.amoun = cbfattchemnet;
                    }
                   
                    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                }
                else
                {
                    Session["attcnt"] = cbfattchemnet;
                    CbfSumEntity objAttachment = new CbfSumEntity();
                    objAttachment.attachmentDate = dateval;
                    objAttachment.boqfile = cbfattchemnet;
                    objAttachment.cbfGid = "932600";
                    Session["cbfid"] = "932600";
                    objAttachment.cbfref = "12";
                    objMAttachment.attachment = objRep.Getattachment_val(objAttachment);

                }
                if (Session["ViewBag"] == "" || Session["ViewBag"] == "disabled")
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
                    if (cbfattchemnet != "undefined")
                    {
                        CbfSumEntity objAttachment = new CbfSumEntity();
                        objAttachment.attachmentDate = dateval;

                        Session["attcnt"] = cbfattchemnet;
                        if (cbfattchemnet != "Attach_1")
                        {
                            objMAttachment.attachment = objRep.Getattachment(cbfattchemnet, "12", "2");
                        }
                        
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
        public JsonResult save_attach(ParDetails parModel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    string hj =Convert.ToString(Session["cbfdetails"]);
                    //string[] attach_cnt = Session["cbfdetails"].ToString().Split('_');
                    if (hj != "undefined")
                    {
                        if (Session["ViewBag"] == "")
                        {

                            Session["cbfid"] = "1";
                            parModel.att_identify = Session["attcnt"].ToString();
                            result = objRep.attach_parinlineEdit(parModel);
                            Session["attach_date_val"] = parModel.Attachment_date;
                        }
                        else
                        {
                            parModel.Budgeted = Session["attcnt"].ToString();
                            result = objRep.attach_parinline(parModel);
                            Session["attach_date_val"] = parModel.Attachment_date;

                        }
                    }
                    else
                    {
                        parModel.Budgeted = Session["attcnt"].ToString();
                        result = objRep.attach_parinline(parModel);
                        Session["attach_date_val"] = parModel.Attachment_date;
                    }
                   
                    if (result == "success") RedirectToAction("Index");

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
                    objAttachment = (CbfSumEntity)Session["attachmentdetails"];
                    if (objAttachment.attachment.Count == 0)
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
               // string directory = @"C:/temp/" + txt1.ToString();
                 //  string directory = HoldFileUploadUrlDSA() + id.ToString() + "." + str[str.Length - 1].ToString();
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
        //                    lsFilename = objCmnFunctions.GetSequenceNo("PAR");
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
            DataTable dtac = new DataTable();
            dtac.Columns.Add("Attachid");
            dtac.Columns.Add("AttachName");
            dtac.Columns.Add("AttachFileType");
            dtac.Columns.Add("Attachlength");
            dtac.Columns.Add("flag");
            int j = 1;
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
                        filename = objCmnFunctions.GetSequenceNo("PAR");
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

                        DataRow dr = dtac.NewRow();
                        dr["Attachid"] = j.ToString();
                        //   dr["AttachName"] = Request.Files[file].FileName.ToString();
                        dr["AttachName"] = Path.GetFileName(Request.Files["boqfile"].FileName);
                        dr["AttachFileType"] = Request.Files["boqfile"].ContentType.ToString();
                        dr["Attachlength"] = Request.Files["boqfile"].ContentLength.ToString();
                        dr["flag"] = 0;

                        dtac.Rows.Add(dr);



                        isUploaded = true;
                        // message = "File uploaded successfully!";
                        //  message = Path.GetFileName(Request.Files["boqfile"].FileName);
                        message = filename;
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
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                    //}
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
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
        [HttpGet]
        public PartialViewResult searchEmp(string listfor)
        {
            List<Employee_gid> objowner = new List<Employee_gid>();
            if (listfor == "search")
            {
                if (Session["objowner"] != null)
                    objowner = (List<Employee_gid>)Session["objowner"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["empcode"] != null)
                    TempData["code"] = TempData["empcode"];
                if (TempData["empname"] != null)
                    TempData["name"] = TempData["empname"];
            }
            else
            {
                Session["objowner"] = "";
                objowner = objRep.GetEmployeeDetails();
            }
            return PartialView("searchEmp", objowner);
        }
        [HttpPost]
        public PartialViewResult searchEmp(Employee_gid objownersearch)
        {
            List<Employee_gid> objowner = new List<Employee_gid>();
            objowner = objRep.GetEmployeeDetails();
            Session["objowner"] = null;
            if (objownersearch != null)
            {
                if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
                {
                    TempData["empcode"] = objownersearch.empCode;
                    ViewBag.empcode = objownersearch.empCode;
                    objowner = objowner.Where(x => objownersearch.empCode == null ||
                        (x.empCode.ToUpper().Contains(objownersearch.empCode.ToUpper()))).ToList();
                    Session["objowner"] = objowner;
                }
                if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
                {
                    TempData["empname"] = objownersearch.empName;
                    ViewBag.empname = objownersearch.empName;
                    objowner = objowner.Where(x => objownersearch.empName == null ||
                        (x.empName.ToUpper().Contains(objownersearch.empName.ToUpper()))).ToList();
                    Session["objowner"] = objowner;
                }
                if (objowner.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
            }
            return PartialView("searchEmp", objowner);
           // return Json(objowner, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult DeletePar(ParDetails objUserModel)
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
                    if (objDt.Rows[i]["sno"].ToString() == objUserModel.ParDetails_Gid.ToString())
                        objDt.Rows.RemoveAt(i);

                }
                objGetPrDetails = objRep.GetParDetailsSave(objDt);


                objGetPrDetails.requestFor = new SelectList(objRep.GetList1(), "requeestForGid", "requestForName");
                objGetPrDetails.capexlist = new SelectList(objRep.GetCapex(), "capexId", "capexname");
                objGetPrDetails.budjectedlist = new SelectList(objRep.GetBudjected(), "budjectedid", "budjectedname");
                Session["parcbfdetails"] = objDt;

                if (objGetPrDetails.lListParDetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                Session["parcbfdetails_list"] = objGetPrDetails;
                return PartialView("ParDetailsForPar", objGetPrDetails);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public PartialViewResult Approvaldelete(Employee_gid employee)
        {
            DataTable objDt = new DataTable();
            CbfSumEntity employeedeteials = new CbfSumEntity();
            if (Session["Approval"] != null)
            {
                objDt = (DataTable)Session["Approval"];
            }
            for (int i = 0; i < objDt.Rows.Count; i++)
            {
                if (objDt.Rows[i]["employee_gid"].ToString() == employee.employeeGid.ToString())
                    objDt.Rows.RemoveAt(i);

            }
            Session["Approval"] = objDt;
            employeedeteials = objRep.SaveEmployee_details(objDt);
            return PartialView("EmployeeDetails", employeedeteials);

        }
        [HttpPost]
        public PartialViewResult Approvaldetailssave(Employee_gid Employee_details)
        {
            DataTable objDt = new DataTable();
            CbfSumEntity objemployee = new CbfSumEntity();
            if (Session["Approval"] != null)
                objDt = (DataTable)Session["Approval"];
            if (Session["Approval"] == null)
            {
                objDt.Columns.Add("sno", typeof(int));
                objDt.Columns.Add("employee_gid", typeof(string));
                objDt.Columns.Add("employee_designation", typeof(string));
                objDt.Columns.Add("employee_code", typeof(string));
                objDt.Columns.Add("employee_name", typeof(string));
                objDt.Columns.Add("Approvaldate", typeof(string));
            }
            int sno = Convert.ToInt32(Session["sno1"]) + 1;
            Session["sno1"] = sno;
            string empcode_id = Employee_details.empCode;
            
                Employee_gid objowner = new Employee_gid();
                objowner = objRep.GetEmployeeDetails(empcode_id);
                if (objowner.employeeGid != 0)
                {
                    objDt.Rows.Add(sno, objowner.employeeGid, objowner.empdesignation, objowner.empCode,
                      objowner.empName, Employee_details.Approvaldate);
                    //}
                    //else
                    //{
                    //    objDt.Rows.Add(sno, Employee_details.employeeGid, Employee_details.empdesignation, Employee_details.empCode,
                    //       Employee_details.empName, Employee_details.Approvaldate);
                    //}
                    Session["Approval"] = objDt;
                    objemployee = objRep.SaveEmployee_details(objDt);
                    return PartialView("EmployeeDetails", objemployee);
                }
                else
                {
                    return PartialView("EmployeeDetails", "Please Enter Valid Employee Id");
                }
              
        }
        [HttpPost]
        public JsonResult Create(Parheader Parheader)
        {
            try
            {
                string data = "";
                CbfSumEntity objSumEntity;
                DataTable objDt = new DataTable();
                //if (Session["attachmentdetails"] != null)
                //{
                    objDt = (DataTable)Session["parcbfdetails"];
                    DataTable objapproval = new DataTable();
                    objapproval = (DataTable)Session["Approval"];
                     data = objRep.Insertparheader(Parheader, objDt, objapproval);
                //}
                //else
                //{
                //    data = "Please Attach The File";
                //}
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Submit(Parheader Parheader)
        {
            try
            {
                int appflag = 0;
                int approverid = 0;
                string data = string.Empty;
                try
                {
                    CbfSumEntity obj = new CbfSumEntity();
                    DataTable dtapprover = new DataTable();
                    DataSet dsff = new DataSet();
                    //if (Session["attachmentdetails"] != null)
                    //{
                        if (Session["Approval"] != null)
                        {
                            dtapprover = (DataTable)Session["Approval"];
                            dsff = objRep.getdelmatfinalapprover(6, Parheader.budgeted, Parheader.ParAmount);
                            if (dtapprover.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtapprover.Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(dsff.Tables[0].Rows[0]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(dtapprover.Rows[i]["employee_gid"].ToString()))
                                    {
                                        appflag = 1;
                                    }

                                }
                            }
                            if (appflag != 1)
                            {
                                for (int i = 0; i < dsff.Tables[1].Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(dsff.Tables[0].Rows[0]["delmatflow_order"].ToString()) < Convert.ToInt32(dsff.Tables[1].Rows[i]["delmatflow_order"].ToString()))
                                    {
                                        //if (Convert.ToInt32(dsfapp.Tables[1].Rows[i]["delmatflow_title_ref_gid"].ToString()) == objapprover.Employeedetails[i].employeeGid)
                                        //{
                                        //    appflag = 1;
                                        //}
                                        for (int j = 0; j < dtapprover.Rows.Count; j++)
                                        {
                                            if (Convert.ToInt32(dsff.Tables[1].Rows[i]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(dtapprover.Rows[j]["employee_gid"].ToString()))
                                            {
                                                appflag = 1;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        if (appflag == 1)
                        {
                            DataTable objDt = new DataTable();
                            objDt = (DataTable)Session["parcbfdetails"];
                            DataTable objapproval = new DataTable();
                            objapproval = (DataTable)Session["Approval"];
                            data = objRep.InsertparheaderSubmit(Parheader, objDt, objapproval);
                            ForMailController mailsender = new ForMailController();
                            int refgid = objRep.GetRefGidForMail("PAR");
                            string reqstatus = objRep.GetRequestStatus(refgid, "PAR");
                            int queuegid = objRep.GetQueueGidForMail(refgid, "PAR");
                            mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), "S", "0");
                        }
                        else
                        {
                            data = appflag.ToString();
                        }
                    //}
                    //else
                    //{
                    //    data = "Please Attach The File";
                    //}
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}

        }

        //SHRINIDHI
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
                //    {
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
                string ji=Convert.ToString(Session["cbfid"]);
                if (Convert.ToString(Session["cbfid"]) == "932600" || Convert.ToString(Session["cbfid"]) == "")
                {
                    string dateval = Session["attach_date_val"].ToString();
                    objAttachment.attachmentDate = dateval;
                    objAttachment.boqfile = Session["attcnt"].ToString();
                    objAttachment.cbfGid = "932600";
                    objAttachment.cbfref = "12";
                    objAttachment.attachment = objRep.Getattachment_val(objAttachment);
                   
                }
                else
                {              
                 //   objAttachment.attachmentDate = dateval;
                    cbfattchemnet = Convert.ToString(Session["cbfdetails"]);
                    Session["attcnt"] = cbfattchemnet;
                    objAttachment.attachment = objRep.Getattachment(cbfattchemnet, "12", "2");       
                }
                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeleteInlineAttachment(ParDetails parModel)
        {
            string result = string.Empty;
            try
            {
                if (Session["ViewBag"] == "")
                {
                    Session["cbfid"] = "1";
                }
                Session["att_gid"] = parModel.Attachment;
                int attid = Convert.ToInt32(parModel.Attachment);
                result = objRep.Deleteattach(attid);

                if (result == "success") RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
                    if (objAttachment.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
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
        [HttpGet]
        public PartialViewResult PARAttachmentDetails() 
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult PARAttachmentFields()
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult PARAttachmentIndex(string rowum = null, string pardetid = null, string viewtype = null)
        {
            try 
            {
                TempData["cbfattachmentviewmode"] = null;
                Session["rownum"] = null;
                Session["cbfdetidforattachment"] = null;
                if (rowum != null && rowum != "")
                {
                    ViewBag.InlineAttachmentGid = rowum;
                    Session["rownum"] = Convert.ToInt32(rowum);
                }
                if (pardetid != null && pardetid != "")
                {
                    ViewBag.InlineAttachmentGid = pardetid;
                    Session["cbfdetidforattachment"] = Convert.ToInt32(pardetid);
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
        public JsonResult DeletePARAttachment(CbfSumEntity objCBFAttachment)
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
        public JsonResult CreatePARAttachment(CbfSumEntity objCBFAttachment)
        {
            try
            {
                if (Session["cbfdetidforattachment"] != null)
                {
                    objCBFAttachment._CBFAttachmentID = (int)Session["cbfdetidforattachment"];
                    objCBFAttachment._CBFAttachmentFor = "PARDET";
                }
                else
                {
                    objCBFAttachment._CBFAttachmentID = (int)Session["rownum"];
                    objCBFAttachment._CBFAttachmentFor = "PARTEMP";
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
