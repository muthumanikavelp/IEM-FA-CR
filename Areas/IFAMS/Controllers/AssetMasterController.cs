using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetMasterController : Controller
    {
        //
        // GET: /IFAMS/AssetMaster/
        DataModel model = new DataModel();
        ErrorLog objErrorLog = new ErrorLog();
        List<string> list = new List<string>();
        [HttpGet]
        public ActionResult AssetMasterSummary()
        {
            TransferMakerModel trf = new TransferMakerModel();
            try
            {
                Session["exceldownload"] = "";
                trf.TModel = model.getassetcat();
                Session["exceldownload"] = trf.TModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View(trf);
        }


        [HttpPost]
        public ActionResult AssetMasterSummary(string category, string filter, FormCollection collections, string command)
        {
            try
            {
                TransferMakerModel trf = new TransferMakerModel();
                trf.TModel = model.getassetcat();
                if (command == "Search")
                {
                    Session["exceldownload"] = "";
                    trf.TModel = model.getassetcat().ToList();
                    if ((string.IsNullOrEmpty(category) || string.IsNullOrWhiteSpace(category))
                        && (string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter))
                       )
                    {
                        trf.TModel = model.getassetcat().ToList();
                    }
                    if (category != null)
                    {
                        ViewBag.category = category;
                        trf.TModel = trf.TModel.Where(x => category == null || (x._toa_number.ToUpper().Contains(category.ToUpper()))).ToList();
                    }

                    if (filter != null)
                    {
                        ViewBag.gl = filter;
                        trf.TModel = trf.TModel.Where(x => filter == null || (x._tfr_date.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }
                   
                    if (trf.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["exceldownload"] = trf.TModel;
                    return View(trf);
                }
                if (command == "Clear")
                {
                    return RedirectToAction("TMSummary", "AssetTransfer");
                }
                else if (command == "TRANSFER")
                {
                    return RedirectToAction("TMSummary");
                }
                return View(trf);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
        }
        [HttpPost]
        public JsonResult GlNumberLoad(string GlNumber)
        {
            List<TransferMakerModel> glnumber = new List<TransferMakerModel>();
            glnumber = model.GetGlNumber(GlNumber).ToList();
            return Json(glnumber);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            list.Add("121020001");
            list.Add("121020004");
            List<string> glno = new List<string>();
            {
                glno = list;
            };
            List<SelectListItem> gl = new List<SelectListItem>();
            foreach (var item in glno)
            {
                gl.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    //Value = item.ToString()
                });
            }
            ViewBag.ClearenceGlNo = gl;

            TransferMakerModel TypeModel = new TransferMakerModel();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult Create(TransferMakerModel TypeModel)
        {
            string check = "";
            try
            {
                check = model.InsertCat(TypeModel);
                if (check == null)
                {
                    RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check, JsonRequestBehavior.AllowGet);
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
            TransferMakerModel TypeModel = model.GetCatById(id);
           
            list.Add("121020001");
            list.Add("121020004");
            List<string> glno = new List<string>();
            {
                glno = list;
            };
            List<SelectListItem> gl = new List<SelectListItem>();
            foreach (var item in glno)
            {
                Boolean selected = false;
                if (item == TypeModel.asset_clearence_gl.ToString())
                {
                    selected = true;
                }
                gl.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected=selected
                    //Value = item.ToString()
                });
            }
            ViewBag.ClearenceGlNo = gl;

            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult Edit(TransferMakerModel TypeModel)
        {
            string check = "";
            try
            {
                check = model.EditCat(TypeModel); ;
                if (check == null)
                {
                    RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Delete(TransferMakerModel TypeModel)
        {
            string check = null;
            try
            {
                var ids = TypeModel._gid;
                check = model.Deletecat(ids);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public ActionResult downloadsexcel()
        {
            List<TransferMakerModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = model.getassetcat().ToList();
            }
            else
            {
                cList = (List<TransferMakerModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Category");
            dt.Columns.Add("GL No ");
            dt.Columns.Add("Dep GL No");
            dt.Columns.Add("Dep Res GL No ");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                , cList[i]._toa_number.ToString()
                , cList[i]._AssetCatCode.ToString()
                , cList[i]._tfr_status.ToString()
                , cList[i]._tfr_date.ToString());
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "AssetCategory.xls");
                return new DownloadFileActionResult(gv, "AssetCategory.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
