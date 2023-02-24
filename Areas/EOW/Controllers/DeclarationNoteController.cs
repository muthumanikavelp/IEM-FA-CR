using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.Configuration;
using ClosedXML.Excel;
using IEM.App_Start;

namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class DeclarationNoteController : Controller
    {
        //
        // GET: /EOW/DeclarationNote/

        private DeclarationNoteRepository decnote;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public DeclarationNoteController()
            : this(new DeclarationNote())
        {

        }
        public DeclarationNoteController(DeclarationNoteRepository objM)
        {
            decnote = objM;
        }
        public ActionResult Index()
        {
            List<EOW_DeclarationNote> Releaserecords = new List<EOW_DeclarationNote>();
            Releaserecords = decnote.DisplayGrid().ToList();           
            List<GetDoctype> Rolelist = new List<GetDoctype>();
            {
                Rolelist = decnote.GetDoctype().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                role.Add(new SelectListItem
                {
                    Text = item.doctypecode.ToString(),
                    Value = item.doctypecode.ToString()
                });
            }
            ViewBag.DocType = role;

            if (Releaserecords.Count == 0)
            {
                ViewBag.Message = "No Records Found";
            }

            return View(Releaserecords);
        }

        [HttpPost]
        public ActionResult Index(string declnotename, string DocType, string periodfrom, string periodto, string command = null)
        {
            List<EOW_DeclarationNote> Releaserecords = new List<EOW_DeclarationNote>();
            if (command == "Search" || command == "Refresh")
            {
                Releaserecords = decnote.DisplayGrid(declnotename, DocType, periodfrom, periodto).ToList();
                List<GetDoctype> Rolelist = new List<GetDoctype>();
                {
                    Rolelist = decnote.GetDoctype().ToList();
                };

                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item.doctypecode == DocType)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.doctypecode.ToString(),
                        Value = item.doctypecode.ToString(),
                        Selected=selected
                    });
                }

                ViewBag.DocType = role;
                ViewBag.periodto = periodto;
                ViewBag.periodfrom = periodfrom;
                ViewBag.declnotename=declnotename;
                if (Releaserecords.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
            }
            if(command=="Clear")
            {
                Releaserecords = decnote.DisplayGrid().ToList();
                List<GetDoctype> Rolelist = new List<GetDoctype>();
                {
                    Rolelist = decnote.GetDoctype().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.doctypecode.ToString(),
                        Value = item.doctypecode.ToString()
                    });
                }
                ViewBag.DocType = role;
            }

            return View(Releaserecords);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            string res = string.Empty;
            EOW_DeclarationNote Typemodel = new EOW_DeclarationNote();
            Typemodel.GetDoctype = new SelectList(decnote.GetDoctype(), "doctypegid", "doctypename", Typemodel.selecteddoctype_gid);
            return PartialView(Typemodel);
        }
        [HttpPost]
        public JsonResult GetValue(EOW_DeclarationNote delnote)
        {
            EOW_DeclarationNote TypeModel = new EOW_DeclarationNote();
            TypeModel.GetDocsubtype = new SelectList(decnote.GetDocsubtype(delnote.doctype_gid), "docsubtypegid", "docsubtypename");
            return Json(decnote.GetDocsubtype(delnote.doctype_gid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertDecnotedetails(EOW_DeclarationNote decnoteforinsert)
        {
            string res = string.Empty;
            DataTable getdoctypecode = new DataTable();
            DataTable getdocsubtype = new DataTable();
            getdoctypecode = decnote.getdoctypecode(decnoteforinsert.doctype_gid);
            getdocsubtype = decnote.getdocsubtypename(decnoteforinsert.docsubtype_gid);
            decnoteforinsert.doctype_code = getdoctypecode.Rows[0]["doctype_code"].ToString();
            decnoteforinsert.docsubtype_name = getdocsubtype.Rows[0]["docsubtype_name"].ToString();
            res = decnote.InsertDecNote(decnoteforinsert);
            return Json(res, JsonRequestBehavior.AllowGet);
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
            EOW_DeclarationNote TypeModel = new EOW_DeclarationNote();
            TypeModel = decnote.DisplayGridById(id);
            TypeModel.GetDoctype = new SelectList(decnote.GetDoctype(), "doctypegid", "doctypename", TypeModel.selecteddoctype_gid);
            TypeModel.GetDocsubtype = new SelectList(decnote.GetDocsubtype(TypeModel.selecteddoctype_gid), "docsubtypegid", "docsubtypename", TypeModel.selecteddocsubtype_gid);
            //TypeModel=decnote.
            //Typemodel.GetDoctype = new SelectList(decnote.GetDoctype(), "doctypegid", "doctypecode", Typemodel.selecteddoctype_gid);
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult UpdateDecnotedetails(EOW_DeclarationNote decnoteforupdate)
        {
            string res = string.Empty;
            DataTable getdoctypecode = new DataTable();
            DataTable getdocsubtype = new DataTable();
            getdoctypecode = decnote.getdoctypecode(decnoteforupdate.doctype_gid);
            getdocsubtype = decnote.getdocsubtypename(decnoteforupdate.docsubtype_gid);
            decnoteforupdate.doctype_code = getdoctypecode.Rows[0]["doctype_code"].ToString();
            decnoteforupdate.docsubtype_name = getdocsubtype.Rows[0]["docsubtype_name"].ToString();
            res = decnote.UpdateDecnote(decnoteforupdate);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDeclartionnote(EOW_DeclarationNote decnoteforupdate)
        {
            string lsresult = string.Empty;
            try
            {
                var ids = decnoteforupdate.declnote_gid;
                lsresult = decnote.DeleteNote(ids);
                ViewBag.viewfor = "delete";
                return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = (DataSet)Session["ExcelExportDeclarationNote"];
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
