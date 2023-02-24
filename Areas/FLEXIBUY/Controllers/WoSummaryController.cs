using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IEM.Common;
using IEM.Areas.FLEXIBUY.Models;
using System.Web.UI.WebControls;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class WoSummaryController : Controller
    {
        //
        // GET: /FLEXIBUY/WOsummary/
        private Irepositorypr objModel;
        public WoSummaryController()
            : this(new fb_DataModelpr())
        {

        }
        public WoSummaryController(Irepositorypr objM)
        {
            objModel = objM;
        }
        ErrorLog objErrorLog = new ErrorLog();
        [HttpGet]
        public ActionResult WoSummary()
        {
            Session["WOHeaderGid"] = null;
            Session["objlist"] = null;
            WoSummary objlist = new WoSummary();
            try
            {              
                objlist.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
                objlist.objWoSummary = objModel.GetWoSummary();
                if (Session["woheaderlist"] != null)
                {
                    objlist.objWoSummary = (List<WoSummary>)Session["woheaderlist"];
                }
                if (Session["statusgid"] != null)
                    objlist.statusgid = (int)Session["statusgid"];
                if (objlist.objWoSummary.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                objlist.grpRes = objModel.GetGroupForUserWO();
                return View(objlist);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(objlist); 
            }
        }

        //[HttpGet]
        //public ActionResult WoSummary(string docnum = null)
        //{
        //    WoSummary objlist = new WoSummary();
        //    objlist.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
        //    objlist.objWoSummary = objModel.GetWoSummary();
        //    if (docnum != null)
        //    {
        //        if ((string.IsNullOrEmpty(docnum)) == false)
        //        {
        //            ViewBag.worefno = docnum;
        //            objlist.objWoSummary = objlist.objWoSummary.Where(x => docnum == null ||
        //                (x.woRefNo.Contains(docnum))).ToList();
        //            Session["woheaderlist"] = objlist.objWoSummary;
        //        }
        //    }
        //    if (Session["woheaderlist"] != null)
        //    {
        //        objlist.objWoSummary = (List<WoSummary>)Session["woheaderlist"];
        //    }
        //    if (Session["statusgid"] != null)
        //        objlist.statusgid = (int)Session["statusgid"];
        //    if (objlist.objWoSummary.Count == 0)
        //    {
        //        ViewBag.records = "No Records Found";
        //    }
        //    objlist.grpRes = objModel.GetGroupForUser();
        //    return View(objlist);
        //}

        [HttpPost]
        public ActionResult WoSummary(string wodate, string command, string worefno, string vendorname, int ddlStatus)
        {
            WoSummary objheader = new WoSummary();
            try
            {              
                Session["woheaderlist"] = null;
                objheader.objWoSummary = objModel.GetWoSummary().ToList();
                objheader.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
                if (command == "search")
                {
                    objheader.statusgid = ddlStatus;
                    Session["statusgid"] = ddlStatus;
                    if (ddlStatus != null && ddlStatus != 0)
                    {
                        //objheader.statusname = ddlStatus.ToString();
                        objheader.statusname = objModel.getStatusName(ddlStatus);
                        objheader.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname", ddlStatus);
                    }
                    if ((string.IsNullOrEmpty(wodate)) == false)
                    {
                        ViewBag.wodate = wodate;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => wodate == null ||
                            (x.woDate.Contains(wodate))).ToList();
                        Session["woheaderlist"] = objheader.objWoSummary;
                    }
                    if ((string.IsNullOrEmpty(worefno)) == false)
                    {
                        ViewBag.worefno = worefno;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => worefno == null ||
                            (x.woRefNo.Contains(worefno))).ToList();
                        Session["woheaderlist"] = objheader.objWoSummary;
                    }

                    if ((string.IsNullOrEmpty(objheader.statusname)) == false)
                    {
                        ViewBag.statusname = objheader.statusname;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => objheader.statusname == null ||
                            (x.status.Contains(objheader.statusname))).ToList();
                        Session["woheaderlist"] = objheader.objWoSummary;
                    }

                    if ((string.IsNullOrEmpty(vendorname)) == false)
                    {
                        ViewBag.vendorname = vendorname;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => vendorname == null ||
                            (x.vendorName.Contains(vendorname))).ToList();
                        Session["woheaderlist"] = objheader.objWoSummary;
                    }
                    if (objheader.objWoSummary.Count == 0)
                    {
                        ViewBag.records = "No records Found";
                    }
                }
                if (objheader.objWoSummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
                Session["woexceldownload"] = objheader.objWoSummary;
                objheader.grpRes = objModel.GetGroupForUser();
                return View(objheader);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(objheader);
            }
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<WoSummary> cList;
            WoSummary obj = new WoSummary();
           
                obj.objWoSummary = objModel.GetWoSummary().ToList();

            if (Session["woexceldownload"] == null)
            {
                cList = obj.objWoSummary;
            }
            else
            {
                cList = (List<WoSummary>)Session["woexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("WO Date");
            dt.Columns.Add("WO No ");
            dt.Columns.Add("Vendor Name");            
            dt.Columns.Add("Status");
            dt.Columns.Add("WO Amount ");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].woDate.ToString()
                , cList[i].woRefNo.ToString()
                , cList[i].vendorName.ToString()
                , cList[i].status.ToString()
                , cList[i].woAmount.ToString()
               
                );
               //   grid.Column("", obj.Sorter("woDate", "WO Date", grid), style: "MidColumn10"),
               //grid.Column("", obj.Sorter("woRefNo", "WO Ref No", grid), style: "MidColumn20"),
               //        grid.Column("", obj.Sorter("wovendorName", "Vendor Name", grid), style: "MidColumn30"),
               //        grid.Column("", obj.Sorter("wostatus", "Status", grid), style: "MidColumn10"),
               //grid.Column("", obj.Sorter("woAmount", "WO Amount", grid), style: "MidColumn10"),
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult(gv, "WO Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
