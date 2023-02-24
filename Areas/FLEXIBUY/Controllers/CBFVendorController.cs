using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class CBFVendorController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        public CBFVendorController()
            : this(new CbfSumModel())        
        {

        }
        public CBFVendorController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        public ActionResult CBFvendorsummary()
        {
            CbfSumEntity objsumentity = new CbfSumEntity();
            objsumentity = objRep.GetCBFSummaryHeader();
            objsumentity.result = objRep.GetGroupForUser();           
            return View(objsumentity);
        }

    }
}
