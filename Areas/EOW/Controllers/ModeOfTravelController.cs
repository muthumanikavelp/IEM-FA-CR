using IEM.Areas.EOW.Models;
using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class ModeOfTravelController : Controller
    {
        private ModeOftravelRepository objModel;
        int id;
        string Result;
        string result;

        private dbLib db = new dbLib();
        private proLib pl = new proLib();

        public ModeOfTravelController()
            : this(new ModeofTravelModel())
        {

        }
        public ModeOfTravelController(ModeOftravelRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult ModeIndex()
        {
            try
            {
                List<ModeOfTravelEntity> mode = new List<ModeOfTravelEntity>();
                mode = objModel.SelectTravelMode().ToList();
                if (mode.Count == 0)
                {
                    ViewBag.Message = "No Records";
                }
                return View(mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ModeIndex(string ModeofTravel = null)
        {
            try
            {
                List<ModeOfTravelEntity> mode = new List<ModeOfTravelEntity>();
                mode = objModel.SelectTravelMode(ModeofTravel).ToList();
                ViewBag.ModeOfTravel = ModeofTravel;
                if (mode.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return View(mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ModeAdd()
        {
            return PartialView();
        }
        public JsonResult ModeSubmit(ModeOfTravelEntity mode)
        {
            try
            {
                Result = objModel.SubmitMode(mode);
                if (Result == "sub")
                {
                    result = "Record Added Successfully!";
                }
                if (Result == "duplicate")
                {
                    result = "You Can't Add Duplicate Record";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ModeEdit(int id)
        {
            try
            {
                Session["Modeid"] = id;
                List<ModeOfTravelEntity> mode = new List<ModeOfTravelEntity>();
                mode = objModel.EditMode(id).ToList();
                Session["TravelName"] = mode[0].ModeOfTravel;
                ViewBag.Modeflag = mode[0].ModeFlag;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult ModeEditSubmit(ModeOfTravelEntity mode)
        {
            try
            {
                if (Session["Modeid"] != null)
                {
                    mode.ModeId = Convert.ToInt16(Session["Modeid"]);
                }
                Result = objModel.SubmitEditMode(mode);
                if (Result == "sub")
                {
                    result = "Record Updated Successfully!";
                }
                if (Result == "duplicate")
                {
                    result = "You Can't Edit Duplicate Record";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ModeView(int id)
        {
            try
            {
                List<ModeOfTravelEntity> mode = new List<ModeOfTravelEntity>();
                mode = objModel.EditMode(id).ToList();
                Session["TravelName"] = mode[0].ModeOfTravel;
                ViewBag.Modeflag = mode[0].ModeFlag;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public PartialViewResult ModeDeletev(int id)
        {
            try
            {
                Session["id"] = id;
                List<ModeOfTravelEntity> mode = new List<ModeOfTravelEntity>();
                mode = objModel.EditMode(id).ToList();
                Session["TravelName"] = mode[0].ModeOfTravel;
                ViewBag.Modeflag = mode[0].ModeFlag;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult ModeDelete()
        {
            try
            {
                //result=objModel.DeleteTravel(Convert.ToInt16(Session["id"]));
                if (result != "EXISTS")
                {
                    result = objModel.DeleteMode(Convert.ToInt16(Session["id"]));
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
