using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class MyProfileController : Controller
    {
        private MyprofileRepository objModel;
        public MyProfileController() :
            this(new MyProfileModel()) { }

        public MyProfileController(MyprofileRepository obj)
        {
            objModel = obj;
        }
        public PartialViewResult Index()
        {
            try
            {
                List<MyProfileDataModel> emp = new List<MyProfileDataModel>();
                emp = objModel.SelectEmployeeDetails().ToList();
                if (emp.Count > 0)
                {
                    ViewBag.employee_ou_name = emp[0].employee_ou_name;
                    ViewBag.employee_product_name = emp[0].employee_product_name;
                    ViewBag.employee_fc_name = emp[0].employee_fc_name;
                    ViewBag.employee_cc_name = emp[0].employee_cc_name;
                    ViewBag.EmployeeId = emp[0].employee_code;
                    ViewBag.EmployeeName = emp[0].employee_name;
                    ViewBag.DateOfBirth = emp[0].employee_dob;
                    ViewBag.DateOfJoin = emp[0].employee_doj;
                    ViewBag.Address = emp[0].employee_addr1;
                    ViewBag.Location = emp[0].employee_city_name;
                    ViewBag.PersonalEmailId = emp[0].employee_personal_email;
                    ViewBag.OfficeEmailId = emp[0].employee_office_email;
                    ViewBag.CardNo = emp[0].employee_card_no;
                    ViewBag.Designation = emp[0].hris_designation;
                    ViewBag.Grade = emp[0].employee_grade_code;
                    ViewBag.Department = emp[0].employee_dept_name;
                    ViewBag.FCCC = emp[0].employee_fccc_code;
                    ViewBag.Supervisor = emp[0].employee_supervisor;
                    ViewBag.ERAACNo = emp[0].employee_era_acc_no;
                    ViewBag.ifsc_code = emp[0].employee_era_ifsc_code;
                    ViewBag.BankName = emp[0].employee_era_bank_code;
                    ViewBag.Gender = emp[0].employee_gender;
                    ViewBag.Status = emp[0].employee_status;
                    ViewBag.hrisdesignation = emp[0].hris_designation;
                    ViewBag.branch = emp[0].branch_flag;
                    ViewBag.iemdesignation = emp[0].employee_iem_designation;
                    ViewBag.employeefunctionname = emp[0].employee_function_name;
                }
                MyProfileDataModel typemodel = new MyProfileDataModel();
                typemodel = objModel.menu(0);
                return PartialView(typemodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public PartialViewResult TreeView()
        {
            return PartialView();
        }

    }
}
