using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class IEM_MST_ADVANCEController : Controller
    {
        CmnFunctions com = new CmnFunctions();
        private Iiem_mst_advancetype advancetype;
        //
        // GET: /MASTERS/IEM_MST_ADVANCE/

         public IEM_MST_ADVANCEController() :
            this(new IEM_MST_ADVANCETYPE()) { }

         public IEM_MST_ADVANCEController(Iiem_mst_advancetype Objist) 
        {
            advancetype = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_advancetype> selectadvacetypegrid = new List<iem_mst_advancetype>();
            selectadvacetypegrid = advancetype.getadvancetypeGrid().ToList();
            if (selectadvacetypegrid.Count == 0)
            {
                ViewBag.Message = "No Record's found";
            }
            return View(selectadvacetypegrid);
           
        }
        [HttpPost]
        public ActionResult Index(string advancetypename, string masglno, string command = null)
        {
            List<iem_mst_advancetype> selectadvacetypegrid = new List<iem_mst_advancetype>();
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                selectadvacetypegrid = advancetype.getadvancetype(advancetypename, masglno).ToList();
                @ViewBag.advancetypename = advancetypename;
                @ViewBag.masglno = masglno;
                if (selectadvacetypegrid.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (command == "Clear")
            {
                selectadvacetypegrid = advancetype.getadvancetypeGrid().ToList();
            }
            return View(selectadvacetypegrid);

        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_advancetype typemodel = new iem_mst_advancetype();
            typemodel.GetGL = new SelectList(advancetype.GetGL(), "gl_gid", "gl_no");
            return PartialView(typemodel);
        }
        [HttpPost]
        public JsonResult create(iem_mst_advancetype insertinfor)
        {
            string res=string.Empty;
            DataTable getglno = new DataTable();
            getglno = advancetype.getglno(int.Parse(insertinfor.advancetype_gl_no));
            if(getglno.Rows.Count>0)
            {
                insertinfor.advancetype_gl_no = getglno.Rows[0]["gl_no"].ToString();
            }
            res = advancetype.InsertAdvanceType(insertinfor);
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            iem_mst_advancetype TypeModel = new iem_mst_advancetype();
            DataTable getglno=new DataTable ();
            DataTable glgid=new DataTable ();
            getglno = advancetype.getglnoByadvanctyid(id);
            if(getglno.Rows.Count>0)
            {
                TypeModel.advancetype_gl_no = getglno.Rows[0]["advancetype_gl_no"].ToString();
            }
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
            TypeModel = advancetype.GetadvancetypeById(id);
            ViewBag.advancesubtypename = TypeModel.advancesubtype_name;
            glgid = advancetype.getglgid(int.Parse(TypeModel.advancetype_gl_no));
            TypeModel.gl_gid = int.Parse(glgid.Rows[0]["gl_gid"].ToString());
            TypeModel.GetGL = new SelectList(advancetype.GetGL(), "gl_gid", "gl_no", TypeModel.gl_gid);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateAdvanceType(iem_mst_advancetype updateadvancetype)
        {
            string res = string.Empty;
            DataTable getglno = new DataTable();
            getglno = advancetype.getglno(int.Parse(updateadvancetype.advancetype_gl_no));
            if (getglno.Rows.Count > 0)
            {
                updateadvancetype.advancetype_gl_no = getglno.Rows[0]["gl_no"].ToString();
            }
            res=advancetype.UpdateAdvanceType(updateadvancetype);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteAdvanceType(iem_mst_advancetype deleteadvancetype)
        {
            string res = string.Empty;
            res = advancetype.DeleteAdvanceType(deleteadvancetype.advancetype_gid);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /MASTERS/IEM_MST_ADVANCE/Details/5

      
    }
}
