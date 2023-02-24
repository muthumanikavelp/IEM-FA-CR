using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using IEM.Common;
using System.Net.Mime;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

    [NoDirectAccess]
    public class SampleAttachmentController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        private IRepository objModel;

        public SampleAttachmentController()
            :this(new SupDataModel()) 
        {

        }
        public SampleAttachmentController(IRepository objM)
        {
            objModel = objM; 
        }
        //
        // GET: /ASMS/SampleAttachment/

        public ActionResult Index()  
        {
            Session["SupplierHeaderGid"] = 1;
            SupAttachment objSubAttachment = new SupAttachment();
            objSubAttachment.lstDocumentGroup = new SelectList(objModel.GetDocumentGroup(), "_DocumentGroupID", "_DocumentGroupName");
            ViewBag.supattachment = objSubAttachment;
            return View();
        }
        [HttpGet]
        public PartialViewResult ContractualAttachment()   
        {
            List<SupAttachment> lst = new List<SupAttachment>();
            return PartialView(lst);
        }
       
        [HttpPost]
        public JsonResult DeleteContractualAttachment(SupAttachment supattachmentmodel) 
        {
            int attachmentID = (int)supattachmentmodel._SupAttachmentID;
            objModel.DeleteSupAttachment(attachmentID);
            return Json(supattachmentmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult CreateContractualAttachment()
        {
            SupAttachment objM = new SupAttachment();
            objM.lstDocumentGroup = new SelectList(objModel.GetDocumentGroup(), "_DocumentGroupID", "_DocumentGroupName");
            // objM.lstDocumentName = new SelectList(objModel.GetDocumentName(), "_DocumentNameID", "_DocumentName");
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateContractualAttachment(SupAttachment supattachmentmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupAttachment(supattachmentmodel);
                  //  int supAttachmentGid = objModel.GetSupAttachmentGID();
                    //DirectoryInfo d = new DirectoryInfo(@"C:\temp\");
                    //FileInfo[] infos = d.GetFiles();
                    //int i = 0;
                    //foreach (FileInfo f in infos)
                    //{
                    //    string filenamewithoutExt=f.Name;
                    //    filenamewithoutExt = filenamewithoutExt.Substring(0, filenamewithoutExt.LastIndexOf(".") + 0);
                    //    if (filenamewithoutExt == "temp_" + i.ToString())
                    //    {
                    //        string ext = Path.GetExtension(f.Name);
                    //        System.IO.File.Move(f.FullName,f.FullName.ToString().Replace(filenamewithoutExt, "SUP" + supAttachmentGid.ToString("00000000")));
                    //        i++;
                    //    }
                    
                    //}
                }
                return Json(supattachmentmodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EditContractualAttachment(int id, string viewfor)
        {
            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            SupAttachment model = objModel.GetSupAttachmentById(id);
            model.lstDocumentGroup = new SelectList(objModel.GetDocumentGroup(), "_DocumentGroupID", "_DocumentGroupName", model.selectedDocumentGroupID);
            model.lstDocumentName = new SelectList(objModel.GetDocumentName(model.selectedDocumentGroupID), "_DocumentNameID", "_DocumentName", model.selectedDocumentNameID);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EditContractualAttachment(SupAttachment supattachmentmodel) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupAttachment(supattachmentmodel);
                }
                return Json(supattachmentmodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetDocumentName(SupAttachment supattachmentmodel) 
        {
            try
            {
                var DocGroupID = supattachmentmodel.selectedDocumentGroupID;
                return Json(objModel.GetDocumentName(DocGroupID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpPost]
        public async Task<JsonResult> UploadedFiles(string fname) 
        {
            try
            {
                string filename="";
                foreach (string file in Request.Files)
                {  
                    var fileContent = Request.Files[file];
                   
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        
                        if (fname != null && fname.Trim() != "")
                        {
                            filename = fname.Substring(0,fname.LastIndexOf(".")+0);
                        }
                        else
                        {
                            filename = objCmnFunctions.GetSequenceNo("SA");
                        }
                        var fileextension = Path.GetExtension(fileContent.FileName); 
                        var stream = fileContent.InputStream;

                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                     //   var path = Path.Combine(@"C:/temp/", filename + fileextension);
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        //using (var fileStream =System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}


                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);


                        filename = filename + fileextension;
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                    //    byte[] bytFile = System.IO.File.ReadAllBytes(path);
                     //   FileServer objserver = new FileServer();
                    //    string result = objserver.SaveFiles(bytFile, filename);
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                }
                return Json(filename);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }
       
        public FileResult DownloadDocument(string id)   
        { 
            try
            {
                string filename = id;
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
              //  var path = Path.Combine(@"C:\temp\", filename);
                var path = Path.Combine(CmnFilePath, filename);


                var apiresult = Cmnfs.DownloadFile(filename).Result;
                if (apiresult == "Fail")
                {
                    return File("", "");
                }
                else
                {
                    byte[] filebyte = Convert.FromBase64String(apiresult);
                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }


               // FileServer ObjService = new FileServer();
               // var isExists = ObjService.CheckFileExists(filename);
              //  if (isExists == 1)
              //  {
              //      byte[] filebyte = ObjService.CopyFiles(filename);
              //      System.IO.File.WriteAllBytes(path, filebyte);
              //  }
              ////  return File(path, objCmnFunctions.GetContentType(filename), filename);
              //  return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
