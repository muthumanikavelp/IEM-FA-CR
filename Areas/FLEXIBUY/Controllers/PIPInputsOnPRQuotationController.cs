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
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PIPInputsOnPRQuotationController : Controller
    {
        private IRepositoryKIR objrep;
        CmnFunctions objCmnFunctions = new CmnFunctions();

        public PIPInputsOnPRQuotationController()
            : this(new prsummodel())
        { }
        public PIPInputsOnPRQuotationController(IRepositoryKIR objm)
        {
            objrep = objm;
        }
        public PartialViewResult PipInputsOnPrQuotation(string id)
        {
            PrSumEntity obj1 = new PrSumEntity();
            PrHeader pr = new PrHeader();

            string ss = "View";

            if (id != null)
            {
                Session["id"] = id;

                pr.prRefNo = id;
                obj1 = objrep.ViewPrDetails(pr, ss);
                obj1.lstattfile = new List<PrSumEntity>();
            }
            return PartialView(obj1);
        }

        [HttpPost]
        public ActionResult PipInputsOnPrQuotation1(PrSumEntity obj, List<HttpPostedFileBase> fileUpload, string id)
        {
            string filename = string.Empty;
            PrSumEntity obj2 = new PrSumEntity();
            List<PrSumEntity> objj = new List<PrSumEntity>();

            PrHeader pr = new PrHeader();

            //if (Session["AttachmentList"] == null)
            //{
            if (obj.FilesToBeUploaded != null)
            {
                foreach (HttpPostedFileBase item in fileUpload)
                {
                    if (item != null && Array.Exists(obj.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                    {
                        string path = "//192.168.0.232/images/";
                        filename = Path.GetFileName(item.FileName);
                        item.SaveAs(Path.Combine(path, filename));
                        //if (obj.attachmentdate != null && obj.attachmentdesc != null)
                        //{
                        PrSumEntity obj1 = new PrSumEntity();
                        obj1.documentName = Path.GetFileName(item.FileName);
                        obj1.attachmentDate = obj.attachmentDate;
                        obj1.attachmentDesc = obj.attachmentDesc;
                        objj.Add(obj1);
                        //}
                        //else
                        //{
                        //    if (obj.attachmentdate == null)
                        //    {
                        //        ViewBag.date = "Please select date";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.description = "Please enter description";
                        //    }
                        //}
                    }

                }
            }

            string ss = "View";
            pr.prRefNo = Session["id"].ToString();
            obj2 = objrep.ViewPrDetails(pr, ss);
            obj2.lstattfile = objj.ToList();

            Session["AttachmentList"] = obj2.lstattfile;
            //}

            return View(obj2);

        }

        [HttpGet]
        public PartialViewResult PIPattach(int row)
        {
           // Session["pipdetails"] = null;
            //Session["pipattachmentList"] = null;
            //Session["AccessTokenheader1"] = null;



            //if (Session["row"] != row)
            //{
            //    Session["AccessToken1"] == "";
            //}
           // Session["AccessTokenheader1"]
            PrSumEntity objMAttachment = new PrSumEntity();
            try
            {
                if (Session["AccessTokenheader1"] == null)
                {
                    
                    string prattchmnet = "";
                    Session["pipdetails"] = row;
                                        
                    if (prattchmnet == "")
                    {
                        objMAttachment.documentName = prattchmnet;
                        objMAttachment = objrep.PIPAttachmentname(objMAttachment, row);
                    }
                    else
                    {
                        objMAttachment.documentName = prattchmnet;
                        objMAttachment = objrep.PIPAttachmentname(objMAttachment, row);
                    }
                    objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                }
                else
                {
                    DataTable dtnew = new DataTable();
                    Session["pipdetails"] = row;
                    dtnew = (DataTable)Session["AccessTokenheader1"];
                    objMAttachment.attachment = new List<Attachment>();
                    if (dtnew.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtnew.Rows.Count; i++)
                        {
                            Attachment obj_getprdetails = new Attachment();
                            obj_getprdetails.attachGid = dtnew.Rows[i]["prdetails"].ToString();
                            obj_getprdetails.attachedDate = string.IsNullOrEmpty(dtnew.Rows[i]["Attachment_Date"].ToString()) ? dtnew.Rows[i]["AttachmentDate"].ToString() : dtnew.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = string.IsNullOrEmpty(dtnew.Rows[i]["Documnet_Name"].ToString()) ? dtnew.Rows[i]["Filename"].ToString() : dtnew.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = string.IsNullOrEmpty(dtnew.Rows[i]["Document_des"].ToString()) ? dtnew.Rows[i]["AttachmentDescription"].ToString() : dtnew.Rows[i]["Document_des"].ToString();
                            objMAttachment.attachment.Add(obj_getprdetails);
                        }
                    }
                   // objMAttachment = (PrSumEntity)Session["oldvalue"];
                  //  objMAttachment = objrep.PIPAttachmentname(objMAttachment, row);
                    //attachment1.attachment=attachment1.attachment.Where(attachment1.attachment)
                    objMAttachment.attachment = objMAttachment.attachment.Where(x => x.attachGid == row.ToString()).ToList();
                }
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UploadedFilesPIP()
        {
            //try
            //{
                string filename = string.Empty;
            //    int rr = 0;
            //    int row = Convert.ToInt32(Session["pipdetails"]);

            //    List<PrDetails> lstprdet = (List<PrDetails>)Session["prdetLst"];
            //    for (int i = 0; i < lstprdet.Count; i++)
            //    {
            //        if (row - 1 == i)
            //        {
            //            rr = lstprdet[i].prDet_Gid;
            //        }

            //    }

            //    foreach (string file in Request.Files)
            //    {
            //        var fileContent = Request.Files[file];

            //        if (fileContent != null && fileContent.ContentLength > 0)
            //        {

            //            if (fname != null && fname.Trim() != "")
            //            {
            //                filename = fname.Substring(0, fname.LastIndexOf(".") + 0) + rr;
            //            }
            //            else
            //            {
            //                filename = objCmnFunctions.GetSequenceNo("PIP") + '-' + rr;
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


            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            try
            {
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string;
                    filename = objCmnFunctions.GetSequenceNo("PR");
                    var fext = Path.GetExtension(myFile.FileName);
                    myFile.SaveAs(Path.Combine(HoldFileUploadUrlDSA(), filename + fext));

                    isUploaded = true;
                   // message = Path.GetFileName(Request.Files["MyFile"].FileName);
                    message = filename + fext;
                }

                 
                               
                return Json(new { isUploaded = isUploaded, message = message }, "text/html");
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
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
        [HttpPost]
        public JsonResult PIPattachgrid(PrSumEntity attachmodel)
        {
            try
            {
                int row;
                string rown ="";
              
               // Attachment obj_getprdetails1;
                PrSumEntity attachment1 = new PrSumEntity();
                if (ModelState.IsValid)
                {
                    row = Convert.ToInt32(Session["pipdetails"]);
                    if (Convert.ToInt32(Session["row"]) != row)
                    {
                        Session["AccessToken1"] = "";
                    }

                    Session["oldvalue"] = null;
                    string rown1 = row.ToString();
                    attachment1 = objrep.PIPAttachmentname(attachmodel, row);
                    Session["oldvalue"] = attachment1;
                    //attachment1.attachment=attachment1.attachment.Where(attachment1.attachment)
                    attachment1.attachment = attachment1.attachment.Where(x => x.attachGid == rown1).ToList();
                        //  (x.attachGid.ToUpper().Contains(row)).ToList();
                   // PrSumEntity obj1 = new PrSumEntity();
                  
                   
                   
                  // if(rown==string.Empty)
                  // {
                     
                  //     rown = row.ToString();

                      
                    
                  //     Attachment obj_getprdetails1 = new Attachment();
                  //     obj_getprdetails1.attachedDate = string.IsNullOrEmpty(attachmodel.attachmentDate.ToString()) ? "0" : attachmodel.attachmentDate.ToString();
                  //     obj_getprdetails1.fileName = string.IsNullOrEmpty(attachmodel.attachName.ToString()) ? string.Empty : attachmodel.attachName.ToString();
                  //     obj_getprdetails1.description = string.IsNullOrEmpty(attachmodel.attachmentDesc.ToString()) ? string.Empty : attachmodel.attachmentDesc.ToString();
                  //     stts.Add(obj_getprdetails1);
                  //     Session["pipattachmentList"] = null;
                  // }
                  // else
                  // {
                  //     if(!rown.Contains(rown))
                  //     {
                          
                  //         rown = "," + row.ToString();
                  //         Attachment obj_getprdetails = new Attachment();
                  //         obj_getprdetails.attachedDate = attachmodel.attachmentDate.ToString();
                  //         obj_getprdetails.fileName = attachmodel.attachName.ToString();
                  //         obj_getprdetails.description = attachmodel.attachmentDesc.ToString();
                  //         stts.Add(obj_getprdetails);
                  //         Session["pipattachmentList"] = null;
                  //     }
                  //     else
                  //     {
                          
                  //         Attachment obj_getprdetails = new Attachment();
                  //         obj_getprdetails.attachedDate = attachmodel.attachmentDate.ToString();
                  //         obj_getprdetails.fileName = attachmodel.attachName.ToString();
                  //         obj_getprdetails.description = attachmodel.attachmentDesc.ToString();
                  //         stts.Add(obj_getprdetails);
                  //         if (Session["pipattachmentList"] != null)
                  //         {
                  //             attachment1 = (PrSumEntity)Session["pipattachmentList"];
                  //             attachment1.attachment.AddRange(attachment1.attachment);
                  //         }
                  //     }
                     
                  // }

                  
                  
                
                  ////  Session["pipattachmentList"] = attachment1;
                  // attachment1.attachment = stts;


                   // PrSumEntity stts = new PrSumEntity();

                    
                   Session["pipattachmentList"] =  attachment1;
                }
                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult PIPattachgrid()
        {
            try
            {
                PrSumEntity obj = new PrSumEntity();

                obj = (PrSumEntity)Session["pipattachmentList"];

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult Downloaded(PrSumEntity prattachmentmodel)
        {
            Session["downfile"] = prattachmentmodel.attachment1;
            PrSumEntity obj = new PrSumEntity();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string ff)
        {

            string txt1 = Session["downfile"].ToString();
            string directory = HoldFileUploadUrlDSA() + txt1.ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            string fileName = "Download" + txt1.ToString();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public JsonResult DeleteAttachmentList(PrSumEntity obj)
        {
            DataTable dt = new DataTable();

            PrSumEntity attachment1 = new PrSumEntity();
            PrSumEntity rr = new PrSumEntity();
            DataTable ss = new DataTable();
            rr = (PrSumEntity)Session["pipattachmentList"];
            ss = (DataTable)Session["AccessTokenheader1"];

            if (Session["pipattachmentList"] != null)
            {
                int rowcount = 1;
                dt.Columns.Add("Srno");
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

                if(ss.Rows.Count>0)
                {
                    for (int i = 0; i < ss.Rows.Count; i++)
                    {
                        if (ss.Rows[i]["Documnet_Name"].ToString() == obj.attachment1)
                        {
                            ss.Rows.RemoveAt(i);
                        }
                        ss.AcceptChanges();
                    }
                }
                List<Attachment> lst = objrep.EditPRAttachmentList(dt);
                attachment1.attachment = lst;

                Session["pipattachmentList"] = attachment1;
                Session["AccessTokenheader1"] = ss;
            }



            return Json(attachment1, JsonRequestBehavior.AllowGet);
        }
    }
}
