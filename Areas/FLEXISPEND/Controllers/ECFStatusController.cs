using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class ECFStatusController : Controller
    {

        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        
        #endregion

        //
        // GET: /FLEXISPEND/ECFStatus/ViewData

        public ActionResult ViewECF(string id, string subId)
        {   
            @ViewBag.EcfId = "";
            @ViewBag.Ecfdet = "";

            @ViewBag.EcfId = id;
            //string EditOption = "N";
            string EOWFrom = plib.sessions.Get("EOWFS_Status");
            plib.sessions.Remove("EOWFS_Status");
            string EditOption = "Y~" + EOWFrom;
            @ViewBag.Ecfdet = string.Format("{0}-{1}-{2}", id, subId, EditOption);

            return View();
        }

    }
}
