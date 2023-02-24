using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Text;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

    [NoDirectAccess]
    public class SupplierTrigerController : Controller
    {
        private IRepositoryRenewal IreS;
        public SupplierTrigerController() : this (new SupRenwalModel())
        {

        }
        public SupplierTrigerController(IRepositoryRenewal IreeS)
        {
            IreS = IreeS;
        }

        
        public ActionResult RenewalTrigger()
        {
            SupplierTriggerRenewal obj = new SupplierTriggerRenewal();
            obj.status = "yes";
            obj.SupplierTrigger = IreS.GetTriggerDetails(obj).ToList();
            ViewBag.status = "grid";
            return View(obj);
        }

        [HttpPost]
        public ActionResult RenewalTrigger(SupplierTriggerRenewal subobj)
        {
            string result = string.Empty;
            try
            {
                result = IreS.GetTriggercount(subobj.Trigger_before.ToString());
                if (result == "0")
                {
                   
                    result = "yes";
                    subobj.status = string.Empty;
                }
                else
                {
                    result = "No";
                    subobj.status = "yes";
                }
                subobj.SupplierTrigger = IreS.GetTriggerDetails(subobj).ToList();
               // if (subobj.SupplierTrigger.Count > 0)
                if (result == "yes")
                {
                    Session["trigger"] = "yes";
                  
                }
                else
                {
                    Session["message"] = "No";
                }

                ViewBag.status = "grid";
            }
            catch(Exception ex)
            {

            }

         
            return View(subobj);
        }

        [HttpPost]
        public JsonResult ChangeUser(SupplierTriggerRenewal model)
        {
            string message = IreS.UpdateSupplierTrigger(model);
            return Json(message, JsonRequestBehavior.AllowGet);
        }
  
        public PartialViewResult RenewlaTriggerEdit(int id)
        {
            DataTable dtGetDetails = new DataTable();
            SupplierTriggerRenewal obj = new SupplierTriggerRenewal();
            dtGetDetails = IreS.GetTriggerEditDetails(id);
            if (dtGetDetails.Rows.Count>0)
            {
                obj.Slno = dtGetDetails.Rows[0]["renewaltrigger_gid"].ToString();
                obj.Trigger_before = dtGetDetails.Rows[0]["renewaltrigger_triggerbefore"].ToString();
                obj.subject = dtGetDetails.Rows[0]["renewaltrigger_subject"].ToString();
                obj.Message = dtGetDetails.Rows[0]["renewaltrigger_message"].ToString();
              
            }
            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult RenewlaTriggerEdit(SupplierTriggerRenewal obj)
        {
            string Message = string.Empty;
            try
            {
               // Message = IreS.GetTriggercount(obj.Trigger_before.ToString());
                //if(Message !="0")
                //{
                //    Message = "No";
                //}
                //else
                //{
                    Message = IreS.UpdateSupplierTrigger(obj);
                //}
                
                if (Message == "Success")
                {
                    Session["message"] = "yes";
                }
                else
                {
                    Session["message"] = "No";
                }
            }
            catch(Exception ex)
            {

            }
           
            return RedirectToAction("../SupplierTriger/RenewalTrigger");
        }

        public JsonResult RenewalTriggerDelete(SupplierTriggerRenewal objj)
        {

            string DeleteMessage = IreS.DeleteSupplierTrigger(objj);
            //if (DeleteMessage == "Success")
            //{
            //    Session["Deleted"] = "yes";
            //}
           // return RedirectToAction("../SupplierTriger/RenewalTrigger");
            return Json(DeleteMessage, JsonRequestBehavior.AllowGet);
        }



    }
}
