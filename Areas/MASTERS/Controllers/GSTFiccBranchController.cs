using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using System.Data;
using Newtonsoft.Json;
using IEM.Helper;
 using IEM.App_Start;
 

namespace IEM.Areas.MASTERS.Controllers
{

     
   [NoDirectAccess]
    public class GSTFiccBranchController : Controller
    {
       
        private GSTFIccBranch objModel;
        

        string Result, ActiveStatus, Branch;
       
        CmnFunctions com = new CmnFunctions();
      List<string> list = new List<string>();
      ErrorLog objErrorLog = new ErrorLog();
      dbLib db = new dbLib();

      public GSTFiccBranchController() : this(new GSTFiccBranchModel()) { }

      public GSTFiccBranchController(GSTFIccBranch obj)
        {
            objModel = obj;
        }
       
         [HttpGet]
        public ActionResult BranchIndex()
        {

            try
            {
                List<EntityGSTFiccBranch> Rolelist = new List<EntityGSTFiccBranch>();
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

                List<EntityGSTFiccBranch> RolelistF = new List<EntityGSTFiccBranch>();
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

                List<EntityGSTFiccBranch> Rolelist1 = new List<EntityGSTFiccBranch>();
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

                List<EntityGSTFiccBranch> branch = new List<EntityGSTFiccBranch>();
                if (Session["Barnch"] != null)
                {
                    EntityGSTFiccBranch Viewdata = new EntityGSTFiccBranch();
                    Viewdata = (EntityGSTFiccBranch)Session["viewbagsdata"];
                    ViewBag.BranchCode = Viewdata.BranchCode;
                    ViewBag.BranchName = Viewdata.BranchName;

                    branch = (List<EntityGSTFiccBranch>)Session["Barnch"];

                    List<EntityGSTFiccBranch> Rolelist4 = new List<EntityGSTFiccBranch>();
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

                    List<EntityGSTFiccBranch> Rolelist5 = new List<EntityGSTFiccBranch>();
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
         public ActionResult BranchIndex(string BranchCode = null, string BranchName = null, string BranchType = null, string City = null, string Branch = null, string ActiveStatus = null)
         {
             try
             {
                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();
                 EntityGSTFiccBranch viewbags = new EntityGSTFiccBranch();
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
                 List<EntityGSTFiccBranch> Rolelist = new List<EntityGSTFiccBranch>();
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

                 List<EntityGSTFiccBranch> Rolelist1 = new List<EntityGSTFiccBranch>();
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
                 List<EntityGSTFiccBranch> RolelistF = new List<EntityGSTFiccBranch>();
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



                 List<EntityGSTFiccBranch> Rolelist = new List<EntityGSTFiccBranch>();
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



                 List<EntityGSTFiccBranch> Rolelist1 = new List<EntityGSTFiccBranch>();
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
         public JsonResult AddBranch(EntityGSTFiccBranch branch)
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

                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();

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
                     ViewBag.BranchAddress = obj[0].BranchAddress;
                     ViewBag.OldBranchCode = obj[0].OldBranchCode;
                     ViewBag.Branch = obj[0].Branchfl;
                     ViewBag.PinCode = obj[0].PinCode;
                     ViewBag.StartLeaseDate = obj[0].StartLeaseDate;
                     ViewBag.EndLeaseDate = obj[0].EndLeaseDate;
                     ViewBag.ActiveStatus = obj[0].ActiveStatus;
                     ViewBag.City = obj[0].City;
                     ViewBag.branch_gstin = obj[0].branch_gstin;
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

                 List<EntityGSTFiccBranch> RolelistF = new List<EntityGSTFiccBranch>();
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
                 //List<EntityGSTFiccBranch> Rolelist = new List<EntityGSTFiccBranch>();
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

                 //List<EntityGSTFiccBranch> Rolelist1 = new List<EntityGSTFiccBranch>();
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

                 EntityGSTFiccBranch TypeModel = new EntityGSTFiccBranch();
                 TypeModel.GetPincode = new SelectList(objModel.getPincode(), "pincode_gid", "pincode_code", obj[0].pincode_gid);
                 TypeModel.Getstate = new SelectList(objModel.getstate(), "state_gid", "statecode", obj[0].state_gid);
                 TypeModel.GetDistrict = new SelectList(objModel.getdistrict(), "district_gid", "dictrictcode", obj[0].district_gid);
                 return PartialView(TypeModel);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         [HttpPost]
         public JsonResult EditBranch(EntityGSTFiccBranch branch)
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
         public PartialViewResult ViewBranch(int id)
         {
             try
             {
                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();
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
                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();
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
         public JsonResult DeleteBranch(EntityGSTFiccBranch branch)
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
                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();
                 EntityGSTFiccBranch emp = new EntityGSTFiccBranch();
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
                         obj = (List<EntityGSTFiccBranch>)Session["SearchEmployeedata"];
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
                 List<EntityGSTFiccBranch> obj = new List<EntityGSTFiccBranch>();
                 EntityGSTFiccBranch emp = new EntityGSTFiccBranch();
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
             List<EntityGSTFiccBranch> BRList = new List<EntityGSTFiccBranch>();
             BRList = (List<EntityGSTFiccBranch>)Session["Branchexport"]; 

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

         [HttpPost]
         public JsonResult GetList_Pincode(EntityGSTFiccBranch stateid)
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

          [HttpPost]
         public JsonResult Getstatebypincode(EntityGSTFiccBranch Pincode_model)
        {
            try
            {

                string Data1 = "";
                string pincodeid = Convert.ToString(Pincode_model.pincode_gid);
                DataSet ds = db.Getstatebypincode(pincodeid);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                //dt = ds.Tables[1];
                //if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1}, JsonRequestBehavior.AllowGet);
                //return Json(objModel.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

          [HttpPost]
          public JsonResult Getdistrictbystate(EntityGSTFiccBranch Pincode_model)
          {
              try
              {

                  string Data1 = "", Data2="";
                  string pincodeid = Convert.ToString(Pincode_model.pincode_gid);
                  string stategid = Convert.ToString(Pincode_model.state_gid);
                  DataSet ds = db.Getdistrictbystate(pincodeid,stategid);
                  DataTable dt = new DataTable();
                  dt = ds.Tables[0];
                  if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                  dt = ds.Tables[1];
                  if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                  return Json(new { Data1,Data2 }, JsonRequestBehavior.AllowGet);
                  //return Json(objModel.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
              }
              catch (Exception ex)
              {
                  objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                  return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
              }
          }


    }
}
