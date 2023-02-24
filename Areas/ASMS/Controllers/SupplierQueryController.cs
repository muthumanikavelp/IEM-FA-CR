using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using System.Reflection;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierQueryController : Controller
    {
        IRepositoryRenewal isq;

        public SupplierQueryController()
            : this(new SupRenwalModel())
        {

        }

        public SupplierQueryController(IRepositoryRenewal isqr)
        {
            isq = isqr;
        }

        public ActionResult SupplierQuery()
        {
            SupplierQuery objq = new SupplierQuery();
            List<SupplierQuery> objdept = new List<SupplierQuery>();
            try
            {
                if (Session["qlist"] != null)
                {
                    objq.qSupplierQueryList = (List<SupplierQuery>)Session["qlist"];
                }
                else
                {
                    objq.qSupplierQueryList = isq.GetSupplierquery(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "full", "0", "0", "0", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
               
                    //Session["qDept"] = objq.qDepartment;
                    //Session["qR_Type"] = objq.qRequestTypeList;
                }

                ViewBag.status = "yes";
            }
            catch (Exception ex)
            {

            }
            ViewBag.Stutasdata = new SelectList(isq.GetStatussMode().ToList(), "StutasId", "StutasName");
            ViewBag.Getcategory = new SelectList(isq.Getcategory().ToList(), "categoryId", "categoryName");
            ViewBag.Getorganizationtype = new SelectList(isq.Getorganizationtype().ToList(), "organizationtypeId", "organizationtypeName");
            ViewBag.Getservicetypey = new SelectList(isq.Getservicetypey().ToList(), "servicetypeId", "servicetypeName");
            objq.qDepartment = new SelectList(isq.GetemployeeDepartment().ToList(), "qDepartmentid", "qDepartmentname");
            objq.qRequestTypeList = new SelectList(isq.GetRequestType().ToList(), "qRequestTypeid", "qRequestTypeName");
            return View(objq);
        }

        [HttpPost]
        public ActionResult SupplierQuery(SupplierQuery sobj, string ddlqSupplierStatus, string ddlcategoryname, string ddlorganizationtypename, string ddlservicetypename, string command,string ddlqRequestStatus, string ddlqreuesttype, string ddlqdepartment)
        {
            string status = string.Empty;
            try
            {

                if (ddlqSupplierStatus == "0")
                {
                    status = string.Empty;
                }
                else
                {
                    status = ddlqSupplierStatus;
                }
                sobj.qSupplierQueryList = isq.GetSupplierquery(sobj.qSupplierCode, sobj.qSupplierName, status, sobj.qCont_startDate, sobj.qCont_EndDateNew, "Search"
                    , ddlcategoryname
            , ddlorganizationtypename
            , ddlservicetypename
            , sobj.employeecode
            , sobj.employeename
            ,ddlqRequestStatus
            ,ddlqreuesttype
            ,ddlqdepartment
            ,sobj.qActionDateFrom
            ,sobj.qActiondateTo);
                Session["qlist"] = sobj.qSupplierQueryList;
                ViewBag.status = "yes";
                ViewBag.StutasId = ddlqSupplierStatus;
                ViewBag.categoryId = ddlcategoryname;
                ViewBag.organizationtypename = ddlorganizationtypename;
                ViewBag.servicetypename = ddlservicetypename;

            }
            catch (Exception ex)
            {

            }
            ViewBag.Stutasdata = new SelectList(isq.GetStatussMode().ToList(), "StutasId", "StutasName");
            ViewBag.Getcategory = new SelectList(isq.Getcategory().ToList(), "categoryId", "categoryName");
            ViewBag.Getorganizationtype = new SelectList(isq.Getorganizationtype().ToList(), "organizationtypeId", "organizationtypeName");
            ViewBag.Getservicetypey = new SelectList(isq.Getservicetypey().ToList(), "servicetypeId", "servicetypeName");
            sobj.qDepartment = new SelectList(isq.GetemployeeDepartment().ToList(), "qDepartmentid", "qDepartmentname");
            sobj.qRequestTypeList = new SelectList(isq.GetRequestType().ToList(), "qRequestTypeid", "qRequestTypeName");
            return View(sobj);
        }
        public ActionResult Ssamdownloadsexcel()
        {
            DataTable dtnew = new DataTable();
            if (Session["exceldownload"] != null)
            {
                dtnew = (DataTable)Session["exceldownload"];
            }

            //dtnew.Columns.Add("SNo");
            //dtnew.Columns.Add("Supplier Code");
            //dtnew.Columns.Add("Supplier Name");
            //dtnew.Columns.Add("Invoice Date");
            //dtnew.Columns.Add("Invoice No");
            //dtnew.Columns.Add("Description");
            //dtnew.Columns.Add("Amount");
            //dtnew.Columns.Add("Provision");
            //dtnew.Columns.Add("Nature of Expenses");
            //dtnew.Columns.Add("Main Category");
            //dtnew.Columns.Add("Sub Category");
            //dtnew.Columns.Add("Function Code");
            //dtnew.Columns.Add("Cost Code");
            //dtnew.Columns.Add("Product Code");
            //dtnew.Columns.Add("OU Code");

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierQuery.xls"));
            Response.ContentType = "application/ms-excel";
            DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }
        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;

        }
    }
}
