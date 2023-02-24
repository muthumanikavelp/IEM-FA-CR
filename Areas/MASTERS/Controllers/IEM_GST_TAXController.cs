using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Data;
using System.IO;
using Excel;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using IEM.Common;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.IFAMS.Models;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{

     [NoDirectAccess]
    public class IEM_GST_TAXController : Controller
    {
        //
        // GET: /MASTERS/IEM_GST_TAX/
        ErrorLog objErrorLog = new ErrorLog();
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        private iem_mst_tgsttax Taxgst;
        string result;
        public IEM_GST_TAXController() :
            this(new IEM_GST_TAX()) { }
        public IEM_GST_TAXController(iem_mst_tgsttax objlis)
        {
            Taxgst = objlis;
        }

        public ActionResult Index()
        {
            List<Entity_taxgst> record = new List<Entity_taxgst>();
            string rolelist = Taxgst.GetRolegroup();
            ViewBag.rolegrop = rolelist;
            Session["rolegroup"] = rolelist;
            if (rolelist == "TXGSTMKR")
            {
                record = Taxgst.getgsttax(rolelist).ToList();
            }
            if (rolelist == "TXGSTCHK")
            {
                record = Taxgst.getgsttax(rolelist).ToList();
            }
            if (record.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }

            return View(record);
        }
        [HttpPost]
        public ActionResult Index(string hsncode = null, string command = null)
        {
            //List<EntityGstHsn> records = new List<EntityGstHsn>();
            List<Entity_taxgst> record = new List<Entity_taxgst>();
            Session["records"] = "";
            if (command == "Search")
            {
                string rolelist = Taxgst.GetRolegroup();
                ViewBag.Rolegrop = rolelist;
                Session["rolegroup"] = rolelist;
                if (rolelist == "TXGSTMKR")
                {
                    record = Taxgst.getgsttaxbyHSN("TXGSTMKRByHSN", hsncode).ToList();
                }
                if (rolelist == "TXGSTCHK")
                {
                    record = Taxgst.getgsttax(rolelist).ToList();
                }
                if (record.Count == 0)
                {
                    ViewBag.record = "No Records Found";
                }
            }
            return View(record);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            Entity_taxgst Typemodel = new Entity_taxgst();
            Typemodel.GetTaxsubtype = new SelectList(Taxgst.Gettaxsubtype(), "taxsubtypeid", "taxsubtype_code");
            Typemodel.Getstatelist = new SelectList(Taxgst.GetStatecode(), "stategid", "State_code");
            Typemodel.GetHsncod = new SelectList(Taxgst.GetHsncode(), "hsnid", "Hsn_code");
            Typemodel.Getgllist = new SelectList(Taxgst.GetGLdtl(), "Taxgl_id", "Glno");
            Typemodel.GetglCr = new SelectList(Taxgst.GetCLtl(), "inputcredit_gid", "CrGlno");
            Typemodel.GetglRe = new SelectList(Taxgst.GetRLdtl(), "reversecharge_glid", "RcGlno");
            return PartialView(Typemodel);
        }
        [HttpPost]
        public JsonResult Gethsndesc(Entity_taxgst Exhsn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    result = Taxgst.Gethsndesc(Exhsn);
                    //Taxgst.Gethsndesc(Exhsn);

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult savegst(Entity_taxgst savegstdtls)
        {
            try
            {
                result = Taxgst.savegst(savegstdtls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            try
            {
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                if (viewfor == "chkedit")
                {
                    ViewBag.viewfor = "chkedit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                else if (viewfor == "delete")
                {
                    ViewBag.viewfor = "delete";
                }
                else if (viewfor == "reject")
                {
                    ViewBag.viewfor = "reject";
                }
                Entity_taxgst Objdata = Taxgst.Geteditdel(id);
                Objdata.GetTaxsubtype = new SelectList(Taxgst.Gettaxsubtype(), "taxsubtypeid", "taxsubtype_code", Objdata.Taxsubtypeid);
                Objdata.Getstatelist = new SelectList(Taxgst.GetStatecode(), "stategid", "State_code", Objdata.stategid);
                Objdata.GetHsncod = new SelectList(Taxgst.GetHsncode(), "hsnid", "Hsn_code", Objdata.hsnid);
                //Objdata.Getgllist = new SelectList(Taxgst.GetGLdtl(), "Taxgl_id", "Glno", Objdata.Taxgl_id);
                Objdata.GetglCr = new SelectList(Taxgst.GetCLtl(), "inputcredit_gid", "CrGlno", Objdata.inputcredit_gid);
                Objdata.GetglRe = new SelectList(Taxgst.GetRLdtl(), "reversecharge_glid", "RcGlno", Objdata.reversecharge_glid);
                return PartialView(Objdata);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult updategst(Entity_taxgst Entgsttax)
        {
            try
            {
                result = Taxgst.Updategsttax(Entgsttax);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Deletegsttax(Entity_taxgst Entgsttax)
        {
            try
            {
                result = Taxgst.Deletegsttax(Entgsttax);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Created by pandiyaraj
        [HttpPost]
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["HoldFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }

        public ActionResult downloadsexcel()
        {
            Session["err"] = "";
            DataTable dtnew = new DataTable();


            string dn = "0";
            string xmlstr = "";
            if (Session["XMLdata"] != null)
            {
                xmlstr = Session["XMLdata"] as String;
                dtnew = Taxgst.uploadGSTTAX(xmlstr, "ss");
                dn = dtnew.Rows.Count.ToString();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "LocalConveyance.xls"));
                Response.ContentType = "application/vnd.ms-excel";
                DataTable dt = dtnew;
                dn = dtnew.Rows.Count.ToString();
            }
            return Json(dn, JsonRequestBehavior.AllowGet);
        }


        public void UploadFilesnew()
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
                        Session["supplierattmtfileexcel"] = hpf;
                        Session["saoriginal"] = hpf.FileName;
                    }
                }
                // return Json(filename);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json("Uplopad failed");
            }
        }



        #region Change void to json
        /*   public void UploadFileslocal()
        {
            try
            {
                foreach (string filenew in Request.Files)
                {
                    var fileContent = Request.Files[filenew];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[filenew] as HttpPostedFileBase;
                        TempData["_attFileupload"] = hpf;

                        HttpPostedFileBase upload = TempData["_attFileupload"] as HttpPostedFileBase;
                        TempData.Remove("_attFileupload");
                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        string GetExten = Path.GetExtension(upload.FileName);
                        TempData["exten"] = "Y";
                        if (GetExten == ".xls" || GetExten == ".xlsx")
                        {
                            TempData["exten"] = "X";
                            if (GetExten == ".xls")
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (GetExten == ".xlsx")
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }

                            reader.IsFirstRowAsColumnNames = true;
                            DataSet result = reader.AsDataSet();
                            String xmlstr = "";
                            reader.Close();
                            DataTable dt = new DataTable();
                            dt = result.Tables[0];
                            Session["errors"] = "X";
                            if (dt.TableName.Trim() == "TaxGST_Upload-Format")
                            {
                                if (dt.Columns.Count == 12)
                                {
                                    xmlstr = result.GetXml();
                                    xmlstr = ConvertDatatableToXML(dt);
                                    string modifiedXml = Regex.Replace(xmlstr,
                                                    @"<effective_date>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</effective_date>",
                                                    @"<effective_date>${month}/${date}/${year}</effective_date>",
                                                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                                    xmlstr = Regex.Replace(modifiedXml,
                                                    @"<Effective_todate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Effective_todate>",
                                                    @"<Effective_todate>${month}/${date}/${year}</Effective_todate>",
                                                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                                    Session["XMLdata"] = xmlstr;
                                    // int index = upload.FileName.LastIndexOf('.');
                                    // string onyName = upload.FileName.Substring(0, index);
                                    dt = Taxgst.uploadGSTTAX(xmlstr, Path.GetFileNameWithoutExtension(upload.FileName));
                                    dt.TableName = "Error List";
                                    Session["errortable"] = dt;
                                    Session["exceltable"] = result;
                                }
                                else
                                {
                                    Session["errors"] = "Invalid Excel Column Format";
                                }
                            }
                            else
                            {
                                Session["errors"] = "Invalid Excel sheet, Sheet name should be TaxGST_Upload-Format";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }*/
        #endregion


        public JsonResult UploadFileslocal()
        {
            string Data = "";
            try
            {
                //foreach (string filenew in Request.Files)
                //{
                //    var fileContent = Request.Files[filenew];

                //    if (fileContent != null && fileContent.ContentLength > 0)
                //    {
                       // HttpPostedFileBase hpf = Request.Files[filenew] as HttpPostedFileBase;
                       // TempData["_attFileupload"] = hpf;

                      //  HttpPostedFileBase upload = TempData["_attFileupload"] as HttpPostedFileBase;
                        HttpPostedFileBase upload = Session["supplierattmtfileexcel"] as HttpPostedFileBase;
                        //TempData.Remove("_attFileupload");
                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;
                        //string GetExten = Path.GetExtension(upload.FileName); Session["saoriginal"]
                        string GetExten = Path.GetExtension(upload.FileName);
                        TempData["exten"] = "Y";
                        if (GetExten == ".xls" || GetExten == ".xlsx")
                        {
                            TempData["exten"] = "X";
                            if (GetExten == ".xls")
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (GetExten == ".xlsx")
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }

                            reader.IsFirstRowAsColumnNames = true;
                            DataSet result = reader.AsDataSet();
                            String xmlstr = "";
                            reader.Close();
                            DataTable dt = new DataTable();
                            dt = result.Tables[0];
                            Session["errors"] = "X";
                            if (dt.TableName.Trim() == "TaxGST_Upload-Format")
                            {
                                if (dt.Columns.Count == 12)
                                {
                                    xmlstr = result.GetXml();
                                    xmlstr = ConvertDatatableToXML(dt);
                                    string modifiedXml = Regex.Replace(xmlstr,
                                                    @"<effective_date>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</effective_date>",
                                                    @"<effective_date>${month}/${date}/${year}</effective_date>",
                                                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                                    xmlstr = Regex.Replace(modifiedXml,
                                                    @"<Effective_todate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Effective_todate>",
                                                    @"<Effective_todate>${month}/${date}/${year}</Effective_todate>",
                                                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                                    Session["XMLdata"] = xmlstr;
                                    // int index = upload.FileName.LastIndexOf('.');
                                    // string onyName = upload.FileName.Substring(0, index);
                                    dt = Taxgst.uploadGSTTAX(xmlstr, Path.GetFileNameWithoutExtension(upload.FileName));
                                    dt.TableName = "Error List";
                                    Data = JsonConvert.SerializeObject(dt);
                                    Session["errortable"] = dt;
                                    Session["exceltable"] = result;
                                }
                                else
                                {
                                    Session["errors"] = "Invalid Excel Column Format";
                                }
                            }
                            else
                            {
                                Session["errors"] = "Invalid Excel sheet, Sheet name should be TaxGST_Upload-Format";
                            }
                        }
                   // }
               // }
                
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Data = JsonConvert.SerializeObject(ex.Message.ToString());
                return Json(Data, JsonRequestBehavior.AllowGet);
            }
            return Json(Data, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public ActionResult ErrorPage()
        {
            List<Entity_taxgst> record = new List<Entity_taxgst>();
            record = Taxgst.uploadlist().ToList();
            return View(record);
        }

        [HttpGet]
        public PartialViewResult bulkcreate(int id, string type)
        {
            // int id = 160;
            string roles = (String)Session["rolegroup"];
            ViewBag.roles = roles;
            if (id <= 0)
            {
                //if (TempData["_attFileupload"] != null)
                //{
                //HttpPostedFileBase upload = TempData["_attFileupload"] as HttpPostedFileBase;
                //TempData.Remove("_attFileupload");
                //Stream stream = upload.InputStream;
                //IExcelDataReader reader = null;
                //string GetExten = Path.GetExtension(upload.FileName); 
                //if (GetExten == ".xls")
                //{
                //    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                //}
                //else if (GetExten == ".xlsx")
                //{
                //    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //}
                //else
                //{
                //    ModelState.AddModelError("File", "This file format is not supported");
                //    return PartialView();
                //}
                //reader.IsFirstRowAsColumnNames = true; 
                //DataSet result = reader.AsDataSet();
                //String xmlstr = "";
                //reader.Close();
                //DataTable dt = new DataTable();
                //dt = result.Tables[0];
                //xmlstr = result.GetXml();
                //xmlstr = ConvertDatatableToXML(dt);
                //string modifiedXml = Regex.Replace(xmlstr,
                //                @"<effective_date>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</effective_date>",
                //                @"<effective_date>${month}/${date}/${year}</effective_date>",
                //                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                //xmlstr = Regex.Replace(modifiedXml,
                //                @"<Effective_todate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Effective_todate>",
                //                @"<Effective_todate>${month}/${date}/${year}</Effective_todate>",
                //                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                //Session["XMLdata"] = xmlstr;
                //// int index = upload.FileName.LastIndexOf('.');
                //// string onyName = upload.FileName.Substring(0, index);
                //dt = Taxgst.uploadGSTTAX(xmlstr, Path.GetFileNameWithoutExtension(upload.FileName));
                //dt.TableName = "Error List";
                //Session["errortable"] = dt;

                DataSet result = new DataSet();
                DataTable dt = new DataTable();
              //  string exten = Convert.ToString(TempData["exten"]);
                ViewBag.errordis = "";
              /*  if (exten == "Y")
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return PartialView();
                }*/
                if (Session["errors"] != "X" && Session["errors"]!=null)
                {
                    string sd = Session["errors"].ToString();
                    ViewBag.errordis = Session["errors"].ToString();
                    ModelState.AddModelError("File", sd);
                    return PartialView();
                }
                dt = (DataTable)Session["errortable"];
                if (Session["errortable"] != null)
                {
                    result = (DataSet)Session["exceltable"];
                    ViewBag.error = "V";
                    dt = result.Tables[0];
                    dt.TableName = "Upload List";
                }
                ModelState.AddModelError("File", "Success");
                return PartialView(dt);
            }
            else if (id > 0)
            {
                DataSet ds = new DataSet();
                // int uploadgid = 160;
                Session["uploadgid"] = id;
                ds = Taxgst.forexcel(id, "G");
                ViewBag.type = type;
                ViewBag.error = "V";
                return PartialView(ds.Tables[0]);

            }
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult bulkcreate()
        {

            return PartialView();
        }
        public ActionResult downloadsErrorexcel()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["errortable"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=GST_TAX_Upload_Error_List.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
        public ActionResult downloadTemplets()
        {
            DataSet ds = new DataSet();
            int ss = 0;
            ds = Taxgst.forexcel(ss, "A");
            ds.Tables[0].TableName = "TaxGST_Upload-Format";
            ds.Tables[1].TableName = "Tax Subtype Code";
            ds.Tables[2].TableName = "State Master";
            ds.Tables[3].TableName = "HSN Code";
            ds.Tables[4].TableName = "GL_No Master";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=GST_TAX_Upload.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult saveuploadgst(Entity_taxgst Entgsttax)
        {
            try
            {
                Entgsttax.uploadgid = (Int32)Session["uploadgid"];
                result = Taxgst.Updateuploadgsttax(Entgsttax);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult Historypage()
        {
            //List<Entity_taxgst> record = new List<Entity_taxgst>();
            //record = Taxgst.historylist().ToList();
            //return View(record);
            return View();
        }

        [HttpGet]
        public PartialViewResult AuditTrail(int taxsubtypeid, int hsnid, int stateid)
        {
            Entity_taxgst gstlist = new Entity_taxgst();
            try
            {
                gstlist.GSTAuditList = Taxgst.GetAuditTrialList(taxsubtypeid, hsnid, stateid).ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return PartialView(gstlist);

        }
        //Created by pandiyaraj
    }
}
