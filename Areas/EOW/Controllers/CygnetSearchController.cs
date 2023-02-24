using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using System.Data;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.EOW.Models;
namespace IEM.Areas.EOW.Controllers
{
    public class CygnetSearchController : Controller
    {
        //
        // GET: /CygnetSearch/
        private CygnetSearch_IRepository objModel;
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        public CygnetSearchController()
            : this(new CygnetDataModel())
        {
           
        }
        public CygnetSearchController(CygnetSearch_IRepository objM)
        {
            objModel = objM;
        }

        public ActionResult Index()
        {
            return PartialView(objModel);
        }
        public ActionResult CygnetSearch()
        {
            return PartialView(objModel);
        }

        [HttpPost]
        public JsonResult InvoiceSearch(CygnetSearchModel data)
        {
            List<CygnetSearchModel> obj = new List<CygnetSearchModel>();
            CygnetSearchModel invsearch = new CygnetSearchModel();
            obj = objModel.SelectInvoiceSearch(data).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CreateECFInvoice(string Cygnet_Gids)
        {
            try
            {
                string Data0 = "";
                DataSet ds = new DataSet();
                //ds = db.CreateECFInvoice(Cygnet_Gids, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }
                    //if (ds.Tables[1].Rows.Count > 0)
                    //    Session["LeaseError"] = ds.Tables[1];

                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
    }
}
