using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class HolidayMasterController : Controller
    {
        private dbLib db = new dbLib();
        private proLib pl = new proLib();
        SupClassificationModel sub=new SupClassificationModel();
        private HolidayRepository objModel;
        int id;
        public HolidayMasterController()
            : this(new HolidayMasterModel())
        {

        }
        public HolidayMasterController(HolidayRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Holiday()
        {
            try
            {
                List<SupClassificationModel> obj = new List<SupClassificationModel>();
                SupClassificationModel sub = new SupClassificationModel();
                obj = objModel.SelectHoliday().ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Holiday(string filter = null)
        {
            try
            {
                List<SupClassificationModel> records = new List<SupClassificationModel>();
                records = objModel.SelectHoliday().ToList();
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records = records.Where(x => filter == null ||
                           (x.HolidayName.ToUpper().Contains(filter.ToUpper()))).ToList();
                } if (records.Count == 0)
                {
                    ViewBag.message = "No Record's Found!";
                }

                return View(records);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult HolidayAdd()
        {
            try
            {
                SupClassificationModel objStudentModel = new SupClassificationModel();
                DataTable dt = objModel.SelectStates();
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["state_name"].ToString(), Value = row["state_gid"].ToString() });
                        objStudentModel.State = names;
                    }
                }
                return PartialView(objStudentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult HolidayNewAdd(SupClassificationModel student,string[] SelectedState)
        {
            String result = String.Empty;
            try
            {
                if (ModelState.IsValid)
                { 
                    if(SelectedState ==null)
                    {
                        result = "Ensure State!";
                   
                    }
                    else
                    {
                        result = objModel.AddHoliday(student);
                        if (result == "EXISTS")
                        {
                            result = "Duplicate Record";
                        }
                        else
                        {

                            result = objModel.AddHolidayState(student, SelectedState);
                            if (result == "INS")
                            {
                                result = "Record Inserted Successfully";
                            }
                            else
                            { result = "Duplicate Record"; }
                        }

                    }
                  
                }
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ViewHoliday(int id)
        {
            try
            {
                SupClassificationModel objStudentModel = new SupClassificationModel();
                DataTable dt = objModel.ViewHoliday(id);
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["state_name"].ToString(), Value = row["state_gid"].ToString() });
                        objStudentModel.State = names;
                        objStudentModel.HolidayDate = row["holiday_date"].ToString();
                        objStudentModel.HolidayName = row["holiday_name"].ToString();
                        objStudentModel.NationalHoliday = Convert.ToChar(row["holiday_national_flag"].ToString());
                        objStudentModel.StateHoliday = Convert.ToChar(row["holiday_state_flag"].ToString());
                        objStudentModel.Cutoff = Convert.ToChar(row["holiday_cutoff_flag"].ToString());

                    }
                }
                return PartialView(objStudentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult EditHoliday(int id)
        {
            try
            {
                Session["ID"] = id;
                SupClassificationModel objStudentModel = new SupClassificationModel();
                DataTable dt = objModel.ViewHoliday(id);

                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objStudentModel.HolidayDate = dt.Rows[i]["holiday_date"].ToString();
                        objStudentModel.HolidayName = dt.Rows[i]["holiday_name"].ToString();
                        objStudentModel.NationalHoliday = Convert.ToChar(dt.Rows[i]["holiday_national_flag"].ToString());
                        objStudentModel.StateHoliday = Convert.ToChar(dt.Rows[i]["holiday_state_flag"].ToString());
                        objStudentModel.Cutoff = Convert.ToChar(dt.Rows[i]["holiday_cutoff_flag"].ToString());
                        objStudentModel.lstSelectedStateGid[i] = string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "" : dt.Rows[i]["state_gid"].ToString();
                    }
                    DataTable data = objModel.SelectStates();
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in data.Rows)
                        {
                            names.Add(new SelectListItem { Text = row1["state_name"].ToString(), Value = row1["state_gid"].ToString() });
                            objStudentModel.State = names;

                        }
                    }
                }
                return PartialView(objStudentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult HolidayEdit(SupClassificationModel student, string[] SelectedState)
        {
            
            if (Session["ID"] != null)
            {
               id = (int)Session["ID"];
            }
            String result = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (SelectedState == null)
                    {
                        result = "Ensure State!";

                    }
                    else
                    {
                        result = objModel.EditHoliday(student, id);
                        if (result == "EXISTS")
                        {
                            result = "Duplicate Record";
                        }
                        else
                        {
                            result = objModel.DeleteHolidayState(student, id);
                            result = objModel.EditHolidayState(student, SelectedState, id);
                            if (result == "INS")
                            {
                                result = "Record Updated Successfully!";
                            }
                        }
                    }

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult Delete(int id)
        {
            try
            {
                Session["ID"] = id;
                SupClassificationModel objStudentModel = new SupClassificationModel();
                DataTable dt = objModel.ViewHoliday(id);
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["state_name"].ToString(), Value = row["state_gid"].ToString() });
                        objStudentModel.State = names;
                        objStudentModel.HolidayDate = row["holiday_date"].ToString();
                        objStudentModel.HolidayName = row["holiday_name"].ToString();
                        objStudentModel.NationalHoliday = Convert.ToChar(row["holiday_national_flag"].ToString());
                        objStudentModel.StateHoliday = Convert.ToChar(row["holiday_state_flag"].ToString());
                        objStudentModel.Cutoff = Convert.ToChar(row["holiday_cutoff_flag"].ToString());

                    }
                }
                //SupClassificationModel objStudentModel = new SupClassificationModel();
                //DataTable dt = objModel.ViewHoliday(id);

                //List<SelectListItem> names = new List<SelectListItem>();
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        objStudentModel.HolidayDate = dt.Rows[i]["holiday_date"].ToString();
                //        objStudentModel.HolidayName = dt.Rows[i]["holiday_name"].ToString();
                //        objStudentModel.NationalHoliday = Convert.ToChar(dt.Rows[i]["holiday_national_flag"].ToString());
                //        objStudentModel.StateHoliday = Convert.ToChar(dt.Rows[i]["holiday_state_flag"].ToString());
                //        objStudentModel.Cutoff = Convert.ToChar(dt.Rows[i]["holiday_cutoff_flag"].ToString());
                //        objStudentModel.lstSelectedStateGid[i] = string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "" : dt.Rows[i]["state_gid"].ToString();
                //    }
                //DataTable data = objModel.SelectStates();
                //if (data.Rows.Count > 0)
                //{
                //    foreach (DataRow row1 in data.Rows)
                //    {
                //        names.Add(new SelectListItem { Text = row1["state_name"].ToString(), Value = row1["state_gid"].ToString() });
                //        objStudentModel.State = names;

                //    }
                //}
                //}

                return PartialView(objStudentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult DeleteHoliday()
        {
            SupClassificationModel sub = new SupClassificationModel();
            String result = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.DeleteHoliday(sub,Convert.ToInt16(Session["ID"]));
                    result = "1";
                }
                else
                {
                    result = "0";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //downloading Excel File
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = db.GetCommonExcelDownload(ViewType, pl.LoginUserId);
            DataTable _downloadingData = ds.Tables[0];
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= " + FileName + ".xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
