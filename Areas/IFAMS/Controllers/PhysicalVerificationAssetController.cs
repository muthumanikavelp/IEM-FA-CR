using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Excel;
using System.Collections;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using ClosedXML.Excel;
using System.Web.UI.WebControls;

namespace IEM.Areas.IFAMS.Controllers
{
    public class PhysicalVerificationAssetController : Controller
    {
        //
        // GET: /IFAMS/PhysicalVerificationAsset/
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();



        public ActionResult VerificationImport()
        {
            //PhysicalVerificationAsset Model = new PhysicalVerificationAsset();
            //List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(new SelectListItem { Text = "2014-2015", Value = "1" });

            //list.Add(new SelectListItem { Text = "2015-2016", Value = "2" });

            //Model.FinancialYear = new SelectList(list, "Value", "Text");

            //ViewBag.FinancialYear = list;

            return View();
        }
        [HttpGet]
        public JsonResult FinancialYearDrop()
        {
            PhysicalVerificationAsset Model = new PhysicalVerificationAsset();
            List<PhysicalVerificationAsset> list = new List<PhysicalVerificationAsset>();
            try
            {
                DataModel DM = new DataModel();
                string Currentyear = DM.toGetFincialyear();
                //for (int i = 0, year = 2012; year <= Currentyear; year++, i = i + 1)
                //{
                Model = new PhysicalVerificationAsset();
                //if (i == 0)
                //{
                //    Model.FinancialYear = "-------Select-------";
                //    Model.Fid = i;
                //}
                //else
                //{
                //  Model.FinancialYear = Convert.ToString(year - 1) + "-" + Convert.ToString(year);
                Model.FinancialYear = Currentyear;
                // }
                list.Add(Model);
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            } return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PVImportFileQuery()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                Verify.PVImportFileQuery = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                if (Verify.PVImportFileQuery.Count == 0)
                { ViewBag.Message = "No Records Found"; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(Verify);
            }
            finally
            {
            }

            return View(Verify);
        }


        [HttpGet]
        public PartialViewResult PVImportFileSerach()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                Verify.PVImportFileQuery = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                if (Verify.PVImportFileQuery.Count == 0)
                { ViewBag.Message = "No Records Found"; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return PartialView("PVImportFileSerach", Verify);
        }

        //[HttpGet]
        //public PartialViewResult PVSerach()
        //{
        //    PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
        //    IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
        //    // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
        //    try
        //    {
        //        Verify.PVSerach = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
        //        if (Verify.PVSerach.Count == 0)
        //        { ViewBag.Message = "No Records Found"; }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //    }
        //    return PartialView("PVSerach",Verify);        
        //}
        public JsonResult FillDropPVImportFileQuery()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                return Json(Model.PVImportFileDrop(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }


        [HttpPost]
        public JsonResult AutoFinancialYear(string term)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                List<PhysicalVerificationAsset> AutoYear = new List<PhysicalVerificationAsset>();
                AutoYear = Model.PVImportFileDrop().ToList();
                return Json(AutoYear, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }


        [HttpPost]
        public JsonResult AutoStatus(string term)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                List<PhysicalVerificationAsset> AutoStatus = new List<PhysicalVerificationAsset>();
                AutoStatus = Model.AutoStatus(term).ToList();
                return Json(AutoStatus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpPost]
        public JsonResult AutoFileName(string term)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                List<PhysicalVerificationAsset> AutoStatus = new List<PhysicalVerificationAsset>();
                AutoStatus = Model.AutoFileName(term).ToList();
                return Json(AutoStatus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }



        [HttpPost]
        public JsonResult AutoBarcode(string term)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            try
            {
                List<PhysicalVerificationAsset> AutoBarcode = new List<PhysicalVerificationAsset>();
                AutoBarcode = Model.AutoBarcode(term).ToList();
                return Json(AutoBarcode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpPost]
        public JsonResult AutoBranch(string term)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                List<PhysicalVerificationAsset> AutoBranch = new List<PhysicalVerificationAsset>();
                AutoBranch = Model.AutoBranch(term).ToList();
                return Json(AutoBranch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }



        [HttpGet]
        public JsonResult YearPVImportFileQuery(string Year)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                return Json(Model.YearPVImportFileDrop(Year), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }




        [HttpGet]
        public JsonResult SummaryImportFileQuery(string Year, string FileName)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                return Json(Model.Summary(Year, FileName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpGet]
        public JsonResult BranchPVImportFileQuery(string Year, string FileName)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                return Json(Model.BranchPVImportFileDrop(Year, FileName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }


        [HttpGet]
        public JsonResult StatusPVImportFileQuery(string Year, string FileName, string Branch)
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                return Json(Model.StatusPVImportFileDrop(Year, FileName, Branch), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpGet]
        public JsonResult StatusPV()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            try
            {
                List<PhysicalVerificationAsset> PVlist = new List<PhysicalVerificationAsset>();
                List<PhysicalVerificationAsset> Model1 = new List<PhysicalVerificationAsset>();
                PhysicalVerificationAsset obj = new PhysicalVerificationAsset();
                PVlist = Model.PVImportSerach();
                var s = PVlist.Select(m => new { m.Status }).Distinct();
                foreach (var st in s)
                {
                    string y = st.Status.ToString();
                    obj = new PhysicalVerificationAsset();
                    obj.Status = st.Status.ToString();
                    Model1.Add(obj);
                }
                obj = new PhysicalVerificationAsset();
                obj.Status = "All";
                Model1.Add(obj);
                return Json(Model1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }





        [HttpGet]
        public PartialViewResult PVImportFileSerach1(PhysicalVerificationAsset Mod)
        {
            try
            {
                //ViewBag.Childsearch = Model.AssetId;
                //ViewBag.command = "SEARCH";
                if (Mod.command == "SEARCH")
                {
                    PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
                    IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
                    Verify.PVImportFileQuery = Model.PVImportSerach();
                    if ((string.IsNullOrEmpty(Mod.FinancialYear) || string.IsNullOrWhiteSpace(Mod.FinancialYear))
                        && (string.IsNullOrEmpty(Mod.FileName) || string.IsNullOrWhiteSpace(Mod.FileName))
                        && (string.IsNullOrEmpty(Mod.Branch) || string.IsNullOrWhiteSpace(Mod.Branch))
                        && (string.IsNullOrEmpty(Mod.Status) || string.IsNullOrWhiteSpace(Mod.Status)))
                    {
                        Verify.PVImportFileQuery = Model.PVImportSerach();
                    }
                    if (Mod.FinancialYear != null)
                    {
                        //Verify.PVImportFileQuery = Model.PVImportSerach();
                        Verify.PVImportFileQuery = Verify.PVImportFileQuery.Where(x => Mod.FinancialYear == null || (x.FinancialYear.ToUpper().Contains(Mod.FinancialYear.ToUpper()))).ToList();
                        //Session["Childlist"] = Records.AssetParentList.ToList();
                    }
                    if (Mod.FileName != null)
                    {
                        //Verify.PVImportFileQuery = Model.PVImportSerach();
                        Verify.PVImportFileQuery = Verify.PVImportFileQuery.Where(x => Mod.FileName == null || (x.FileName.ToUpper().Contains(Mod.FileName.ToUpper()))).ToList();
                    }
                    if (Mod.Branch != null)
                    {
                        //Verify.PVImportFileQuery = Model.PVImportSerach();
                        Verify.PVImportFileQuery = Verify.PVImportFileQuery.Where(x => Mod.Branch == null || (x.Branch.ToUpper().Contains(Mod.Branch.ToUpper()))).ToList();
                    }
                    if (Mod.Status != null)
                    {
                        //Verify.PVImportFileQuery = Model.PVImportSerach();
                        Verify.PVImportFileQuery = Verify.PVImportFileQuery.Where(x => Mod.Status == null || (x.Status.ToUpper().Equals(Mod.Status.ToUpper()))).ToList();
                    }
                    if (Verify.PVImportFileQuery.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return PartialView("PVImportFileSerach", Verify);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return PartialView();
        }

        public ActionResult PVReport()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            try
            {
                //Session["Verify.PVReport"] = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                Model.inserttopv("", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                Verify.PVReport = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                if (Session["Verify.PVReport"] != null)
                {
                    Verify.PVReport = (List<PhysicalVerificationAsset>)Session["Verify.PVReport"];
                }
                if (Verify.PVReport.Count == 0)
                { ViewBag.Message = "No Records Found"; }
                ViewBag.DropStatus = 0;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(Verify);
            }
            finally
            {
            }
            return View(Verify);
        }

        [HttpPost]
        public ActionResult PVReport(string txtyear, string txtassetid, string txtassetsubcat, string txtbarcode, string txtlocation, string txtfilename, string txtstatus, string command)
        {
            int AsPerFA = 0;
            int Verified = 0;
            int NotVerified = 0;
            int NotInFAR = 0;
            int AsFileImp = 0;
            decimal FARegWDV = 0;
            ViewBag.FAOverAll = "0";
            ViewBag.FALocation = "0";
            ViewBag.Verified = "0";
            ViewBag.MisMatched = "0";
            ViewBag.NotVerifiedInLoc = "0";
            ViewBag.NotInFA = "0";
            ViewBag.FileCount = "0";
            ViewBag.FAOverAll_wdv = "0";
            ViewBag.FALocation_wdv = "0"; ViewBag.Verified_wdv = "0";
            ViewBag.MisMatched_wdv = "0";
            ViewBag.NotVerifiedInLoc_wdv = "0"; ViewBag.NotInFA_wdv = "0"; ViewBag.FileCount_wdv = "0";
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            try
            {
                if (command == "SEARCH")
                {
                    Session["Verify.PVReport"] = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                    PhysicalVerificationAsset PVReport = new PhysicalVerificationAsset();
                    IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
                    if (txtyear == "")
                    {
                        return RedirectToAction("PVReport", "PhysicalVerificationAsset");
                        //Verify.PVReport = Enumerable.Empty<PhysicalVerificationAsset>().ToList<PhysicalVerificationAsset>();
                        //return View(Verify);
                    }
                    else
                    {
                        Verify.PVReport = Model.PVImportSerach();
                        if (txtstatus == "All")
                        {
                            if (txtyear != "")
                            {
                                int FARegCount = Model.FARegister();
                                FARegWDV = Model.FARegisterWDV();
                                ViewBag.AsPerFACount = FARegCount;
                                ViewBag.AsPerFAWDV = FARegWDV;
                                ViewBag.txtyear = txtyear;
                                ViewBag.DropStatus = txtstatus;
                                var P = Model.PVImportSerach();
                                int c = 0, v = 0, f = 0;
                                int c_wdv = 0, v_wdv = 0, f_wdv = 0;
                                foreach (var p in P)
                                {
                                    if (p.Status == "Not Verified" && p.FileName == "")
                                    {
                                        c = c + 1;
                                        c_wdv += Convert.ToInt32(p.wdv);
                                    }
                                    if (p.Status == "Verified" && p.FileName != "")
                                    {
                                        v = v + 1;
                                        v_wdv += Convert.ToInt32(p.wdv);
                                    }
                                    if (p.Status == "Not Verified" && p.FileName != "")
                                    {
                                        f = f + 1;
                                        f_wdv += Convert.ToInt32(p.wdv);
                                    }
                                }
                                DataTable dt = Model.GetLocFromYear(txtyear);
                                foreach (DataRow row in dt.Rows)
                                {
                                    var PV = Model.SummaryLoc(txtyear, row["pv_assetdet_assetbranch"].ToString());
                                    foreach (var p in PV)
                                    {
                                        Verified = Verified + Convert.ToInt32(p.FAMatched);
                                        NotVerified = NotVerified + Convert.ToInt32(p.FAMissMatched);
                                        NotInFAR = NotInFAR + Convert.ToInt32(p.NotinFA);
                                        AsFileImp = AsFileImp + Convert.ToInt32(p.NoOfRecords);
                                        NotVerified = p.NotVerfied;
                                    }
                                }
                                int A = 0, B = 0, C = 0, D = 0;
                                decimal A_wdv = 0, B_wdv = 0, C_wdv = 0, D_wdv = 0;
                                int Notver = Model.NotVerified(txtyear);
                                // ViewBag.NotVerified = NotVerified + AsPerFA;
                                ViewBag.NotInFAR = NotInFAR;
                                ViewBag.NotInFAR = NotInFAR;
                                // int V = Model.FAVerified();                                
                                string result = Model.FAVerified();
                                string[] FA = result.Split(',');
                                int V = Convert.ToInt32(FA[0]);
                                decimal V_wdv = Convert.ToDecimal(FA[1]);
                                //  int NotV = (FARegCount - Model.FANotVerified()) + Model.PVNotVerified();
                                string[] FANV = Model.FANotVerified().Split(',');
                                string[] PVNV = Model.PVNotVerified().Split(',');
                                int NotV = (FARegCount - Convert.ToInt32(FANV[0])) + Convert.ToInt32(PVNV[0]);
                                decimal NotV_wdv = (FARegWDV - Convert.ToDecimal(FANV[1])) + Convert.ToDecimal(PVNV[1]);
                                ViewBag.Verified = V;
                                ViewBag.VerifiedWDV = V_wdv;
                                ViewBag.NotVerified = NotV;
                                ViewBag.AsFileImp = AsFileImp;
                                A = Convert.ToInt32(FA[0]);
                                A_wdv = Convert.ToDecimal(FA[1]);
                                // B = Model.PVNotVerified();
                                B = Convert.ToInt32(FANV[0]);
                                B_wdv = Convert.ToDecimal(FANV[1]);
                                C = FARegCount - A;
                                C_wdv = FARegWDV - A_wdv;
                                ViewBag.FAOverAll = A + B + C;
                                ViewBag.FAOverAll_wdv = A_wdv + B_wdv + C_wdv;
                                ViewBag.FALocation = FARegCount;
                                ViewBag.FALocation_wdv = FARegWDV;
                                ViewBag.Verified = A;
                                ViewBag.Verified_wdv = A_wdv;
                                ViewBag.MisMatched = B;
                                ViewBag.MisMatched_wdv = B_wdv;
                                ViewBag.NotVerifiedInLoc = C;
                                ViewBag.NotVerifiedInLoc_wdv = C_wdv;
                                ViewBag.NotInFA = NotInFAR;
                                ViewBag.FileCount = AsFileImp;
                                Model.inserttopv(txtfilename, (A + B + C).ToString(), (FARegCount).ToString(), A.ToString(), B.ToString(), C.ToString(), NotInFAR.ToString(), AsFileImp.ToString(), (A_wdv + B_wdv + C_wdv).ToString(), (FARegWDV).ToString(), A_wdv.ToString(), B_wdv.ToString(), C_wdv.ToString(), "", "");
                            }
                        }
                        else
                        {
                            if ((string.IsNullOrEmpty(txtyear) || string.IsNullOrWhiteSpace(txtyear))
                                && (string.IsNullOrEmpty(txtassetid) || string.IsNullOrWhiteSpace(txtassetid))
                                 && (string.IsNullOrEmpty(txtlocation) || string.IsNullOrWhiteSpace(txtlocation))
                                 && (string.IsNullOrEmpty(txtbarcode) || string.IsNullOrWhiteSpace(txtbarcode))
                                  && (string.IsNullOrEmpty(txtstatus) || string.IsNullOrWhiteSpace(txtstatus))
                                    && (string.IsNullOrEmpty(txtfilename) || string.IsNullOrWhiteSpace(txtfilename)))
                            {
                                Verify.PVReport = Model.PVImportSerach();
                            }
                            if (!string.IsNullOrEmpty(txtyear))
                            {
                                ViewBag.txtyear = txtyear;
                                Verify.PVReport = Verify.PVReport.Where(x => txtyear == null || (x.FinancialYear.ToUpper().Contains(txtyear.ToUpper()))).ToList();
                                if (txtyear != "")
                                {
                                    int FARegCount = Model.FARegister();
                                    FARegWDV = Model.FARegisterWDV();
                                    ViewBag.AsPerFAWDV = FARegWDV;
                                    ViewBag.AsPerFACount = FARegCount;
                                    var P = Model.PVImportSerach();
                                    int c = 0, v = 0, f = 0;
                                    int cWDV = 0, vWDV = 0, fWDV = 0;
                                    foreach (var p in P)
                                    {
                                        if (p.Status == "Not Verified" && p.FileName == "")
                                        {
                                            c = c + 1;
                                            cWDV += Convert.ToInt32(p.wdv);
                                        }
                                        if (p.Status == "Verified" && p.FileName != "")
                                        {
                                            v = v + 1;
                                            vWDV += Convert.ToInt32(p.wdv);
                                        }
                                        if (p.Status == "Not Verified" && p.FileName != "")
                                        {
                                            f = f + 1;
                                            fWDV += Convert.ToInt32(p.wdv);
                                        }
                                    }
                                    DataTable dt = Model.GetLocFromYear(txtyear);
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        var PV = Model.SummaryLoc(txtyear, row["pv_assetdet_assetbranch"].ToString());
                                        foreach (var p in PV)
                                        {
                                            Verified = Verified + Convert.ToInt32(p.FAMatched);
                                            NotVerified = NotVerified + Convert.ToInt32(p.FAMissMatched);
                                            NotInFAR = NotInFAR + Convert.ToInt32(p.NotinFA);
                                            AsFileImp = AsFileImp + Convert.ToInt32(p.NoOfRecords);
                                            NotVerified = p.NotVerfied;
                                        }
                                    }
                                    int A = 0, B = 0, C = 0, D = 0;
                                    decimal A_wdv = 0, B_wdv = 0, C_wdv = 0, D_wdv = 0;
                                    int Notver = Model.NotVerified(txtyear);
                                    // ViewBag.NotVerified = NotVerified + AsPerFA;
                                    ViewBag.NotInFAR = NotInFAR;
                                    string result = Model.FAVerified();
                                    string[] FA = result.Split(',');
                                    int V = Convert.ToInt32(FA[0]);
                                    decimal V_wdv = Convert.ToDecimal(FA[1]);
                                    string[] FANV = Model.FANotVerified().Split(',');
                                    string[] PVNV = Model.PVNotVerified().Split(',');
                                    int NotV = (FARegCount - Convert.ToInt32(FANV[0])) + Convert.ToInt32(PVNV[0]);
                                    decimal NotV_wdv = (FARegWDV - Convert.ToDecimal(FANV[1])) + Convert.ToDecimal(PVNV[1]);
                                    ViewBag.Verified = V;
                                    ViewBag.VerifiedWDV = V_wdv;
                                    ViewBag.NotVerified = NotV;
                                    ViewBag.NotVerified_wdv = NotV_wdv;
                                    ViewBag.AsFileImp = AsFileImp;
                                    //  A = Model.FAVerified();
                                    A = Convert.ToInt32(FA[0]);
                                    A_wdv = Convert.ToDecimal(FA[1]);
                                    // B = Model.PVNotVerified();
                                    B = Convert.ToInt32(FANV[0]);
                                    B_wdv = Convert.ToDecimal(FANV[1]);
                                    C = FARegCount - A;
                                    C_wdv = FARegWDV - A_wdv;
                                    ViewBag.FAOverAll = A + B + C;
                                    ViewBag.FAOverAll_wdv = A_wdv + B_wdv + C_wdv;
                                    // ViewBag.FALocation = FARegCount;
                                    ViewBag.FALocation = A + C;
                                    ViewBag.FALocation_wdv = A_wdv + C_wdv;
                                    ViewBag.Verified = A;
                                    ViewBag.Verified_wdv = A_wdv;
                                    ViewBag.MisMatched = B;
                                    ViewBag.MisMatched_wdv = B_wdv;
                                    ViewBag.NotVerifiedInLoc = C;
                                    ViewBag.NotVerifiedInLoc_wdv = C_wdv;
                                    ViewBag.NotInFA = NotInFAR;
                                    ViewBag.FileCount = AsFileImp;
                                    Model.inserttopv(txtfilename, (A + B + C).ToString(), (FARegCount).ToString(), A.ToString(), B.ToString(), C.ToString(), NotInFAR.ToString(), AsFileImp.ToString(), (A_wdv + B_wdv + C_wdv).ToString(), (FARegWDV).ToString(), A_wdv.ToString(), B_wdv.ToString(), C_wdv.ToString(), "", "");
                                }
                                //  List<PhysicalVerificationAsset> PVlist = new List<PhysicalVerificationAsset>();
                                //  PVlist = Verify.PVReport.Where(x => txtyear == null || (x.FinancialYear.ToUpper().Contains(txtyear.ToUpper()))).ToList();
                                //  PVlist.Select(m => new { m.Status }).Distinct(); 
                                ////  int s = Verify.PVReport.Where(x => txtyear == null || (x.Status.ToUpper().Contains("Verified"))).Count();
                                // // int s = PVlist.Select(m => new { m.Status }).Contains("").; 
                                //  int s = PVlist.Count(x => x.Status.Equals("Verified"));
                                //  int s1 = PVlist.Count(x => x.Status.Equals("Not Verified"));
                                // // records.Count(x => x.Line.Contains(".dwg"));
                            }
                            if (!string.IsNullOrEmpty(txtassetid))
                            {
                                ViewBag.txtassetid = txtassetid;
                                Verify.PVReport = Verify.PVReport.Where(x => txtassetid == null || (x.AssetID.ToUpper().Contains(txtassetid.ToUpper()))).ToList();
                            }
                            if (!string.IsNullOrEmpty(txtlocation))
                            {
                                ViewBag.txtlocation = txtlocation;
                                Verify.PVReport = Verify.PVReport.Where(x => txtlocation == null || (x.Branch.ToUpper().Contains(txtlocation.ToUpper()))).ToList();
                                int LocFA = 0;
                                if (txtlocation != "")
                                {
                                    var Summary = Model.SummaryLoc(txtyear, txtlocation);
                                    // int NotinBranch = Model.NotinLoc(txtlocation);
                                    foreach (var data in Summary)
                                    {
                                        LocFA = data.AsPerFA;
                                        Verified = data.FAMatched;
                                        NotVerified = data.FAMissMatched;
                                        NotInFAR = data.NotinFA;
                                        // AsFileImp = data.TotalFileLoc;
                                        AsFileImp = data.NoOfRecords;
                                    }
                                    //ViewBag.Verified = Verified;
                                    //ViewBag.NotVerified = NotVerified;
                                    //ViewBag.NotInFAR = NotInFAR;
                                    //ViewBag.AsFileImp = AsFileImp;
                                    ViewBag.txtlocation = txtlocation;
                                    Verify.PVReport = Verify.PVReport.Where(x => txtlocation == null || (x.Branch.ToUpper().Equals(txtlocation.ToUpper()))).ToList();
                                    List<PhysicalVerificationAsset> PVlist1 = new List<PhysicalVerificationAsset>();
                                    PVlist1 = Verify.PVReport.ToList();
                                    int FileCount1 = PVlist1.Count();
                                    var P = Model.PVImportSerach();
                                    int A = 0, B = 0, C = 0, D = 0;
                                    decimal A_wdv = 0, B_wdv = 0, C_wdv = 0, D_wdv = 0;
                                    int FAOverall = 0, FALocation = 0, Matched = 0, MisMatched = 0, NotVerifiedInLoc = 0, NotInFA = 0;
                                    decimal FAOverall_wdv = 0, FALocation_wdv = 0, Matched_wdv = 0, MisMatched_wdv = 0, NotVerifiedInLoc_wdv = 0, NotInFA_wdv = 0;
                                    //string branch = "";
                                    string filename = "";
                                    foreach (var pv in P)
                                    {
                                        if (pv.Status == "Verified" && pv.Branch == txtlocation)
                                        {
                                            filename = pv.FileName;
                                        }
                                    }
                                    foreach (var pv in P)
                                    {
                                        string t = pv.FileName;
                                        if (pv.Status == "Verified" && pv.Branch == txtlocation)
                                        {
                                            A = A + 1;
                                            A_wdv += Convert.ToInt32(pv.wdv);
                                        }
                                        if (filename != "")
                                        {
                                            if (pv.Status == "Not Verified" && pv.FileName == filename)
                                            {
                                                B_wdv += Convert.ToInt32(pv.wdv);
                                                B = B + 1;
                                            }
                                        }
                                        if (pv.Branch == txtlocation && pv.Status == "Not Verified" && pv.FileName == "")
                                        {
                                            C_wdv += Convert.ToInt32(pv.wdv);
                                            C = C + 1;
                                        }
                                        if (filename != "")
                                        {
                                            if (pv.Status == "Not in FA Register" && pv.FileName == filename)
                                            {
                                                D_wdv += Convert.ToInt32(pv.wdv);
                                                D = D + 1;
                                            }
                                        }
                                    }
                                    int loc = Model.FileByLoc(txtlocation);
                                    FAOverall = A + B + C;
                                    FAOverall_wdv = A_wdv + B_wdv + C_wdv;
                                    FALocation = A + C;
                                    FALocation_wdv = A_wdv + C_wdv;
                                    Matched = A;
                                    Matched_wdv = A_wdv;
                                    MisMatched = B;
                                    MisMatched_wdv = B_wdv;
                                    NotVerifiedInLoc = C;
                                    NotVerifiedInLoc_wdv = C_wdv;
                                    NotInFA = D;
                                    NotInFA_wdv = D_wdv;
                                    ViewBag.FAOverAll = FAOverall;
                                    ViewBag.FALocation = FALocation;
                                    ViewBag.Verified = Matched;
                                    ViewBag.MisMatched = MisMatched;
                                    ViewBag.NotVerifiedInLoc = NotVerifiedInLoc;
                                    ViewBag.NotInFA = NotInFA;
                                    ViewBag.FileCount = A + B + D;
                                    ViewBag.FAOverAll_wdv = FAOverall_wdv;
                                    ViewBag.FALocation_wdv = FALocation_wdv;
                                    ViewBag.Verified_wdv = Matched_wdv;
                                    ViewBag.MisMatched_wdv = MisMatched_wdv;
                                    ViewBag.NotVerifiedInLoc_wdv = NotVerifiedInLoc_wdv;
                                    ViewBag.NotInFA_wdv = NotInFA_wdv;
                                    ViewBag.FileCount_wdv = A_wdv + B_wdv + D_wdv;
                                    Model.inserttopv(txtfilename, FAOverall.ToString(), FALocation.ToString(), Matched.ToString(), MisMatched.ToString(), NotVerifiedInLoc.ToString(), NotInFA.ToString(), (A + B + D).ToString(), FAOverall_wdv.ToString(), FALocation_wdv.ToString(), Matched_wdv.ToString(), MisMatched_wdv.ToString(), NotVerifiedInLoc_wdv.ToString(), NotInFA_wdv.ToString(), (A_wdv + B_wdv + D_wdv).ToString());
                                }
                            }
                            if (!string.IsNullOrEmpty(txtbarcode))
                            {
                                ViewBag.txtbarcode = txtbarcode;
                                Verify.PVReport = Verify.PVReport.Where(x => txtbarcode == null || (x.Barcode.ToUpper().Contains(txtbarcode.ToUpper()))).ToList();
                            }
                            if (!string.IsNullOrEmpty(txtfilename))
                            {
                                ViewBag.txtfilename = txtfilename;
                                Verify.PVReport = Verify.PVReport.Where(x => txtfilename == null || (x.FileName.ToUpper().Contains(txtfilename.ToUpper()))).ToList();
                                if (!string.IsNullOrWhiteSpace(txtfilename))
                                {
                                    ViewBag.txtfilename = txtfilename;
                                    Verify.PVReport = Verify.PVReport.Where(x => txtfilename == null || (x.FileName.ToUpper().Equals(txtfilename.ToUpper()))).ToList();
                                    List<PhysicalVerificationAsset> PVlist = new List<PhysicalVerificationAsset>();
                                    PVlist = Verify.PVReport.ToList();
                                    int FileCount = PVlist.Count();
                                    var P = Model.PVImportSerach();
                                    int A = 0, B = 0, C = 0, D = 0;
                                    decimal A_wdv = 0, B_wdv = 0, C_wdv = 0, D_wdv = 0;
                                    int FAOverall = 0, FALocation = 0, Matched = 0, MisMatched = 0, NotVerifiedInLoc = 0, NotInFA = 0;
                                    decimal FAOverall_wdv = 0, FALocation_wdv = 0, Matched_wdv = 0, MisMatched_wdv = 0, NotVerifiedInLoc_wdv = 0, NotInFA_wdv = 0;
                                    string branch = "";
                                    foreach (var pv in P)
                                    {
                                        if (pv.Status == "Verified" && pv.FileName == txtfilename)
                                        {
                                            branch = pv.Branch;
                                        }
                                    }
                                    foreach (var pv in P)
                                    {
                                        if (pv.Status == "Verified" && pv.FileName == txtfilename)
                                        {
                                            A = A + 1;
                                            A_wdv += Convert.ToInt32(pv.wdv);
                                        }
                                        if (pv.Status == "Not Verified" && pv.FileName == txtfilename)
                                        {
                                            B_wdv += Convert.ToInt32(pv.wdv);
                                            B = B + 1;
                                        }
                                        if (branch != "")
                                        {
                                            if (pv.Branch == branch && pv.Status == "Not Verified" && pv.FileName != txtfilename)
                                            {
                                                C = C + 1;
                                                C_wdv += Convert.ToInt32(pv.wdv);
                                            }
                                        }
                                        if (pv.Status == "Not in FA Register" && pv.FileName == txtfilename)
                                        {
                                            D = D + 1;
                                            D_wdv += Convert.ToInt32(pv.wdv);
                                        }
                                    }
                                    int loc = Model.FileByLoc(txtfilename);
                                    //C = loc - A;
                                    //FAOverall = A + B + C;
                                    //FALocation = A + C;
                                    //Matched = A;
                                    //MisMatched = B;
                                    //NotVerifiedInLoc = C;
                                    //NotInFA = D;
                                    FAOverall = A + B + C;
                                    FAOverall_wdv = A_wdv + B_wdv + C_wdv;
                                    FALocation = A + C;
                                    FALocation_wdv = A_wdv + C_wdv;
                                    Matched = A;
                                    Matched_wdv = A_wdv;
                                    MisMatched = B;
                                    MisMatched_wdv = B_wdv;
                                    NotVerifiedInLoc = C;
                                    NotVerifiedInLoc_wdv = C_wdv;
                                    NotInFA = D;
                                    NotInFA_wdv = D_wdv;
                                    //ViewBag.FAOverAll = FAOverall;
                                    //ViewBag.FALocation = FALocation;
                                    //ViewBag.Verified = Matched;
                                    //ViewBag.MisMatched = MisMatched;
                                    //ViewBag.NotVerifiedInLoc = NotVerifiedInLoc;
                                    //ViewBag.NotInFA = NotInFA;
                                    //ViewBag.FileCount = FileCount;
                                    ViewBag.FAOverAll = FAOverall;
                                    ViewBag.FALocation = FALocation;
                                    ViewBag.Verified = Matched;
                                    ViewBag.MisMatched = MisMatched;
                                    ViewBag.NotVerifiedInLoc = NotVerifiedInLoc;
                                    ViewBag.NotInFA = NotInFA;
                                    ViewBag.FileCount = A + B + D;
                                    ViewBag.FAOverAll_wdv = FAOverall_wdv;
                                    ViewBag.FALocation_wdv = FALocation_wdv;
                                    ViewBag.Verified_wdv = Matched_wdv;
                                    ViewBag.MisMatched_wdv = MisMatched_wdv;
                                    ViewBag.NotVerifiedInLoc_wdv = NotVerifiedInLoc_wdv;
                                    ViewBag.NotInFA_wdv = NotInFA_wdv;
                                    ViewBag.FileCount_wdv = A_wdv + B_wdv + D_wdv;
                                    Model.inserttopv(txtfilename, FAOverall.ToString(), FALocation.ToString(), Matched.ToString(), MisMatched.ToString(), NotVerifiedInLoc.ToString(), NotInFA.ToString(), (A + B + D).ToString(), FAOverall_wdv.ToString(), FALocation_wdv.ToString(), Matched_wdv.ToString(), MisMatched_wdv.ToString(), NotVerifiedInLoc_wdv.ToString(), NotInFA_wdv.ToString(), (A_wdv + B_wdv + D_wdv).ToString());
                                    //int RecCount = PVlist.Count();
                                    //int V = PVlist.Count(x => x.Status.Equals("Verified"));
                                    //int NotVer = PVlist.Count(x => x.Status.Equals("Not Verified"));
                                    //int NotInFAR1 = PVlist.Count(x => x.Status.Equals("Not in FA Register"));
                                    //int fa = Model.FileByLoc(txtfilename);
                                    //ViewBag.AsPerFACount = fa;
                                    //ViewBag.AsFileImp = RecCount;
                                    //ViewBag.Verified = V;
                                    //ViewBag.NotVerified = NotVer;
                                    //ViewBag.NotInFAR = NotInFA;
                                }
                            }
                            if (!string.IsNullOrEmpty(txtstatus))
                            {
                                if (txtstatus == "-----Select------" || txtstatus == "0")
                                {
                                    ViewBag.DropStatus = 0;
                                    txtstatus = "";
                                }
                                Verify.PVReport = Verify.PVReport.Where(x => txtstatus == null || (x.Status.ToUpper().Contains(txtstatus.ToUpper()))).ToList();
                                if (txtstatus != "" && txtstatus != "0" && txtstatus != "-----Select------")
                                {
                                    ViewBag.DropStatus = txtstatus;
                                    Verify.PVReport = Verify.PVReport.Where(x => txtstatus == null || (x.Status.ToUpper().Equals(txtstatus.ToUpper()))).ToList();
                                }
                            }
                            Session["Verify.PVReport"] = Verify.PVReport;
                            if (Verify.PVReport.Count == 0)
                            {
                                ViewBag.Message = "No records found";
                            }
                        }
                    }
                }
                if (command == "Clear")
                {
                    return RedirectToAction("PVReport", "PhysicalVerificationAsset");
                }
                if (command == "Download")
                {
                    IfamsPhysicalVerificationModel Model11 = new IfamsPhysicalVerificationModel();
                    Model11.PrintRecon();
                    Verify.PVReport = (List<PhysicalVerificationAsset>)Session["Verify.PVReport"];
                }
                return View(Verify);
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

        public ActionResult PhysicalAssetdownloadsexcel()
        {
            try
            {
                using (DataTable dtnew = new DataTable())
                {
                    dtnew.Columns.Add("SNo");
                    dtnew.Columns.Add("Barcode");
                    dtnew.Columns.Add("Branch");
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtnew, "PhysicalVerificaiton_Template");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=PhysicalVerificaiton_Template.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {

            }
            return View();
        }


        //public ActionResult PhysicalAssetdownloadsexcel()
        //{
        //    try
        //    {
        //        DataTable dtnew = new DataTable();
        //        dtnew.Columns.Add("SNo");
        //        dtnew.Columns.Add("Barcode");
        //        dtnew.Columns.Add("Location Code");
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Verificaiton_Template1.xls"));
        //        Response.ContentType = "application/ms-excel";
        //        DataTable dt = dtnew;
        //        string str = string.Empty;
        //        foreach (DataColumn dtcol in dt.Columns)
        //        {
        //            Response.Write(str + dtcol.ColumnName);
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            str = "";
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                Response.Write(str + Convert.ToString(dr[j]));
        //                str = "\t";
        //            }
        //            Response.Write("\n");
        //        }
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return View();
        //}
        public async Task<JsonResult> UploadFilesnew()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["RAISERattmtfileexcel"] = hpf;
                        Session["orginal"] = hpf.FileName;
                        string f = hpf.FileName.ToString();
                    }
                }
                return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Upload failed");
            }
            finally
            {
            }
        }
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["RAISERattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["RAISERattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    Session["PVFileName"] = Extension.ToString();
                    string n = string.Format("TOA{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["filename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                    // path1 = @"C:\temp\" + n + " _ " + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SheetData> objparent = new List<SheetData>();
                SheetData objModel;
                int count = 0;
                string sheets = "";
                FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                DataSet result1 = new DataSet();
                if (Extension1 == ".xlsx")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else if (Extension1 == ".xls")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = error.ToString();
                    objparent.Add(objModel);
                }
                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = sheets.ToString();
                    objparent.Add(objModel);
                }
                Session["Tempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }

        [HttpPost]
        public ActionResult Print(PhysicalVerificationAsset obj)
        {
            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            try
            {
                Model.PrintRecon();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

        public JsonResult Makerupdate(PhysicalVerificationAsset obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                int year = DateTime.Now.Year;
                DataModel DM = new DataModel();
                string FinancialYear = DM.toGetFincialyear(); ;
                if (obj.FinancialYear.ToString() == FinancialYear.ToString())
                {
                    DataSet result1 = new DataSet();
                    result1 = (DataSet)Session["Tempdataset"];
                    var dataTable = new DataTable();
                    dataTable = result1.Tables[obj.SheetName.ToString()];
                    foreach (DataColumn dtColumn in dataTable.Columns)
                    {
                        strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                    }
                    strCols = strCols.Substring(0, strCols.Length - 1);
                    strColArr = strCols.Split(':');
                    if (strColArr[0].ToString() == "SNo"
                        && strColArr[1].ToString() == "Barcode"
                        && strColArr[2].ToString() == "Branch")
                    {
                        mag = "Success";
                    }
                    else
                    {
                        mag = "Faild";
                    }
                    if (mag == "Success")
                    {
                        int count = dataTable.Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (dataTable.Rows[i]["SNo"].ToString() == "" && dataTable.Rows[i]["Barcode"].ToString() == "" && dataTable.Rows[i]["Branch"].ToString() == "")
                            {
                                dataTable.Rows[i].Delete();
                                dataTable.AcceptChanges();
                                count--;
                                i--;
                            }
                        }
                        Session["Tempdatatable"] = dataTable;
                    }
                }
                else
                {
                    mag = "Please Select Current Financial Year";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult Uploadstatus(string ddlSelectsheet, string Financialyear)
        {
            try
            {
                if (Session["Tempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatable"];
                    datasupload(errdataval, ddlSelectsheet, Financialyear);
                    errdataval = (DataTable)Session["Errdatatable"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(string.Empty);
        }

        private void datasupload(DataTable dataTable, string SheetName, string Financialyear)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string duplicateasset = "";
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");

            string FileName1 = Session["PVFileName"].ToString();

            SheetName = FileName1;
            string AssetIdstatus = "";
            string AssetCodestatus = "";

            IfamsPhysicalVerificationModel Model = new IfamsPhysicalVerificationModel();
            try
            {




                if (Model.CheckLocationPV(Financialyear, dataTable.Rows[0][2].ToString()) == "Exists")
                {
                    Model.UpdatePV(Financialyear, dataTable.Rows[0][2].ToString());

                }
                else
                {
                    string FileName = Model.SheetExists(Financialyear, SheetName);
                    if (SheetName.ToString() == FileName.ToString())
                    {
                        string len = "Verificaiton_Template";
                        int index = len.Length;
                        if (index != -1)
                        {
                            string x = SheetName.Substring(index);
                            int y = Convert.ToInt32(x);
                            y = y + 1;
                            SheetName = len + Convert.ToString(y);
                        }

                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string assetdata = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();
                        if (duplicateasset.Contains(assetdata) == true)
                        {

                            errs = "Duplicate Barcode Found";
                            //AssetIdstatus = "notexists";
                        }
                        else
                        {
                            duplicateasset += duplicateasset == "" ? assetdata : ("," + assetdata);
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Barcode must not be empty";
                                // AssetIdstatus = "notexists";
                            }
                            else
                            {
                                assetdata = Model.GetAssetId(dataTable.Rows[i][1].ToString());
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = Model.GetStatusexcel(assetdata, assetdata, "PVAssetId");
                                //AssetIdstatus = status;
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Asset Id Was Not Found";

                                    }
                                    else
                                    {
                                        errs = errs + "," + "Asset Id Was Not Found ";


                                    }
                                }

                            }


                            if (dataTable.Rows[i][2].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch mismatch";
                                    //AssetCodestatus = "notexists";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch mismatch";
                                    //AssetCodestatus = "notexists";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][2].ToString();
                                status = Model.GetStatusexcel(assetdata, exceltext, "AssetLoc");
                                AssetCodestatus = status;
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Branch mismatch";

                                    }
                                    else
                                    {
                                        errs = errs + "," + "Branch mismatch";

                                    }
                                }
                            }



                        }


                        string PVStatus = Model.GetPVStatus(dataTable.Rows[i][1].ToString(), dataTable.Rows[i][2].ToString());
                        if (PVStatus == "Verified")
                        {

                            Model.InsertPV(assetdata, exceltext, PVStatus, Financialyear, SheetName, dataTable.Rows[i][1].ToString());

                        }
                        else if (PVStatus.ToString() == "Not Verified")
                        {

                            Model.InsertPV(assetdata, exceltext, PVStatus, Financialyear, SheetName, dataTable.Rows[i][1].ToString());
                        }
                        else if (PVStatus.ToString() == "Not in FA Register")
                        {
                            Model.InsertPV(assetdata, exceltext, PVStatus, Financialyear, SheetName, dataTable.Rows[i][1].ToString());
                        }


                        if (errs != "")
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, RowNo, errs);
                        }

                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please Select Valid Sheet");
                }
                duplicateasset = "";
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;
                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
        }


        public ActionResult downloadsexcel()
        {
            try
            {
                Session["err"] = "";
                DataTable dtnew = (DataTable)Session["Errdatatable"];
                DataTable dtmainnew = (DataTable)Session["maindatatable"];
                dtmainnew.Columns.Add("Error Description");
                for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                        if (i == line)
                            dtmainnew.Rows[line - 1]["Error Description"] = dtnew.Rows[j]["Error Description"];
                    }
                DataTable dt = dtmainnew;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "PhysicalVerification_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=PhysicalVerification_ErrorLog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Transfer_ErrorLog.xls"));
                //Response.ContentType = "application/ms-excel";
                //DataTable dt = dtnew;
                //string str = string.Empty;
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write(str + dtcol.ColumnName);
                //    str = "\t";
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + Convert.ToString(dr[j]));
                //        str = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

        public PartialViewResult Recon()
        {
            return PartialView();
        }
        public ActionResult PVRExceldownload()
        {

            List<PhysicalVerificationAsset> cList;
            cList = (List<PhysicalVerificationAsset>)Session["Verify.PVReport"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Barcode Number");
            dt.Columns.Add("Asset Id");
            dt.Columns.Add("Asset Category");
            dt.Columns.Add("Asset Sub-Category");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Status");
           // dt.Columns.Add("Remarks");
            dt.Columns.Add("File Name");
            dt.Columns.Add("ReceiptDate");
            dt.Columns.Add("Financial Year");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].Barcode.ToString()
                , cList[i].AssetID.ToString()
                , cList[i].Asset_Category.ToString()
                , cList[i].Asset_Sub_Category.ToString()
                , cList[i].Branch.ToString()
                , cList[i].Status.ToString()
               // , cList[i].ReMarks.ToString()
                , cList[i].FileName.ToString()
                , cList[i].ReceiptDate.ToString()
                , cList[i].FinancialYear.ToString());
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "PhysicalVerification of Asset Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
