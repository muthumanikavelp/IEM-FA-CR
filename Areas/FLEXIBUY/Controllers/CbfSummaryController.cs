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
    public class CbfSummaryController : Controller
    {
        private IrepositoryAn objrep;
        public CbfSummaryController()
            : this(new CbfSumModel())
        {

        }
        public CbfSummaryController(IrepositoryAn objM)
        {
            objrep = objM;
        }
        [HttpGet]
        public ActionResult CbfSummaryIndex()
        {
            CbfSumEntity obj;
            obj = objrep.GetCbfSumry();
            obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
            Session["cbfsummary"] = obj;
            obj.result = objrep.GetGroupForUserCBF();
            return View(obj);
        }

        //[HttpGet]
        //public ActionResult CbfSummaryIndex(string docnum = null)
        //{
        //    CbfSumEntity obj;
        //    obj = objrep.GetCbfSumry();
        //    obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
        //    if (docnum != null)
        //    {
        //        if (!(string.IsNullOrEmpty(docnum.ToUpper())))
        //        {
        //            ViewBag.txtcbfno = docnum;
        //            obj.cbfSum = obj.cbfSum.Where(x => docnum.ToUpper() == null || (x.cbfNo.Contains(docnum.ToUpper()))).ToList();
        //            if (obj.cbfSum.Count == 0)
        //            {
        //                ViewBag.Message = "No records found";
        //            }
        //        }
        //    }

        //    Session["cbfsummary"] = obj;
        //    //obj.result = objrep.GetGroupForUser();
        //    return View(obj);
        //}

        [HttpPost]
        public ActionResult CbfSummaryIndex(string txtcbfno, string txtcbfdate, string command, string txtcbfmode, string txtstatus)
        {
            Session["cbfheaderlist"] = null;
            //CbfSumEntity obj;

            //obj = objrep.GetCbfSumry();
            //obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
            //if (txtstatus != null && txtstatus != "0")
            //{
            //    obj.amoun = objrep.getStatusName(Convert.ToInt32(txtstatus));
            //    obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname", txtstatus);
            //}
            //if (!(string.IsNullOrEmpty(txtcbfno.ToUpper())))
            //{
            //    ViewBag.txtcbfno = txtcbfno;
            //    obj.cbfSum = obj.cbfSum.Where(x => txtcbfno.ToUpper() == null || (x.cbfNo.Contains(txtcbfno.ToUpper()))).ToList();
            //    if (obj.cbfSum.Count == 0)
            //    {
            //        ViewBag.Message = "No records found";
            //    }
            //}
            //if (!(string.IsNullOrEmpty(txtcbfdate)))
            //{
            //    ViewBag.txtcbfdate = txtcbfdate;
            //    obj.cbfSum = obj.cbfSum.Where(x => txtcbfdate.ToUpper() == null || (x.cbfDate.Contains(txtcbfdate.ToUpper()))).ToList();
            //    if (obj.cbfSum.Count == 0)
            //    {
            //        ViewBag.Message = "No records found";
            //    }
            //}
            //if (!(string.IsNullOrEmpty(txtcbfmode)) && txtcbfmode!= "0")
            //{
            //    ViewBag.txtcbfmode = txtcbfmode;
            //    obj.cbfSum = obj.cbfSum.Where(x => txtcbfmode.ToUpper() == null || (x.cbfMode.Contains(txtcbfmode.ToUpper()))).ToList();
            //    if (obj.cbfSum.Count == 0)
            //    {
            //        ViewBag.Message = "No records found";
            //    }
            //    ViewBag.cbfmode = txtcbfmode;
            //}
            //if ((string.IsNullOrEmpty(txtcbfmode)) == false)
            //{
            //    ViewBag.statusname = obj.amoun;

            //    obj.cbfSum = obj.cbfSum.Where(x => obj.amoun == null ||
            //        (x.cbfStatus.Contains(obj.amoun))).ToList();
            //}
            //if ((string.IsNullOrEmpty(txtcbfno)) == true && (string.IsNullOrEmpty(txtcbfdate)) == true && (string.IsNullOrEmpty(txtcbfmode)) == true && (string.IsNullOrEmpty(txtstatus)) == true)
            //{
            //    ViewBag.Error = "Please fill search Criteria";
            //}
            //if (command != "SEARCH")
            //{
            //    obj = (CbfSumEntity)Session["cbfsummary"];
            //}
            //return View(obj);
            CbfSumEntity obj;

            obj = objrep.GetCbfSumry();
            obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
            if (txtstatus != null && txtstatus != "0")
            {
                obj.amoun = objrep.getStatusName(Convert.ToInt32(txtstatus));
                obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname", txtstatus);
            }
            if ((string.IsNullOrEmpty(obj.amoun)) == false)
            {
                ViewBag.statusname = obj.amoun;
                obj.cbfSum = obj.cbfSum.Where(x => obj.amoun == null ||
                    (x.cbfStatus.Contains(obj.amoun))).ToList();
                Session["cbfheaderlist"] = obj;
            }
            if (!(string.IsNullOrEmpty(txtcbfno.ToUpper())))
            {
                ViewBag.txtcbfno = txtcbfno;
                obj.cbfSum = obj.cbfSum.Where(x => txtcbfno.ToUpper() == null || (x.cbfNo.Contains(txtcbfno.ToUpper()))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (!(string.IsNullOrEmpty(txtcbfdate)))
            {
                ViewBag.txtcbfdate = txtcbfdate;
                obj.cbfSum = obj.cbfSum.Where(x => txtcbfdate.ToUpper() == null || (x.cbfDate.Contains(txtcbfdate.ToUpper()))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (!(string.IsNullOrEmpty(txtcbfmode)) && txtcbfmode != "0")
            {
                ViewBag.txtcbfmode = txtcbfmode;
                obj.cbfSum = obj.cbfSum.Where(x => txtcbfmode.ToUpper() == null || (x.cbfMode.Contains(txtcbfmode.ToUpper()))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                ViewBag.cbfmode = txtcbfmode;
            }
            if ((string.IsNullOrEmpty(txtcbfmode)) == false)
            {
                ViewBag.statusname = obj.amoun;

                obj.cbfSum = obj.cbfSum.Where(x => obj.amoun == null ||
                    (x.cbfStatus.Contains(obj.amoun))).ToList();
            }
            if ((string.IsNullOrEmpty(txtcbfno)) == true && (string.IsNullOrEmpty(txtcbfdate)) == true && (string.IsNullOrEmpty(txtcbfmode)) == true && (string.IsNullOrEmpty(txtstatus)) == true)
            {
                ViewBag.Error = "Please fill search Criteria";
            }
            if ((string.IsNullOrEmpty(obj.statusname)) == false)
            {
                ViewBag.statusname = obj.statusname;
                obj.ListParHeader = obj.ListParHeader.Where(x => obj.statusname == null ||
                    (x.status.Contains(obj.statusname))).ToList();
                Session["cbfsummarystatus"] = obj;
            }
            if (command != "SEARCH")
            {
                obj = (CbfSumEntity)Session["cbfsummary"];
            }
            obj.result = objrep.GetGroupForUser();
            Session["cbfexceldownload"] = obj.cbfSum;
            return View(obj);
        }
        [HttpPost]
        public JsonResult CbfNo(CbfSummarymodel prdegid1)
        {
            string refno = objrep.GeneartePrRefno(prdegid1.cbfGid, prdegid1.cbfNo);
            return Json(refno, JsonRequestBehavior.AllowGet);
        }


        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<CbfSummarymodel> cList;
            CbfSumEntity obj = new CbfSumEntity();
            obj = objrep.GetCbfSumry();

            if (Session["cbfexceldownload"] == null)
            {
                cList = obj.cbfSum;
            }
            else
            {
                cList = (List<CbfSummarymodel>)Session["cbfexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("CBF Mode");
            dt.Columns.Add("CBF No ");
            dt.Columns.Add("CBF Date ");
            dt.Columns.Add("CBF End Date ");
            dt.Columns.Add("Department");
            dt.Columns.Add("Fincon Budgeted");
            dt.Columns.Add("CBF Amount ");
            dt.Columns.Add("CBF Project Owner");
            dt.Columns.Add("Deviation Amount ");
            dt.Columns.Add("Status");
            dt.Columns.Add("Description ");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].cbfMode.ToString()
                , cList[i].cbfNo.ToString()
                , cList[i].cbfDate.ToString()
                , cList[i].cbfEnddate.ToString()
                , cList[i].cbfRequestfor.ToString()
                , cList[i].fincon_Bugt.ToString()
                , cList[i].cbfProjectOwner.ToString()
                , cList[i].cbfAmo.ToString()
                , cList[i].cbfDevi_Amo.ToString()
                , cList[i].cbfStatus.ToString()
                , cList[i].cbfDescription.ToString()
                );                     
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();            
            if (gv.Rows.Count != 0)
            {                
                return new DownloadFileActionResult(gv, "CBF Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }

}
