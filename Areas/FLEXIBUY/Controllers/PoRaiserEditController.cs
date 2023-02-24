using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Excel;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Collections;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class PoRaiserEditController : Controller
    {
        //
        // GET: /FLEXIBUY/PoRaiserEdit1/
        ForMailController mailsender = new ForMailController();
        ArrayList lalpodetails = new ArrayList();
        ArrayList lalshiplist = new ArrayList();
        ErrorLog objErrorLog = new ErrorLog();
        string deleteflag = "Y";
        private Irepositorypr objModel;
        int count = 0;
        public PoRaiserEditController()
            : this(new fb_DataModelpr())
        {

        }
        public PoRaiserEditController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult PoEdit()
        {
            poraiser obj = new poraiser();
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
            Session["excelshipment"] = null;
            Session["deleteval"] = null;
            Session["poTempdetail"] = null;
            //Session["podelete"]=null;
            //  Session["viewfor"] = null;
            if (Session["polist"] != null)
            {
                obj = (poraiser)Session["polist"];
            }
            obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName");
            obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername");
            //Session["podetail"] = null;
            //Session["details"] = null;
            return View("PoEdit", obj);
        }
        [HttpGet]
        public PartialViewResult PoEditDetails(poraiser objraiser)
        {
            //Session["ShipTemp"] = null;
            Session["viewfor"] = null;
            if (objraiser.viewfor == "edit")
            {
                Session["viewfor"] = "edit";
                // ViewBag.viewfor = "edit";
            }
            else if (objraiser.viewfor == "view")
            {
                Session["viewfor"] = "view";
                //ViewBag.viewfor = "view";
            }
            else if (objraiser.viewfor == "cancel")
            {
                Session["viewfor"] = "cancel";
                //ViewBag.viewfor = "cancel";
            }
            else if (objraiser.viewfor == "delete")
            {
                Session["viewfor"] = "delete";
                //ViewBag.viewfor = "delete";
            }
            else if (objraiser.viewfor == "checkercancel")
            {
                Session["viewfor"] = "checkercancel";
            }
            else if (objraiser.viewfor == "checker")
            {
                Session["viewfor"] = "checker";
            }
            else if (objraiser.viewfor == "closure")
            {
                Session["viewfor"] = "closure";
            }
            else if (objraiser.viewfor == "closurechecker")
            {
                Session["viewfor"] = "closurechecker";
            }
            else if (objraiser.viewfor == "amend")
            {
                Session["viewfor"] = "amend";
            }
            else if (objraiser.viewfor == "delmat")
            {
                Session["viewfor"] = "delmat";
            }

            poraiser obj = new poraiser();
            shipment objship = new shipment();
            DataTable objtable = new DataTable();
            DataTable objTable = objModel.GetPoDetails(objraiser.podetGid);
            Session["podetail"] = objTable;
            DataTable dt = new DataTable();
            for (int i = 0; i < objTable.Rows.Count; i++)
            {
                dt = objModel.GetShipTable(Convert.ToInt32(objTable.Rows[i]["podetails_gid"].ToString()));
            }
            Session["totalship"] = dt;
            Session["shipdelete"] = dt;
            //objship.shiplist = objModel.GetShipmentDetails(dt);
            obj.objlist = objModel.GetPoDetailsList(objTable);
            obj.podetGid = obj.objlist[0].podetGid;
            obj.poheadGid = obj.objlist[0].poheadGid;
            obj.poCancelGid = objraiser.poCancelGid;
            obj.remarks = objraiser.remarks;
            obj.poClosureGid = objraiser.poClosureGid;
            obj.porefno = obj.objlist[0].porefno;
            obj.podate = obj.objlist[0].podate;
            obj.poenddate = obj.objlist[0].poenddate;
            obj.cbfheadAmount = obj.objlist[0].cbfheadAmount;
            obj.devamount = obj.objlist[0].devamount;
            obj.cbfdetailsQty = obj.objlist[0].cbfdetailsQty;
            obj.projmanagergid = obj.objlist[0].projmanagergid;
            obj.vendorNote = obj.objlist[0].vendorNote;
            obj.vendorgid = obj.objlist[0].vendorgid;
            obj.tempName = obj.objlist[0].tempName;
            obj.templname = obj.objlist[0].templname;
            obj.additionTemplate = obj.objlist[0].additionTemplate;
            obj.itType = obj.objlist[0].itType;
            obj.department = obj.objlist[0].department;

            obj.vendorName = obj.objlist[0].vendorName;
            obj.templateGid = obj.objlist[0].templateGid;
            obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
            obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername", obj.projmanagergid);
            obj.raisedby = objModel.GetLoginUser();
            //if (Session["podelete"] != null)
            //    obj.count = (int)Session["podelete"];
            Session["polist"] = obj;
            return PartialView("PoEdit", obj);
        }
        [HttpPost]
        public PartialViewResult PoDetailTempSave(poraiser objraiser)
        {
            try
            {
                ViewBag.podetails = "Second";
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                poraiser obj = new poraiser();
                if (Session["podetail"] != null)
                {
                    dt1 = (DataTable)Session["podetail"];
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        if (dt1.Rows[i]["podetails_gid"].ToString() == objraiser.podetGid.ToString())
                        {
                            dt1.Rows[i]["podetails_qty"] = objraiser.quantity;
                            dt1.Rows[i]["podetails_unitprice"] = objraiser.unitprice;
                            dt1.Rows[i]["podetails_base_amt"] = objraiser.baseamount;
                            dt1.Rows[i]["podetails_desc"] = objraiser.prodservicedesc;
                            dt1.Rows[i]["podetails_discount"] = objraiser.discount;
                            dt1.Rows[i]["podetails_tax1"] = objraiser.tax1;
                            dt1.Rows[i]["podetails_tax2"] = objraiser.tax2;
                            dt1.Rows[i]["podetails_tax3"] = objraiser.tax3;
                            dt1.Rows[i]["podetails_total"] = objraiser.total;
                        }
                    }
                }
                Session["poTempdetail"] = dt1;
                obj.objlist = objModel.GetPoDetailsList(dt1);
                obj.porefno = obj.objlist[0].porefno;
                obj.podate = obj.objlist[0].podate;
                obj.poenddate = obj.objlist[0].poenddate;
                obj.projmanagergid = obj.objlist[0].projmanagergid;
                obj.vendorNote = obj.objlist[0].vendorNote;
                obj.tempName = obj.objlist[0].tempName;
                obj.additionTemplate = obj.objlist[0].additionTemplate;
                obj.itType = obj.objlist[0].itType;
                obj.department = obj.objlist[0].department;
                obj.vendorName = obj.objlist[0].vendorName;
                obj.templateGid = obj.objlist[0].templateGid;
                obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
                obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername", obj.projmanagergid);
                obj.raisedby = objModel.GetLoginUser();
                return PartialView("PoEditDetails", obj);
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
                if (Session["podetail"] != null)
                    objDt = (DataTable)Session["podetail"];
                if (Session["ShipBulkTable"] != null)
                    dt = (DataTable)Session["ShipBulkTable"];
                if (Session["ShipTemp"] != null)
                    dt = (DataTable)Session["ShipTemp"];
                if (Session["excelFinalTable"] != null)
                    dt = (DataTable)Session["excelFinalTable"];
                if (Session["shipdelete"] != null)
                    dt = (DataTable)Session["shipdelete"];
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["poshipment_podet_gid"].ToString() == objraiser.podetGid.ToString())
                            //objDt = objModel.DeletePoDetails(objraiser.podetGid);
                            dt.Rows.RemoveAt(j);
                        if (Session["shiparraylist"] != null)
                            lalshiplist = (ArrayList)Session["shiparraylist"];
                        lalshiplist.Add(objraiser.podetGid.ToString());
                        Session["shiparraylist"] = lalshiplist;
                    }
                }
                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    if (objDt.Rows[i]["podetails_gid"].ToString() == objraiser.podetGid.ToString())
                        //objDt = objModel.DeletePoDetails(objraiser.podetGid);
                        objDt.Rows.RemoveAt(i);

                    if (Session["ArrayList"] != null)
                        lalpodetails = (ArrayList)Session["ArrayList"];
                    lalpodetails.Add(objraiser.podetGid.ToString());
                    Session["ArrayList"] = lalpodetails;
                }
                obj.objlist = objModel.getpolist(objDt);
                return PartialView("PoEditDetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public PartialViewResult shipmentdetails(int poDetGid)
        {
            try
            {
                shipment obj = new shipment();
                int count = 0;
                DataTable objTable = new DataTable();
                // DataView dv = new DataView();
                Session["poDetGid"] = poDetGid;
                // obj.cbfdetGid = cbfdetGid;
                //  List<shipment> lLstShip = new List<shipment>();
                obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                if (Session["ShipTemp"] != null)
                {
                    DataTable objtable = (DataTable)Session["ShipTemp"];
                    DataView dv = new DataView();
                    objtable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                    dv = objtable.DefaultView;
                    objtable = dv.ToTable();
                    if (objtable.Rows.Count == 0)
                    {
                        DataTable dt = objModel.GetShipTable(poDetGid);
                        //if (dt.Columns.Count == 10)
                        if (!dt.Columns.Contains("Slno") && !dt.Columns.Contains("deleteflag"))
                        {
                            dt.Columns.Add("Slno");
                            dt.Columns.Add("deleteflag");
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["Slno"] = count + 1;

                            }
                        }
                        objtable = (DataTable)Session["ShipTemp"];
                        if (dt.Rows.Count > 0)
                        {
                            objtable.Merge(dt);
                        }
                        Session["ShipTemp"] = objtable;
                    }
                }
                if (Session["ShipBulkTable"] == null && Session["ShipTemp"] == null && Session["excelFinalTable"] == null)
                {
                    DataTable dt = objModel.GetShipTable(poDetGid);
                    if (!dt.Columns.Contains("Slno"))
                        dt.Columns.Add("Slno");
                    if (!dt.Columns.Contains("deleteflag"))
                        dt.Columns.Add("deleteflag");
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["deleteflag"].ToString() == "Y")
                                dt.Rows[i]["Slno"] = count + 1;

                        }
                    }
                    Session["ShipTemp"] = dt;
                    obj.shiplist = objModel.GetShipmentDetails(dt);
                }

                if (Session["ShipBulkTable"] != null)
                {
                    objTable = (DataTable)Session["ShipBulkTable"];
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        if (Session["deleteval"] == null || Session["deleteval"] == "")
                        {
                            Session["deleteval"] = objTable.Rows[i]["deleteflag"].ToString();
                        }
                    }
                    if (Session["deleteval"] != "")
                    {
                        DataView dv = new DataView();
                        objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                        //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] +"";
                        dv = objTable.DefaultView;
                        objTable = dv.ToTable();
                        obj.shiplist = objModel.GetShipmentDetails(objTable);
                    }
                    else
                    {
                        DataView dv = new DataView();
                        objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                        dv = objTable.DefaultView;
                        objTable = dv.ToTable();
                        obj.shiplist = objModel.GetShipmentDetails(objTable);
                    }
                    //DataView dv = new DataView();
                    //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                    //dv = objTable.DefaultView;
                    //objTable = dv.ToTable();
                    //obj.shiplist = objModel.GetShipmentDetails(objTable);
                }

                else
                {
                    if (Session["ShipTemp"] != null)
                    {
                        objTable = (DataTable)Session["ShipTemp"];
                        for (int i = 0; i < objTable.Rows.Count; i++)
                        {
                            if (Session["deleteval"] == null || Session["deleteval"] == "")
                            {
                                Session["deleteval"] = objTable.Rows[i]["deleteflag"].ToString();
                            }
                        }
                        if (Session["deleteval"] != "" && Session["deleteval"] != null)
                        {
                            DataView dv = new DataView();
                            //objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                            objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                            //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] +"";
                            dv = objTable.DefaultView;
                            objTable = dv.ToTable();
                            obj.shiplist = objModel.GetShipmentDetails(objTable);
                        }
                        else
                        {
                            DataView dv = new DataView();
                            objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                            dv = objTable.DefaultView;
                            objTable = dv.ToTable();
                            obj.shiplist = objModel.GetShipmentDetails(objTable);
                        }
                    }

                    if (Session["excelFinalTable"] != null)
                    {
                        objTable = (DataTable)Session["excelFinalTable"];
                        DataView dv1 = new DataView();
                        objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                        dv1 = objTable.DefaultView;
                        objTable = dv1.ToTable();
                        obj.shiplist = objModel.ExcelToShipment(objTable);
                    }
                }

                if (Session["count"] == null)
                {
                    Session["count"] = obj.shiplist.Count;
                }
                return PartialView("ShipmentViewDetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult shipmentsave()
        {
            shipment objlist = new shipment();
            if (Session["shipmentlist"] != null)
            {
                objlist.shiplist = (List<shipment>)Session["shipmentlist"];
            }
            objlist.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            return PartialView("ShipmentViewDetails", objlist);
        }
        [HttpPost]
        public PartialViewResult saveshipment(shipment objShipmentdetails)
        {
            try
            {
                DataTable objTempTable = new DataTable();
                objShipmentdetails.branchgid = objModel.GetBranchId(objShipmentdetails.branchCode);
                int empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
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
                    if (Session["poDetGid"] != null)
                        objTempTable.Rows.Add(count + 1, objShipmentdetails.shipmentgid, objShipmentdetails.branchCode, objShipmentdetails.shippedqty,
                        count + 1, Session["poDetGid"], objShipmentdetails.branchgid, 0, "", objShipmentdetails.branchName, objShipmentdetails.address,
                        objShipmentdetails.location, objShipmentdetails.inchargeID);
                    Session["ShipBulkTable"] = objTempTable;
                    Session["count"] = count + 1;
                }
                else
                {
                    if (Session["ShipTemp"] != null)
                    {
                        objTempTable = (DataTable)Session["ShipTemp"];
                        count = objTempTable.Rows.Count;
                        objTempTable.Rows.Add(Session["poDetGid"], 0, objShipmentdetails.shipmentgid, objShipmentdetails.branchgid, objShipmentdetails.shippedqty,
                        objShipmentdetails.branchCode, objShipmentdetails.branchName, objShipmentdetails.address,
                        objShipmentdetails.location, objShipmentdetails.inchargeID, empgid, count + 1, "");
                        Session["ShipTemp"] = objTempTable;
                        Session["count"] = count + 1;
                    }
                    //else
                    //{
                    //    objTempTable = (DataTable)Session["ShipTemp"];
                    //    //objTempTable.DefaultView.RowFilter = "cbfdetgid=" + TempData["cbfdetGid"] + "";
                    //    //dv = objTempTable.DefaultView;
                    //    //if (dv.Count == 0)
                    //    //{
                    //    count = objTempTable.Rows.Count;
                    //    if (Session["cbfdetSno"] != null)
                    //        objTempTable.Rows.Add(count + 1, objShipmentdetails.shipmentgid, objShipmentdetails.branchName, objShipmentdetails.address,
                    //        objShipmentdetails.location, objShipmentdetails.inchargeID, objShipmentdetails.branchgid, objShipmentdetails.branchCode, objShipmentdetails.shippedqty, Session["cbfdetSno"]);
                    //}
                }
                if (objTempTable.Rows.Count > 0)
                {
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (Session["deleteval"] == null || Session["deleteval"] == "")
                        {
                            Session["deleteval"] = objTempTable.Rows[i]["deleteflag"].ToString();
                        }
                    }
                    if (Session["deleteval"] != "")
                    {
                        DataView dv = new DataView();
                        objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                        //objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                        //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] +"";
                        dv = objTempTable.DefaultView;
                        objTempTable = dv.ToTable();
                        objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                        Session["shipmentlist"] = objShipmentdetails.shiplist;
                    }
                    else
                    {
                        DataView dv = new DataView();
                        objTempTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                        dv = objTempTable.DefaultView;
                        objTempTable = dv.ToTable();
                        objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                        Session["shipmentlist"] = objShipmentdetails.shiplist;
                    }
                    //DataView dv = new DataView();
                    //objTempTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                    //dv = objTempTable.DefaultView;
                    //objTempTable = dv.ToTable();
                    //objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                    //Session["lShipList"] = objShipmentdetails.shiplist;
                    objShipmentdetails.selectedValue = objShipmentdetails.shipmentgid;
                    objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                    Session["shipmentlist"] = objShipmentdetails.shiplist;

                }
                return PartialView("ShipmentViewDetails", objShipmentdetails);
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
                int ij = 0;
                DataView dv = new DataView();
                objTempTable = (DataTable)Session["ShipBulkTable"];
                Session["shipdeleted"] = (DataTable)Session["ShipBulkTable"];
                //dv = objTempTable.DefaultView;
                //objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                //objTempTable = dv.ToTable();

                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    if (objTempTable.Rows[i]["deleteflag"].ToString() != "Y")
                        objTempTable.Rows[i]["Slno"] = i - ij + 1;
                    else
                    {
                        ij = ij + 1;
                        objTempTable.Rows[i]["Slno"] = 0;
                    }
                }
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    if ((objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()) && (Convert.ToInt32(objTempTable.Rows[i]["poshipment_gid"]) > 0))
                    {
                        objTempTable.Rows[i]["deleteflag"] = "Y";
                    }
                    else if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString())
                    {
                        objTempTable.Rows.RemoveAt(i);
                    }
                    Session["ShipBulkTable"] = objTempTable;
                    // objTempTable.Rows.RemoveAt(i);
                }
            }
            else
            {
                if (Session["ShipTemp"] != null)
                {
                    int ij = 0;
                    int j = 1;
                    DataView dv = new DataView();
                    objTempTable = (DataTable)Session["ShipTemp"];
                    DataRow[] foundrows;
                    foundrows = objTempTable.Select("poshipment_podet_gid='" + objShipmentdetails.shipGid.ToString() + "'");
                    //dv = objTempTable.DefaultView;
                    //objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    //objTempTable = dv.ToTable();
                    for (int i = 0; i < foundrows.Length; i++)
                    {

                        if (foundrows[i]["deleteflag"].ToString() != "Y")
                        {
                            foundrows[i]["Slno"] = i - ij + 1;
                            //objTempTable.Rows[i]["Slno"] = i - ij + 1;
                        }
                        else
                        {
                            ij = ij + 1;
                            objTempTable.Rows[i]["Slno"] = 0;
                        }
                    }

                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if ((objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()) && (Convert.ToInt32(objTempTable.Rows[i]["poshipment_gid"]) > 0))
                        {
                            objTempTable.Rows[i]["deleteflag"] = "Y";
                        }
                        else if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString())
                        {
                            objTempTable.Rows.RemoveAt(i);
                        }
                        Session["ShipTemp"] = objTempTable;
                        // objTempTable.Rows.RemoveAt(i);
                        //objTempTable.Rows[i]["deleteflag"] = "Y";
                        //if (Session["ArrayList"] != null)

                        //    laLshipDetails = (ArrayList)Session["ArrayList"];
                        //laLshipDetails.Add(objShipmentdetails.cbfdetGid.ToString());
                        //Session["ArrayList"] = laLshipDetails;
                    }
                }
                if (Session["excelFinalTable"] != null)
                {
                    DataView dv = new DataView();
                    objTempTable = (DataTable)Session["excelFinalTable"];
                    //dv = objTempTable.DefaultView;
                    //objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    //objTempTable = dv.ToTable();
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if ((objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()) && (Convert.ToInt32(objTempTable.Rows[i]["poshipment_gid"]) > 0))
                        {
                            objTempTable.Rows[i]["deleteflag"] = "Y";
                        }
                        else if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString())
                        {
                            objTempTable.Rows.RemoveAt(i);
                        }
                        Session["excelFinalTable"] = objTempTable;
                        //if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim())
                        //    objTempTable.Rows.RemoveAt(i);
                    }
                }
            }
            //if (objTempTable.Rows.Count == 0)
            //    Session["ShipTemp"] = null ;

            if (objTempTable.Rows.Count > 0)
            {
                DataView dv = new DataView();
                objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                //objTempTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                dv = objTempTable.DefaultView;
                objTempTable = dv.ToTable();
                objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                Session["shipmentlist"] = objShipmentdetails.shiplist;
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            }
            else
            {
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                Session["shipmentlist"] = objShipmentdetails.shiplist;
            }
            return PartialView("ShipmentViewDetails", objShipmentdetails);
        }

        [HttpPost]
        public PartialViewResult UpdateShipment(shipment objShipmentdetails)
        {
            DataTable objTempTable = new DataTable();
            if (Session["ShipBulkTable"] != null)
            {

                DataView dv = new DataView();
                // objTempTable = (DataTable)Session["ShipBulkTable"];
                objTempTable = (DataTable)Session["ShipBulkTable"];
                dv = objTempTable.DefaultView;
                objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                objTempTable = dv.ToTable();
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    objTempTable.Rows[i]["Slno"] = i + 1;
                }
                for (int i = 0; i < objTempTable.Rows.Count; i++)
                {
                    if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim())
                    {
                        objTempTable.Rows[i]["Quantity"] = objShipmentdetails.shippedqty;
                        //  objTempTable.Rows[i]["empgid"] = objShipmentdetails.empgid;
                        objTempTable.Rows[i]["branch_incharge"] = objShipmentdetails.inchargeID;
                        objTempTable.Rows[i]["employeegid"] = objShipmentdetails.empgid;
                    }
                }
                Session["ShipBulkTable"] = objTempTable;
            }
            else
            {
                if (Session["ShipTemp"] != null)
                {

                    // DataView dv = new DataView();
                    // objTempTable = (DataTable)Session["ShipBulkTable"];
                    objTempTable = (DataTable)Session["ShipTemp"];
                    DataRow[] shiprow = objTempTable.Select("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    //dv = objTempTable.DefaultView;
                    //objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    //objTempTable = dv.ToTable();
                    //objTempTable = (DataTable)Session["ShipTemp"];
                    for (int i = 0; i < shiprow.Length; i++)
                    {
                        shiprow[i]["Slno"] = i + 1;
                    }
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim()
                            && objTempTable.Rows[i]["deleteflag"].ToString() != "Y")
                        {
                            objShipmentdetails.empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
                            objTempTable.Rows[i]["Quantity"] = objShipmentdetails.shippedqty;
                            objTempTable.Rows[i]["branch_incharge"] = objShipmentdetails.inchargeID;
                            objTempTable.Rows[i]["employeegid"] = objShipmentdetails.empgid;
                        }
                    }
                    Session["ShipTemp"] = objTempTable;
                }
                if (Session["excelFinalTable"] != null)
                {
                    DataView dv = new DataView();
                    objTempTable = (DataTable)Session["excelFinalTable"];
                    dv = objTempTable.DefaultView;
                    objTempTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    objTempTable = dv.ToTable();
                    for (int i = 0; i < objTempTable.Rows.Count; i++)
                    {
                        if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim())
                        {
                            objShipmentdetails.empgid = objModel.GetemployId(objShipmentdetails.inchargeID);
                            objTempTable.Rows[i]["Quantity"] = objShipmentdetails.shippedqty;
                            objTempTable.Rows[i]["branch_incharge"] = objShipmentdetails.inchargeID;
                            objTempTable.Rows[i]["employeegid"] = objShipmentdetails.empgid;
                        }
                        //if (objTempTable.Rows[i]["Slno"].ToString() == objShipmentdetails.SrNo.ToString().Trim())
                        //    objTempTable.Rows.RemoveAt(i);
                    }
                    Session["excelFinalTable"] = objTempTable;
                }
            }
            if (objTempTable.Rows.Count > 0)
            {

                objShipmentdetails.shiplist = objModel.GetShipmentDetails(objTempTable);
                Session["shipmentlist"] = objShipmentdetails.shiplist;
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
            }
            else
            {
                objShipmentdetails.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                objShipmentdetails.shiplist = objModel.GetShipment(objTempTable);
                Session["shipmentlist"] = objShipmentdetails.shiplist;

            }
            return PartialView("ShipmentViewDetails", objShipmentdetails);
        }
        [HttpPost]
        public JsonResult getshipmentlist()
        {
            shipment obj = new shipment();
            return Json(objModel.getshipmentType(), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult BranchDetails(string listfor)
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
            return PartialView("BranchDetails", objbanklist);
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
            return Json(objowner, JsonRequestBehavior.AllowGet);
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
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ShipmentTemplate.xls"));
            Response.ContentType = "application/ms-excel";
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
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Shipment.xls"));
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
        //public JsonResult ColumnValidation(BulkUpload objValid)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        string strCols = string.Empty;
        //        string[] strColArr;
        //        TempData["quantity"] = objValid.quantity;
        //        Session["cbfSlNo"] = objValid.cbfSlNo;
        //        string excelname = objValid.sheetName + "$";
        //        string excelConnectionString = Session["excelFilePathLocal"].ToString();
        //        using (OleDbConnection con = new OleDbConnection(excelConnectionString))
        //        {
        //            var dataTable = new DataTable();
        //            string query = string.Format("SELECT * FROM [{0}]", excelname);
        //            con.Open();
        //            OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
        //            adapter.Fill(dataTable);
        //            Session["ExcelTable"] = dataTable;

        //            foreach (DataColumn dtColumn in dataTable.Columns)
        //            {
        //                strCols = strCols + dtColumn.ColumnName.Trim() + ":";
        //            }
        //            strCols = strCols.Substring(0, strCols.Length - 1);
        //            strColArr = strCols.Split(':');
        //            if (strColArr[0].ToString() == "SNo" && strColArr[1].ToString() == "ShipmentType" && strColArr[2].ToString() == "BranchCode" && strColArr[3].ToString() == "Quantity" && strColArr.Count().ToString() == "4")
        //            {
        //                result = "Success";
        //            }
        //        }
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                    DataView dv = new DataView();
                    // objTempTable = (DataTable)Session["excelFinalTable"];
                    dv = objDt.DefaultView;
                    objDt.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    objDt = dv.ToTable();
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
                if (objErrTable.Rows.Count == 0)
                {
                    Session["maindatatable"] = objMainTable;
                    DataTable dt = new DataTable();
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
                    Session["ShipBulkTable1"] = dt;
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
            return PartialView("ShipmentViewDetails", obj);
        }
        [HttpPost]
        public PartialViewResult ExcelToShipment()
        {
            try
            {
                int slNo = 0;
                int poDetGid = 0;
                //if (Session["cbfSlNo"] != null)
                //    slNo = (int)Session["cbfSlNo"];
                if (Session["poDetGid"] != null)
                    poDetGid = (int)Session["poDetGid"];
                shipment obj = new shipment();
                DataTable objTable = new DataTable();
                DataTable dt = new DataTable();
                objTable = (DataTable)Session["ShipBulkTable1"];
                if (objTable.Columns.Count == 4)
                {
                    objTable.Columns.Add("Slno");
                    objTable.Columns.Add("poshipment_podet_gid");
                    objTable.Columns.Add("poshipment_branch_gid");
                    objTable.Columns.Add("poshipment_gid");
                    objTable.Columns.Add("deleteflag");
                }
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    //if (objTable.Rows[i]["Slno"].ToString() == null || objTable.Rows[i]["Slno"].ToString() == "")
                    //    objTable.Rows[i]["Slno"] = slNo;
                    if (objTable.Rows[i]["poshipment_podet_gid"].ToString() == null || objTable.Rows[i]["poshipment_podet_gid"].ToString() == "")
                        objTable.Rows[i]["poshipment_podet_gid"] = poDetGid;
                    //if (objTable.Rows[i]["branchgid"].ToString() == null || objTable.Rows[i]["branchgid"].ToString() == "")
                    //    objTable.Rows[i]["branchgid"] = (int)Session["branchgid"];
                }
                Session["excelFinalTable"] = objTable;
                Session["excelshipment"] = objModel.ShipmentFinalEditTable(objTable);
                obj.shiplist = objModel.ExcelShipmentEdit(objTable);

                for (int j = 0; j < obj.shiplist.Count; j++)
                {
                    for (int s = 0; s < objTable.Rows.Count; s++)
                    {
                        objTable.Rows[s]["poshipment_branch_gid"] = obj.shiplist[j].branchgid;
                    }
                }
                Session["excelFinalTable"] = objTable;

                if (Session["ShipTemp"] != null)
                {
                    DataTable objTemp = new DataTable();
                    objTemp = (DataTable)Session["ShipTemp"];
                    DataView dv = new DataView();
                    // objTempTable = (DataTable)Session["excelFinalTable"];
                    //dv = objTemp.DefaultView;
                    //objTemp.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                    //objTemp = dv.ToTable();
                    //for (int i = 0; i < objTemp.Rows.Count; i++)
                    //{
                    //    if (Session["deleteval"] == null || Session["deleteval"] == "")
                    //    {
                    //        Session["deleteval"] = objTable.Rows[i]["deleteflag"].ToString();
                    //    }
                    //}
                    if (objTemp.Rows.Count > 0)
                    {
                        objTable.Merge(objTemp, true, MissingSchemaAction.Ignore);
                    }
                }
                if (objTable.Rows.Count > 0)
                {
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        objTable.Rows[i]["Slno"] = i + 1;
                    }
                }
                Session["ShipBulkTable"] = objTable;
                DataView dataview1 = new DataView();
                //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                //objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                dataview1 = objTable.DefaultView;
                objTable = dataview1.ToTable();
                obj.shiplist = objModel.ExcelShipmentEdit(objTable);
                Session["shiplist"] = obj.shiplist;
                obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                ViewBag.excelupload = "Excel Upload";
                return PartialView("ShipmentViewDetails", obj);
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
                //DataTable objdelShip = new DataTable();
                if (Session["poTempdetail"] != null)
                {
                    objTable = (DataTable)Session["poTempdetail"];
                }
                else
                {
                    if (Session["podetail"] != null)
                        objTable = (DataTable)Session["podetail"];
                }
                //if (Session["shipdeleted"] != null)
                //    objdelShip = (DataTable)Session["shipdeleted"];
                if (Session["ShipBulkTable"] != null)
                {
                    objExShip = (DataTable)Session["ShipBulkTable"];
                }
                if (Session["ShipTemp"] != null)
                    objShipTable = (DataTable)Session["ShipTemp"];
                else
                {
                    if (Session["excelFinalTable"] != null)
                        objShipExcelTable = (DataTable)Session["excelFinalTable"];
                }
                objpoheader.result = objModel.UpdatePo(objpoheader, objTable, objShipTable, objShipExcelTable, objExShip);

                if (objpoheader.result == "Updated Successfully")
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

        public JsonResult PoSummaryDelete(poraiser objdel)
        {
            try
            {
                if (objdel.poheadGid != null)
                    objdel.result = objModel.DeletePoSummaryDetails(objdel);
                if (objdel.result != null)
                {
                    return Json(objdel, JsonRequestBehavior.AllowGet);
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

        public JsonResult PoSummaryCancel(poraiser objdel)
        {
            try
            {
                if (objdel.poheadGid != null)
                    objdel.result = objModel.CancelPoSummaryDetails(objdel);
                if (objdel.result != null)
                {
                    return Json(objdel, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult searchEmp(string listfor = null)
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
            }
            return PartialView("searchEmp", obj);
        }
        [HttpPost]
        public PartialViewResult searchEmp(EmployeeSearch objownersearch)
        {
            EmployeeSearch obj = new EmployeeSearch();
            //List<EmployeeSearch> objowner = new List<EmployeeSearch>();
            obj.lListEmp = objModel.GetEmployeeDetails();
            // objowner = objModel.GetEmployeeDetails();
            Session["objowner"] = null;
            if (objownersearch != null)
            {
                if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
                {
                    TempData["empcode"] = objownersearch.empCode;
                    //ViewBag.empcode = objownersearch.empCode;
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
            return PartialView("searchEmp", obj);
            //return Json(objowner, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult tab_audittraial(poraiser pat)
        {
            try
            {
                fb_DataModelpr model = new fb_DataModelpr();
                poraiser at = new poraiser();
                if (pat.gid != 0)
                {
                    at.auditTrailLst = model.PopulateAuditTrail(pat);
                }
                return PartialView(at);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }

        }

        [HttpGet]
        public PartialViewResult VendorSelectionEdit(string listfor)
        {
            List<vendorselection> lstVendor = new List<vendorselection>();
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
            return PartialView("VendorSelectionEdit", lstVendor);
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
            return PartialView("VendorSelectionEdit", objvendorsearch);
        }
        [HttpPost]
        public JsonResult CheckShipmentQty(string podetgid)
        {
            try
            {
                int PoDetGid = Convert.ToInt32(podetgid);
                DataTable dt = new DataTable();
                DataTable dtTemp = new DataTable();
                if (Session["ShipTemp"] != null)
                {
                    dt = (DataTable)Session["ShipTemp"];
                }
                else
                {
                    dtTemp = objModel.GetShipTable(PoDetGid);
                    if (!dtTemp.Columns.Contains("Slno"))
                        dtTemp.Columns.Add("Slno");
                    if (!dtTemp.Columns.Contains("deleteflag"))
                        dtTemp.Columns.Add("deleteflag");
                    if (dtTemp.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtTemp.Rows.Count; i++)
                        {
                            if (dtTemp.Rows[i]["deleteflag"].ToString() == "Y")
                                dtTemp.Rows[i]["Slno"] = count + 1;

                        }
                    }
                    dt = dtTemp;
                }
                if (dt.Rows.Count > 0)
                {
                   
                    int Qty = 0;
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        if (Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["deleteflag"].ToString()) ? "" : dt.Rows[i]["deleteflag"].ToString()) != "Y")
                        {
                            if (Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["poshipment_podet_gid"].ToString()) ? "0" : dt.Rows[i]["poshipment_podet_gid"].ToString()) == PoDetGid)
                            {
                                Qty += Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["Quantity"].ToString()) ? "0" : dt.Rows[i]["Quantity"].ToString());
                            }
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
        [HttpGet]
        public ActionResult PoEditRT() 
        {
            poraiser obj = new poraiser();
            try
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
                Session["excelshipment"] = null;
                Session["deleteval"] = null;
                Session["poTempdetail"] = null;

                if (Session["polist"] != null)
                {
                    obj = (poraiser)Session["polist"];
                }
                obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName");
                obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername");
           
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            
            return View("PoEditRT", obj);
        }
        
        [HttpGet]
        public PartialViewResult PoEditDetailsRT(string id)
        {
            poraiser objraiser = new poraiser();
            Session["viewfor"] = "view";
            //ViewBag.viewfor = "view";
            objraiser.podetGid = Convert.ToInt32(id);
            poraiser obj = new poraiser();
            try
            {
                shipment objship = new shipment();
                DataTable objtable = new DataTable();
                DataTable objTable = objModel.GetPoDetails(objraiser.podetGid);
                Session["podetail"] = objTable;
                DataTable dt = new DataTable();
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    dt = objModel.GetShipTable(Convert.ToInt32(objTable.Rows[i]["podetails_gid"].ToString()));
                }
                Session["totalship"] = dt;
                Session["shipdelete"] = dt;
                //objship.shiplist = objModel.GetShipmentDetails(dt);
                obj.objlist = objModel.GetPoDetailsList(objTable);
                obj.podetGid = obj.objlist[0].podetGid;
                obj.poheadGid = obj.objlist[0].poheadGid;
                obj.poCancelGid = objraiser.poCancelGid;
                obj.remarks = objraiser.remarks;
                obj.poClosureGid = objraiser.poClosureGid;
                obj.porefno = obj.objlist[0].porefno;
                obj.podate = obj.objlist[0].podate;
                obj.poenddate = obj.objlist[0].poenddate;
                obj.cbfheadAmount = obj.objlist[0].cbfheadAmount;
                obj.devamount = obj.objlist[0].devamount;
                obj.cbfdetailsQty = obj.objlist[0].cbfdetailsQty;
                obj.projmanagergid = obj.objlist[0].projmanagergid;
                obj.vendorNote = obj.objlist[0].vendorNote;
                obj.vendorgid = obj.objlist[0].vendorgid;
                obj.tempName = obj.objlist[0].tempName;
                obj.templname = obj.objlist[0].templname;
                obj.additionTemplate = obj.objlist[0].additionTemplate;
                obj.itType = obj.objlist[0].itType;
                obj.department = obj.objlist[0].department;

                obj.vendorName = obj.objlist[0].vendorName;
                obj.templateGid = obj.objlist[0].templateGid;
                obj.templateList = new SelectList(objModel.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
                obj.projmanagerList = new SelectList(objModel.GetProjectOwnerList(), "projmanagergid", "projmanagername", obj.projmanagergid);
                obj.raisedby = objModel.GetLoginUser();
                //if (Session["podelete"] != null)
                //    obj.count = (int)Session["podelete"];
                Session["polist"] = obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return PartialView("PoEditRT", obj);
        }
    }
}
