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
    public class SCNReleaseForWOController : Controller
    {
        //
        // GET: /FLEXIBUY/SCNReleaseForWO/
          int totalcount;
        ErrorLog objErrorLog = new ErrorLog();
        private Irepositorypr objrep;
        public SCNReleaseForWOController()
            : this(new fb_DataModelpr())
        {

        }
        public SCNReleaseForWOController(Irepositorypr objM)
        {
            objrep = objM;
        }

        [HttpGet]
        public ActionResult SCNRelease()
        {
            List<poRaising> obj = new List<poRaising>();
            try
            {               
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }
        public JsonResult GetWODetails()
        {
            poRaising objlist = new poRaising();
            List<poRaising> obj = new List<poRaising>();
            try
            {
                objlist.groupRes = objrep.GetReqGroup();
                if (objlist.groupRes == "IT" || objlist.groupRes == "PIP")
                {
                    obj = objrep.SCNWORelease(objlist.groupRes);
                }
                //obj = objrep.SCNWORelease().ToList();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
