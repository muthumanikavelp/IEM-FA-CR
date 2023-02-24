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
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace IEM.Areas.IFAMS.Controllers
{
    public class AssetGroupingController : Controller
    {
        //
        // GET: /IFAMS/AssetGrouping/
        private IRepository ifr;
        private CmnFunctions objCmnFunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public AssetGroupingController() :
            this(new DataModel())
        {

        }
        public AssetGroupingController(IRepository objModel)
        {
            ifr = objModel;
        }

        public ActionResult GMSummary(string command, string GroupId)
        {
            GroupModel gm = new GroupModel();
            try
            {
                Session["Searchasset"] = null;
                gm.GModel = ifr.GetDetailForMkrSummary(Convert.ToString(objCmnFunc.GetLoginUserGid())).ToList();
                if (command == "SEARCH")
                {
                    if (GroupId != null)
                    {
                        ViewBag.ecfno = GroupId;
                        gm.GModel = gm.GModel.Where(x => GroupId.ToUpper() == null || (x._group_id.ToUpper().Contains(GroupId.ToUpper()))).ToList();

                    }

                }
                if (command == "CLEAR")
                {
                    gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
                }
                if (gm.GModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["AssetGroupingExceldownload"] = gm.GModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(gm);
            }
            finally
            {
            }
            return View(gm);
        }




        public ActionResult GCSummary(string command, string GroupId)
        {
            GroupModel gm = new GroupModel();
            try
            {
                gm.GModel = ifr.GetDetailForChkSummary().ToList();
                if (command == "SEARCH")
                {
                    if (GroupId != null)
                    {
                        ViewBag.ecfno = GroupId;
                        gm.GModel = gm.GModel.Where(x => GroupId.ToUpper() == null || (x._group_id.ToUpper().Contains(GroupId.ToUpper()))).ToList();

                    }

                }
                if (command == "CLEAR")
                {
                    gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
                }
                if (gm.GModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(gm);
            }
            finally
            {
            }
            return View(gm);
        }

        [HttpGet]
        public PartialViewResult GroupView(string id, string viewfor)
        {
            GroupModel gm = new GroupModel();
            try
            {
                Session["gid"] = Convert.ToInt32(id);
                Session["grp"] = null;
                if (viewfor != null)
                    ViewBag.viewfor = "checkerView";
                gm.GModelView = ifr.GetDetailForView(id);
                if (id != null)
                {
                    gm._group_id = id.ToString();
                }
                if (gm.GModelView.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return PartialView(gm);
        }








        [HttpPost]
        public JsonResult GroupView(string command, string no, string CHECHKER, string remark)
        {
            string status = "";
            try
            {
                if (command == "Reject")
                {
                    status = "REJECTED";
                    CHECHKER = ifr.ApproveRejectGrp(no, status, remark);
                }
                if (command == "Approve")
                {
                    status = "APPROVED";
                    CHECHKER = ifr.ApproveRejectGrp(no, status, remark);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(CHECHKER, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult CreateGroup(string listfor, string grpid)
        {
            GroupModel gm = new GroupModel();
            try
            {
                Session["gid"] = Convert.ToInt32(grpid);
                gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
                if (grpid != null)
                {
                    gm._group_id = grpid.ToString();
                }
                if (listfor == "SEARCH")
                {
                    if (Session["Searchasset"] != null)
                    {
                        gm.GModel = (List<GroupModel>)Session["Searchasset"];
                        Session["Searchasset"] = null;
                    }
                }
                if (gm.GModel.Count == 0)
                {
                    ViewBag.RecordMessage = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(gm);
            }
            finally
            {
            }
            return PartialView(gm);
        }




        //[HttpGet]
        //public ActionResult CreateGroupNew(string listfor, string grpid)
        //{
        //    GroupModel gm = new GroupModel();
        //    try
        //    {
        //        Session["gid"] = Convert.ToInt32(grpid);
        //        gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
        //        if (grpid != null)
        //        {
        //            gm._group_id = grpid.ToString();
        //        }
        //        if (listfor == "SEARCH")
        //        {
        //            if (Session["Searchasset"] != null)
        //            {
        //                gm.GModel = (List<GroupModel>)Session["Searchasset"];
        //                Session["Searchasset"] = null;
        //            }
        //        }
        //        if (gm.GModel.Count == 0)
        //        {
        //            ViewBag.RecordMessage = "No records found";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return View(gm);
        //    }
        //    finally
        //    {
        //    }
        //    return View(gm);
        //}





        [HttpPost]
        public JsonResult CreateGroup(string command, GroupModel model, string[] ids)
        {
            GroupModel gm = new GroupModel();
            string capdate = model._cap_date;
            string location = model._loc;
            try
            {
                gm.GModel = ifr.GetDetailToGroupsearch(location, capdate).ToList();
                //if (command == "SEARCH")
                //{
                //    if (location != null)
                //    {
                //        ViewBag.ecfno = location;
                //        gm.GModel = gm.GModel.Where(x => location.ToUpper() == null || (x._loc.ToUpper().Equals(location.ToUpper()))).ToList();
                //        Session["Searchasset"] = gm.GModel;
                //    }
                //    if (capdate != null)
                //    {
                //        ViewBag.capdate = capdate;
                //        gm.GModel = gm.GModel.Where(x => capdate == null || (x._cap_date.ToUpper().Contains(capdate.ToUpper()))).ToList();
                //        Session["Searchasset"] = gm.GModel;
                //    }
                //}
                Session["Searchasset"] = gm.GModel;
                ViewBag.ecfno = location;
                ViewBag.capdate = capdate;
                if (command == "CLEAR")
                {
                    gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
                }
                if (command == "SUBMIT")
                {

                }
                if (gm.GModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(gm, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SubmitasGroup(BarcodeGenerationModel model, string ids, string grpid)
        {
            string result = string.Empty;
            string Maker = string.Empty;
            Maker = objCmnFunc.authorize("264");
            try
            {
                if (Maker == "Success")
                {
                    if (ModelState.IsValid)
                    {
                        if (ids != "")
                        {
                            result = ifr.GenerateGoupId(ids, grpid);

                        }
                        else
                        {
                            result = "Error";
                        }

                    }
                }
                else
                {
                    result = "Unauthorized User!";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            // return PartialView("CreateGroup", "AssetGrouping");
            return Json(result, JsonRequestBehavior.AllowGet);
        
        }
         [HttpGet]
        public ActionResult CreateGroupnew()
        {
            GroupModel gm = new GroupModel();
            gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
            if (Session["Searchasset"] != null)
            {
                gm.GModel = (List<GroupModel>)Session["Searchasset"];
                
            }
            return View(gm);
        }
        [HttpPost]
        public ActionResult CreateGroupnew(string command, string LocationCode, string[] ids)
        {
            GroupModel gm = new GroupModel();
            //string capdate = model._cap_date;
            string location = LocationCode;
            Session["Searchasset"] = null;
            try
            {
                gm.GModel = ifr.GetDetailToGroupsearch(location, "").ToList();
                Session["Searchasset"] = gm.GModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View(gm);
        }
        [HttpPost]
        public JsonResult GetAssetDetails(string LocationCode = null, string capdate = null)
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = ifr.GetAssetDetails(LocationCode, capdate);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [HttpGet]
        public ActionResult EditCreateGroupnew(string grpid)
        {
            ViewBag.groupid = grpid;
            GroupModel gm = new GroupModel();
            gm.GModel = Enumerable.Empty<GroupModel>().ToList<GroupModel>();
            if (Session["Searchasset"] != null)
            {
                gm.GModel = (List<GroupModel>)Session["Searchasset"];

            }
            return View(gm);
        }
        [HttpPost]
        public ActionResult EditCreateGroupnew(string grpid,string command, string LocationCode, string[] ids)
        {
            ViewBag.groupid = grpid;
            GroupModel gm = new GroupModel();
            //string capdate = model._cap_date;
            string location = LocationCode;
            Session["Searchasset"] = null;
            try
            {
                gm.GModel = ifr.GetDetailToGroupsearch(location, "").ToList();
                Session["Searchasset"] = gm.GModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View(gm);
        }
        public ActionResult AssetGroupingExceldownload()
        {

            List<GroupModel> cList;
            cList = (List<GroupModel>)Session["AssetGroupingExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Group ID Number");
            dt.Columns.Add("Asset Count");            
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i]._group_id.ToString()
                     , cList[i]._asset_count.ToString()
                       );
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Group ID Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
