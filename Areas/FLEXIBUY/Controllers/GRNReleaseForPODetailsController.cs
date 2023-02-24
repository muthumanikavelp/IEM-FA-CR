using IEM.Areas.FLEXIBUY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class GRNReleaseForPODetailsController : Controller
    {
        private Irepositorypr objModel;

        public GRNReleaseForPODetailsController()
            : this(new fb_DataModelpr())
        {

        }
        public GRNReleaseForPODetailsController(Irepositorypr objM)
        {

            objModel = objM;
        }

        public ActionResult Index(int podetails)
        {
            Session["grn_da"] = null;
            Session["grn_shipmentlist"] = null;
            Session["grn_cbfprdeta"] = null;
            poraiser obj = new poraiser();
            shipment objship = new shipment();
            DataTable objtable = new DataTable();
            poraiser objraiser = new poraiser();
            objraiser.podetGid = podetails;
            DataTable objTable = objModel.GetPoDetails(objraiser.podetGid);
            Session["podetail"] = objTable;
            DataTable dt = new DataTable();
            for (int i = 0; i < objTable.Rows.Count; i++)
            {
                dt = objModel.GetShipTable(Convert.ToInt32(objTable.Rows[i]["podetails_gid"].ToString()));
            }
            Session["totalship"] = dt;
            Session["shipdelete"] = dt;
           // objship.shiplist = objModel.GetShipmentDetails(dt);
            obj.objlist = objModel.GetPoDetailsListGRN(objTable);
            obj.podetGid = obj.objlist[0].podetGid;
            obj.poheadGid = obj.objlist[0].poheadGid;
            obj.porefno = obj.objlist[0].porefno;
            obj.podate = obj.objlist[0].podate;
            obj.poenddate = obj.objlist[0].poenddate;
            obj.cbfheadAmount = obj.objlist[0].cbfheadAmount;
            obj.devamount = obj.objlist[0].devamount;
            obj.cbfdetailsQty = obj.objlist[0].cbfdetailsQty;
            obj.projmanagergid = obj.objlist[0].projmanagergid;
            obj.vendorNote = obj.objlist[0].vendorNote;
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
            return PartialView("Index", obj);
        }
        [HttpGet]
        public PartialViewResult Shipment()
        {
            try
            {
                shipment objlist = new shipment();
                if (Session["grn_shipmentlist"] != null)
                {
                    objlist.shiplist = (List<shipment>)Session["grn_shipmentlist"];
                }
                objlist.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");

                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        [HttpPost]
        public PartialViewResult Shipment(poraiser ObjPoraiser)
        {
            try
            {
                shipment objlist = new shipment();
                objlist.shiplist = objModel.getbranchdetailsGrn(ObjPoraiser);
                objlist.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                Session["grn_shipmentlist"] = objlist;
                if(objlist.shiplist.Count==0)
                {
                    ViewBag.records = "No Records Found";
                }
                return PartialView("Shipment", objlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpGet]
        //public PartialViewResult GRNReleaseGrid()
        //{
        //    try
        //    {
        //        ViewBag.NoRecordsFound = "";
        //        List<shipment> objgetprdetails = (List<shipment>)Session["grn_cbfprdeta"];
        //        if (objgetprdetails.Count == 0)
        //        {
        //            ViewBag.NoRecordsFound = "No Records Found";
        //        }
        //        return PartialView("GRNReleaseGrid", objgetprdetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }

        //}
        [HttpGet]
        public PartialViewResult GRNReleaseGrid()
        {
            try
            {
                shipment objshipment = new shipment();
              
                if (Session["grn_cbfprdeta"] != null)
                {

                    objshipment.shiplist = (List<shipment>)Session["grn_cbfprdeta"];
                    objshipment.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                }
                return PartialView("GRNReleaseGrid", objshipment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult releasedQty(shipment objshipment)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                Session["grn_cbfprdeta"] = null;
                DataTable objdt = new DataTable();
                DataTable objdt1 = new DataTable();
                objshipment.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                //   List<shipment> objgetprdetails = new List<shipment>();
                if (Session["grn_da"] != null)
                    objdt1 = (DataTable)Session["grn_da"];

                //if (Session["grn_da"] != null)
                //{
                   // objdt1 = (DataTable)Session["grn_da"];
                    if (objdt1.Rows.Count > 0)
                    {
                        DataRow[] foundship = objdt1.Select("poshipment_gid = '" + objshipment.shipmentgid + "'");
                        if (foundship.Length == 0)
                        {
                                objshipment.shipGid = Convert.ToInt16(objshipment.shipmentgid);
                                objdt = objModel.DtTablegrn(objshipment);
                        }
                    }
                //}
                else
                {
                    objshipment.shipGid = Convert.ToInt16(objshipment.shipmentgid);
                    objdt = objModel.DtTablegrn(objshipment);
                }
                if (objdt != null)
                    objdt1.Merge(objdt);
                Session["grn_da"] = objdt1;
                objshipment.shiplist = objModel.GetGRNDetailsForSave(objdt1);
                Session["grn_cbfprdeta"] = objshipment.shiplist;
                if (objshipment.shiplist.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
                return PartialView("GRNReleaseGrid", objshipment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult releasedQtyTempSave()
        {
            shipment objshipment = new shipment();
            try
            {
                if (Session["grn_da"] != null)
                {
                    objshipment.shiplist = (List<shipment>)Session["grn_cbfprdeta"];
                }
                objshipment.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                return PartialView("GRNReleasedGrid", objshipment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult releasedQtyTempSave(shipment objshipment)
        {
            try
            {
                DataTable objTable = new DataTable();
                if (Session["grn_da"] != null)
                    objTable = (DataTable)Session["grn_da"];
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    if (objTable.Rows[i]["poshipment_gid"].ToString() == objshipment.shipmentgid.ToString())
                    {
                        objTable.Rows[i]["releasedqty"] = objshipment.releasedqty;
                    }
                }
                Session["grn_da"] = objTable;
                Session["balancedQty"] = objshipment.balancedqty;
                objshipment.shiplist = objModel.GetGRNDetailsForTempSave(objTable);
                objshipment.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                return PartialView("GRNReleaseGrid", objshipment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult DeleteRelease(shipment objdelete)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["grn_da"] != null)
                {
                    dt = (DataTable)Session["grn_da"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["poshipment_gid"].ToString() == objdelete.shipmentgid.ToString())
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                    Session["grn_da"] = dt;
                    objdelete.shiplist = objModel.GetGRNDetailsForSave(dt);
                    objdelete.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                }
                if(objdelete.shiplist.Count==0)
                {
                    ViewBag.records = "No Records Found";
                }
                return PartialView("GRNReleaseGrid", objdelete);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult SaveReleaseDetails(shipment objrelease)
        {
            try
            {
                poraiser obj = new poraiser();
                DataTable objTable = new DataTable();
                if (Session["grn_da"]!=null)
                {
                    objTable = (DataTable)Session["grn_da"];
                }
                obj.result = objModel.InsertRelease(objTable);
                if (obj.result != null)
                    return Json(obj.result, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
