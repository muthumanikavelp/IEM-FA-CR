using IEM.Areas.ASMS.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class ModificationController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IRepository objModel;

        public ModificationController()
            : this(new SupDataModel())
        {

        }
        public ModificationController(IRepository objM)
        {
            objModel = objM;
        }

        //
        // GET: /ASMS/Modification/
        [HttpGet]
        public ActionResult Index(string queuefor = null, string requesttype = null, string reqstatus = null, string fromlink = null)
        {            
          //  Session["ModificationQueueSearch"] = null;
            Session["isApproved"] = "";
            Session["isFinancialReviewer"] = null;
            if (queuefor == "edit")
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "1";
                Session["PageMode"] = "2";
            }
            else if (queuefor == "submitmode")
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "2";
                Session["PageMode"] = "4";
            }
            else if (queuefor == "resubmitmode")
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "6";
                Session["PageMode"] = "4";
            }
            else if (queuefor == "view")
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "3";
                Session["PageMode"] = "5";
            }
            else if (queuefor == "approval")
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "4";
                Session["PageMode"] = "3";

            }
            else if (queuefor == "renewal")
            {
                Session["Totalsuppliers"] = null;
                Session["queuefor"] = "5";
                Session["PageMode"] = "6";
                Session["Supplierrenewalpro"] = null;
            }
            else if (queuefor == "totalsuppliers")
            {
                fromlink = "totalsuppliers";
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "7";
                Session["PageMode"] = "5";
                ViewBag.ModificationListTitle = "Total Suppliers";
            }
            else if (queuefor == "active")
            {
                fromlink = "active";
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "8";
                Session["PageMode"] = "5";
                ViewBag.ModificationListTitle = "Active Suppliers";
            }
            else if (queuefor == "inactive")
            {
                fromlink = "inactive";
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "9";
                Session["PageMode"] = "5";
                ViewBag.ModificationListTitle = "InActive Suppliers";
            }
            else if (queuefor == "expired")
            {
                fromlink = "expired";
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "10";
                Session["PageMode"] = "5";
                ViewBag.ModificationListTitle = "Expired Suppliers";
            }
            else if (queuefor == "financereview")
            {
                fromlink = "financereview";
                Session["Supplierrenewalpro"] = null;
                Session["queuefor"] = "11";
                Session["PageMode"] = "5";
                ViewBag.ModificationListTitle = "Pending For Finance Review";
            }
            if (requesttype != null)
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                TempData["requesttype"] = requesttype;
            }
            if (reqstatus != null)
            {
                Session["Totalsuppliers"] = null;
                Session["Supplierrenewalpro"] = null;
                TempData["requeststatus"] = reqstatus;
                if (reqstatus.Trim() == "draft")
                {
                    Session["reqstatus"] = "draft";
                    ViewBag.ModificationListTitle = "Supplier List - Draft";
                }
                else if (reqstatus.Trim() == "inprocess")
                {
                    Session["reqstatus"] = "waitingforapproval";
                    ViewBag.ModificationListTitle = "Supplier List - Inprocess";
                }
                else if (reqstatus.Trim() == "approved")
                {
                    Session["reqstatus"] = "approved";
                    ViewBag.ModificationListTitle = "Supplier List - Approved";
                }
                else if (reqstatus.Trim() == "rejected")
                {
                    Session["reqstatus"] = "rejected";
                    ViewBag.ModificationListTitle = "Supplier List - Rejected";
                }
                else if (reqstatus.Trim() == "waitingforapproval")
                {
                    Session["reqstatus"] = "waitingforapproval";
                    ViewBag.ModificationListTitle = "Supplier List - For My Approval";
                }
            }
            if (fromlink != null)
            {
                Session["reqstatus"] = null;
                if (fromlink.Trim() == "modification")
                {
                    Session["Totalsuppliers"] = null;
                    Session["Supplierrenewalpro"] = null;
                    ViewBag.ModificationListTitle = "Supplier Queue - Modification";
                }
                else if (fromlink.Trim() == "renewal")
                {
                    Session["Totalsuppliers"] = null;
                    Session["Supplierrenewalpro"] = "Renewal";
                    ViewBag.ModificationListTitle = "Supplier Queue - Renewal";
                }
                else if (fromlink.Trim() == "totalsuppliers")
                {
                    Session["Totalsuppliers"] = "totalsuppliers";
                    ViewBag.ModificationListTitle = "Supplier List - Total Suppliers";
                }
                else if (fromlink.Trim() == "active")
                {
                    Session["Totalsuppliers"] = "active";
                    ViewBag.ModificationListTitle = "Supplier List - ACTIVE";
                }
                else if (fromlink.Trim() == "inactive")
                {
                    Session["Totalsuppliers"] = "inactive";
                    ViewBag.ModificationListTitle = "Supplier List - INACTIVE";
                }
                else if (fromlink.Trim() == "expired")
                {
                    Session["Totalsuppliers"] = "expired";
                    ViewBag.ModificationListTitle = "Supplier List - Expired";
                }
                else if (fromlink.Trim() == "financereview")
                {
                    Session["Totalsuppliers"] = "financereview";
                    ViewBag.ModificationListTitle = "Supplier List - Pending For Finance Review";
                }
            }
            AgeingBucket ageing = new AgeingBucket();
            ageing.AgeingBucketData = new SelectList(objModel.GetAgeingBucket().ToList(), "_AgeingBucketValue", "_AgeingBucketText");
            ViewBag.GetAgeingBucket = ageing;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string txtSupCodeMod = null, string txtSupNameMod = null, string txtAgeingBucket = null, string command = null, string commandnew = null)
        {
            List<SupplierHeader> records = new List<SupplierHeader>();
            if (command == "Search" || command == "Refresh")
            {
                records = objModel.GetSupHeaderDetailssearch(txtSupNameMod, txtSupCodeMod, txtAgeingBucket).ToList();
              Session["ModificationQueueSearch"] = records.ToList();
              // TempData["Modification"] = records.ToList();
                AgeingBucket ageing = new AgeingBucket();
                ageing.AgeingBucketData = new SelectList(objModel.GetAgeingBucket().ToList(), "_AgeingBucketValue", "_AgeingBucketText");
                ViewBag.GetAgeingBucket = ageing;
                ViewBag.docval = txtAgeingBucket;
                ViewBag.SupCodeMod = txtSupCodeMod;
                ViewBag.SupNameMod = txtSupNameMod;
            }
            else if (command == "Clear")
            {
                Session["ModificationQueueSearch"] = null;
                AgeingBucket ageing = new AgeingBucket();
                ageing.AgeingBucketData = new SelectList(objModel.GetAgeingBucket().ToList(), "_AgeingBucketValue", "_AgeingBucketText");
                ViewBag.GetAgeingBucket = ageing;
                ViewBag.SupCodeMod = "";
                ViewBag.SupNameMod = "";
            }

            return View();
        }
        public PartialViewResult ModificationQueue()
        {
            return PartialView();
        }
        public PartialViewResult RequestHistory(string SupHeadGid)
        {
            DataTable dt = new DataTable();
            List<SupplierHeader> lst = new List<SupplierHeader>();
            lst = objModel.GetRequestHistory(Convert.ToInt32(SupHeadGid)).ToList();
            dt = objModel.GetSupplierName(Convert.ToInt32(SupHeadGid));
            ViewBag.Code = dt.Rows[0]["supplierheader_suppliercode"].ToString();
            ViewBag.Name = dt.Rows[0]["supplierheader_name"].ToString().ToUpper();
            if (lst.Count > 0)
            {
                ViewBag.nextapprover = dt.Rows[0]["nextapprover"].ToString().ToUpper();
            }
            return PartialView(lst);
        }

    }
}
