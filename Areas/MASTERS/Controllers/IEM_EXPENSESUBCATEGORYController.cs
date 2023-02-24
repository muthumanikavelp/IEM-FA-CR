using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Areas.MASTERS.Models.IEM.Areas.MASTERS.Models;
using IEM.Common;
using IEM.App_Start;


namespace IEM.Areas.MASTERS.Controllers
{
     [NoDirectAccess]
    public class IEM_EXPENSESUBCATEGORYController : Controller
    {
        //
        // GET: /IEM_EXPENSESUBCATEGORY/
        private Iiem_mst_expensesubcategory expsubcat;
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions com = new CmnFunctions();
        string result;
        public IEM_EXPENSESUBCATEGORYController() :
            this(new IEM_MST_EXPENSESUBCATEGORY()) { }

        public IEM_EXPENSESUBCATEGORYController(Iiem_mst_expensesubcategory Objist)
        {
            expsubcat = Objist;
        }
        public ActionResult Index()
        {
            iem_mst_expensesubcategory TypeModel = new iem_mst_expensesubcategory();
            TypeModel.Getexpnature = new SelectList(expsubcat.Getexpnature(), "expnature_gid", "expnature_name");
            TypeModel.Getexpcat = new SelectList(expsubcat.Getexpcat(), "expcat_gid", "expcat_name");
            TypeModel.GetHsncode = new SelectList(expsubcat.GetHsncode(), "Hsn_gid", "hsn_code");
            ViewBag.expensecatsubcat = TypeModel;

            List<iem_mst_expensesubcategory> records = new List<iem_mst_expensesubcategory>();
            records = expsubcat.getexpsubcategory().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string filter1 = null, string expnatTypee = null, string expcatType = null, string command = null)
        {
            iem_mst_expensesubcategory TypeModel = new iem_mst_expensesubcategory();
            TypeModel.Getexpnature = new SelectList(expsubcat.Getexpnature(), "expnature_gid", "expnature_name");
            TypeModel.Getexpcat = new SelectList(expsubcat.GetexpcatBy_Id(Convert.ToInt32(expnatTypee)), "expcat_gid", "expcat_name");
            ViewBag.expensecatsubcat = TypeModel;

            List<iem_mst_expensesubcategory> records = new List<iem_mst_expensesubcategory>();
            if (command == "Search" || command == "Refresh")
            {
                records = expsubcat.getexpsubcategory1(filter, filter1, expnatTypee, expcatType).ToList();
                @ViewBag.filter = filter;
                @ViewBag.filter1 = filter1;
                @ViewBag.filter2 = expnatTypee;
                if (expcatType != null)
                {
                    @ViewBag.filter3 = expcatType;
                }


                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if (command == "Clear")
            {
                records = expsubcat.getexpsubcategory().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {

            iem_mst_expensesubcategory TypeModel = new iem_mst_expensesubcategory();
            TypeModel.Getexpnature = new SelectList(expsubcat.Getexpnature(), "expnature_gid", "expnature_name");
            TypeModel.Getexpcat = new SelectList(expsubcat.Getexpcat(), "expcat_gid", "expcat_name");
            TypeModel.GetHsncode = new SelectList(expsubcat.GetHsncode(), "Hsn_gid", "hsn_code");
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult CreateExpSubcat(iem_mst_expensesubcategory ExpCatModel, string hsn)
        {
            try
            {
                if (hsn != null)
                {
                    hsn = hsn.Replace("\"", "");
                    hsn = hsn.Replace("[", "");
                    hsn = hsn.Replace("]", "");
                }

                ExpCatModel.expsubcat_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                string check = expsubcat.Insertexpsubcategory(ExpCatModel, hsn);
                    if (check == null)
                    {
                        RedirectToAction("Index");
                    }
                    if (check == "success")
                    {
                        result = "success";
                    }
                    if (check != "success")
                    {
                        result = "Fail";
                    }
                    if (check == "Already Exist")
                    {
                        result = "Duplicate";
                    }

                return Json(result, JsonRequestBehavior.AllowGet);
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
            else if (viewfor == "delete")
            {
                ViewBag.viewfor = "delete";
            }

            iem_mst_expensesubcategory TypeModel = expsubcat.GetexpsubcategoryById(id);
            TypeModel.Getexpnature = new SelectList(expsubcat.Getexpnature(), "expnature_gid", "expnature_name", TypeModel.expnature_gid);
            TypeModel.Getexpcat = new SelectList(expsubcat.Getexpcat(), "expcat_gid", "expcat_name", TypeModel.expcat_gid);
            TypeModel.GetHsncode = new SelectList(expsubcat.GetHsncode(), "Hsn_gid", "hsn_code", TypeModel.Hsn_gid);
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> AllHSN = new List<SelectListItem>();
            AllHSN = expsubcat.GetAllHsn();
            items = expsubcat.GetHsnforexpense(id.ToString());

            int[] SelectedHSNIDs = new int[100];

            for (int i = 0; i < items.Count; i++)
            {
                SelectedHSNIDs[i] = Convert.ToInt16(items[i].Value);
            }
            if (SelectedHSNIDs.Length == 0)
            {
                SelectedHSNIDs[0] = 0;
            }
            MultiSelectList teamsList = new MultiSelectList(AllHSN.OrderBy(i => i.Text), "Value", "Text", SelectedHSNIDs);
            TypeModel.Items = teamsList;
            TypeModel.SelectedItemIds = SelectedHSNIDs;
            TypeModel.hsn_desc = expsubcat.GethsndescEXPENSE(id.ToString());
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult expcat(iem_mst_expensesubcategory expensen)
        {
            try
            {

                int ExpCat_gid = expensen.expsubcat_gid;
                return Json(expsubcat.GetexpcatBy_Id(ExpCat_gid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EditExpsubCat(iem_mst_expensesubcategory ExpCatModel, string hsn)
        {
            try
            {
                if (hsn != null)
                {
                    hsn = hsn.Replace("\"", "");
                    hsn = hsn.Replace("[", "");
                    hsn = hsn.Replace("]", "");
                }
                ExpCatModel.expsubcat_update_by = int.Parse(com.GetLoginUserGid().ToString());
                string check = expsubcat.Updatexpsubcategory(ExpCatModel, hsn);
                if (check == null)
                {
                    RedirectToAction("Index");
                }
                if (check != "success")
                {
                    result = "Fail";
                }
                if (check == "success")
                {
                    result = "success";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeletExpsubcat(iem_mst_expensecategory ExpCat)
        {
            try
            {
                result = expsubcat.Deletexpsubcategory(ExpCat.expcat_gid);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        // GET: /IEM_EXPENSESUBCATEGORY/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /IEM_EXPENSESUBCATEGORY/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /IEM_EXPENSESUBCATEGORY/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IEM_EXPENSESUBCATEGORY/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /IEM_EXPENSESUBCATEGORY/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IEM_EXPENSESUBCATEGORY/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /IEM_EXPENSESUBCATEGORY/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult Gethsndesc(string hsngid)
        {
            string cobj = string.Empty;
            try
            {
                if (hsngid != null)
                {

                    hsngid = hsngid.Replace("\"", "");
                    hsngid = hsngid.Replace("[", "");
                    hsngid = hsngid.Replace("]", "");
                    cobj = expsubcat.Gethsndesc(hsngid);


                }
                return Json(cobj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

    }
}
