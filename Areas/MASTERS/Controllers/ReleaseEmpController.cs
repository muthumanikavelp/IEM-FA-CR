using IEM.Common;
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
    public class ReleaseEmpController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private EmployeeReleaseRepository objModel;

        public ReleaseEmpController()
            : this(new EmpReleaseDataModel())
        {

        }
        public ReleaseEmpController(EmployeeReleaseRepository objM)
        {
            objModel = objM;
        }
        //
        // GET: /MASTERS/ReleaseEmp/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult SearchEmpRelease(string listfor = null) 
        {
            try
            {
                List<EmployeeRelease> lst = new List<EmployeeRelease>(); 
                //if (listfor == "search")
                //{
                //    if (Session["SearchReleaseEmployee"] != null)
                //    {
                        lst = (List<EmployeeRelease>)Session["SearchReleaseEmployee"];
                //    }
                //}
               
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpGet]
        //public PartialViewResult SearchEmpReleaseFields()  
        //{
        //    try
        //    {
        //        EmployeeRelease objEmpRel = new EmployeeRelease();
        //        return PartialView(objEmpRel); 
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpPost]
        public JsonResult UpdateEmpRelease(EmployeeRelease objEmprelease) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateEmployeeDetails(objEmprelease); 
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
               // throw ex;
                return Json(0, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }
        //[HttpPost]
        //public JsonResult FilterEmpRelease(EmployeeRelease objEmprelease) 
        //{
        //    try
        //    {
        //        var res = 0; ;
        //        List<EmployeeRelease> objEmp = new List<EmployeeRelease>();
        //        objEmp = objModel.GetEmployeeDetails(objCmnFunctions.GetLoginUserGid()).ToList();
               
        //        if (objEmp != null)
        //        {
        //            if ((string.IsNullOrEmpty(objEmprelease._EmployeeCode)) == false)
        //            {
        //                objEmp = objEmp.Where(x => objEmprelease._EmployeeCode == null ||
        //                    (x._EmployeeCode.ToUpper().Contains(objEmprelease._EmployeeCode.ToUpper()))).ToList();
        //                Session["SearchReleaseEmployee"] = objEmp;
        //            }
        //            if ((string.IsNullOrEmpty(objEmprelease._EmployeeName)) == false)
        //            {
        //                objEmp = objEmp.Where(x => objEmprelease._EmployeeName == null ||
        //                    (x._EmployeeName.ToUpper().Contains(objEmprelease._EmployeeName.ToUpper()))).ToList();
        //                Session["SearchReleaseEmployee"] = objEmp;
        //            }
        //        }
        //        if (objEmp.Count() == 0)
        //        {
        //            res = 0;
        //        }
        //        else
        //        {
        //            res = 1;
        //        }
        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
