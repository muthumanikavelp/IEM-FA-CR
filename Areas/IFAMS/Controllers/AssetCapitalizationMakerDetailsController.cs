using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using System.Data;
using IEM.Common;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.EOW.Models;
using System.Text;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetCapitalizationMakerDetailsController : Controller
    {
        private Ifams_Repository irr;
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions cmnfunc = new CmnFunctions();
        public AssetCapitalizationMakerDetailsController()
            : this(new Ifams_Model())
        {

        }
        public AssetCapitalizationMakerDetailsController(Ifams_Repository irry)
        {
            irr = irry;
        }

        //public ActionResult Assetcap_mak_details(int id)
        //{
        //    capitalizationMaker objecf = new capitalizationMaker();
        //    List<capitalizationMaker> objl = new List<capitalizationMaker>();
        //    DataTable dt = new DataTable();
        //    Session["Ecfgid"] = id;
        //    try
        //    {
        //        dt = irr.Getinvoice(id);
        //        if (dt.Rows.Count > 0)
        //        {

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                objecf = new capitalizationMaker();
        //                objecf.Ecfgid = Convert.ToInt32(dt.Rows[0]["Ecfgid"].ToString());
        //                objecf.EcfNo = dt.Rows[0]["EcfNo"].ToString();
        //                objecf.Ecfdate = dt.Rows[0]["EcfDate"].ToString();
        //                objecf.EcfAmount = dt.Rows[0]["EcfAmount"].ToString();
        //                objecf.Vendor = dt.Rows[0]["VendorName"].ToString();
        //                objecf.invoicegid = dt.Rows[i]["Invoicegid"].ToString();
        //                objecf.invoiceno = dt.Rows[i]["InvoiceNo"].ToString();
        //                objecf.invoiceamount = Convert.ToDecimal(dt.Rows[i]["InvoiceAmount"].ToString());
        //                objecf.invoicedate = dt.Rows[i]["InvoiceDate"].ToString();
        //                objl.Add(objecf);                        
        //            }

        //            objecf.InvoiceList = objl;
        //            objecf.Podetailslist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Itemlevellist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Capitalizationgridlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            irr.deletetemp(objecf.Ecfgid.ToString());
        //        }
        //        else
        //        {
        //            objecf.InvoiceList = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>(); ;
        //            objecf.Podetailslist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Itemlevellist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Capitalizationgridlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //            objecf.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return View(objecf);
        //}

        [HttpPost]
        public ActionResult Assetcap_mak_details(capitalizationMaker objin)
        {
            try
            {

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return RedirectToAction("");
        }

        [HttpPost]
        public PartialViewResult AssetPoDetails(capitalizationMaker obj)
        {
            capitalizationMaker objpo = new capitalizationMaker();
            try
            {

                objpo.Podetailslist = irr.Getpoqtydetails(Convert.ToInt32(obj.invoicegid));

            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            //  return Json(objpo, JsonRequestBehavior.AllowGet);
            return PartialView(objpo);
        }

        // [HttpPost]
        // public PartialViewResult AssetPofulldetails(capitalizationMaker objj)
        public ActionResult Assetcap_mak_details(int id, string view)
        {
            string statsu = string.Empty;
            capitalizationMaker objpof = new capitalizationMaker();
            DataTable dt = new DataTable();

            try
            {
                Session["searchlist"] = "";
                irr.deletetemp(id.ToString());
                if (view == "Draft")
                {
                    objpof.Itemlevellist = irr.Getpodetails(id);
                }
                else if (view == "wait")
                {
                    statsu = "2";
                    objpof.Itemlevellist = irr.Getcapitalized(id, statsu);
                }
                else if (view == "waitt")
                {
                    statsu = "2";
                    objpof.Itemlevellist = irr.Getcapitalized(id, statsu);
                }
                else if (view == "approval")
                {
                    statsu = "3";
                    objpof.Itemlevellist = irr.Getcapitalized(id, statsu);
                }
                else if (view == "reject")
                {
                    statsu = "4";
                    objpof.Itemlevellist = irr.Getcapitalized(id, statsu);
                }
                Session["searchlist"] = id;
                if (objpof.Itemlevellist.Count > 0)
                {


                    objpof.EcfNo = objpof.Itemlevellist[0].EcfNo;
                    objpof.ecfamt = objpof.Itemlevellist[0].ecfamt;
                    objpof.Ecfdate = objpof.Itemlevellist[0].Ecfdate;
                    objpof.invoiceno = objpof.Itemlevellist[0].invoiceno;
                    objpof.invoiceamount = objpof.Itemlevellist[0].invoiceamount;
                    objpof.Vendor = objpof.Itemlevellist[0].Vendor;
                    objpof.Grn_gid = objpof.Itemlevellist[0].Grn_gid;
                    Session["invoiceGid"] = objpof.Itemlevellist[0].invoicegid;
                    Session["EcfGid"] = objpof.Itemlevellist[0].Ecfgid;
                    // objpof.invoiceqty = irr.GetInvoiceQty(objpof.Itemlevellist[0].invoicegid);
                    dt = irr.Getcapamount(objpof.Itemlevellist[0].Ecfgid, id);
                    if (dt.Rows.Count > 0)
                    {
                        objpof.Already_cap = Convert.ToDecimal(dt.Rows[0]["CapitalizedAmt"].ToString());
                        objpof.Pending_cap = Convert.ToDecimal(dt.Rows[0]["PendingAmount"].ToString());
                        // objpof.Available_cap = objpof.Itemlevellist[0].invoiceamount;
                        objpof.Available_cap = Convert.ToDecimal(dt.Rows[0]["CapitalizedAmt"].ToString());
                    }
                    else
                    {
                        // objpof.Already_cap =0;

                        // objpof.Available_cap = objpof.Itemlevellist[0].invoiceamount;
                        // objpof.Available_cap = objpof.Itemlevellist.AsEnumerable().Sum(a => a.TotalAmount);
                        //  objpof.Pending_cap = objpof.Itemlevellist[0].ecfamt - objpof.Available_cap;
                        objpof.Available_cap = 0;
                        objpof.Pending_cap = objpof.Itemlevellist[0].invoiceamount;
                        if (view != "Draft")
                        {
                            objpof.Available_cap = objpof.Itemlevellist.AsEnumerable().Sum(a => a.TotalAmount);
                            objpof.Pending_cap = objpof.Pending_cap - (objpof.Available_cap + objpof.Expense);
                            objpof.Pending_cap = objpof.Pending_cap - objpof.Available_cap;
                        }
                    }
                    Session["invoiceNo"] = objpof.invoiceno;
                    /*  objpof.Itemlevellist[0].ddlSubcategory = new SelectList(irr.GetAssetSubCategory(), "Assetsubcategoryid", "AssetSubcategpry", selectedValue: 4);
                    objpof.Itemlevellist[0].ddlbranch = new SelectList(irr.GetBranchandlocation(), "ddllocacode", "ddllocacode", selectedValue: 4);
                    objpof.pohgid = objj.pohgid;
                    Session["searchlist"] = objpof.Itemlevellist;
                    Session["InvoiceAmount"] = objpof.Itemlevellist[0].invoiceamount;*/

                    objpof.Addtaxlist = irr.Getinvoicetaxdet(id, view);
                    if (objpof.Addtaxlist.Count > 0)
                    {//ka
                        
                        objpof.Expense = objpof.Addtaxlist.AsEnumerable().Sum(a => a.Exp_Now);
                        //objpof.Expense = objpof.Pending_cap;
                       // objpof.Available_cap = objpof.Available_cap + objpof.Expense;
                        objpof.Available_cap = objpof.Addtaxlist.AsEnumerable().Sum(a => a.Available_cap);
                        objpof.Pending_cap = objpof.ecfamt - (objpof.Available_cap + objpof.Expense);

                        //objpof.Pending_cap = objpof.Pending_cap;
                        // objpof.Expense = Convert.ToDecimal(string.IsNullOrEmpty(objpof.Addtaxlist[0].Expense.ToString()) ? "0" : objpof.Addtaxlist[0].Expense.ToString());
                    }
                }

                else
                {
                    objpof.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                }
                objpof.view = view;
                objpof.invoicegid = id.ToString();
                Session["Taxinvgid"] = objpof.Itemlevellist[0].invoicegid;
                Session["Taxinvgno"] = objpof.Itemlevellist[0].invoiceno;
                Session["Taxamt"] = objpof.Itemlevellist[0].Amount;
                Session["Taxdis"] = objpof.Itemlevellist[0].Discount;
                Session["Taxt1"] = objpof.Itemlevellist[0].Tax1;
                Session["Taxothers"] = objpof.Itemlevellist[0].othres;
                Session["Taxtotal"] = objpof.Itemlevellist[0].TotalAmount;
            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            return View(objpof);
        }
        [HttpPost]
        public PartialViewResult PofullSearch(capitalizationMaker objj)
        {
            capitalizationMaker objpof = new capitalizationMaker();
            try
            {
                //objpof.Itemlevellist = irr.Getpodetails(objj);               
                string status = "2";
                objpof.Itemlevellist = irr.Getcapitalized(Convert.ToInt32(Session["searchlist"]), status); //irr.Getcapitalized(((int)Session["searchlist"]).ToString());
                if (objpof.Itemlevellist.Count == 0)
                {
                    status = "3";
                    objpof.Itemlevellist = irr.Getcapitalized(Convert.ToInt32(Session["searchlist"]), status); //irr.Getcapitalized(((int)Session["searchlist"]).ToString());
                }
                if (objpof.Itemlevellist.Count == 0)
                {
                    status = "4";
                    objpof.Itemlevellist = irr.Getcapitalized(Convert.ToInt32(Session["searchlist"]), status); //irr.Getcapitalized(((int)Session["searchlist"]).ToString());
                }
                ViewBag.asst = objj.AssetCode;
                ViewBag.loc = objj.locationName;
                if (!string.IsNullOrEmpty(objj.AssetCode))
                {
                    objpof.Itemlevellist = objpof.Itemlevellist.Where(a => objj.AssetCode == null || (a.AssetCode.ToUpper().Contains(objj.AssetCode.ToUpper()))).ToList();
                }
                if (!string.IsNullOrEmpty(objj.locationName))
                {
                    objpof.Itemlevellist = objpof.Itemlevellist.Where(a => objj.locationName == null || (a.LocationCode.ToUpper().Contains(objj.locationName.ToUpper()))).ToList();
                }
                // Session["searchlist"] = objpof.Itemlevellist;
                if (!string.IsNullOrEmpty(objj.view)) { objpof.view = objj.view; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView("AssetPofulldetails", objpof);
        }

        [HttpPost]
        public PartialViewResult CapitalizationGrid(capitalizationMaker objca)
        {
            capitalizationMaker objcal = new capitalizationMaker();
            try
            {
                // objcal.Capitalizationgridlist = irr.GetCapitalizationgrid(objca.pohgid);
                objcal.Capitalizationgridlist = (List<capitalizationMaker>)Session["searchlist"];
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(objcal);
        }

        [HttpGet]
        public PartialViewResult CapitalizationGrid(int id)
        {
            capitalizationMaker objcal = new capitalizationMaker();
            string rvalue = string.Empty;
            List<capitalizationMaker> objlist = new List<capitalizationMaker>();
            objcal.Capitalizationgridlist = (List<capitalizationMaker>)Session["searchlist"];
            try
            {
                // objcal.Capitalizationgridlist = irr.GetCapitalizationgrid(id);
                //  objcal.Capitalizationgridlist = (List<capitalizationMaker>)Session["searchlist"];

                int index = 0;
                if (id != 0)
                {
                    index = 1 - id;
                }


                objcal.Capitalizationgridlist.RemoveAt(index);
                // objcal.Capitalizationgridlist = objlist;
                rvalue = "1";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(objcal);
        }

        public ActionResult AssetPofulldetails()
        {
            capitalizationMaker objpof = new capitalizationMaker();
            try
            {
                objpof.Itemlevellist = (List<capitalizationMaker>)Session["searchlist"];

            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            return PartialView("AssetPofulldetails", objpof);
            // return Json(objpof, JsonRequestBehavior.AllowGet);
        }

        //  public PartialViewResult TaxUpdate(int pogid)
        public PartialViewResult TaxUpdate()
        {
            capitalizationMaker objpof = new capitalizationMaker();
            try
            {

                objpof.invoicegid = Session["Taxinvgid"].ToString();
                objpof.invoiceno = Session["Taxinvgno"].ToString();
                objpof.Amount = (decimal)Session["Taxamt"];
                objpof.Discount = (decimal)Session["Taxdis"];
                objpof.Tax1 = (decimal)Session["Taxt1"];
                objpof.othres = (decimal)Session["Taxothers"];
                objpof.TotalAmount = (decimal)Session["Taxtotal"];

                // objpof.pohgid = pogid;
                //   objpof.Itemlevellist = irr.Getpodetails(objpof);
                //TempData["iemlist"] = objpof.Itemlevellist;
            }
            catch (Exception ex)
            { objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); }
            return PartialView(objpof);
        }

        [HttpPost]
        public PartialViewResult TaxUpdate(capitalizationMaker obj)
        {
            capitalizationMaker objtax = new capitalizationMaker();
            try
            {
                objtax.Itemlevellist = irr.Updatefulltax(Convert.ToInt32(obj.invoicegid), obj.Amount, obj.Discount, obj.Tax1, obj.othres, obj.TotalAmount);
                Session["Taxinvgid"] = obj.invoicegid;
                Session["Taxamt"] = obj.Amount;
                Session["Taxdis"] = obj.Discount;
                Session["Taxt1"] = obj.Tax1;
                Session["Taxothers"] = obj.othres;
                Session["Taxtotal"] = obj.TotalAmount;
                objtax.view = "Draft";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView("AssetPofulldetails", objtax);
            //  return Json(objtax, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TaxEdit(capitalizationMaker Tobj)
        {
            capitalizationMaker tnobj = new capitalizationMaker();
            try
            {
                tnobj.Itemlevellist = (List<capitalizationMaker>)TempData["iemlist"];
                tnobj.Itemlevellist[0].Discount = Tobj.Discount;
                tnobj.Itemlevellist[0].Tax1 = Tobj.Tax1;
                tnobj.Itemlevellist[0].Tax2 = Tobj.Tax2;
                tnobj.Itemlevellist[0].othres = Tobj.othres;
                tnobj.Itemlevellist[0].TotalAmount = Tobj.TotalAmount;
                // return Json(tnobj, JsonRequestBehavior.AllowGet);
                //  TempData["iemlistNew"] = tnobj;
                //return View("../AssetCapitalizationMakerDetails/AssetPofulldetails");
                //return  RedirectToAction( "AssetPofulldetails","AssetCapitalizationMakerDetails");}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView("AssetPofulldetails", tnobj);

        }


        [HttpPost]
        public ActionResult SaveData(FormCollection collections, Models.capitalizationMaker json)
        {
            String result = String.Empty;
            DataTable dtgetcapitalization = new DataTable();
            capitalizationMaker tnobj = new capitalizationMaker();
            string invoice = (string)Session["invoiceNo"];
            try
            {
                tnobj = irr.UpdateIndexDetails(json, invoice);
                //tnobj.Itemlevellist = (List<capitalizationMaker>)Session["searchlist"];
                //tnobj.Itemlevellist[json.index].Discount = json.Discount;
                //tnobj.Itemlevellist[json.index].Tax1 = json.Tax1;
                //tnobj.Itemlevellist[json.index].Tax2 = json.Tax2;
                //tnobj.Itemlevellist[json.index].othres = json.othres;
                //tnobj.Itemlevellist[json.index].TotalAmount = json.TotalAmount;
                //result = "1";
                //  Session["searchlist"] = tnobj.Itemlevellist;
                //Session["TotalAmount"] = tnobj.Itemlevellist[json.index].TotalAmount;
                //return Json(result, JsonRequestBehavior.AllowGet);
                tnobj.view = "Draft";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            // return Json(tnobj,JsonRequestBehavior.AllowGet);
            return View("AssetPofulldetails", tnobj);

        }

        [HttpPost]
        public PartialViewResult RemoveCapitalization(capitalizationMaker obj)
        {
            capitalizationMaker objcal = new capitalizationMaker();
            try
            {
                string rvalue = string.Empty;
                List<capitalizationMaker> objlist = new List<capitalizationMaker>();
                objlist = (List<capitalizationMaker>)Session["searchlist"];

                objcal.Capitalizationgridlist = objlist;

                // return Json(objcal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return PartialView("CapitalizationGrid", objcal);

        }


        public PartialViewResult AddTaxAmount()
        {
            capitalizationMaker addobj = new capitalizationMaker();
            try
            {
                if (Session["list"] == null)
                {
                    addobj.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                }
                else
                {
                    addobj.Addtaxlist = (List<capitalizationMaker>)Session["list"];
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return PartialView(addobj);
        }

        [HttpPost]
        public PartialViewResult AddTaxAmount(capitalizationMaker addobj)
        {
            List<capitalizationMaker> addobjl = new List<capitalizationMaker>();
            try
            {
                if (addobj.Amount > 0)
                {
                    addobjl.Add(new capitalizationMaker() { invoiceno = (string)Session["invoiceNo"], glcode = addobj.glcode, Description = addobj.Description, TaxAmount = addobj.Amount });
                    addobj.Addtaxlist = addobjl.ToList();
                    Session["list"] = addobj.Addtaxlist;
                }
                else
                {
                    addobj.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            // return Json(addobj,JsonRequestBehavior.AllowGet);
            return PartialView("AddTaxAmount", addobj);
        }

        [HttpPost]
        public JsonResult Asstaxinsert(string ids)
        {
            string[] id = ids.Split(',');

            decimal invoicetamt;
            decimal invamt;
            decimal Tax = Convert.ToDecimal("0.00");
            decimal TotalAmt;
            decimal exps;
            decimal exp_now;
            string verified = string.Empty;
            string Result = string.Empty;
            string Maker = string.Empty;
            List<capitalizationMaker> lnobj = new List<capitalizationMaker>();
            List<capitalizationMaker> Tobj = new List<capitalizationMaker>();

            DataTable dttax = new DataTable();
            Maker = cmnfunc.authorize("228");
            try
            {
                if (Maker == "Success")
                {
                    lnobj = irr.Getpodetails(((int)Session["searchlist"]).ToString());
                    for (int i = 0; i < lnobj.Count; i++)
                    {
                        for (int ii = 0; ii < id.Length; ii++)
                        {
                            if (id[ii] != "" && !string.IsNullOrEmpty(id[ii]) && id[ii] != "undefined")
                                if (id[ii] == lnobj[i].Grn_detgid.ToString())
                                    Tobj.Add(lnobj[i]);
                        }
                    }
                    dttax = irr.Getexpsum(Tobj[0].invoicegid.ToString(), "1");
                    invoicetamt = Tobj[0].invoiceamount;
                    invamt = Tobj.AsEnumerable().Sum(o => o.TotalAmount);
                    exps = Tobj.AsEnumerable().Sum(o => o.Tax1);
                    Tax = exps;
                    //Tax = Convert.ToDecimal(string.IsNullOrEmpty(dttax.Rows[0]["exp_cap_now"].ToString()) ? "0" : dttax.Rows[0]["exp_cap_now"].ToString());
                    exp_now = Convert.ToDecimal(string.IsNullOrEmpty(dttax.Rows[0]["exp_expense_now"].ToString()) ? "0" : dttax.Rows[0]["exp_expense_now"].ToString());
                    TotalAmt = invamt + exp_now;


                    if (Math.Floor(invoicetamt) >= Math.Floor(TotalAmt))
                    {

                        verified = irr.Verify_details(id, Tobj[0].invoicegid);
                        if (verified == "SUCCESS")
                        {
                            Result = irr.SubmittoChecker(Tobj, id);
                        }
                        else if (verified == "branch_date")
                        {
                            Result = "Branch Launch date is Empty in the selected list";
                        }
                        else if (verified == "Please check the podetails")
                        {
                            Result = "Please check the podetails";
                        }
                        else
                        {
                            Result = "Doesn't form a Group";
                        }

                    }
                    else
                    {
                        Result = "UnSuccess";
                        return Json(new { Result, TotalAmt }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Result = "Unauthorized User!";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message.ToString();
            }
            //return View(Result);
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public PartialViewResult DeleteTax()
        {
            capitalizationMaker cobj = new capitalizationMaker();
            cobj.Addtaxlist = (List<capitalizationMaker>)Session["list"];
            string Result = string.Empty;
            try
            {
                cobj.Addtaxlist.RemoveAt(0);
                Result = "Success";
                Session["list"] = null;
                cobj.Addtaxlist = Enumerable.Empty<capitalizationMaker>().ToList<capitalizationMaker>();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            // return Json(Result,JsonRequestBehavior.AllowGet);
            return PartialView("AddTaxAmount", cobj);
        }

        [HttpGet]
        public PartialViewResult PoEdit(poraiser objraiser, string viewfor, string id)
        {
            fb_DataModelpr objModelfb = new fb_DataModelpr();
            poraiser obj = new poraiser();
            try
            {
                //Session["ShipTemp"] = null;
                Session["viewfor"] = null;
                if (viewfor == "edit")
                {
                    Session["viewfor"] = "edit";
                    // ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    Session["viewfor"] = "view";
                    //ViewBag.viewfor = "view";
                }
                else if (viewfor == "cancel")
                {
                    Session["viewfor"] = "cancel";
                    //ViewBag.viewfor = "cancel";
                }
                else if (viewfor == "delete")
                {
                    Session["viewfor"] = "delete";
                    //ViewBag.viewfor = "delete";
                }
                else if (viewfor == "checkercancel")
                {
                    Session["viewfor"] = "checkercancel";
                }
                else if (viewfor == "checker")
                {
                    Session["viewfor"] = "checker";
                }
                else if (viewfor == "closure")
                {
                    Session["viewfor"] = "closure";
                }
                else if (viewfor == "closurechecker")
                {
                    Session["viewfor"] = "closurechecker";
                }
                else if (viewfor == "amend")
                {
                    Session["viewfor"] = "amend";
                }
                else if (viewfor == "delmat")
                {
                    Session["viewfor"] = "delmat";
                }
                shipment objship = new shipment();
                DataTable objtable = new DataTable();
                DataTable objTable = objModelfb.GetPoDetails(Convert.ToInt16(id));
                Session["podetail"] = objTable;
                DataTable dt = new DataTable();
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    dt = objModelfb.GetShipTable(Convert.ToInt32(objTable.Rows[i]["podetails_gid"].ToString()));
                }
                Session["totalship"] = dt;
                Session["shipdelete"] = dt;
                //objship.shiplist = objModel.GetShipmentDetails(dt);
                obj.objlist = objModelfb.GetPoDetailsList(objTable);
                if (obj.objlist.Count > 0)
                {
                    obj.podetGid = obj.objlist[0].podetGid;
                    obj.poheadGid = obj.objlist[0].poheadGid;
                    obj.poCancelGid = objraiser.poCancelGid;
                    obj.remarks = objraiser.remarks;
                    obj.poClosureGid = objraiser.poClosureGid;
                    obj.porefno = obj.objlist[0].porefno;
                    obj.podate = obj.objlist[0].podate;
                    obj.poenddate = obj.objlist[0].poenddate;
                    obj.cbfheadAmount = obj.objlist[0].cbfheadAmount;
                    obj.devamount = obj.objlist[0].devamount;
                    obj.cbfdetailsQty = obj.objlist[0].cbfdetailsQty;
                    obj.projmanagergid = obj.objlist[0].projmanagergid;
                    obj.vendorNote = obj.objlist[0].vendorNote;
                    obj.vendorgid = obj.objlist[0].vendorgid;
                    obj.tempName = obj.objlist[0].tempName;
                    obj.templname = obj.objlist[0].templname;
                    obj.additionTemplate = obj.objlist[0].additionTemplate;
                    obj.itType = obj.objlist[0].itType;
                    obj.department = obj.objlist[0].department;

                    obj.vendorName = obj.objlist[0].vendorName;
                    obj.templateGid = obj.objlist[0].templateGid;
                    obj.templateList = new SelectList(objModelfb.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
                    obj.projmanagerList = new SelectList(objModelfb.GetProjectOwnerList(), "projmanagergid", "projmanagername", obj.projmanagergid);
                    obj.raisedby = objModelfb.GetLoginUser();
                    //if (Session["podelete"] != null)
                    //    obj.count = (int)Session["podelete"];
                    Session["polist"] = obj;
                    Session["Eowtabclose"] = "Eowtabclose";
                }
                //  return RedirectToAction("PoEdit", "PoRaiserEdit");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return PartialView(obj);
        }


        public PartialViewResult grnconfirmation(grnconfirmation gr, int grnheadgid)
        {
            prsummodel objrep = new prsummodel();
            grnconfirmationdetails objgrn = new grnconfirmationdetails();
            try
            {
                objgrn.grnconfirmationdetail = objrep.getgrnconfirmationdetails(gr, grnheadgid);
                objgrn.grnheadGid = grnheadgid;
                objgrn.grnrefno = objgrn.grnconfirmationdetail[0].grnrefno.ToString();
                objgrn.grndate = objgrn.grnconfirmationdetail[0].grndate.ToString();
                objgrn.vendorname = objgrn.grnconfirmationdetail[0].vendorname.ToString();
                objgrn.poworefno = objgrn.grnconfirmationdetail[0].poworefno.ToString();
                objgrn.dcno = objgrn.grnconfirmationdetail[0].dcno.ToString();
                objgrn.invoiceno = objgrn.grnconfirmationdetail[0].invoiceno.ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(objgrn);
        }


        [HttpGet]
        public PartialViewResult shipmentdetails(int poDetGid)
        {
            fb_DataModelpr objModel = new fb_DataModelpr();
            string deleteflag = "Y";
            shipment obj = new shipment();
            int count = 0;
            DataTable objTable = new DataTable();
            try
            {    // DataView dv = new DataView();
                Session["poDetGid"] = poDetGid;
                // obj.cbfdetGid = cbfdetGid;
                //  List<shipment> lLstShip = new List<shipment>();
                obj.shipmentlist = new SelectList(objModel.getshipmentType(), "shipmentgid", "shipmentName");
                if (Session["ShipTemp"] != null)
                {
                    DataTable objtable = (DataTable)Session["ShipTemp"];
                    DataView dv = new DataView();
                    objtable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                    dv = objtable.DefaultView;
                    objtable = dv.ToTable();
                    if (objtable.Rows.Count == 0)
                    {
                        DataTable dt = objModel.GetShipTable(poDetGid);
                        //if (dt.Columns.Count == 10)
                        if (!dt.Columns.Contains("Slno") && !dt.Columns.Contains("deleteflag"))
                        {
                            dt.Columns.Add("Slno");
                            dt.Columns.Add("deleteflag");
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["Slno"] = count + 1;

                            }
                        }
                        objtable = (DataTable)Session["ShipTemp"];
                        if (dt.Rows.Count > 0)
                        {
                            objtable.Merge(dt);
                        }
                        Session["ShipTemp"] = objtable;
                    }
                }
                if (Session["ShipBulkTable"] == null && Session["ShipTemp"] == null && Session["excelFinalTable"] == null)
                {
                    DataTable dt = objModel.GetShipTable(poDetGid);
                    if (!dt.Columns.Contains("Slno"))
                        dt.Columns.Add("Slno");
                    if (!dt.Columns.Contains("deleteflag"))
                        dt.Columns.Add("deleteflag");
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["deleteflag"].ToString() == "Y")
                                dt.Rows[i]["Slno"] = count + 1;

                        }
                    }
                    Session["ShipTemp"] = dt;
                    obj.shiplist = objModel.GetShipmentDetails(dt);
                }

                if (Session["ShipBulkTable"] != null)
                {
                    objTable = (DataTable)Session["ShipBulkTable"];
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        if (Session["deleteval"] == null || Session["deleteval"] == "")
                        {
                            Session["deleteval"] = objTable.Rows[i]["deleteflag"].ToString();
                        }
                    }
                    if (Session["deleteval"] != "")
                    {
                        DataView dv = new DataView();
                        objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                        //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] +"";
                        dv = objTable.DefaultView;
                        objTable = dv.ToTable();
                        obj.shiplist = objModel.GetShipmentDetails(objTable);
                    }
                    else
                    {
                        DataView dv = new DataView();
                        objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                        dv = objTable.DefaultView;
                        objTable = dv.ToTable();
                        obj.shiplist = objModel.GetShipmentDetails(objTable);
                    }
                    //DataView dv = new DataView();
                    //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                    //dv = objTable.DefaultView;
                    //objTable = dv.ToTable();
                    //obj.shiplist = objModel.GetShipmentDetails(objTable);
                }

                else
                {
                    if (Session["ShipTemp"] != null)
                    {
                        objTable = (DataTable)Session["ShipTemp"];
                        for (int i = 0; i < objTable.Rows.Count; i++)
                        {
                            if (Session["deleteval"] == null || Session["deleteval"] == "")
                            {
                                Session["deleteval"] = objTable.Rows[i]["deleteflag"].ToString();
                            }
                        }
                        if (Session["deleteval"] != "" && Session["deleteval"] != null)
                        {
                            DataView dv = new DataView();
                            //objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND deleteflag <> '" + deleteflag + "'");
                            objTable.DefaultView.RowFilter = ("poshipment_podet_gid = '" + Session["poDetGid"] + "' AND (deleteflag= '" + "" + "' OR deleteflag is NULL)");
                            //objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] +"";
                            dv = objTable.DefaultView;
                            objTable = dv.ToTable();
                            obj.shiplist = objModel.GetShipmentDetails(objTable);
                        }
                        else
                        {
                            DataView dv = new DataView();
                            objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                            dv = objTable.DefaultView;
                            objTable = dv.ToTable();
                            obj.shiplist = objModel.GetShipmentDetails(objTable);
                        }
                    }

                    if (Session["excelFinalTable"] != null)
                    {
                        objTable = (DataTable)Session["excelFinalTable"];
                        DataView dv1 = new DataView();
                        objTable.DefaultView.RowFilter = "poshipment_podet_gid = " + Session["poDetGid"] + "";
                        dv1 = objTable.DefaultView;
                        objTable = dv1.ToTable();
                        obj.shiplist = objModel.ExcelToShipment(objTable);
                    }
                }

                if (Session["count"] == null)
                {
                    Session["count"] = obj.shiplist.Count;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return PartialView("ShipmentViewDetailsnew", obj);
        }

        [HttpPost]
        public ActionResult SaveDataTax(capitalizationMaker tax)
        {
            String result = String.Empty;
            DataTable dtgetcapitalization = new DataTable();
            capitalizationMaker tnobjtax = new capitalizationMaker();
            try
            {
                tnobjtax.Addtaxlist = irr.UpdateIndexDetailstax(tax);
                tnobjtax.view = "Draft";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View("AddTaxAmount", tnobjtax);

        }
        [HttpGet]
        public PartialViewResult PoQuantity(string id)
        {
            EOW_PO Header = new EOW_PO();
            List<EOW_PO> getsingleid = new List<EOW_PO>();
            List<EOW_PO> TypeModel = new List<EOW_PO>();
            try
            {
                getsingleid = irr.GetPoDetailssingledata(id).ToList();
                Header.POGid = id.ToString();
                Header.PONo = getsingleid[0].PONo.ToString();
                Header.POdate = getsingleid[0].POdate.ToString();
                Header.POStatus = getsingleid[0].POStatus.ToString();
                Header.POBalamount = getsingleid[0].POBalamount.ToString();
                Header.POUtilizedamount = getsingleid[0].POUtilizedamount.ToString();
                Header.POApprovedStatus = getsingleid[0].POApprovedStatus.ToString();
                ViewBag.poheaderheader = Header;


                TypeModel = irr.GetPoDetailsitem(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(TypeModel.ToList());
        }


        [HttpPost]
        public JsonResult Posubcategory(string values)
        {
            SelectList cobj;
            try
            {
                cobj = new SelectList(irr.GetAssetSubCategory(values), "Assetsubcategoryid", "AssetSubcategpry");
                return Json(cobj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDepNGLcode(string values)
        {
            string cobj = string.Empty;
            try
            {
                cobj = irr.GetDepNGLcode(values);
                string[] cobj1 = cobj.Split(',');
                return Json(cobj1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult UpdateLineItems(int grnid, int invid)
        {
            capitalizationMaker TypeModel = new capitalizationMaker();
            try
            {
                TypeModel = irr.Getpodetailstoedit(grnid, invid);
                string subNmain = irr.GetCatNSubcatId(TypeModel.AssetSubcategpry);
                string[] sub = subNmain.Split(',');
                ViewBag.CatId = sub[0];
                ViewBag.subcatID = sub[1];
                TypeModel.ddlCategory = new SelectList(irr.Getassetcategory(), "assetCategoryId", "assetCategoryName", sub[0]);
                TypeModel.ddlSubcategory = new SelectList(irr.GetAssetSubCategory(sub[0]), "Assetsubcategoryid", "AssetSubcategpry", sub[1]);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }

            return PartialView(TypeModel);
        }


        [HttpPost]
        public ActionResult RefreshGrid(string invoicegid = null)
        {
            capitalizationMaker tnobj = new capitalizationMaker();
            try
            {
                tnobj = irr.RefreshCapitalisationdetails(string.IsNullOrEmpty(invoicegid) ? "0" : invoicegid);
                //tnobj.Itemlevellist = (List<capitalizationMaker>)Session["searchlist"];
                //tnobj.Itemlevellist[json.index].Discount = json.Discount;
                //tnobj.Itemlevellist[json.index].Tax1 = json.Tax1;
                //tnobj.Itemlevellist[json.index].Tax2 = json.Tax2;
                //tnobj.Itemlevellist[json.index].othres = json.othres;
                //tnobj.Itemlevellist[json.index].TotalAmount = json.TotalAmount;
                //result = "1";
                //  Session["searchlist"] = tnobj.Itemlevellist;
                //Session["TotalAmount"] = tnobj.Itemlevellist[json.index].TotalAmount;
                //return Json(result, JsonRequestBehavior.AllowGet);
                tnobj.view = "Draft";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            // return Json(tnobj,JsonRequestBehavior.AllowGet);
            return View("AssetPofulldetails", tnobj);

        }

    }

}
