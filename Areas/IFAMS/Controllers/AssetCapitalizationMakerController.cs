using IEM.Areas.IFAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetCapitalizationMakerController : Controller
    {
        private Ifams_Repository cmr;

        public AssetCapitalizationMakerController()
            : this(new Ifams_Model())
        {

        }

        public AssetCapitalizationMakerController(Ifams_Repository cmrr)
        {
            cmr = cmrr;
        }
        public ActionResult CapitalizationMaker()
        {
            capitalizationMaker objcp = new capitalizationMaker();
            try
            {

                objcp.GetApproveList = cmr.getapprovedetails();
                //  objcp.GetApproveList = objcp.Getapplist;
                objcp.CapitalizationList = cmr.GetECFdetails();
              //  objcp.CapitalizationListGRN = cmr.GetECFdetailsGRN();
              //  objcp.CapitalizationListbr = cmr.GetECFdetailsBranch();
                Session["cmaker"] = objcp.CapitalizationList;
                Session["Applist"] = objcp.GetApproveList;
                //Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
            }
            catch (Exception ex)
            { }
            return View(objcp);
        }

        [HttpPost]
        public ActionResult CapitalizationMaker(capitalizationMaker objs, string Refersh)
        {
            List<capitalizationMaker> objl = new List<capitalizationMaker>();
            // objl = (List<capitalizationMaker>)TempData["cmaker"];
            capitalizationMaker objm = new capitalizationMaker();
            objm.CapitalizationList = (List<capitalizationMaker>)Session["cmaker"];
            try
            {
                //  objm.CapitalizationList = objl.Where(a => a.EcfNo.ToUpper().Contains(objs.EcfNo)).ToList();
                if (Refersh != "Refersh")
                {
                    if ((string.IsNullOrEmpty(objs.EcfNo)) == false)
                    {
                        //ViewBag.txtSuppliercode = txtSuppliercode;
                        objm.CapitalizationList = objm.CapitalizationList.Where(x => objs.EcfNo == null ||
                           (x.EcfNo.ToUpper().Contains(objs.EcfNo.ToUpper()))).ToList();
                        // Session["cmaker"] = objm.CapitalizationList;
                    }
                    if ((string.IsNullOrEmpty(objs.invoiceno)) == false)
                    {
                        objm.CapitalizationList = objm.CapitalizationList.Where(x => objs.invoiceno == null ||
                          (x.invoiceno.ToUpper().Contains(objs.invoiceno.ToUpper()))).ToList();
                        //Session["cmaker"] = objm.CapitalizationList;
                    }
                    //if ((string.IsNullOrEmpty(objs.EcfAmount)) == false)
                    //{
                    //    objm.CapitalizationList = objm.CapitalizationList.Where(x => objs.EcfAmount == null ||
                    //      (x.EcfAmount.ToUpper().Contains(objs.EcfAmount.ToUpper()))).ToList();
                    //  //  Session["cmaker"] = objm.CapitalizationList;
                    //}
                    if ((string.IsNullOrEmpty(objs.Vendor)) == false)
                    {
                        objm.CapitalizationList = objm.CapitalizationList.Where(x => objs.Vendor == null ||
                           (x.Vendor.ToUpper().Contains(objs.Vendor.ToUpper()))).ToList();
                        // Session["cmaker"] = objm.CapitalizationList;
                    }

                }
                else
                {
                    objm.CapitalizationList = (List<capitalizationMaker>)Session["cmaker"];
                }
                Session["capexceldownload"] = objm.CapitalizationList;
                objm.GetApproveList = cmr.getapprovedetails();
            }
            catch (Exception ex)
            {

            }
            return View(objm);
        }

        [HttpPost]
        public PartialViewResult SearchApplist(capitalizationMaker objalist)
        {
            capitalizationMaker objcpapp = new capitalizationMaker();
            objcpapp.GetApproveList = (List<capitalizationMaker>)Session["Applist"];
            try
            {

                //if (Refersh != "Refersh")
                //{
                    if ((string.IsNullOrEmpty(objalist.EcfNo)) == false)
                    {

                        objcpapp.GetApproveList = objcpapp.GetApproveList.Where(x => objalist.EcfNo == null ||
                           (x.EcfNo.ToUpper().Contains(objalist.EcfNo.ToUpper()))).ToList();

                    }
                    if ((string.IsNullOrEmpty(objalist.invoiceno)) == false)
                    {
                        objcpapp.GetApproveList = objcpapp.GetApproveList.Where(x => objalist.invoiceno == null ||
                          (x.invoiceno.ToUpper().Contains(objalist.invoiceno.ToUpper()))).ToList();

                    }
                    if ((string.IsNullOrEmpty(objalist.Vendor)) == false)
                    {
                        objcpapp.GetApproveList = objcpapp.GetApproveList.Where(x => objalist.Vendor == null ||
                           (x.Vendor.ToUpper().Contains(objalist.Vendor.ToUpper()))).ToList();

                    }
                    if (objalist.ddlstatus != "SELECT")
                    {
                        objcpapp.GetApproveList = objcpapp.GetApproveList.Where(x => objalist.ddlstatus == null ||
                           (x.Status.ToUpper().Contains(objalist.ddlstatus.ToUpper()))).ToList();

                    }
               // }
                    if ((string.IsNullOrEmpty(objalist.EcfNo)) && (string.IsNullOrEmpty(objalist.invoiceno)) && (string.IsNullOrEmpty(objalist.Vendor)) && objalist.ddlstatus == "SELECT")
                    {
                        objcpapp.GetApproveList = (List<capitalizationMaker>)Session["Applist"];
                    }
                   
                

                

               
            }
            catch (Exception ex)
            {

            }
            return PartialView("ApprovalList", objcpapp);
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            capitalizationMaker objm = new capitalizationMaker();
            objm.CapitalizationList = cmr.GetECFdetails();
            List<capitalizationMaker> cList;
            if (Session["capexceldownload"] == null)
            {
                cList = objm.CapitalizationList;
            }
            else
            {
                cList = (List<capitalizationMaker>)Session["capexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("ECF No");
            dt.Columns.Add("ECF Date");
            dt.Columns.Add("ECF Amount");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Invoice Amount");
            dt.Columns.Add("Vendor Name");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].EcfNo.ToString()
                    , cList[i].Ecfdate.ToString()
                    , cList[i].EcfAmount.ToString()
                    , cList[i].invoiceno.ToString()
                    , cList[i].invoiceamount.ToString()
                    , cList[i].Vendor.ToString()                   
                    );
            }            

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Capitalization-Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }


    }
}
