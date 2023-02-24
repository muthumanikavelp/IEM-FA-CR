using IEM.Areas.FLEXIBUY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class ParSummaryController : Controller
    {
        private IrepositoryAn objrep;
        public ParSummaryController()
            : this(new CbfSumModel())
        {

        }
        public ParSummaryController(IrepositoryAn objM)
        {
            objrep = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            CbfSumEntity obj;
            obj = objrep.GetParSummary();
            obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
            if (Session["parheaderlist"] != null)
            {
                obj = (CbfSumEntity)Session["parheaderlist"];
            }
            if (Session["statusgid"] != null)
                obj.statusgid = (int)Session["statusgid"];
            //Session["PARSUMMARY"] = obj;
            return View(obj);
        }
        //[HttpGet]
        //public ActionResult Index(string docnum = null)
        //{
        //    CbfSumEntity obj;
        //    obj = objrep.GetParSummary();
        //    obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
        //    if (docnum != null)
        //    {
        //        if ((string.IsNullOrEmpty(docnum)) == false)
        //        {
        //            ViewBag.txtparno = docnum;
        //            obj.ListParHeader = obj.ListParHeader.Where(x => docnum == null ||
        //                (x.ParNo.Contains(docnum))).ToList();
        //            Session["parheaderlist"] = obj;
        //        }
        //    }
        //    if (Session["parheaderlist"] != null)
        //    {
        //        obj = (CbfSumEntity)Session["parheaderlist"];
        //    }
        //    if (Session["statusgid"] != null)
        //        obj.statusgid = (int)Session["statusgid"];
        //    //Session["PARSUMMARY"] = obj;
        //    return View(obj);
        //}
        
        [HttpPost]
        public ActionResult Index(string txtparno, string command, string txtpardate, int txtstatus)
        {
            CbfSumEntity obj;
            Session["parheaderlist"] = null;
            obj = objrep.GetParSummary();
            obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
            if (command == "search")
            {
                obj.statusgid = txtstatus;
                Session["statusgid"] = txtstatus;
                if (txtstatus != null && txtstatus != 0)
                {
                    //objheader.statusname = ddlStatus.ToString();
                    obj.statusname = objrep.getStatusName(txtstatus);
                    obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname", txtstatus);
                }
                if ((string.IsNullOrEmpty(txtparno)) == false)
                {
                    ViewBag.txtparno = txtparno;
                    obj.ListParHeader = obj.ListParHeader.Where(x => txtparno == null ||
                        (x.ParNo.Contains(txtparno))).ToList();
                    Session["parheaderlist"] = obj;
                }
                if ((string.IsNullOrEmpty(txtpardate)) == false)
                {
                    ViewBag.txtpardate = txtpardate;
                    obj.ListParHeader = obj.ListParHeader.Where(x => txtpardate == null ||
                        (x.ParDate.Contains(txtpardate))).ToList();
                    Session["parheaderlist"] = obj;
                }

                if ((string.IsNullOrEmpty(obj.statusname)) == false)
                {
                    ViewBag.statusname = obj.statusname;
                    obj.ListParHeader = obj.ListParHeader.Where(x => obj.statusname == null ||
                        (x.status.Contains(obj.statusname))).ToList();
                    Session["parheaderlist"] = obj;
                }
                if (obj.ListParHeader.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            Session["parexceldownload"] = obj.ListParHeader;
            if (obj.ListParHeader.Count == 0)
            {
                ViewBag.records = "No records Found";
            }
            return View(obj);
        }

        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<Parheader> cList;
            CbfSumEntity obj = new CbfSumEntity();
            obj = objrep.GetParSummary();

            if (Session["parexceldownload"] == null)
            {
                cList = obj.ListParHeader;
            }
            else
            {
                cList = (List<Parheader>)Session["parexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("PAR No");
            dt.Columns.Add("PAR Date");           
            dt.Columns.Add("Status");
            dt.Columns.Add("Description ");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].ParNo.ToString()
                , cList[i].ParDate.ToString()
                , cList[i].status.ToString()
                , cList[i].pardescription.ToString()
                );
            }
                       

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult(gv, "PAR Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
