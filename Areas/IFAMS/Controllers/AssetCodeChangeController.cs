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
    public class assetcodechangeController : Controller
    {
        //
        // GET: /IFAMS/assetcodechange/

        //public ActionResult AssetCodeChange()
        //{
        //    return View();
        //}

        private AssetCCRepository ccfr;
        private IRepository Tfr;
        private IfamsRepository_M safr;
        private CmnFunctions objcmnfunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public assetcodechangeController() : this(new IfamsAssetCCDataModel(), new DataModel(),new IfamsDataModel_M()) { }

        public assetcodechangeController(AssetCCRepository objModel, IRepository obTOA, IfamsRepository_M obSOA)
        {
            ccfr = objModel;
            Tfr = obTOA;
            safr = obSOA;
        }

        [HttpGet]
        public ActionResult AssetCodeChange()
        {
            try
            {
                //AssetCCModel records = new AssetCCModel();
                Session["accTransferno"] = null;
                Session["accStatus"] = null;
                Session["accErrdatatable"] = null;
                Session["accmaindatatable"] = null;
                Session["accsupplierattmtfileexcel"] = null;
                Session["accorginal"] = null;
                Session["accerr"] = null;
                Session["accTempdataset"] = null;
                Session["accfilename"] = null;
                Session["accTempdatatable"] = null;
                Session["accgid"] = null;
                //records.CodeModel = Enumerable.Empty<AssetCCModel>().ToList<AssetCCModel>();
                ////string success = ccfr.getTheUser("ACCMKR");
                ////ViewBag.success = success;
                ////if (success == "success")
                ////{
                ////    records.CodeModel = ccfr.GetAccDetail(Convert.ToString(objcmnfunc.GetLoginUserGid())).ToList();
                ////    if (records.CodeModel.Count == 0) { ViewBag.Message = "No Records Found"; }
                ////    return View(records);
                ////}
                ////else
                ////{
                ////    records.CodeModel = ccfr.GetAccDetail(Convert.ToString(objcmnfunc.GetLoginUserGid())).ToList();
                ////    if (records.CodeModel.Count == 0) { ViewBag.Message = "No Records Found"; }
                ////    return View(records);

                ////}
               
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
               // return View();
            }
            finally
            {
            }
            return View();
        }



        //[HttpGet]
        //public PartialViewResult AssetCodeChange()
        //{
        //    try
        //    {
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult AssetCCdownloadexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Old Asset Code");
                dtnew.Columns.Add("New Asset Code");
                dtnew.Columns.Add("Remarks");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "AssetCodeChange_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AssetCodeChange_Template.xlsx");
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

        //public ActionResult AssetCCdownloadexcel()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("SNo");
        //    dt.Columns.Add("Asset Id");
        //    dt.Columns.Add("Old Asset Code");
        //    dt.Columns.Add("New Asset Code");
        //    dt.Columns.Add("Remarks");
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("Content-disposition", string.Format("attachment; filename={0}", "AssetCodeChange_Template.xls"));
        //    Response.ContentType = "application/ms-excel";
        //    DataTable dt1 = dt;
        //    string str = string.Empty;
        //    foreach (DataColumn dt1col in dt1.Columns)
        //    {
        //        Response.Write(str + dt1col.ColumnName);
        //        str = "\t";
        //    }
        //    Response.Write("\n");
        //    foreach (DataRow dr in dt1.Rows)
        //    {
        //        str = "";
        //        for (int j = 0; j < dt1.Columns.Count; j++)
        //        {
        //            Response.Write(str + Convert.ToString(dr[j]));
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //    }
        //    Response.End();
        //    return View();
        //}

        public JsonResult AccMakerupdate(AssetCCModel obj)
        {
            string mag = "";
            string strcols = "";
            string[] strColArr;
            string Maker = string.Empty;
            Maker = objcmnfunc.authorize("254");
            try
            {
                if (Maker == "Success")
                {
                    DataSet resultacc = new DataSet();
                    resultacc = (DataSet)Session["accTempdataset"];
                    var dataTable = new DataTable();
                    dataTable = resultacc.Tables[obj.accSheetname.ToString().Trim()];
                    foreach (DataColumn dtColumn in dataTable.Columns)
                    {
                        strcols = strcols + dtColumn.ColumnName.Trim() + ":";
                    }
                    strcols = strcols.Substring(0, strcols.Length - 1);
                    strColArr = strcols.Split(':');
                    if (strColArr[0].ToString() == "SNo"
                        && strColArr[1].ToString() == "Asset Id"
                        && strColArr[2].ToString() == "Old Asset Code"
                        && strColArr[3].ToString() == "New Asset Code"
                        && strColArr[4].ToString() == "Remarks"
                        && strColArr.Count().ToString() == "5")
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
                                    && dataTable.Rows[i]["Old Asset Code"].ToString() == ""
                                    && dataTable.Rows[i]["New Asset Code"].ToString() == ""
                                    && dataTable.Rows[i]["Remarks"].ToString() == ""
                                    )
                                {
                                    dataTable.Rows[i].Delete();
                                    dataTable.AcceptChanges();
                                    count--;
                                    i--;
                                }
                            }
                            Session["accTempdatatable"] = dataTable;
                        }
                    }
                }
                else
                {
                    mag = "Unauthorized User!";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public PartialViewResult AccUploadstatus(string ddlSheetname)
        {
            try
            {
                if (Session["accTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["accTempdatatable"];
                    accdatasupload(errdataval);
                    errdataval = (DataTable)Session["accErrdatatable"];
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

        public void accdatasupload(DataTable dataTable)
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
                                status = ccfr.GetStatusExcel(assetdata, exceltext, "AssetIdCC");
                                if (status == "Free")
                                {
                                    status = ccfr.GetStatusExcel(assetdata, exceltext, "AssetIdCCProcess");
                                    if (status == "Free")
                                    {
                                        spRowCommaSeptated += spRowCommaSeptated == "" ? assetdata : ("," + assetdata);
                                        //string asset = dataTable.Rows[0][1].ToString();
                                        spgrpId1 = Tfr.GetGroupGid(dataTable.Rows[0][1].ToString());
                                       // spgrpId1 = Tfr.GetGroupGid(asset);
                                        spgrpId2 = Tfr.GetGroupGid(assetdata);
                                        if (spgrpId2 == spgrpId1 || spgrpId2 == "")
                                        {
                                            spgrpId1 = safr.GetGrpCountSA(spgrpId1);
                                            if (spgrpId1 != "0")
                                                if (Convert.ToInt32(spgrpId1) != dataTable.Rows.Count)
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
                                                errs = errs + "," + "All Asset ID must be of same Group.";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (errs == "")
                                        {
                                            errs = status;
                                        }
                                        else{
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
                        }
                        //                errs = "Asset Id Was Not Found";
                        //            }
                        //            else
                        //            {
                        //                errs = errs + "," + "Asset Id Was Not Found";
                        //            }
                        //        }
                        //    }
                        //}

                        //if (duplicateasset.Contains(assetdata) == true)
                        //{
                        //    errs = "Duplicate Asset id Found";
                        //}
                        //else
                        //{
                            if (dataTable.Rows[i][2].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Old Asset Code must not be empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Old Asset Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][2].ToString();
                                status = ccfr.GetStatusExcel(assetdata, exceltext, "AssetcodeOld");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Old Asset Code Was Not Found";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Old Asset Code Was Not Found";
                                    }
                                }
                            }
                       // }


                        //if (duplicateasset.Contains(assetdata) == true)
                        //{
                        //    errs = "Duplicate Asset id Found";
                        //}
                        //else
                        //{
                            if (dataTable.Rows[i][3].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "New Asset Code must not be empty";
                                }
                                else
                                {
                                    errs = errs + "," + "New Asset Code must not be empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][3].ToString();
                                status = ccfr.GetStatusExcel(assetdata, exceltext, "Assetcode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "New Asset Code Was Not Found";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "New Asset Code Was Not Found";
                                    }
                                }
                            }
                        //}

                            if (dataTable.Rows[i][3] == dataTable.Rows[i][2]) {

                                if (errs == "")
                                {
                                    errs = "New Asset Code and old asset code must not be same";
                                }
                                else
                                {
                                    errs = errs + "," + "New Asset Code and old asset code must not be same";
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
                Session["accErrdatatable"] = Errdatatable;
                Session["accmaindatatable"] = maindatatable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }

        public void ACCUploadFilesnew()
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
                        Session["accsupplierattmtfileexcel"] = hpf;
                        Session["accorginal"] = hpf.FileName;
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
        public async Task<JsonResult> ACCUploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["accsupplierattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["accsupplierattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("ACC-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["accfilename"] = n + "_" + Extension;
                    path1 = @"C:\temp\" + n + " _" + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<AccSheetData> objparent = new List<AccSheetData>();
                AccSheetData objModel1;
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
                    objModel1 = new AccSheetData();
                    objModel1.accSheetid = count;
                    objModel1.accSheetname = sheets.ToString();
                    objparent.Add(objModel1);
                }


                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel1 = new AccSheetData();
                    objModel1.accSheetid = count;
                    objModel1.accSheetname = sheets.ToString();
                    objparent.Add(objModel1);
                }


                Session["accTempdataset"] = result1;
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
        public PartialViewResult acclocaldetails(string ddlSelectsheet)
        {
            Session["accerr"] = "flag";
            return PartialView();
        }

        public JsonResult acclocaldetails()
        {

            string acccheck = "";
            try
            {
                Session["accerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["accmaindatatable"];
                string filename = Session["accfilename"].ToString();
                acccheck = ccfr.UpdateACC(uploadedData, filename).ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(acccheck, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Errdownloadsexcel()
        {
            try
            {
                Session["accerr"] = "";
                DataTable dtnew = (DataTable)Session["accErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["accmaindatatable"];
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
                    wb.Worksheets.Add(dt, "AssetCodeChange_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AssetCodeChange_Errorlog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "AssetCC_Errorlog.xls"));
                //Response.ContentType = "appliction/ms-excel";
                //DataTable dt = dtnew;
                //string str = string.Empty;
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write(str + dtcol.ColumnName);
                //    str = "\t";
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + Convert.ToString(dr[j]));
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
            return View();
        }


    }
}
