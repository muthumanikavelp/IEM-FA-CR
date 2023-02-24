using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace IEM.Areas.IFAMS.Controllers
{
    public class AssetCodeMasterController : Controller
    {
        private Ifams_Repository iry;
        ErrorLog objErrorLog = new ErrorLog();
        Ifams_Model mod = new Ifams_Model();
        public AssetCodeMasterController()
            : this(new Ifams_Model())
        {

        }

        public AssetCodeMasterController(Ifams_Repository iiry)
        {
            iry = iiry;
        }
        public ActionResult AssetCodeMaster()
        {
            Session["ExcelExportAsset"] = null;
            Ifams_Property objas = new Ifams_Property();
            List<Ifams_Property> lobjas = new List<Ifams_Property>();
            try
            {
                // Session["AssetDeatils"] = iry.GetAssetDetails(objas);
                objas.AssetDetails = iry.GetAssetDetails(objas);
                Session["AssetDeatils"] = objas.AssetDetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }

            // lobjas = iry.Getassetcategory();
            //  objas.assetCategory = new SelectList(lobjas, "assetCategoryId", "assetCategoryName");
            //  objas.assetSubCategory = new SelectList(string.Empty, "assetSubCategoryId", "assetSubCategoryName");

            return View(objas);
        }

        [HttpPost]
        public ActionResult AssetCodeMaster(Ifams_Property gobj, string Refersh)
        {
            List<Ifams_Property> objsl = new List<Ifams_Property>();
            objsl = (List<Ifams_Property>)Session["AssetDeatils"];

            try
            {
                if (Refersh == "Refersh")
                {
                    @ViewBag.assetCode = "";
                    @ViewBag.assetDesc = "";
                    gobj.assetCode = null;
                    gobj.assetDesc = null;
                    gobj.AssetDetails = objsl;
                }
                else
                {
                    if (!string.IsNullOrEmpty(gobj.assetCode) && !string.IsNullOrEmpty(gobj.assetDesc))
                    {

                        //ViewBag.txtSuppliercode = txtSuppliercode;
                        //objActi.SupplierActiveList = objActi.SupplierActiveList.Where(x => txtSuppliercode == null ||
                        //   (x.SupplierCode.ToUpper().Contains(txtSuppliercode.ToUpper()))).ToList();
                        //Session["objlist1"] = objActi.SupplierActiveList;
                        //gobj.AssetDetails = objsl.Where(a => a.assetCode == null
                        //    || (a.assetCode.ToUpper().Contains(gobj.assetCode.ToUpper()))

                        //    ).ToList();
                        gobj.AssetDetails = objsl.Where(p => p.assetCode.ToUpper().Contains(gobj.assetCode.ToUpper())
                            && p.assetDesc.ToUpper().Contains(gobj.assetDesc.ToUpper())).ToList();
                        //  gobj.AssetDetails=objsl a=>a.assetCode == (a.assetCode.ToUpper().Contains(gobj.assetCode.ToUpper())) && a.assetDesc == (a.assetCode.ToUpper().Contains(gobj.assetCode.ToUpper()))).ToList()
                    }
                    else if (!string.IsNullOrEmpty(gobj.assetCode))
                    {
                        gobj.AssetDetails = objsl.Where(a => a.assetCode == null
                            || (a.assetCode.ToUpper().Contains(gobj.assetCode.ToUpper()))).ToList();
                    }
                    else if (!string.IsNullOrEmpty(gobj.assetDesc))
                    {
                        gobj.AssetDetails = objsl.Where(a => a.assetDesc == null
                        || (a.assetDesc.ToUpper().Contains(gobj.assetDesc.ToUpper()))).ToList();
                    }
                    else
                    {
                        gobj.AssetDetails = objsl;
                    }
                    @ViewBag.assetCode = gobj.assetCode;
                    @ViewBag.assetDesc = gobj.assetDesc;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }

            return View(gobj);
        }

        public ActionResult Subcategory(int category)
        {

            List<Ifams_Property> lobsub = new List<Ifams_Property>();
            var subcate = lobsub;
            try
            {
                lobsub = iry.Getsubcategory(category);
                subcate = lobsub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json(subcate, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult NewAssetCodeMaster()
        {

            Ifams_Property objas = new Ifams_Property();
            List<Ifams_Property> lobjas = new List<Ifams_Property>();
            List<SelectListItem> AllHSN = new List<SelectListItem>();
            int[] SelectedHSNIDs = new int[] { 0 };
            AllHSN = iry.GetAllHsn();
            try
            {
                if (TempData["model"] != null)
                {
                    objas = (Ifams_Property)TempData["model"];
                    // objas.Status = "edit";
                }
                else if (TempData["modelview"] != null)
                {

                    objas = (Ifams_Property)TempData["modelview"];
                    //  objas.Status = "view";
                }
                else
                {

                    lobjas = iry.Getassetcategory();
                    List<Ifams_Property> Objhsn = new List<Ifams_Property>();
                    Objhsn = iry.GetHsncode();
                    objas.assetCategory = new SelectList(lobjas, "assetCategoryId", "assetCategoryName");
                    objas.assetSubCategory = new SelectList(string.Empty, "assetSubCategoryId", "assetSubCategoryName");
                    objas.GetHsncode = new SelectList(Objhsn, "Hsn_gid", "hsn_code");
                    objas.Status = "submit";
                    MultiSelectList teamsList = new MultiSelectList(AllHSN.OrderBy(i => i.Text), "Value", "Text", SelectedHSNIDs);
                    objas.Items = teamsList;
                    objas.SelectedItemIds = SelectedHSNIDs;
                    Session["viewfor"] = "submit";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return PartialView(objas);
        }
        [HttpPost]
        public ActionResult NewAssetCodeMaster(Ifams_Property obj, string ddlSubcategory, string ddlclassication, string ddlcategory, string ownedByName)
        {
            // Ifams_Property objas = new Ifams_Property();
            //List<Ifams_Property> lobjas = new List<Ifams_Property>();
            //lobjas = iry.Getassetcategory();
            //objas.assetCategory = new SelectList(lobjas, "assetCategoryId", "assetCategoryName");
            //objas.assetSubCategory = new SelectList(string.Empty, "assetSubCategoryId", "assetSubCategoryName");
            string Result = string.Empty;
            try
            {
                if (obj.Status == "edit")
                {
                    //obj.assetSubCategoryName = ddlSubcategory;
                    obj.classficationName = ddlclassication;
                    obj.assetCategoryName = ddlcategory;
                    obj.ownedByName = ownedByName;
                    Result = iry.UpdateAssetDetails(obj,"1");
                }
                else if (obj.Status == "submit")
                {
                    //obj.assetSubCategoryName = ddlSubcategory;
                    obj.classficationName = ddlclassication;
                    obj.assetCategoryName = ddlcategory;
                    obj.ownedByName = ownedByName;
                    Result = iry.InsertAssetDetails(obj,"1");
                }

                Session["message"] = obj.Status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return RedirectToAction("../AssetCodeMaster/AssetCodeMaster");
        }


        public ActionResult EditAssetCode(string id, string type)
        {
            List<Ifams_Property> obje = new List<Ifams_Property>();
            List<Ifams_Property> objeN = new List<Ifams_Property>();
            List<Ifams_Property> Objhsn = new List<Ifams_Property>();
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> AllHSN = new List<SelectListItem>();
            Objhsn = iry.GetHsncode();
            obje = (List<Ifams_Property>)Session["AssetDeatils"];
            Ifams_Property objed = new Ifams_Property();
            Session["viewfor"] = type;
            try
            {
                objed.assetgid = id;
                if (id != string.Empty)
                {
                    objed.AssetDetails = obje.Where(a => a.assetgid == id).ToList();
                    TempData["filterdata"] = objed.AssetDetails;

                    AllHSN = iry.GetAllHsn();
                    items = iry.GetHsnforasset(id);

                    //for (int i2 = 0; i2 < Objhsn.Count; i2++)
                    //{
                    //    for (int i = 0; i < items.Count; i++)
                    //    {
                    //        if (Objhsn[i2].Hsn_gid.ToString().Trim() == items[i].Hsn_gid.ToString().Trim())
                    //        {
                    //            objed.GetHsncode.SelectedValue = true;
                    //        }
                    //    }
                    //}


                    int[] SelectedHSNIDs = new int[100];

                    for (int i = 0; i < items.Count; i++)
                    {
                        SelectedHSNIDs[i] = String.IsNullOrEmpty(items[i].Value.ToString()) ? 0 : Convert.ToInt16(items[i].Value);
                    }
                    if (SelectedHSNIDs.Length == 0)
                    {
                        SelectedHSNIDs[0] = 0;
                    }
                    MultiSelectList teamsList = new MultiSelectList(AllHSN.OrderBy(i => i.Text), "Value", "Text", SelectedHSNIDs);
                    objed.Items = teamsList;
                    objed.SelectedItemIds = SelectedHSNIDs;
                    objed.assetgid = objed.AssetDetails[0].assetgid.ToString();
                    objed.assetCode = objed.AssetDetails[0].assetCode.ToString();
                    objed.assetDesc = objed.AssetDetails[0].assetDesc.ToString();
                    objed.hsn_code = objed.AssetDetails[0].hsn_code.ToString();

                    //objed.GetHsncode = new SelectList(Objhsn, "Hsn_gid", "hsn_code", items);
                    ////ViewBag.EntidadList = new SelectList(Objhsn, "Hsn_gid", "hsn_code", items);
                 //   objed.GetHsncode = new SelectList(Objhsn, "Hsn_gid", "hsn_code");

                    //for (int i2 = 0; i2 < items.Count; i2++)
                    //{
                    objed.GetHsncode = new SelectList(items, "value", "text", selectedValue: "1");
                    //}

                    //objed.GetHsncode = new SelectList(Objhsn, "Hsn_gid", "hsn_code", selectedValue: objed.AssetDetails[0].Hsn_gid);
                    //objed.GetHsncode = new SelectList(Objhsn, "Hsn_gid", "hsn_code", selectedValue: Objhsn != null && Objhsn.Contains(items));
                    objeN = iry.Getassetcategory();
                    objed.hsn_desc = iry.GethsndescASSET(id);
                    objed.assetCategory = new SelectList(objeN, "assetCategoryId", "assetCategoryName", selectedValue: objed.AssetDetails[0].assetCategoryId);
                    objed.glCode = objed.AssetDetails[0].glCode.ToString();
                    objed.classficationId = Convert.ToInt32(objed.AssetDetails[0].classficationName.ToString() == "PM" ? "1" : "2");
                    objed.ownedByName = string.IsNullOrEmpty(objed.AssetDetails[0].ownedByName.ToString()) ? "0" : objed.AssetDetails[0].ownedByName.ToString();

                    objed.Verifiable = objed.AssetDetails[0].Verifiable;
                    objed.NonVerifiable = objed.AssetDetails[0].NonVerifiable;

                    objed.Barcode = (objed.AssetDetails[0].BarcodeIsMandatory == "Required") ? true : false;
                    objed.Quantified = (objed.AssetDetails[0].IsQuantified == "Yes") ? true : false;

                    objed.Mandatory = objed.AssetDetails[0].Mandatory;
                    objed.NonMantadatory = objed.AssetDetails[0].NonMantadatory;

                    objed.Movable = objed.AssetDetails[0].Movable;
                    objed.Immovable = objed.AssetDetails[0].Immovable;

                    objed.SLM = objed.AssetDetails[0].SLM;
                    objed.LPM = objed.AssetDetails[0].LPM;

                    objed.umonths = Convert.ToInt32(string.IsNullOrEmpty(objed.AssetDetails[0].umonths.ToString()) ? "0" : objed.AssetDetails[0].umonths.ToString());
                    objed.descRate = objed.AssetDetails[0].descRate.ToString();
                    objed.descGlCode = objed.AssetDetails[0].descGlCode.ToString();
                    objed.descResGlCode = objed.AssetDetails[0].descResGlCode.ToString();

                    //objSu = iry.Getsubcategory(objed.AssetDetails[0].assetCategoryId);
                    //objed.assetSubCategory = new SelectList(objSu, "assetSubCategoryId", "assetSubCategoryName", selectedValue: objed.AssetDetails[0].assetSubCategoryId);
                    objed.Status = type;
                    TempData["model"] = objed;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            //  return View("../AssetCodeMaster/NewAssetCodeMaster", objed);
            // return RedirectToAction("NewAssetCodeMaster","AssetCodeMaster");
            return RedirectToAction("../AssetCodeMaster/NewAssetCodeMaster", DateTime.Now.AddSeconds('1'));
        }

        public ActionResult viewTrigger(string id)
        {
            List<Ifams_Property> obje = new List<Ifams_Property>();
            List<Ifams_Property> objeN = new List<Ifams_Property>();
            List<Ifams_Property> objSu = new List<Ifams_Property>();
            obje = (List<Ifams_Property>)Session["AssetDeatils"];
            Ifams_Property objed = new Ifams_Property();
            try
            {
                objed.assetgid = id;
                if (id != string.Empty)
                {
                    //objed.AssetDetails = obje.Where(a => a.assetgid == id).ToList();
                    ////  TempData["filterdata"] = objed.AssetDetails;
                    //objed.assetgid = objed.AssetDetails[0].assetgid.ToString();
                    //objed.assetCode = objed.AssetDetails[0].assetCode.ToString();
                    //objed.assetDesc = objed.AssetDetails[0].assetDesc.ToString();
                    //objed.glCode = objed.AssetDetails[0].glCode.ToString();
                    //objed.descGlCode = objed.AssetDetails[0].descGlCode.ToString();
                    //objed.descRate = objed.AssetDetails[0].descRate.ToString();
                    //objed.descResGlCode = objed.AssetDetails[0].descResGlCode.ToString();
                    //objed.Barcode = objed.AssetDetails[0].Barcode;
                    //objed.Quantified = objed.AssetDetails[0].Quantified;
                    //objed.Mandatory = (objed.AssetDetails[0].Mandatory == "true") ? "1" : "0";
                    //objed.NonMantadatory = objed.AssetDetails[0].NonMantadatory;
                    //objed.Verifiable = objed.AssetDetails[0].Verifiable;
                    //objed.NonVerifiable = objed.AssetDetails[0].NonVerifiable;
                    //objed.SLM = objed.AssetDetails[0].SLM;
                    //objed.LPM = objed.AssetDetails[0].LPM;

                    //objeN = iry.Getassetcategory();
                    //objSu = iry.Getsubcategory(objed.AssetDetails[0].assetCategoryId);
                    //objed.assetCategory = new SelectList(objeN, "assetCategoryId", "assetCategoryName", selectedValue: objed.AssetDetails[0].assetCategoryId);
                    //objed.assetSubCategory = new SelectList(objSu, "assetSubCategoryId", "assetSubCategoryName", selectedValue: objed.AssetDetails[0].assetSubCategoryId);

                    objed.AssetDetails = obje.Where(a => a.assetgid == id).ToList();
                    TempData["filterdata"] = objed.AssetDetails;

                    objed.assetgid = objed.AssetDetails[0].assetgid.ToString();
                    objed.assetCode = objed.AssetDetails[0].assetCode.ToString();
                    objed.assetDesc = objed.AssetDetails[0].assetDesc.ToString();
                    objeN = iry.Getassetcategory();
                    objed.assetCategory = new SelectList(objeN, "assetCategoryId", "assetCategoryName", selectedValue: objed.AssetDetails[0].assetCategoryId);
                    objed.glCode = objed.AssetDetails[0].glCode.ToString();
                    objed.classficationId = Convert.ToInt32(objed.AssetDetails[0].classficationName.ToString() == "PM" ? "1" : "2");
                    objed.ownedByName = string.IsNullOrEmpty(objed.AssetDetails[0].ownedByName.ToString()) ? "0" : objed.AssetDetails[0].ownedByName.ToString();

                    objed.Verifiable = objed.AssetDetails[0].Verifiable;
                    objed.NonVerifiable = objed.AssetDetails[0].NonVerifiable;

                    objed.Barcode = (objed.AssetDetails[0].BarcodeIsMandatory == "Required") ? true : false;
                    objed.Quantified = (objed.AssetDetails[0].IsQuantified == "Yes") ? true : false;

                    objed.Mandatory = objed.AssetDetails[0].Mandatory;
                    objed.NonMantadatory = objed.AssetDetails[0].NonMantadatory;

                    objed.Movable = objed.AssetDetails[0].Movable;
                    objed.Immovable = objed.AssetDetails[0].Immovable;

                    objed.SLM = objed.AssetDetails[0].SLM;
                    objed.LPM = objed.AssetDetails[0].LPM;

                    objed.umonths = Convert.ToInt32(string.IsNullOrEmpty(objed.AssetDetails[0].umonths.ToString()) ? "0" : objed.AssetDetails[0].umonths.ToString());
                    objed.descRate = objed.AssetDetails[0].descRate.ToString();
                    objed.descGlCode = objed.AssetDetails[0].descGlCode.ToString();
                    objed.descResGlCode = objed.AssetDetails[0].descResGlCode.ToString();

                    objed.Status = "view";
                    TempData["modelview"] = objed;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return View("../AssetCodeMaster/NewAssetCodeMaster", objed);
            //return RedirectToAction("NewAssetCodeMaster", "AssetCodeMaster");
        }

        public JsonResult DeleteAssetDetails(string id)
        {
            string dresult = string.Empty;
            try
            {
                dresult = iry.DeleteAssetDetails(id);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json(dresult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGLDetails(string values)
        {
            string cobj = string.Empty;
            try
            {
                cobj = iry.GetGLDetails(values);
                string[] cobj1 = cobj.Split(',');
                return Json(cobj1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Gethsndesc(string hsngid)
        {
            string cobj = string.Empty;
            try
            {
                if(hsngid!=null)
                {

                    hsngid = hsngid.Replace("\"", "");
                    hsngid = hsngid.Replace("[", "");
                    hsngid = hsngid.Replace("]", "");
                cobj = iry.Gethsndesc(hsngid);
                    

                }
                return Json(cobj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult Create(Ifams_Property obj, string ddlSubcategory, string ddlclassication, string ddlcategory, string ownedByName)
        //{

        //    try
        //    {
        //        obj.classficationName = ddlclassication;
        //        obj.assetCategoryName = ddlcategory;
        //        obj.ownedByName = ownedByName;

        //        string check =  iry.InsertAssetDetails(obj);
        //        if (check == null)
        //        {
        //            RedirectToAction("AssetCodeMaster");
        //        }
        //        return Json(check, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}


        [HttpPost]
        public JsonResult Create(Ifams_Property DocNameModel, string hsn) 
        {

            try
            {

                if(hsn!=null)
                {
                    hsn = hsn.Replace("\"", "");
                    hsn = hsn.Replace("[", "");
                    hsn = hsn.Replace("]", "");
                }
                string check = iry.InsertAssetDetails(DocNameModel,hsn);
                string Result = "";
                if (check == null)
                {
                    RedirectToAction("AssetCodeMaster");
                }
                if (check == "success")
                {
                    Result = "success";
                }
                if (check != "success")
                {
                    Result = "Fail";
                }
                if (check == "Already Exist")
                {
                    Result = "Duplicate";
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult upadte(Ifams_Property DocNameModel, string hsn)
        {

            try
            {
                if (hsn != null)
                {
                    hsn = hsn.Replace("\"", "");
                    hsn = hsn.Replace("[", "");
                    hsn = hsn.Replace("]", "");
                }
                string check = iry.UpdateAssetDetails(DocNameModel, hsn);
                if (check == null)
                {
                    RedirectToAction("AssetCodeMaster");
                }
                if (check != "success")
                {
                    check = "Fail";
                }
                if(check=="success")
                {
                    check = "success";
                }
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            Ifams_Property objas = new Ifams_Property();
            DataSet ds = (DataSet)Session["ExcelExportAsset"];
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
