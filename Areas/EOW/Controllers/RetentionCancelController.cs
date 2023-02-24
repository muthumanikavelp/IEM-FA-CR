using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers

{

    [NoDirectAccess]
    public class RetentionCancelController : Controller
    {
        DataTable dt = new DataTable();
        RetentionCancelData sub = new RetentionCancelData();
        private RetentionCancel objModel;
        String result = String.Empty;
        public RetentionCancelController()
            : this(new RetentionCancelModel())
        {

        }
        public RetentionCancelController(RetentionCancel objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<RetentionCancelData> Releaserecords = new List<RetentionCancelData>();
            Releaserecords = objModel.RetentionReleaseGrid().ToList();
            if (Releaserecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }

            return View(Releaserecords);
        }
        [HttpPost]
        public ActionResult Index(string ReleaseDate = null, string ECFDate = null, string ECFNo = null, string InvoiceNo = null, string Suppliercode = null, string Suppliername = null, string extendeddate = null)
        {
            List<RetentionCancelData> Releaserecords = new List<RetentionCancelData>();
            Releaserecords= objModel.Search( ReleaseDate,ECFDate,ECFNo,InvoiceNo,Suppliercode,Suppliername,extendeddate).ToList();
            return View(Releaserecords);
        }
        [HttpGet]
        public PartialViewResult Cancel(int id)
        {
            Session["InvoiceGid"]=id;
            RetentionCancelData Releaserecords = new RetentionCancelData();
            Releaserecords = objModel.SelectById(id);
            return PartialView(Releaserecords);

        }
        public JsonResult CancelUpdate(RetentionCancelData ret)
        {
            if (Session["InvoiceGid"]!=null)
            {
                ret.invoice_gid =Convert.ToInt16(Session["InvoiceGid"]);
            }
            result= objModel.CancelUpdate(ret);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}
