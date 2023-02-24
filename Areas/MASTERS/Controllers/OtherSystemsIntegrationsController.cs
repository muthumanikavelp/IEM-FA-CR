using IEM.Areas.EOW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using IEM.Common;
using System.Web.Script.Serialization;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;
  

namespace IEM.Areas.MASTERS.Controllers
{

    
    [NoDirectAccess]
    public class OtherSystemsIntegrationsController : Controller
    {
        //
        // GET: /MASTERS/OtherSystemsIntegrations/
        ErrorLog objErrorLog = new ErrorLog();
        BranchModel branch = new BranchModel();
        private othersystemintegrationRepository objModel;
        public OtherSystemsIntegrationsController()
            : this(new OtherSystemModel())
        {

        }
         public OtherSystemsIntegrationsController(othersystemintegrationRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
         {
            return View();
        }
        public PartialViewResult FC()
        {

            return PartialView();
        }
        public PartialViewResult CC()
        {

            return PartialView();
        }
        public PartialViewResult Product()
        {

            return PartialView();
        }
        public PartialViewResult Gl()
        {

            return PartialView();
        }
        public PartialViewResult Branch()
        {

            return PartialView();
        }
        public PartialViewResult Employee()
        {

            return PartialView();
        }
       
    }
}
