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
using System.Collections;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class ParRaiserEditController : Controller
    {
        //
        // GET: /FLEXIBUY/ParRaiserEdit/
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        ArrayList lalCbfdetails = new ArrayList();
        public ParRaiserEditController()
            : this(new CbfSumModel())
        {

        }
        public ParRaiserEditController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        [HttpGet]
        public PartialViewResult getApproverList()
        {
            List<CbfSumEntity> lst = new List<CbfSumEntity>();
            return PartialView(lst);
        }
        public ActionResult ParRaiserEditIndex(int parheadgid, string viewfor)
        {
            Session["objPardetails"] = null;
            Session["paredit_Approval"] = null;
            Session["parcbfdetails"] = null;
            Session["approvals"] = null;
            Session["sno"] = null;
            Session["sno1"] = null;
            Session["parheadergid"] = null;
            Session["pardetailsgid"] = null;
            Session["par_ArrayList"] = null;
            Session["par_ArrayListpar"] = null;
            Session["pardetails"] = null;
            Session["attcnt"] = null;
            CbfSumEntity objPardetails = new CbfSumEntity();
            if (viewfor == "edit")
            {
                Session["viewfor"] = "edit";
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                Session["viewfor"] = "view";
                ViewBag.viewfor = "view";
            }
            else if (viewfor == "delete")
            {
                Session["viewfor"] = "delete";
                ViewBag.viewfor = "delete";
            }
            else if (viewfor == "checker")
            {
                Session["viewfor"] = "checker";
                ViewBag.viewfor = "checker";
            }
            Session["mode"] = "9";
            Session["inline"] = "12";
            objPardetails = objRep.getParEdit(parheadgid.ToString());
            Session["objPardetails"] = objPardetails;
            Session["parheadergid"] = parheadgid;
            objPardetails.requestFor = new SelectList(objRep.GetList1(), "requeestForGid", "requestForName");
            objPardetails.capexlist = new SelectList(objRep.GetCapex(), "capexId", "capexname");
            objPardetails.budjectedlist = new SelectList(objRep.GetBudjected(), "budjectedid", "budjectedname");
            return View(objPardetails);
        }
        [HttpPost]
        public JsonResult Delete()
        {
            string data = objRep.DeletePAR();
            return Json(data, JsonRequestBehavior.AllowGet);
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
                {
                    objDt = (DataTable)Session["parcbfdetails"];
                }
                else
                {

                    objDt = (DataTable)Session["pardetailsgid"];

                }
                if (!objDt.Columns.Contains("sno"))
                {

                    objDt.Columns.Add("sno", typeof(int));

                }
                if (!objDt.Columns.Contains("BOQ"))
                {
                    objDt.Columns.Add("BOQ", typeof(int));
                }
                //if (Session["parcbfdetails"] == null)
                //{
                //    objDt.Columns.Add("sno", typeof(int));
                //    objDt.Columns.Add("Expense", typeof(string));
                //    objDt.Columns.Add("department", typeof(string));
                //    objDt.Columns.Add("bugdect", typeof(string));
                //    objDt.Columns.Add("Description", typeof(string));
                //    objDt.Columns.Add("fromyear", typeof(string));
                //    objDt.Columns.Add("toyear", typeof(string));
                //    objDt.Columns.Add("Amount", typeof(decimal));
                //    objDt.Columns.Add("remarks", typeof(string));


                //}
                if (pardetails.ParDetails_Gid.ToString() == null || pardetails.ParDetails_Gid == 0)
                {
                    int sno = Convert.ToInt32(Session["sno"]) + 1;
                    Session["sno"] = sno;
                    var row = objDt.NewRow();
                    row["sno"] = sno;
                    row["Expense"] = pardetails.Expense;
                    row["department"] = pardetails.Department;
                    row["bugdect"] = pardetails.Budgeted;
                    row["Description"] = pardetails.Description;
                    row["fromyear"] = pardetails.FromYear;
                    row["toyear"] = pardetails.ToYear;
                    row["Amount"] = pardetails.ParAmount;
                    row["remarks"] = pardetails.Remarks;
                    row["BOQ"] = pardetails.ParDetails_Gid;
                    objDt.Rows.Add(row);

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
                            objDt.Rows[i]["BOQ"] = pardetails.ParDetails_Gid;

                        }
                    }

                }

                Session["parcbfdetails"] = objDt;
                objcbf = objRep.GetParDetailsSave(objDt);
                ViewBag.viewfor = "edit";
                objcbf.selectedValue = pardetails.Department.ToString();

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
        [HttpPost]
        public PartialViewResult editparupdate(ParDetails pardetails)
        {
            try
            {
                CbfSumEntity objGetPrdetails = new CbfSumEntity();
                if (Session["parcbfdetails"] == null)
                {
                    Session["parcbfdetails"] = Session["pardetailsgid"];
                }
                DataTable objDt = (DataTable)Session["parcbfdetails"];
                if (objDt.Columns.Contains("BOQ"))
                {

                }
                else
                {
                    objDt.Columns.Add("BOQ", typeof(int));
                }

                if (objDt.Rows.Count > 0)
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        string sa = objDt.Rows[i]["pardetails_gid"].ToString();
                        if (pardetails.ParGid.ToString() == objDt.Rows[i]["pardetails_gid"].ToString())
                        {
                            objDt.Rows[i]["Expense"] = pardetails.Expense;
                            objDt.Rows[i]["department"] = pardetails.Department;
                            objDt.Rows[i]["bugdect"] = pardetails.Budgeted;
                            objDt.Rows[i]["Description"] = pardetails.Description;
                            objDt.Rows[i]["fromyear"] = pardetails.FromYear;
                            objDt.Rows[i]["toyear"] = pardetails.ToYear;
                            objDt.Rows[i]["Amount"] = pardetails.ParAmount;
                            objDt.Rows[i]["remarks"] = pardetails.Remarks;
                            objDt.Rows[i]["BOQ"] = sa;
                        }
                    }
                }
                ViewBag.viewfor = "edit";
                Session["parcbfdetails"] = objDt;
                objGetPrdetails = objRep.GetParDetailsSave(objDt);
                objGetPrdetails.selectedValue = pardetails.Department.ToString();
                objGetPrdetails.requestFor = new SelectList(objRep.GetList1(), "requeestForGid", "requestForName");
                objGetPrdetails.capexlist = new SelectList(objRep.GetCapex(), "capexId", "capexname");
                objGetPrdetails.budjectedlist = new SelectList(objRep.GetBudjected(), "budjectedid", "budjectedname");
                return PartialView("ParDetailsForPar", objGetPrdetails);
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
        [HttpPost]
        public PartialViewResult DeletePar(ParDetails objUserModel)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objGetPrDetails = new CbfSumEntity();
                DataTable objDt = new DataTable();
                if (Session["parcbfdetails"] != null)
                {
                    objDt = (DataTable)Session["parcbfdetails"];

                }
                else
                {
                    Session["parcbfdetails"] = Session["pardetailsgid"];
                    objDt = (DataTable)Session["parcbfdetails"];
                }

                if (objUserModel.ParDetails_Gid == null || objUserModel.ParDetails_Gid.ToString() == "0")
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["pardetails_gid"].ToString() == objUserModel.ParGid.ToString())
                        {
                            objDt.Rows.RemoveAt(i);

                            lalCbfdetails = (ArrayList)Session["par_ArrayListpar"];
                            lalCbfdetails.Add(objUserModel.ParGid.ToString());
                            Session["par_ArrayListpar"] = lalCbfdetails;
                        }



                    }
                }
                else
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["sno"].ToString() == objUserModel.ParDetails_Gid.ToString())
                            objDt.Rows.RemoveAt(i);



                    }

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
                Session["attachment"] = objAttachment1;
                if (objAttachment1.attachment.Count == 0)
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
                    // TempData["empcode"] = objownersearch.empCode;
                    ViewBag.empcode = objownersearch.empCode;
                    objowner = objowner.Where(x => objownersearch.empCode == null ||
                        (x.empCode.ToUpper().Contains(objownersearch.empCode.ToUpper()))).ToList();
                    Session["objowner"] = objowner;
                }
                if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
                {
                    // TempData["empname"] = objownersearch.empName;
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
            //return Json(objowner, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult EmployeeDetails()
        {
            CbfSumEntity objemployee = new CbfSumEntity();
            objemployee = (CbfSumEntity)Session["approvalobj"];

            return PartialView(objemployee);
        }
        [HttpPost]
        public JsonResult Approvaldetailssave(Employee_gid Employee_details)
        {
            DataTable objDt = new DataTable();
            DataTable tempdt = new DataTable();

            CbfSumEntity objemployee;
            List<CbfSumEntity> lst = new List<CbfSumEntity>();
            if (Session["paredit_Approval"] != null)
            {
                //  objDt = (DataTable)Session["paredit_Approval"];
                lst = (List<CbfSumEntity>)Session["paredit_Approval"];
            }
            //else
            //{

            //    objDt = (DataTable)Session["approvals"];

            //}

            if (!objDt.Columns.Contains("sno"))
            {
                objDt.Columns.Add("sno", typeof(int));
            }
            if (Session["paredit_Approval"] == null)
            {
                if (!objDt.Columns.Contains("sno"))
                {
                    objDt.Columns.Add("sno", typeof(int));
                }
            }

            //int sno = Convert.ToInt32(Session["sno"]) + 1;
            //Session["sno"] = sno;

            objemployee = new CbfSumEntity();
            //objemployee.slNo = lst.Count + 1;
            //objemployee.approvalgid = "";
            //objemployee.employeeGid = Employee_details.employeeGid;
            //objemployee.empCode = Employee_details.empCode;
            //objemployee.empName = Employee_details.empName;
            //objemployee.empdesignation = Employee_details.empdesignation;
            //objemployee.Approvaldate = Employee_details.Approvaldate;
            //lst.Add(objemployee);
            string empcode_id = Employee_details.empCode;

            Employee_gid objowner = new Employee_gid();
            objowner = objRep.GetEmployeeDetails(empcode_id);
            if (objowner.employeeGid != 0)
            {
                objemployee.slNo = lst.Count + 1;
                objemployee.approvalgid = "";
                objemployee.employeeGid = objowner.employeeGid;
                objemployee.empCode = objowner.empCode;
                objemployee.empName = objowner.empName;
                objemployee.empdesignation = objowner.empdesignation;
                objemployee.Approvaldate = Employee_details.Approvaldate;
                lst.Add(objemployee);
                objemployee = objRep.SaveEmployee_details(objDt);
                //objDt.Rows.Add(Session["tempdt"]);
                Session["paredit_Approval"] = lst;
                Session["approvalobj"] = objemployee;
                Session["Approval"] = objemployee;
                //   return PartialView("EmployeeDetails", objemployee);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please Select Valid Employee Id", JsonRequestBehavior.AllowGet);
            }

           
        }
        [HttpPost]
        public PartialViewResult Approvaldelete(Employee_gid employee)
        {
            try
            {
                DataTable objDt = new DataTable();
                CbfSumEntity employeedeteials = new CbfSumEntity();
                List<CbfSumEntity> lst = new List<CbfSumEntity>();
                if (Session["paredit_Approval"] != null)
                {
                    // objDt = (DataTable)Session["paredit_Approval"];
                    lst = (List<CbfSumEntity>)Session["paredit_Approval"];

                }
                //else
                //{
                //    objDt = (DataTable)Session["approvals"];
                //    if (!objDt.Columns.Contains("sno"))
                //    {
                //        objDt.Columns.Add("sno", typeof(int));
                //    }
                //}
                //if (lst.Count >0)
                //{
                //    for (int i = 0; i < lst.Count; i++)
                //    {
                //        objDt.Rows[i]["sno"] = i + 1;
                //    }
                //}
                string dat = Convert.ToString(employee.employeeGid);
                //if (dat == null)
                //{
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].employeeGid.ToString() == dat)
                        // objDt.Rows.RemoveAt(i);
                        lst.RemoveAt(i);
                }
                //}
                //else
                //{
                //    for (int i = 0; i < objDt.Rows.Count; i++)
                //    {
                //        if (objDt.Rows[i]["parapprover_gid"].ToString() == employee.approvalgid.ToString())
                //            objDt.Rows.RemoveAt(i);
                //        if (Session["par_ArrayList"] != null)
                //            lalCbfdetails = (ArrayList)Session["par_ArrayList"];
                //        lalCbfdetails.Add(employee.approvalgid.ToString());
                //        Session["par_ArrayList"] = lalCbfdetails;

                //    }

                //}

                Session["paredit_Approval"] = lst;
                employeedeteials = objRep.SaveEmployee_details(objDt);
                return PartialView("EmployeeDetails", employeedeteials);
            }
            catch (Exception ex)
            {
                throw ex;
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
                DataTable dt = new DataTable();
                //if (Session["attachmentdetails"] != null)
                //{
                    objDt = (DataTable)Session["parcbfdetails"];
                    if (objDt == null)
                    {
                        dt = (DataTable)Session["pardetails"];
                    }
                    DataTable objapproval = new DataTable();
                    // objapproval = (DataTable)Session["paredit_Approval"];
                    data = objRep.updateparheader(Parheader, objDt, dt, objapproval);
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
            int appflag = 0;
            // int approverid = 0;
            string data = string.Empty;
            try
            {

                DataTable dtfspp = new DataTable();
                dtfspp = objRep.dtgetemployeeilist(Convert.ToInt32(Parheader.ParHeader_Gid));
                //Session["approvalobj"]
                DataSet dsff = new DataSet();
                CbfSumEntity objapprover = new CbfSumEntity();
                //if (Session["approvalobj"] != null)
                //{
                //  objapprover = (CbfSumEntity)Session["approvalobj"];
                //dsfapp = objRep.getdelmatfinalapprover(6, "PAR", Parheader.ParAmount);
                //if (dtfspp.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtfspp.Rows.Count; i++)
                //    {
                //        if (Convert.ToInt32(dsfapp.Tables[0].Rows[0]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(dtfspp.Rows[i]["parapprover_emp_gid"].ToString()))
                //        {
                //            appflag = 1;
                //        }

                //    }
                //}

                //if (appflag != 1)
                //{
                //    for (int i = 0; i < dsfapp.Tables[1].Rows.Count; i++)
                //    {
                //        if (Convert.ToInt32(dsfapp.Tables[0].Rows[0]["delmatflow_order"].ToString()) < Convert.ToInt32(dsfapp.Tables[1].Rows[i]["delmatflow_order"].ToString()))
                //        {
                //            //if (Convert.ToInt32(dsfapp.Tables[1].Rows[i]["delmatflow_title_ref_gid"].ToString()) == objapprover.Employeedetails[i].employeeGid)
                //            //{
                //            //    appflag = 1;
                //            //}
                //            for (int j = 0; j < dtfspp.Rows.Count; j++)
                //            {
                //                if (Convert.ToInt32(dsfapp.Tables[1].Rows[i]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(dtfspp.Rows[j]["parapprover_emp_gid"].ToString()))
                //                {
                //                    appflag = 1;
                //                }

                //            }
                //        }
                //    }
                //}
                //if (Session["attachmentdetails"] != null)
               // {
                    if (Session["paredit_Approval"] != null)
                    {
                        List<CbfSumEntity> lst = new List<CbfSumEntity>();
                        // dtapprover = (DataTable)Session["Approval"];
                        lst = (List<CbfSumEntity>)Session["paredit_Approval"];
                        dsff = objRep.getdelmatfinalapprover(6, Parheader.budgeted, Parheader.ParAmount);
                        for (int i = 0; i < lst.Count; i++)
                        {
                            if (Convert.ToInt32(dsff.Tables[0].Rows[0]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(lst[i].employeeGid.ToString()))
                            {
                                appflag = 1;
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
                                    for (int j = 0; j < lst.Count; j++)
                                    {
                                        if (Convert.ToInt32(dsff.Tables[1].Rows[i]["delmatflow_title_ref_gid"].ToString()) == Convert.ToInt32(lst[j].employeeGid.ToString()))
                                        {
                                            appflag = 1;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    // }
                    if (appflag == 1)
                    {
                        DataTable objDt = new DataTable();
                        DataTable dt = new DataTable();
                        objDt = (DataTable)Session["parcbfdetails"];
                        if (objDt == null)
                        {
                            dt = (DataTable)Session["pardetails"];
                        }
                        DataTable objapproval = new DataTable();
                        //objapproval = (DataTable)Session["paredit_Approval"];
                        data = objRep.updateparheaderSubmit(Parheader, objDt, dt, objapproval);

                        int refgid = Convert.ToInt32(Parheader.ParHeader_Gid);
                        string reqstatus = objRep.GetRequestStatus(refgid, "PAR");
                        int queuegid = objRep.GetQueueGidForMail(refgid, "PAR");
                        ForMailController mailsender = new ForMailController();
                        string curapprovalstage = "0";
                        if (reqstatus == "S")
                        {
                            curapprovalstage = "0";
                        }
                        else
                        {
                            curapprovalstage = "1";
                        }
                        mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                        RedirectToAction("Index");
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
        [HttpPost]
        public JsonResult Approved(string parRemarks)
        {
            try
            {
                string data = objRep.parcheckerapproval(parRemarks);
                if (Session["parheadergid"] != null)
                {
                    int refgid = Convert.ToInt32(Session["parheadergid"]);
                    string reqstatus = objRep.GetRequestStatus(refgid, "PAR");
                    int queuegid = objRep.GetQueueGidForMail(refgid, "PAR");
                    ForMailController mailsender = new ForMailController();
                    string curapprovalstage = "0";
                    if (reqstatus == "S")
                    {
                        curapprovalstage = "0";
                    }
                    else
                    {
                        curapprovalstage = "1";
                    }
                    mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
                throw ex;
            }




        }
        [HttpPost]
        public JsonResult Rejected(string parRemarks)
        {
            string data = objRep.parcheckerReject(parRemarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //SHRINIDHI
        [HttpGet]
        public PartialViewResult BoqAttachgridinline()
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

        public ActionResult ParRaiserEditIndexView(int parheadgid) 
        {
            Session["objPardetails"] = null;
            Session["paredit_Approval"] = null;
            Session["parcbfdetails"] = null;
            Session["approvals"] = null;
            Session["sno"] = null;
            Session["sno1"] = null;
            Session["parheadergid"] = null;
            Session["pardetailsgid"] = null;
            Session["par_ArrayList"] = null;
            Session["par_ArrayListpar"] = null;
            Session["pardetails"] = null;
            Session["attcnt"] = null;
            CbfSumEntity objPardetails = new CbfSumEntity();
          
            Session["viewfor"] = "view";
            ViewBag.viewfor = "view";
            
            Session["mode"] = "9";
            Session["inline"] = "12";
            objPardetails = objRep.getParEdit(parheadgid.ToString());
            Session["objPardetails"] = objPardetails;
            Session["parheadergid"] = parheadgid;
            objPardetails.requestFor = new SelectList(objRep.GetList1(), "requeestForGid", "requestForName");
            objPardetails.capexlist = new SelectList(objRep.GetCapex(), "capexId", "capexname");
            objPardetails.budjectedlist = new SelectList(objRep.GetBudjected(), "budjectedid", "budjectedname");
            return View(objPardetails);
        }
    }
}
