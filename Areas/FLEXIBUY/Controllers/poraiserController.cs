
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using Excel;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class poraiserController : Controller
    {
        //
        // GET: /FLEXIBUY/poraiser/
        ErrorLog objErrorLog = new ErrorLog();
        ForMailController mailsender = new ForMailController();
        private Irepositorypr objModel;
        int count = 0;
        public poraiserController()
            : this(new fb_DataModelpr())
        {

        }
        public poraiserController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult poraiser1()
        {
            Session["podetail"] = null;
            Session["details"] = null;
            return View();
        }
        [HttpGet]
        public PartialViewResult VendorSelection(string listfor)
        {
            List<vendorselection> lstVendor = new List<vendorselection>();
            TempData["code"] = "";
            TempData["name"] = "";
            if (listfor == "search")
            {
                if (Session["objvendor"] != null)
                    lstVendor = (List<vendorselection>)Session["objvendor"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["vendorcode"] != null)
                    TempData["code"] = TempData["vendorcode"];
                if (TempData["vendorname"] != null)
                    TempData["name"] = TempData["vendorname"];
            }
            else
            {
                Session["objvendor"] = null;
                lstVendor = objModel.getvendorselection();
            }
            return PartialView("VendorSelection", lstVendor);
        }
        [HttpPost]
        public PartialViewResult searchvendor(vendorselection objsearchfilter)
        {
            List<vendorselection> objvendorsearch = new List<vendorselection>();
            objvendorsearch = objModel.getvendorselection();
            if ((string.IsNullOrEmpty(objsearchfilter.vendorcode)) == false)
            {
                TempData["vendorcode"] = objsearchfilter.vendorcode;
                objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorcode == null ||
                    (x.vendorcode.ToUpper().Contains(objsearchfilter.vendorcode.ToUpper()))).ToList();
                Session["objvendor"] = objvendorsearch;
            }
            if ((string.IsNullOrEmpty(objsearchfilter.vendorname)) == false)
            {
                TempData["vendorname"] = objsearchfilter.vendorname;
                objvendorsearch = objvendorsearch.Where(x => objsearchfilter.vendorname == null ||
                    (x.vendorname.ToUpper().Contains(objsearchfilter.vendorname.ToUpper()))).ToList();
                Session["objvendor"] = objvendorsearch;
            }
            if (objvendorsearch.Count == 0)
            {
                TempData["Norecords"] = "No records Found";
            }
            return PartialView("VendorSelection", objvendorsearch);
        }

        [HttpGet]
        public PartialViewResult podetails(string function_param, decimal cbfamount)
        {
            Session["ShipBulkTable"] = null;
            Session["ShipTemp"] = null;
            Session["count"] = null;
            Session["excelFinalTable"] = null;
            Session["cbfSlNo"] = null;
            Session["ExcelTable"] = null;
            Session["Errdatatable"] = null;
            Session["maindatatable"] = null;
            Session["excelFilePathLocal"] = null;
            Session["maindatatablenew"] = null;
            Session["poTempdetail"] = null;
            Session["excelshipment"] = null;
            Session["quant"] = null;
            Session["exceldatas"] = null;
            Session["shipfilename"] = null;
            Session["branchcode"] = null;
            poraiser obj = new poraiser();
            DataTable objtable = new DataTable();
            //if (Session["podetail"] != null)
            //{
            //    objtable = (DataTable)Session["podetail"];
            //    obj.objlist = objModel.getpolist(objtable);
            //}

            string[] words = function_param.Split(',');
            foreach (string detgid in words)
            {
                DataTable dt = objModel.getcbfdetails(detgid);
                objtable.Merge(dt);
            }

            Session["podetail"] = objtable;
            obj.objlist = objModel.getpolist(objtable);
            obj.podate = DateTime.Now.ToShortDateString();
            obj.cbfheadAmount = cbfamount;
            obj.uomgid = obj.objlist[0].uomgid;
            obj.vendorgid = obj.objlist[0].vendorgid;
            obj.prodservgid = obj.objlist[0].prodservgid;
            obj.requestforgid = obj.objlist[0].requestforgid;
            obj.department = obj.objlist[0].department;
            obj.vendorName = obj.objlist[0].vendorName;
            //obj.devamount = obj.objlist[0].devamount;
            obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName");
            obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername");
            obj.raisedby = objModel.GetLoginUser();
            if (Session["podelete"] != null)
                obj.count = (int)Session["podelete"];
            //if (Session["details"] != null)
            //{
            //    obj = (poraiser)Session["details"];
            //}
            obj.result = objModel.GetReqGroup();
            return PartialView("poraiser1", obj);
        }

        [HttpPost]
        public PartialViewResult poraiser1(poraiser objraiser)
        {
            try
            {
                ViewBag.podetails = "Second";
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                poraiser obj = new poraiser();
                Session["quant"] = null;
                if (Session["podetail"] != null)
                {
                    dt1 = (DataTable)Session["podetail"];
                    if (dt1.Columns.Count <= 20)
                    {
                        dt1.Columns.Add("discount");
                        dt1.Columns.Add("tax1");
                        dt1.Columns.Add("tax2");
                        dt1.Columns.Add("tax3");
                        dt1.Columns.Add("total");
                    }
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        if (dt1.Rows[i]["cbfdetails_gid"].ToString() == objraiser.cbfdetailsgid.ToString())
                        {
                            dt1.Rows[i]["cbfdetails_dumqty"] = objraiser.quantity;
                            dt1.Rows[i]["cbfdetails_unitprice"] = objraiser.unitprice;
                            dt1.Rows[i]["cbfdetails_totalamt"] = objraiser.baseamount;
                            dt1.Rows[i]["prodservice_description"] = objraiser.prodservicedesc;
                            dt1.Rows[i]["discount"] = objraiser.discount;
                            dt1.Rows[i]["tax1"] = objraiser.tax1;
                            dt1.Rows[i]["tax2"] = objraiser.tax2;
                            dt1.Rows[i]["tax3"] = objraiser.tax3;
                            dt1.Rows[i]["total"] = objraiser.total;
                        }
                        else
                        {
                            if (dt1.Rows[i]["total"].ToString() == null || dt1.Rows[i]["total"].ToString() == "")
                                dt1.Rows[i]["total"] = dt1.Rows[i]["cbfdetails_unitprice"];
                        }
                    }
                }
                Session["poTempdetail"] = dt1;
                obj.objlist = objModel.getpolist(dt1);
                obj.department = obj.objlist[0].department;
                obj.vendorName = obj.objlist[0].vendorName;
                obj.devamount = obj.objlist[0].devamount;
                // Session["details"] = obj;
                // Session["cbfprdeta"] = objgetprdetails;
                //return json(1, jsonrequestbehavior.allowget);


                return PartialView("podetails", obj);
                //return Json(obj.objlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult PoDelete(poraiser objraiser)
        {
            try
            {
                poraiser obj = new poraiser();
                DataTable objDt = new DataTable();
                DataTable dt = new DataTable();
                DataTable objtable = new DataTable();
                DataTable objtbl = new DataTable();

                if (Session["podetail"] != null)
                    objDt = (DataTable)Session["podetail"];
                if (Session["ShipBulkTable"] != null)
                    dt = (DataTable)Session["ShipBulkTable"];
                if (Session["ShipTemp"] != null)
                    objtable = (DataTable)Session["ShipTemp"];
                if (Session["excelFinalTable"] != null)
                    objtbl = (DataTable)Session["excelFinalTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["cbfdetgid"].ToString() == objraiser.cbfdetailsgid.ToString())
                            //objDt = objModel.DeletePoDetails(objraiser.podetGid);
                            dt.Rows.RemoveAt(j);
                    }
                    Session["ShipBulkTable"] = dt;
                }
                if (objtable.Rows.Count > 0)
                {
                    for (int j = 0; j < objtable.Rows.Count; j++)
                    {
                        if (objtable.Rows[j]["cbfdetgid"].ToString() == objraiser.cbfdetailsgid.ToString())
                            //objDt = objModel.DeletePoDetails(objraiser.podetGid);
                            objtable.Rows.RemoveAt(j);
                    }
                    Session["ShipTemp"] = objtable;
                }

                if (objtbl.Rows.Count > 0)
                {
                    for (int j = 0; j < objtbl.Rows.Count; j++)
                    {
                        if (objtbl.Rows[j]["cbfdetgid"].ToString() == objraiser.cbfdetailsgid.ToString())
                            //objDt = objModel.DeletePoDetails(objraiser.podetGid);
                            objtbl.Rows.RemoveAt(j);
                    }
                    Session["excelFinalTable"] = objtbl;
                }
                if (objDt.Rows.Count > 0)
                {
                    for (int i = 0; i < objDt.Rows.Count; i++)
                    {
                        if (objDt.Rows[i]["cbfdetails_gid"].ToString() == objraiser.cbfdetailsgid.ToString())
                            objDt.Rows.RemoveAt(i);
                    }
                }
                obj.objlist = objModel.getpolist(objDt);
                Session["podelete"] = obj.objlist.Count;
                return PartialView("podetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public PartialViewResult shipmentdetails(int cbfdetGid)
        {
            try
            {
                shipment obj = new shipment();
                DataTable objTable = new DataTable();
                // DataView dv = new DataView();
                Session["cbfdetSno"] = cbfdetGid;
                obj.cbfdetGid = cbfdetGid;
                List<shipment> lLstShip = new List<shipment>();
                obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                obj.shiplist = lLstShip;
                if (Session["ShipTemp"] != null)
                {
                    objTable = (DataTable)Session["ShipTemp"];
                    DataView dv = new DataView();
                    objTable.DefaultView.RowFilter = "cbfdetgid = " + Session["cbfdetSno"] + "";
                    dv = objTable.DefaultView;
                    objTable = dv.ToTable();
                    obj.shiplist = objModel.GetShipment(objTable);
                }
                if (Session["excelFinalTable"] != null)
                {
                    objTable = (DataTable)Session["excelFinalTable"];
                    DataView dv1 = new DataView();
                    objTable.DefaultView.RowFilter = "cbfdetgid = " + Session["cbfdetSno"] + "";
                    dv1 = objTable.DefaultView;
                    objTable = dv1.ToTable();
                    obj.shiplist = objModel.ExcelToShipment(objTable);
                }
                return PartialView("shipmentdetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult saveshipment()
        {
            shipment obj = new shipment();
            if (Session["shipsavelist"] != null)
            {
                obj.shiplist = (List<shipment>)Session["shipsavelist"];
            }
            obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            return PartialView("shipmentdetails", obj);
        }
        [HttpPost]
        public PartialViewResult saveshipment(shipment objShipmentdetails)
        {
            try
            {
                DataTable objTempTable = new DataTable();
                int empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
                objShipmentdetails.branchgid = objModel.GetBranchId(objShipmentdetails.branchCode);
                Session["branchcode"] = objShipmentdetails.branchCode;
                //DataView dv=new DataView();
                //if (Session["count"] != null)
                //{
                //    count = (int)Session["count"];
                //}
                int count = 0;
                if (Session["excelshipment"] != null)
                {
                    objTempTable = (DataTable)Session["excelshipment"];
                    if (Session["count"] != null)

                        count = objTempTable.Rows.Count;
                    else
                        count = Convert.ToInt32(Session["count"]);
                    if (Session["cbfdetSno"] != null)
                        objTempTable.Rows.Add(count + 1, objShipmentdetails.shipmentgid, objShipmentdetails.branchCode, objShipmentdetails.shippedqty,
                        count + 1, Session["cbfdetSno"], objShipmentdetails.branchgid, empgid, objShipmentdetails.branchName, objShipmentdetails.address,
                        objShipmentdetails.location, objShipmentdetails.inchargeID);
                    Session["ShipBulkTable"] = objTempTable;
                    Session["count"] = count + 1;
                }
                else
                {
                    if (Session["ShipTemp"] == null)
                    {
                        objTempTable.Columns.Add("Slno");
                        objTempTable.Columns.Add("shipmenttype");
                        objTempTable.Columns.Add("branchName");
                        objTempTable.Columns.Add("address");
                        objTempTable.Columns.Add("location");
                        objTempTable.Columns.Add("inchargeID");
                        objTempTable.Columns.Add("branchgid");
                        objTempTable.Columns.Add("empgid");
                        objTempTable.Columns.Add("branchCode");
                        objTempTable.Columns.Add("Quantity");
                        objTempTable.Columns.Add("cbfdetgid");


                        if (Session["cbfdetSno"] != null)
                            objTempTable.Rows.Add(count + 1, objShipmentdetails.shipmentgid, objShipmentdetails.branchName, objShipmentdetails.address,
                                objShipmentdetails.location, objShipmentdetails.inchargeID, objShipmentdetails.branchgid, empgid,
                                objShipmentdetails.branchCode, objShipmentdetails.shippedqty, Session["cbfdetSno"]);
                        Session["ShipTemp"] = objTempTable;
                        Session["count"] = count + 1;

                    }
                    else
                    {
                        objTempTable = (DataTable)Session["ShipTemp"];
                        //objTempTable.DefaultView.RowFilter = "cbfdetgid=" + TempData["cbfdetGid"] + "";
                        //dv = objTempTable.DefaultView;
                        //if (dv.Count == 0)
                        //{
                        count = objTempTable.Rows.Count;
                        if (Session["cbfdetSno"] != null)
                            objTempTable.Rows.Add(count + 1, objShipmentdetails.shipmentgid, objShipmentdetails.branchName, objShipmentdetails.address,
                            objShipmentdetails.location, objShipmentdetails.inchargeID, objShipmentdetails.branchgid, empgid,
                            objShipmentdetails.branchCode, objShipmentdetails.shippedqty, Session["cbfdetSno"]);
                    }
                }
                if (objTempTable.Rows.Count > 0)
                {
                    DataView dv = new DataView();
                    objTempTable.DefaultView.RowFilter = "cbfdetgid = " + Session["cbfdetSno"] + "";
                    dv = objTempTable.DefaultView;
                    objTempTable = dv.ToTable();
                    objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                    Session["shipsavelist"] = objShipmentdetails.shiplist;
                    //Session["lShipList"] = objShipmentdetails.shiplist;
                    objShipmentdetails.selectedValue = objShipmentdetails.shipmentgid;
                    objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");

                }
                return PartialView("shipmentdetails", objShipmentdetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult DeleteShipment(shipment objShipmentdetails)
        {

            DataTable objTempTable = new DataTable();
            if (Session["ShipBulkTable"] != null)
            {
                objTempTable = (DataTable)Session["ShipBulkTable"];
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    objTempTable.Rows[i]["Slno"] = i + 1;
                }
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()
                        && objTempTable.Rows[i]["cbfdetgid"].ToString() == objShipmentdetails.cbfdetGid.ToString().Trim())
                        objTempTable.Rows.RemoveAt(i);
                }
                Session["ShipBulkTable"] = objTempTable;
            }
            else
            {
                if (Session["ShipTemp"] != null)
                {
                    objTempTable = (DataTable)Session["ShipTemp"];
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        objTempTable.Rows[i]["Slno"] = i + 1;
                    }
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()
                            && objTempTable.Rows[i]["cbfdetgid"].ToString() == objShipmentdetails.cbfdetGid.ToString().Trim())
                            objTempTable.Rows.RemoveAt(i);
                    }
                    Session["ShipTemp"] = objTempTable;
                }
                if (Session["excelFinalTable"] != null)
                {
                    objTempTable = (DataTable)Session["excelFinalTable"];
                    //for (int i = 0; i < objTempTable.Rows.Count; i++)
                    //{
                    //    objTempTable.Rows[i]["Slno"] = i + 1;
                    //}
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (objTempTable.Rows[i]["cbfdetgid"].ToString() == objShipmentdetails.SrNo.ToString().Trim())
                            objTempTable.Rows.RemoveAt(i);
                    }
                    Session["excelFinalTable"] = objTempTable;
                }
            }
            if (objTempTable.Rows.Count > 0)
            {
                DataView dv = new DataView();
                objTempTable.DefaultView.RowFilter = "cbfdetgid = " + Session["cbfdetSno"] + "";
                dv = objTempTable.DefaultView;
                objTempTable = dv.ToTable();
                objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                Session["shipsavelist"] = objShipmentdetails.shiplist;
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            }
            else
            {
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                Session["shipsavelist"] = objShipmentdetails.shiplist;
            }
            return PartialView("shipmentdetails", objShipmentdetails);
        }
        [HttpPost]
        public PartialViewResult UpdateShipment(shipment objShipmentdetails)
        {
            if (objShipmentdetails.branchCode == null || objShipmentdetails.branchCode == "")
                objShipmentdetails.branchCode = (string)Session["branchcode"];
            DataTable objTempTable = new DataTable();
            objShipmentdetails.branchgid = objModel.GetBranchId(objShipmentdetails.branchCode);

            if (Session["excelshipment"] != null)
            {
                objTempTable = (DataTable)Session["excelshipment"];
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    objTempTable.Rows[i]["Slno"] = i + 1;
                }
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()
                        && objTempTable.Rows[i]["cbfdetgid"].ToString() == objShipmentdetails.cbfdetGid.ToString())
                    {
                        objShipmentdetails.empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
                        objTempTable.Rows[i]["Quantity"] = objShipmentdetails.shippedqty;
                        objTempTable.Rows[i]["inchargeID"] = objShipmentdetails.inchargeID;
                        objTempTable.Rows[i]["empgid"] = objShipmentdetails.empgid;
                    }

                }
                Session["excelshipment"] = objTempTable;
            }
            else
            {
                if (Session["ShipTemp"] != null)
                {
                    objTempTable = (DataTable)Session["ShipTemp"];

                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        objTempTable.Rows[i]["Slno"] = i + 1;
                    }
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()
                            && objTempTable.Rows[i]["cbfdetgid"].ToString() == objShipmentdetails.cbfdetGid.ToString())
                        {
                            objShipmentdetails.empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
                            objTempTable.Rows[i]["Quantity"] = objShipmentdetails.shippedqty;
                            objTempTable.Rows[i]["inchargeID"] = objShipmentdetails.inchargeID;
                            objTempTable.Rows[i]["empgid"] = objShipmentdetails.empgid;
                        }
                    }
                    Session["ShipTemp"] = objTempTable;
                }
            }
            if (objTempTable.Rows.Count > 0)
            {

                DataView dv = new DataView();
                dv = objTempTable.DefaultView;
                objTempTable.DefaultView.RowFilter = "cbfdetgid = " + objShipmentdetails.cbfdetGid.ToString() + "";
                objTempTable = dv.ToTable();
                objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                Session["shipsavelist"] = objShipmentdetails.shiplist;
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            }
            else
            {
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                Session["shipsavelist"] = objShipmentdetails.shiplist;
            }
            return PartialView("shipmentdetails", objShipmentdetails);
        }
        [HttpPost]
        public JsonResult getshipmentlist()
        {
            shipment obj = new shipment();
            return Json(objModel.getshipmentType(), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult branchdetails(string listfor)
        {
            TempData["code"] = "";
            TempData["name"] = "";
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
                objbanklist = objModel.getbranchdetails();
            }
            return PartialView("branchdetails", objbanklist);
        }

        [HttpPost]
        public JsonResult searchbranch(shipment objbranchsearch)
        {
            TempData["branchCode"] = null;
            TempData["branchName"] = null;
            List<shipment> objowner = new List<shipment>();
            objowner = objModel.getbranchdetails();
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
                        (x.branchName.ToUpper().Contains(objbranchsearch.branchName.ToUpper()))).ToList();
                    Session["objbranch"] = objowner;
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> ExcelUplFiles()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["shipfilename"] = hpf;
                    }
                }
                return Json(filename);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        [HttpPost]
        public async Task<JsonResult> FileUpload()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["shipfilename"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["shipfilename"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("Shipment-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    path1 = HoldFileUploadUrlDSA() + n + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SheetDatas> objparent = new List<SheetDatas>();
                SheetDatas objModel;
                int count = 0;
                string sheets = "";
                FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                DataSet result1 = new DataSet();
                if (Extension1 == ".xlsx")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else if (Extension1 == ".xls")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objModel = new SheetDatas();
                    objModel.sheetId = count;
                    objModel.sheetName = error.ToString();
                    objparent.Add(objModel);
                }

                //FileStream stream = new FileStream();
                //stream = File.Open(path1, FileMode.Open, FileAccess.Read);
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //IExcelDataReader excelReaderf = ExcelReaderFactory.CreateBinaryReader(stream);
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                //IExcelDataReader excelReadern = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                //DataSet result = excelReader.AsDataSet();
                //4. DataSet - Create column names from first row

                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel = new SheetDatas();
                    objModel.sheetId = count;
                    objModel.sheetName = sheets.ToString();
                    objparent.Add(objModel);
                }

                Session["exceldatas"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }
        //public async Task<JsonResult> FileUpload()
        //{
        //    HttpPostedFileBase httpfileName = null;
        //    string excelConnectionString = "";
        //    try
        //    {
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                httpfileName = Request.Files[file] as HttpPostedFileBase;
        //                Session["localFileup"] = httpfileName;
        //            }
        //            string Extension = Path.GetFileName(httpfileName.FileName);
        //            string n = string.Format("Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        //            string Extension1 = System.IO.Path.GetExtension(Request.Files[file].FileName);
        //            string path1 = @"C:\temp\" + n + Extension;
        //            if (System.IO.File.Exists(path1))
        //            {
        //                System.IO.File.Delete(path1);
        //            }
        //            Request.Files[file].SaveAs(path1);

        //            excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
        //            Session["excelFilePathLocal"] = excelConnectionString;

        //        }
        //        return Json(GetSheetData(excelConnectionString).ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}

        //public List<SheetDatas> GetSheetData(string excelConnectionString)
        //{
        //    List<SheetDatas> objparent = new List<SheetDatas>();
        //    SheetDatas objSheet;
        //    OleDbConnection con = null;
        //    DataTable dt = null;
        //    con = new OleDbConnection(excelConnectionString);
        //    con.Open();
        //    dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //    if (dt == null)
        //    {
        //        return null;
        //    }
        //    int count = 0;
        //    string sheets = "";
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        sheets = row["TABLE_NAME"].ToString();
        //        sheets = sheets.Replace("$", "");
        //        count++;
        //        objSheet = new SheetDatas();
        //        objSheet.sheetId = count;
        //        objSheet.sheetName = sheets.ToString();
        //        objparent.Add(objSheet);
        //    }
        //    return objparent;
        //}
        public ActionResult DownloadExcel()
        {
            DataTable objTable = new DataTable();
            objTable.Columns.Add("SNo");
            objTable.Columns.Add("ShipmentType");
            objTable.Columns.Add("BranchCode");
            objTable.Columns.Add("Quantity");
            Response.ClearContent();
            Response.Buffer = true;
            //Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("content-disposition", "attachment; filename=ShipmentTemplate.xlsx");
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ShipmentTemplate.xls"));
            //Response.ContentType = "application/ms-excel";
            Response.ContentType = "application/vnd.ms-excel";
            // Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            DataTable dt = objTable;
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
        public ActionResult DownloadErrorLogExcel()
        {
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            Response.ClearContent();
            Response.Buffer = true;
            // Response.AddHeader('Content-Disposition: attachment; filename="Shipment.xls"');
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Shipment.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
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
        //[HttpGet]
        //public PartialViewResult UploadSummaryTable()
        //{
        //    try
        //    {
        //        BulkUpload obj = new BulkUpload();
        //        DataTable objTable = new DataTable();
        //        objTable = (DataTable)Session["maindatatable"];
        //        obj.lListUpload = objModel.GetUploadSummary(objTable);
        //        return PartialView("UploadSummaryTable", obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult ColumnValidation(BulkUpload objValid)
        {
            try
            {
                string result = string.Empty;
                string strCols = string.Empty;
                string[] strColArr;
                TempData["quantity"] = objValid.quantity;
                Session["cbfSlNo"] = objValid.cbfSlNo;
                //string excelname = objValid.sheetName + "$";
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["exceldatas"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[objValid.sheetName.ToString()];
                Session["ExcelTable"] = dataTable;

                // string excelConnectionString = Session["excelFilePathLocal"].ToString();
                //using (OleDbConnection con = new OleDbConnection(excelConnectionString))
                //{
                //    var dataTable = new DataTable();
                //    string query = string.Format("SELECT * FROM [{0}]", excelname);
                //    con.Open();
                //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                //    adapter.Fill(dataTable);
                //    Session["ExcelTable"] = dataTable;

                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo" && strColArr[1].ToString() == "ShipmentType" && strColArr[2].ToString() == "BranchCode" && strColArr[3].ToString() == "Quantity" && strColArr.Count().ToString() == "4")
                {
                    result = "Success";
                }
                //}
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BulkUploadStatus(string ddlSelectsheet)
        {
            BulkUpload objValid = new BulkUpload();
            try
            {
                if (Session["ExcelTable"] != null)
                {
                    DataTable objTable = new DataTable();
                    objTable = (DataTable)Session["ExcelTable"];
                    if (TempData["quantity"] != null)
                        objValid.quantity = (int)TempData["quantity"];
                    ValidateExcel(objTable, objValid.quantity);
                    DataTable objerrTable = new DataTable();
                    objerrTable = (DataTable)Session["Errdatatable"];
                    if (objerrTable.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView();
        }

        private void ValidateExcel(DataTable objTable, int quantity)
        {
            int sno = 0;
            int shipqty = 0;
            int chkQty = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string columnName = string.Empty;

            DataTable objMainTable = new DataTable();
            objMainTable = objTable;
            totalrecord = objMainTable.Rows.Count;
            DataTable objErrTable = new DataTable();
            objErrTable.Columns.Add("SNo");
            objErrTable.Columns.Add("Line");
            objErrTable.Columns.Add("Column Name");
            objErrTable.Columns.Add("Error Description");


            try
            {
                if (Session["ShipTemp"] != null)
                {
                    DataTable objDt = new DataTable();
                    objDt = (DataTable)Session["ShipTemp"];
                    if (objDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < objDt.Rows.Count; i++)
                        {
                            shipqty += Convert.ToInt32(objDt.Rows[i]["Quantity"].ToString());
                        }
                        chkQty = quantity - shipqty;
                    }
                }
                if (objTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    int qty = 0;
                    int totalQty = 0;
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        errs = "";
                        columnName = string.Empty;
                        RowNo = i + 1;
                        if (objTable.Rows[i][1].ToString() == "")
                        {
                            errs = "Shipment Type Should Not be Empty ";
                            columnName = objTable.Columns[1].ToString();
                        }
                        else
                        {
                            exceltext = objTable.Rows[i][1].ToString();
                            status = objModel.GetShipmentStatus(exceltext, "ShipmentType");
                            if (status == "notexists")
                            {
                                if (errs == "" && columnName == "")
                                {
                                    errs = "Shipment Type Was Not Found ";
                                    columnName = objTable.Columns[1].ToString();
                                }
                                else
                                {
                                    errs = errs + "," + "Shipment Type Was Not Found ";
                                    columnName = columnName + "," + objTable.Columns[1].ToString();
                                }
                            }
                        }

                        if (objTable.Rows[i][2].ToString() == "")
                        {

                            if (errs == "" && columnName == "")
                            {
                                errs = "Branch Code Should Not be Empty";
                                columnName = objTable.Columns[2].ToString();
                            }
                            else
                            {
                                errs = errs + "," + "Branch Code Should Not be Empty";
                                columnName = columnName + "," + objTable.Columns[2].ToString();
                            }
                        }
                        else
                        {
                            exceltext = objTable.Rows[i][2].ToString();
                            status = objModel.GetShipmentStatus(exceltext, "branchCode");
                            if (status == "notexists")
                            {
                                if (errs == "" && columnName == "")
                                {
                                    errs = "Branch Code Was Not Found ";
                                    columnName = objTable.Columns[2].ToString();
                                }
                                else
                                {
                                    errs = errs + "," + "Branch Code Was Not Found ";
                                    columnName = columnName + "," + objTable.Columns[2].ToString();
                                }
                            }

                        }
                        if (objTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "" && columnName == "")
                            {
                                errs = "Quantity Should Not be Empty";
                                columnName = objTable.Columns[3].ToString();
                            }
                            else
                            {
                                errs = errs + "," + "Quantity Should Not be Empty";
                                columnName = columnName + "," + objTable.Columns[3].ToString();
                            }
                        }
                        else
                        {
                            qty = Convert.ToInt32(objTable.Rows[i][3].ToString());
                            totalQty += qty;
                            //if (qty > quantity)
                            //    errs = errs + "," + "Shipped Quantity was greater than the Actual Quantity";
                            if (chkQty != 0)
                            {
                                if (totalQty > chkQty)
                                {
                                    if (errs == "" && columnName == "")
                                    {
                                        errs = "Shipped Quantity was greater than the Actual Quantity";
                                        columnName = objTable.Columns[3].ToString();
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Shipped Quantity was greater than the Actual Quantity";
                                        columnName = columnName + "," + objTable.Columns[3].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (totalQty > quantity)
                                {
                                    if (errs == "" && columnName == "")
                                    {
                                        errs = "Shipped Quantity was greater than the Actual Quantity";
                                        columnName = objTable.Columns[3].ToString();
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Shipped Quantity was greater than the Actual Quantity";
                                        columnName = columnName + "," + objTable.Columns[3].ToString();
                                    }
                                }
                            }
                        }

                        if (errs != "")
                        {
                            sno++;
                            objErrTable.Rows.Add(sno, RowNo, columnName, errs);
                        }
                    }

                }

                else
                {
                    objErrTable.Rows.Add(1, "Please  Select Valid Sheet");
                }
                invalid = objErrTable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.validRecords = "Total No of Vaild Records :" + valid;
                ViewBag.invalidRecords = "Total No of InVaild Records :" + invalid;
                ViewBag.totalRecords = "Total No of Records in Excel File :" + totalrecord;
                Session["Errdatatable"] = objErrTable;
                Session["maindatatable"] = objMainTable;
                DataTable dt = new DataTable();
                if (objErrTable.Rows.Count == 0)
                {
                    if (Session["ShipBulkTable"] == null)
                    {
                        dt = (DataTable)Session["maindatatable"];
                    }
                    else
                    {
                        dt = (DataTable)Session["ShipBulkTable"];
                    }

                    if (objMainTable != null)
                        dt.Merge(objMainTable);
                    Session["ShipBulkTable"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ExcelToShip()
        {
            shipment obj = new shipment();
            if (Session["shiplist"] != null)
                obj.shiplist = (List<shipment>)Session["shiplist"];
            obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            return PartialView("shipmentdetails", obj);
        }
        [HttpPost]
        public PartialViewResult ExcelToShipment()
        {
            try
            {
                int slNo = 0;
                int cbfgid = 0;
                if (Session["cbfSlNo"] != null)
                    slNo = (int)Session["cbfSlNo"];
                if (Session["cbfdetSno"] != null)
                    cbfgid = (int)Session["cbfdetSno"];
                shipment obj = new shipment();
                DataTable objTable = new DataTable();
                DataTable dt = new DataTable();
                objTable = (DataTable)Session["ShipBulkTable"];
                if (objTable.Columns.Count == 4)
                {
                    objTable.Columns.Add("Slno");
                    objTable.Columns.Add("cbfdetgid");
                    objTable.Columns.Add("branchgid");
                    objTable.Columns.Add("empgid");
                }
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    if (objTable.Rows[i]["Slno"].ToString() == null || objTable.Rows[i]["Slno"].ToString() == "")
                        objTable.Rows[i]["Slno"] = slNo;
                    if (objTable.Rows[i]["cbfdetgid"].ToString() == null || objTable.Rows[i]["cbfdetgid"].ToString() == "")
                        objTable.Rows[i]["cbfdetgid"] = cbfgid;
                    //if (objTable.Rows[i]["branchgid"].ToString() == null || objTable.Rows[i]["branchgid"].ToString() == "")
                    //    objTable.Rows[i]["branchgid"] = (int)Session["branchgid"];
                }
                Session["excelFinalTable"] = objTable;
                Session["excelshipment"] = objModel.ShipmentFinalTable(objTable);
                obj.shiplist = objModel.ExcelToShipment(objTable);

                for (int j = 0; j < obj.shiplist.Count; j++)
                {
                    for (int s = 0; s < objTable.Rows.Count; s++)
                    {
                        objTable.Rows[s]["branchgid"] = obj.shiplist[j].branchgid;
                    }
                }
                Session["excelFinalTable"] = objTable;

                if (Session["ShipTemp"] != null)
                {
                    DataTable objTemp = new DataTable();
                    objTemp = (DataTable)Session["ShipTemp"];
                    if (objTemp.Rows.Count > 0)
                    {
                        objTable.Merge(objTemp, true, MissingSchemaAction.Ignore);
                    }
                }
                Session["ShipBulkTable"] = objTable;
                DataView dv = new DataView();
                objTable.DefaultView.RowFilter = "cbfdetgid = " + Session["cbfdetSno"] + "";
                dv = objTable.DefaultView;
                objTable = dv.ToTable();
                obj.shiplist = objModel.ExcelToShipment(objTable);
                Session["shiplist"] = obj.shiplist;
                obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                ViewBag.excelupload = "Excel Upload";
                return PartialView("shipmentdetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CreatePoTemplate()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult SavePoTemplate(PoTemplate objTemplate)
        {
            objTemplate.result = objModel.InsertPoTemplate(objTemplate);
            if (objTemplate.result != null)
            {
                return Json(objTemplate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TemplateDetails(int id)
        {
            try
            {
                PoTemplate objTemplate = new PoTemplate();
                if (id != null)
                {
                    objTemplate.templateContent = objModel.GetTemplateContent(id);
                }
                return Json(objTemplate.templateContent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpPost]
        public JsonResult GetTemplateList()
        {
            return Json(objModel.GetTemplateList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePoDetails(poraiser objpoheader)
        {
            try
            {
                DataTable objTable = new DataTable();
                DataTable objShipTable = new DataTable();
                DataTable objShipExcelTable = new DataTable();
                DataTable objExShip = new DataTable();
                if (Session["poTempdetail"] != null)
                {
                    objTable = (DataTable)Session["poTempdetail"];
                }
                else
                {
                    if (Session["podetail"] != null)
                        objTable = (DataTable)Session["podetail"];
                }
                if (Session["ShipBulkTable"] != null)
                {
                    objExShip = (DataTable)Session["ShipBulkTable"];
                }
                else
                {
                    if (Session["ShipTemp"] != null)
                        objShipTable = (DataTable)Session["ShipTemp"];
                    if (Session["excelFinalTable"] != null)
                        objShipExcelTable = (DataTable)Session["excelFinalTable"];
                }

                objpoheader.result = objModel.InsertPo(objpoheader, objTable, objShipTable, objShipExcelTable, objExShip);

                if (objpoheader.result == "Inserted Successfully")
                {
                    if (objpoheader.actionName == "Submit")
                    {
                        CbfSumModel objforMail = new CbfSumModel();
                        int refgid = objforMail.GetRefGidForMail("PO");
                        string reqstatus = objforMail.GetRequestStatus(refgid, "PO");
                        int queuegid = objforMail.GetQueueGidForMail(refgid, "PO");
                        string curapprovalstage = "0";
                        if (reqstatus == "S")
                        {
                            curapprovalstage = "0";
                        }
                        else
                        {
                            curapprovalstage = "1";
                        }
                        mailsender.sendusermail("FB", "PO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                    }
                    return Json(objpoheader.result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult searchEmploy(string listfor = null)
        {
            // List<Employee_gid> objowner = new List<Employee_gid>();
            EmployeeSearch obj = new EmployeeSearch();
            if (listfor == "search")
            {
                if (Session["objowner"] != null)
                    obj.lListEmp = (List<EmployeeSearch>)Session["objowner"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["empcode"] != null)
                    TempData["code"] = TempData["empcode"];
                if (TempData["empname"] != null)
                    TempData["name"] = TempData["empname"];
            }
            else
            {
                Session["objowner"] = "";
                obj.lListEmp = objModel.GetEmployeeDetails();
                obj.rowNum = listfor;
                Session["rownumemp"] = listfor;
            }
            return PartialView("searchEmploy", obj);
        }
        [HttpPost]
        public PartialViewResult searchEmploy(EmployeeSearch objownersearch)
        {
            EmployeeSearch obj = new EmployeeSearch();
            //List<EmployeeSearch> objowner = new List<EmployeeSearch>();
            obj.lListEmp = objModel.GetEmployeeDetails();
            // objowner = objModel.GetEmployeeDetails();
            // Session["objowner"] = null;
            if (objownersearch != null)
            {
                if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
                {
                    TempData["empcode"] = objownersearch.empCode;
                    // ViewBag.empcode = objownersearch.empCode;
                    obj.lListEmp = obj.lListEmp.Where(x => objownersearch.empCode == null ||
                        (x.empCode.ToUpper().Contains(objownersearch.empCode.ToUpper()))).ToList();
                    Session["objowner"] = obj.lListEmp;
                }
                if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
                {
                    TempData["empname"] = objownersearch.empName;
                    //ViewBag.empname = objownersearch.empName;
                    obj.lListEmp = obj.lListEmp.Where(x => objownersearch.empName == null ||
                        (x.empName.ToUpper().Contains(objownersearch.empName.ToUpper()))).ToList();
                    Session["objowner"] = obj.lListEmp;
                }
                if (obj.lListEmp.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
            }
            return PartialView("searchEmploy", obj);
            //return Json(objowner, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckShipmentQty(string cbfdetgid) 
        {
            try
            {
                int CBFDetGid = Convert.ToInt32(cbfdetgid); 
                DataTable dt = new DataTable();
                if (Session["ShipTemp"] != null)
                {
                    dt = (DataTable)Session["ShipTemp"];
                }
               
                if (dt.Rows.Count > 0)
                {

                    int Qty = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["cbfdetgid"].ToString()) ? "0" : dt.Rows[i]["cbfdetgid"].ToString()) == CBFDetGid)
                            {
                                Qty += Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["Quantity"].ToString()) ? "0" : dt.Rows[i]["Quantity"].ToString());
                            }
                    }
                    return Json(Qty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
