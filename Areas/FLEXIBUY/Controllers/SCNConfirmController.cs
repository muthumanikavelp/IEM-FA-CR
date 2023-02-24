using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using IEM.Areas.FLEXIBUY.Models;
using System.Text;
using IEM.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.WebControls;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class SCNConfirmController : Controller
    {
        //
        // GET: /FLEXIBUY/SCNConfirm/
            int totalcount;
        ErrorLog objErrorLog = new ErrorLog();
        private IRepositoryKIR objrep;
        public SCNConfirmController()
            : this(new prsummodel())
        {

        }
        public SCNConfirmController(IRepositoryKIR objM)
        {
            objrep = objM;
        }

        [HttpGet]
        public ActionResult SCNConfirmSummary()
        {
            List<grnconfirmation> obj = new List<grnconfirmation>();
            try
            {
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        public JsonResult GetSCNConfirm()
        {          
            List<grnconfirmation> obj = new List<grnconfirmation>();
            try
            {
                obj = objrep.getscnconfirmation();               
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SCNConfirm(grnconfirmation gr, int grnheadgid)
        {
            try
            {
                grnconfirmationdetails objgrn = new grnconfirmationdetails();
                objgrn.grnconfirmationdetail = objrep.getscnconfirmationdetails(gr, grnheadgid);
                objgrn.grnheadGid = grnheadgid;
                objgrn.grnrefno = objgrn.grnconfirmationdetail[0].grnrefno.ToString();
                objgrn.grndate = objgrn.grnconfirmationdetail[0].grndate.ToString();
                objgrn.vendorname = objgrn.grnconfirmationdetail[0].vendorname.ToString();
                objgrn.poworefno = objgrn.grnconfirmationdetail[0].poworefno.ToString();
                objgrn.dcno = objgrn.grnconfirmationdetail[0].dcno.ToString();
                objgrn.invoiceno = objgrn.grnconfirmationdetail[0].invoiceno.ToString();
                return View(objgrn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetSCNdetailsConfirm(grnconfirmation gr, int SCNheadgid)
        {
            List<grnconfirmationdetails> obj = new List<grnconfirmationdetails>();
            try
            {
                gr.grnheadgid = SCNheadgid;
                obj = objrep.getscnconfirmationdetails(gr, SCNheadgid);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveScnConfirm(grnconfirmation objgrn)
        {
            try
            {

                string approve = objrep.scnapprove(objgrn);
                return Json(approve, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult Scnsavedetails(grnconfirmationdetails objgrndet)
        {

            DataTable dt = new DataTable();
            grnconfirmationdetails objgrndet1 = new grnconfirmationdetails();
            if (Session["grndetails"] != null || Session["grndetails"] != "")
            {
                dt = (DataTable)Session["grndetails"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (!dt.Columns.Contains("manuname") && !dt.Columns.Contains("assetslno") && !dt.Columns.Contains("putdate"))
                        //{
                        //    dt.Columns.Add("manuname");
                        //    dt.Columns.Add("assetslno");
                        //    dt.Columns.Add("putdate");

                        //}
                        if (!dt.Columns.Contains("IsSerial") && !dt.Columns.Contains("branch_flag") && !dt.Columns.Contains("GRNType"))
                        {
                            dt.Columns.Add("IsSerial");
                            dt.Columns.Add("branch_flag");
                            dt.Columns.Add("GRNType");
                        }
                        if (dt.Rows[i]["grninwrddet_gid"].ToString() == objgrndet.grndetGid.ToString())
                        {
                            dt.Rows[i]["grninwrddet_mft_name"] = string.IsNullOrEmpty(objgrndet.manfName) ? "" : objgrndet.manfName.ToString();
                            dt.Rows[i]["grninwrddet_assetsrlno"] = string.IsNullOrEmpty(objgrndet.assetSlno) ? "" : objgrndet.assetSlno.ToString();
                            dt.Rows[i]["grninwrddet_puttousedatetime"] = string.IsNullOrEmpty(objgrndet.putToUseDate) ? "" : objgrndet.putToUseDate.ToString();
                        }
                    }
                }

                string result = objrep.insertgrncfmdetails(objgrndet, dt);
                if (result == "Success")
                {
                    objgrndet1.grnconfirmationdetail = objrep.rebindconfirmdetails(dt);

                }


            }

            return PartialView("SCNDetails", objgrndet1);

        }

        [HttpPost]
        public string Scnvalidatesavedetails(grnconfirmationdetails objgrndet)
        {
            string result = "";
            DataTable dt = new DataTable();
            grnconfirmationdetails objgrndet1 = new grnconfirmationdetails();
            if (Session["grndetails"] != null || Session["grndetails"] != "")
            {
                dt = (DataTable)Session["grndetails"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (!dt.Columns.Contains("manuname") && !dt.Columns.Contains("assetslno") && !dt.Columns.Contains("putdate"))
                        //{
                        //    dt.Columns.Add("manuname");
                        //    dt.Columns.Add("assetslno");
                        //    dt.Columns.Add("putdate");

                        //}
                        //if (!dt.Columns.Contains("IsSerial") && !dt.Columns.Contains("branch_flag") && !dt.Columns.Contains("GRNType"))
                        //{
                        //    dt.Columns.Add("IsSerial");
                        //    dt.Columns.Add("branch_flag");
                        //    dt.Columns.Add("GRNType");
                        //}
                        if (dt.Rows[i]["grninwrddet_gid"].ToString() == objgrndet.grndetGid.ToString())
                        {
                            dt.Rows[i]["grninwrddet_mft_name"] = string.IsNullOrEmpty(objgrndet.manfName) ? "" : objgrndet.manfName.ToString();
                            dt.Rows[i]["grninwrddet_assetsrlno"] = string.IsNullOrEmpty(objgrndet.assetSlno) ? "" : objgrndet.assetSlno.ToString();
                            dt.Rows[i]["grninwrddet_puttousedatetime"] = string.IsNullOrEmpty(objgrndet.putToUseDate) ? "" : objgrndet.putToUseDate.ToString();
                        }
                    }
                }

                  result = objrep.insertgrncfmdetails(objgrndet, dt);
              
            }

            return result;

        }
        //public JsonResult GetSCNdetailsConfirmattach(grnconfirmation gr, int SCNheadgid)
        //{
        //    List<grnconfirmationdetails> obj = new List<grnconfirmationdetails>();
        //    try
        //    {
        //        gr.grnheadgid = SCNheadgid;
        //        obj = objrep.Attachmentname(gr);
        //        return Json(obj, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json(obj, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}
