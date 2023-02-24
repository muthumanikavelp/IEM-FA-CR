using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
namespace IEM.Areas.IFAMS.Controllers
{
    public class pvimportfilequeryController : Controller
    {
        //
        // GET: /IFAMS/pvimportfilequery/

        public ActionResult PvImportFileQuery()
        {
            filequery fq = new filequery();
            
            fq.query = Enumerable.Empty<filequery>().ToList<filequery>();
            if (fq.query.Count == 0)
                ViewBag.Message = "No Records Found";
            return View(fq);
        }

    }
}
