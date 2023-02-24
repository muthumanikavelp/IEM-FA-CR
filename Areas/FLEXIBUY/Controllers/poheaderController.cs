using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class poheaderController : Controller
    {
        //
        // GET: /FLEXIBUY/poheader/
        private Irepositorypr objModel;
        
        public poheaderController()
            : this(new fb_DataModelpr())
        {

        }
        public poheaderController(Irepositorypr objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index(string viewfor)
        {
            poRaising objlist = new poRaising();

            if (viewfor == "refresh")
            {
                Session["poheaderlist"] = null;
                Session["statusgid"] = null;
                Session["objlist"] = null;
            }
            objlist.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
            objlist.objposummary = objModel.getpoSummary();
            if (ViewBag.search == "search")
            {
                if (Session["poheaderlist"] != null)
                {
                    objlist.objposummary = (List<poRaising>)Session["poheaderlist"];
                }
                if (Session["statusgid"] != null)
                    objlist.statusgid = (int)Session["statusgid"];
            }
            if (objlist.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            //Session["FlexiDash"] = null; 
            objlist.groupRes = objModel.GetGroupForUserPO();
            return View(objlist);
        }

        //[HttpGet]
        //public ActionResult Index(string viewfor, string docnum = null)
        //{
        //    poRaising objlist = new poRaising();

        //    if (viewfor == "refresh")
        //    {
        //        Session["poheaderlist"] = null;
        //        Session["statusgid"] = null;
        //    }
        //    objlist.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
        //    objlist.objposummary = objModel.getpoSummary();
        //    if (docnum != null)
        //    {
        //        if ((string.IsNullOrEmpty(docnum)) == false)
        //        {
        //            ViewBag.porefno = docnum;
        //            objlist.objposummary = objlist.objposummary.Where(x => docnum == null ||
        //                (x.poRefNo.Contains(docnum))).ToList();
        //            Session["poheaderlist"] = objlist.objposummary;
        //        }
        //    }
        //    if (Session["poheaderlist"] != null)
        //    {
        //        objlist.objposummary = (List<poRaising>)Session["poheaderlist"];
        //    }
        //    if (Session["statusgid"] != null)
        //        objlist.statusgid = (int)Session["statusgid"];

        //    if (objlist.objposummary.Count == 0)
        //    {
        //        ViewBag.records = "No Records Found";
        //    }
        //    objlist.groupRes = objModel.GetGroupForUser();
        //    return View(objlist);
        //}

        [HttpPost]
        public ActionResult Index(string podate,string command, string porefno, string vendorname, int ddlStatus)
        {
            poRaising objheader = new poRaising();
            Session["poheaderlist"] = null;
            objheader.objposummary = objModel.getpoSummary().ToList();
            objheader.statusList = new SelectList(objModel.getStatus(), "statusgid", "statusname");
            ViewBag.search = "search";
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
                if ((string.IsNullOrEmpty(podate)) == false)
                {
                    ViewBag.podate = podate;
                    objheader.objposummary = objheader.objposummary.Where(x => podate == null ||
                        (x.poDate.Contains(podate))).ToList();
                    Session["poheaderlist"] = objheader.objposummary;
                }
                if ((string.IsNullOrEmpty(porefno)) == false)
                {
                    ViewBag.porefno = porefno;
                    objheader.objposummary = objheader.objposummary.Where(x => porefno == null ||
                        (x.poRefNo.Contains(porefno))).ToList();
                    Session["poheaderlist"] = objheader.objposummary;
                }

                if ((string.IsNullOrEmpty(objheader.statusname)) == false)
                {
                    ViewBag.statusname = objheader.statusname;
                    objheader.objposummary = objheader.objposummary.Where(x => objheader.statusname == null ||
                        (x.status.Contains(objheader.statusname))).ToList();
                    Session["poheaderlist"] = objheader.objposummary;
                }

                if ((string.IsNullOrEmpty(vendorname)) == false)
                {
                    ViewBag.vendorname = vendorname;
                    objheader.objposummary = objheader.objposummary.Where(x => vendorname == null ||
                        (x.vendorName.Contains(vendorname))).ToList();
                    Session["poheaderlist"] = objheader.objposummary;
                }
                if (objheader.objposummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
                Session["poexceldownload"] = objheader.objposummary;
               }
                if (objheader.objposummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
                objheader.groupRes = objModel.GetGroupForUser();
                return View(objheader);
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<poRaising> cList;
            poRaising obj = new poRaising();



            if (Session["poexceldownload"] == null)
            {
                cList = objModel.getpoSummary().ToList();
            }
            else
            {
                cList = (List<poRaising>)Session["poexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");

            dt.Columns.Add("PO No ");
            dt.Columns.Add("PO Date ");
            dt.Columns.Add("PO Version");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Status");
            dt.Columns.Add("PO Amount ");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1

                , cList[i].poRefNo.ToString()
                , cList[i].poDate.ToString()
                , cList[i].poVersion.ToString()
                , cList[i].vendorName.ToString()
                , cList[i].status.ToString()
                , cList[i].poAmount.ToString()
                );
            }

            //grid.Column("poDate", obj.Sorter("", "PO Date", grid), style: "colSmallCenter"),
            //                 grid.Column("", obj.Sorter("poRefNo", "PO Ref No", grid), style: "numcolumn"),
            //                      grid.Column("", obj.Sorter("poVersion", "", grid), style: "centerAlign"),
            //                 grid.Column("", obj.Sorter("povendorName", "Vendor Name", grid), style: "collargeBreak"),
            //                 grid.Column("", obj.Sorter("postatus", "", grid), style: "colmedium"),
            //                 //grid.Column("", obj.Sorter("poAmount", "PO Amount", grid), style: "colamountlarge"),
            //                grid.Column("", obj.Sorter("poAmount", "", grid), format: @<text>  @Html.Raw(String.Format("{0:#,##}", obj.GetINRAmount(Convert.ToString(@item.poAmount)))) </text>, canSort: true, style: "colamountlarge"),

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult(gv, "PO Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
