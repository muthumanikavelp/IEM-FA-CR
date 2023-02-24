using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using IEM.Areas.FLEXISPEND.Models;

namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class SectorController : Controller
    {

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        FSRepository _fsRep = new FSRepository();
        Message msg = new Message();
        DataTable dt = new DataTable();
        #endregion

        [HttpGet]

        public ActionResult Index()
        {

            sectorreport sreport = new sectorreport();
            List<sectorreport> sreports = new List<sectorreport>();

            sreport.report = db.Getsector();

            return View();


        }


        //[HttpPost]

        //public JsonResult Fetchsectorreport()
        //{

        //    //DataSet ds = new DataSet();

        //    try
        //    {
        //        string data1 = "", data2 = "";
        //        DataSet ds = db.Getsector();
        //        if (ds != null)
        //        {
        //            dt = ds.Tables[0];
        //            if (dt.Rows.Count > 0)
        //            {
        //                data1 = JsonConvert.SerializeObject(dt);
        //            }
        //            //dt = ds.Tables[1];
        //            //if (dt.Rows.Count > 0)
        //            //{
        //            //    data2 = JsonConvert.SerializeObject(dt);
        //            //}
        //            return Json(new { data1 }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return null;
        //        }


        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

    }
}
