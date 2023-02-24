using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using IEM.Areas.MASTERS.Controllers;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class PaymentAdviceController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        ForMailController MailController = new ForMailController();
        #endregion

        // PRINT
        // GET: /FLEXISPEND/PaymentAdvice/
        public ActionResult Print()
        {
            return View();
        }

        // EMAIL
        // GET: /FLEXISPEND/PaymentAdvice/
        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchPaymentAdvice(string BatchNo, string PayDateFrom, string PayDateTo, string PVNoFrom, string PVNoTo, string ClaimType, string PVAmount, string EmpNameId,
        string EmpCodeId, string SuppCodeId, string SuppNameId, string PayMode, string ViewType, string Location)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetPaymentAdvice(BatchNo, PVNoFrom, PVNoTo, plib.ConvertDate(PayDateFrom), plib.ConvertDate(PayDateTo), ClaimType, (PVAmount == null || PVAmount.Trim() == "") ? "0" : PVAmount, EmpNameId, EmpCodeId, SuppCodeId, SuppNameId, PayMode, ViewType, Location, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        // GET: /FLEXISPEND/PaymentAdvice/PrintPreview
        public ActionResult PrintPreview()
        {
            List<PaymentAdviceDetails> result = new List<PaymentAdviceDetails>();
            if (TempData["PaymentAdvicePrint"] != null)
            {
                DataSet ds = TempData["PaymentAdvicePrint"] as DataSet;
                if (ds != null)
                {
                    if (ds.Tables.Count > 2) {
                        result = GetListDetails(ds.Tables[1], ds.Tables[2], ds.Tables[0].Rows[0]["Address"].ToString());
                    }
                }
                else { result = null; }
            }
            else
            {
                result = null;
            }
            return View(result);
        }

        [HttpPost]
        public JsonResult FetchPaymentAdvicePrint(string BatchNo, string PayDateFrom, string PayDateTo, string PVNoFrom, string PVNoTo, string ClaimType, string PVAmount, string EmpNameId,
        string EmpCodeId, string SuppCodeId, string SuppNameId, string PayMode, string ViewType, string Location)
        {
            try
            {
                DataSet ds = db.GetPaymentAdvice(BatchNo, PVNoFrom, PVNoTo, plib.ConvertDate(PayDateFrom), plib.ConvertDate(PayDateTo), ClaimType, (PVAmount == null || PVAmount.Trim() == "") ? "0" : PVAmount, EmpNameId, EmpCodeId, SuppCodeId, SuppNameId, PayMode, ViewType, Location, plib.LoginUserId);
                if (ds != null)
                {
                    TempData["PaymentAdvicePrint"] = null;
                    TempData["PaymentAdvicePrint"] = ds as DataSet;
                    TempData.Keep("PaymentAdvicePrint");

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { TempData["PaymentAdvicePrint"] = null; }

                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public List<PaymentAdviceDetails> GetListDetails(DataTable dtParent, DataTable dtChild, string BranchAddress)
        {
            List<PaymentAdviceDetails> result = new List<PaymentAdviceDetails>();
            DataTable dt = null;
            if (dtParent != null && dtChild != null)
            {
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    PaymentAdviceDetails parent = new PaymentAdviceDetails();
                    parent.PvId = dtParent.Rows[i]["PvId"].ToString();
                    parent.IsTransactionDetail = dtParent.Rows[i]["Bit"].ToString();
                    parent.BeneficiaryName = dtParent.Rows[i]["BeneficiaryName"].ToString();
                    parent.EmployeeSupplierName = dtParent.Rows[i]["EmployeeSupplierName"].ToString();
                    parent.BeneficiaryAddress = dtParent.Rows[i]["BenAddress"].ToString();
                    parent.BranchAddress = BranchAddress;
                    parent.PaymentNo = dtParent.Rows[i]["PVNo"].ToString();
                    parent.ECFBatchNo = dtParent.Rows[i]["PaymentBatchNo"].ToString();
                    parent.Location = dtParent.Rows[i]["Location"].ToString();
                    parent.ChequeDate = dtParent.Rows[i]["ChqDate"].ToString();
                    parent.ChequeNo = dtParent.Rows[i]["ChqNo"].ToString();
                    parent.RaiserName = dtParent.Rows[i]["Raiser"].ToString();
                    parent.RaiserCode = dtParent.Rows[i]["RaiserCode"].ToString();
                    parent.TDS = dtParent.Rows[i]["Tds"].ToString();
                    parent.Amount = dtParent.Rows[i]["Amount"].ToString();
                    parent.Total = dtParent.Rows[i]["PVAmount"].ToString();
                    parent.EmpStatus = dtParent.Rows[i]["EmployeeStatus"].ToString().Trim();

                    dt = LoadChildList(dtChild, parent.PvId);
                    List<ClaimTransactionDetails> childArray = new List<ClaimTransactionDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            ClaimTransactionDetails child = new ClaimTransactionDetails();
                            //child.PvId = cdr["PvId"].ToString();
                            child.BillNo = cdr["BillNo"].ToString();
                            child.BillDate = cdr["BillDate"].ToString();
                            child.BillAmount = cdr["BillAmount"].ToString();
                            child.NetAmount = cdr["NetAmount"].ToString();
                            child.Tds = cdr["Tds"].ToString();
                            child.DedAdvRecovery = cdr["DedAdvRecovery"].ToString();
                            child.Wct = cdr["Wct"].ToString();
                            childArray.Add(child);
                        }
                    }
                    parent.InnerTransactions = childArray;

                    result.Add(parent);
                }
            }
            return result;
        }

        public DataTable LoadChildList(DataTable dt, string FilterId)
        {
            DataRow[] dr = dt.Select("PvId = " + FilterId);
            DataTable tdt = dt.Copy();
            tdt.Rows.Clear();
            foreach (DataRow tdr in dr)
            {
                tdt.ImportRow(tdr);
            }
            return tdt;
        }

        //Email alert
        [HttpPost]
        public JsonResult FetchEmailAlert(string BatchNo, string PayDateFrom, string PayDateTo, string PVNoFrom, string PVNoTo, string ClaimType, string PVAmount, string EmpNameId,
        string EmpCodeId, string SuppCodeId, string SuppNameId, string PayMode, string ViewType, string Location)
        {
            string MailRes="",MailTempbit="",Data1="",Msg="",Clear="";
            int SuccCount = 0, FailCount = 0;
            try
            {
                DataSet ds = db.GetPaymentAdvice(BatchNo, PVNoFrom, PVNoTo, plib.ConvertDate(PayDateFrom), plib.ConvertDate(PayDateTo), ClaimType, (PVAmount == null || PVAmount.Trim() == "") ? "0" : PVAmount, EmpNameId, EmpCodeId, SuppCodeId, SuppNameId, PayMode, ViewType, Location, plib.LoginUserId);
                if (ds != null)
                {
                  //Table
                    if(ds .Tables[1].Rows .Count >0)
                    {   
                        for (int i=0; i<ds .Tables [1].Rows .Count ; i++)
                        {
                            if(ds .Tables [1].Rows[i]["EmailId"].ToString ()!="")
                            {
                                MailRes = MailController.sendusermailfs("FS", "Payment Advice Email Alert", ds.Tables[1].Rows[i]["PvId"].ToString(), "s", new[] { ds.Tables[1].Rows[i]["EmailId"].ToString() }, "0");
                                if (MailRes == "Sucess")
                                {
                                    SuccCount++;
                                    MailTempbit = "1";
                                    Msg = "Mail Sent Successfully!";
                                    Clear = "true";
                                    
                                }
                                else
                                {
                                    FailCount++;
                                    if (MailTempbit == "1") { }
                                    else { MailTempbit = "0";
                                    Msg = "Mail has not been sent!";
                                    Clear = "false";
                                    }
                                };
                            }
                            else
                            {
                                FailCount++;
                                if (MailTempbit == "")   //for email id not available
                                {
                                    MailTempbit = "0";
                                    Msg = "Mail has not been sent!";
                                    Clear = "false";
                                }
                            }

                        }                  
                    }
                    else
                    {
                        Msg = "Data not available for mail sending!";
                        Clear = "false";
                    }

                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Message", typeof(string));
                    dt2.Columns.Add("Clear", typeof(string));
                    dt2.Rows.Add(new object[] { Msg + "  Total Record:" + ds.Tables[1].Rows.Count+", Success Mail:"+SuccCount +", Failure Mail:"+FailCount , Clear });
                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

                }
                else { //TempData["PaymentAdvicePrint"] = null;
                }

                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

     
    }

    #region Payment Advice
    public class PaymentAdviceDetails
    {
        public string PvId { get; set; }
        public string IsTransactionDetail { get; set; }
        public string BeneficiaryName { get; set; }
        public string EmployeeSupplierName { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string BranchAddress { get; set; }
        public string PaymentNo { get; set; }
        public string ECFBatchNo { get; set; }
        public string Location { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public string RaiserName { get; set; }
        public string RaiserCode { get; set; }
        public string TDS { get; set; }
        public string Amount { get; set; }
        public string Total { get; set; }
        public string EmpStatus { get; set; }
        public List<ClaimTransactionDetails> InnerTransactions { get; set; }
    }

    public class ClaimTransactionDetails
    {
        //public string PvId { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string BillAmount { get; set; }
        public string NetAmount { get; set; }
        public string Tds { get; set; }
        public string DedAdvRecovery { get; set; }
        public string Wct { get; set; }
    }
    #endregion
}
