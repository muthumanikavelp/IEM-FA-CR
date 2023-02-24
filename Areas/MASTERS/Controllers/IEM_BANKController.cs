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
    public class IEM_BANKController : Controller
    {
        private Iiem_mst_bank bank;
        CmnFunctions com = new CmnFunctions();
       
        public IEM_BANKController() :
            this(new IEM_MST_BANK()) { }

        public IEM_BANKController(Iiem_mst_bank Objist) 
        {
            bank = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_bank> g = new List<iem_mst_bank>();
            g = bank.getbank().ToList();
            return View(g);
        }

        [HttpPost]
        public ActionResult Index(string filter_code = "",string filter_name = "",string command=null)
        {

            List<iem_mst_bank> records = new List<iem_mst_bank>();
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
            records = bank.getbank(filter_code,filter_name).ToList();
            Session["records"] = records;
            @ViewBag.filter_code = filter_code;
            @ViewBag.filter_name = filter_name;
            if (records.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
            }
            if(command=="Clear")
            {
                 records = bank.getbank().ToList();
            }
            return View(records);
        }
        //
        // GET: /IEM_BANK/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /IEM_BANK/Create

        public ActionResult Create(string empid)
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_bank TypeModel = new iem_mst_bank();
            return PartialView(TypeModel);
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
            iem_mst_bank TypeModel = bank.GetBankById(id);
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult Delete(int id, string viewfor)
        {           
            if (viewfor == "delete")
            {
                ViewBag.viewfor = "delete";
            }
            iem_mst_bank TypeModel = bank.GetBankById(id);
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult ViewBank(int id, string viewfor)
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
            iem_mst_bank TypeModel = bank.GetBankById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateBankDetails(iem_mst_bank TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {

                    TypeModel.bank_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = bank.UpdateBank(TypeModel);

                    if (result == "success") RedirectToAction("Index");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public PartialViewResult Delete(int id)
        //{
        //    try
        //    {               
        //        iem_mst_bank TypeModel = bank.GetBankById(id);
        //        return PartialView(TypeModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        [HttpPost]
        public JsonResult DeleteBank(iem_mst_bank TypeModel)
        {
            string lsresult = string.Empty;
            try
            {
                var ids = TypeModel.bank_gid;
                lsresult = bank.DeleteBank(ids);
                ViewBag.viewfor = "delete";
                return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        [HttpPost]
        public JsonResult Create(iem_mst_bank TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.bank_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = bank.InsertBank(TypeModel);

                    if (result == "success") RedirectToAction("Index");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
    }
}
