using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.Web.Mvc;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class SearchController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        // GET: /FLEXISPEND/Search/
        public ActionResult ProductivityTracking()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetProductivityTracking(string DateFrom, string DateTo, string Activity)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetProductivityTrackingDW(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), Activity, plib.LoginUserId);
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

        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult Downloadproductivitydatewise(string DateFrom, string DateTo, string Activity)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetProductivityTrackingDW(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), Activity, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        
                        if (dt.Rows.Count > 0)
                        {
                            wb.Worksheets.Add(ds.Tables[0]);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= Productivity Tracking Daywise Report.xls");
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

        [HttpPost]
        public JsonResult GetProductivityTrackingEmployee(string DateFrom, string DateTo, string EmpNameId, string Activity)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetProductivityTrackingUser(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), EmpNameId, Activity, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
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

        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult Downloadproductivitymonthwise(string DateFrom, string DateTo, string EmpNameId, string Activity)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetProductivityTrackingUser(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), EmpNameId, Activity, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        if (dt.Rows.Count > 0)
                        {
                            wb.Worksheets.Add(ds.Tables[0]);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= Productivity Tracking Monthwise Report.xls");
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

        // GET: /FLEXISPEND/Search/
        public ActionResult Query()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetQuery(string ECFNo, string ECFDate, string DocTypeId, string RaiserId, string AuthrId, string DocStatus, string SuppCodeId, string SuppNameId,
        string InvoiceNo, string InvAmount, string ECFAmount, string PVNo, string PVDate, string InwardAWBNo, string ChqAWBNo, string Employee, string ChequeNo, string ChqueAmount, string PhyReceptDateFrom, string PhyReceptDateTo, string pono, string cbfNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetSearchQuery(ECFNo, plib.ConvertDate(ECFDate), DocTypeId, RaiserId, AuthrId, DocStatus, SuppCodeId, SuppNameId,
                    InvoiceNo, InvAmount == null || InvAmount == "" ? "0" : InvAmount, ECFAmount == null || ECFAmount == "" ? "0" : ECFAmount, PVNo, plib.ConvertDate(PVDate), InwardAWBNo, ChqAWBNo, Employee, ChequeNo, ChqueAmount == null || ChqueAmount == "" ? "0" : ChqueAmount, plib.ConvertDate(PhyReceptDateFrom), plib.ConvertDate(PhyReceptDateTo), "0", "0", plib.LoginUserId,pono,cbfNo);
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


        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult DownloadqueryReport(string ECFNo, string ECFDate, string DocTypeId, string RaiserId, string AuthrId, string DocStatus, string SuppCodeId, string SuppNameId,
        string InvoiceNo, string InvAmount, string ECFAmount, string PVNo, string PVDate, string InwardAWBNo, string ChqAWBNo, string Employee, string ChequeNo, string ChqueAmount, string PhyReceptDateFrom, string PhyReceptDateTo, string pono, string cbfNo)
        {
            try
            {

                string Data1 = "", Data2 = "";
                DataSet ds = db.GetSearchQuery(ECFNo, plib.ConvertDate(ECFDate), DocTypeId, RaiserId, AuthrId, DocStatus, SuppCodeId, SuppNameId,
                    InvoiceNo, InvAmount == null || InvAmount == "" ? "0" : InvAmount, ECFAmount == null || ECFAmount == "" ? "0" : ECFAmount, PVNo, plib.ConvertDate(PVDate), InwardAWBNo, ChqAWBNo, Employee, ChequeNo, ChqueAmount == null || ChqueAmount == "" ? "0" : ChqueAmount, plib.ConvertDate(PhyReceptDateFrom), plib.ConvertDate(PhyReceptDateTo), "0", "0", plib.LoginUserId, pono, cbfNo);
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

                            Response.AddHeader("content-disposition", "attachment;filename= Query Report.xls");
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

        // GET: /FLEXISPEND/Search/
        public ActionResult DocumentDetails(string id = "", string subId = "", string ecfNo = "")
        {
            //checker view
            @ViewBag.rptEcfId = "";
            @ViewBag.rptEcfdet = "";

            @ViewBag.rptEcfId = id;
            @ViewBag.rptEcfdet = string.Format("{0}-{1}-{2}", id, subId, "Y");

            SearchData result = new SearchData();
            if (ecfNo != "")
            {
                //Get The ECF PayMent Details
                DataSet ds = db.GetSearchQuery(ecfNo, "", "0", "0", "0", "", "0", "0", "", "0", "0", "", "", "", "", "", "", "0", "", "", id, "1", plib.LoginUserId,"","");
                if (ds != null)
                {
                    if (ds.Tables.Count > 5)
                        result = GetListDetails(ds);
                    else
                        result = null;
                }
            }
            return View(result);
        }

        public SearchData GetListDetails(DataSet ds)
        {
            try
            {
                SearchData parent = new SearchData();
                DataTable dt = null;
                if (ds != null)
                {
                    List<InexDetail> InexData = new List<InexDetail>();
                    List<InexDespatchDetail> InexDespatchData = new List<InexDespatchDetail>();
                    List<PaymentDetail> PaymentData = new List<PaymentDetail>();
                    List<PaymentDispatch> PaymentDispatchData = new List<PaymentDispatch>();
                    List<PaymentStatus> PaymentStatusData = new List<PaymentStatus>();
                    List<ReissueDetail> Reissuedata = new List<ReissueDetail>();

                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            InexDetail child = new InexDetail();

                            child.SlNo = cdr["SlNo"].ToString();
                            child.InexBy = cdr["InexBy"].ToString();
                            child.ReviewBy = cdr["ReviewBy"].ToString();
                            child.InexDate = cdr["InexDate"].ToString();
                            child.ActionStatus = cdr["ActionStatus"].ToString();
                            child.Remarks = cdr["Remarks"].ToString();
                            child.Reason = cdr["Reason"].ToString();
                            InexData.Add(child);
                        }
                    }
                    parent.InexDetails = InexData;

                    dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow cdr in dt.Rows)
                        {
                            InexDespatchDetail child = new InexDespatchDetail();

                            child.SlNo = (i + 1).ToString();
                            child.awbNo = cdr["awbNo"].ToString();
                            child.courierName = cdr["courierName"].ToString();
                            child.despatchTo = cdr["despatchTo"].ToString();
                            child.despatchAddress = cdr["despatchAddress"].ToString();
                            child.despatchDate = cdr["despatchDate"].ToString();

                            InexDespatchData.Add(child);
                            i++;
                        }
                    }
                    parent.InexDespatchDetail = InexDespatchData;

                    dt = ds.Tables[2];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            PaymentDetail child = new PaymentDetail();

                            child.Slno = cdr["SlNo"].ToString();
                            child.PVDate = cdr["PVDate"].ToString();
                            child.PVNo = cdr["PVNo"].ToString();
                            child.PayMode = cdr["PayMode"].ToString();
                            child.PVAmount = cdr["PVAmount"].ToString();
                            child.PayMentBatchNo = cdr["PaymentBatchNo"].ToString();
                            child.ClaimType = cdr["ClaimType"].ToString();
                            child.Code = cdr["EmployeeSupplierCode"].ToString();
                            child.Name = cdr["EmployeeSupplierName"].ToString();

                            child.ChequeNo = cdr["ChqNo"].ToString();
                            child.ChequeDate = cdr["ChqDate"].ToString();
                            child.Bank = cdr["Bank"].ToString();
                            child.ChequeBookNo = cdr["ChqbookNo"].ToString();
                            child.Status = cdr["Status"].ToString();
                            PaymentData.Add(child);
                        }
                    }
                    parent.PaymentDetail = PaymentData;

                    dt = ds.Tables[3];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            PaymentDispatch child = new PaymentDispatch();

                            child.Slno = cdr["SlNo"].ToString();
                            child.PVDate = cdr["PVDate"].ToString();
                            child.PVNo = cdr["PVNo"].ToString();
                            child.PayMode = cdr["PayMode"].ToString();
                            child.PVAmount = cdr["PVAmount"].ToString();
                            child.PaymentBatchNo = cdr["PaymentBatchNo"].ToString();
                            child.DespatchDate = cdr["DespatchDate"].ToString();
                            child.CourierName = cdr["CourierName"].ToString();
                            child.AWBNo = cdr["AWBNo"].ToString();
                            PaymentDispatchData.Add(child);
                        }
                    }
                    parent.PaymentDispatch = PaymentDispatchData;

                    dt = ds.Tables[4];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            PaymentStatus child = new PaymentStatus();

                            child.Slno = cdr["SlNo"].ToString();
                            child.PVDate = cdr["PVDate"].ToString();
                            child.PVNo = cdr["PVNo"].ToString();
                            child.PayMode = cdr["PayMode"].ToString();
                            child.PVAmount = cdr["PVAmount"].ToString();
                            child.PayMentBatchNo = cdr["PaymentBatchNo"].ToString();
                            child.ClaimType = cdr["ClaimType"].ToString();
                            child.Code = cdr["EmployeeSupplierCode"].ToString();
                            child.Name = cdr["EmployeeSupplierName"].ToString();

                            child.ChequeNo = cdr["ChqNo"].ToString();
                            child.ChequeDate = cdr["ChqDate"].ToString();
                            child.Bank = cdr["Bank"].ToString();
                            child.ChequeBookNo = cdr["ChqbookNo"].ToString();
                            child.Status = cdr["Status"].ToString();
                            PaymentStatusData.Add(child);
                        }
                    }
                    parent.PaymentStatus = PaymentStatusData;

                    dt = ds.Tables[5];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            ReissueDetail child = new ReissueDetail();

                            child.SlNo = cdr["SlNo"].ToString();
                            child.PVDate = cdr["PVDate"].ToString();
                            child.PVNo = cdr["PVNo"].ToString();
                            child.PayMode = cdr["PayMode"].ToString();
                            child.PVAmount = cdr["PVAmount"].ToString();
                            child.ClaimType = cdr["ClaimType"].ToString();
                            child.EmployeeSupplierCode = cdr["EmployeeSupplierCode"].ToString();
                            child.EmployeeSupplierName = cdr["EmployeeSupplierName"].ToString();
                            child.PaymentBatchNo = cdr["PaymentBatchNo"].ToString();
                            Reissuedata.Add(child);
                        }
                    }
                    parent.ReissueDetail = Reissuedata;
                }
                return parent;
            }
            catch
            {
                return null;
            }
        }

        // GET: /FLEXISPEND/Search/
        public ActionResult PaymentAuditTrail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPaymentAuditTrail(string PVNo, string ECFNo, string InvoiceNo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string BankId, string ChqNo)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                DataSet ds = db.GetPaymentAuditTrail(PVNo, ECFNo, InvoiceNo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, BankId, ChqNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[3];
                    if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }


        //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

        [HttpPost]
        public JsonResult DownloadPaymentAuditTrail(string PVNo, string ECFNo, string InvoiceNo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string BankId, string ChqNo)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPaymentAuditTrail(PVNo, ECFNo, InvoiceNo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, BankId, ChqNo, plib.LoginUserId);
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

                            Response.AddHeader("content-disposition", "attachment;filename= Payment Audittrail Report.xls");
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

        //----------------------------------------End---------------------------------------//

        #region Temp Class
        public class SearchData
        {
            public List<InexDetail> InexDetails { get; set; }
            public List<InexDespatchDetail> InexDespatchDetail { get; set; }
            public List<PaymentDetail> PaymentDetail { get; set; }
            public List<PaymentDispatch> PaymentDispatch { get; set; }
            public List<PaymentStatus> PaymentStatus { get; set; }
            public List<ReissueDetail> ReissueDetail { get; set; }
        }

        public class InexDetail
        {
            public string SlNo { get; set; }
            public string InexBy { get; set; }
            public string ReviewBy { get; set; }
            public string InexDate { get; set; }
            public string ActionStatus { get; set; }
            public string Remarks { get; set; }
            public string Reason { get; set; }
        }

        public class InexDespatchDetail
        {
            public string SlNo { get; set; }
            public string awbNo { get; set; }
            public string courierName { get; set; }
            public string despatchTo { get; set; }
            public string despatchAddress { get; set; }
            public string despatchDate { get; set; }
        }

        public class PaymentDetail
        {
            public string Slno { get; set; }
            public string PVDate { get; set; }
            public string PVNo { get; set; }
            public string PayMode { get; set; }
            public string PVAmount { get; set; }
            public string PayMentBatchNo { get; set; }

            public string ClaimType { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }

            public string ChequeNo { get; set; }
            public string ChequeDate { get; set; }
            public string Bank { get; set; }
            public string ChequeBookNo { get; set; }
            public string Status { get; set; }
        }

        public class PaymentDispatch
        {
            public string Slno { get; set; }
            public string PVDate { get; set; }
            public string PVNo { get; set; }
            public string PayMode { get; set; }
            public string PVAmount { get; set; }
            public string PaymentBatchNo { get; set; }
            public string DespatchDate { get; set; }
            public string CourierName { get; set; }
            public string AWBNo { get; set; }
        }

        public class PaymentStatus
        {
            public string Slno { get; set; }
            public string PVDate { get; set; }
            public string PVNo { get; set; }
            public string PayMode { get; set; }
            public string PVAmount { get; set; }
            public string PayMentBatchNo { get; set; }

            public string ClaimType { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }

            public string ChequeNo { get; set; }
            public string ChequeDate { get; set; }
            public string Bank { get; set; }
            public string ChequeBookNo { get; set; }
            public string Status { get; set; }
        }

        public class ReissueDetail
        {
            public string SlNo { get; set; }
            public string PVDate { get; set; }
            public string PVNo { get; set; }
            public string PayMode { get; set; }
            public string PVAmount { get; set; }
            public string ClaimType { get; set; }
            public string EmployeeSupplierCode { get; set; }
            public string EmployeeSupplierName { get; set; }
            public string PaymentBatchNo { get; set; }
        }
        #endregion
    }
}
