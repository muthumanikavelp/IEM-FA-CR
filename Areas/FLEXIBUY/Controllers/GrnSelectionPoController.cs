using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;
using System.Globalization;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class GrnSelectionPoController : Controller
    {
        //
        // GET: /FLEXIBUY/GrnSelectionPo/
        private Irepositorypr objModel;
        ErrorLog objErrorLog = new ErrorLog();
        public GrnSelectionPoController()
            : this(new fb_DataModelpr())
        {

        }
        public GrnSelectionPoController(Irepositorypr objM)
        {

            objModel = objM;
        }
        [HttpGet]
        public ActionResult GrnSelectionForPo(string viewfor, string grnType)
        {
            try
            {
                GRNInward objlist = new GRNInward();
                Session["attachment"] = null;
                Session["attachmentdetails"] = null;
                Session["AccessToken"] = null;
                Session["viewfor"] = null;
                Session["grnTable"] = null;
                if (viewfor == "refresh")
                {
                    Session["list"] = null;
                }
                Session["grnType"] = grnType;
                //if (grnType == "cwip")
                //{
                //    Session["viewfor"] = grnType;
                //    objlist.groupRes = objModel.GetReqGroup();
                //    if (objlist.groupRes == "IT" || objlist.groupRes == "PIP")
                //    {
                //    objlist.objInwardList = objModel.GetSelectionForPoCwip(objlist.groupRes);
                //    }
                //}
                //else
                //{
                objlist.objInwardList = objModel.GetSelectionForPo();
                //}
                if (Session["poselection"] != null)
                {
                    objlist.objInwardList = (List<GRNInward>)Session["poselection"];
                }
                if (objlist.objInwardList.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                return View(objlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GrnSelectionForPo(string podate, string command, string txtprrefno)
        {
            GRNInward objheader = new GRNInward();
            if (Session["grnType"] != null)
                objheader.grnType = (string)Session["grnType"];
            //if (objheader.grnType == "cwip")
            //{
            //    Session["viewfor"] = objheader.grnType;
            //    objheader.groupRes = objModel.GetReqGroup();
            //    if (objheader.groupRes == "IT" || objheader.groupRes == "PIP")
            //    {
            //        objheader.objInwardList = objModel.GetSelectionForPoCwip(objheader.groupRes);
            //    }
            //}
            //else
            //{
            objheader.objInwardList = objModel.GetSelectionForPo();
            //}
            Session["poselection"] = null;
            //  objheader.statusList = new SelectList(objModel.getStatusForGRN(), "statusgid", "statusname");
            if (command == "search")
            {

                if ((string.IsNullOrEmpty(podate)) == false)
                {
                    ViewBag.grndate = podate;
                    objheader.objInwardList = objheader.objInwardList.Where(x => podate == null ||
                        (x.poDate.Contains(podate))).ToList();
                    Session["grnheaderlist"] = objheader.objInwardList;
                }
                if ((string.IsNullOrEmpty(txtprrefno)) == false)
                {
                    ViewBag.grnrefno = txtprrefno;
                    objheader.objInwardList = objheader.objInwardList.Where(x => txtprrefno == null ||
                        (x.poRefNo.Contains(txtprrefno))).ToList();
                    Session["grnheaderlist"] = objheader.objInwardList;
                }

                if (objheader.objInwardList.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            if (objheader.objInwardList.Count == 0)
            {
                ViewBag.records = "No records Found";
            }
            return View(objheader);
        }
        [HttpGet]
        public ActionResult GrnInwardViewDetails(int grnheadGid, string viewfor, string grnType)
        {
            try
            {
                Session["viewfor"] = null;
                if (viewfor == "view")
                {
                    Session["viewfor"] = viewfor;
                }
                else if (viewfor == "edit")
                {
                    Session["viewfor"] = viewfor;
                }

                GRNInward obj = new GRNInward();
                obj.objInwardList = objModel.GetGRNDetailsForCwip(grnheadGid);
                //if (grnType == "direct")
                //{
                //    obj.objInwardList = objModel.GetGRNDetails(grnheadGid);

                //}
                //else
                //{
                //    obj.objInwardList = objModel.GetGRNDetailsForCwip(grnheadGid);
                //}
                Session["grninward"] = obj.objInwardList;
                obj.poRefNo = obj.objInwardList[0].poRefNo;
                obj.grnRefNo = obj.objInwardList[0].grnRefNo;
                obj.grnDcNo = obj.objInwardList[0].grnDcNo;
                obj.grnRemarks = obj.objInwardList[0].grnRemarks;
                obj.grnInvoiceNo = obj.objInwardList[0].grnInvoiceNo;
                obj.grnheadgid = grnheadGid;
                obj.grndetgid = obj.objInwardList[0].grndetgid;
                obj.grnReleaseGid = obj.objInwardList[0].grnReleaseGid;
                obj.raisedBy = objModel.GetLoginUser();
                obj.grnDate = DateTime.Now.ToShortDateString();
                obj.vendorName = obj.objInwardList[0].vendorName;
                return View("GrnInward", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult GrnInward(int poheadGid, string viewfor, string podetail)
        {
            try
            {
                Session["parheadergid"] = poheadGid;
                GRNInward obj = new GRNInward();
                int podetGid = Convert.ToInt32(podetail);
                //if (viewfor == "cwip")
                //{
                //    obj.objInwardList = objModel.GetPoDetailsForGRNCwip(poheadGid);
                //}
                //else 
                //{ 
                obj.objInwardList = objModel.GetPoDetailsForGRN(podetGid, viewfor);
                //}
                Session["grninward"] = obj.objInwardList;
                obj.poRefNo = obj.objInwardList[0].poRefNo;
                obj.poheadGid = poheadGid;
                obj.grnReleaseGid = obj.objInwardList[0].grnReleaseGid;
                obj.raisedBy = objModel.GetLoginUser();
                obj.grnDate = DateTime.Now.ToShortDateString();
                obj.vendorName = obj.objInwardList[0].vendorName;
                return View("GrnInward", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult GrnInwardDetails()
        {
            GRNInward obj = new GRNInward();
            if (Session["grndetails"] != null)
            {
                obj.objInwardList = (List<GRNInward>)Session["grndetails"];
                if (obj.objInwardList.Count > 0)
                    ViewBag.result = "checked";
            }

            if (Session["grndetails"] == null)
            {
                if (Session["grninward"] != null)
                    obj.objInwardList = (List<GRNInward>)Session["grninward"];

            }

            return PartialView("GrnInwardDetails", obj);
        }
        [HttpPost]
        public PartialViewResult GrnInwardDetails(GRNInward objInward)
        {
            DataTable dttb = new DataTable();
            DataTable dt = new DataTable();
            DataTable objTable = new DataTable();
            try
            {
                if (Session["grnTable"] != null)
                    dt = (DataTable)Session["grnTable"];
                dttb = (DataTable)Session["grnTable"];
                string[] words = objInward.podetailsGid.Split(',');
                foreach (string detgid in words)
                {
                    if (Session["grnTable"] != null)
                        dt = (DataTable)Session["grnTable"];

                    //DataRow[] foundrows;
                    //foundrows = dt.Select("grnreleaseforpo_gid='" + detgid + "'");
                    DataView dataview1 = new DataView();
                    dt.DefaultView.RowFilter = ("grnreleaseforpo_gid = '" + detgid + "'");
                    dataview1 = dt.DefaultView;
                    dt = dataview1.ToTable();
                    objTable.Merge(dt);
                    dttb.DefaultView.RowFilter = ("grnreleaseforpo_gid not in ('" + detgid + "')");
                    dataview1 = dttb.DefaultView;
                    dttb = dataview1.ToTable();
                }
                if (objTable.Rows.Count > 0)
                {
                    if (!objTable.Columns.Contains("received_qty"))
                        objTable.Columns.Add("received_qty");
                    if (!objTable.Columns.Contains("received_date"))
                        objTable.Columns.Add("received_date");
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        if (objInward.chkResult == "Released")
                        {
                            objTable.Rows[i]["received_qty"] = Convert.ToInt32(objTable.Rows[i]["grnreleaseforpo_released_qty"]);
                        }
                        else
                        {
                            if (objTable.Rows[i]["received_qty"].ToString() != "1")
                            {
                                objTable.Rows[i]["received_qty"] = 0;
                            }

                        }
                        objTable.Rows[i]["received_date"] = objInward.receivedDate;
                    }
                }
                dttb.Merge(objTable);
                Session["grnTable"] = dttb;
                objInward.objInwardList = objModel.GetPoDetailsTemp(dttb);
                if (objInward.objInwardList.Count > 0)
                    ViewBag.result = "checked";
                Session["grndetails"] = objInward.objInwardList;

                return PartialView("GrnInwardDetails", objInward);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult GrnInwardTempDetails()
        {
            GRNInward obj = new GRNInward();
            if (Session["grndetTemplist"] != null)
                obj.objInwardList = (List<GRNInward>)Session["grndetTemplist"];
            return PartialView("GrnInwardDetails", obj);

        }
        [HttpGet]
        public PartialViewResult GrnnewInwardTempDetails()
        {
            GRNInward objInward = new GRNInward();
            DataTable dt = new DataTable();
            if (Session["grninward"] != null)
                dt = (DataTable)Session["grnTable"];
            objInward.objInwardList = objModel.GetPoDetailsTemp(dt);
            //Session["grndetTemplist"] = objInward.objInwardList;
            return PartialView("GrnInwardDetails", objInward);

        }
        [HttpPost]
        public JsonResult GrnIncheckDetails(GRNInward objInward)
        {
            try
            {
                string result = "";
                string value = Convert.ToString(objInward.grnReleaseGid);
                result = objModel.Getchckdetails(value);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult GrnInwardTempDetails(GRNInward objInward)
        {
            DataTable dt = new DataTable();
            try
            {

                if (Session["grnTable"] != null)
                    dt = (DataTable)Session["grnTable"];
                //else if (Session["grndetTable"] != null)
                //    dt = (DataTable)Session["grndetTable"];
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("received_date"))
                    {
                        //dt.Columns.Add("received_qty");
                        dt.Columns.Add("received_date");
                    }
                    if (!dt.Columns.Contains("received_qty"))
                    {
                        dt.Columns.Add("received_qty");
                        //dt.Columns.Add("received_date");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["grnreleaseforpo_gid"].ToString() == Convert.ToString(objInward.grnReleaseGid))
                        {
                            dt.Rows[i]["received_qty"] = objInward.quantReceived;
                            dt.Rows[i]["received_date"] = objInward.receivedDate;
                            dt.Rows[i]["grnreleaseforpo_balanceqty"] = (Convert.ToDecimal(objInward.quantBalanced));
                        }
                    }
                }
                Session["grnTable"] = dt;
                objInward.objInwardList = objModel.GetPoDetailsTemp(dt);
                Session["grndetTemplist"] = objInward.objInwardList;
                return PartialView("GrnInwardDetails", objInward);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult SaveGrnDetails(GRNInward objgrnheader)
        {
            try
            {
                DataTable objTable = new DataTable();
                DataTable dt = new DataTable();
                DataTable dtAttach = new DataTable();

                if (Session["grnTable"] != null)
                    dt = (DataTable)Session["grnTable"];
                DataView dataview1 = new DataView();
                dt.DefaultView.RowFilter = ("received_qty <> '0'");
                dt.DefaultView.RowFilter = ("received_date <> ''");
                dataview1 = dt.DefaultView;
                dt = dataview1.ToTable();
                objTable.Merge(dt);
                //string[] words = objgrnheader.podetailsGid.Split(',');
                //foreach (string detgid in words)
                //{
                //    if (Session["grnTable"] != null)
                //        dt = (DataTable)Session["grnTable"];
                //    DataView dataview1 = new DataView();
                //    dt.DefaultView.RowFilter = ("grnreleaseforpo_gid = '" + detgid + "'");
                //    dataview1 = dt.DefaultView;
                //    dt = dataview1.ToTable();
                //    objTable.Merge(dt);
                //}

                if (Session["AccessToken"] != null)
                {
                    dtAttach = (DataTable)Session["AccessToken"];
                }
                //if (Session["grnTable"] != null)
                //objTable = (DataTable)Session["grnTable"];
                objgrnheader.result = objModel.InsertGrn(objgrnheader, objTable, dtAttach);
                if (objgrnheader.result != null)
                    return Json(objgrnheader.result, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult SaveGrnEditDetails(GRNInward objgrnheader)
        {
            try
            {
                DataTable objTable = new DataTable();
                DataTable dt = new DataTable();
                DataTable dtAttach = new DataTable();
                // Sugumar -2016-09-21 
                if (Session["grnTable"] != null)
                    dt = (DataTable)Session["grnTable"];
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("received_date"))
                    {
                        //dt.Columns.Add("received_qty");
                        dt.Columns.Add("received_date");
                    }
                    if (!dt.Columns.Contains("received_qty"))
                    {
                        dt.Columns.Add("received_qty");
                        //dt.Columns.Add("received_date");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // if (dt.Rows[i]["grnreleaseforpo_gid"].ToString() == Convert.ToString(objInward.grnReleaseGid))
                        //{
                        dt.Rows[i]["received_qty"] = dt.Rows[i]["grninwrddet_reced_qty"];
                        dt.Rows[i]["received_date"] = dt.Rows[i]["grnreleaseforpo_released_date"];
                        dt.Rows[i]["grnreleaseforpo_balanceqty"] = Convert.ToDecimal(dt.Rows[i]["grnreleaseforpo_balanceqty"]);
                        //}
                    }
                }
                DataView dataview1 = new DataView();
                dt.DefaultView.RowFilter = ("received_qty <> '0'");
                dt.DefaultView.RowFilter = ("received_date <> ''");
                dataview1 = dt.DefaultView;
                dt = dataview1.ToTable();
                objTable.Merge(dt);
                // Sugumar -2016-09-21 -end
                //string[] words = objgrnheader.podetailsGid.Split(',');
                //foreach (string detgid in words)
                //{
                //    if (Session["grnTable"] != null)
                //        dt = (DataTable)Session["grnTable"];
                //    DataView dataview1 = new DataView();
                //    dt.DefaultView.RowFilter = ("grnreleaseforpo_gid = '" + detgid + "'");
                //    dataview1 = dt.DefaultView;
                //    dt = dataview1.ToTable();
                //    objTable.Merge(dt);
                //}
                //if (Session["grnTable"] != null)
                //objTable = (DataTable)Session["grnTable"];
                //HttpContext.Current.Session["AccessToken"]
                if (Session["AccessToken"] != null)
                    dtAttach = (DataTable)Session["AccessToken"];
                objgrnheader.result = objModel.UpdateGrn(objgrnheader, objTable, dtAttach);
                if (objgrnheader.result != null)
                    return Json(objgrnheader.result, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult UpdateGrnEditDetails(GRNInward objgrnheader)
        {
            try
            {
                DataTable objTable = new DataTable();
                DataTable dtAttach = new DataTable();
                GRNInward obj = new GRNInward();
                if (Session["grnTable"] != null)
                    objTable = (DataTable)Session["grnTable"];
                if (Session["AccessToken"] != null)
                    dtAttach = (DataTable)Session["AccessToken"];
                if (objTable.Rows.Count > 0)
                {
                    if (!objTable.Columns.Contains("received_date"))
                    {
                        //dt.Columns.Add("received_qty");
                        objTable.Columns.Add("received_date");
                    }
                    if (!objTable.Columns.Contains("received_qty"))
                    {
                        objTable.Columns.Add("received_qty");
                        //dt.Columns.Add("received_date");
                    }
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        objTable.Rows[i]["received_qty"] = objTable.Rows[i]["grninwrddet_reced_qty"];
                        objTable.Rows[i]["received_date"] = objTable.Rows[i]["grnreleaseforpo_released_date"];
                        objTable.Rows[i]["grnreleaseforpo_balanceqty"] = Convert.ToDecimal(objTable.Rows[i]["grnreleaseforpo_balanceqty"]);
                    }
                }

                objgrnheader.result = objModel.UpdateGrn(objgrnheader, objTable, dtAttach);
                if (objgrnheader.result != null)
                {
                    return Json(objgrnheader.result, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult BoqAttach()
        {
            try
            {
                Session["cbfdetails"] = "";
                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                GRNInward objMAttachment = new GRNInward();
                fb_DataModelpr objRep = new fb_DataModelpr();
                if (cbfattchemnet == "")
                {
                    objMAttachment.attach_id = cbfattchemnet;
                    objMAttachment = objRep.Attachmentname(objMAttachment);
                }
                else
                {
                    objMAttachment.attach_id = cbfattchemnet;
                    objMAttachment = objRep.Attachmentname(objMAttachment);
                }
                objMAttachment.attachdate = DateTime.Now.ToShortDateString();
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BoqAttachGrid()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                GRNInward objAttachment = new GRNInward();

                if (Session["cbfdetails"].ToString() == "")
                {
                    objAttachment = (GRNInward)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                else
                {
                    objAttachment = (GRNInward)Session["attachmentdetails"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult BoqAttachGrid(GRNInward objAttachment)
        {
            try
            {
                fb_DataModelpr objRep = new fb_DataModelpr();
                ViewBag.NoRecordsFound = "";
                GRNInward objAttachment1 = new GRNInward();
                if (ModelState.IsValid)
                {
                    objAttachment.attach_id = Session["cbfdetails"].ToString();
                    if (objAttachment.attach_id == "IEM.Areas.FLEXIBUY.Models.GRNInward")
                    {
                        objAttachment.attach_id = "";
                    }
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
                        Session["attachmentdetails"] = objAttachment1;
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
                throw ex;
            }
        }



        public JsonResult DeleteAttachment(GRNInward obj)
        {
            fb_DataModelpr objRep = new fb_DataModelpr();
            ViewBag.NoRecordsFound = "";
            DataTable AttachDelete;
            GRNInward objAttachment1 = new GRNInward();
            string s = Session["cbfdetails"].ToString();
            if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.GRNInward")
            {
                AttachDelete = (DataTable)Session["AccessToken"];
            }
            else
            {
                AttachDelete = (DataTable)Session["AccessTokenheader"];

            }
            if (AttachDelete.Rows.Count > 0)
            {
                for (int i = 0; i < AttachDelete.Rows.Count; i++)
                {
                    if (AttachDelete.Rows[i]["Sno"].ToString() == obj.attachment1)
                    {
                        AttachDelete.Rows.RemoveAt(i);
                        objAttachment1.attach_id = Session["cbfdetails"].ToString();
                    }
                }
            }


            objAttachment1 = objRep.Attachmentname(objAttachment1);
            objAttachment1.attach_id = Session["cbfdetails"].ToString();

            if (Session["cbfdetails"].ToString() == "" || Session["cbfdetails"].ToString() == "IEM.Areas.FLEXIBUY.Models.GRNInward")
            {
                Session["attachment"] = objAttachment1;
                if (objAttachment1.attachment.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            else
            {
                Session["attachmentdetails"] = objAttachment1;
                if (objAttachment1.attachmentCbfdetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            return Json(objAttachment1, JsonRequestBehavior.AllowGet);
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
        public virtual ActionResult UploadedFiles(string attach = null, string attaching_format = null)
        {
            CmnFunctions objCmnFunctions = new CmnFunctions();
            bool isUploaded = false;
            string message = "File upload failed";
            string filename = "";
            try
            {
                Session["filename"] = null;
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            var stream = fileContent.InputStream;
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                            }
                            bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                            if (isEXE == false)
                            {
                                HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase; 
                                isUploaded = true;
                                Session["filename"] = hpfBase;

                                string pathForSaving = HttpContext.Application["path"] as string; 

                                filename = Path.GetFileNameWithoutExtension(fileContent.FileName);
                                filename = objCmnFunctions.GetSequenceNoFb("GRN", "", "");  
                                var path = Path.Combine(HoldFileUploadUrlDSA(), filename + fileextension);
                                using (var fileStream = System.IO.File.Create(path))
                                {
                                    stream.CopyTo(fileStream);
                                }                                
                                string Filestring = Convert.ToBase64String(bytFile);
                                FileServer Objserver = new FileServer();
                                //objErrorLog.WriteErrorLog(Filestring, filename + fileextension);
                                string result = Objserver.SaveFileToServer(Filestring, filename + fileextension).Result;
                                //objErrorLog.WriteErrorLog(result, path);
                                if (result == "success")
                                {
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }
                                filename = filename + fileextension;
                                Session["filename"] = filename;
                                isUploaded = true;
                                message = "File uploaded successfully!";
                                message = filename;
                            }
                            else
                            {
                                message = "Invalid File Format!";
                                isUploaded = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                message = string.Format("File upload failed: {0}", ex.Message);

            }
            //return Json(new { isUploaded = isUploaded, message = message }, "text/html");
            return Json(new { isUploaded, message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult UploadedFilesold()
        {
            CmnFunctions objCmnFunctions = new CmnFunctions();

            string filename = "";

            bool isUploaded = false;
            string message = "File upload failed";
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase myFile = Request.Files["attachname"];
                    if (myFile != null && myFile.ContentLength != 0)
                    {
                        string pathForSaving = HttpContext.Application["path"] as string; ;


                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        filename = objCmnFunctions.GetSequenceNoFb("GRN", "", "");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var path = Path.Combine(HoldFileUploadUrlDSA(), filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        byte[] bytFile = System.IO.File.ReadAllBytes(path);
                        string Filestring = Convert.ToBase64String(bytFile);
                        FileServer Objserver = new FileServer();
                        //objErrorLog.WriteErrorLog(Filestring, filename + fileextension);
                        string result = Objserver.SaveFileToServer(Filestring, filename + fileextension).Result;
                        //objErrorLog.WriteErrorLog(result, path);
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        filename = filename + fileextension;
                        Session["filename"] = filename;
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        message = filename;
                        //objErrorLog.WriteErrorLog(message, isUploaded.ToString());


                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                message = string.Format("File upload failed: {0}", ex.Message);

            }
            //return Json(new { isUploaded = isUploaded, message = message }, "text/html");
            return Json(new { isUploaded, message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCheck(string prodservicecode, string Qty)
        {
            DataTable dt = new DataTable();
            GRNInward objInward = new GRNInward();
            DataTable dts = new DataTable();

            try
            {
                string type = objModel.Getgrntype(prodservicecode);
                decimal cnt = Convert.ToDecimal(Qty);
                var values = cnt.ToString(CultureInfo.InvariantCulture).Split('.');
                int firstValue = int.Parse(values[0]);
                int secondValue = int.Parse(values[1]);
                string typp = "";
                if (type != null)
                {
                    if (type == "int" & secondValue == 0)
                    {
                        typp = "success";
                    }
                    else if (type == "Decimal")
                    {
                        typp = "success";
                    }
                    else
                    {
                        if (Session["grnTable"] != null)
                            dt = (DataTable)Session["grnTable"];
                        if (!dt.Columns.Contains("received_date"))
                        {
                            dt.Columns.Add("received_date");
                        }
                        objInward.objInwardList = objModel.GetPoDetailsTemp(dt);
                        typp = "Not Valid";
                    }
                }
                else
                {
                    if (Session["grnTable"] != null)
                        dt = (DataTable)Session["grnTable"];
                    if (!dt.Columns.Contains("received_date"))
                    {
                        dt.Columns.Add("received_date");
                    }
                    objInward.objInwardList = objModel.GetPoDetailsTemp(dt);
                    typp = "Not Valid";
                }

                return Json(typp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

