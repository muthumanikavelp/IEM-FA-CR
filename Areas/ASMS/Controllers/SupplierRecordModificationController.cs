using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

    [NoDirectAccess]
    public class SupplierRecordModificationController : Controller
    {
        public IRepositoryRenewal Irr;
        public SupplierRecordModificationController()
            :this(new SupRenwalModel())
        {

        }
        public SupplierRecordModificationController(IRepositoryRenewal Irrc)
        {
            Irr = Irrc;
        }

        public ActionResult SupplierRecordModification()
        {
            SupplierR_Modification objActi = new SupplierR_Modification();
            DataSet getdt = new DataSet();
            List<SupplierR_Modification> objvar = new List<SupplierR_Modification>();
            List<SupplierR_Modification> obj = new List<SupplierR_Modification>();
            if (Session["searchresult"] != null && Session["classfication"] != null)
            {
                objActi.RSupplierClassification = new SelectList((List<SupplierR_Modification>)Session["classfication"], "RSupplierClassificationID", "RSupplierClassificationName");

                objActi.SupplierRActiveList = (List<SupplierR_Modification>)Session["searchresult"];
                ViewBag.status = "yes";
            }
            else
            {
                getdt = Irr.Get_SupplierModification(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                if (getdt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                    {
                        objActi = new SupplierR_Modification();
                        objActi.RSupplierheadergid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                        objActi.RSupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                        objActi.RSupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                        objActi.RSupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                        objActi.RSupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                        objActi.RRequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                        objActi.RRequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                        // objActi.ActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                        objvar.Add(objActi);
                    }

                }
               
                if (getdt.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                    {
                        objActi = new SupplierR_Modification();
                        objActi.RSupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                        objActi.RSupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                        obj.Add(objActi);
                    }

                }
                objActi.RSupplierClassification = new SelectList(obj, "RSupplierClassificationID", "RSupplierClassificationName");
                ViewBag.status = "yes";

                objActi.SupplierRActiveList = objvar.ToList();
            }
              

             

            return View(objActi);
        }

        [HttpPost]
        public ActionResult SupplierRecordModification(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            SupplierR_Modification objActi = new SupplierR_Modification();
            DataSet getdt = new DataSet();
            List<SupplierR_Modification> objvar = new List<SupplierR_Modification>();
            List<SupplierR_Modification> obj = new List<SupplierR_Modification>();
            getdt = Irr.Get_SupplierModification(txtSuppliercode, txtSupplierName, ddlSupplierStatus, ddlSup_Clasification, ddlRequestType, ddlRequestStatus);
            if (getdt.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < getdt.Tables[0].Rows.Count; i++)
                {
                    objActi = new SupplierR_Modification();
                    objActi.RSupplierheadergid = getdt.Tables[0].Rows[i]["SupplierHeadergid"].ToString();
                    objActi.RSupplierCode = getdt.Tables[0].Rows[i]["SupplierCode"].ToString();
                    objActi.RSupplierName = getdt.Tables[0].Rows[i]["SupplierName"].ToString();
                    objActi.RSupplierClassificationName = getdt.Tables[0].Rows[i]["SupClassficationType"].ToString();
                    objActi.RSupplierStatusName = getdt.Tables[0].Rows[i]["CurrentStatus"].ToString();
                    objActi.RRequestTypeName = getdt.Tables[0].Rows[i]["RequestType"].ToString();
                    objActi.RRequestStatusName = getdt.Tables[0].Rows[i]["RequestStatus"].ToString();
                    // objActi.ActivityCount = getdt.Tables[0].Rows[i]["ActivityCount"].ToString();
                    objvar.Add(objActi);
                }
                Session["Researchresult"] = objvar;
            }
            else
            {
                ViewBag.value = "RecordNtFound";
            }
            if (getdt.Tables[1].Rows.Count > 0)
            {

                for (int j = 0; j < getdt.Tables[1].Rows.Count; j++)
                {
                    objActi = new SupplierR_Modification();
                    objActi.RSupplierClassificationID = Convert.ToInt32(getdt.Tables[1].Rows[j]["suppliercategory_gid"].ToString());
                    objActi.RSupplierClassificationName = getdt.Tables[1].Rows[j]["suppliercategory_name"].ToString();
                    obj.Add(objActi);
                }
                Session["Reclassfication"] = obj;
            }

                objActi.RSupplierClassification = new SelectList(obj, "RSupplierClassificationID", "RSupplierClassificationName");
                ViewBag.status = "yes";
            
            objActi.SupplierRActiveList = objvar.ToList();

            return View(objActi);
        }

    }
}
