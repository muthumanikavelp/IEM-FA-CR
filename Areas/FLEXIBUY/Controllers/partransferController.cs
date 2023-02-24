using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using IEM.Areas.FLEXIBUY.Models;
using System.Text;
using IEM.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.WebControls;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class partransferController : Controller
    {
        //
        // GET: /FLEXIBUY/partransfer/
        int totalcount;
        ErrorLog objErrorLog = new ErrorLog();
        private IrepositoryAn objrep;
        public partransferController()
            : this(new CbfSumModel())
        {

        }
        public partransferController(IrepositoryAn objM)
        {
            objrep = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            Session["dttransferamount"] = null;
            Session["transtotalamt"] = null;
            Session["trnsinamnt"] = null;
            Session["trnsoutamt"] = null;
            Session["tranoutamt"] = null;
            try
            {
                Session["dttransferamount"] = null;
                Session["parheadergid"] = null;
                List<partransfer> obj = new List<partransfer>();
                obj = objrep.getpartransfersummary().ToList();
                ViewBag.process = "process";
                return View(obj);

            }

            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpPost]
        public ActionResult Index(string txtparefno, string command)
        {

            //CbfSumEntity obj;

            List<partransfer> obj = new List<partransfer>();
            obj = objrep.getpartransfersummary().ToList();
            string parefno = "";
            //Session["prsummary1"] = obj.lstprSum;
            if (command == "SEARCH")
            {
                if ((string.IsNullOrEmpty(txtparefno)) == false)
                {
                    ViewBag.process = "process";
                    ViewBag.txtparno = txtparefno;
                    // objrep.parserach(parefno);
                    //obj.lstparSum = obj.lstparSum.Where(x => txtprrefno == null || (x.parheaderrefno.ToUpper().Contains(txtprrefno.ToUpper()))).ToList();
                    //if (obj.lstparSum.Count == 0)
                    //{
                    //    ViewBag.Message = "No records found";
                    //}
                    obj = obj.Where(x => txtparefno == null ||
                    (x.parheaderrefno.ToUpper().Contains(txtparefno.ToUpper()))).ToList();

                    if (obj.Count == 0)
                    {
                        ViewBag.records = "No Records Found";
                    }

                }
            }
            ViewBag.process = "process";
            return View(obj);
            //List<SupClassificationModel> records = new List<SupClassificationModel>();
            //records = objModel.SelectSlabname().ToList();

            //if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
            //{
            //    ViewBag.filter = filter.ToString();

            //    records = records.Where(x => filter == null ||
            //        (x.Slabname.ToUpper().Contains(filter.ToUpper()))).ToList();
            //}
            //if (records.Count == 0)
            //{
            //    ViewBag.records = "No Records Found";
            //}
            //return View(records);
        }
        [HttpGet]
        public PartialViewResult pardetailstransfer(string id)
        {

            ViewBag.process = "Postdata";
            Session["parheadergid"] = id;
            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            obj = objrep.getpardetailtransfersummary(id).ToList();
            totalcount = obj.Count;
            return PartialView(obj);
        }
        public PartialViewResult Transferamount(string id)
        {
            pardetailtransfer obj = new pardetailtransfer();
            obj.pardetailsgid = Convert.ToInt32(id);
            return PartialView(obj);

            //List<pardetailtransfer> obj = new List<pardetailtransfer>();
            //obj = objrep.getpardetailamountsummary(id).ToList();            
            //return PartialView(obj);
        }
        //[HttpGet]
        //public PartialViewResult pardetailsapprover()
        //{ 
        //    return PartialView();
        //}

        public PartialViewResult pardetailsapprover(string viewfor)
        {

            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            string id = null;
            if (viewfor == "revert")
            {
                id = Session["parheadergid"].ToString();

                obj = objrep.getpardetailtransfersummary(id).ToList();
            }

            if (viewfor == "update")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["dttransferamount"];
                obj = objrep.getpardetailswithamount(dt, viewfor).ToList();
                Session["dttransferamount"] = null;
                Session["transtotalamt"] = null;
                Session["trnsinamnt"] = null;
                Session["trnsoutamt"] = null;
            }

            

            return PartialView("pardetailstransfer", obj);

        }

        public PartialViewResult transferamountsave(pardetailtransfer objtransfer)
        {
            //if (Convert.ToInt32(Session["trnsoutamt"]) == Convert.ToInt32(Session["tranoutamt"]))
            //{
            //    //Session["trnsoutamt"] = null;
            //    //Session["trnsinamnt"] = null;
            //    //Session["transtotalamt"] = null;
            //    Session["tranoutamt"] = null;

            //}
            int res = 0;
            string retvl = "";
            int totaltranfer = 0;
            int totalintransfer = 0;
            string viewfor = "save";
            DataTable dt = new DataTable();
            if (Session["dttransferamount"] != null)
            {
                dt = (DataTable)Session["dttransferamount"];
                if (dt.Rows.Count > 0)
                {
                    //if (!dt.Columns.Contains("Transfer IN Amount") && !dt.Columns.Contains("Transfer Out Amount"))
                    //{
                    //    dt.Columns.Add("Transfer IN Amount");
                    //    dt.Columns.Add("Transfer Out Amount");
                    //}
                    Session["trnsoutamt"] = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                       
                        if (dt.Rows[i]["pardetails_gid"].ToString() == objtransfer.pardetailsgid.ToString())
                        {
                            if (Convert.ToInt32(Session["trnsinamnt"]) == 0 || Convert.ToString(Session["trnsinamnt"]) == "")
                            {
                                dt.Rows[i]["TransferIn"] = objtransfer.pardetailstransferinamount;
                                if (dt.Rows[i]["TransferOut"] == "0" || Convert.ToString(dt.Rows[i]["TransferOut"]) == "")
                                {
                                    dt.Rows[i]["TransferOut"] = objtransfer.pardetailstransferoutamount;
                                }
                                totalintransfer = Convert.ToInt32(objtransfer.pardetailstransferoutamount);
                                totaltranfer = Convert.ToInt32(objtransfer.pardetailstransferinamount);
                            }
                            else
                            {
                                totalintransfer = Convert.ToInt32(Session["trnsoutamt"]);
                                totaltranfer = Convert.ToInt32(Session["trnsinamnt"]);
                                int totalinamt = 0;
                                totalinamt = Convert.ToInt32(objtransfer.pardetailstransferinamount);
                                totalinamt += totaltranfer;
                                totaltranfer = totalinamt;
                                if (Convert.ToInt32(Session["trnsoutamt"]) >= totaltranfer)
                                {

                                    dt.Rows[i]["TransferIn"] = objtransfer.pardetailstransferinamount;
                                    if (Convert.ToInt32(Session["trnsoutamt"]) != objtransfer.pardetailstransferoutamount)
                                    {
                                        dt.Rows[i]["TransferOut"] = objtransfer.pardetailstransferoutamount;
                                    }
                                    else
                                    {
                                        dt.Rows[i]["TransferOut"] = "0";
                                        objtransfer.pardetailstransferoutamount = Convert.ToInt32(dt.Rows[i]["TransferOut"]);
                                    }

                                }
                                else
                                {
                                    totaltranfer = Convert.ToInt32(Session["trnsinamnt"]);
                                    totalintransfer = Convert.ToInt32(Session["trnsoutamt"]);
                                    objtransfer.pardetailstransferinamount = 0;
                                    objtransfer.pardetailstransferoutamount = 0;
                                    dt.Rows[i]["TransferIn"] = objtransfer.pardetailstransferinamount;
                                    dt.Rows[i]["TransferOut"] = objtransfer.pardetailstransferoutamount;
                                    retvl = "Total Transfer In Amount Must Be Less Than Or Equal To TranferOut Amount";
                                }


                                Session["trnsoutamt"] = Convert.ToInt32(totalintransfer) + objtransfer.pardetailstransferoutamount;
                            }

                        }
                        string jj = Convert.ToString(Session["trnsoutamt"]);
                        if (Convert.ToString(dt.Rows[i]["TransferOut"]) != "")
                        {
                            if (Convert.ToString(Session["trnsoutamt"]) != "")
                            {
                                Session["trnsoutamt"] = Convert.ToInt32(dt.Rows[i]["TransferOut"])  + Convert.ToInt32(Session["trnsoutamt"]);
                            }
                            else
                            {
                                Session["trnsoutamt"] = Convert.ToInt32(dt.Rows[i]["TransferOut"]) ;
                            }
                        }
                    }

                    Session["trnsinamnt"] = Convert.ToInt32(totaltranfer);
                    //Session["trnsoutamt"] = Convert.ToInt32(totalintransfer);

                }
            }
            int totalamt = 0;

            if (objtransfer.pardetailstransferinamount != null && objtransfer.pardetailstransferinamount != 0)
            {
                if (Session["transtotalamt"] != null)
                {
                    totalamt = Convert.ToInt32(objtransfer.pardetailstransferinamount) + Convert.ToInt32(Session["transtotalamt"].ToString());
                }
                else
                {
                    totalamt = Convert.ToInt32(objtransfer.pardetailstransferinamount);
                    Session["transtotalamt"] = Convert.ToString(totalamt);
                }
                if (Convert.ToInt32(Session["trnsoutamt"]) < totalamt)
                {
                    retvl = "Total Transfer In Amount Must Be Less Than Or Equal To TranferOut Amount";
                }
            }
            //if (Convert.ToInt32(Session["trnsoutamt"]) == totaltranfer)
            //{
            //    Session["trnsoutamt"] = null;
            //    Session["trnsinamnt"] = null;
            //    Session["transtotalamt"] = null;
            //    //StringBuilder sb = new StringBuilder();
            //    //sb.Append("<script>");
            //    ////sb.Append("function alertbox() {");
            //    ////sb.Append("if (confirm('Are you sure?') == true) ");
            //    ////sb.Append("{");
            //    ////sb.Append("document.getElementById('" + hdnYesNo.ClientID + "').value = 'YES';");
            //    ////sb.Append("}");
            //    ////sb.Append("else");
            //    ////sb.Append("{");
            //    ////sb.Append("document.getElementById('" + hdnYesNo.ClientID + "').value = 'NO';");
            //    ////sb.Append("}");
            //    //sb.Append("sessionStorage.clear();");
            //    //sb.Append("</script>");


            //}
            //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
            //                new DataColumn("Transfer IN Amount", typeof(string)),
            //                new DataColumn("Transfer Out Amount",typeof(string)) });
            // dt.Rows.Add(objtransfer.pardetailsgid, objtransfer.pardetailstransferinamount, objtransfer.pardetailstransferoutamount);

            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            obj = objrep.getpardetailswithamount(dt, viewfor).ToList();

            //if (res == 1)
            //{
            //    return PartialView(res);
            //}

            if (retvl == "")
            {
                return PartialView("pardetailstransfer", obj);
            }
            else
            {
                return PartialView("pardetailstransfer", retvl);
            }
        }
        public JsonResult transferamountsave1(pardetailtransfer objtransfer)
        {
            string retvl = "";
            try
            {
                int totalinamount = 0;
                int totaloutamount = 0;
                int inputinamount = 0;
                int inputoutamount  = 0;
                if (objtransfer.pardetailstransferinamount != null)
                {
                    inputinamount = Convert.ToInt32(objtransfer.pardetailstransferinamount);
                }
                if (objtransfer.pardetailstransferoutamount != null)
                {
                    inputoutamount = Convert.ToInt32(objtransfer.pardetailstransferoutamount);
                }
                 DataTable dt = new DataTable();
                 if (Session["dttransferamount"] != null)
                 {
                     dt = (DataTable)Session["dttransferamount"];
                     if (dt.Rows.Count > 0)
                     {
                         for (int i = 0; i < dt.Rows.Count; i++)
                         {
                             if (inputinamount > 0)
                             {
                                 if (dt.Rows[i]["pardetails_gid"].ToString() != Convert.ToString(objtransfer.pardetailsgid))
                                 {
                                     totalinamount += Convert.ToInt32(dt.Rows[i]["TransferIn"].ToString());
                                 }
                             }
                             else
                             {
                                 totalinamount += Convert.ToInt32(dt.Rows[i]["TransferIn"].ToString());
                             }
                             totaloutamount += Convert.ToInt32(dt.Rows[i]["TransferOut"].ToString());
                         }
                     }
                     totalinamount += inputinamount;
                     totaloutamount += inputoutamount;

                     if (totaloutamount < totalinamount)
                     {
                       //  retvl = "Total Transfer In Amount Must Be Less Than Or Equal To TranferOut Amount";
                         retvl = "Err001";
                     }
                     else
                     {
                         foreach (DataRow row in dt.Rows)
                         {
                             if (row["pardetails_gid"].ToString() == Convert.ToString(objtransfer.pardetailsgid))
                             {
                                 if (inputinamount > 0)
                                 {
                                     int balamnt = Convert.ToInt32(row["balanced"].ToString());
                                     row.SetField("TransferIn", inputinamount);
                                     balamnt = balamnt + inputinamount;
                                     row.SetField("balanced", balamnt);
                                 }
                                 else
                                 {
                                     int utilamnt = Convert.ToInt32(row["Untilized"].ToString());
                                     int balamnt = Convert.ToInt32(row["balanced"].ToString());
                                     if (balamnt < inputoutamount)
                                     {
                                         //  retvl = "Tranfer Out Amount Should Not Be Greater Than Balanced Amount";
                                         retvl = "Err002";
                                     }
                                     else
                                     {
                                         row.SetField("TransferOut", inputoutamount);
                                         utilamnt = utilamnt + inputoutamount;
                                         row.SetField("Untilized", utilamnt);
                                         balamnt = balamnt - inputoutamount;
                                         row.SetField("balanced", balamnt);
                                     }
                                     
                                 }
                             }
                         }

                     }
                     Session["dttransferamount"] = dt;
                 }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            if (retvl == "")
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(retvl, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult loadpardetails() 
        {
            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            try
            {
                if (Session["dttransferamount"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dttransferamount"];
                    if (dt.Rows.Count > 0)
                    {
                        obj = objrep.getpardetailswithamount(dt, "save").ToList();
                    }
                }
                return PartialView("pardetailstransfer", obj); 
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("pardetailstransfer", "error"); 
            }
          
        }
        [HttpGet]
        public ActionResult ParTransferReport() 
        {
            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            try
            {
               // obj = objrep.getpardetailtransfersummary_report().ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        public JsonResult GetparDetails()
        {
            List<pardetailtransfer> obj = new List<pardetailtransfer>();
            try
            {
                obj = objrep.getpardetailtransfersummary_report().ToList();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult downloadsexcel()
        {
            try
            {
                string mt = null;
                List<pardetailtransfer> cList = new List<pardetailtransfer>();
                cList = objrep.getpardetailtransfersummary_report().ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("S.No.");
                dt.Columns.Add("PAR NO");
                dt.Columns.Add("Request For");
                dt.Columns.Add("Line Description");
                dt.Columns.Add("Line Amount");
                dt.Columns.Add("Utilized Amount");
                dt.Columns.Add("Transfer In");
                dt.Columns.Add("Transfer Out");
                dt.Columns.Add("Balance Amount");
                for (int i = 0; i < cList.Count; i++)
                {
                    dt.Rows.Add(
                        i + 1,
                        cList[i].parheader_refno.ToString(),
                        cList[i].pardetailsrequestfor.ToString(),
                        cList[i].pardetails_desc.ToString(),
                        cList[i].pardetailslineamount.ToString(),
                        cList[i].pardetailsutilizedamount.ToString(),
                        cList[i].pardetailstransferinamount.ToString(),
                        cList[i].pardetailstransferoutamount.ToString(),
                         cList[i].pardetailsbalencedamount.ToString()
                        );
                }
                //export to excel from gridview
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                if (gv.Rows.Count != 0)
                {
                    return new DownloadFileActionResult(gv, "DocumentGroup.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
