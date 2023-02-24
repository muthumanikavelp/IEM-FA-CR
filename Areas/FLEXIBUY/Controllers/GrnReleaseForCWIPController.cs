using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class GrnReleaseForCWIPController : Controller
    {
        //
        // GET: /FLEXIBUY/GrnReleaseForCWIP/

        private Irepositorypr objModel;
        public GrnReleaseForCWIPController()
            : this(new fb_DataModelpr())
        {

        }
        public GrnReleaseForCWIPController(Irepositorypr objM)
        {

            objModel = objM;
        }
        [HttpGet]
        public ActionResult CwipSummaryForGrn(string viewfor)
        {
            GRNInward objlist = new GRNInward();
            objlist.objInwardList = objModel.GetgrnSummaryForCwip();
            Session["branch_table"] = null;
            Session["grnTable"] = null;
            // objlist.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            //objlist.objWoSummary = objModel.GetWoSummary();
            //objlist.objInwardList = objModel.GetgrnSummary();
            if (viewfor == "refresh")
            {
                objlist.objInwardList = objModel.GetgrnSummaryForCwip();
                Session["grnheaderlist"] = null;
                // objlist.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
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
        public ActionResult CwipSummaryForGrn(string grndate, string command, string grnrefno, string vendorname)
        {
            GRNInward objheader = new GRNInward();
            Session["grnheaderlist"] = null;
            objheader.objInwardList = objModel.GetgrnSummaryForCwip();
            //  objheader.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            if (command == "search")
            {
                //objheader.statusgid = ddlStatus;
                //Session["statusgid"] = ddlStatus;
                //if (ddlStatus != null && ddlStatus != 0)
                //{
                //    //objheader.statusname = ddlStatus.ToString();
                //    objheader.statusname = objModel.getStatusName(ddlStatus);
                //    objheader.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname", ddlStatus);
                //}
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
        [HttpGet]
        public ActionResult GrnRelease(int grnheadGid)
        {
            try
            {

                //Session["viewfor"] = "view";
                GRNInward obj = new GRNInward();
                obj.objInwardList = objModel.GetGRNDetails(grnheadGid);
                Session["grninward"] = obj.objInwardList;
                obj.poRefNo = obj.objInwardList[0].poRefNo;
                obj.grnRefNo = obj.objInwardList[0].grnRefNo;
                obj.grnDcNo = obj.objInwardList[0].grnDcNo;
                obj.grnRemarks = obj.objInwardList[0].grnRemarks;
                obj.grnInvoiceNo = obj.objInwardList[0].grnInvoiceNo;
                obj.grnheadgid = grnheadGid;
                obj.grndetgid = obj.objInwardList[0].grndetgid;
                obj.grnReleaseGid = obj.objInwardList[0].grnReleaseGid;
                obj.raisedBy = objModel.GetLoginUser();
                obj.grnDate = DateTime.Now.ToShortDateString();
                obj.vendorName = obj.objInwardList[0].vendorName;
                return View("GrnRelease", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult GrnReleaseTempSave()
        {
            GRNInward obj = new GRNInward();
            if (Session["grndetTemplist"] != null)
                obj.objInwardList = (List<GRNInward>)Session["grndetTemplist"];
            return PartialView("GrnDetails", obj);

        }

        [HttpPost]
        public PartialViewResult GrnReleaseTemp(GRNInward objInward)
        {
            DataTable dt = new DataTable();
            try
            {

                if (Session["grnTable"] != null)
                    dt = (DataTable)Session["grnTable"];
                //else if (Session["grndetTable"] != null)
                //    dt = (DataTable)Session["grndetTable"];
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("assetSlno"))
                    {
                        dt.Columns.Add("assetSlno");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["grninwrddet_gid"].ToString() == Convert.ToString(objInward.grndetgid))
                        {
                            dt.Rows[i]["assetSlno"] = objInward.assetSlno;
                        }
                    }
                }
                Session["grnTable"] = dt;
                objInward.objInwardList = objModel.GetGrnDetailsTemp(dt);
                Session["grndetTemplist"] = objInward.objInwardList;
                return PartialView("GrnDetails", objInward);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BranchDetailsForCwip(string listfor)
        {
            List<shipment> objbanklist = new List<shipment>();
            if (listfor == "search")
            {
                if (Session["objbranch"] != null)
                    objbanklist = (List<shipment>)Session["objbranch"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["branchCode"] != null)
                    TempData["code"] = TempData["branchCode"];
                if (TempData["branchName"] != null)
                    TempData["name"] = TempData["branchName"];
            }
            else
            {
                Session["objowner"] = "";
                objbanklist = objModel.getbranchdetailsForCwip();
            }
            return PartialView("BranchDetailsForCwip", objbanklist);
        }

        [HttpPost]
        public JsonResult searchbranch(shipment objbranchsearch)
        {
            List<shipment> objowner = new List<shipment>();
            objowner = objModel.getbranchdetailsForCwip();
            if (objbranchsearch != null)
            {
                if ((string.IsNullOrEmpty(objbranchsearch.branchCode)) == false)
                {
                    TempData["branchCode"] = objbranchsearch.branchCode;
                    objowner = objowner.Where(x => objbranchsearch.branchCode == null ||
                        (x.branchCode.ToUpper().Contains(objbranchsearch.branchCode.ToUpper()))).ToList();
                    Session["objbranch"] = objowner;
                }
                if ((string.IsNullOrEmpty(objbranchsearch.branchName)) == false)
                {
                    TempData["branchName"] = objbranchsearch.branchName;
                    objowner = objowner.Where(x => objbranchsearch.branchName == null ||
                        (x.location.ToUpper().Contains(objbranchsearch.branchName.ToUpper()))).ToList();
                    Session["objbranch"] = objowner;
                }
            }
            return Json(objowner, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult BranchUpdate(shipment objshipment)
        {
            try
            {
                DataTable objtable = new DataTable();
                string[] words = objshipment.grndetGid.Split(',');
                string[] qty = objshipment.releasedqty.ToString().Split(',');
                string[] shipmentgid = objshipment.poshipmentgid.ToString().Split(',');
                string[] rowval=new string [] {};
                if (Session["branch_table"] != null)
                    objtable = (DataTable)Session["branch_table"];
                if (objtable.Rows.Count > 0)
                    rowval = objtable.AsEnumerable().Select(s=>s.Field<string>("grninwddetgid")).ToArray<string>();

                for(int i=0;i<words.Length;i++)
                {
                    if (rowval.Contains(words[i].ToString()) == false || rowval.Count() ==0)
                    {
                        objshipment.releasedqty = qty[i];
                        objshipment.poshipmentgid = shipmentgid[i];
                        DataTable dt = objModel.GetBranchDetailsForGRN(words[i], objshipment);
                        objtable.Merge(dt);
                    }
                    //if (objtable.Rows.Count > 0)
                    //{
                    //        DataRow[] objrow = objtable.Select("grninwddetgid='" + words[i] + "'");
                    //        for (int j = 0; j < objrow.Length; j++)
                    //        {
                                  
                    //         }
                    //        }
                    //else
                    //{
                    //    objshipment.releasedqty = qty[i];
                    //    objshipment.poshipmentgid = shipmentgid[i];
                    //    DataTable dt = objModel.GetBranchDetailsForGRN(words[i], objshipment);
                    //    objtable.Merge(dt);
                    //}
                   // DataTable dt = objModel.GetBranchDetailsForGRN(detgid, objshipment);
                 
                }
                Session["branch_table"] = objtable;
                objshipment.shiplist = objModel.GetBranchListForGRN(objtable);
                if (objshipment.shiplist.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                return PartialView(objshipment);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult DeleteBranch()
        {
            try
            {
                shipment objshipment = new shipment();
                DataTable dt = new DataTable();
                if(Session["branch_table"]!=null)
                {
                    dt = (DataTable)Session["branch_table"];
                }
                objshipment.shiplist = objModel.GetBranchListForGRN(dt);
                return PartialView("BranchUpdate", objshipment);
            }
            catch(Exception ex)
            {
                 throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult DeleteBranch(shipment objdelete)
        {
            try
            {
                DataTable dt = new DataTable();
                if(Session["branch_table"]!=null)
                {
                    dt = (DataTable)Session["branch_table"];
                    if (dt.Rows.Count > 0)
                    {
                        //if (!dt.Columns.Contains("srno"))
                        //    dt.Columns.Add("srno");

                        //for (int i = 0; i < dt.Rows.Count; i++)
                        //{

                        //    dt.Rows[i]["srno"] = i + 1;
                        //}
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["grninwddetgid"].ToString() == objdelete.grndetGid.ToString())
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                    Session["branch_table"] = dt;
                    objdelete.shiplist = objModel.GetBranchListForGRN(dt);
                    if(objdelete.shiplist.Count==0)
                    {
                        ViewBag.records = "No Records Found";
                    }
                }
                return PartialView("BranchUpdate",objdelete);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult SaveCwipReleaseDetails(GRNInward objInward)
        {
            DataTable objtable=new DataTable();
            DataTable dt=new DataTable();
            string result = "";
            try 
            {
                if (Session["branch_table"] != null)
                {
                    objtable = (DataTable)Session["branch_table"];
                    if (Session["grnTable"] != null)
                        dt = (DataTable)Session["grnTable"];
                    if (!dt.Columns.Contains("assetSlno"))
                        dt.Columns.Add("assetSlno");
                     result = objModel.InsertCwipRelease(objtable, objInward, dt);
                }
                else
                {
                    result = "branch";
                }
                if (result == "Success" || result == "branch")
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                   return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
