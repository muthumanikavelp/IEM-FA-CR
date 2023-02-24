using System;
using IEM.Common;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using IEM.Areas.IFAMS.Models;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class SectorController : Controller
    {
        //
        // GET: /IFAMS/Sector/

        private sectorrep objrep;
        private CmnFunctions objcmnr = new CmnFunctions();
        ErrorLog objerrlog = new ErrorLog();
        DataTable dt = new DataTable();

        public SectorController()
            : this(new sectormodel())
        {

        }

        public SectorController(sectorrep objRModel)
        {
            objrep = objRModel;
        }


        public ActionResult Index()
        {
            sectorreport secrpt = new sectorreport();
            List<sectorreport> sectorreports = new List<sectorreport>();
            try
            {
                secrpt.secrep = objrep.Getsectorreport().ToList();
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(secrpt);
        }

        public ActionResult sectorexl()
        {
            try
            {

                List<sectorreport> secrep = new List<sectorreport>();
                secrep = objrep.Getsectorexcelreport().ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("S.No");
                dt.Columns.Add("ECF_NO");
                dt.Columns.Add("EMP_CODE");
                dt.Columns.Add("EMP_NAME");
                dt.Columns.Add("VEN_NAME");
                dt.Columns.Add("GROSS_AMT");
                dt.Columns.Add("TDS_AMT");
                dt.Columns.Add("NET_AMT");
                dt.Columns.Add("CHQ_NO");
                dt.Columns.Add("PV_NO");
                dt.Columns.Add("LOC");
                dt.Columns.Add("AWB_NO");
                dt.Columns.Add("SEND_TO");

                for (int i = 0; i < secrep.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                        secrep[i].ecfno.ToString(),
                        secrep[i].employee_code.ToString(),
                        secrep[i].employee_name.ToString(),
                        secrep[i].supplierheader_name.ToString(),
                        secrep[i].grossamount.ToString(),
                        secrep[i].tdsamount.ToString(),
                        secrep[i].netamount.ToString(),
                        secrep[i].chqno.ToString(),
                        secrep[i].payrunvoucherpvno.ToString(),
                        secrep[i].location.ToString(),
                        secrep[i].awbno.ToString(),
                        secrep[i].sendto.ToString());
                }

                DataTable _downloadingData = Session["sectorreport"] as DataTable;
                string attachment = "attachment; filename=FAReport.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";

                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int k;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (k = 0; k < dt.Columns.Count; k++)
                        {
                            Response.Write(tab + dr[k].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                }


            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }

            return RedirectToAction("index");

        }

        public ActionResult sectortxt()
        {
            try
            {

                List<sectorreport> secrep = new List<sectorreport>();
                secrep = objrep.Getsectorreport().ToList();

                string txt = string.Empty;
                string totalchqcount = "";
                string amount = "";
                decimal totalchqamount = 0;


                dt = Session["sectorreport"] as DataTable;

                txt = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString() + "  Sectorwise Report ";
                txt += "\r\n";

                foreach (DataColumn column in dt.Columns)
                {

                    txt += column.ColumnName + "\t\t";
                }

                txt += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {

                        txt += row[column.ColumnName].ToString() + "\t\t";

                    }

                    txt += "\r\n";

                }

                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {

                    amount = dt.Rows[i]["payrunvoucher_amount"].ToString();
                    totalchqamount += Convert.ToDecimal(amount);


                }

                totalchqcount = dt.Rows.Count.ToString();


                txt += "Total Cheques :" + totalchqcount + "\t\t";
                txt += "Total Amount :" + totalchqamount;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.txt");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(txt);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            Session["sectorreport"] = null;
            return RedirectToAction("index");

        }

        public ActionResult sectorwisetxt()
        {
            try
            {

                List<sectorreport> secrep = new List<sectorreport>();
                secrep = objrep.Getsectorwisereport().ToList();
                decimal payamount = 0;
                string txt = string.Empty;
                string columnname = "";
                string branchcode = "";
                dt = Session["sectorwisereport"] as DataTable;
                int count = 0;
                int totalcount = 0;
                txt = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString() + " Sectorwise Report ";
                txt += "\r\n";
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {

                        txt += row[column.ColumnName].ToString() + "\t\t";

                        columnname = column.ColumnName.ToString();

                    }
                    txt += "\r\n";

                    if (row[5].ToString() != "Chq No")
                    {

                        totalcount += count + 1;
                        payamount += Convert.ToDecimal(row[3].ToString());
                    }

                    else
                    {
                        if (totalcount != 0)
                        {
                            txt += "\r\n";
                            txt += "Total Cheques : -" + totalcount + "\t\t";
                            txt += "Total Amount : -" + payamount + "\t\t";
                            txt += "\r\n";
                            txt += System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString() + " Sectorwise Report ";
                            totalcount = 0;
                            count = 0;
                            payamount = 0;
                        }



                    }



                }

                txt += "Total Cheques: -" + totalcount + "\t\t";
                txt += "Total Amount : -" + payamount + "\t\t";
                txt += "\r\n";
                totalcount = 0;
                count = 0;
                payamount = 0;

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.txt");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(txt);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            Session["sectorwisereport"] = null;
            return RedirectToAction("index");
        }
    }
}
