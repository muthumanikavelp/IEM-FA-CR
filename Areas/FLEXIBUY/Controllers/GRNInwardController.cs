using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;
using System.Web.UI.WebControls;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class GRNInwardController : Controller
    {
        //
        // GET: /FLEXIBUY/PoReleaseForGRN/

        private Irepositorypr objModel;
        public GRNInwardController()
            : this(new fb_DataModelpr())
        {

        }
        public GRNInwardController(Irepositorypr objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult GrnInwardSummary(string viewfor)
        {
            GRNInward objlist = new GRNInward();
            objlist.objInwardList = objModel.GetgrnSummary();
            objlist.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            //objlist.objWoSummary = objModel.GetWoSummary();
           // objlist.objInwardList = objModel.GetgrnSummary();
            if(viewfor=="refresh")
            {
                objlist.objInwardList = objModel.GetgrnSummary();
                Session["grnheaderlist"] = null;
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
              
            return View(objlist);
        }
        [HttpPost]
        public ActionResult GrnInwardSummary(string grndate, string command, string grnrefno, string vendorname, int ddlStatus)
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
                Session["grnexceldownload"] = objheader.objInwardList;
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

        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<GRNInward> cList;
            GRNInward obj = new GRNInward();
           
            obj.objInwardList = objModel.GetgrnSummary();

            if (Session["grnexceldownload"] == null)
            {
                cList = obj.objInwardList;
            }
            else
            {
                cList = (List<GRNInward>)Session["grnexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("GRN Date");
            dt.Columns.Add("GRN No");
            dt.Columns.Add("PO No");
            dt.Columns.Add("PO Version");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("PO Amount");
            dt.Columns.Add("Status");
            
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].grnDate.ToString()
                , cList[i].grnRefNo.ToString()
                , cList[i].poRefNo.ToString()
                , cList[i].poVersion.ToString()
                , cList[i].vendorName.ToString()
                 , cList[i].productName.ToString()
                , cList[i].poAmount.ToString()
                , cList[i].grnStatus.ToString()               
                );

       
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult(gv, "GRN Inward Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
