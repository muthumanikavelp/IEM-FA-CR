using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{

     [NoDirectAccess]
    public class IEM_FINYEARController : Controller
    {
        private Iiem_mst_finyear Finanyear;
        CmnFunctions com = new CmnFunctions();

        public IEM_FINYEARController() :
            this(new IEM_MST_FINYEAR()) { }

        public IEM_FINYEARController(Iiem_mst_finyear Objist)
        {
            Finanyear = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_finayear> records = new List<iem_mst_finayear>();
            records = Finanyear.GetFinyear().ToList();
            return View(records); 
        }
        [HttpPost]
        public ActionResult Index(string financeyearcode = null, string finperiodfrom = null, string finperiodto = null, string command = null)
        {
            List<iem_mst_finayear> records = new List<iem_mst_finayear>();
            if (command == "Search" || command == "Refresh")
            {
                records = Finanyear.GetFinyear(financeyearcode, finperiodfrom, finperiodto).ToList();
                if (records.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                @ViewBag.finaceyearcode = financeyearcode;
                @ViewBag.periodfrom = finperiodfrom;
                @ViewBag.periodto = finperiodto;
            }
            else if (command == "Clear")
            {
                records = Finanyear.GetFinyear().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_finayear TypeModel = new iem_mst_finayear();           
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult  CreateFinanceYear(iem_mst_finayear Inserfina)
        {
            string res = string.Empty;
            res = Finanyear.InsertFinyear(Inserfina);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            else if (viewfor == "delete")
            {
                ViewBag.viewfor = "delete";
            }
            iem_mst_finayear TypeModel = new iem_mst_finayear();
            TypeModel = Finanyear.GetFinyearById(id);            
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateFinanceYear(iem_mst_finayear Updatefinance)
        {
            string res=string.Empty;
            res=Finanyear.UpdateFinyear(Updatefinance);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Deletefinanceyear(iem_mst_finayear deltefina)
        {
            string res = string.Empty;
            res = Finanyear.DeleteFinyear(deltefina.finyear_gid);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}

