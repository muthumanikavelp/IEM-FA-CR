using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.EOW.Models;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class SlabMasterController : Controller
    {
        string Result;
        SupClassificationModel sub=new SupClassificationModel();
        private IRepository objModel;
        DataTable dt = new DataTable();
        DataTable table;
        Int32 To;
        public SlabMasterController()
            : this(new SupDataModel())
        {

        }
        public SlabMasterController(IRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Slab()
        {
            Session["slabname"] = null;
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            obj = objModel.SelectSlabname().ToList();
            if (obj.Count > 0)
            {
                dt.Columns.Add("SlabRange", typeof(Int32));
                dt.Columns.Add("From", typeof(int));
                dt.Columns.Add("To", typeof(int));
                dt.Columns.Add("SlabName", typeof(string));
                Session["Datatable"] = dt;
                for (int i = 0; i < obj.Count; i++)
                {
                    dt.Rows.Add(sub.Slabrange_Name, sub.Slabrange_From, sub.Slabrange_To, sub.Slabname);
                }
                Session["Datatable"] = dt;
            }
            if (obj.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
        
            return View(obj);
        }
        [HttpPost]
        public ActionResult Slab(string filter = null)
        {
            List<SupClassificationModel> records = new List<SupClassificationModel>();
            records = objModel.SelectSlabname().ToList();

            if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
            {
                ViewBag.filter = filter.ToString();
              
                records = records.Where(x => filter == null ||
                    (x.Slabname.ToUpper().Contains(filter.ToUpper()))).ToList();
            }
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);

          
        }
        [HttpPost]
        public ActionResult NewSlab(string newslab)
        {
            
            return View();
        }
       
        //[HttpPost]
        //public JsonResult Submit(SupClassificationModel sub)
        //{
        //    String result = String.Empty;
        //    if (Session["Datatable"] != null)
        //    {
        //        dt = (DataTable)Session["Datatable"];
        //    }
        //    Result = objModel.DeleteSlab(sub);
        //    Result = objModel.AddSlab(sub);
        //    if (Result == "EXSIST")
        //    {
        //        result = "EXSIST";
        //    }
        //    else
        //    {

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            sub.Slabrange_Name = row["SlabRange"].ToString();
        //            sub.Slabrange_From = row["From"].ToString();
        //            sub.Slabrange_To = row["To"].ToString();
        //            Result = objModel.AddSlabRange(sub);
        //        }
        //        if (Result == "sub")
        //        {
        //            result = "sub";
        //        }
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult NewSlabCreationUpdate(SupClassificationModel sub)
        {
            String result = String.Empty;
            if (Session["Datatable"] != null)
            {
                dt = (DataTable)Session["Datatable"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        To = Convert.ToInt32(row["To"]);
                    }
                }
               
            }
            
            //if (To < Convert.ToInt32(sub.Slabrange_From))
            //{
                foreach (DataRow row in dt.Rows)
                {
                    if (row["SlabRange"].ToString() == sub.Slabrange_Name)
                    {
                        row["SlabRange"] = sub.Slabrange_Name;
                        row["From"] = sub.Slabrange_From;
                        row["To"] = sub.Slabrange_To;
                        Session["Datatable"] = dt;
                        result = "1";
                    }
                }
                
            //}
            //else
            //{
            //    result = "Maxval";
            //}
            foreach (DataRow row in dt.Rows)
            {
                sub.Slabrange_Name = row["SlabRange"].ToString();
                sub.Slabrange_From = row["From"].ToString();
                sub.Slabrange_To = row["To"].ToString();
               
            }
     
            return Json(result, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        public JsonResult Deleteslabrangeedit(SupClassificationModel student)
        {
            string result = string.Empty;
            try
            {
                result = objModel.DeleteSlab_rangeedit(student.Slabid);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteSlab(SupClassificationModel student)
        {
           
            string result = string.Empty;
            try
            {
                result = objModel.DeleteSlab_all(student.Slabrange_Slabgid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult View(int id, string viewfor)
        {
            if (viewfor == "Delete")
            {
                ViewBag.viewfor = "Delete";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            Session["slab_id"] = id;
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            if ( id != 0)
            {

                obj = objModel.selectslabrangeedit(id).ToList();
                if (obj.Count > 0)
                {
                    Session["slabnameView"] = obj[0].Slabname.ToString();
                }
           
            }

            return PartialView(obj);
        }
        public PartialViewResult Delete(int id)
        {
            Session["id"] = id;
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            if (id != 0)
            {

                obj = objModel.selectslabrangeedit(id).ToList();
                if (obj.Count > 0)
                {
                    Session["slabnameView"] = obj[0].Slabname.ToString();
                }

            }

            return PartialView(obj);
        }
        //[HttpPost]
        //public JsonResult DeleteSlabindex()
        //{
        //    String result = String.Empty;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            objModel.DeleteSlabIndex(Convert.ToInt16(Session["id"]));
        //            result = "1";
        //        }
        //        else
        //        {
        //            result = "0";
        //        }
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
     
        [HttpGet]
        public PartialViewResult NewSlabCreation(string listfor, string name, string viewfor)
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            if (listfor != null)
            {
                obj = objModel.GridLoad().ToList();
            }
            if (name=="0")
            {  Session["process"] = "";}
            Session["save"] = "";
            return PartialView(obj);
        }
        public JsonResult SaveSlab(SupClassificationModel sub)
        {
            string nmk = Convert.ToString(Session["save"]);
            if (Convert.ToString(Session["save"]) == "")
            {
                if (Convert.ToDecimal(sub.Slabrange_From) < Convert.ToDecimal(sub.Slabrange_To))
                {
                    Result = objModel.SaveSlab(sub);
                }
                else
                {
                    Result = "Slab Range To Value Must Be Greater Then Slab Range From Value";
                }
                
            }
            if (Result != "Duplicate Slab Name !!" && Result != "Slab Range To Value Must Be Greater Then Slab Range From Value")
            {
                if (Convert.ToDecimal(sub.Slabrange_From) < Convert.ToDecimal(sub.Slabrange_To))
                {
                    Result = objModel.SaveSlabRanges(sub);
                }
                else
                {
                    Result = "Slab Range To Value Must Be Greater Then Slab Range From Value";
                }
                if (Result == "Maxval")
                {
                    Result = "Maxval";
                }
                if (Result == "sub")
                {
                    Result = "sub";
                }
                if (Result == "1")
                {
                    Result = "sub";
                }
                if (Result == "Can't Insert Less Than Slab To Range")
                {
                    Result = "Can't Insert Less Than Slab To Range";
                }
                if (Result == "Please Enter Correct Flow Range")
                {
                    Result = "Please Enter Correct Flow Range";
                }
                if (Result == "This Slab Range Based On Some Other Delmat Matrix")
                {
                    Result = "This Slab Range Based On Some Other Delmat Matrix";
                }
                if (Result == "This Slab  Based On Some Other Delmat Matrix")
                {
                    Result = "This Slab  Based On Some Other Delmat Matrix";
                }
                Session["process"] = sub.Slabname;
                Session["save"] = "1";
                ViewBag.process = "data";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            Session["save"] = "1";
            Session["process"] = "";
            Session["Slab_id"] = id;
            Session["slabId"] = "0";
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            obj = objModel.GridLoadEdit(id).ToList();
            if (obj.Count > 0)
            {
                Session["slabname"] = obj[0].Slabname;
                Session["slabId"] = obj[0].Slabid;
            }
            
            return PartialView(obj);
        }
       
    }
}
