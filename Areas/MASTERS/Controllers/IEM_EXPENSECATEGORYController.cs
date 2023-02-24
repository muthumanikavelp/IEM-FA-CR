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
    public class IEM_EXPENSECATEGORYController : Controller
    {
        //
        // GET: /IEM_EXPENSECATEGORY/
        private Iiem_mst_expensecategory expcat;
        CmnFunctions com = new CmnFunctions();
        List<string> list = new List<string>();
        public IEM_EXPENSECATEGORYController() :
            this(new IEM_MST_EXPENSECATEGORY()) { }

        public IEM_EXPENSECATEGORYController(Iiem_mst_expensecategory Objist)
        {
            expcat = Objist;
        }
        public ActionResult Index()
        {
            iem_mst_expensesubcategory TypeModel = new iem_mst_expensesubcategory();
            TypeModel.Getexpnature = new SelectList(expcat.Getexpnature(), "expnature_gid", "expnature_name");
            ViewBag.expensecatsubcat = TypeModel;

            List<iem_mst_expensecategory> records = new List<iem_mst_expensecategory>();
            records = expcat.getexpcategory().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string expcat_code = null, string expcat_name = null, string expnatTypee = null, string command = null)
        {
            iem_mst_expensesubcategory TypeModel = new iem_mst_expensesubcategory();
            TypeModel.Getexpnature = new SelectList(expcat.Getexpnature(), "expnature_gid", "expnature_name");
            ViewBag.expensecatsubcat = TypeModel;

            List<iem_mst_expensecategory> records = new List<iem_mst_expensecategory>();
            if (command == "Search" || command == "Refresh")
            {
                records = expcat.getexpcategory(expcat_code, expcat_name, expnatTypee).ToList();

                @ViewBag.expcat_code = expcat_code;
                @ViewBag.expcat_name = expcat_name;
                @ViewBag.filter2 = expnatTypee;
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if (command == "Clear")
            {
                records = expcat.getexpcategory().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            //list.Add("121020001");
            //list.Add("121020004");
            //List<string> glno = new List<string>();
            //{
            //    glno = list;
            //};
            //List<SelectListItem> gl = new List<SelectListItem>();
            //foreach (var item in glno)
            //{

            //    gl.Add(new SelectListItem
            //    {
            //        Text = item.ToString(),
            //        //Value = item.ToString()
            //    });
            //}

            //ViewBag.ClearenceGlNo = gl;

            iem_mst_expensecategory TypeModel = new iem_mst_expensecategory();
            TypeModel.Getexpnature = new SelectList(expcat.Getexpnature(), "expnature_gid", "expnature_name");
            TypeModel.GetGL = new SelectList(expcat.GetGL(), "gl_gid", "gl_no");
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult CreateExpNat(iem_mst_expensecategory ExpCat)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    //GetGLById getgl = expcat.GetGLById(ExpCat.gl_gid);
                    //ExpCat.gl_no = getgl.gl_no;
                    ExpCat.gl_no = ExpCat.gl_gid.ToString();
                    ExpCat.expcat_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = expcat.Insertexpcategory(ExpCat);

                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
            

            iem_mst_expensecategory TypeModel = expcat.GetexpcategoryById(id);
            TypeModel.Getexpnature = new SelectList(expcat.Getexpnature(), "expnature_gid", "expnature_name", TypeModel.selectedexpnature_gid);
            TypeModel.GetGL = new SelectList(expcat.GetGL(), "gl_no", "gl_no", TypeModel.gl_no);
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult EditExpCat(iem_mst_expensecategory ExpCat)
        {
            string result = string.Empty;
            try
            {

                //GetGLById getgl = expcat.GetGLById(ExpCat.selectedgl_no);
                ExpCat.gl_no = Convert.ToString(ExpCat.selectedgl_no);
                result = expcat.Updatexpcategory(ExpCat);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletExpcat(iem_mst_expensecategory ExpCat)
        {
            string result = string.Empty;
            try
            {
                result = expcat.Deletexpcategory(ExpCat.expcat_gid);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult GLSearch(string listfor)
        {
            try
            {
                List<iem_mst_expensecategory> obj = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory Getvaluesearchvalue = new iem_mst_expensecategory();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (iem_mst_expensecategory)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.expcat_name;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.expcat_code;
                        obj = (List<iem_mst_expensecategory>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = expcat.SelectGLCode().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GLSearchWithParam(string GLName = "", string GLCode = "")
        {
            List<iem_mst_expensecategory> recordsearch = new List<iem_mst_expensecategory>();
            iem_mst_expensecategory emp = new iem_mst_expensecategory();
            if (GLName != "" || GLCode != "")
            {
                @ViewBag.EmployeeName = GLName;
                @ViewBag.EmployeeCode = GLCode;
                emp.expcat_code = GLCode;
                emp.expcat_name = GLName;
                Session["SerchViewbag"] = emp;
                recordsearch = expcat.SelectGLSearch(GLName, GLCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = expcat.SelectGLSearch(GLName, GLCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }

    }
}
