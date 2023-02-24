using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Data;
using IEM.App_Start;
 

namespace IEM.Areas.MASTERS.Controllers
{
    
    [NoDirectAccess]
    public class BranchController : Controller
    {
        private BranchRepository objModel;
        string Result,ActiveStatus,Branch;
        List<string> list = new List<string>();
        public BranchController() :
            this(new BranchModel()) { }

        public BranchController(BranchRepository obj)
        {
            objModel = obj;
        }
        [HttpGet]
        public ActionResult BranchIndex()
        {
            try
            {
                List<BranchDataModel> Rolelist = new List<BranchDataModel>();
                {
                    Rolelist = objModel.SelectLoction().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.Location.ToString(),
                        Value = item.Location_gid.ToString()
                    });
                }
                ViewBag.location = role;

                List<BranchDataModel> RolelistF = new List<BranchDataModel>();
                {
                    RolelistF = objModel.SlectBranchType().ToList();
                };
                List<SelectListItem> roleF = new List<SelectListItem>();
                foreach (var item in RolelistF)
                {
                    roleF.Add(new SelectListItem
                    {
                        Text = item.BranchType.ToString(),
                        Value = item.Branchtype_gid.ToString()
                    });
                }
                ViewBag.BranchType = roleF;

                List<BranchDataModel> Rolelist1 = new List<BranchDataModel>();
                {
                    Rolelist1 = objModel.SelectCity().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.City.ToString(),
                        Value = item.City_gid.ToString()
                    });
                }
                ViewBag.City = role1;

                list.Add("Yes");
                list.Add("No");
                List<string> Rolelist2 = new List<string>();
                {
                    Rolelist2 = list;
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    role2.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.Branch = role2;


                List<string> Rolelist3 = new List<string>();
                {
                    Rolelist3 = list;
                };
                List<SelectListItem> role3 = new List<SelectListItem>();
                foreach (var item in Rolelist3)
                {
                    role3.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.ActiveStatus = role3;

                List<BranchDataModel> branch = new List<BranchDataModel>();
                if (Session["Barnch"] != null)
                {
                    BranchDataModel Viewdata = new BranchDataModel();
                    Viewdata = (BranchDataModel)Session["viewbagsdata"];
                    ViewBag.BranchCode = Viewdata.BranchCode;
                    ViewBag.BranchName = Viewdata.BranchName;

                    branch = (List<BranchDataModel>)Session["Barnch"];

                    List<BranchDataModel> Rolelist4 = new List<BranchDataModel>();
                    {
                        Rolelist4 = objModel.SlectBranchType().ToList();
                    };
                    List<SelectListItem> role4 = new List<SelectListItem>();
                    foreach (var item in Rolelist4)
                    {
                        bool selected = false;
                        if (item.Branchtype_gid.ToString() == Viewdata.BranchType)
                        {
                            selected = true;
                        }
                        role4.Add(new SelectListItem
                        {
                            Text = item.BranchType.ToString(),
                            Value = item.Branchtype_gid.ToString(),
                            Selected = selected
                        });
                    }
                    ViewBag.BranchType = role4;

                    List<BranchDataModel> Rolelist5 = new List<BranchDataModel>();
                    {
                        Rolelist5 = objModel.SelectCity().ToList();
                    };
                    List<SelectListItem> role5 = new List<SelectListItem>();
                    foreach (var item in Rolelist5)
                    {
                        bool selected = false;
                        if (item.City_gid.ToString() == Viewdata.City)
                        {
                            selected = true;
                        }
                        role5.Add(new SelectListItem
                        {
                            Text = item.City.ToString(),
                            Value = item.City_gid.ToString(),
                            Selected = selected
                        });
                    }
                    ViewBag.City = role5;

                    list.Add("Yes");
                    list.Add("No");
                    List<string> Rolelist6 = new List<string>();
                    {
                        Rolelist6 = list;
                    };
                    List<SelectListItem> role6 = new List<SelectListItem>();
                    foreach (var item in Rolelist6)
                    {
                        bool selected = false;
                        if (item == Viewdata.Branchfl)
                        {
                            selected = true;
                        }
                        role6.Add(new SelectListItem
                        {
                            Text = item.ToString(),
                            Selected = selected
                            //Value = item.ToString()
                        });
                    }
                    ViewBag.Branch = role6;


                    List<string> Rolelist7 = new List<string>();
                    {
                        Rolelist7 = list;
                    };
                    List<SelectListItem> role7 = new List<SelectListItem>();
                    foreach (var item in Rolelist7)
                    {
                        bool selected = false;
                        if (item == Viewdata.ActiveStatus)
                        {
                            selected = true;
                        }
                        role7.Add(new SelectListItem
                        {
                            Text = item.ToString(),
                            Selected = selected
                            //Value = item.ToString()
                        });
                    }
                    ViewBag.ActiveStatus = role7;
                }
                else
                {
                    branch = objModel.SelectBranch().ToList();
                }
                if (branch.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.Message = "No Records";
                }
                Session["Branchexport"] = branch;
                return View(branch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult BranchIndex(string BranchCode = null, string BranchName = null, string BranchType = null, string City = null, string Branch = null, string ActiveStatus=null)
        {
            try
            {
                List<BranchDataModel> obj = new List<BranchDataModel>();
                BranchDataModel viewbags = new BranchDataModel();
                obj = objModel.Search(BranchCode, BranchName, BranchType, City, Branch, ActiveStatus).ToList();
                Session["Barnch"] = obj;
                ViewBag.BranchCode = BranchCode;
                ViewBag.BranchName = BranchName;
                if (obj.Count == 0)
                {
                    ViewBag.alert = "No Records";
                }

                viewbags.BranchCode = BranchCode;
                viewbags.BranchName = BranchName;
                viewbags.BranchType = BranchType;
                viewbags.City = City;
                viewbags.Branchfl = Branch;
                viewbags.ActiveStatus = ActiveStatus;
                Session["viewbagsdata"] = viewbags;

                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                //obj = objModel.SelectCity().ToList();
                List<BranchDataModel> Rolelist = new List<BranchDataModel>();
                {
                    Rolelist = objModel.SlectBranchType().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item.Branchtype_gid.ToString() == BranchType)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.BranchType.ToString(),
                        Value = item.Branchtype_gid.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.BranchType = role;

                List<BranchDataModel> Rolelist1 = new List<BranchDataModel>();
                {
                    Rolelist1 = objModel.SelectCity().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.City_gid.ToString() == City)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.City.ToString(),
                        Value = item.City_gid.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.City = role1;

                list.Add("Yes");
                list.Add("No");
                List<string> Rolelist2 = new List<string>();
                {
                    Rolelist2 = list;
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    bool selected = false;
                    if (item == Branch)
                    {
                        selected = true;
                    }
                    role2.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected
                        //Value = item.ToString()
                    });
                }
                ViewBag.Branch = role2;


                List<string> Rolelist3 = new List<string>();
                {
                    Rolelist3 = list;
                };
                List<SelectListItem> role3 = new List<SelectListItem>();
                foreach (var item in Rolelist3)
                {
                    bool selected = false;
                    if (item == ActiveStatus)
                    {
                        selected = true;
                    }
                    role3.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected
                        //Value = item.ToString()
                    });
                }
                ViewBag.ActiveStatus = role3;
                Session["Branchexport"] = obj;
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Clear()
        {
            Session["Barnch"] = null;
            Session["viewbagsdata"] = null;
            return RedirectToAction("BranchIndex", "Branch");
        }
        [HttpGet]
        public PartialViewResult AddBranchView()
        {
            try
            {
                List<BranchDataModel> RolelistF = new List<BranchDataModel>();
                {
                    RolelistF = objModel.SelectLoction().ToList();
                };
                List<SelectListItem> roleF = new List<SelectListItem>();
                foreach (var item in RolelistF)
                {

                    roleF.Add(new SelectListItem
                    {
                        Text = item.Location.ToString(),
                        Value = item.Location_gid.ToString()
                    });
                }
                ViewBag.location = roleF;



                List<BranchDataModel> Rolelist = new List<BranchDataModel>();
                {
                    Rolelist = objModel.SlectBranchType().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.BranchType.ToString(),
                        Value = item.Branchtype_gid.ToString()
                    });
                }
                ViewBag.BranchType = role;



                List<BranchDataModel> Rolelist1 = new List<BranchDataModel>();
                {
                    Rolelist1 = objModel.SelectCity().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.City.ToString(),
                        Value = item.City_gid.ToString()
                    });
                }
                ViewBag.City = role1;

                list.Add("Yes");
                list.Add("No");
                List<string> Rolelist2 = new List<string>();
                {
                    Rolelist2 = list;
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    role2.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.Branch = role2;

                List<string> Rolelist3 = new List<string>();
                {
                    Rolelist3 = list;
                };
                List<SelectListItem> role3 = new List<SelectListItem>();
                foreach (var item in Rolelist3)
                {
                    role3.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.ActiveStatus = role3;

                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public JsonResult AddBranch(BranchDataModel branch)
        {
            try
            {
                Result = objModel.InsertBranch(branch);
                if (Result == "sub")
                {
                    Result = "Successfully Saved";
                }
                if (Result == "DuplicateBranchCode")
                {
                    Result = "You Can't Add Duplicate Branch Code";
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EditBranchView(int id)
        {
            try
            {
                Session["branch_gid"] = id;

                List<BranchDataModel> obj = new List<BranchDataModel>();

                obj = objModel.SelectBranchEdit(id).ToList();
                if (obj.Count != 0 && obj != null)
                {
                    ViewBag.BranchCode = obj[0].BranchCode;
                    ViewBag.BranchName = obj[0].BranchName;
                    ViewBag.BranchTier = obj[0].BranchTier;
                    ViewBag.BranchIncharge = obj[0].BranchIncharge;
                    ViewBag.BranchType = obj[0].BranchType;
                    ViewBag.EmployeeId = obj[0].EmployeeId;
                    ViewBag.StartDate = obj[0].StartDate;
                    ViewBag.EndDate = obj[0].ClosedDate;
                    ViewBag.Location = obj[0].Location;
                    ViewBag.Location_gid = obj[0].Location_gid;
                    ViewBag.BranchAddress = obj[0].BranchAddress;
                    ViewBag.OldBranchCode = obj[0].OldBranchCode;
                    ViewBag.Branch = obj[0].Branchfl;
                    ViewBag.PinCode = obj[0].PinCode;
                    ViewBag.StartLeaseDate = obj[0].StartLeaseDate;
                    ViewBag.EndLeaseDate = obj[0].EndLeaseDate;
                    ViewBag.ActiveStatus = obj[0].ActiveStatus;
                    ViewBag.City = obj[0].City;
                    ViewBag.City_gid = obj[0].City_gid;
                    ViewBag.pincode = obj[0].PinCode;
                    ViewBag.gstin = obj[0].gstin;
                    ViewBag.District = obj[0].District;
                    ViewBag.District_gid = obj[0].District_gid;
                    ViewBag.state = obj[0].state;
                    ViewBag.state_gid = obj[0].state_gid;
                }
                //if (obj[0].Branchfl == "N")
                //{
                //    Branch = "NO";
                //}
                //else
                //{
                //    Branch = "Yes";
                //}
                //if (obj[0].ActiveStatus == "Y")
                //{
                //    ActiveStatus = "Yes";
                //}
                //else
                //{
                //    ActiveStatus = "NO";
                //}

                List<BranchDataModel> RolelistF = new List<BranchDataModel>();
                {
                    RolelistF = objModel.SelectLoction().ToList();
                };
                List<SelectListItem> roleF = new List<SelectListItem>();
                foreach (var item in RolelistF)
                {
                    bool selected = false;
                    if (item.Location == obj[0].Location)
                    {
                        selected = true;
                    }
                    roleF.Add(new SelectListItem
                    {
                        Text = item.Location.ToString(),
                        Value = item.Location_gid.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.location = roleF;


                //obj = objModel.SelectCity().ToList();
                //List<BranchDataModel> Rolelist = new List<BranchDataModel>();
                //{
                //    Rolelist = objModel.SlectBranchType().ToList();
                //};
                //List<SelectListItem> role = new List<SelectListItem>();
                //foreach (var item in Rolelist)
                //{
                //    bool selected = false;
                //    if (item.Branchtype_gid == obj[0].Branchtype_gid)
                //    {
                //        selected = true;
                //    }
                //    role.Add(new SelectListItem
                //    {
                //        Text = item.BranchType.ToString(),
                //        Value = item.Branchtype_gid.ToString(),
                //        Selected = selected
                //    });
                //}
                //ViewBag.BranchType = role;

                //List<BranchDataModel> Rolelist1 = new List<BranchDataModel>();
                //{
                //    Rolelist1 = objModel.SelectCity().ToList();
                //};
                //List<SelectListItem> role1 = new List<SelectListItem>();
                //foreach (var item in Rolelist1)
                //{
                //    bool selected = false;
                //    if (item.City == obj[0].City)
                //    {
                //        selected = true;
                //    }
                //    role1.Add(new SelectListItem
                //    {
                //        Text = item.City.ToString(),
                //        Value = item.City_gid.ToString(),
                //        Selected = selected
                //    });
                //}
                //ViewBag.City = role1;

                //list.Add("Yes");
                //list.Add("No");
                //List<string> Rolelist2 = new List<string>();
                //{
                //    Rolelist2 = list;
                //};
                //List<SelectListItem> role2 = new List<SelectListItem>();
                //foreach (var item in Rolelist2)
                //{
                //    bool selected = false;
                //    if (item == Branch)
                //    {
                //        selected = true;
                //    }
                //    role2.Add(new SelectListItem
                //    {
                //        Text = item.ToString(),
                //        Selected = selected
                //       
                //    });
                //}
                //ViewBag.Branch = role2;


                //List<string> Rolelist3 = new List<string>();
                //{
                //    Rolelist3 = list;
                //};
                //List<SelectListItem> role3 = new List<SelectListItem>();
                //foreach (var item in Rolelist3)
                //{
                //    bool selected = false;
                //    if (item == ActiveStatus)
                //    {
                //        selected = true;
                //    }
                //    role3.Add(new SelectListItem
                //    {
                //        Text = item.ToString(),
                //        Selected = selected
                //        //Value = item.ToString()
                //    });
                //}
                //ViewBag.ActiveStatus = role3;

                BranchDataModel TypeModel = new BranchDataModel();
                TypeModel.GetPincode = new SelectList(objModel.getPincode(), "pincode_list", "pincode_gid", obj[0].PinCode);
                return PartialView(TypeModel);
              //  return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EditBranch(BranchDataModel branch)
        {
            try
            {
                if (Session["branch_gid"] != null)
                {
                    branch.Branch_gid = Convert.ToInt16(Session["branch_gid"]);
                }
                Result = objModel.UpdateBranch(branch);
                if (Result == "suc")
                {
                    Result = "Successfully Update";
                }
                if (Result == "DuplicateBranchCode")
                {
                    Result = "You Can't Add Duplicate Branch Code";
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ViewBranch(int id)
        {
            try
            {
                List<BranchDataModel> obj = new List<BranchDataModel>();
                obj = objModel.SelectBranchEdit(id).ToList();
                if (obj.Count != 0 && obj != null)
                {
                    //if (obj[0].Branchfl == "N")
                    //{
                    //    ViewBag.Branch = "NO";
                    //}
                    //else
                    //{
                    //    ViewBag.Branch = "Yes";
                    //}
                    //if (obj[0].ActiveStatus == "Y")
                    //{
                    //    ViewBag.ActiveStatus = "Yes";
                    //}
                    //else
                    //{
                    //    ViewBag.ActiveStatus = "NO";
                    //}
                    ViewBag.BranchCode = obj[0].BranchCode;
                    ViewBag.BranchName = obj[0].BranchName;
                    ViewBag.BranchTier = obj[0].BranchTier;
                    ViewBag.BranchIncharge = obj[0].BranchIncharge;
                    ViewBag.BranchType = obj[0].BranchType;
                    ViewBag.EmployeeId = obj[0].EmployeeId;
                    ViewBag.StartDate = obj[0].StartDate;
                    ViewBag.EndDate = obj[0].ClosedDate;
                    ViewBag.Location = obj[0].Location;
                    ViewBag.BranchAddress = obj[0].BranchAddress;
                    ViewBag.OldBranchCode = obj[0].OldBranchCode;
                    ViewBag.Branch = obj[0].Branchfl;
                    ViewBag.PinCode = obj[0].PinCode;
                    ViewBag.StartLeaseDate = obj[0].StartLeaseDate;
                    ViewBag.EndLeaseDate = obj[0].EndLeaseDate;
                    ViewBag.ActiveStatus = obj[0].ActiveStatus;
                    ViewBag.pincode = obj[0].PinCode;
                    ViewBag.gstin = obj[0].gstin;
                    ViewBag.District = obj[0].District;
                    ViewBag.District_gid = obj[0].District_gid;
                    ViewBag.state = obj[0].state;
                    ViewBag.state_gid = obj[0].state_gid;
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult DeleteBranch(int id)
        {
            try
            {
                Session["Dbranch_gid"] = id;
                List<BranchDataModel> obj = new List<BranchDataModel>();
                obj = objModel.SelectBranchEdit(id).ToList();
                if (obj.Count != 0 && obj != null)
                {
                    if (obj[0].Branchfl == "N")
                    {
                        ViewBag.Branch = "NO";
                    }
                    else
                    {
                        ViewBag.Branch = "Yes";
                    }
                    if (obj[0].ActiveStatus == "Y")
                    {
                        ViewBag.ActiveStatus = "Yes";
                    }
                    else
                    {
                        ViewBag.ActiveStatus = "NO";
                    }
                    ViewBag.BranchCode = obj[0].BranchCode;
                    ViewBag.BranchName = obj[0].BranchName;
                    ViewBag.BranchTier = obj[0].BranchTier;
                    ViewBag.BranchIncharge = obj[0].BranchIncharge;
                    ViewBag.EmployeeId = obj[0].EmployeeId;
                    ViewBag.StartDate = obj[0].StartDate;
                    ViewBag.EndDate = obj[0].ClosedDate;
                    ViewBag.Location = obj[0].Location;
                    ViewBag.BranchAddress = obj[0].BranchAddress;
                    ViewBag.City = obj[0].City;
                    ViewBag.BranchType = obj[0].BranchType;
                    ViewBag.OldBranchCode = obj[0].OldBranchCode;
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeleteBranch(BranchDataModel branch)
        {
            try
            {
                if (Session["Dbranch_gid"] != null)
                {
                    branch.Branch_gid = Convert.ToInt16(Session["Dbranch_gid"]);
                }
                Result = objModel.DeleteBranch(branch.Branch_gid);
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EmployeeSearch(string listfor)
        {
            try
            {
                List<BranchDataModel> obj = new List<BranchDataModel>();
                BranchDataModel emp = new BranchDataModel();
                if (Session["RaiserName"] != null)
                {
                    ViewBag.EmployeeName = Session["RaiserName"].ToString();
                }
                if (Session["RaiserCode"] != null)
                {
                    ViewBag.EmployeeCode = Session["RaiserCode"].ToString();
                }
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        obj = (List<BranchDataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    obj = objModel.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult EmployeeSearchforBranch(string RaiserName = "", string RaiserCode = "")
        {
            try
            {
                List<BranchDataModel> obj = new List<BranchDataModel>();
                BranchDataModel emp = new BranchDataModel();
                if (RaiserCode == "" && RaiserName == "")
                {
                    obj = objModel.SelectEmployee().ToList();
                }
                else
                {

                }
                obj = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                if (obj != null)
                {

                    ViewBag.EmployeeCode = RaiserCode;
                    ViewBag.EmployeeName = RaiserName;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record's Found !";
                    }

                }
                return PartialView("EmployeeSearch", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult excelexport()
        {
            List<BranchDataModel> BRList = new List<BranchDataModel>();
            BRList = (List<BranchDataModel>)Session["Branchexport"]; 



            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Branch Code");
            dt.Columns.Add("Branch Name");
            dt.Columns.Add("City");
            dt.Columns.Add("BranchTier");
            dt.Columns.Add("BranchIncharge");
            dt.Columns.Add("Start Date");
            dt.Columns.Add("Close Date");
            dt.Columns.Add("Active Status");
           // dt.Columns.Add("Branch");


            for (int i = 0; i < BRList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    BRList[i].BranchCode.ToString(),
                    BRList[i].BranchName.ToString(),
                    BRList[i].City.ToString(),
                    BRList[i].BranchTier.ToString(),
                    BRList[i].BranchIncharge.ToString(),
                    BRList[i].StartDate.ToString(),
                    BRList[i].ClosedDate.ToString(),
                    BRList[i].ActiveStatus.ToString()
                   // BRList[i].Branch.ToString()


                                       );
            }


            string attachment = "attachment; filename=BranchReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            string strquery = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int k;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (k = 0; k < dt.Columns.Count; k++)
                    {
                       
                            Response.Write(tab + dr[k].ToString());
                            tab = "\t";
                       
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            return RedirectToAction("Index");



        }
        //--Pandiaraj 29/06/2017 GSTIN add
        [HttpPost]
        public JsonResult GetList_Pincode(BranchDataModel stateid)
        {
            try
            {

                string id = stateid.pincode_code;
                return Json(objModel.GetDistrictByStateId(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
