using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Web.Helpers;
using IEM.Common;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using IEM.App_Start;
 

namespace IEM.Areas.MASTERS.Controllers
{

     
     [NoDirectAccess]
    public class FbProductServiceController : Controller
    {
        //
        // GET: /Product/
        private dbLib db = new dbLib();
        ErrorLog objErrorLog = new ErrorLog();
        private FbIrepository objModel;
        public FbProductServiceController()
            : this(new FbDataModel())
        {

        }
        public FbProductServiceController(FbIrepository objM)
        {
            objModel = objM;
        }
        //GET:/Product/Create
        [HttpGet]
        public ActionResult Index()
        {
            Session["ExcelExportProduct"] = null;
            FbEntity obj = new FbEntity();
            obj.lListProdService = objModel.GetProductServiceDetails().ToList();
            if (Request.IsAjaxRequest())
            {
                if (Session["records"] != null)
                {
                    obj.lListProdService = (List<FbEntity>)Session["records"];
                }
            }
            if (obj.lListProdService.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }

            ViewBag.parent = "--Select Parent--";
            ViewBag.Type = "--Select Type--";
            ViewBag.CategoryType = "--Select Category--";
            return View(obj);

        }
        [HttpPost]
        public ActionResult Index(string txtproduct, string txtproductname, string command, string parent, string Type, string CategoryType)
        {
            FbEntity obj = new FbEntity();
            obj.lListProdService = objModel.GetProductServiceDetails().ToList();
            ViewBag.parent = string.Empty;
            ViewBag.Type = string.Empty;
            ViewBag.CategoryType = string.Empty;
            Session["records"] = null;
            if (command == "search")
            {
                if ((string.IsNullOrEmpty(txtproduct)) == false)
                {
                    ViewBag.txtproduct = txtproduct;
                    obj.lListProdService = obj.lListProdService.Where(x => txtproduct == null ||
                       (x.productCode.ToUpper().Contains(txtproduct.ToUpper()))).ToList();
                    Session["records"] = obj.lListProdService;
                }
                if (parent != "--Select Parent--")
                {
                    ViewBag.parent = parent;
                    obj.lListProdService = obj.lListProdService.Where(x => parent == null ||
                        (x.parentFlag.Contains(parent))).ToList();
                    Session["records"] = obj.lListProdService;
                }
                else
                {
                    ViewBag.parent = "--Select Parent--";
                }
                if (Type != "--Select Type--")
                {
                    ViewBag.Type = Type;
                    obj.lListProdService = obj.lListProdService.Where(x => Type == null ||
                        (x.productServflag.Contains(Type))).ToList();
                    Session["records"] = obj.lListProdService;
                }
                else
                {
                    ViewBag.Type = "--Select Type--";
                }
                if (CategoryType != "--Select Category--")
                {
                    ViewBag.CategoryType = CategoryType;
                    obj.lListProdService = obj.lListProdService.Where(x => CategoryType == null ||
                        (x.productService.Contains(CategoryType))).ToList();
                    Session["records"] = obj.lListProdService;
                }
                else
                {
                    ViewBag.CategoryType = "--Select Category--";
                }
                if ((string.IsNullOrEmpty(txtproductname)) == false)
                {
                    ViewBag.txtproductname = txtproductname;
                    obj.lListProdService = obj.lListProdService.Where(x => txtproductname == null ||
                        (x.productName.ToUpper().Contains(txtproductname.ToUpper()))).ToList();
                    Session["records"] = obj.lListProdService;
                }
                if (obj.lListProdService.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
                if (command != "search")
                {
                    obj.lListProdService = (List<FbEntity>)Session["records"];
                }
            }
            return View(obj);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            FbEntity objDetails = new FbEntity();
            objDetails.productList = new SelectList(objModel.GetProductList(), "parentProductGid", "parentProductName");
            objDetails.serviceList = new SelectList(objModel.GetServiceList(), "serviceGid", "serviceName");
            //objDetails.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory");
            //objDetails.expenseList = new SelectList(objModel.GetExpenseCategory(), "expenseGid", "expenseCategory");
            objDetails.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory");
            objDetails.ExpNatureList = new SelectList(objModel.GetExpNatureList(), "ExpNatureGid", "ExpNature");
            return PartialView(objDetails);

        }
        //Getting glcode based on Selecting Asset or Expense Category

        public JsonResult Assetglcode(string id)
        {
            try
            {
                FbEntity objDetails = new FbEntity();
                objDetails.assetGid = Convert.ToInt32(id);
                if (objDetails.assetGid != null)
                {
                    objDetails.glCode = objModel.GetAssetGl(objDetails.assetGid);
                }
                return Json(objDetails.glCode, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult Expenseglcode(string id)
        {
            try
            {
                FbEntity objDetails = new FbEntity();
                objDetails.expenseGid = Convert.ToInt32(id);
                if (objDetails.expenseGid != null)
                {
                    objDetails.glCode = objModel.GetExpenseGl(objDetails.expenseGid);
                }
                return Json(objDetails.glCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [HttpPost]
        public JsonResult Create(FbEntity objDetails)
        {
            try
            {
                objDetails.result = objModel.InsertProductServiceDetails(objDetails);
                if (objDetails.result != null)
                {
                    return Json(objDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
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
            FbEntity obj = new FbEntity();
            List<FbEntity> objDetails = new List<FbEntity>();
            obj.productId = id;
            objDetails = objModel.GetProductServiceDetailsById(id);
            obj.productGid = objDetails[0].productGid;
            obj.serviceGid = obj.productGid;
            Session["gid"] = obj.productGid;
            obj.productServflag = objDetails[0].productServflag.ToString();
            Session["productService"] = obj.productServflag;
            obj.productCode = objDetails[0].productCode.ToString();
            obj.service = obj.productCode;
            obj.productName = objDetails[0].productName.ToString();
            obj.serviceName = obj.productName;
            obj.productDescription = objDetails[0].productDescription.ToString();
            obj.serviceDescription = obj.productDescription;
            obj.category = objDetails[0].category.ToString();
            Session["Category"] = obj.category;
            obj.parentFlag = objDetails[0].parentFlag.ToString();
            Session["parent"] = obj.parentFlag;
            obj.parentProduct = objDetails[0].parentProduct.ToString();
            obj.productParentGid = Convert.ToInt32(objDetails[0].productParentGid.ToString());
            obj.serviceParentGid = obj.productParentGid;
            obj.assetCategory = objDetails[0].assetCategory.ToString();
            obj.expenseCategory = obj.assetCategory;
            obj.assetGid = objDetails[0].assetGid;
            obj.expenseGid = obj.assetGid;
            obj.glCode = objDetails[0].glCode.ToString();
            if (obj.productParentGid == 0)
            {
                obj.productList = new SelectList(objModel.GetProductListUpd(), "productParentGid", "parentProductName");
            }
            else
            {
                obj.productList = new SelectList(objModel.GetProductListUpd(), "productParentGid", "parentProductName", obj.productParentGid);
            }
            if (obj.serviceParentGid == 0)
            {
                obj.serviceList = new SelectList(objModel.GetServiceListUpd(), "serviceParentGid", "serviceName");
            }
            else
            {
                obj.serviceList = new SelectList(objModel.GetServiceListUpd(), "serviceParentGid", "serviceName", obj.serviceParentGid);
            }


            if (obj.category == "Asset")
            {
                obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory", obj.assetGid);
                obj.expenseGid = 0;
            }
            else
            {
                obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory");
            }
            if (obj.category == "Expense")
            {
                obj.expenseList = new SelectList(objModel.GetExpenseCategory(), "expenseGid", "expenseCategory", obj.expenseGid);
                obj.assetGid = 0;
            }
            else
            {
                obj.expenseList = new SelectList(objModel.GetExpenseCategory(), "expenseGid", "expenseCategory");
            }
            return PartialView(obj);
        }

        [HttpPost]
        public JsonResult Edit(FbEntity objDetails)
        {
            try
            {
                objDetails.result = objModel.UpdateProductServiceDetails(objDetails);
                if (objDetails.result != null)
                {

                    return Json(objDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult UpdateNew(FbEntity objDetails)
        {
            try
            {
                objDetails.result = objModel.UpdateProdServNew(objDetails);
                if (objDetails.result != null)
                {

                    return Json(objDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EditNew(int id, string viewfor)
        {
            FbEntity obj = new FbEntity();
            try
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

                List<FbEntity> objDetails = new List<FbEntity>();
                obj.productId = id;
                objDetails = objModel.GetProductServiceDetailsById(id);
                obj.productGid = objDetails[0].productGid;
                obj.serviceGid = obj.productGid;
                Session["gid"] = obj.productGid;
                obj.productServflag = objDetails[0].productServflag.ToString();
                Session["productService"] = obj.productServflag;
                obj.productCode = objDetails[0].productCode.ToString();
                obj.service = obj.productCode;
                obj.productName = objDetails[0].productName.ToString();
                obj.serviceName = obj.productName;
                obj.productDescription = objDetails[0].productDescription.ToString();
                obj.serviceDescription = obj.productDescription;
                obj.category = objDetails[0].category.ToString();
                Session["Category"] = obj.category;
                obj.parentFlag = objDetails[0].parentFlag.ToString();
                Session["parent"] = obj.parentFlag;
                obj.parentProduct = objDetails[0].parentProduct.ToString();
                obj.productParentGid = Convert.ToInt32(objDetails[0].productParentGid.ToString());
                obj.serviceParentGid = obj.productParentGid;
                // obj.assetCategory = objDetails[0].assetCategory.ToString();
                //  obj.expenseCategory = obj.assetCategory;
                obj.assetGid = objDetails[0].assetGid;
                obj.expenseGid = obj.assetGid;
                obj.glCode = objDetails[0].glCode.ToString();

                obj.SelectedAssetCatGid = objDetails[0].SelectedAssetCatGid;
                obj.SelectedAssetSubCatGid = objDetails[0].SelectedAssetSubCatGid;
                obj.SelectedExpGid = objDetails[0].SelectedExpGid;
                obj.SelectedExpNatureGid = objDetails[0].SelectedExpNatureGid;

                if (obj.productParentGid == 0)
                {
                    obj.productList = new SelectList(objModel.GetProductListUpd(), "productParentGid", "parentProductName");
                }
                else
                {
                    obj.productList = new SelectList(objModel.GetProductListUpd(), "productParentGid", "parentProductName", obj.productParentGid);
                }
                if (obj.serviceParentGid == 0)
                {
                    obj.serviceList = new SelectList(objModel.GetServiceListUpd(), "serviceParentGid", "serviceName");
                }
                else
                {
                    obj.serviceList = new SelectList(objModel.GetServiceListUpd(), "serviceParentGid", "serviceName", obj.serviceParentGid);
                }


                if (obj.category == "Asset")
                {
                    obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory", obj.SelectedAssetCatGid);
                    if (obj.SelectedAssetSubCatGid == 0)
                    {
                        obj.AssetSubCatList = new SelectList(objModel.GetAssetSubCatList(obj.SelectedAssetCatGid), "AssetSubCatGid", "AssetSubCat");
                    }
                    else
                    {
                        obj.AssetSubCatList = new SelectList(objModel.GetAssetSubCatList(obj.SelectedAssetCatGid), "AssetSubCatGid", "AssetSubCat", obj.SelectedAssetSubCatGid);
                    }

                    obj.ExpNatureList = new SelectList(objModel.GetExpNatureList(), "ExpNatureGid", "ExpNature");
                    obj.expenseList = new SelectList(Enumerable.Empty<SelectListItem>());
                    //   obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory", obj.assetGid);
                    obj.expenseGid = 0;
                }
                else
                {
                    obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory");
                }
                if (obj.category == "Expense")
                {
                    obj.AssetSubCatList = new SelectList(Enumerable.Empty<SelectListItem>());
                    obj.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory");
                    obj.ExpNatureList = new SelectList(objModel.GetExpNatureList(), "ExpNatureGid", "ExpNature", obj.SelectedExpNatureGid);
                    obj.expenseList = new SelectList(objModel.GetExpenseCatList(obj.SelectedExpNatureGid), "expenseGid", "expenseCategory", obj.SelectedExpGid);
                    obj.assetGid = 0;
                }
                else
                {
                    obj.expenseList = new SelectList(objModel.GetExpenseCategory(), "expenseGid", "expenseCategory");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return PartialView(obj);
        }


        [HttpPost]
        public JsonResult Delete(FbEntity objdel)
        {
            try
            {


                objdel.parentProductGid = Convert.ToInt32(objModel.GetParentProductGid(objdel.productGid));
                if (objdel.parentProductGid == null || objdel.parentProductGid == 0)
                {
                    objModel.DeleteProductServiceDetails(objdel);
                    return Json(objdel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetCategory(string id)
        {
            try
            {
                FbEntity objDetails = new FbEntity();
                objDetails.productParentGid = Convert.ToInt32(id);
                if (objDetails.productParentGid != null)
                {
                    DataSet value = objModel.GetCategory(objDetails.productParentGid);
                    for(int i = 0; i < value.Tables[0].Rows.Count; i++)
                    {
                        objDetails.category = value.Tables[0].Rows[0]["prodservice_category"].ToString();
                        objDetails.glCode = value.Tables[0].Rows[0]["prodservice_gl_no"].ToString();
                        if (objDetails.category == "A")
                        {
                            if (value.Tables[0].Rows[0]["prodservice_assetcategory_gid"] != null)
                            {
                                objDetails.categoryList = new SelectList(objModel.GetAssetCategory(), "assetGid", "assetCategory", Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_assetcategory_gid"]));
                                if (value.Tables[0].Rows[0]["prodservice_assetsubcategory_gid"] != null && Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_assetsubcategory_gid"]) != 0)
                                {
                                    objDetails.AssetSubCatList = new SelectList(objModel.GetAssetSubCatList(Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_assetcategory_gid"])), "AssetSubCatGid", "AssetSubCat", Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_assetsubcategory_gid"]));
                                }
                                else
                                {
                                    objDetails.AssetSubCatList = new SelectList(objModel.GetAssetSubCatList(Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_assetcategory_gid"])), "AssetSubCatGid", "AssetSubCat");
                                }

                            }
                        }
                        if (objDetails.category == "E")
                        {
                            if (value.Tables[0].Rows[0]["prodservice_expcatcategory_gid"] != null)
                            {
                                objDetails.ExpNatureList = new SelectList(objModel.GetExpNatureList(), "ExpNatureGid", "ExpNature", Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_expcatcategory_gid"]));                                
                                if (value.Tables[0].Rows[0]["prodservice_expsubcategory_gid"] != null && Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_expsubcategory_gid"]) != 0)
                                {
                                    objDetails.expenseList = new SelectList(objModel.GetExpenseCatList(Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_expcatcategory_gid"])), "expenseGid", "expenseCategory", Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_expsubcategory_gid"]));
                                }
                                else
                                    objDetails.expenseList = new SelectList(objModel.GetExpenseCatList(Convert.ToInt16(value.Tables[0].Rows[0]["prodservice_expcatcategory_gid"])), "expenseGid", "expenseCategory");
                            }
                        }                                                
                    }
                }
                return Json(objDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetAssetSubCat(FbEntity objAssetCat)
        {
            try
            {
                var AssetCatId = objAssetCat.SelectedAssetCatGid;
                return Json(objModel.GetAssetSubCatList(AssetCatId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<FbEntity> lst = new List<FbEntity>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetExpenseCat(FbEntity objAssetCat)
        {
            try
            {
                var AssetCatId = objAssetCat.SelectedExpNatureGid;
                return Json(objModel.GetExpenseCatList(AssetCatId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<FbEntity> lst = new List<FbEntity>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {

            DataSet ds = (DataSet)Session["ExcelExportProduct"];
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
