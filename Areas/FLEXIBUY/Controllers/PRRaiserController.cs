using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PRRaiserController : Controller
    {

        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IRepositoryKIR objrep;
        ErrorLog objErrorLog = new ErrorLog();

        public PRRaiserController()
            : this(new prsummodel())
        { }
        public PRRaiserController(IRepositoryKIR objm)
        {
            objrep = objm;
        }
        public ActionResult Index(PrHeader pr)
        {
            Session["sertemp"] = null;
            Session["protemp"] = null;
            Session["prattachment"] = null;
            Session["AccessTokenheader1"] = null;
            Session["AccessToken1"] = null;
            Session["attachment"] = null;
            Session["cbfAttachment"] = null;
            Session["cbfdetails"] = null;
            Session["count"] = null;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                int getbranchid = objrep.Getbranchgid();
                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchgid", "branchName", selectedValue: getbranchid);
                int getfcccgid = objrep.Getfcccgid();
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "FCCCgid", "FCCCName", selectedValue: getfcccgid);
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "Requestforgid", "RequestforName");
                //=Session["employee_name"] 
                //int id = 1;
                //string prefix = "PR";
                //string refno = objrep.genearteprrefno(id, prefix).ToString();
                //pr.prrefno = refno.ToString();
                pr.prDate = DateTime.Now.ToShortDateString();

                obj.prHead = pr;
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }

        }
        [HttpPost]
        public JsonResult productdetails(Product Product)
        {
            try
            {
                int data = 0;
                DataTable ObjDtpar = new DataTable();
                ObjDtpar = (DataTable)(Session["protemp"]);
                if (ObjDtpar != null)
                {
                    DataRow[] matches = ObjDtpar.Select("product_gid='" + Product.product_gid + "'");
                    data = matches.Count();
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult productdetailsservice(Product Product)
        {
            try
            {
                int data = 0;
                DataTable ObjDtpar = new DataTable();
                ObjDtpar = (DataTable)(Session["sertemp"]);
                //commend kulothungan 29-07-2015
                if (ObjDtpar != null)
                {
                    DataRow[] matches = ObjDtpar.Select("product_gid='" + Product.product_gid + "'");
                    data = matches.Count();
                }
                // data =Convert.ToInt32(Product.product_gid);
                Session["Productgid"] = Product.product_gid;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult updatepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";
                if (Session["protemp"] != null)
                {
                    temppro = (DataTable)Session["protemp"];

                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_code"].ToString() == product.product_Code.ToString().Trim())
                        {
                            temppro.Rows[i]["product_unit"] = product.productUnit_Gid;
                            temppro.Rows[i]["product_qty"] = product.product_Qty;

                        }
                    }

                }


                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                    obj.lstproduct = objrep.GetProduct(temppro);
                }


                return PartialView("InsertPro", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("InsertPro", obj);
            }
        }

        [HttpPost]
        public PartialViewResult deletepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";
                if (Session["protemp"] != null)
                {
                    temppro = (DataTable)Session["protemp"];

                    for (int i = 0; i < temppro.Rows.Count; i++)
                    {
                        if (temppro.Rows[i]["product_Code"].ToString() == product.product_Code.ToString().Trim())
                        {
                            temppro.Rows.RemoveAt(i);
                        }
                    }

                }


                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                    obj.lstproduct = objrep.GetProduct(temppro);
                }


                return PartialView("InsertPro", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("InsertPro", obj);
            }
        }



        [HttpPost]
        public PartialViewResult savepr(Product product)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                string tax = "Product";

                if (Session["protemp"] == null)
                {
                    int rowcount = 1;

                    temppro.Columns.Add("Srno");
                    temppro.Columns.Add("product_Group");
                    temppro.Columns.Add("product_gid");
                    temppro.Columns.Add("product_Code");
                    temppro.Columns.Add("product_Name");
                    temppro.Columns.Add("product_Description");
                    temppro.Columns.Add("product_Qty");
                    temppro.Columns.Add("product_Unit");
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid);
                    Session["protemp"] = temppro;

                }
                else
                {
                    temppro = (DataTable)Session["protemp"];
                    int rowcount = temppro.Rows.Count + 1;
                    temppro.Rows.Add(rowcount, product.prodservgrp_Gid, product.product_gid, product.product_Code, product.product_Name, product.product_Description, product.product_Qty, product.productUnit_Gid);

                }

                if (temppro.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetProduct(temppro);
                    obj.selectedValue = product.prodservgrp_Gid;
                    obj.selectedValue1 = product.productUnit_Gid;

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");


                }

                return PartialView("InsertPro", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("InsertPro", obj);
            }
        }

        [HttpPost]
        public PartialViewResult deletese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                string tax = "Service";
                if (Session["sertemp"] != null)
                {
                    tempserv = (DataTable)Session["sertemp"];

                    for (int i = 0; i < tempserv.Rows.Count; i++)
                    {
                        if (tempserv.Rows[i]["service_Code"].ToString() == service.service_Code.ToString().Trim())
                        {
                            tempserv.Rows.RemoveAt(i);
                        }
                    }

                }


                if (tempserv.Rows.Count > 0)
                {
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");

                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                    obj.lstproduct = objrep.GetProduct(tempserv);
                }


                return PartialView("InsertSer", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("InsertSer", obj);
            }
        }

        [HttpPost]
        public PartialViewResult savese(Product service)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable tempserv = new DataTable();
                string tax = "Service";

                if (Session["sertemp"] == null)
                {
                    int rowcount = 1;

                    tempserv.Columns.Add("Srno");
                    tempserv.Columns.Add("service_Group");
                    tempserv.Columns.Add("service_Code");
                    tempserv.Columns.Add("service_Name");
                    tempserv.Columns.Add("service_Description");
                    tempserv.Columns.Add("product_gid");
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);
                    Session["sertemp"] = tempserv;

                }
                else
                {
                    tempserv = (DataTable)Session["sertemp"];
                    int rowcount = tempserv.Rows.Count + 1;
                    tempserv.Rows.Add(rowcount, service.prodservgrp_Gid, service.service_Code, service.service_Name, service.service_Description, service.product_gid);
                    //praveen
                    Session["sertemp"] = tempserv;
                }

                if (tempserv.Rows.Count > 0)
                {
                
                    obj.lstproduct = objrep.GetService(tempserv);
                    obj.selectedValue = service.prodservgrp_Gid;


                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "ProductGroupgid", "ProductGroupName");
                }

                return PartialView("InsertSer", obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView("InsertSer", obj);
            }
        }


        [HttpPost]
        public PartialViewResult InsertPro(Product id1)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                string tax = "Product";


                if (id1.product_Code != null)
                {
                    obj.lstproduct = objrep.GetSelectedProduct(id1);
                    obj.productGroupGid = Convert.ToInt32(HttpContext.Session["ProductGroupgid"]);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName", obj.productGroupGid);
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");
                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.ddlUom = new SelectList(objrep.GetUomList(), "uomGid", "uomCode");

                    obj.lstproduct = objrep.GetSelectedProduct(id1);

                }

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        [HttpPost]
        public PartialViewResult InsertSer(Product id)
        {
            DataTable tempser = new DataTable();
            PrSumEntity obj = new PrSumEntity();
            try
            {
                string tax = "Service";



                if (id.service_Code != null)
                {
                    obj.lstproduct = objrep.GetSelectedService(id);
                    obj.productGroupGid = Convert.ToInt32(HttpContext.Session["ProductGroupgid"]);
                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName", obj.productGroupGid);


                }
                else
                {

                    obj.ddlProductGroup = new SelectList(objrep.GetProductGroupList(tax), "productGroupGid", "productGroupName");
                    obj.lstproduct = objrep.GetSelectedService(id);

                }

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(obj);
            }
        }

        //[HttpGet]
        //public PartialViewResult getprodlist(int id, string listfor = null)
        //{

        //    PrSumEntity objPr = new PrSumEntity();
        //    try
        //    {
        //        if (listfor == null)
        //        {
        //            objPr = objrep.GetProdServDetails(id);
        //            HttpContext.Session["ProductGroupgid"] = id;
        //        }
        //        else
        //        {
        //            List<Product> objEmp = new List<Product>();
        //            if (Session["SearchProd"] != null)
        //            {
        //                objEmp = (List<Product>)Session["SearchProd"];
        //                objPr.lstproduct = objEmp;
        //                HttpContext.Session["ProductGroupgid"] = id;
        //            }

        //        }

        //        return PartialView("getprodlist", objPr);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return PartialView("getprodlist", objPr);
        //    }
        //}

        [HttpGet]
        public PartialViewResult getprodlist(int id,string listfor = null)
        {
            // List<Employee_gid> objowner = new List<Employee_gid>();
            PrSumEntity objPr = new PrSumEntity();
            if (listfor == "search")
            {
                if (Session["SearchProd"] != null)
                    objPr.lstproduct = (List<Product>) Session["SearchProd"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["prodcode"] != null)
                    TempData["code"] = TempData["prodcode"];
                if (TempData["prodname"] != null)
                    TempData["name"] = TempData["prodname"];
            }
            else
            {
                Session["SearchProd"] = "";
                objPr = objrep.GetProdServDetails(id);
            }
            return PartialView("getprodlist", objPr);
        }
        //[HttpPost]
        //public JsonResult SearchProd(Product objSearchProd)
        //{
        //    try
        //    {
        //        int id = Convert.ToInt32(objSearchProd.product_gid);
        //        List<Product> objEmp = new List<Product>();
        //        PrSumEntity objPr = new PrSumEntity();
        //        objPr = objrep.GetProdServDetails(id);
        //        objEmp = objPr.lstproduct;
        //        if (objPr != null)
        //        {
        //            if ((string.IsNullOrEmpty(objSearchProd.product_Code)) == false)
        //            {
        //                objEmp = objEmp.Where(x => objSearchProd.product_Code == null ||
        //                    (x.product_Code.ToUpper().Contains(objSearchProd.product_Code.ToUpper()))).ToList();
        //                Session["SearchProd"] = objEmp;
        //            }
        //            if ((string.IsNullOrEmpty(objSearchProd.product_Name)) == false)
        //            {
        //                objEmp = objEmp.Where(x => objSearchProd.product_Name == null ||
        //                    (x.product_Name.ToUpper().Contains(objSearchProd.product_Name.ToUpper()))).ToList();
        //                Session["SearchProd"] = objEmp;
        //            }
        //        }
        //        return Json(objEmp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw ex;
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json(0, JsonRequestBehavior.AllowGet);
        //    }


        //}

        [HttpPost]
        public PartialViewResult SearchProd(Product objSearchProd)
        {
            try
            {
                int id = Convert.ToInt32(objSearchProd.product_gid);
               // List<Product> objEmp = new List<Product>();
                PrSumEntity objPr = new PrSumEntity();
                objPr = objrep.GetProdServDetails(id);
                if (objPr != null)
                {
                    if ((string.IsNullOrEmpty(objSearchProd.product_Code)) == false)
                    {
                        TempData["prodcode"] = objSearchProd.product_Code;
                        objPr.lstproduct = objPr.lstproduct.Where(x => objSearchProd.product_Code == null ||
                            (x.product_Code.ToUpper().Contains(objSearchProd.product_Code.ToUpper()))).ToList();
                        Session["SearchProd"] = objPr.lstproduct;
                    }
                    if ((string.IsNullOrEmpty(objSearchProd.product_Name)) == false)
                    {
                        TempData["prodname"] = objSearchProd.product_Name;
                        objPr.lstproduct = objPr.lstproduct.Where(x => objSearchProd.product_Name == null ||
                            (x.product_Name.ToUpper().Contains(objSearchProd.product_Name.ToUpper()))).ToList();
                        Session["SearchProd"] = objPr.lstproduct;
                    }
                    if (objPr.lstproduct.Count == 0)
                    {
                        TempData["Norecords"] = "No records Found";
                    }
                }
                return PartialView("getprodlist", objPr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet]
        //public PartialViewResult getservlist(int id)
        //{

        //    PrSumEntity objM = new PrSumEntity();
        //    try
        //    {
        //        objM = objrep.GetProdServDetails(id);
        //        HttpContext.Session["ProductGroupgid"] = id;
        //        return PartialView(objM);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return PartialView(objM);
        //    }
        //}

        [HttpGet]
        public PartialViewResult getservlist(int id, string listfor = null)
        {
            PrSumEntity objPr = new PrSumEntity();
            if (listfor == "search")
            {
                if (Session["SearchServ"] != null)
                    objPr.lstproduct = (List<Product>)Session["SearchServ"];
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["servcode"] != null)
                    TempData["code"] = TempData["servcode"];
                if (TempData["servname"] != null)
                    TempData["name"] = TempData["servname"];
            }
            else
            {
                Session["SearchServ"] = "";
                objPr = objrep.GetProdServDetails(id);
            }
            return PartialView("getservlist", objPr);
        }

        [HttpPost]
        public PartialViewResult SearchService(Product objSearchProd)
        {
            try
            {
                int id = Convert.ToInt32(objSearchProd.product_gid);
                PrSumEntity objPr = new PrSumEntity();
                objPr = objrep.GetProdServDetails(id);
                if (objPr != null)
                {
                    if ((string.IsNullOrEmpty(objSearchProd.service_Code)) == false)
                    {
                        TempData["servcode"] = objSearchProd.service_Code;
                        objPr.lstproduct = objPr.lstproduct.Where(x => objSearchProd.service_Code == null ||
                            (x.service_Code.ToUpper().Contains(objSearchProd.service_Code.ToUpper()))).ToList();
                        Session["SearchServ"] = objPr.lstproduct;
                    }
                    if ((string.IsNullOrEmpty(objSearchProd.service_Name)) == false)
                    {
                        TempData["servname"] = objSearchProd.service_Name;
                        objPr.lstproduct = objPr.lstproduct.Where(x => objSearchProd.service_Name == null ||
                            (x.service_Name.ToUpper().Contains(objSearchProd.service_Name.ToUpper()))).ToList();
                        Session["SearchServ"] = objPr.lstproduct;
                    }
                    if (objPr.lstproduct.Count == 0)
                    {
                        TempData["Norecords"] = "No records Found";
                    }
                }
                return PartialView("getservlist", objPr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetProductGroup()
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                return Json(objrep.GetProductGroupList("Product"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetServiceGroup()
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                return Json(objrep.GetProductGroupList("Service"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getuom()
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                return Json(objrep.GetUomList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult saveprraiser(PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();
                //int headegid = 0;
                //string Result = string.Empty;
                if (Session["protemp"] != null)
                {
                    temppro = (DataTable)Session["protemp"];

                    lstpro = objrep.GetProduct(temppro);
                    obj = objrep.InsertPrheader(pr, "Save");
                    //headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.InsertPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);
                    //Result = objrep.Insertqueue(headegid,"PIP");

                }
                if (Session["sertemp"] != null)
                {
                    tempser = (DataTable)Session["sertemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.InsertPrheader(pr, "Save");
                    //headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.InsertPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);
                    //Result = objrep.Insertqueue(headegid,"PIP");

                }

                if (Session["prattachment"] != null)
                {
                    PrSumEntity attachLst = new PrSumEntity();
                    attachLst = (PrSumEntity)Session["prattachment"];

                    obj = objrep.InsertAttachment(obj.prHead.prRefNo, "PR", attachLst.attachment);

                }
                return Json("Index", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Index", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult submitprraiser(PrHeader pr)
        {
            try
            {
                string Result = string.Empty;
                PrSumEntity obj = new PrSumEntity();
                PrSumEntity obj1 = new PrSumEntity();
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();
                int headegid = 0;

                if (Session["protemp"] != null)
                {
                    temppro = (DataTable)Session["protemp"];

                    lstpro = objrep.GetProduct(temppro);
                    obj = objrep.InsertPrheader(pr, "Submit");
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.InsertPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);
                    Result = objrep.Insertqueue(headegid, pr.requestForName, pr);
                }
                if (Session["sertemp"] != null)
                {
                    tempser = (DataTable)Session["sertemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.InsertPrheader(pr, "Submit");
                    headegid = Convert.ToInt32(obj.prDet.prDet_PrHeader_Gid);
                    obj = objrep.InsertPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);
                    Result = objrep.Insertqueue(headegid, pr.requestForName, pr);
                }


                if (Session["prattachment"] != null)
                {
                    PrSumEntity attachLst = new PrSumEntity();
                    attachLst = (PrSumEntity)Session["prattachment"];

                    obj = objrep.InsertAttachment(obj.prHead.prRefNo, "PR", attachLst.attachment);

                }

                //string mail = Session["PRqueuegid"].ToString();
                //mailsender.sendusermail("FB", "PR", mail, "S", "0");
                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                int refgid = objMail.GetRefGidForMail("PR");
                string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");

                Session["AccessToken1"] = null;
                Session["AccessTokenheader1"] = null;
                return Json("Index", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Index", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public PartialViewResult PRattach()
        {
            PrSumEntity objMAttachment = new PrSumEntity();
            try
            {


                string prattchmnet = "";
                prsummodel objrep1 = new prsummodel();


                if (prattchmnet == "")
                {
                    //objMAttachment.documentName = prattchmnet;
                    //--------------------------subburaj--------------17-08-2015
                    objMAttachment = (PrSumEntity)Session["attachment"];
                    //objMAttachment = objrep1.Attachmentname(objMAttachment);
                }
                else
                {
                    //objMAttachment.documentName = prattchmnet;
                    objMAttachment = objrep1.Attachmentname(objMAttachment);
                }
                objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objMAttachment);
            }
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {
            //try
            //{
            //    string filename = "";
            //    foreach (string file in Request.Files)
            //    {
            //        var fileContent = Request.Files[file];

            //        if (fileContent != null && fileContent.ContentLength > 0)
            //        {

            //            if (fname != null && fname.Trim() != "")
            //            {
            //                filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
            //            }
            //            else
            //            {
            //                filename = objCmnFunctions.GetSequenceNo("PR");
            //            }



            //            var fileextension = Path.GetExtension(fileContent.FileName);
            //            var stream = fileContent.InputStream;
            //            var path = Path.Combine(@"C:/temp/", filename + fileextension);
            //            using (var fileStream = System.IO.File.Create(path))
            //            {
            //                stream.CopyTo(fileStream);
            //            }
            //            filename = filename + fileextension;
            //        }
            //    }
            //    return Json(filename);
            //}
            //catch (Exception)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("Upload failed");
            //}

            //DataTable dtac = new DataTable();
            //dtac.Columns.Add("Attachid");
            //dtac.Columns.Add("AttachName");
            //dtac.Columns.Add("AttachFileType");
            //dtac.Columns.Add("Attachlength");
            //dtac.Columns.Add("flag");
            //int j = 1;
            string filename = "";
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["MyFile"];
                if (myFile != null && myFile.ContentLength != 0)
                {

                    string pathForSaving = HttpContext.Application["path"] as string; ;
                    //if (this.CreateFolderIfNeeded(pathForSaving))
                    //{

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        // filename = filename.Substring(0, filename.LastIndexOf(".") + 0);
                        filename = objCmnFunctions.GetSequenceNo("PR");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var path = Path.Combine(HoldFileUploadUrlDSA(), filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filename"] = filename;
                        //myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        //myFile.SaveAs(Path.Combine(@"C:\temp\", myFile.FileName));

                        //DataRow dr = dtac.NewRow();
                        //dr["Attachid"] = j.ToString();
                        ////   dr["AttachName"] = Request.Files[file].FileName.ToString();
                        //dr["AttachName"] = Path.GetFileName(Request.Files["MyFile"].FileName);
                        //dr["AttachFileType"] = Request.Files["MyFile"].ContentType.ToString();
                        //dr["Attachlength"] = Request.Files["MyFile"].ContentLength.ToString();
                        //dr["flag"] = 0;

                        //dtac.Rows.Add(dr);



                        isUploaded = true;
                        // message = "File uploaded successfully!";
                        // message = Path.GetFileName(Request.Files["MyFile"].FileName);
                        message = filename;
                        //if (Session["supplierattmtfileDe"] != null)
                        //{
                        //    int a = 1;
                        //    DataTable dtprevalue = new DataTable();
                        //    dtprevalue = (DataTable)Session["supplierattmtfileDe"];
                        //    for (int k = 0; dtprevalue.Rows.Count > k; k++)
                        //    {
                        //        DataRow dr1 = dtac.NewRow();
                        //        dr1["Attachid"] = dtac.Rows.Count + 1;
                        //        dr1["AttachName"] = dtprevalue.Rows[0]["AttachName"].ToString();
                        //        dr1["AttachFileType"] = dtprevalue.Rows[0]["AttachFileType"].ToString();
                        //        dr1["Attachlength"] = dtprevalue.Rows[0]["Attachlength"].ToString();
                        //        dr1["flag"] = 1;
                        //        dtac.Rows.Add(dr1);
                        //        a++;
                        //    }

                        //}
                        //Session["supplierattmtfileDe"] = dtac;

                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                    //}
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }

        [HttpPost]
        public JsonResult PRattachgrid(PrSumEntity attachmodel)
        {

            try
            {
                PrSumEntity attachment1 = new PrSumEntity();
                try
                {
                    if (ModelState.IsValid)
                    {
                        attachment1 = objrep.PRAttachmentname(attachmodel);
                        Session["prattachment"] = attachment1.attachment;
                    }
                    return Json(attachment1, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    return Json(attachment1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult PRattachgrid()
        {
            try
            {
                PrSumEntity obj = new PrSumEntity();
                try
                {
                    obj = (PrSumEntity)Session["prattachment"];

                    return PartialView(obj);
                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    return PartialView(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult Downloaded(PrSumEntity prattachmentmodel)
        {
            Session["downfile"] = prattachmentmodel.attachment1;
            PrSumEntity obj = new PrSumEntity();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public FileResult Downloadpr(string ff)
        {

            string txt1 = Session["downfile"].ToString();
            string directory = HoldFileUploadUrlDSA() + txt1.ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            string fileName = "Download" + txt1.ToString();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public JsonResult DeleteAttachment(PrSumEntity obj)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            prsummodel objRep = new prsummodel();
            PrSumEntity attachment1 = new PrSumEntity();
            try
            {
                PrSumEntity rr = new PrSumEntity();

                rr = (PrSumEntity)Session["prattachment"];


                if (Session["prattachment"] != null)
                {
                    int rowcount = 1;
                    dt.Columns.Add("Sno");
                    //dt.Columns.Add("Filename");
                    //dt.Columns.Add("AttachmentDate");
                    //dt.Columns.Add("AttachmentDescription");
                    dt.Columns.Add("Documnet_Name");
                    dt.Columns.Add("Attachment_Date");
                    dt.Columns.Add("Document_des");
                    for (int i = 0; i < rr.attachment.Count; i++)
                    {
                        dt.Rows.Add(rowcount, rr.attachment[i].fileName, rr.attachment[i].attachedDate, rr.attachment[i].description);
                        rowcount = rowcount + 1;
                    }
                    string s = Session["cbfdetails"].ToString();
                    if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
                    {
                        Session["cbfdetails"] = "";
                    }
                    else
                    {
                        Session["cbfdetails"] = "";
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // if (dt.Rows[i]["Filename"].ToString() == obj.attachment1)
                        if (dt.Rows[i]["Documnet_Name"].ToString() == obj.attachment1)
                        {
                            dt.Rows.RemoveAt(i);
                            attachment1.attachid = Session["cbfdetails"].ToString();
                        }
                        dt.AcceptChanges();
                    }

                    if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
                    {
                        Session["AccessToken1"] = dt;
                        Session["cbfdetails"] = "";
                    }
                    else
                    {
                        Session["AccessTokenheader1"] = dt;
                        Session["cbfdetails"] = "";
                    }

                    attachment1 = objRep.Attachmentname(attachment1);
                    attachment1.attachid = Session["cbfdetails"].ToString();


                    List<Attachment> lst = objrep.EditPRAttachmentList(dt);
                    attachment1.attachment = lst;
                    if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
                    {
                        Session["attachment"] = attachment1;
                        if (attachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachmentdetails"] = attachment1;
                        if (attachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    Session["prattachment"] = attachment1;

                }



                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(attachment1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DownloadDocument(CbfSumEntity obj)
        {
            Session["downfile"] = obj.attachment1;
            CbfSumEntity obj1 = new CbfSumEntity();
            return Json(obj1, JsonRequestBehavior.AllowGet);
        }
        public FileResult Download(CbfSumEntity obj)
        {

            try
            {
                string txt1 = Session["downfile"].ToString();
                string directory = HoldFileUploadUrlDSA() + txt1.ToString();
                byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                string fileName = "Download" + txt1.ToString();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public PartialViewResult BoqAttachGrid()
        {
            PrSumEntity objAttachment = new PrSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";


                if (Session["cbfdetails"].ToString() == "")
                {
                    objAttachment = (PrSumEntity)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                else
                {
                    objAttachment = (PrSumEntity)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objAttachment);
            }
        }
        [HttpPost]
        public JsonResult BoqAttachGrid(PrSumEntity objAttachment)
        {
            PrSumEntity objAttachment1 = new PrSumEntity();
            try
            {
                ViewBag.NoRecordsFound = "";

                if (ModelState.IsValid)
                {
                    objAttachment.attachid = Session["cbfdetails"].ToString();
                    if (objAttachment.attachid == "IEM.Areas.FLEXIBUY.Models.PrSumEntity")
                    {
                        objAttachment.attachid = "";
                    }
                    prsummodel objRep = new prsummodel();
                    objAttachment1 = objRep.Attachmentname(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }
        }

    }


}