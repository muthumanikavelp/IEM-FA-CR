using IEM.Areas.FLEXIBUY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class cbfselectionController : Controller
    {
        //
        // GET: /FLEXIBUY/cbfselection/
         private Irepositorypr objModel;
        public cbfselectionController()
            : this(new fb_DataModelpr())
        {

        }
        public cbfselectionController(Irepositorypr objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult cbfselection()
        {
            cbfselection objdetails = new cbfselection();
            objdetails.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname");
            objdetails.result = objModel.GetReqGroup();
            objdetails.cbfsummary = objModel.getcbfheader(objdetails.result);
            Session["da"] = null;
            if (Session["cbfheaderlist"] != null)
            {
                objdetails.cbfsummary = (List<cbfselection>)Session["cbfheaderlist"];
            }
            if (Session["requestgid"] != null)
                objdetails.requestforgid = (int)Session["requestgid"];
            if (objdetails.cbfsummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
          
            return View(objdetails);
        }

        [HttpPost]
        public ActionResult cbfselection(string cbfno,string cbfdate,string cbfenddate,int ddldepartment,string command)
        {
            cbfselection objcbfsearch =  new cbfselection();
            Session["cbfheaderlist"] = null;
            objcbfsearch.result = objModel.GetReqGroup();
            objcbfsearch.cbfsummary = objModel.getcbfheader(objcbfsearch.result).ToList();
            objcbfsearch.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname");
            if (command =="search")
            {
                objcbfsearch.requestforgid = ddldepartment;
                Session["requestgid"] = ddldepartment;
                if (ddldepartment != null && ddldepartment != 0)
                {
                    //objheader.statusname = ddlStatus.ToString();
                    objcbfsearch.requestforname = objModel.getrequestName(ddldepartment);
                    objcbfsearch.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname", ddldepartment);
                }
                //objcbfsearch.requestforname = objModel.getrequestName(objcbfsearch.requestforgid);
                if ((string.IsNullOrEmpty(cbfno)) == false)
                {
                    ViewBag.cbfno = objcbfsearch.cbfno;
                    objcbfsearch.cbfsummary = objcbfsearch.cbfsummary.Where(x => cbfno == null ||
                        (x.cbfno.Contains(cbfno))).ToList();
                    Session["cbfheaderlist"] = objcbfsearch.cbfsummary;
                }
                if ((string.IsNullOrEmpty(cbfdate)) == false)
                {
                    ViewBag.cbfdate = objcbfsearch.cbfdate;
                    objcbfsearch.cbfsummary = objcbfsearch.cbfsummary.Where(x => cbfdate == null ||
                        (x.cbfdate.Contains(cbfdate))).ToList();
                    Session["cbfheaderlist"] = objcbfsearch.cbfsummary;
                }
                if ((string.IsNullOrEmpty(cbfenddate)) == false)
                {
                    ViewBag.cbfenddate = cbfenddate;
                    objcbfsearch.cbfsummary = objcbfsearch.cbfsummary.Where(x => cbfenddate == null ||
                        (x.cbfEnddate.Contains(cbfenddate))).ToList();
                    Session["cbfheaderlist"] = objcbfsearch.cbfsummary;
                }
                if ((string.IsNullOrEmpty(objcbfsearch.requestforname)) == false)
                {
                    ViewBag.statusname = objcbfsearch.requestforname;
                    objcbfsearch.cbfsummary = objcbfsearch.cbfsummary.Where(x => objcbfsearch.requestforname == null ||
                        (x.department.Contains(objcbfsearch.requestforname))).ToList();
                    Session["cbfheaderlist"] = objcbfsearch.cbfsummary;
                }

                if (objcbfsearch.cbfsummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            return View(objcbfsearch);
        }
        [HttpGet]
        public PartialViewResult cbfdetails()
        {
            cbfdetail obj = new cbfdetail();
            obj.lstcbfdetail=(List<cbfdetail>)Session["objlist"];
            if(obj.lstcbfdetail.Count==0)
            {
                ViewBag.records = "No records Found";
            }
            if (Session["cbfheadamount"] != null)
                obj.cbfheadAmount = (Decimal)Session["cbfheadamount"];
            return PartialView("cbfdetails", obj);

        }
        [HttpPost]
        public JsonResult cbfdetails(cbfselection objcbf)
        {
            try
            {

                DataTable dt = new DataTable();
               // DataTable dt1 = new DataTable();
                List<cbfdetail> objlist = new List<cbfdetail>();
                cbfdetail obj = new cbfdetail();
                dt = objModel.getTable(objcbf);
                obj.lstcbfdetail = objModel.getcbflist(dt);
                Session["cbfheadamount"] = objcbf.cbfamount;
                //if (Session["da"] != null)
                //{
                //    dt1 = (DataTable)Session["da"];
                //}
                //if (objcbf.cbfheadgid != 0)
                //{
                //    dt = objModel.getTable(objcbf);
                //    if (dt != null)
                //    {
                //        dt1.Merge(dt);
                //    }
                //    Session["da"] = dt1;
                //    objlist = objModel.getcbflist(dt1);
                //    if (objlist.Count == 0)
                //    {
                //        ViewBag.records = "No records Found";
                //    }
                //}
                //else if (objcbf.uncheckgid != 0)
                //{
                //    DataView dv = new DataView();
                //    dv = dt1.DefaultView;
                //    dv.RowFilter = " cbfdetails_cbfhead_gid <>" + objcbf.uncheckgid + "";
                //    dt = dv.ToTable();

                //    Session["da"] = dt;
                //    objlist = objModel.getcbflist(dt);
                //    if (objlist.Count == 0)
                //    {
                //        ViewBag.records = "No records Found";
                //    }
                //}
                //if(obj.lstcbfdetail.Count==0)
                //{
                //    ViewBag.records = "No records Found";
                //}
                Session["objlist"] = obj.lstcbfdetail;
                return Json(obj,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ViewboqattachmentForPo()
        {
            //ViewBag.viewfor = Session["viewfor"];
            ViewBOQ objBoq = new ViewBOQ();
            int cbfgid = Convert.ToInt32(Request.QueryString["id"].ToString());
            objBoq.boqlist = objModel.GetAttachmentDetails(cbfgid);
            return PartialView(objBoq);
        }

        public JsonResult DownloadDocument(ViewBOQ objBoq)
        {
            Session["downfile"] = objBoq.poFileName;
            // ViewBOQ obj1 = new ViewBOQ();
            return Json(objBoq, JsonRequestBehavior.AllowGet);
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        public FileResult Download(fb_Entitypr obj)
        {

            try
            {
                string txt1 = Session["downfile"].ToString();
                string directory = HoldFileUploadUrlDSA() + txt1.ToString();
                byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                string fileName = "Download" + txt1.ToString();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
