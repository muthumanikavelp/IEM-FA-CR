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
    public class SCNReleaseForWODetailsController : Controller
    {
        //
        // GET: /FLEXIBUY/SCNReleaseForWODetails/
        ErrorLog objErrorLog = new ErrorLog();
        private Irepositorypr objModel;

        public SCNReleaseForWODetailsController()
            : this(new fb_DataModelpr())
        {

        }
        public SCNReleaseForWODetailsController(Irepositorypr objM)
        {

            objModel = objM;
        }
        
        [HttpGet]
        public ActionResult Index(int wodetails)
        {
            poraiser obj = new poraiser();
            poRaising objlist = new poRaising();
            List<poraiser> objwo = new List<poraiser>();
           
            try
            {
                obj.objlist = objModel.SCNWOReleaseHead(wodetails);
                obj.podetGid = obj.objlist[0].podetGid;
                obj.poheadGid = obj.objlist[0].poheadGid;
                obj.porefno = obj.objlist[0].porefno;
                obj.podate = obj.objlist[0].podate;
                obj.raisedby = obj.objlist[0].tempName;
                obj.department = obj.objlist[0].department;
                obj.vendorName = obj.objlist[0].vendorName;
                obj.vendorNote = obj.objlist[0].vendorNote;              
                return PartialView("Index", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        public JsonResult GetWODetailsSCN(int wohead)
        {
            poraiser objlist = new poraiser();
            List<poraiser> obj = new List<poraiser>();
            try
            {
                obj = objModel.SCNWODetails(wohead);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveReleaseWODetails(shipment objrelease)
        {
            try
            {
                string headgidWO = Convert.ToString(objrelease.sheetGid);
                poraiser obj = new poraiser();
                obj.result = objModel.InsertWORelease(headgidWO);
                if (obj.result != null && obj.result != "")
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
