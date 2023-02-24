using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Areas.EOW.Models;
using IEM.Common;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class IEM_TAXSUBTYPEController : Controller
    {
        //
        // GET: /MASTERS/IEM_TAXSUBTYPE/
        private EOW_DataModel objModelTravel;
        private Iiem_mst_Tax_subtype Taxsubtype;
        public IEM_TAXSUBTYPEController() :
            this(new IEM_MST_TAXSUBTYPE()) { }

        public IEM_TAXSUBTYPEController(Iiem_mst_Tax_subtype taxsubobj)
        {
            Taxsubtype = taxsubobj;
        }
        public ActionResult Index()
        {
            List<iem_mst_tax_subtype> taxsubrecords = new List<iem_mst_tax_subtype>();
            taxsubrecords = Taxsubtype.gettaxsubtype().ToList();
            if (taxsubrecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(taxsubrecords);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null, string filter_name1 = null)
        {
            List<iem_mst_tax_subtype> taxsubrecords = new List<iem_mst_tax_subtype>();
            taxsubrecords = Taxsubtype.gettaxsubtypeid(filter_name, filter_name1).ToList();
            @ViewBag.filter_name = filter_name;
            @ViewBag.filter_name1 = filter_name1;
            if (taxsubrecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(taxsubrecords);
        }
        [HttpGet]
        public PartialViewResult Create()
        {

            iem_mst_tax_subtype TypeModel = new iem_mst_tax_subtype();
            TypeModel.ExpNatureofExpdata = new SelectList(Taxsubtype.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName");
            TypeModel.Gettaxsub = new SelectList(Taxsubtype.Gettaxsubvl(), "tax_gid", "tax_name");
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult GlNumber(string GlNumber)
        {
            List<iem_mst_tax_subtype> glnumber = new List<iem_mst_tax_subtype>();
            glnumber = Taxsubtype.GetGlNumber(GlNumber).ToList();
            return Json(glnumber);
        }
        [HttpPost]
        public JsonResult GetExpenseCategory(iem_mst_tax_subtype EmployeeeExpense)
        {
            return Json(Taxsubtype.ExpenseCategorydata(EmployeeeExpense.NatureofExpensesId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubCategory(iem_mst_tax_subtype EmployeeeExpense)
        {
            return Json(Taxsubtype.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Createtaxsub(iem_mst_tax_subtype taxModel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    taxModel.taxsubtype_insert_by = int.Parse(Session["employee_gid"].ToString());
                    result = Taxsubtype.Inserttaxsub(taxModel);
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
        public JsonResult Delettaxsub(iem_mst_tax_subtype taxModel)
        {
            string result = string.Empty;
            try
            {
                var ids = taxModel.taxsubtype_gid;
                result = Taxsubtype.Deletetaxsub(ids);

                return Json(result, JsonRequestBehavior.AllowGet);
                //Taxsubtype.Deletetaxsub(taxModel.taxsubtype_gid);
                // return Json(taxModel, JsonRequestBehavior.AllowGet);

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

            iem_mst_tax_subtype TypeModel = Taxsubtype.GettaxsubById(id);
            TypeModel.ExpNatureofExpdata = new SelectList(Taxsubtype.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName", TypeModel.NatureofExpensesId.ToString());
            TypeModel.ExpCatdata = new SelectList(Taxsubtype.ExpenseCategorydata(TypeModel.NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", TypeModel.ExpenseCategoryId.ToString());
            TypeModel.ExpSubCatdata = new SelectList(Taxsubtype.SubCategorydata(TypeModel.ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", TypeModel.SubCategoryId.ToString());
            TypeModel.Gettaxsub = new SelectList(Taxsubtype.Gettaxsubvl(), "tax_gid", "tax_name", TypeModel.taxsubtype_parent_name);
            ViewBag.TaxSubTypeHeader = TypeModel;
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult EditTAX(iem_mst_tax_subtype taxCat)
        {
            string result = string.Empty;
            //try
            //{
            //    taxCat.taxsubtype_update_by = int.Parse(Session["employee_gid"].ToString());
            //   result= Taxsubtype.Updattaxsub(taxCat);

            //   return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                if (ModelState.IsValid)
                {

                    result = Taxsubtype.Updattaxsub(taxCat);
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
