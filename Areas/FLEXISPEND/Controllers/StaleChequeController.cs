using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class StaleChequeController : Controller
    {
        #region Decalartion
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        CmnFunctions cmnfunc = new CmnFunctions();
        #endregion

        //
        // GET: /FLEXISPEND/StaleCheque/
        public ActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStaleChequeEntry(string ChqDateFrom, string ChqDateTo, string ChqNo, string Amount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId, string PvDate, string PvNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetStaleChequeEntry(plib.ConvertDate(ChqDateFrom), plib.ConvertDate(ChqDateTo), ChqNo, Amount == null || Amount == "" ? "0" : Amount, SuppCodeId, SuppNameId, EmpCodeId, EmpNameId, plib.ConvertDate(PvDate), PvNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetStaleChequeTransactionDetails(string PVId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPVDetailsWIthECF(PVId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetStaleChequeEntry(string PvIds)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("309");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetStaleChequeEntry(PvIds, plib.LoginUserId);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                    else { return null; }
                }
                else
                {
                    Data1 = "Unauthorized User!";
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /FLEXISPEND/StaleCheque/
        public ActionResult Maker()
        {
            return View();
        }

        public ActionResult Maker_stale()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetStaleChequeMaker(string ChqDateFrom, string ChqDateTo, string ChqNo, string DocAmount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId, string DocTypeId, string DocNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetStaleChequeMaker(plib.ConvertDate(ChqDateFrom), plib.ConvertDate(ChqDateTo), ChqNo, DocAmount == null || DocAmount == "" ? "0" : DocAmount, SuppCodeId, SuppNameId, EmpCodeId, EmpNameId, DocTypeId, DocNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetStaleChequeMaker(string PvId, string BankId, string ChqBookNo, string ChqNo, string ChqDate, string Remarks)
        {
            try
            {
                string Data1 = "";
                string Maker = string.Empty;
                Maker = cmnfunc.authorize("310");
                if (Maker == "Success")
                {
                    DataSet ds = db.SetStaleChequeMaker(PvId, BankId, ChqBookNo, ChqNo, ChqDate, Remarks, plib.LoginUserId);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                    else { return null; }
                }
                else
                {
                    Data1 = "Unauthorized User!";
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /FLEXISPEND/StaleCheque/
        public ActionResult Checker_stale()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStaleChequeChecker(string ChqDateFrom, string ChqDateTo, string ChqNo, string DocAmount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId, string DocNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetStaleChequeChecker(plib.ConvertDate(ChqDateFrom), plib.ConvertDate(ChqDateTo), ChqNo, DocAmount == null || DocAmount == "" ? "0" : DocAmount, SuppCodeId, SuppNameId, EmpCodeId, EmpNameId, DocNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetStaleChequeChecker(string PvId, string Status, string Remarks)
        {
            try
            {
                string Data1 = "", AttachmentName = "";

                if (Session["_attSCCAFile"] != null)
                {
                    HttpPostedFileBase _attFile = Session["_attSCCAFile"] as HttpPostedFileBase;
                    AttachmentName = _attFile.FileName.ToString();
                }
                
                DataSet ds = db.SetStaleChequeChecker(PvId, Status, AttachmentName, Remarks, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (AttachmentName != "" && (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString() == "1"))
                    {
                        try
                        {
                            //upload the file to server.
                            HttpPostedFileBase _attFile = Session["_attSCCAFile"] as HttpPostedFileBase;
                            var stream = _attFile.InputStream;
                            string uploaderUrl = string.Format("{0}{1}", plib.StaleChqCheckerAtachment, _attFile.FileName);
                            using (var fileStream = System.IO.File.Create(uploaderUrl))
                            {
                                stream.CopyTo(fileStream);
                            }

                            //remove the temp data content.
                            Session.Remove("_attSCCAFile");
                        }
                        //catch the error exception at the time of file uploading.
                        catch { }
                    }
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //File Uploading Part
        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019
        {
            try
            {
                Session["_attSCCAFile"] = null;

                //foreach (string file in Request.Files)
                //{
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    Session["_attSCCAFile"] = hpfBase;
                //}
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            var stream = fileContent.InputStream;
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                            }
                            bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                            if (isEXE == false)
                            {
                                HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                                Session["_attSCCAFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [HttpPost]
        public JsonResult StalePaymodedetails(string PvId, string ChqNo, string Remarks,string ChqBookNo)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetStaleChequepaymodedetails(PvId, ChqNo, plib.LoginUserId, Remarks, ChqBookNo);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    //dt = ds.Tables[1];
                    //if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    //dt = ds.Tables[1];
                    //if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1,Data2,Data3 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

    }
}
