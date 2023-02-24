using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;
 

namespace IEM.Areas.MASTERS.Controllers
{

     
      [NoDirectAccess]
    public class PayBankController : Controller
    {
        private PayBankRepository objModel;
        string Result;
        List<string> list = new List<string>();
        public PayBankController()
            : this(new PayBankModel())
        {

        }
        public PayBankController(PayBankRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult PayBankIndex()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPayBankDetails()
        {
            List<PayBankDataModel> lstDashBoard = new List<PayBankDataModel>();
            lstDashBoard = objModel.SelectPayBankGrid().ToList();
            //return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public PartialViewResult NewPayBank()
        {
            List<PayBankDataModel> Rolelist = new List<PayBankDataModel>();
            {
                Rolelist = objModel.SelectBank().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {

                role.Add(new SelectListItem
                {
                    Text = item.BankName.ToString(),
                    Value = item.BankGid.ToString(),
                });
            }
            ViewBag.BankName = role;
            //glno
            List<PayBankDataModel> Rolelist2 = new List<PayBankDataModel>();
            {
                Rolelist2 = objModel.SelectEmployeeSearch("", "").ToList();
            };
            List<SelectListItem> role2 = new List<SelectListItem>();
            foreach (var item in Rolelist2)
            {

                role2.Add(new SelectListItem
                {
                    Text = item.PayBankGlNo.ToString(),
                    Value = item.Payglid.ToString(),
                });
            }
            ViewBag.BankGLNO = role2;
            //Active
            list.Add("Active");
            list.Add("InActive");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    //Value = item.ToString()
                });
            }
            ViewBag.BankStatus = role1;

            return PartialView();
        }
        [HttpPost]
        public JsonResult getAutocomplete(string RaiserName)
        {
            List<PayBankDataModel> obj = new List<PayBankDataModel>();

            obj = objModel.SelectEmployeeSearch(RaiserName, "").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePayBank(PayBankDataModel pay)
        {
            List<PayBankDataModel> obj = new List<PayBankDataModel>();
            Result = objModel.InsertPayBank(pay);
            if (Result == "suc")
            {
                Result = "Saved Successfully";
            }
            if (Result == "DuplicateBranchCode")
            {
                Result = "Duplicate Record";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditPayBank(PayBankDataModel pay)
        {
            List<PayBankDataModel> obj = new List<PayBankDataModel>();
            pay.PayBankGid = Convert.ToInt16(Session["Editid"].ToString());
            Result = objModel.UpdatePayBank(pay);
            if (Result == "sub")
            {
                Result = "Saved Successfully";
            }
            if (Result == "DuplicateBranchCode")
            {
                Result = "Duplicate Record";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult EditPayBank(int id)
        {
            Session["Editid"] = id;
            PayBankDataModel objdata = objModel.SelectEditPayBank(id);

            ViewBag.PayBankAccNo = objdata.PayBankAcNo.ToString();
            ViewBag.PayBankIfSCCode = objdata.PayBankIfscCode.ToString();
            ViewBag.PayBankBranchName = objdata.PayBankBranchName.ToString();
            ViewBag.RecivedDateFrom = objdata.PeriodFrom.ToString();
            ViewBag.RecivedDateTo = objdata.PeriodTo.ToString();


            if (objdata.PayBankStatus == "Y")
            {
                objdata.PayBankStatus = "Active";
            }
            if (objdata.PayBankStatus == "N")
            {
                objdata.PayBankStatus = "InActive";
            }
            //
            List<PayBankDataModel> Rolelist = new List<PayBankDataModel>();
            {
                Rolelist = objModel.SelectBank().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                Boolean selected = false;
                if (item.BankName.ToString() == objdata.BankName.ToString())
                {
                    selected = true;

                }
                role.Add(new SelectListItem
                {
                    Text = item.BankName.ToString(),
                    Value = item.BankGid.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankName = role;
            //glno
            List<PayBankDataModel> Rolelist2 = new List<PayBankDataModel>();
            {
                Rolelist2 = objModel.SelectEmployeeSearch("", "").ToList();
            };
            List<SelectListItem> role2 = new List<SelectListItem>();
            foreach (var item in Rolelist2)
            {
                Boolean selected = false;
                if (item.PayBankGlNo.ToString() == objdata.PayBankGlNo.ToString())
                {
                    selected = true;

                }
                role2.Add(new SelectListItem
                {
                    Text = item.PayBankGlNo.ToString(),
                    Value = item.Payglid.ToString(),
                    Selected=selected
                });
            }
            ViewBag.BankGLNO = role2;
            //Active
            list.Add("Active");
            list.Add("InActive");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                Boolean selected = false;
                if (item == objdata.PayBankStatus.ToString())
                {
                    selected = true;

                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankStatus = role1;

            return PartialView(); 
        }
        public PartialViewResult ViewPayBank(int id)
        {

            PayBankDataModel objdata = objModel.SelectEditPayBank(id);

            ViewBag.PayBankAccNo = objdata.PayBankAcNo.ToString();
            ViewBag.PayBankIfSCCode = objdata.PayBankIfscCode.ToString();
            ViewBag.PayBankBranchName = objdata.PayBankBranchName.ToString();
            ViewBag.RecivedDateFrom = objdata.PeriodFrom.ToString();
            ViewBag.RecivedDateTo = objdata.PeriodTo.ToString();


            if (objdata.PayBankStatus == "Y")
            {
                objdata.PayBankStatus = "Active";
            }
            if (objdata.PayBankStatus == "N")
            {
                objdata.PayBankStatus = "InActive";
            }
            //
            List<PayBankDataModel> Rolelist = new List<PayBankDataModel>();
            {
                Rolelist = objModel.SelectBank().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                Boolean selected = false;
                if (item.BankName.ToString() == objdata.BankName.ToString())
                {
                    selected = true;

                }
                role.Add(new SelectListItem
                {
                    Text = item.BankName.ToString(),
                    Value = item.BankGid.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankName = role;
            //glno
            List<PayBankDataModel> Rolelist2 = new List<PayBankDataModel>();
            {
                Rolelist2 = objModel.SelectEmployeeSearch("", "").ToList();
            };
            List<SelectListItem> role2 = new List<SelectListItem>();
            foreach (var item in Rolelist2)
            {
                Boolean selected = false;
                if (item.PayBankGlNo.ToString() == objdata.PayBankGlNo.ToString())
                {
                    selected = true;

                }
                role2.Add(new SelectListItem
                {
                    Text = item.PayBankGlNo.ToString(),
                    Value = item.Payglid.ToString(),
                    Selected=selected
                });
            }
            ViewBag.BankGLNO = role2;
            //Active
            list.Add("Active");
            list.Add("InActive");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                Boolean selected = false;
                if (item == objdata.PayBankStatus.ToString())
                {
                    selected = true;

                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankStatus = role1;

            return PartialView();
        }


        public PartialViewResult DeletePayBank(int id)
        {
            Session["delid"] = id;
            PayBankDataModel objdata = objModel.SelectEditPayBank(id);

            ViewBag.PayBankAccNo = objdata.PayBankAcNo.ToString();
            ViewBag.PayBankIfSCCode = objdata.PayBankIfscCode.ToString();
            ViewBag.PayBankBranchName = objdata.PayBankBranchName.ToString();
            ViewBag.RecivedDateFrom = objdata.PeriodFrom.ToString();
            ViewBag.RecivedDateTo = objdata.PeriodTo.ToString();


            if (objdata.PayBankStatus == "Y")
            {
                objdata.PayBankStatus = "Active";
            }
            if (objdata.PayBankStatus == "N")
            {
                objdata.PayBankStatus = "InActive";
            }
            //
            List<PayBankDataModel> Rolelist = new List<PayBankDataModel>();
            {
                Rolelist = objModel.SelectBank().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                Boolean selected = false;
                if (item.BankName.ToString() == objdata.BankName.ToString())
                {
                    selected = true;

                }
                role.Add(new SelectListItem
                {
                    Text = item.BankName.ToString(),
                    Value = item.BankGid.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankName = role;
            //glno
            List<PayBankDataModel> Rolelist2 = new List<PayBankDataModel>();
            {
                Rolelist2 = objModel.SelectEmployeeSearch("", "").ToList();
            };
            List<SelectListItem> role2 = new List<SelectListItem>();
            foreach (var item in Rolelist2)
            {
                Boolean selected = false;
                if (item.PayBankGlNo.ToString() == objdata.PayBankGlNo.ToString())
                {
                    selected = true;

                }
                role2.Add(new SelectListItem
                {
                    Text = item.PayBankGlNo.ToString(),
                    Value = item.Payglid.ToString(),
                    Selected=selected
                });
            }
            ViewBag.BankGLNO = role2;
            //Active
            list.Add("Active");
            list.Add("InActive");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                Boolean selected = false;
                if (item == objdata.PayBankStatus.ToString())
                {
                    selected = true;

                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                });
            }
            ViewBag.BankStatus = role1;

            return PartialView();
        }
        [HttpPost]
        public JsonResult DeletePayBank()
        {
            try
            {
                Result = objModel.DeletePayBank(Convert.ToInt16(Session["delid"].ToString()));
                if (Result == "Sucess")
                {
                    Result = "deleted Successfully";
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
