
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IEM.Areas.MASTERS.Controllers
{
    public class OwnerShipTransferController : Controller
    {
        #region
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions cmnfnc = new CmnFunctions();
        private TransferRepository objModel;
        #endregion
        //
        // GET: /MASTERS/OwnerShipTransfer/
        public OwnerShipTransferController()
            : this(new TransferModel())
        {


        }
        public OwnerShipTransferController(TransferRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetOwnerShip()
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {

                DataSet ds = new DataSet();
                DataTable dt;

                List<TransferDataModel> Rolelist = new List<TransferDataModel>();
                {
                    Rolelist = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString()
                    });
                }
                //ViewBag.Employee = role;
                //ds = objModel.GetProductGroup(flag);

                if (role.Count > 0) { ErrorMsg = "0"; data0 = JsonConvert.SerializeObject(role); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

            //return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesAutoComplete(string txt)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {

                DataSet ds = new DataSet();
                DataTable dt;
                List<string> result = new List<string>();

                dt = objModel.SelectEmployeeDetails(txt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

            //return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmployeeQueue(int transfertype = 0, int emptoid = 0, int reftype = 0, int subdoctype = 0, int ecfstatus=0)
        {
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            int empfromid = cmnfnc.GetLoginUserGid();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = null;

                dt = objModel.SelectEmployeeQueue(empfromid, emptoid, transfertype, reftype, subdoctype, ecfstatus);

                if (dt.Rows.Count > 0) { ErrorMsg = "0"; data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

            //return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateOwnerShip(int transfertype = 0, int emptoid = 0, int reftype = 0, string Data = null)
        {
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {  //Gidlist gid=new Gidlist();
                var gid = JsonConvert.DeserializeObject<Gidlist>(Data);

                var arr = gid.GidData.ToString().Split(',');
                int empfromid = cmnfnc.GetLoginUserGid();
                if (arr.Length > 1)
                {
                    for (var i = 0; i < arr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arr[i]))
                        {
                            if (transfertype == 1)
                                objModel.TransferOwnerShip(empfromid, emptoid, transfertype, reftype, Convert.ToInt32(arr[i]), 0);
                            else
                                objModel.TransferOwnerShip(empfromid, emptoid, transfertype, reftype, 0, Convert.ToInt32(arr[i]));
                        }
                    }
                }
                else
                {
                    ErrorMsg = "0";
                }


                return Json(new { data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult GetEcfSubDocType()
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {

                DataSet ds = new DataSet();
                DataTable dt;

                dt = objModel.GetEcfSubDocType();

                List<SelectListItem> role = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        role.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["docsubtype_name"]),
                            Value = Convert.ToString(dr["docsubtype_gid"])

                        });
                    }
                }
                //ViewBag.Employee = role;
                //ds = objModel.GetProductGroup(flag);

                if (role.Count > 0) { ErrorMsg = "0"; data0 = JsonConvert.SerializeObject(role); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

            //return Json("", JsonRequestBehavior.AllowGet);
        }




    }
}
