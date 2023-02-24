using IEM.Areas.IFAMS.Models;
using IEM.Common;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;


namespace IEM.Areas.IFAMS.Controllers
{
    public class AssetImpairmentsController : Controller
    {
        //
        // GET: /IFAMS/AssetImpairments/
        private ifamsIRepositoryx imp;
        private IfamsRepository_M sal;
        private IRepository Tfr;
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        CommonIUD objCommonIUD = new CommonIUD();

        public AssetImpairmentsController()
            : this(new Ifams_Modelx(), new IfamsDataModel_M(), new DataModel())
        {

        }

        public AssetImpairmentsController(ifamsIRepositoryx objModel, IfamsRepository_M objModel1, DataModel objModeltr)
        {
            imp = objModel;
            sal = objModel1;
            Tfr = objModeltr;
        }

        public ActionResult AssetImpairmentsSummary()
        {
            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel Records = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                ViewBag.StatusFlage = "DRAFT";
                Ifams_Modelx Model = new Ifams_Modelx();
                Records.ImpModel = imp.GetImpDetail("DRAFT");
                if (Records.ImpModel.Count == 0) { ViewBag.Message = "No records found"; }
                ViewBag.UserRole = "IOAMKR";
                Session["Role"] = "IOAMKR";
                Session["ImpairExceldownload"] = Records.ImpModel;
                return View(Records);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
        }


        public ActionResult AssetImpairmentsSummaryy()
        {
            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel Records = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                Ifams_Modelx Model = new Ifams_Modelx();
                Records.ImpModel = imp.GetImpDetailChecker();
                if (Records.ImpModel.Count == 0) { ViewBag.Message = "No records found"; }
                ViewBag.UserRole = "IOACHK";
                Session["Role"] = "IOACHK";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View(Records);
        }

        [HttpPost]
        public ActionResult AssetImpairmentsSummaryy(string filter, string filter1, string command)
        {
            try
            {
                if (command == "SEARCH")
                {
                    ViewBag.UserRole = "IOAMKR";
                    IFAMS.Models.Ifams_Propertyx.ImpairmentsModel details = new IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
                    details.ImpModel = imp.GetImpDetailChecker();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)))
                    {
                        details.ImpModel = imp.GetImpDetailChecker();
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        details.ImpModel = details.ImpModel.Where(x => filter == null || (x._imp_number.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.filter1 = filter1;
                        details.ImpModel = details.ImpModel.Where(x => filter1 == null || (x._imp_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (details.ImpModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["ImpairExceldownload"] = details.ImpModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("AssetImpairmentsSummaryy", "AssetImpairments"); }
                Session["Role"] = "IOACHK";
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {

            }
        }


        public JsonResult ImpairStatus()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            Ifams_Modelx Model = new Ifams_Modelx();
            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel details = new IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
            try
            {
                return Json(Model.GetImpairStatus(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }


        [HttpPost]
        public ActionResult AssetImpairmentsSummary(string filter, string filter1, string command, string StatusFlage)
        {
            try
            {
                if (command == "SEARCH")
                {
                    IFAMS.Models.Ifams_Propertyx.ImpairmentsModel details = new IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
                    details.ImpModel = imp.GetImpDetail(StatusFlage);
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter))
                        && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1))
                          && ((string.IsNullOrEmpty(StatusFlage) || string.IsNullOrWhiteSpace(StatusFlage))
                        ))
                    {
                        details.ImpModel = imp.GetImpDetail(StatusFlage);
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        details.ImpModel = details.ImpModel.Where(x => filter == null || (x._imp_number.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.filter1 = filter1;
                        details.ImpModel = details.ImpModel.Where(x => filter1 == null || (x._imp_date.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (StatusFlage != null)
                    {
                        ViewBag.StatusFlage = StatusFlage;
                        details.ImpModel = details.ImpModel.Where(x => StatusFlage == null || (x.impstatus.ToUpper().Contains(StatusFlage.ToUpper()))).ToList();
                    }
                    if (details.ImpModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(details);

                }
                if (command == "Clear")
                { return RedirectToAction("AssetImpairmentsSummary", "AssetImpairments"); }
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }

        }

        public ActionResult Impairdownloadexcel()
        {
            try
            {
                using (DataTable dtnew = new DataTable())
                {
                    dtnew.Columns.Add("SNo");
                    dtnew.Columns.Add("Asset Id");
                    dtnew.Columns.Add("Branch");
                    dtnew.Columns.Add("Impairment Date");
                    dtnew.Columns.Add("Inward No");
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtnew, "Impairment_Template");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=Impairment_Template.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View();
        }


        public ActionResult errlogdownloadsexcel()
        {
            try
            {
                Session["err"] = "";
                DataTable dtnew = (DataTable)Session["Errdatatable"];
                DataTable dtmainnew = (DataTable)Session["maindatatable"];
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
                    wb.Worksheets.Add(dt, "Impairment_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Impairment_ErrorLog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        //  Response.Flush();
                        Response.End();
                    }
                }
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ImpairmentofAsset.xls"));
                //Response.ContentType = "application/ms-excel";
                //DataTable dt = dtnew;
                //string str = string.Empty;
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write(str + dtcol.ColumnName);
                //    str = "\t";
                //}
                //Response.Write("\n");
                //foreach (DataRow drr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + Convert.ToString(drr[j]));
                //        str = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
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


        [HttpPost]
        public JsonResult MakerImpairments()
        {
            string msg = "";
            string Maker = string.Empty;
            Maker = objCmnFunctions.authorize("226");
            try
            {
                if (Maker == "Success")
                {
                    int GidforMake = (int)Session["GidforMaker"];
                    IFAMS.Models.Ifams_Modelx Model = new Ifams_Modelx();
                    msg = Model.UpdateMaker(GidforMake);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(new { msg, Maker }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult UploadStatusPartial()
        {
            return PartialView();
        }
        public PartialViewResult ViewImpairments(string id, string viewfor)
        {

            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel Records = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                Ifams_Modelx Model = new Ifams_Modelx();
                Records.ImpModel = imp.GetImpView(id);
                Session["GidforMaker"] = Convert.ToInt32(id.ToString());
                Records.impstatus = id;
                if (Records.ImpModel.Count == 0) { ViewBag.Message = "No records found"; }
                if (viewfor.ToString() == "IOACHK")
                {
                    ViewBag.UserRole = "IOACHK";
                    string Status = Model.GetStatus(Convert.ToInt32(id));
                    if (Status.ToString() == "APPROVED")
                    {
                        ViewBag.Status = "APPROVED";
                    }
                    else if (Status.ToString() == "REJECTED")
                    {
                        ViewBag.Status = "REJECTED";
                    }
                    else if (Status.ToString() == "DRAFT")
                    {
                        ViewBag.Status = "DRAFT";
                    }
                    else if (Status.ToString() == "viewRevoke")
                    {
                        ViewBag.Status = "viewRevoke";
                    }
                }
                else if (viewfor.ToString() == "IOAMKR")
                {
                    ViewBag.UserRole = "IOAMKR";
                    string Status = Model.GetStatus(Convert.ToInt32(id));
                    if (Status.ToString() == "WAITING FOR APPROVAL")
                    {
                        ViewBag.Status = "WAITING FOR APPROVAL";
                    }
                    else if (Status.ToString() == "APPROVED")
                    {
                        ViewBag.Status = "APPROVED";
                    }
                    else if (Status.ToString() == "REJECTED")
                    {
                        ViewBag.Status = "REJECTED";
                    }
                }
                else if (viewfor.ToString() == "viewDetail")
                {
                    ViewBag.UserRole = "IOACHK";
                    ViewBag.Status = "viewDetail";
                }
                else if (viewfor.ToString() == "viewRevoke")
                {
                    ViewBag.UserRole = "IOACHK";
                    ViewBag.Status = "viewRevoke";
                }
                else
                {
                    ViewBag.UserRole = "Invalid User";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return PartialView(Records);
        }


        public JsonResult Approve(string Approve, string Remarks)
        {
            string Msg = "";
            string Checker = string.Empty;
            Checker = objCmnFunctions.authorize("227");
            try
            {
                if (Checker == "Success")
                {
                    int GidforApp = (int)Session["GidforMaker"];
                    if (Approve.ToString() == "Approve")
                    {
                        Ifams_Modelx Model = new Ifams_Modelx();
                        Msg = Model.UpdateCheckImpairStatus(GidforApp, Remarks);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Reject(string Reject, string Remarks)
        {
            string Msg = "";
            string Checker = string.Empty;
            Checker = objCmnFunctions.authorize("227");
            try
            {
                if (Checker == "Success")
                {
                    int GidforApp = (int)Session["GidforMaker"];
                    if (Reject.ToString() == "Reject")
                    {
                        Ifams_Modelx Model = new Ifams_Modelx();
                        Msg = Model.UpdateCheckImpairStatusRej(GidforApp, Remarks);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }




        public void ImpairUploadFilesnew()
        {
            string filename = "";
            try
            {

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["RAISERattmtfileexcel"] = hpf;
                        Session["original"] = hpf.FileName;
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            //return Json(filename);
        }

        public PartialViewResult IOAAuditTrial()
        {
            try { return PartialView(); }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }

        }


        [HttpPost]
        public async Task<JsonResult> ImpairUploadFiles()
        {
            string Extension1 = "";
            string error = "";
            List<Ifams_Propertyx.ImpSheetData> objparent = new List<Ifams_Propertyx.ImpSheetData>();
            try
            {
                string path1 = "";
                if (Session["RAISERattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["RAISERattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("IOA{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["filename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                    // path1 = @"C:\temp\" + n + " _ " + Extension;

                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }

                Ifams_Propertyx.ImpSheetData objmodel1;
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
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objmodel1 = new Ifams_Propertyx.ImpSheetData();
                    objmodel1.ImpairSheetid = count;
                    objmodel1.ImpairSheetname = error.ToString();
                    objparent.Add(objmodel1);
                }
                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objmodel1 = new Ifams_Propertyx.ImpSheetData();
                    objmodel1.ImpairSheetid = count;
                    objmodel1.ImpairSheetname = sheets.ToString();
                    objparent.Add(objmodel1);
                }
                Session["Tempdataset"] = result1;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);

        }
        public FileResult SADownloadDocument(string id)
        {
            string filename = "";
            var CmnFilePath = objCmnFunctions.IEMAttachmentPath();

            var sapath = Path.Combine(CmnFilePath, filename);
            //var sapath = Path.Combine(@"C:\temp\", filename);
            try
            {
                filename = id;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
            return File(sapath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }


        public JsonResult ImpairMakerUpdate(Ifams_Propertyx.ImpSheetData obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdataset"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[obj.ImpairSheetname.ToString()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Branch"
                     && strColArr[3].ToString() == "Impairment Date"
                    && strColArr[4].ToString() == "Inward No")
                {
                    mag = "Success";
                }
                else
                {
                    mag = "Faild";
                }

                if (mag == "Success")
                {
                    int count = dataTable.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataTable.Rows[i]["SNo"].ToString() == "" && dataTable.Rows[i]["Asset Id"].ToString() == ""
                            && dataTable.Rows[i]["Branch"].ToString() == ""
                            && dataTable.Rows[i]["Impairment Date"].ToString() == ""
                            && dataTable.Rows[i]["Inward No"].ToString() == "")
                        {
                            dataTable.Rows[i].Delete();
                            count = count - 1;
                            i = i - 1;
                            dataTable.AcceptChanges();

                        }
                    }
                    Session["Tempdatatable"] = dataTable;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult ImpairUploadstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["Tempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatable"];
                    Impairdatasupload(errdataval);
                    errdataval = (DataTable)Session["Errdatatable"];
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


        [HttpGet]
        public PartialViewResult ImpLogStatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["Tempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatable"];
                    Impairdatasupload(errdataval);
                    errdataval = (DataTable)Session["Errdatatable"];
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


        private void Impairdatasupload(DataTable dataTable)
        {
            IFAMS.Models.Ifams_Modelx Model = new IFAMS.Models.Ifams_Modelx();
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            IfamsPhysicalVerificationModel Model1 = new IfamsPhysicalVerificationModel();
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    string exceltxt = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    string strRowCommaSeparated = "";
                    //string assetdata = "";
                    string sagrpId1 = "";
                    string sagrpId2 = "";

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string assetdata = dataTable.Rows[i][1].ToString();
                        errs = "";
                        RowNo = i + 1;
                        //if (dataTable.Rows[i][1].ToString() == "")
                        //{
                        //    errs = "Asset Id must not be empty ";
                        //}
                        //else
                        //{
                        //    exceltxt = dataTable.Rows[i][1].ToString();
                        //    status = Model.GetAssetId(dataTable.Rows[i]["Asset Id"].ToString());

                        //    if (status.ToString() == "Exists" || status.ToString() == "")
                        //    {
                        //        if (errs == "")
                        //        {
                        //            errs = "Asset Id Was Not Found";
                        //        }
                        //        else
                        //        {
                        //            errs = errs + "," + "Asset Id Was Not Found";
                        //        }
                        //    }

                        //}
                        //Muthu

                        if (strRowCommaSeparated.Contains(assetdata) == true)
                        {

                            errs = "Duplicate Asset id Found";
                        }
                        else
                        {


                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty ";
                            }
                            else
                            {
                                exceltxt = dataTable.Rows[i][1].ToString();
                                status = sal.GetSaleStatusexcel(assetdata, exceltxt, "AssetId");
                                if (status == "Free")
                                {
                                    status = sal.GetSaleStatusexcel(assetdata, exceltxt, "AssetIdProcess");
                                    if (status == "Free")
                                    {
                                        strRowCommaSeparated += strRowCommaSeparated == "" ? assetdata : ("," + assetdata);
                                        sagrpId1 = Tfr.GetGroupGid(dataTable.Rows[0][1].ToString());
                                        sagrpId2 = Tfr.GetGroupGid(assetdata);
                                        if (sagrpId2 == sagrpId1 || sagrpId2 == "")
                                        {
                                            sagrpId1 = sal.GetGrpCountSA(sagrpId1);
                                            if (sagrpId1 != "0")
                                                if (Convert.ToInt32(sagrpId1) != dataTable.Rows.Count)
                                                {
                                                    if (errs == "")
                                                    {
                                                        errs = "All assets in the Group are not provided.";
                                                    }
                                                    else
                                                    {
                                                        errs = errs + "," + "All assets in the Group are not Provided.";
                                                    }
                                                }
                                        }

                                        else
                                        {
                                            if (errs == "")
                                            {
                                                errs = "All Asset Id must be of Same Group.";
                                            }
                                            else
                                            {
                                                errs = errs + "," + "All Asset ID must be of Same Group.";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (errs == "")
                                        {
                                            errs = status;
                                        }
                                        else
                                        {
                                            errs = errs + "," + status;
                                        }
                                    }
                                }
                                else
                                {
                                    if (errs == "")
                                    {
                                        errs = status;
                                    }
                                    else
                                    {
                                        errs = errs + "," + status;
                                    }
                                }
                            }


                            //*******************************************************Muthu




                            if (dataTable.Rows[i][2].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch should not be empty.";
                                    //AssetCodestatus = "notexists";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch should not be empty.";
                                    //AssetCodestatus = "notexists";
                                }
                            }
                            else
                            {
                                string exceltext = dataTable.Rows[i][2].ToString();
                                // string assetdata = dataTable.Rows[i][1].ToString();
                                status = Model1.GetStatusexcel(assetdata, exceltext, "AssetLoc");
                                //AssetCodestatus = status;
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Asset belongs to different branch.";

                                    }
                                    else
                                    {
                                        errs = errs + "," + "Asset belongs to different branch.";

                                    }
                                }
                            }


                            string format = "dd-mm-yyyy";
                            DateTime dateTime;

                            if (dataTable.Rows[i][3].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Date of Impairment should not be Empty.";
                                }
                                else
                                {
                                    errs = errs + "," + "Date of Impairment should not be Empty.";
                                }
                            }
                            else if (DateTime.TryParse(dataTable.Rows[i][3].ToString(),
                                // format,
                                //    System.Globalization.CultureInfo.InvariantCulture,
                                // System.Globalization.DateTimeStyles.None,
                                     out dateTime))
                            {
                                string exceltext = dataTable.Rows[i][3].ToString();
                                DateTime date = Convert.ToDateTime(exceltext);
                                DateTime year = DateTime.Today;
                                string finyear = ifr.toGetFincialyear(date);
                                string fintodayyear = ifr.toGetFincialyear();
                                string[] arrYer = finyear.Split('-');
                                if (finyear != fintodayyear)
                                {

                                    if (errs == "")
                                    {
                                        errs = "Date does not belong to this financial year.";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Date does not belong to this financial year.";
                                    }
                                }
                            }
                            else
                            {
                                if (errs == "")
                                {
                                    errs = "Invalid date format.";
                                }
                                else
                                {
                                    errs = errs + "," + "Invalid date format.";
                                }
                            }


                            //Muthu
                            if (dataTable.Rows[i][4].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Inward No must not be empty.";
                                }
                                else
                                {
                                    errs = errs + "," + "Inward No must not be empty.";
                                }
                            }

                            else
                            {
                                exceltxt = dataTable.Rows[i][4].ToString();
                                // string assetdata = dataTable.Rows[i][1].ToString(); //Muthu
                                status = sal.GetSaleStatusexcel(assetdata, exceltxt, "Inward No");
                                if (status != "Free")
                                {
                                    if (errs == "")
                                    {
                                        errs = status;
                                    }
                                    else
                                    {
                                        errs = errs + "," + status;
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
                ViewBag.vbvalid = "Total No of Valid Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel file :" + totalrecord;
                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }


        public FileResult DownloadDocument(string id)
        {
            try
            {
                string filename = id;
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var path = Path.Combine(CmnFilePath, filename);

                //  var path = Path.Combine(@"C:\Temp\", filename);
                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult == "Fail")
                {
                    return File("", "");
                }
                else
                {
                    byte[] filebyte = Convert.FromBase64String(apiresult);
                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return File("", "");
            }
        }

        [HttpGet]
        public PartialViewResult ToAttach(int id, string viewfor)
        {
            Ifams_Modelx model = new Ifams_Modelx();
            Ifams_Propertyx.ImpairmentsModel Attlist = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";
                }
                if (id != 0)
                {
                    Attlist._gid = id;
                    Attlist._attach_list = model.Getattachment(id.ToString(), model.GetRef("IOA"), model.GetAttachType());
                }
                if (Attlist._attach_list.Count == 0)
                    ViewBag.Message = "No records found";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }

            return PartialView(Attlist);
        }
        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {
            string filename = "";
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["_upld_fname"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string; ;

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        filename = objCmnFunctions.GetSequenceNoFam("IOAA");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        //  var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
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
                        Session["filename"] = filename;
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        message = filename;
                    }

                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        [HttpPost]
        public JsonResult Save_attach(Ifams_Propertyx.ImpairmentsModel model)
        {
            Ifams_Modelx obj = new Ifams_Modelx();
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = obj.save_attachment(model);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult toaAttachgrid(int id, string viewfor)
        {
            Ifams_Modelx model = new Ifams_Modelx();
            Ifams_Propertyx.ImpairmentsModel Attlist = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";
                }
                if (id != 0)
                {
                    Attlist._gid = id;
                    Attlist._attach_list = model.Getattachment(id.ToString(), model.GetRef("IOA"), model.GetAttachType());
                }
                if (Attlist._attach_list.Count == 0)
                    ViewBag.Message = "No records found";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(Attlist);
        }

        public JsonResult ImpairAttachDelete(int id)
        {
            string result = "";
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                string[,] codes = new string[,]
	                {
                         {"attachment_isremoved", "Y"}	            
                    };
                string[,] whrcol = new string[,]
	                    {
                          {"attachment_gid", id.ToString ()}
                        };
                result = delecomm.DeleteCommon(codes, whrcol, "iem_trn_tattachment");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public PartialViewResult Impairlocaldetails(string ddlSelectsheet)
        {
            Session["err"] = "flag";
            return PartialView();
        }


        [HttpPost]
        public JsonResult Impairlocaldetails()
        {
            string check = "";
            try
            {
                Session["err"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["maindatatable"];

                string filename = Session["filename"].ToString();
                check = imp.InsertImpair(uploadedData, filename).ToString();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally { }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckFileExists(string id)
        {
            string result = "0";
            try
            {
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var localpath = Path.Combine(CmnFilePath, id);
                //  var localpath = Path.Combine(@"C:\temp\", id);
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


        public JsonResult DeleteImpair(string id)
        {
            string Msg = "";

            Ifams_Modelx Model = new Ifams_Modelx();

            try
            {
                Msg = Model.DeleteImpair(id);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        DataModel ifr = new DataModel();
        public ActionResult RevokeSummary(string gid)
        {
            try
            {
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }
        public JsonResult Revoke(string gid)
        {
            string check = string.Empty;
            string checker = string.Empty;
            checker = objCmnFunctions.authorize("226");
            try
            {
                if (checker == "Success")
                {
                    check = imp.RevokeImpair(gid);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check,checker, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RevokeSummary1()
        {
            List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> obj = new List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel>();
            try
            {
                obj = imp.GetImpDetail("APPROVED").ToList();
                Session["ReImpairExceldownload"] = imp.GetImpDetail("APPROVED").ToList();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        public ActionResult ImpairExceldownload()
        {

            List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> cList;
            cList = (List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel>)Session["ImpairExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Impairment Off Number");
            dt.Columns.Add("No of Records");
            dt.Columns.Add("Impairment Off Date");
            dt.Columns.Add("Impairment Off Status");
            dt.Columns.Add("Uploded File");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i]._imp_number.ToString()
                , cList[i]._no_records.ToString()
                , cList[i]._imp_date.ToString()
                , cList[i].impstatus.ToString()
                , cList[i]._upld_fname.ToString());
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Impairment Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

        public ActionResult ReImpairExceldownload(List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> cList)
        {

            // List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> cList;
            cList = (List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel>)Session["ReImpairExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Impairment Off Number");
            dt.Columns.Add("No of Records");
            dt.Columns.Add("Impairment Off Date");
            dt.Columns.Add("Impairment Off Status");

            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i]._imp_number.ToString()
                , cList[i]._no_records.ToString()
                , cList[i]._imp_date.ToString()
                , cList[i].impstatus.ToString()
               );
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Revoke Impairment Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
