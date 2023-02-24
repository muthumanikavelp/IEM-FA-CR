using IEM.Common;
using IEM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using IEM.App_Start;

namespace IEM.Controllers
{

    [NoDirectAccess]
    public class IEMCommonController : Controller
    {
        
        private IEMRepository objModel;
        ErrorLog objErrorLog = new ErrorLog();
        public IEMCommonController()
            :this(new IEMDataModel()) 
        {

         }
        public IEMCommonController(IEMRepository objM)
        {
            objModel = objM; 
        }
        //
        // GET: /IEMCommon/

        public ActionResult Welcome() 
        {
            try
            {
                Session["Menu"] = objModel.GetMenu();
                //  Session["employee_gid"] = 2;
               // return RedirectToAction("Index", "Dashboard", new { area = "EOW"}); 
                //return Redirect("~/EOW/Dashboard/Index");
                return Redirect("~/Dashboard/Dashboard/Index");
              
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
       
        }
        public ActionResult Policy() 
        {
            List<PolicyDataModel> Policy = new List<PolicyDataModel>();
            try
            {
                Policy = objModel.GetPolicyFiles().ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(Policy);
        }

        public FileResult DownloadPolicy(int policyid)
        {            
            List<PolicyDataModel> Policy = new List<PolicyDataModel>();
            Policy = objModel.GetPolicyFilesView(policyid).ToList();
            
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Download"));
            byte[] fileBytes = System.IO.File.ReadAllBytes(Policy[0].DownloadPath);
            string fileName = Policy[0].FileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
