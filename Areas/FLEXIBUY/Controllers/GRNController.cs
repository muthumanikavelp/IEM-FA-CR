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
    public class GRNController : Controller
    {
        //
        // GET: /FLEXIBUY/GRN/


        private IRepositoryKIR objrep;


        public GRNController()
            : this(new prsummodel())

        { }
        public GRNController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        [HttpGet]
        public ActionResult grnconfirmationsummary()
        {
            //List<grnconfirmation> grnconfirm = new List<grnconfirmation>();
            grnconfirmation objconfirm = new grnconfirmation();
            objconfirm.grnconfirmations = objrep.getgrnconfirmation();
            //grnconfirm = objrep.getgrnconfirmation();
            if(objconfirm.grnconfirmations.Count==0)
            {
                ViewBag.records = "No Records Found ";
            }
            return View(objconfirm);
        }
        [HttpPost]
        public ActionResult grnconfirmationsummary(string grnrefno,string command)
        {
            //List<grnconfirmation> obj = new List<grnconfirmation>();
            grnconfirmation objconfirm = new grnconfirmation();
            objconfirm.grnconfirmations = objrep.getgrnconfirmation();
            if (command == "search")
            {
                if ((string.IsNullOrEmpty(grnrefno)) == false)
                {
                    ViewBag.grnrefno = grnrefno;
                    objconfirm.grnconfirmations = objconfirm.grnconfirmations.Where(x => grnrefno.ToUpper() == null || (x.grnrefno.Contains(grnrefno.ToUpper()))).ToList();
                    //objconfirm.grnconfirmations = objrep.chechgrnprefno(grnrefno);

                    if (objconfirm.grnconfirmations.Count == 0)
                    {
                        ViewBag.records = "No records found";
                    }
                }
            }
            Session["grninexceldownload"] = objconfirm.grnconfirmations;
            return View("grnconfirmationsummary", objconfirm);

        }

        public ActionResult grnconfirmation(grnconfirmation gr, int grnheadgid)
        {
            try
            {
                grnconfirmationdetails objgrn = new grnconfirmationdetails();
                objgrn.grnconfirmationdetail = objrep.getgrnconfirmationdetails(gr,grnheadgid);
                objgrn.grnheadGid = grnheadgid;
                objgrn.grnrefno = objgrn.grnconfirmationdetail[0].grnrefno.ToString();
                objgrn.grndate = objgrn.grnconfirmationdetail[0].grndate.ToString();
                objgrn.vendorname = objgrn.grnconfirmationdetail[0].vendorname.ToString();
                objgrn.poworefno = objgrn.grnconfirmationdetail[0].poworefno.ToString();
                objgrn.dcno = objgrn.grnconfirmationdetail[0].dcno.ToString();
                objgrn.invoiceno = objgrn.grnconfirmationdetail[0].invoiceno.ToString();
                objgrn.GRNdes = objgrn.grnconfirmationdetail[0].GRNdes.ToString();
                return View(objgrn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string Grninwrddetailsave(grnconfirmationdetails objgrndet)
        {
            string result="";
            DataTable dt = new DataTable();
            grnconfirmationdetails objgrndet1 = new grnconfirmationdetails();
           
            if (Session["grndetails"] != null || Session["grndetails"] != "")
            {
                dt = (DataTable)Session["grndetails"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (!dt.Columns.Contains("manuname") && !dt.Columns.Contains("assetslno") && !dt.Columns.Contains("putdate"))
                        //{
                        //    dt.Columns.Add("manuname");
                        //    dt.Columns.Add("assetslno");
                        //    dt.Columns.Add("putdate");

                        //}
                        if (!dt.Columns.Contains("IsSerial") && !dt.Columns.Contains("branch_flag") && !dt.Columns.Contains("GRNType"))
                        {
                            dt.Columns.Add("IsSerial");
                            dt.Columns.Add("branch_flag");
                            dt.Columns.Add("GRNType");
                        }
                        if (dt.Rows[i]["grninwrddet_gid"].ToString() == objgrndet.grndetGid.ToString())
                        {
                            dt.Rows[i]["grninwrddet_mft_name"] = string.IsNullOrEmpty(objgrndet.manfName) ? "" : objgrndet.manfName.ToString();
                            dt.Rows[i]["grninwrddet_assetsrlno"] = string.IsNullOrEmpty(objgrndet.assetSlno) ? "" : objgrndet.assetSlno.ToString();
                            dt.Rows[i]["grninwrddet_puttousedatetime"] = string.IsNullOrEmpty(objgrndet.putToUseDate) ? "" : objgrndet.putToUseDate.ToString();
                        }
                    }
                }

               
                result = objrep.insertgrncfmdetails(objgrndet, dt);
 
            }
            return result;
        }

        [HttpPost]

        public PartialViewResult Grnsavedetails(grnconfirmationdetails objgrndet)
        {

            DataTable dt = new DataTable();
            grnconfirmationdetails objgrndet1 = new grnconfirmationdetails();
            if (Session["grndetails"] != null || Session["grndetails"] != "")
            {
                dt = (DataTable)Session["grndetails"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (!dt.Columns.Contains("manuname") && !dt.Columns.Contains("assetslno") && !dt.Columns.Contains("putdate"))
                        //{
                        //    dt.Columns.Add("manuname");
                        //    dt.Columns.Add("assetslno");
                        //    dt.Columns.Add("putdate");

                        //}
                        if (!dt.Columns.Contains("IsSerial") && !dt.Columns.Contains("branch_flag") && !dt.Columns.Contains("GRNType"))
                        {
                            dt.Columns.Add("IsSerial");
                            dt.Columns.Add("branch_flag");
                            dt.Columns.Add("GRNType");
                        }
                        if (dt.Rows[i]["grninwrddet_gid"].ToString() == objgrndet.grndetGid.ToString())
                        {
                            dt.Rows[i]["grninwrddet_mft_name"] = string.IsNullOrEmpty(objgrndet.manfName) ? "" : objgrndet.manfName.ToString();
                            dt.Rows[i]["grninwrddet_assetsrlno"] = string.IsNullOrEmpty(objgrndet.assetSlno) ? "" : objgrndet.assetSlno.ToString();
                            dt.Rows[i]["grninwrddet_puttousedatetime"] = string.IsNullOrEmpty(objgrndet.putToUseDate) ? "" : objgrndet.putToUseDate.ToString();
                        }
                    }
                }

                string result = objrep.insertgrncfmdetails(objgrndet, dt);
                if (result == "Success")
                {
                    objgrndet1.grnconfirmationdetail = objrep.rebindconfirmdetails(dt);

                }


            }
            return PartialView("grndetails", objgrndet1);

        }
        [HttpPost]
        public JsonResult SaveGrnConfirm(grnconfirmation objgrn)
        {
            try
            {

                string approve = objrep.grnapprove(objgrn);
                return Json(approve, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<grnconfirmation> cList;
            grnconfirmation obj = new grnconfirmation();
            obj.grnconfirmations = objrep.getgrnconfirmation();

            if (Session["grninexceldownload"] == null)
            {
                cList = obj.grnconfirmations;
            }
            else
            {
                cList = (List<grnconfirmation>)Session["grninexceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("GRN Date");
            dt.Columns.Add("GRN No");
            dt.Columns.Add("PO No");
            dt.Columns.Add("PO Version");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Received Qty");           

            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].grndate.ToString()
                , cList[i].grnrefno.ToString()
                , cList[i].poworefno.ToString()
                , cList[i].poVersion.ToString()
                , cList[i].vendorname.ToString()
                , cList[i].grnQuantity.ToString()
                
                );


            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult(gv, "GRN Confirmation Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}

