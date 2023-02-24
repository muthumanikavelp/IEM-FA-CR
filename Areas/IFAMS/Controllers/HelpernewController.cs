using IEM.Areas.IFAMS.Models;
using IEM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace IEM.Areas.IFAMS.Controllers
{
    public class HelpernewController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        AssetRepository _fsRep = new AssetRepository();
        #endregion
        public JsonResult GetAutoCompleteVendor(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompletevendor(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

    }
}
