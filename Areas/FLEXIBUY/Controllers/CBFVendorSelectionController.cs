using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class CBFVendorSelectionController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        public CBFVendorSelectionController()
            : this(new CbfSumModel())
        {

        }
        public CBFVendorSelectionController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        public ActionResult CBFVendorselection(int lnid)
        {
            Session["Cbfdetails"] = null;
            CbfSumEntity objPardetails = new CbfSumEntity();
            objPardetails = objRep.Getcbfchecker(lnid.ToString());
            Session["objPardetails"] = objPardetails;
            Session["cbfheadergid"] = lnid;

            objPardetails.productGroupList = new SelectList(objRep.Getproductlist(), "productgroupid", "productgroupidname");
            objPardetails.uomList = new SelectList(objRep.GetUomList(), "uomgid", "uomcode");
            objPardetails.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname", objPardetails.cbfCheckerHeader.requeestForGidChecker);
            objPardetails.projectOwner1 = new SelectList(objRep.GetList(), "projectownergid", "projectownername", objPardetails.cbfCheckerHeader.projectOwnerGidChecker);
            objPardetails.branchCode1 = new SelectList(objRep.GetBranchCode(), "branchcodegid", "branchcodename", objPardetails.cbfCheckerHeader.branchCodeGidChecker);
            objPardetails.Supervisor = new SelectList(objRep.GetSupervisor(), "supervisorGidChecker", "SupervisornameChecker");

            return View(objPardetails);
        }
        [HttpGet]
        public PartialViewResult CbfVendorDetails()
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["CbfChecker"];
            return PartialView(objCrheader);
        }

        [HttpGet]
        public PartialViewResult VendorSelection(string listfor)
        {
            List<vendorselection> lstVendor = new List<vendorselection>();
            if (listfor == "search")
            {
                if (Session["objvendorcapex"] != null)
                    lstVendor = (List<vendorselection>)Session["objvendorcapex"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["vendorcode"] != null)
                    TempData["code"] = TempData["vendorcode"];
                if (TempData["vendorname"] != null)
                    TempData["name"] = TempData["vendorname"];
            }
            else
            {
                Session["objvendorcapex"] = null;
                lstVendor = objRep.getvendorselection();
            }
            return PartialView("VendorSelection", lstVendor);
        }

        [HttpPost]
        public PartialViewResult searchvendorforcapex(vendorselection objsearchfilter)
        {
            List<vendorselection> objvendorsearch = new List<vendorselection>();
            objvendorsearch = objRep.getvendorselection();
            if ((string.IsNullOrEmpty(objsearchfilter.vendorcode)) == false)
            {
                TempData["vendorcode"] = objsearchfilter.vendorcode;
                objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorcode == null ||
                    (x.vendorcode.ToUpper().Contains(objsearchfilter.vendorcode.ToUpper()))).ToList();
                Session["objvendorcapex"] = objvendorsearch;
            }
            if ((string.IsNullOrEmpty(objsearchfilter.vendorname)) == false)
            {
                TempData["vendorname"] = objsearchfilter.vendorname;
                objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorname == null ||
                    (x.vendorname.ToUpper().Contains(objsearchfilter.vendorname.ToUpper()))).ToList();
                Session["objvendorcapex"] = objvendorsearch;
            }
            if (objvendorsearch.Count == 0)
            {
                TempData["Norecords"] = "No records Found";
            }
            return PartialView("VendorSelection", objvendorsearch);
        }

        //public PartialViewResult VendorSelection()
        //{
        //    List<vendorselection> lstVendor = new List<vendorselection>();
        //    lstVendor = objRep.getvendorselection();
        //    return PartialView(lstVendor);
        //}
        [HttpPost]
        public PartialViewResult Vendorsave(CbfCheckerDetails Objcbfdetails)
        {
            try
            {
                CbfSumEntity objcbf = new CbfSumEntity();
                DataTable objdt = new DataTable(); ;
                if (Session["Cbfdetailsvendor"] != null)
                {
                    objdt = (DataTable)Session["Cbfdetailsvendor"];
                    if (!objdt.Columns.Contains("vendorname"))
                    {
                        objdt.Columns.Add("vendorname");
                        objdt.Columns.Add("vendorgid");
                    }
                    if (objdt.Rows.Count > 0)
                    {
                        for (int i = 0; i < objdt.Rows.Count; i++)
                        {
                            if (Objcbfdetails.cbfDetailGidChecker.ToString() == objdt.Rows[i]["cbfdetails_gid"].ToString())
                            {
                                objdt.Rows[i]["vendorname"] = Objcbfdetails.vendorselection;
                                objdt.Rows[i]["vendorgid"] = Objcbfdetails.vendorgid;
                            }

                        }
                    }

                }
                else
                {
                    objdt = (DataTable)Session["Cbfdetailsvendor"];
                }
                Session["vendorselection"] = objdt;
                objcbf = objRep.Getvendorgridcopex(objdt);
                Session["CbfDetails_save"] = objcbf;
                return PartialView("CbfVendorDetails", objcbf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Save()
        {
            string data = objRep.vendorselectionsave();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Viewboqattachment()
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["objPardetails"];
            return PartialView(objCrheader);
        }
        [HttpPost]
        public PartialViewResult Viewboqattachment(string lsCbfAttach)
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["objPardetails"];
            return PartialView(objCrheader);
        }
    }
}
