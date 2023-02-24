using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using IEM.App_Start;

namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class GRNReleaseForPOController : Controller
    {    private Irepositorypr objModel;

    CmnFunctions objCmnFunctions = new CmnFunctions();
    ErrorLog objErrorLog = new ErrorLog();
        public GRNReleaseForPOController()
            : this(new fb_DataModelpr())
        {

        }
        public GRNReleaseForPOController(Irepositorypr objM)
        {
            
            objModel = objM;
        }
        [HttpGet]
        public ActionResult GRNRelease(string viewfor)
        {
            poRaising objlist = new poRaising();
            List<poRaising> obj = new List<poRaising>();
            if (viewfor == "refresh")
            {
                Session["grn_poheaderlist"] = null;
                Session["grn_statusgid"] = null;
            }
            objlist.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
            objlist.groupRes = objModel.GetReqGroup();
            if(objlist.groupRes=="IT" || objlist.groupRes=="PIP")
            {
                objlist.objposummary = objModel.getgrnReleaseForPO(objlist.groupRes);
            }
            else
            {
                objlist.objposummary = obj;
            }
            if (Session["grn_poheaderlist"] != null)
            {
                objlist.objposummary = (List<poRaising>)Session["poheaderlist"];
            }
            string nm = Convert.ToString(Session["grn_statusgid"]);
            if (Session["grn_statusgid"] != null && Session["grn_statusgid"] != "0")
            {
                //objlist.statusgid = (int)Session["statusgid"];
                objlist.statusgid = (int)Session["grn_statusgid"];
            }
              

            if (objlist.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View("GRNRelease",objlist);
        }
        [HttpPost]
        public ActionResult GRNRelease(string podate, string command, string porefno, string vendorname, int ddlStatus)
        {
            poRaising objheader = new poRaising();
            Session["grn_poheaderlist"] = null;
            objheader.groupRes = objModel.GetReqGroup();
            if (objheader.groupRes == "IT" || objheader.groupRes == "PIP")
            {
                objheader.objposummary = objModel.getgrnReleaseForPO(objheader.groupRes).ToList();
            }
            objheader.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
            if (objheader.objposummary != null)
            {
                if (command == "search")
                {
                    objheader.statusgid = ddlStatus;
                    Session["grn_statusgid"] = ddlStatus;
                    if (ddlStatus != null && ddlStatus != 0)
                    {
                        //objheader.statusname = ddlStatus.ToString();
                        objheader.statusname = objModel.getStatusName(ddlStatus);
                        objheader.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname", ddlStatus);
                    }
                    if ((string.IsNullOrEmpty(podate)) == false)
                    {
                        ViewBag.podate = podate;
                        objheader.objposummary = objheader.objposummary.Where(x => podate == null ||
                            (x.poDate.Contains(podate))).ToList();
                        Session["grn_poheaderlist"] = objheader.objposummary;
                    }
                    if ((string.IsNullOrEmpty(porefno)) == false)
                    {
                        ViewBag.porefno = porefno;
                        objheader.objposummary = objheader.objposummary.Where(x => porefno == null ||
                            (x.poRefNo.Contains(porefno))).ToList();
                        Session["grn_poheaderlist"] = objheader.objposummary;
                    }

                    if ((string.IsNullOrEmpty(objheader.statusname)) == false)
                    {
                        ViewBag.statusname = objheader.statusname;
                        objheader.objposummary = objheader.objposummary.Where(x => objheader.statusname == null ||
                            (x.status.Contains(objheader.statusname))).ToList();
                        Session["grn_poheaderlist"] = objheader.objposummary;
                    }

                    if ((string.IsNullOrEmpty(vendorname)) == false)
                    {
                        ViewBag.vendorname = vendorname;
                        objheader.objposummary = objheader.objposummary.Where(x => vendorname == null ||
                            (x.vendorName.Contains(vendorname))).ToList();
                        Session["grn_poheaderlist"] = objheader.objposummary;
                    }
                    if (objheader.objposummary.Count == 0)
                    {
                        ViewBag.records = "No records Found";
                    }
                }
                if (objheader.objposummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            else
            {
                ViewBag.records = "No records Found";
            }
            
           
            return View(objheader);
        }
        [HttpPost]
        public JsonResult GrnReleaseFull(string pogid = null) 
        {
            string result = string.Empty; 
            string ErrorMsg = string.Empty;
            try
            {
                int pogid1 = Convert.ToInt32(string.IsNullOrEmpty(pogid)?"0":pogid);
                if (pogid1 > 0)
                    ErrorMsg = objModel.GrnReleaseFull(pogid1);
                else
                    ErrorMsg = "0";
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "0";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
