using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Excel;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using Ionic.Zip;
using System.Data.SqlClient;
using IEM.App_Start;

namespace IEM.Areas.ifams.Controllers
{

    [NoDirectAccess]
    public class ECFDocDLController : Controller
    {
        //
        // GET: /EOW/ECFDocDL/
        MASTERS.Controllers.ForMailController fm = new MASTERS.Controllers.ForMailController();
        DataModel dm = new DataModel();
        private CmnFunctions objcmnfunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private FileServer objFs = new FileServer();
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Session["Assetqty"] = null;
                Session["AssetSerNo"] = null;
                Session["Assetdetid"] = null;
                //Session["accerr"] = null;
                Session["aqtyTempdataset"] = null;
                Session["aserTempdataset"] = null;
                Session["accfilename"] = null;
                Session["aqtyTempdatatable"] = null;
                Session["aserTempdatatable"] = null;
                Session["accgid"] = null;
                Session["aqtyerr"] = null;
                Session["asererr"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View();
        }
        public ActionResult ECFDocDLdownloadexcel()
        {

            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("ECF Number");
                //dtnew.Columns.Add("Remarks"); 
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "ECF_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ECF_Template.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }


            return View();
        }

        public JsonResult ECFDLUPdate(ECFDLDOCModel obj)
        {
            string mag = "";
            string strcols = "";
            string[] strColArr;
            try
            {
                DataSet resultacc = new DataSet();
                resultacc = (DataSet)Session["aqtyTempdataset"];
                var dataTable = new DataTable();
                dataTable = resultacc.Tables[obj.SheetName.ToString().Trim()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strcols = strcols + dtColumn.ColumnName.Trim() + ":";
                }
                strcols = strcols.Substring(0, strcols.Length - 1);
                strColArr = strcols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "ECF Number"
                                        && strColArr.Count().ToString() == "2")
                {
                    mag = "Success";
                }
                else
                {
                    mag = "Faild";
                }
                if (mag == "Success")
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        int count = dataTable.Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (dataTable.Rows[i]["SNo"].ToString() == ""
                                && dataTable.Rows[i]["ECF Number"].ToString() == ""
                                )
                            {
                                dataTable.Rows[i].Delete();
                                dataTable.AcceptChanges();
                                count--;
                                i--;
                            }
                        }
                        Session["aqtyTempdatatable"] = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult ecfdlUploadstatus(string ddlSheetname)
        {
            try
            {
                if (Session["aqtyTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["aqtyTempdatatable"];
                    ecfdldatasupload(errdataval);
                    errdataval = (DataTable)Session["aqtyErrdatatable"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView();
        }
        public void ecfdldatasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            // string duplicateasset = "";
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string assetdata = "";
                    string status = "";
                    string errs = "";
                    string spgrpId1 = "";
                    string spgrpId2 = "";
                    string spRowCommaSeptated = "";
                    int RowNo = 0;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();
                        if (spRowCommaSeptated.Contains(assetdata) == true)
                        {
                            errs = "Duplicate ECF number Found";
                        }
                        else
                        {
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "ECF number must not be empty";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = dm.GetStatusexcel(assetdata, exceltext, "ECFCheck");
                                if (status == "not")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid ECF Number";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid ECF Number";
                                    }

                                }
                            }
                        }
                        if (errs != "")
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, RowNo, errs);
                        }
                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please Select Valid Sheet");
                }
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Valid Record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of Invalid Record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record(s) in Excel File :" + totalrecord;
                Session["aqtyErrdatatable"] = Errdatatable;
                Session["aqtymaindatatable"] = maindatatable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public void ecfdlUploadFilesnew()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["aqtysupplierattmtfileexcel"] = hpf;
                        Session["aqtyorginal"] = hpf.FileName;
                    }
                }
                //return Json(filename);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json("Upload failed");
            }
        }
        [HttpPost]
        public async Task<JsonResult> ecfdlUploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["aqtysupplierattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["aqtysupplierattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("AQS-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["aqtyfilename"] = n + "_" + Extension;
                    var CmnFilePath = objcmnfunc.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _" + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<AssetqtyModel> objparent = new List<AssetqtyModel>();
                AssetqtyModel objModel1;
                int count = 0;
                string sheets = "";
                FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                DataSet result1 = new DataSet();
                if (Extension1 == ".xlsx")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else if (Extension1 == ".xls")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objModel1 = new AssetqtyModel();
                    objModel1.aqtySheetid = count;
                    objModel1.aqtySheetname = sheets.ToString();
                    objparent.Add(objModel1);
                }


                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel1 = new AssetqtyModel();
                    objModel1.aqtySheetid = count;
                    objModel1.aqtySheetname = sheets.ToString();
                    objparent.Add(objModel1);
                }


                Session["aqtyTempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }


        //[HttpGet]
        //public PartialViewResult ECFDLlocaldetails(string ddlSelectsheet)
        //{
        //    Session["accerr"] = "flag";
        //    return PartialView();
        //}

        public ActionResult ECFDLlocaldetails()
        {

            //string acccheck = "";
            try
            {
                var memoryStream = new MemoryStream();
                Session["accerr"] = "flag";
                DataTable uploadedData, dt = new DataTable();
                uploadedData = (DataTable)Session["aqtymaindatatable"];

                string filename = "ECF_Docs" + System.DateTime.Today.ToShortDateString() + ".zip";
               var CmnFilePath = objcmnfunc.IEMAttachmentPath();
               var localpath = Path.Combine(CmnFilePath, filename);

                   byte[] bytimg = objFs.ECFDocs(uploadedData);
                   System.IO.File.WriteAllBytes(localpath, bytimg);

               return File(localpath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                //SqlConnection con = new SqlConnection();
                //con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                //con.Open();
                //SqlDataAdapter da = new SqlDataAdapter();

                //using (var zip = new Ionic.Zip.ZipFile())
                //{
                //    string mainDirectory = "ECF_and_Docs_" + System.DateTime.Today.ToShortDateString();
                //    string folderLocation = string.Empty;
                //    for (int i = 0; i < uploadedData.Rows.Count; i++)
                //    {
                //        SqlCommand cmd = new SqlCommand("pr_eow_trn_docdownload", con);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add("@ecfno", SqlDbType.VarChar).Value = uploadedData.Rows[i]["ECF Number"].ToString();
                //        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getdocs";
                //        dt = new DataTable();
                //        da = new SqlDataAdapter(cmd);
                        
                //        da.Fill(dt);
                //        objFs.CreateDirectoryNew(mainDirectory);
                //        if (dt.Rows.Count > 0)
                //        {
                //            folderLocation = @"\\\\192.168.0.106\\IEMAttachments\\"+mainDirectory +"\\" + uploadedData.Rows[i]["ECF Number"].ToString() + "\\";
                //            if (objFs.CreateDirectoryNew(mainDirectory + "\\" + uploadedData.Rows[i]["ECF Number"].ToString()) == "success")
                //            {
                //                objFs.SaveFiles(fm.challanPrintmail(dt.Rows[0]["ecf_gid"].ToString(), dt.Rows[0]["ecf_supplier_employee"].ToString()), uploadedData.Rows[i]["ECF Number"].ToString() + ".pdf");

                //                string fileToCopy =  uploadedData.Rows[i]["ECF Number"].ToString() + ".pdf";
                //                objFs.MoveFilesECF(mainDirectory + "\\" + uploadedData.Rows[i]["ECF Number"].ToString(), fileToCopy);                               
                               
                //                for (int ii = 0; ii < dt.Rows.Count; ii++)
                //                {
                //                    fileToCopy =  dt.Rows[ii]["file"].ToString();
                //                    objFs.MoveFilesECF(mainDirectory +"\\" + uploadedData.Rows[i]["ECF Number"].ToString(), fileToCopy); 
                //                }

                //            }
                           
                //        }
                //    }
                //    foreach (var dire in System.IO.Directory.GetDirectories(@"\\\\192.168.0.106\\IEMAttachments\\" + mainDirectory + "\\"))
                //    {
                //        zip.AddDirectory(folderLocation, dire);
                //    }
                  
                //    zip.Save(memoryStream);
                //}

                //memoryStream.Seek(0, 0);
               // return File(memoryStream, "application/octet-stream", "ECF_and_Docs_"+System.DateTime.Today.ToShortDateString() +".zip");

                //DirectoryInfo dirInfo = new DirectoryInfo(@"\\192.168.0.106\IEMAttachments\");
                //List<FileInfo> files = dirInfo.GetFiles().ToList();
                //List<string> filenames = dirInfo.GetFiles().Select(i => i.Name).ToList();

                ////using (var zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(Response.OutputStream))
                ////{
                ////    // Give the file name of downloaded zip file
                ////    Response.AddHeader("Content-Disposition", "attachment; filename=Description.zip");
                ////    // Define content type
                ////    Response.ContentType = "application/zip";
                ////    // Get all file path one by one
                ////    foreach (var path in files)
                ////    {
                ////        // Get every file size

                ////        byte[] fileBytes = System.IO.File.ReadAllBytes(@"\\192.168.0.106\IEMAttachments\" + "SA1608020009.txt");
                ////        // Get every file path
                ////        var fileEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(Path.GetFileName(@"\\192.168.0.106\IEMAttachments\"))
                ////        {
                ////            Size = fileBytes.Length
                ////        };
                ////        zipStream.PutNextEntry(fileEntry);
                ////        zipStream.Write(fileBytes, 0, fileBytes.Length);
                ////    }
                ////    // Clear and closed zipStream object
                ////    zipStream.Flush();
                ////    zipStream.Close();
                ////}
                //DirectoryInfo dirInfo = new DirectoryInfo(@"\\192.168.0.106\IEMAttachments\");
                //List<FileInfo> files = dirInfo.GetFiles().ToList();
                //List<string> filenames = dirInfo.GetFiles().Select(i => i.Name).ToList();
                //using (ZipFile zip = new ZipFile())
                //{
                //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                //    zip.AddDirectoryByName("Files");

                //    string filePath = @"\\192.168.0.106\IEMAttachments\";
                //    zip.AddFile(filePath, "Files");

                //    Response.Clear();
                //    Response.BufferOutput = false;
                //    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                //    Response.ContentType = "application/zip";
                //  Response.AddHeader("content-disposition", "attachment; filename=" + zipName); 
                //    zip.Save(Response.OutputStream);
                //    Response.End();
                //}
                //acccheck = dm.UpdateAQty(uploadedData, filename).ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return RedirectToAction("Index");
            }
        }
        public ActionResult ECFDocDLErrdownloadsexcel()
        {
            try
            {
                Session["aqtyerr"] = "";
                DataTable dtnew = (DataTable)Session["aqtyErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["aqtymaindatatable"];
                dtmainnew.Columns.Add("Error Description");
                for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                        if (i == line)
                            dtmainnew.Rows[line - 1]["Error Description"] = dtnew.Rows[j]["Error Description"];
                    }
                DataTable dt = dtmainnew;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "ECF_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AssetSerialNo_Errorlog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

    }
}
