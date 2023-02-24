using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class SearchFilterController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objuid = new CommonIUD();

        private IRepositoryKIR objrep;

        public SearchFilterController()
            : this(new prsummodel())
        { }
        public SearchFilterController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetparDetails()
        {
            List<flexibuydashboard> obj = new List<flexibuydashboard>();
            try
            {
                obj = objrep.getMyRequestquery().ToList();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult downloadsexcel()
        {
            string mt = null;
            string txtdbdocdate = null;
            string txtdocnumber = null;
            string txtdbdocamount = null;
            string txtdbdocraiser = null;
            string ddldbDocStatus = null;
            List<flexibuydashboard> cList;
            flexibuydashboard viewbags = new flexibuydashboard();
            if (Session["DashSearchfilter_fb"] == null)
            {
                cList = objrep.getMyRequestqueryExcel(txtdbdocdate, txtdocnumber, txtdbdocamount, txtdbdocraiser, ddldbDocStatus, txtdbdocraiser).ToList();
                //cList = objrep.GetDocName(mt).ToList();
            }
            else
            {
                cList = (List<flexibuydashboard>)Session["DashSearchfilter_fb"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Doc Type");
            dt.Columns.Add("Doc No.");
            dt.Columns.Add("Doc Date");
            dt.Columns.Add("Raiser");
            dt.Columns.Add("Description");
            dt.Columns.Add("Status");
            dt.Columns.Add("Amount");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].category.ToString()
                    , cList[i].docNo.ToString()
                    , cList[i].ddate.ToString()
                , cList[i].raiser.ToString()
                , cList[i].description.ToString()
                , cList[i].status.ToString()
                , cList[i].amount.ToString());
            }
            // export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            viewbags.StatusTypeName = ddldbDocStatus;
            Session["request"] = viewbags;
            Session["DashSearchfilter_fb1"] = gv;
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["DashSearchfilter_fb1"], "SearchResult.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
