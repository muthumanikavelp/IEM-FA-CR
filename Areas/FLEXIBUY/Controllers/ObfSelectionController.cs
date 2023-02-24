using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class ObfSelectionController : Controller
    {
        //
        // GET: /FLEXIBUY/ObfSelection/
        private Irepositorypr objModel;
        public ObfSelectionController()
            : this(new fb_DataModelpr())
        {

        }
        public ObfSelectionController(Irepositorypr objM)
        {
            objModel = objM;
        }
        public ActionResult obfselection()
        {
            Session["WOHeaderGid"] = null;
            obfselection objdetails = new obfselection();
            objdetails.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname");
            objdetails.result = objModel.GetReqGroup();
            objdetails.obfsummary = objModel.getobfheader(objdetails.result);
            Session["da"] = null;
            if (Session["obfheaderlist"] != null)
            {
                objdetails.obfsummary = (List<obfselection>)Session["obfheaderlist"];
            }
            if (Session["requestgid"] != null)
                objdetails.requestforgid = (int)Session["requestgid"];
            if (objdetails.obfsummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(objdetails);
        }

        [HttpPost]
        public ActionResult obfselection(string obfno, string obfdate, string obfenddate, int ddldepartment, string command)
        {
            obfselection objobfsearch = new obfselection();
            Session["obfheaderlist"] = null;
            objobfsearch.result = objModel.GetReqGroup();
            objobfsearch.obfsummary = objModel.getobfheader(objobfsearch.result).ToList();
            objobfsearch.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname");
            if (command == "search")
            {
                objobfsearch.requestforgid = ddldepartment;
                Session["requestgid"] = ddldepartment;
                if (ddldepartment != null && ddldepartment != 0)
                {
                    //objheader.statusname = ddlStatus.ToString();
                    objobfsearch.requestforname = objModel.getrequestName(ddldepartment);
                    objobfsearch.requestlist = new SelectList(objModel.getrequestfor(), "requestforgid", "requestforname", ddldepartment);
                }
                //objobfsearch.requestforname = objModel.getrequestName(objobfsearch.requestforgid);
                if ((string.IsNullOrEmpty(obfno)) == false)
                {
                    ViewBag.obfno = objobfsearch.obfno;
                    objobfsearch.obfsummary = objobfsearch.obfsummary.Where(x => obfno == null ||
                        (x.obfno.Contains(obfno))).ToList();
                    Session["obfheaderlist"] = objobfsearch.obfsummary;
                }
                if ((string.IsNullOrEmpty(obfdate)) == false)
                {
                    ViewBag.obfdate = objobfsearch.obfdate;
                    objobfsearch.obfsummary = objobfsearch.obfsummary.Where(x => obfdate == null ||
                        (x.obfdate.Contains(obfdate))).ToList();
                    Session["obfheaderlist"] = objobfsearch.obfsummary;
                }
                if ((string.IsNullOrEmpty(obfenddate)) == false)
                {
                    ViewBag.obfenddate = obfenddate;
                    objobfsearch.obfsummary = objobfsearch.obfsummary.Where(x => obfenddate == null ||
                        (x.obfEnddate.Contains(obfenddate))).ToList();
                    Session["obfheaderlist"] = objobfsearch.obfsummary;
                }
                if ((string.IsNullOrEmpty(objobfsearch.requestforname)) == false)
                {
                    ViewBag.statusname = objobfsearch.requestforname;
                    objobfsearch.obfsummary = objobfsearch.obfsummary.Where(x => objobfsearch.requestforname == null ||
                        (x.department.Contains(objobfsearch.requestforname))).ToList();
                    Session["obfheaderlist"] = objobfsearch.obfsummary;
                }

                if (objobfsearch.obfsummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            return View(objobfsearch);
        }
        [HttpGet]
        public PartialViewResult obfdetails()
        {
            obfdetail obj = new obfdetail();
            obj.lstobfdetail = (List<obfdetail>)Session["objlist"];
            if (obj.lstobfdetail.Count == 0)
            {
                ViewBag.records = "No records Found";
            }
            return PartialView("obfdetails", obj);
        }
        [HttpPost]
        public JsonResult obfdetails(obfselection objobf)
        {
            try
            {

                DataTable dt = new DataTable();
                // DataTable dt1 = new DataTable();
                List<obfdetail> objlist = new List<obfdetail>();
                obfdetail obj = new obfdetail();
                dt = objModel.getObfTable(objobf);  
                obj.lstobfdetail  = objModel.getobflist(dt);
                if (Session["objlist"] != null)
                {
                    objlist = (List<obfdetail>)Session["objlist"];
                    obj.lstobfdetail.AddRange(objlist);

                }
                Session["objlist"] = obj.lstobfdetail;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
