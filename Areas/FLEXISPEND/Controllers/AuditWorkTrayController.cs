using ClosedXML.Excel;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class AuditWorkTrayController : Controller
    {
        #region Declaration
        proLib plib = new proLib();

        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        DataTable dt;
        ErrorLog objErrorLog = new ErrorLog();
        #endregion

        #region Audit Tray Dashboard
        // GET: /FLEXISPEND/AuditWorkTray/
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetAuditWorkTray(string DocNo, string InvoiceNo, string DocTypeId, string DocAmount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId)
        {
            try
            {
                string Data0 = "", Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "",
                    Data10 = "", Data11 = "", Data12 = "", Data13 = "", Data14 = "", Data15 = "", Data16 = "", Data17 = "", Data18 = "", Data19 = "", Data20 = "",Data21="",Data22="", ecfid = "";
                DataSet ds = db.GetAuditWorkTray(DocNo, InvoiceNo, DocTypeId, DocAmount.Trim() == "" ? "0" : DocAmount, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[3];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[4];
                    if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[5];
                    if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[6];
                    if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[7];
                    if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[8];
                    if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[9];
                    if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[10];
                    if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[11];
                    if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[12];
                    if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[13];
                    if (dt.Rows.Count > 0) { Data13 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[14];
                    if (dt.Rows.Count > 0) { Data14 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[15];
                    if (dt.Rows.Count > 0) { Data15 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[16];
                    if (dt.Rows.Count > 0) { Data16 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[17];
                    if (dt.Rows.Count > 0) { Data17 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[18];
                    if (dt.Rows.Count > 0) { Data18 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[19];
                    if (dt.Rows.Count > 0) { Data19 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[20];
                    if (dt.Rows.Count > 0) { Data20 = JsonConvert.SerializeObject(dt); }
                    //Ramya
                    dt = ds.Tables[21];
                    if (dt.Rows.Count > 0) { Data21 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[22];
                    if (dt.Rows.Count > 0) { Data22 = JsonConvert.SerializeObject(dt); }

                    if (TempData["ecfid"]!="" || TempData["ecfid"] != null)
                    //if ((!TempData["ecfid"].ToString().Equals("")) || TempData["ecfid"] != null)
                    {
                        TempData["ecfid"] = ecfid;

                        //ds = db.GetIsAssigned(ecfid, plib.LoginUserId, "N");
                        TempData["ecfid"] = "";
                    }


                    return Json(new { Data0, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12, Data13, Data14, Data15, Data16, Data17, Data18, Data19, Data20, Data21, Data22 }, JsonRequestBehavior.AllowGet);
                //Ramya
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }



        [HttpPost]
        public JsonResult SetLeaseBulkApproval(string ECFIds, string Status, string Remarks)
        {
            try
            {
                string Data0 = "";
                DataSet ds = db.SetLeaseBulkApproval(ECFIds, Status, Remarks, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }
                    if (ds.Tables[1].Rows.Count > 0)
                        Session["LeaseError"] = ds.Tables[1];

                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //downloading Excel File
        public JsonResult DownloadErrorLeaseDocument()
        {
            DataTable _downloadingData = null;
            _downloadingData = Session["LeaseError"] as DataTable;
            Session["LeaseError"] = null;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= LeaseBulkError.xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIsAssigned(string ecfId, string uid)
        {
            try
            {
                DataSet ds = new DataSet();
                string Data0 = "";

                if (uid == "0")
                {
                    ds = db.GetIsAssigned(ecfId, "0");
                }
                else { ds = db.GetIsAssigned(ecfId, plib.LoginUserId); }

                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }
                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }


            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Supplier Invoice Maker
        public ActionResult SupplierInvoiceMaker(string data, string data1)
        {
            return RedirectToAction("SupplierInvoiceDetails", "AuditWorkTray", new { id = data, subId = data1, area = "FLEXISPEND" });
        }

        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoiceDetails
        public ActionResult SupplierInvoiceDetails(string id, string subId)
        {
            @ViewBag.EcfId = "";
            @ViewBag.Ecfdet = "";
            Session["EcfIdForTax"] = id;
            @ViewBag.EcfId = id;

            //Get the employee Role
            string EditOption = "Y";
            EditOption = _fsRep.GetEmployeeRole(plib.LoginUserId);

            EditOption += "~0~Y";

            @ViewBag.IsRedup = _fsRep.GetEmployeeRoleChecker(plib.LoginUserId);

            @ViewBag.Ecfdet = string.Format("{0}-{1}-{2}", id, subId, EditOption);
            try
            {
                DataSet ds = db.GetCreditlineByECFGID(id);
                DataTable dtIFSCCode = ds.Tables[0];
                ViewBag.IFSCCodemsg = "";
                if (dtIFSCCode.Rows.Count > 0)
                {
                    string IFSCCode = "";
                    IFSCCode = dtIFSCCode.Rows[0]["IfscCode"].ToString();
                    //Beneficiary=dtIFSCCode.Rows[0]["Beneficiary"].ToString();

                    if (IFSCCode.ToString().Trim().Equals(""))
                    {
                        ViewBag.IFSCCodemsg = "IFSC Code is not updated for this ECF!!";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetAuditCheckList(string EcfId)
        {
            try
            {
                string Data0 = "", Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                bool enableOpts = true;
                DataSet ds = db.GetAuditCheckList(EcfId, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 2)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        enableOpts = dt.Rows[0]["Role"].ToString().ToLower() == "m" ? false : true;
                        Data1 = JsonConvert.SerializeObject(dt);
                    }

                    List<ParentGroup> result = GetListDetails(ds.Tables[2], ds.Tables[3], enableOpts);
                    if (result != null) { Data2 = JsonConvert.SerializeObject(result); }

                    dt = ds.Tables[4];
                    if (dt.Rows.Count > 0)
                    {
                        Data3 = JsonConvert.SerializeObject(dt);
                    }

                    dt = ds.Tables[5];
                    if (dt.Rows.Count > 0)
                    {
                        Data4 = JsonConvert.SerializeObject(dt);
                    }
                    return Json(new { Data0, Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //Save Check List Grid
        public JsonResult SetAuditCheckList(string EcfId, string ChkLstIds, string Status, string Remarks, string Reason)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetAuditCheckList(EcfId, ChkLstIds, Status, Remarks, Reason, plib.LoginUserId);
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
            catch { return null; }
        }

        //Update Reason For Check List
        public JsonResult SetAuditCheckListReason(string EcfId, string ChkLstId, string Status, string Reason, string ChkLstIds)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetAuditCheckListReason(EcfId, ChkLstId, Status, Reason, ChkLstIds, plib.LoginUserId);
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
            catch { return null; }
        }

        public List<ParentGroup> GetListDetails(DataTable dtParent, DataTable dtChild, bool EnableOpt)
        {
            List<ParentGroup> result = new List<ParentGroup>();
            DataTable dt = null;
            if (dtParent != null && dtChild != null)
            {
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    ParentGroup parent = new ParentGroup();
                    parent.GroupId = dtParent.Rows[i]["ChecklistId"].ToString();
                    parent.GroupName = dtParent.Rows[i]["Title"].ToString();
                    dt = LoadChildList(dtChild, dtParent.Rows[i]["ChecklistId"].ToString());

                    List<ChildGroup> childArray = new List<ChildGroup>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            ChildGroup child = new ChildGroup();
                            child.ChkLstId = cdr["ChecklistId"].ToString();
                            child.ParentId = cdr["ParentId"].ToString();
                            child.Title = cdr["Title"].ToString();
                            child.Reason = cdr["Reason"].ToString();
                            child.EnableOpt = EnableOpt;
                            childArray.Add(child);
                        }
                    }
                    parent.ChildGroup = childArray;
                    result.Add(parent);
                }
            }
            return result;
        }

        public DataTable LoadChildList(DataTable dt, string FilterId)
        {
            DataRow[] dr = dt.Select("ParentId = " + FilterId);
            DataTable tdt = dt.Copy();
            tdt.Rows.Clear();
            foreach (DataRow tdr in dr)
            {
                tdt.ImportRow(tdr);
            }
            return tdt;
        }
        #endregion

        #region SupplierInvoice
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult SupplierInvoice()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetSupplierInvoiceMaker(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "";
                DataSet ds = db.GetSupplierInvoiceMaker(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }
                Session["EcfIdForTax"] = EcfId;
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetInvoiceDetails(string InvId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "";
                DataSet ds = db.GetInvoiceDetails(InvId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                //RCM added by Ramya
                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }
				
                Session["Ainvoicegid"] = InvId; //correction by Kavitha GST need to go 03-08-2017
               // return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7 }, JsonRequestBehavior.AllowGet);

                var jsonResult = Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;

            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetInvoiceTaxDetails(InvoiceTax iTax)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetTaxDetails(iTax.InvoiceTaxgid, iTax.InvId, iTax.SupplierId, iTax.TaxId, iTax.SubTaxId, iTax.TaxRate, iTax.TaxableAmt, iTax.TaxAmount, iTax.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //GST TAX
        [HttpPost]
        public JsonResult SetGSTInvoiceTaxDetails(InvoiceTax iTax)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetGSTTaxDetails(iTax.InvoiceTaxgid, iTax.InvId, iTax.SupplierId, iTax.GSNapplicable, iTax.HSNgid, iTax.TaxId, iTax.SubTaxId, iTax.TaxRate, iTax.TaxableAmt, iTax.TaxAmount, iTax.IsRemoved, plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetDebitLineDetails(InvoiceDebitLine iDebit)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                DataSet ds = db.SetDebitLineDetails(iDebit.DebitlineGId, iDebit.ECFId, iDebit.invid, iDebit.expnaturegid, iDebit.expCatId,
                    iDebit.expSubcatId, iDebit.CCCode, iDebit.FCCode, iDebit.OUCode, iDebit.ProductCode, iDebit.Amount, iDebit.Desc, iDebit.IsRemoved, plib.LoginUserId,iDebit.hsngid,iDebit.rcm);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                //Ramya added for RCM
                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1, Data2, Data3,Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult CheckrcmExists(RCMcheck ircm)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.CheckrcmExists(ircm.ECFId, ircm.DebitlineGId, ircm.invid, ircm.hsngid, ircm.ProviderLocation,ircm.action);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

             
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetCreditLineDetails(InvoiceCreditLine iCredit)
        {
            try
            {
                string Data1 = "", Data2 = "";

                DataSet ds = db.SetCreditLineDetails(iCredit.Ecfid, iCredit.InvId, iCredit.CreditlineGId, iCredit.RefId, iCredit.paymode, iCredit.RefNo,
                    iCredit.Beneficiary, iCredit.Desc, iCredit.Amount, iCredit.BankId, iCredit.IsRemoved, iCredit.IfscCode, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SetEditCreditLineDetails(InvoiceCreditLine iCredit)
        {
            try
            {
                string Data1 = "", Data2 = "";

                DataSet ds = db.SetEditCreditLineDetails(iCredit.Ecfid, iCredit.InvId, iCredit.CreditlineGId, iCredit.RefId, iCredit.paymode, iCredit.RefNo,
                    iCredit.Beneficiary, iCredit.Desc, iCredit.Amount, iCredit.BankId, iCredit.IsRemoved, iCredit.IfscCode, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetWithHoldingTaxDetails(WHTax whTax)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetWithHoldingTax(whTax.withholdtaxgid, whTax.Invoicegid, whTax.TaxId, whTax.taxsubtypegid, whTax.TaxRate, whTax.TaxableAmt, whTax.TaxAmount,
                    whTax.NetAmount, whTax.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetAttachmentDetails(Attachment attachment)
        {
            try
            {
                FileServer Cmnfs = new FileServer();
                string Data1 = "", Data2 = "", Data3 = "";
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Msg", typeof(string));
                dt1.Columns.Add("Clear", typeof(bool));
                //dt1.Rows.Add(new object[] { "Ensure Attachemnt File!", false });
                dt1.Rows.Add(new object[] { "Invalid File format!", false });

                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                    attachment.AttachmentName = _attFile.FileName.ToString();
                }
                if (attachment.AttachmentName == null && attachment.IsRemoved == "0")
                {
                    if (dt1.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt1); }
                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                DataSet ds = db.SetAttachmentDetails(attachment.EcfId, attachment.Invoicegid == null ? "0" : attachment.Invoicegid, attachment.AttachmentId, attachment.AttachmentName, attachment.AttachmentType, attachment.Desc, attachment.IsRemoved, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    if (attachment.AttachmentName != "" && attachment.IsRemoved == "0" && (ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() != "true" || ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() != "1"))
                    {
                        try
                        {
                            //upload the file to server.
                            HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                            var stream = _attFile.InputStream;
                            string uploaderUrl = string.Format("{0}{1}", plib.HoldFileUploadUrl, ds.Tables[0].Rows[0]["ID"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                            using (var fileStream = System.IO.File.Create(uploaderUrl))
                            {
                                // stream.CopyTo(fileStream);
                            }
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                                ViewBag.Result = "inside File Stream";
                            }
                            var FileString = Convert.ToBase64String(bytFile);
                            var result = Cmnfs.SaveFileToServer(FileString, ds.Tables[0].Rows[0]["ID"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]).Result;
                            if (result == "success")
                            {
                                if (System.IO.File.Exists(uploaderUrl))
                                {
                                    System.IO.File.Delete(uploaderUrl);
                                }
                            }
                            //remove the temp data content.
                            TempData.Remove("_attFile");
                        }
                        catch { }
                    }
                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult SetECFHeaderDetails(ECFHeader ecf)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetECFHeaderDetails(ecf.EcfId, ecf.ReducedAmt, ecf.ProcessedAmt, ecf.PaymentNett, ecf.Amort,ecf.currencygid, ecf.curencycode, ecf.currencyrate,
                    ecf.currencyamt, ecf.ecfamt, ecf.ecfdesc, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetInvoiceHeaderDetails(InvoiceHeader inv)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5="",Data6="";
                DataSet ds = db.SetInvoiceHeaderDetails(inv.EcfId, inv.InvId, plib.ConvertDate(inv.InvDate), inv.InvNo, inv.Desc, inv.Amount, inv.WOTaxAmount, plib.ConvertDate(inv.ServiceMonth), inv.ProvisionFlag,
                    inv.Amort, inv.RetentionFlag, inv.rententionRate == null ? "0" : inv.rententionRate, inv.rententionAmount == null ? "0" : inv.rententionAmount,
                    plib.ConvertDate(inv.ReleaseOn == null ? "" : inv.ReleaseOn), inv.IsVerify, inv.IsRemoved, inv.GstCharged, inv.ProviderLocation, inv.GstinVendor, inv.ReceiverLocation, inv.GstinFiccl, inv.Suppliergid, plib.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }  
                if (ds.Tables.Count > 3)
                {
                    dt = ds.Tables[3];
                    if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                }
                if (ds.Tables.Count > 4)
                {
                    dt = ds.Tables[4];
                    if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }
                }

                if (ds.Tables.Count > 5)
                {
                    dt = ds.Tables[5];
                    if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetAdhocVendor(string Name, string GSTIN, string LocationId, string Suppliergid, string IsGST)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetAdhocVendor(Name, GSTIN, LocationId, Suppliergid, IsGST, plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetInvoiceHeaderDetailsEmp(InvoiceHeader inv)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "";
                DataSet ds = db.SetInvoiceHeaderDetailsEmp(inv.EcfId, inv.InvId, inv.ProviderLocation, inv.GstinVendor, inv.Suppliergid, inv.InvNo, plib.ConvertDate(inv.InvDate), inv.Amount, inv.Desc, inv.WOTaxAmount, plib.ConvertDate(inv.ServiceMonth),
                    inv.IsVerify, inv.IsRemoved, inv.GstCharged, inv.ReceiverLocation, inv.GstinFiccl, plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 3)
                    {
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 4)
                    {
                        dt = ds.Tables[4];
                        if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }
                    }

                    //Added by ramya
                    if (ds.Tables.Count > 5)
                    {
                        dt = ds.Tables[5];
                        if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }
                    }
                }
                Session["Ainvoicegid"] = inv.InvId;
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                TempData["_attFile"] = null;
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
                                TempData["_attFile"] = hpfBase;
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
        public FileResult Download(string Id, string FileName)
        {
            //string[] str = FileName.Split('.');
            //string directory = plib.HoldFileUploadUrl + Id.ToString() + "." + str[str.Length - 1].ToString();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
            try
            {
                string[] str = FileName.Split('.');
                string directory = Id.ToString() + "." + str[str.Length - 1].ToString();
                FileServer ObjService = new FileServer();
                var Filenamecont = ObjService.DownloadFile(directory).Result;
                if (Filenamecont != "Fail")
                {
                    if (Filenamecont != "error")
                    {
                        string fileName = "Download" + FileName.ToString();
                        byte[] fileBytes = Convert.FromBase64String(Filenamecont);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                    else
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                        string fileName = "Download" + Id.ToString();
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                }
                else
                {
                    // byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                    //string fileName = "Download" + Id.ToString();
                    return File("", System.Net.Mime.MediaTypeNames.Application.Octet, "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]

        public JsonResult GetTaxRate(string TaxId, string SubTaxgid, string SId, string View)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
               string invoiceid = Session["Ainvoicegid"] == null ? "0" : Session["Ainvoicegid"].ToString();
               DataSet ds = db.GetTaxRateSupplier(TaxId, SubTaxgid, SId, View, plib.LoginUserId, invoiceid);//session invoiceid added must go 03-08-2017

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            Data2 = dt.Rows[0]["Id"].ToString();
                        }
                    }

                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            Data3 = dt.Rows[0]["Id"].ToString();
                        }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult FetchARFDetails(string SupplierId, string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchPaymentRefNo(SupplierId, "", ViewType, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult FetchBenificiaryDetails(string PayMode, string SupplierId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetSupplierBeneficiary(SupplierId, PayMode);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SearchGLDetails(string GLDet, string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchPaymentRefNo("", GLDet, ViewType, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetAmortScheduleDetails(AmortSchedule aSchedule)
        {
            try
            {
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("debitlinegid", typeof(Int32));
                dt1.Columns.Add("DPercent", typeof(Double));
                string[] row = aSchedule.Percentage.Split('~');
                foreach (string col in row)
                    dt1.Rows.Add(new object[] { col.Split('/')[0], col.Split('/')[1] });

                string Data1 = "";
                DataSet ds = db.SetAmortSchedule(aSchedule.amortgid, aSchedule.EcfId, aSchedule.InvId, aSchedule.supplierId, aSchedule.amount, aSchedule.glno, plib.ConvertDate(aSchedule.startdate), plib.ConvertDate(aSchedule.enddate),
                    aSchedule.frequencygid, aSchedule.split, aSchedule.active, aSchedule.isremoved, dt1, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetAmortSchedule(AmortSchedule aSchedule)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortReScheduleDetails(aSchedule.InvId, aSchedule.amount, aSchedule.frequencygid, plib.ConvertDate(aSchedule.startdate), plib.ConvertDate(aSchedule.enddate), aSchedule.split, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetAmortSplit(AmortSchedule aSchedule)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetAmortSplitDetails(plib.ConvertDate(aSchedule.startdate), plib.ConvertDate(aSchedule.enddate), aSchedule.frequencygid, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetECFAmort(string InvId, string AmortId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetECFAmort(InvId, AmortId, plib.LoginUserId);

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPOMappingHeader(string POHeader)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPOMappingHeader(POHeader, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPOMappingDetails(string InvoicePOId, string INVId)
        {
            try
            {
                DataTable dt1 = null;
                string Data1 = "";
                DataSet ds = db.GetPOMappingDetails(InvoicePOId, INVId);

                dt = ds.Tables[0];
                dt1 = dt.Copy();
                dt1.Columns.Add("POInvoicedQty", typeof(double));
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow[] row = ds.Tables[1].Select("podetails_gid=" + dt1.Rows[i]["podetails_gid"].ToString());
                    if (row.Length > 0)
                    {
                        dt1.Rows[i]["POInvoicedQty"] = row[0]["POinvoiceqty"].ToString();
                    }
                }
                dt = dt1;
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetCurrencyRate(string Currency)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("27", Currency, plib.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SubmitInvoice(string InvId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetInvoiceDetails(InvId, plib.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SubmitInvoiceEmp(string InvId, string IsTravel = "0")
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetInvoiceDetailsEmp(InvId, IsTravel, plib.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RunDedup(string EcfId)
        {
            try
            {
                //string sessionId = "ECFRunDedup" + EcfId;
                //Session[sessionId] = null;
                string Data1 = "", Data2 = "";
                DataSet ds = db.ECFRunDedup(EcfId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    //dt = ds.Tables[2];
                    //if (dt.Rows.Count > 0)
                    //{
                    //    DataTable _tmpdata = dt;
                    //    _tmpdata.TableName = "Dedup Details";
                    //    Session[sessionId] = _tmpdata;
                    //}

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //downloading Excel File
        public JsonResult DownloadDedupData(string id = "")
        {
            string sessionId = "ECFRunDedup" + id;
            DataTable _downloadingData = Session[sessionId] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Dedup Details.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UploadDebitLine()
        {
            try
            {
                TempData["_debitLineFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    TempData["_debitLineFile"] = hpfBase;
                }
            }
            catch (Exception)
            {
            }
        }



        public JsonResult DebitLineExcelSheets(string InvId)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                string Data1 = "", Data2 = "",Data3=""; DataSet Ds;
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Msg", typeof(string));
                dt2.Columns.Add("Clear", typeof(string));

                if (TempData["_debitLineFile"] != null)
                {
                    HttpPostedFileBase uploadedExcelFile = TempData["_debitLineFile"] as HttpPostedFileBase;
                    string[] y = uploadedExcelFile.FileName.Split('.');

                    //if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = uploadedExcelFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", plib.ExcelDebitLineUrl, uploadedExcelFile.FileName.Split('\\')[uploadedExcelFile.FileName.Split('\\').Length - 1]);

                        using (var fileStream = System.IO.File.Create(uploaderUrl))
                            stream.CopyTo(fileStream);

                        TempData["_debitLineFile"] = null;

                        var connectionstring = "";
                       /* //if (y[y.Length - 1] == "xlsx" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        //    connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                        //Ramya Changed version.12.0 is not working at HFC Server
                        if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            //connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        //else
                        //    connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";*/

                        if (y[y.Length - 1] == "xlsx" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                           // connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
                        else
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";

                        try
                        {
                            using (var conn = new OleDbConnection(connectionstring))
                            {
                                conn.Open();
                                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
                                    var adapter = new OleDbDataAdapter(cmd);
                                    Ds = new DataSet();
                                    adapter.Fill(Ds);
                                    conn.Close();
                                    conn.Dispose();
                                    if (Ds != null && Ds.Tables[0].Rows.Count != 0)
                                    {
                                        Ds.Tables[0].Columns.RemoveAt(0);
                                        DataSet ds1 = db.ExcelDebitlineUpload(InvId, Ds.Tables[0], plib.LoginUserId);
                                        if (ds1 != null && ds1.Tables.Count > 0)
                                        {
                                            dt = ds1.Tables[0];
                                            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[1];
                                            if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[3];
                                            if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                                            if (ds1.Tables[0].Rows[0]["Clear"].ToString() == "2")
                                                Session["DebitlineErrorList"] = ds1.Tables[2];

                                            return Json(new { Data1, Data2 ,Data3}, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        dt2.Rows.Add(new object[] { "Excel file should not be empty!", "0" });
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            dt2.Rows.Add(new object[] { "Please upload the valid file!" + ex.Message.ToString(), "0" });
                        }
                    }
                    else
                        dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", "0" });
                }
                else
                    dt2.Rows.Add(new object[] { "Upload excel file", "0" });
                if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }

        public JsonResult DebitlineExcel(string InvId)
        {
            Session["DebitlineDownlinkFile"] = null;
            DataSet ds = db.GetDebitLineDownloadDetails(InvId, plib.LoginUserId);
            if (ds != null)
                Session["DebitlineDownlinkFile"] = ds.Tables[0];
            return Json("Sucess", JsonRequestBehavior.AllowGet);
        }

        //downloading Excel File
        public JsonResult DownloadExcel()
        {
            DataTable _downloadingData = null;
            _downloadingData = Session["DebitlineDownlinkFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= Debitline.xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        //downloading Excel File
        public JsonResult DownloadErrorExcel()
        {
            DataTable _downloadingData = null;
            _downloadingData = Session["DebitlineErrorList"] as DataTable;
            Session["DebitlineErrorList"] = null;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= Debitline.xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetDebitlineReset(string INVId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetDebitlineReset(INVId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Non Travel Claim
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult NonTravelClaim()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEmployeeNonTravelClaim(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
                DataSet ds = db.GetNonTravelClaim(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[11];
                if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }

                if (ds.Tables.Count > 11)
                {
                    dt = ds.Tables[12];
                    if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetNTExpenseDetails(InvoiceDebitLine iDebit)
        {
            try
            {

                string Data1 = "", Data2 = "", Data3 = "", Data4 = "";
                DataSet ds = db.SetNTPaymentDetails(iDebit.DebitlineGId, iDebit.ECFId, iDebit.invid, iDebit.expnaturegid, iDebit.expCatId,
                    iDebit.expSubcatId, plib.ConvertDate(iDebit.ClaimFrm), plib.ConvertDate(iDebit.ClaimTo), iDebit.CCCode, iDebit.FCCode, iDebit.OUCode,
                    iDebit.ProductCode, iDebit.Amount, iDebit.Desc, iDebit.IsRemoved, plib.LoginUserId, iDebit.hsngid,iDebit.rcm);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                //Ramya
                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Local Conveyance
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult LocalConveyance()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEmployeeLocalConveyance(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data12 = "";
                DataSet ds = db.GetLocalConveyance(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[12];
                if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data12 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetLocalConveyanceDetails(string EcfId, string EmpId, string ExpId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetLocalConveyanceDetails(EcfId, EmpId, ExpId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RunDedupLC(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.ECFRunDedupLC(EcfId, plib.LoginUserId);
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

        //downloading Excel File
        public JsonResult DownloadDedupLCData(string id = "")
        {
            string sessionId = "ECFRunDedup" + id;
            DataTable _downloadingData = Session[sessionId] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Dedup Details.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RunDedupEmployee(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.RunDedupEmployee(EcfId, plib.LoginUserId);
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

        //downloading Excel File
        public JsonResult DownloadDedupEmployeeData(string id = "")
        {
            string sessionId = "ECFRunDedup" + id;
            DataTable _downloadingData = Session[sessionId] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Dedup Details.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Travel Claim
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult TravelClaim()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEmployeeTravelClaim(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "", Data13 = "";
                DataSet ds = db.GetTravelClaim(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[10];
                if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[12];
                if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }

                if (ds.Tables.Count > 12)
                {
                    dt = ds.Tables[13];
                    if (dt.Rows.Count > 0) { Data13 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12, Data13 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetTravelExpenseDetails(TravelDetails tDebit)
        {
            try
            {
                string Data1 = "", Data2 = "",Data3="",Data4="";
                //DataSet ds = db.SetTravelExpenseDetails(tDebit.ecftravelgid, tDebit.ecfId, tDebit.invId, tDebit.expnaturegid, tDebit.expCatId, tDebit.expSubcatId, tDebit.placefrom,
                //    tDebit.placeto, plib.ConvertDate(tDebit.claimperiodfrm), plib.ConvertDate(tDebit.claimperiodto), tDebit.trasportgid, tDebit.transportclassgid, tDebit.Distance, tDebit.Rate, tDebit.FCCode,
                //    tDebit.CCCode, tDebit.OUCode, tDebit.ProductCode, tDebit.Amount, tDebit.Desc, tDebit.IsRemoved, plib.LoginUserId);

                DataSet ds = db.SetTravelExpenseDetails(tDebit.ecftravelgid, tDebit.invId, tDebit.ecfId, tDebit.expnaturegid, tDebit.expCatId, tDebit.expSubcatId, tDebit.placefrom,
                    tDebit.placeto, plib.ConvertDate(tDebit.claimperiodfrm), plib.ConvertDate(tDebit.claimperiodto), tDebit.trasportgid, tDebit.transportclassgid, tDebit.Distance, tDebit.Rate, tDebit.FCCode,
                    tDebit.CCCode, tDebit.OUCode, tDebit.ProductCode, tDebit.Amount, tDebit.Desc, tDebit.IsRemoved, plib.LoginUserId,tDebit.hsngid,tDebit.rcm);

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
            catch
            {
                return null;
            }
        }

        #endregion

        #region Petty Cash
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult PettyCash()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEmployeePettyCash(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
                DataSet ds = db.GetEmployeePettyCash(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[11];
                if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }

                if (ds.Tables.Count > 11)
                {
                    dt = ds.Tables[12];
                    if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Advance Request
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult AdvanceRequest()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetAdvanceRequest(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "";
                DataSet ds = db.GetAdvanceRequest(EcfId, plib.LoginUserId);


                Session["EcfIdForTax"] = EcfId;


                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
               Session["Ainvoicegid"] = dt.Rows[0]["Invid"].ToString(); //correction by kavitha 30-06-2017 GST  need to go production 03-08-2017
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[10];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }
                //----------------Shrinidhi 03.05.2016-----------------------------
                dt = ds.Tables[11];
                if (dt.Rows.Count > 0) { Data12 = JsonConvert.SerializeObject(dt); }

                //---------------------------------------------------------------
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetARFDebit(ARFDebit aDebit)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetAdvanceRequestDebit(aDebit.ecfartgid, aDebit.ECFId, aDebit.advancetypegid, aDebit.promoinvoice, aDebit.pono, aDebit.cbfno, aDebit.CCCode,
                    aDebit.FCCode, aDebit.OUCode, aDebit.ProductCode, aDebit.Amount, plib.ConvertDate(aDebit.liQdate), aDebit.desc, aDebit.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetWithHoldingTaxARF(WHTax whTax)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetWithHoldingTaxARF(whTax.withholdtaxgid, whTax.Invoicegid, whTax.TaxId, whTax.taxsubtypegid, whTax.TaxRate, whTax.TaxableAmt, whTax.TaxAmount,
                    whTax.NetAmount, whTax.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RunDedupARF(string EcfId)
        {
            try
            {

                string Data1 = "", Data2 = "";
                DataSet ds = db.ECFRunDedupARF(EcfId, plib.LoginUserId);
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

        //downloading Excel File
        public JsonResult DownloadDedupARFData(string id = "")
        {
            string sessionId = "ECFRunDedup" + id;
            DataTable _downloadingData = Session[sessionId] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Dedup Details.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DSA Supplier Invoice
        // GET: /FLEXISPEND/AuditWorkTray/SupplierInvoice
        public PartialViewResult DSASupplier()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetDSASupplierInvoice(string EcfId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "";
                DataSet ds = db.GetSupplierInvoiceDSA(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[5];
                if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) { Data7 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) { Data8 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) { Data9 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0) { Data10 = JsonConvert.SerializeObject(dt); }

                //dt = ds.Tables[10];
                //if (dt.Rows.Count > 0) { Data11 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetDSAInvoiceDetails(string InvId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetDSAInvoiceDetails(InvId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                Session["Ainvoicegid"] = InvId;
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetWithHoldingTaxDSA(WHTax whTax)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetWithHoldingTaxDSA(whTax.withholdtaxgid, whTax.Invoicegid, whTax.TaxId, whTax.taxsubtypegid, whTax.TaxRate, whTax.TaxableAmt, whTax.TaxAmount,
                    whTax.NetAmount, whTax.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public void UploadDSAWithHoldingTax()
        {
            try
            {
                TempData["_DSAWHTFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    TempData["_DSAWHTFile"] = hpfBase;
                }
            }
            catch (Exception)
            {
            }
        }

        public JsonResult UploadDSAWithHoldingTaxExcel(string ecfid)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = ""; DataSet Ds;
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Msg", typeof(string));
                dt2.Columns.Add("Clear", typeof(Boolean));

                if (TempData["_DSAWHTFile"] != null)
                {
                    HttpPostedFileBase uploadedExcelFile = TempData["_DSAWHTFile"] as HttpPostedFileBase;
                    if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = uploadedExcelFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", plib.ExcelDebitLineUrl, uploadedExcelFile.FileName);

                        using (var fileStream = System.IO.File.Create(uploaderUrl))
                            stream.CopyTo(fileStream);

                        TempData["_DSAWHTFile"] = null;

                        var connectionstring = "";
                        if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            //connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                            connectionstring = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        else
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        try
                        {
                            using (var conn = new OleDbConnection(connectionstring))
                            {
                                conn.Open();
                                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
                                    var adapter = new OleDbDataAdapter(cmd);
                                    Ds = new DataSet();
                                    adapter.Fill(Ds);
                                    conn.Close();
                                    conn.Dispose();
                                    if (Ds != null && Ds.Tables[0].Rows.Count != 0)
                                    {
                                        Ds.Tables[0].Columns.RemoveAt(0);
                                        DataSet ds1 = db.ExcelDSAWHTUpload(ecfid, Ds.Tables[0], plib.LoginUserId);
                                        if (ds1 != null && ds1.Tables.Count > 0)
                                        {
                                            dt = ds1.Tables[0];
                                            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[1];
                                            if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[2];
                                            if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                                            return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        dt2.Rows.Add(new object[] { "Excel file should not be empty!", "false" });
                                    }
                                }
                            }
                        }
                        catch
                        {
                            dt2.Rows.Add(new object[] { "Please upload the valid file!", false });
                        }
                    }
                    else
                        dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", false });
                }
                else
                    dt2.Rows.Add(new object[] { "Upload excel file", false });
                if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }

        [HttpPost]
        public void UploadDSADebitline()
        {
            try
            {
                Session["_DSADebitlineFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    Session["_DSADebitlineFile"] = hpfBase;
                }
            }
            catch (Exception)
            {
            }
        }

        [HttpPost]
        public JsonResult FillDSADebitLineSheets()
        {
            try
            {
                string Data1 = "";
                if (Session["_DSADebitlineFile"] != null)
                {
                    HttpPostedFileBase uploadedExcelFile = Session["_DSADebitlineFile"] as HttpPostedFileBase; string[] y = uploadedExcelFile.FileName.Split('.');

                    //if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = uploadedExcelFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", plib.ExcelDebitLineUrl, uploadedExcelFile.FileName);

                        using (var fileStream = System.IO.File.Create(uploaderUrl))
                            stream.CopyTo(fileStream);

                        var connectionstring = "";
                        //if (y[y.Length - 1] == "xlsx" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        //    connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                        //Ramya Changed version.12.0 is not working at HFC Server
                        if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            connectionstring = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        else
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        try
                        {
                            using (var conn = new OleDbConnection(connectionstring))
                            {
                                conn.Open();
                                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                                conn.Close();
                                conn.Dispose();

                                //Fill the sheet Name to dropDown
                                if (sheets.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(sheets); }
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult UploadDSAdebitlineExcel(string ECFId, string SheetName)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = ""; DataSet Ds;
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Message", typeof(string));
                dt2.Columns.Add("Clear", typeof(Boolean));

                if (Session["_DSADebitlineFile"] != null)
                {
                    HttpPostedFileBase uploadedExcelFile = Session["_DSADebitlineFile"] as HttpPostedFileBase;
                    string[] y = uploadedExcelFile.FileName.Split('.');

                    //if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = uploadedExcelFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", plib.ExcelDebitLineUrl, uploadedExcelFile.FileName);

                        Session["_DSADebitlineFile"] = null;

                        var connectionstring = "";
                        //if (y[y.Length - 1] == "xlsx" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        //    connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                        //Ramya Changed version.12.0 is not working at HFC Server
                        if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            connectionstring = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        else
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        try
                        {
                            using (var conns = new OleDbConnection(connectionstring))
                            {
                                conns.Open();
                                using (var cmd = conns.CreateCommand())
                                {
                                    cmd.CommandText = "SELECT * FROM [" + SheetName + "] ";
                                    var adapter = new OleDbDataAdapter(cmd);
                                    Ds = new DataSet();
                                    adapter.Fill(Ds);
                                    conns.Close();
                                    conns.Dispose();
                                    if (Ds != null && Ds.Tables[0].Rows.Count != 0)
                                    {
                                        Ds.Tables[0].Columns.RemoveAt(0);
                                        DataSet ds1 = db.SetExcelDSADebitlineUpload(ECFId, Ds.Tables[0], plib.LoginUserId);
                                        if (ds1 != null && ds1.Tables.Count > 0)
                                        {
                                            dt = ds1.Tables[0];
                                            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[1];
                                            if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                                            dt = ds1.Tables[2];
                                            if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                                            return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        dt2.Rows.Add(new object[] { "Excel file should not be empty!", "false" });
                                    }
                                }
                            }
                        }
                        catch
                        {
                            dt2.Rows.Add(new object[] { "Please upload the valid file!", false });
                        }
                    }
                    else
                        dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", false });
                }
                else
                    dt2.Rows.Add(new object[] { "Upload excel file", false });
                if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }

        [HttpPost]
        public JsonResult SetDSAReset(string ECFId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetDSAReset(ECFId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RunDedupDSA(string EcfId)
        {
            try
            {
                //string sessionId = "ECFRunDedup" + EcfId;
                //Session[sessionId] = null;
                string Data1 = "", Data2 = "";
                DataSet ds = db.ECFRunDedupDSA(EcfId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    //dt = ds.Tables[2];
                    //if (dt.Rows.Count > 0)
                    //{
                    //    DataTable _tmpdata = dt;
                    //    _tmpdata.TableName = "Dedup Details";
                    //    Session[sessionId] = _tmpdata;
                    //}

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //downloading Excel File
        public JsonResult DownloadDedupDSAData(string id = "")
        {
            string sessionId = "ECFRunDedup" + id;
            DataTable _downloadingData = Session[sessionId] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Dedup Details.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Multiple Invoice [PC, Traval & Non-Traval]
        [HttpPost]
        public JsonResult GetInvoiceDetailsPettyCash(string InvId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "";
                DataSet ds = db.GetInvoiceDetailsPettyCash(InvId, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 3)
                    {
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 4)
                    {
                        dt = ds.Tables[4];
                        if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }
                    }
                    //Added by ramya
                    if (ds.Tables.Count > 5)
                    {
                        dt = ds.Tables[5];
                        if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }
                    }
                }
                Session["Ainvoicegid"] = InvId;
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetInvoiceDetailsTravel(string InvId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "",Data6="";
                DataSet ds = db.GetInvoiceDetailsTravel(InvId, plib.LoginUserId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 3)
                    {
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 4)
                    {
                        dt = ds.Tables[4];
                        if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }
                    }
					   if (ds.Tables.Count > 5)
                    {
					dt = ds.Tables[5];
                        if (dt.Rows.Count > 0) { Data6 = JsonConvert.SerializeObject(dt); }
                    }
                }

                
                Session["Ainvoicegid"] = InvId;
                return Json(new { Data1, Data2, Data3, Data4, Data5,Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetAuditForTraveClaim(string EcfId)
        {
            try
            {

                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "", Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "", Data13 = "";
                DataSet ds = db.GetTravelClaimHistory(EcfId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) 
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["PMode"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
               
                    Data1 = JsonConvert.SerializeObject(dt);
                }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) 
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["taxsubtype"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
                    Data2 = JsonConvert.SerializeObject(dt); 
                }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) 
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["NatureOfExpenses"] = "No Current Value";
                        dt.Rows.Add(row);
                    }

                    Data3 = JsonConvert.SerializeObject(dt);
                }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["expnature"] = "No Current Value";
                        dt.Rows.Add(row);
                    }

                    Data4 = JsonConvert.SerializeObject(dt);
                }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["Advancetype"] = "No Current Value";
                        dt.Rows.Add(row);
                    }

                }
                else
                {
                    DataRow row = dt.NewRow();
                    row["Slno"] = "Tax Value";
                    row["Advancetype"] = "No Tax Records Found!!!";
                    dt.Rows.Add(row);
                }
                Data5 = JsonConvert.SerializeObject(dt);


                dt = ds.Tables[5];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["expnature"] = "No Current Value";
                        dt.Rows.Add(row);
                    }

                }
                else
                {
                    DataRow row = dt.NewRow();
                    row["Slno"] = "Tax Value";
                    row["expnature"] = "No Tax Records Found!!!";
                    dt.Rows.Add(row);
                }
                Data6 = JsonConvert.SerializeObject(dt);



                dt = ds.Tables[6];
                if (dt.Rows.Count > 0) 
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["InvNo"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
                    Data7 = JsonConvert.SerializeObject(dt); 
                }

                dt = ds.Tables[7];
                if (dt.Rows.Count > 0) {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["GsnApplicable"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
                   
                }
                else
                {
                    DataRow row = dt.NewRow();
                    row["Slno"] = "Tax Value";
                    row["GsnApplicable"] = "No Tax Records Found!!!";
                    dt.Rows.Add(row);
                }
                Data8 = JsonConvert.SerializeObject(dt);
                
                dt = ds.Tables[8];
                if (dt.Rows.Count > 0) {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["AmortFlag"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
                    Data9 = JsonConvert.SerializeObject(dt);
                }

                dt = ds.Tables[9];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Slno"].ToString() != "Current Value")
                    {
                        DataRow row = dt.NewRow();
                        row["Slno"] = "Current Value";
                        row["expnature"] = "No Current Value";
                        dt.Rows.Add(row);
                    }
                    Data10 = JsonConvert.SerializeObject(dt);
                }


                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10 }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        
    }

    #region Supplier Invoice Maker Classes
    public class EmployeeRole
    {
        public string RoleName { get; set; }
    }

    public class ParentGroup
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<ChildGroup> ChildGroup { get; set; }
    }

    public class ChildGroup
    {
        public string ChkLstId { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public bool EnableOpt { get; set; }
    }
    #endregion
}
