using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{

    [NoDirectAccess]
    public class IEM_TAX_RATEController : Controller
    {
        //
        // GET: /MASTERS/IEM_TAX_RATE/

        private Iiem_mst_Tax_rate Taxrate;
        public IEM_TAX_RATEController() :
            this(new IEM_MST_TAX_RATE()) { }

        public IEM_TAX_RATEController(Iiem_mst_Tax_rate taxsubobj) 
        {
            Taxrate = taxsubobj;
        }
        public ActionResult Index()
        {
            List<iem_mst_tax_rate> taxraterecords = new List<iem_mst_tax_rate>();
            taxraterecords = Taxrate.gettaxrate().ToList();
            if (taxraterecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(taxraterecords);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null, string filter_name1 = null)
        {
            List<iem_mst_tax_rate> objowner = new List<iem_mst_tax_rate>();
            objowner = Taxrate.gettaxrate().ToList();

            if ((string.IsNullOrEmpty(filter_name)) == false)
                {
                    ViewBag.filter_name = filter_name;
                    objowner = objowner.Where(x => filter_name == null ||
                        (x.taxrate_tax_gid.ToUpper().Contains(filter_name.ToUpper()))).ToList();
                }
            if ((string.IsNullOrEmpty(filter_name1)) == false)
                {
                    ViewBag.filter_name1 = filter_name1;
                    objowner = objowner.Where(x => filter_name1 == null ||
                        (x.taxrate_taxsubtype_gid.ToUpper().Contains(filter_name1.ToUpper()))).ToList();
                }
            
            if (objowner.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(objowner);
            //List<iem_mst_tax_rate> taxsubrecords = new List<iem_mst_tax_rate>();
            //taxsubrecords = Taxrate.gettaxrateid(filter_name, filter_name1).ToList();
            //@ViewBag.filter_name = filter_name;
            //@ViewBag.filter_name1 = filter_name1;
            //if (taxsubrecords.Count == 0)
            //{
            //    ViewBag.records = "No Records Found";
            //}
            //return View(taxsubrecords);
        }
        [HttpGet]
        public PartialViewResult Create()
        {

            iem_mst_tax_rate TypeModel = new iem_mst_tax_rate();
            TypeModel.Gettaxratetaxname = new SelectList(Taxrate.Gettaxname(), "tax_gid", "tax_name");
            TypeModel.Gettaxratetaxsubname = new SelectList(Taxrate.Gettaxsubname(), "taxsub_gid", "taxsub_name");
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult Taxratesub(iem_mst_tax_rate expensentax)
        {
            try
            {

                int ExpCat_gid = Convert.ToInt32(expensentax.taxrate_tax_gid);
                return Json(Taxrate.Gettaxsubname_id(ExpCat_gid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Createtaxrate(iem_mst_tax_rate taxModel1)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    taxModel1.taxrate_insert_by = int.Parse(Session["employee_gid"].ToString());
                   result = Taxrate.Inserttaxrate(taxModel1);
                    if (result == "success") RedirectToAction("Index");

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Delettaxrate(iem_mst_tax_rate taxModel)
        {
            try
            {
                Taxrate.Deletetaxrate(taxModel.taxrate_gid);
                return Json(taxModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            else if (viewfor == "Delete")
            {
                ViewBag.viewfor = "Delete";
            }
            iem_mst_tax_rate TypeModel = Taxrate.gettaxratebyid(id);
            TypeModel.Gettaxratetaxname = new SelectList(Taxrate.Gettaxname(), "tax_gid", "tax_name", TypeModel.taxrate_tax_gid);
            TypeModel.Gettaxratetaxsubname = new SelectList(Taxrate.Gettaxsubname(), "taxsub_gid", "taxsub_name", TypeModel.taxrate_taxsubtype_gid);
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult edittaxrate(iem_mst_tax_rate taxCat)
        {
            string result = string.Empty;
            //try
            //{
            //    taxCat.taxrate_update_by = int.Parse(Session["employee_gid"].ToString());
            //    Taxrate.Updattaxrate(taxCat);

            //    return Json(taxCat, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                if (ModelState.IsValid)
                {
                    result = Taxrate.Updattaxrate(taxCat);

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
