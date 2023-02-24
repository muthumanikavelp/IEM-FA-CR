using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.Common;

namespace IEM.Areas.IFAMS.Controllers
{
    public class CapitalizationDateController : Controller
    {
        //
        // GET: /IFAMS/CapitalizationDate/
        // private IfamsAssetMergeRepository AssetMergeR;

        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public ActionResult CapitalizationDateMaker()
        {

            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                Capdate.capdat = Model.CapitalizationDateMaker();
                if (Capdate.capdat.Count == 0)
                { ViewBag.Message = "No Records Found"; }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }

            return View(Capdate);
        }



        [HttpPost]
        public ActionResult CapitalizationDateMaker(string filterID, string filterDate, string filterLoc, string command)
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                if (command == "SEARCH")
                {

                    Capdate.capdat = Model.CapitalizationDateMaker();
                    if ((string.IsNullOrEmpty(filterID) || string.IsNullOrWhiteSpace(filterID)) && (string.IsNullOrEmpty(filterDate) || string.IsNullOrWhiteSpace(filterDate)) && (string.IsNullOrEmpty(filterLoc) || string.IsNullOrWhiteSpace(filterLoc)))
                    {
                        Capdate.capdat = Model.CapitalizationDateMaker();
                    }
                    if (filterID != null)
                    {
                        ViewBag.filterID = filterID;
                        Capdate.capdat = Capdate.capdat.Where(x => filterID == null || (x.assetid.ToUpper().Contains(filterID.ToUpper()))).ToList();
                    }
                    if (filterDate != null)
                    {
                        ViewBag.filterDate = filterDate;
                        Capdate.capdat = Capdate.capdat.Where(x => filterDate == null || (x.capnewdate.ToUpper().Contains(filterDate.ToUpper()))).ToList();
                    }
                    if (filterLoc != null)
                    {
                        ViewBag.filterLoc = filterLoc;
                        Capdate.capdat = Capdate.capdat.Where(x => filterLoc == null || (x.location.ToUpper().Contains(filterLoc.ToUpper()))).ToList();
                    }
                    if (Capdate.capdat.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(Capdate);
                }

                if (command == "Clear")
                { return RedirectToAction("CapitalizationDateMaker", "CapitalizationDate"); }

                Session["Role"] = "IOACHK";

                return View(Capdate);
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



        public ActionResult CapitalizationDateChecker()
        {

            //captializationdatechecker capdate = new captializationdatechecker();
            //capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();
            //ViewBag.Message = "No Records Found";
            // return View(capdate);




            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                Capdate.capdat = Model.CapitalizationDateChecker();
                if (Capdate.capdat.Count == 0)
                { ViewBag.Message = "No Records Found"; }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }

            return View(Capdate);







        }




        [HttpPost]
        public ActionResult CapitalizationDateChecker(string filterID, string filterDate, string filterLoc, string command)
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                if (command == "SEARCH")
                {

                    Capdate.capdat = Model.CapitalizationDateChecker();
                    if ((string.IsNullOrEmpty(filterID) || string.IsNullOrWhiteSpace(filterID)) && (string.IsNullOrEmpty(filterDate) || string.IsNullOrWhiteSpace(filterDate)) && (string.IsNullOrEmpty(filterLoc) || string.IsNullOrWhiteSpace(filterLoc)))
                    {
                        Capdate.capdat = Model.CapitalizationDateChecker();
                    }
                    if (filterID != null)
                    {
                        ViewBag.filterID = filterID;
                        Capdate.capdat = Capdate.capdat.Where(x => filterID == null || (x.assetid.ToUpper().Contains(filterID.ToUpper()))).ToList();
                    }
                    if (filterDate != null)
                    {
                        ViewBag.filterDate = filterDate;
                        Capdate.capdat = Capdate.capdat.Where(x => filterDate == null || (x.capnewdate.ToUpper().Contains(filterDate.ToUpper()))).ToList();
                    }
                    if (filterLoc != null)
                    {
                        ViewBag.filterLoc = filterLoc;
                        Capdate.capdat = Capdate.capdat.Where(x => filterLoc == null || (x.location.ToUpper().Contains(filterLoc.ToUpper()))).ToList();
                    }
                    if (Capdate.capdat.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(Capdate);
                }

                if (command == "Clear")
                { return RedirectToAction("CapitalizationDateChecker", "CapitalizationDate"); }

                Session["Role"] = "IOACHK";

                return View(Capdate);
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
















        //[HttpGet]
        //public PartialViewResult ChangeCapDate()
        //{


        //    return PartialView();
        //}

        [HttpGet]
        public PartialViewResult ChangeCapDate()
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                Capdate.changedate = Enumerable.Empty<captializationdate>().ToList<captializationdate>();
                if (Capdate.changedate.Count == 0)
                { ViewBag.Message = "No Records Found"; }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }


            return PartialView(Capdate);
        }


        [HttpGet]
        public PartialViewResult CDAuditTrial()
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                //Capdate.changedate = Model.ChangeDateMaker();
                //if (Capdate.changedate.Count == 0)
                //{ ViewBag.Message = "No Records Found"; }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }


            return PartialView();
        }


        [HttpGet]
        public PartialViewResult ViewCapitalizationDate(string viewfor, string id)
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            Session["Assetdetgid"] = id.ToString();
            Session["Role"] = viewfor;
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {

                Capdate.viewchangedate = Model.ViewChangeDate(Convert.ToInt32(id));
                if (Capdate.viewchangedate.Count == 0)
                { ViewBag.Message = "no records found"; }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }


            return PartialView(Capdate);
        }











        [HttpPost]
        public PartialViewResult SearchChangeCapDate(captializationdate Mod)
        {
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            try
            {

                if (Mod.command == "SEARCH")
                {
                    Capdate.changedate = Model.ChangeDateMaker(Mod.location, Mod.assetid);
                 //   Session["AssetId"] = Mod.assetid.ToString();

                    if (Capdate.changedate.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                }

               

                return PartialView("ChangeCapDate", Capdate);
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();

            }
            finally
            {

            }



        }




        [HttpPost]
        public JsonResult ChangeCapDateMaker(captializationdate Mod)
        {
            string Message = "";
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            string Maker = string.Empty;
            Maker = objCmnFunctions.authorize("249");
            // capdate.capdat = Enumerable.Empty<captializationdatechecker>().ToList<captializationdatechecker>();

            try
            {
                if (Maker == "Success")
                {
                    Message = Model.ChangeCapdateMaker(Mod.capnewdate.ToString() + "," + Mod.capchangenewdate.ToString(), Mod.assetgid.ToString());
                }
                else
                {
                    Message = "Unauthorized User!";
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }


            return Json(Message, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UpdateCapdateChecker(string ids)
        {
            string Message = "";
            string CheckedIds = "";
            string[] CheckedValues;
            string Checker = string.Empty;
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            Checker = objCmnFunctions.authorize("250");
            int index = ids.IndexOf("undefined");
            if (index != -1)
            {
                CheckedIds = ids.Remove(index);
            }

            try
            {
                if (Checker == "Success")
                {
                    CheckedValues = CheckedIds.Split(',').ToArray();
                    Message = Model.ChangeCapdateMaker(CheckedValues);
                }

                else
                {
                    Message = "Unauthorized User!";
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }


            return Json(Message, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult RejectUpdateCapdateChecker(string ids)
        {
            string Message = "";
            string CheckedIds = "";
            string[] CheckedValues;
            string Checker = string.Empty;
            Checker = objCmnFunctions.authorize("250");
            captializationdate Capdate = new captializationdate();
            IfamsAssetMergeDataModel Model = new IfamsAssetMergeDataModel();
            int index = ids.IndexOf("undefined");
            if (index != -1)
            {
                CheckedIds = ids.Remove(index);
            }

            try
            {
                if (Checker == "Success")
                {
                    CheckedValues = CheckedIds.Split(',').ToArray();
                    Message = Model.RejChangeCapdateMaker(CheckedValues);
                }
                else
                {
                    Message = "Unauthorized User!";
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }


            return Json(Message, JsonRequestBehavior.AllowGet);
        }

    }
}
