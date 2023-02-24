using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class RetentionReleaseController : Controller
    {
        //
        // GET: /EOW/RetentionRelease/
        private RetentionRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        public RetentionReleaseController()
            : this(new RetentionRelease())
        {

        }
        public RetentionReleaseController(RetentionRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult RetentionIndex()
        {
            //List<Retention_Release> Releaserecords = new List<Retention_Release>();
            //Releaserecords = objModel.RetentionReleaseGrid().ToList();
            //if (Releaserecords.Count == 0)
            //{
            //    ViewBag.records = "No Records Found";
            //}

            return View();
        }
        [HttpPost]
        public ActionResult RetentionIndex(string filter_name = null, string filter_name1 = null, string ECFNo = null, string InvoiceNo = null, string Suppliercode = null, string Suppliername = null, string extendeddate = null)
        {
            List<Retention_Release> Releaserecords = new List<Retention_Release>();
            Releaserecords = objModel.RetentionReleaseGridSearch(filter_name, filter_name1, ECFNo, InvoiceNo, Suppliercode, Suppliername, extendeddate).ToList();
            if (Releaserecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            ViewBag.filter_name = filter_name;
            ViewBag.filter_name1 = filter_name1;
            ViewBag.ECFNo = ECFNo;
            ViewBag.InvoiceNo = InvoiceNo;
            ViewBag.Suppliercode = Suppliercode;
            ViewBag.Suppliername = Suppliername;
            ViewBag.extendeddate = extendeddate;
            return View(Releaserecords);
        }
        [HttpGet]
        public PartialViewResult attachpop()
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult RetentionReleaselog(int id)
        {
            Session["log_id"] = id;
            Retention_Release TypeModel = new Retention_Release();
            //  TypeModel = objModel.debitgrid(ecf,invoice);
            // TypeModel.GetReceipt = new SelectList(PhyReceipt.GetDoc(), "Doc_gid", "Doc_name", TypeModel.Doc_gid);
            return PartialView(TypeModel);

        }
        [HttpGet]
        public PartialViewResult RetentionReleaseDoc_Index(int id)
        {

            Retention_Release TypeModel = objModel.RetentionReleaseGrid_id(id);
            Session["ecf"] = TypeModel.invoice_ecf_gid;
            Session["invoice"] = TypeModel.invoice_gid;
            Session["ecfcredit"] = TypeModel.invoice_ecf_gid;
            Session["invoicecredit"] = TypeModel.invoice_gid;
            Session["balance"] = TypeModel.Balance_amount;
            Session["ecf_raiser"] = TypeModel.ecf_raiser;
            //  TypeModel = objModel.debitgrid(ecf,invoice);
            // TypeModel.GetReceipt = new SelectList(PhyReceipt.GetDoc(), "Doc_gid", "Doc_name", TypeModel.Doc_gid);
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult _EmpAttachmentCreate(Retention_Release EmployeeeExpenseModel)
        {
            string img = "";
            try
            {
                //if (ModelState.IsValid)
                //{
                //  if (Session["empattmtfile"] != null)
                {
                    string ecfgid = Convert.ToString(Session["ecf"].ToString());
                    if (ecfgid != "")
                    {
                        EmployeeeExpenseModel.attachment_by = Convert.ToInt32(Session["employee_gid"].ToString());
                        EmployeeeExpenseModel.attachment_refgid = Convert.ToInt32(ecfgid);
                        HttpPostedFileBase savefile = (HttpPostedFileBase)TempData["_attFile"];
                        string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel);
                        if (getname != "")
                        {
                            //string path = "//192.168.0.203/temp/";
                            //string path = @"C:\temp\";
                            //string path1 = Path.GetFileName(savefile.FileName);
                            //string[] str = path1.Split('.');
                            ////var index = path1.LastIndexOf(".") + 1;
                            //getname = getname + "." + str[1].ToString();
                            //string savedFileName = Path.Combine(path, getname);
                            //savefile.SaveAs(savedFileName);
                            //img = "Yes";

                            var fileextension = Path.GetExtension(savefile.FileName);
                            var stream = savefile.InputStream;
                            var path = Path.Combine(@"C:\temp\", getname + fileextension);
                            //using (var fileStream = System.IO.File.Create(path))
                            //{
                            //    stream.Seek(0, SeekOrigin.Begin);
                            //    stream.CopyTo(fileStream);
                            //}

                            //Muthu 2016-10-19
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                                ViewBag.Result = "inside File Stream";
                            }
                            var FileString = Convert.ToBase64String(bytFile);
                            var filename = getname + "." + fileextension;
                            var result = Cmnfs.SaveFileToServer(FileString, filename).Result;

                            TempData["_attFile"] = null;
                            img = "Yes";
                        }
                    }
                }
                //}
                ViewBag.SearchItems = null;
                return Json(img, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public void UploadFiles()
        {
            try
            {
                TempData["_attFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    TempData["_attFile"] = hpfBase;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public JsonResult DeleteAttachment(Retention_Release attach)
        {
            String result = String.Empty;
            result = objModel.DeleteAttachment(attach.attachment_gid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getattachdata()
        {
            List<Retention_Release> lst = new List<Retention_Release>();
            DataTable GetflowgidCount = new DataTable();
            RetentionRelease rais = new RetentionRelease();
            GetflowgidCount = rais.Selectattach(Session["ecf"].ToString());
            if (GetflowgidCount.Rows.Count > 0)
            {
                foreach (DataRow row in GetflowgidCount.Rows)
                {

                    lst.Add(
                    new Retention_Release
                    {
                        attachment_gid = Convert.ToInt32(row["attachment_gid"].ToString()),
                        attachment_filename = Convert.ToString(row["attachment_filename"]),
                        attachment_type = Convert.ToString(row["attachment_attachmenttype_gid"].ToString()),
                        attachment_desc = Convert.ToString(row["attachment_desc"]),
                        attachment_nameby = Convert.ToString(row["attachment_by"]),
                        attachment_date = Convert.ToString(row["attachment_date"].ToString())

                    });

                };
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult adjuse_creditline()
        {
            Retention_Release TypeModel = new Retention_Release();

            TypeModel.GetPaymodecredit = new SelectList(objModel.GetPaymodecredit(), "paymode_gidcredit", "paymode_namecredit");
            TypeModel.GetRefcredit = new SelectList(objModel.Getrefcredit(), "payment_accountnocredit", "payment_accountnocredit");
            TypeModel.creditamount = Session["balance"].ToString();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult RetentionReleasecreditref(Retention_Release expensentax1)
        {
            try
            {
                return Json(objModel.Getrefcreditpaymode(expensentax1), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpPost]
        //public JsonResult expcategory(Retention_Release expensentax)
        //{
        //    try
        //    {

        //        int ExpCat_gid = Convert.ToInt32(expensentax.expense_gid);
        //        return Json(objModel.Getexpcategory(expensentax), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[HttpPost]
        //public JsonResult expsubcategory(Retention_Release expensensub)
        //{
        //    try
        //    {
        //        return Json(objModel.Getexpsubcategory(expensensub), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpGet]
        public PartialViewResult debitline()
        {
            Retention_Release TypeModel = new Retention_Release();
            //List<Retention_Release> Releaserecordsdebit = new List<Retention_Release>();
            //  Releaserecordsdebit = objModel.debitgrid(Session["ecf"], Session["invoice"]).ToList();
            //TypeModel = objModel.employeedetil();
            //TypeModel.Getexpense = new SelectList(objModel.Getexpenseid(), "expense_gid", "expense_name");
            //TypeModel.Getexpensesubcategory = new SelectList(objModel.Getexpsubcategoryload(), "expensesubcate_gid", "expensesubcate_name");
            //TypeModel.Getexpensecategory = new SelectList(objModel.Getexpcategoryload(), "expensecate_gid", "expensecate_name");
            //TypeModel.Getfc = new SelectList(objModel.Getfcid(), "fc_gid", "fc_name");
            //TypeModel.Getcc = new SelectList(objModel.Getccid(), "cc_gid", "cc_name");
            //TypeModel.Getfccc = new SelectList(objModel.Getfcccid(), "fccc_gid", "fccc_name");           
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult creditline()
        {
            Retention_Release TypeModel = new Retention_Release();
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult Release_Attachment()
        {
            Retention_Release TypeModel = new Retention_Release();
            return PartialView(TypeModel);
        }
        public JsonResult RetentionRelease_Save(Retention_Release Student)
        {
            string[] rearray;
            int retentionserialno = 0;
            string getretionlogserialno = "";
            string result = "";
            getretionlogserialno = objModel.GetSerialNo();
            if (getretionlogserialno != "")
            {
                retentionserialno = int.Parse(getretionlogserialno);
                retentionserialno++;
                Student.serialno = retentionserialno;
            }
            else
            {
                retentionserialno++;
                Student.serialno = retentionserialno;
            }
            //if (Student.Balance_amount == 0)
            //  {
            Student.ecf_raiser = Session["ecf_raiser"].ToString();
            result = objModel.SaveRelease(Student);
            if (result != "")
            {
                rearray = result.Split('-');
                if (rearray.ToString() != "")
                    Session["ecf"] = rearray[0];
                result = objModel.dummyinvoice(Session["ecf"].ToString());
                // result = "Success";
                // }
                // else
                //{
                //     result = "Release Amount Must Be Equal To Credit & Debit Amount";
                //}
                Session["dummy_invoice"] = result;
                Session["invoice"] = rearray[1];
                Session["balance"] = Student.release_amount;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RetentionRelease_creditsave(Retention_Release Student)
        {
            string result = "";
            result = objModel.SaveReleasecredit(Student);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult RetentionRelease_debitsave(Retention_Release Student)
        //{
        //    string result = "";
        //    result = objModel.SaveReleasedebit(Student);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult RetentionRelease_Verify(Retention_Release verify)
        {
            string result = "";
            result = objModel.ReleaseVerifyamount(verify);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetRetentionIndexrpt()
        {
            List<Retention_Release> lstholdrelse = new List<Retention_Release>();
            lstholdrelse = objModel.RetentionReleaseGrid().ToList();
            var jsonResult = Json(new { qrylist = lstholdrelse }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
