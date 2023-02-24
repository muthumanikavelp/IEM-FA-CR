using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class GrnInwardForCwipController : Controller
    {
        //
        // GET: /FLEXIBUY/GrnInwardForCwip/

       private Irepositorypr objModel;
        public GrnInwardForCwipController()
            : this(new fb_DataModelpr())
        {

        }
        public GrnInwardForCwipController(Irepositorypr objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult GrnInwardCwipSummary(string viewfor)
        {
            GRNInward objlist = new GRNInward();
            List<GRNInward> obj = new List<GRNInward>();
            objlist.result = objModel.GetReqGroup();
            objlist.objInwardList = objModel.GetgrnSummary();
            objlist.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
          //  objlist.objInwardList = objModel.GetgrnSummary();
            //objlist.objWoSummary = objModel.GetWoSummary();
            //objlist.objInwardList = objModel.GetgrnSummary();
           
            if(viewfor=="refresh")
            {
                objlist.objInwardList = objModel.GetgrnSummary();
                objlist.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            }
            if (Session["grnheaderlist"] != null)
            {
                objlist.objInwardList = (List<GRNInward>)Session["grnheaderlist"];
            }

            if (objlist.objInwardList.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
          
            //objlist.result = objModel.GetGroupForUser(); 
            return View(objlist);
        }
        [HttpPost]
        public ActionResult GrnInwardCwipSummary(string grndate, string command, string grnrefno, string vendorname, int ddlStatus)
        {
            GRNInward objheader = new GRNInward();
            Session["grnheaderlist"] = null;
            objheader.objInwardList = objModel.GetgrnSummary();
            objheader.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            if (command == "search")
            {
                objheader.statusgid = ddlStatus;
                Session["statusgid"] = ddlStatus;
                if (ddlStatus != null && ddlStatus != 0)
                {
                    //objheader.statusname = ddlStatus.ToString();
                    objheader.statusname = objModel.getStatusName(ddlStatus);
                    objheader.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname", ddlStatus);
                }
                if ((string.IsNullOrEmpty(grndate)) == false)
                {
                    ViewBag.grndate = grndate;
                    objheader.objInwardList = objheader.objInwardList.Where(x => grndate == null ||
                        (x.grnDate.Contains(grndate))).ToList();
                    Session["grnheaderlist"] = objheader.objInwardList;
                }
                if ((string.IsNullOrEmpty(grnrefno)) == false)
                {
                    ViewBag.grnrefno = grnrefno;
                    objheader.objInwardList = objheader.objInwardList.Where(x => grnrefno == null ||
                        (x.grnRefNo.Contains(grnrefno))).ToList();
                    Session["grnheaderlist"] = objheader.objInwardList;
                }

                if ((string.IsNullOrEmpty(objheader.statusname)) == false)
                {
                    ViewBag.statusname = objheader.statusname;
                    objheader.objInwardList = objheader.objInwardList.Where(x => objheader.statusname == null ||
                        (x.grnStatus.Contains(objheader.statusname))).ToList();
                    Session["grnheaderlist"] = objheader.objInwardList;
                }

                if ((string.IsNullOrEmpty(vendorname)) == false)
                {
                    ViewBag.vendorname = vendorname;
                    objheader.objInwardList = objheader.objInwardList.Where(x => vendorname == null ||
                        (x.vendorName.Contains(vendorname))).ToList();
                    Session["poheaderlist"] = objheader.objInwardList;
                }
                if (objheader.objInwardList.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            if (objheader.objInwardList.Count == 0)
            {
                ViewBag.records = "No records Found";
            }
            return View(objheader);
        }
    }
}
