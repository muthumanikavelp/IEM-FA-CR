using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
namespace IEM.Areas.IFAMS.Controllers
{
    public class physicalverificationreportController : Controller
    {
        //
        // GET: /IFAMS/physicalverificationreport/
        ErrorLog objErrorLog = new ErrorLog();
        public ActionResult PhysicalverificationReport()
        {
            report rep = new report();
            try
            {                
                rep.rep = Enumerable.Empty<report>().ToList<report>();
                if (rep.rep.Count == 0)
                    ViewBag.Message = "No Records Found";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View(rep);
        }
    }
}
