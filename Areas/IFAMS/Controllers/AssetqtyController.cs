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
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetqtyController : Controller
    {
        //
        // GET: /IFAMS/Assetqty/

        public ActionResult Index()
        {
            return View();
        }

        private AssetqtyRepository_M qtyr;
        private IRepository Tfr;
        private IfamsRepository_M safr;
        private CmnFunctions objcmnfunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public AssetqtyController() : this(new IfamsAssetqtyDataModel_M(), new DataModel(), new IfamsDataModel_M()) { }
        public AssetqtyController(AssetqtyRepository_M objModel, IRepository obTOA, IfamsRepository_M obSOA)
        {
            qtyr = objModel;
            Tfr = obTOA;
            safr = obSOA;
        }

        [HttpGet]
        public ActionResult AssetqtySerialno()
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

        public ActionResult Assetqtydownloadexcel()
        {

            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Asset Quantity");
                //dtnew.Columns.Add("Remarks"); 
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "AssetQty_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AssetQty_Template.xlsx");
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

        public ActionResult AssetSerdownloadexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Asset Serialno");
                //dtnew.Columns.Add("Remarks"); 
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "AssetSerialno_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AssetSerialno_Template.xlsx");
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
        public JsonResult AqtyUpdate(AssetqtyModel obj)
        {
            string mag = "";
            string strcols = "";
            string[] strColArr;
            try
            {
                DataSet resultacc = new DataSet();
                resultacc = (DataSet)Session["aqtyTempdataset"];
                var dataTable = new DataTable();
                dataTable = resultacc.Tables[obj.aqtySheetname.ToString().Trim()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strcols = strcols + dtColumn.ColumnName.Trim() + ":";
                }
                strcols = strcols.Substring(0, strcols.Length - 1);
                strColArr = strcols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Asset Quantity"
                    //&& strColArr[3].ToString() == "New Asset Code"
                    //&& strColArr[4].ToString() == "Remarks"
                    && strColArr.Count().ToString() == "3")
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
                                && dataTable.Rows[i]["Asset Id"].ToString() == ""
                                && dataTable.Rows[i]["Asset Quantity"].ToString() == ""
                                //&& dataTable.Rows[i]["New Asset Code"].ToString() == ""
                                //&& dataTable.Rows[i]["Remarks"].ToString() == ""
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




        public JsonResult ASerNoUpdate(AssetqtyModel obj)
        {
            string mag = "";
            string strcols = "";
            string[] strColArr;
            try
            {
                DataSet resultacc = new DataSet();
                resultacc = (DataSet)Session["aqtyTempdataset"];
                var dataTable = new DataTable();
                dataTable = resultacc.Tables[obj.aqtySheetname.ToString().Trim()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strcols = strcols + dtColumn.ColumnName.Trim() + ":";
                }
                strcols = strcols.Substring(0, strcols.Length - 1);
                strColArr = strcols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Asset Serialno"
                    //&& strColArr[3].ToString() == "New Asset Code"
                    //&& strColArr[4].ToString() == "Remarks"
                    && strColArr.Count().ToString() == "3")
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
                                && dataTable.Rows[i]["Asset Id"].ToString() == ""
                                && dataTable.Rows[i]["Asset Serialno"].ToString() == ""
                                //&& dataTable.Rows[i]["New Asset Code"].ToString() == ""
                                //&& dataTable.Rows[i]["Remarks"].ToString() == ""
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

        //[HttpPost]
        public PartialViewResult AqtyUploadstatus(string ddlSheetname)
        {
            try
            {
                if (Session["aqtyTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["aqtyTempdatatable"];
                    aqtydatasupload(errdataval);
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

        public PartialViewResult ASerUploadstatus(string ddlSheetname)
        {
            try
            {
                if (Session["aqtyTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["aqtyTempdatatable"];
                    aSerdatasupload(errdataval);
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


        public void aqtydatasupload(DataTable dataTable)
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
                            errs = "Duplicate Asset id Found";
                        }
                        else
                        {
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = qtyr.GetStatusExcel(assetdata, exceltext, "AssetIdCheck");
                                if (status == "FREE")
                                {
                                    status = qtyr.GetStatusExcel(assetdata, exceltext, "AssetIdqty");
                                    if (status != "FREE")
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
                                else {
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

                        if (dataTable.Rows[i][2].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Asset Quantity must not be empty";
                            }
                            else
                            {
                                errs = errs + "," + "Asset Quantity Should Not be Empty";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString();
                            if (Convert.ToInt32(exceltext) > 1)
                            {
                                if (errs == "")
                                {
                                    errs = "Asset Quantity Greater Then One";
                                }
                                else
                                {
                                    errs = errs + "," + "Asset Quantity Greater Then One";
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
        public void aSerdatasupload(DataTable dataTable)
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
                            errs = "Duplicate Asset id Found";
                        }
                        else
                        {
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = qtyr.GetStatusExcel(assetdata, exceltext, "AssetIdSer");
                                if (status != "FREE")
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

                        if (dataTable.Rows[i][2].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Asset Serial No must not be empty";
                            }
                            else
                            {
                                errs = errs + "," + "Asset Serial No Should Not be Empty";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString();
                            if (exceltext.Length > 30)
                            {
                                if (errs == "")
                                {
                                    errs = "Asset Serial No. is lengthy";
                                }
                                else
                                {
                                    errs = errs + "," + "Asset Serial No. is lengthy";
                                }
                            }
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
        public void AqtyUploadFilesnew()
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
        public async Task<JsonResult> AqtyUploadFiles()
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


        [HttpGet]
        public PartialViewResult aqtylocaldetails(string ddlSelectsheet)
        {
            Session["accerr"] = "flag";
            return PartialView();
        }

        public JsonResult aqtylocaldetails()
        {

            string acccheck = "";
            try
            {
                Session["accerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["aqtymaindatatable"];
                string filename = Session["aqtyfilename"].ToString();
                acccheck = qtyr.UpdateAQty(uploadedData, filename).ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(acccheck, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult aSerlocaldetails(string ddlSelectsheet)
        {
            Session["accerr"] = "flag";
            return PartialView();
        }

        public JsonResult aSerlocaldetails()
        {

            string acccheck = "";
            try
            {
                Session["accerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["aqtymaindatatable"];
                string filename = Session["aqtyfilename"].ToString();
                acccheck = qtyr.UpdateASer(uploadedData, filename).ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(acccheck, JsonRequestBehavior.AllowGet);
        }

        public ActionResult assetqtyErrdownloadsexcel()
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
                    wb.Worksheets.Add(dt, "AssetQty_Template");

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


        public ActionResult assetserErrdownloadsexcel()
        {
            try
            {
                Session["asererr"] = "";
                DataTable dtnew = (DataTable)Session["aserErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["asermaindatatable"];
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
                    wb.Worksheets.Add(dt, "AssetSerialNo_Template");

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
