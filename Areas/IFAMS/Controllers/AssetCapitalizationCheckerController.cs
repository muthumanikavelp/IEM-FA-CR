using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetCapitalizationCheckerController : Controller
    {
        CmnFunctions cmnfunc = new CmnFunctions();
        private Ifams_Repository ccr;

        public AssetCapitalizationCheckerController()
            : this(new Ifams_Model())
        {

        }

        public AssetCapitalizationCheckerController(Ifams_Repository ccrr)
        {
            ccr = ccrr;
        }
        public ActionResult AssetCapitalizationChecker()
        {
            capitalizationMaker obj = new capitalizationMaker();
            // List<capitalizationMaker> getInvoice = new List<capitalizationMaker>();

            obj.Getapplist = ccr.getapprovedetailswait();
            //if (obj.Getdetapplist.Count==0)
            //{
            //     obj.Getdetapplist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
            //}
          
            // obj.Getapplist = getInvoice;
            Session["chklist"] = obj.Getapplist;
            return View(obj);
        }

        [HttpPost]

        public ActionResult AssetCapitalizationChecker(capitalizationMaker gobj, string Refersh)
        {
            gobj.Getapplist = (List<capitalizationMaker>)Session["chklist"];
            try
            {
                
                if (Refersh != "Refersh")
                {
                if ((string.IsNullOrEmpty(gobj.EcfNo)) == false)
                {
                    //ViewBag.txtSuppliercode = txtSuppliercode;
                    gobj.Getapplist = gobj.Getapplist.Where(x => gobj.EcfNo == null ||
                       (x.EcfNo.ToUpper().Contains(gobj.EcfNo.ToUpper()))).ToList();
                    // Session["cmaker"] = objm.CapitalizationList;
                }
                if ((string.IsNullOrEmpty(gobj.invoiceno)) == false)
                {
                    gobj.Getapplist = gobj.Getapplist.Where(x => gobj.invoiceno == null ||
                      (x.invoiceno.ToUpper().Contains(gobj.invoiceno.ToUpper()))).ToList();
                    //Session["cmaker"] = objm.CapitalizationList;
                }
              
                if ((string.IsNullOrEmpty(gobj.Vendor)) == false)
                {
                    gobj.Getapplist = gobj.Getapplist.Where(x => gobj.Vendor == null ||
                       (x.Vendor.ToUpper().Contains(gobj.Vendor.ToUpper()))).ToList();
                    // Session["cmaker"] = objm.CapitalizationList;
                }

                }
                else
                {
                    gobj.CapitalizationList = (List<capitalizationMaker>)Session["chklist"];
                }


            }
            catch (Exception ex)
            {

            }
            return View(gobj);
        }

        public PartialViewResult CaptalizationAssetDetails(int id)
        {
            capitalizationMaker objdetials = new capitalizationMaker();
            objdetials = ccr.getcapapprovaldetails(id);
            return PartialView(objdetials);
        }
        [HttpPost]
       // public PartialViewResult CaptalizationAssetDetails(string command,string inid)
        public JsonResult CaptalizationAssetDetails(capitalizationMaker obj)
        {
            string Remarks = string.IsNullOrEmpty(obj.Remarks) ? string.Empty : obj.Remarks;
            string Checker = string.Empty;
            Checker = cmnfunc.authorize("229");
            capitalizationMaker objdetials = new capitalizationMaker();
            if (Checker == "Success")
            {
                if (obj.commend == "APPROVED")
                {
                    ccr.FinalApprovalstage(Convert.ToInt32(obj.invoicegid), "APPROVED", Remarks);
                }
                if (obj.commend == "REJECT")
                {

                    ccr.FinalApprovalstage(Convert.ToInt32(obj.invoicegid), "REJECTED", Remarks);
                }
            }
            else
            {
                Checker = "Unauthorized User!";
            }
           // objdetials = ccr.getcapapprovaldetails(id);
            return Json(objdetials,JsonRequestBehavior.AllowGet);
        }

    }
}
