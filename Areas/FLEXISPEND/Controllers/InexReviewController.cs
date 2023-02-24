using ClosedXML.Excel;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Controllers;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class InexReviewController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        Message msg = new Message();
        DataTable dt = new DataTable();
        ForMailController mail = new ForMailController();
        #endregion

        //
        // GET: /FLEXISPEND/InexReviewInex/
        public ActionResult Summary()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchInexDetails(string InexFrom, string InexTo, string InexById, string DocAmount, string DocFrom, string DocTo, string DocTypeId, string DocNo, string EmpCodeId,
           string EmpNameId, string SuppCodeId, string SuppNameId, string  ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetInex(plib.ConvertDate(InexFrom), plib.ConvertDate(InexTo), InexById, DocAmount==""?"0":DocAmount, plib.ConvertDate(DocFrom), plib.ConvertDate(DocTo), DocTypeId, DocNo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, ViewType, plib.LoginUserId);
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

        //Update Inex Review 
        public JsonResult UpdateInexReview(string InexId, string Status, string Remarks)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetInexReview(InexId, Status, Remarks, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    //for sending mail
                    if(Status == "Approve")
                    {
                        string[] data = { };
                        DataSet inex_ecf_gid = db.GetEcfByInexID(InexId);
                        string ecf_gid = inex_ecf_gid.Tables[0].Rows[0]["inex_ecf_gid"].ToString();

                        mail.sendusermailfs("FS", "Inex Submission", ecf_gid, "S", data, "0");
                    }
                    
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Dispatch()
        {
            return View();
        }

        //Dispatch Inex Review 
        public JsonResult DispatchInex(string InexId, string DispatchTo, string BranchId, string DispatchDate, string DispatchAddress, string CourierId, string AWB_no, string Remarks)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetInexDispatch(InexId, DispatchTo, BranchId, plib.ConvertDate(DispatchDate), DispatchAddress, CourierId, AWB_no, Remarks, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //Dispatch Inex Review 
        public JsonResult DispatchReprocessInex(string InexId, string Remarks)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetInexReprocess(InexId, Remarks, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public ActionResult DocumentDetails(string id = "", string subId = "")
        {
            //checker View
            @ViewBag.InexEcfId = "";
            @ViewBag.InexEcfdet = "";

            @ViewBag.InexEcfId = id;
            @ViewBag.InexEcfdet = string.Format("{0}-{1}-{2}", id, subId, "Y");
            return View();
        }

        public ActionResult InexDetails(string id = "1")
        {
            @ViewBag.ViewType = "";

            @ViewBag.ViewType = id;
            return View();
        }

        //Get Inex Details 
        public JsonResult GetInexDetails(string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetInexDetails(ViewType, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //Get Inex Details 
        public JsonResult GetInexReason(string ECFGId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                dt = null;
                DataSet ds = db.GetInexReason(ECFGId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult DownloadInexreviewReport(string InexFrom, string InexTo, string InexById, string DocAmount, string DocFrom, string DocTo, string DocTypeId, string DocNo, string EmpCodeId,
           string EmpNameId, string SuppCodeId, string SuppNameId, string ViewType)
        {
            try
            {

                string Data1 = "", Data2 = "";
                DataSet ds = db.GetInex(plib.ConvertDate(InexFrom), plib.ConvertDate(InexTo), InexById, DocAmount == "" ? "0" : DocAmount, plib.ConvertDate(DocFrom), plib.ConvertDate(DocTo), DocTypeId, DocNo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, ViewType, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            wb.Worksheets.Add(ds.Tables[1]);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= Inex Review Summary Report.xls");
                        }
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        //---------------------------------End------------------------------------------//
        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult DownloadInexDespatchReport(string InexFrom, string InexTo, string InexById, string DocAmount, string DocFrom, string DocTo, string DocTypeId, string DocNo, string EmpCodeId,
           string EmpNameId, string SuppCodeId, string SuppNameId, string ViewType)
        {
            try
            {

                string Data1 = "", Data2 = "";
                DataSet ds = db.GetInex(plib.ConvertDate(InexFrom), plib.ConvertDate(InexTo), InexById, DocAmount == "" ? "0" : DocAmount, plib.ConvertDate(DocFrom), plib.ConvertDate(DocTo), DocTypeId, DocNo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, ViewType, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            wb.Worksheets.Add(ds.Tables[1]);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= Inex Despatch Summary Report.xls");
                        }
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        //---------------------------------End------------------------------------------//
    }
}
