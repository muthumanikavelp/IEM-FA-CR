using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
 

namespace IEM.Areas.MASTERS.Controllers
{

     
 [NoDirectAccess]
    public class TransferController : Controller
    {
        DataTable dt = new DataTable();
        private TransferRepository objModel;
        String result = String.Empty;
        string modulename;
        public TransferController()
            : this(new TransferModel())
        {

        }
        public TransferController(TransferRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {

            try
            {
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
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString()
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString()
                    });
                }
                ViewBag.OwnerShip = role2;

                List<TransferDataModel> trn = new List<TransferDataModel>();
                trn = objModel.Select().ToList();
                if (trn.Count == 0)
                {
                    ViewBag.alert = "No Records";
                }
                Session["transferexport"] = trn;
                return View(trn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Index(string TransferDateFrom = null, string TransferDateTo = null, string Employee = null, string Module = null, string OwnerShip = null)
        {
            try
            {
                ViewBag.TransferDateFrom = TransferDateFrom;
                ViewBag.TransferDateTo = TransferDateTo;

                List<TransferDataModel> Rolelist = new List<TransferDataModel>();
                {
                    Rolelist = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == Employee.ToString())
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == Module.ToString())
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    Boolean selected = false;
                    if (item.ModuleId.ToString() == OwnerShip.ToString())
                    {
                        selected = true;
                    }
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.OwnerShip = role2;
                List<TransferDataModel> transfer = new List<TransferDataModel>();
                transfer = objModel.Search(TransferDateFrom, TransferDateTo, Employee, Module, OwnerShip).ToList();
                if (transfer.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.Message = "No Records Found";
                }
                Session["transferexport"] = transfer;
                return View(transfer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult New()
        {
            try
            {
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
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString()
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString()
                    });
                }
                ViewBag.OwnerShip = role2;
                TransferDataModel owner = new TransferDataModel();
                dt = objModel.SelectOwnership(modulename);
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["ownership_name"].ToString(), Value = row["ownership_gid"].ToString() });
                        owner.OwnerShip = names;
                    }
                }
                return PartialView(owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult AddOwner(TransferDataModel trn, string[] SelectedOwner)
        {
            if (SelectedOwner == null)
            {
                SelectedOwner = new string[] { "" };
            }
            result = objModel.InsertOwner(trn);
            result = objModel.InsertOwnerDetails(SelectedOwner);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult view(int id)
        {
            try
            {
                List<TransferDataModel> selectedid = new List<TransferDataModel>();

                selectedid = objModel.SelectedId(id).ToList();
                List<TransferDataModel> Rolelist = new List<TransferDataModel>();
                {
                    Rolelist = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TranferFrom.ToString())
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TransferTo.ToString())
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    Boolean selected = false;
                    if (item.ModuleId.ToString() == selectedid[0].ModuleId.ToString())
                    {
                        selected = true;
                    }
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.OwnerShip = role2;

                ViewBag.TransferDate = selectedid[0].TransferDate;
                ViewBag.Remarks = selectedid[0].Remarks;

                dt = objModel.Selectedownerid(id);
                TransferDataModel owner = new TransferDataModel();
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["ownership_name"].ToString(), Value = row["ownership_gid"].ToString() });
                        owner.OwnerShip = names;
                    }
                }
                return PartialView(owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult onchange(TransferDataModel transfer)
        {
            List<TransferDataModel> emp = new List<TransferDataModel>();
            TransferDataModel owner;
            dt = objModel.SelectOwnership(transfer.ModuleName);
            foreach (DataRow row in dt.Rows)
            {
                owner = new TransferDataModel();
                owner.OwnerShipId = Convert.ToInt32(row["ownership_gid"].ToString());
                owner.OwnerShipName = row["ownership_name"].ToString();
                emp.Add(owner);
            }
            return Json(emp, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Edit(int id,string viewfor)
        {
            try
            {
                if (viewfor == "Edit")
                {
                    ViewBag.viewfor = "Edit";
                }
                else if (viewfor == "View")
                {
                    ViewBag.viewfor = "View";
                }
                else if (viewfor == "Delete")
                {
                    ViewBag.viewfor = "Delete";
                }
                Session["Transferid"] = id;
                TransferDataModel owner = new TransferDataModel();
                List<TransferDataModel> selectedid = new List<TransferDataModel>();
                selectedid = objModel.SelectedId(id).ToList();
                List<TransferDataModel> Rolelist = new List<TransferDataModel>();
                {
                    Rolelist = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TranferFrom.ToString())
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TransferTo.ToString())
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    Boolean selected = false;
                    if (item.ModuleId.ToString() == selectedid[0].ModuleId.ToString())
                    {
                        selected = true;
                        modulename = item.ModuleName;
                    }
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.OwnerShip = role2;

                ViewBag.TransferDate = selectedid[0].TransferDate;
                ViewBag.Remarks = selectedid[0].Remarks;
                DataTable dt = objModel.Selectedownerid(id);
                if (dt.Rows.Count > 0)
                {
                    owner.TranferId = Convert.ToInt32(dt.Rows[0]["transferowndet_transferown_gid"].ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        owner.lstSelectedOwnerGid[i] = string.IsNullOrEmpty(dt.Rows[i]["ownership_gid"].ToString()) ? "" : dt.Rows[i]["ownership_gid"].ToString();
                    }
                    dt = objModel.SelectOwnership(modulename);

                    List<SelectListItem> names = new List<SelectListItem>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            names.Add(new SelectListItem { Text = row["ownership_name"].ToString(), Value = row["ownership_gid"].ToString() });
                            owner.OwnerShip = names;
                        }
                    }
                }
                return PartialView(owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult EditOwner(TransferDataModel trn, string[] SelectedOwner)
        {
            try
            {
                if(SelectedOwner==null)
                {
                   SelectedOwner = new string[] {""};
                }
                //if (Session["Transferid"]!=null)
                //{
                //    trn.TranferId =Convert.ToInt16(Session["Transferid"]);
                //}
                result = objModel.EditOwner(trn);
                result = objModel.Delete(Convert.ToInt16(trn.TranferId));
                result = objModel.EditOwnerDetails(SelectedOwner, Convert.ToInt16(trn.TranferId));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult DeleteOwnerView(int id)
        {
            try
            {
                Session["Transferid"] = id;
                List<TransferDataModel> selectedid = new List<TransferDataModel>();
                selectedid = objModel.SelectedId(id).ToList();
                List<TransferDataModel> Rolelist = new List<TransferDataModel>();
                {
                    Rolelist = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TranferFrom.ToString())
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Employee = role;
                //2
                List<TransferDataModel> Rolelist1 = new List<TransferDataModel>();
                {
                    Rolelist1 = objModel.SelectEmployee().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    Boolean selected = false;
                    if (item.EmployeeId.ToString() == selectedid[0].TransferTo.ToString())
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.EmployeeName.ToString(),
                        Value = item.EmployeeId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Module = role1;
                //3
                List<TransferDataModel> Rolelist2 = new List<TransferDataModel>();
                {
                    Rolelist2 = objModel.SelectModule().ToList();
                };
                List<SelectListItem> role2 = new List<SelectListItem>();
                foreach (var item in Rolelist2)
                {
                    Boolean selected = false;
                    if (item.ModuleId.ToString() == selectedid[0].ModuleId.ToString())
                    {
                        selected = true;
                    }
                    role2.Add(new SelectListItem
                    {
                        Text = item.ModuleName.ToString(),
                        Value = item.ModuleId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.OwnerShip = role2;

                ViewBag.TransferDate = selectedid[0].TransferDate;
                ViewBag.Remarks = selectedid[0].Remarks;

                dt = objModel.Selectedownerid(id);
                TransferDataModel owner = new TransferDataModel();
                List<SelectListItem> names = new List<SelectListItem>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add(new SelectListItem { Text = row["ownership_name"].ToString(), Value = row["ownership_gid"].ToString() });
                        owner.OwnerShip = names;
                    }
                }
                return PartialView(owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteOwner(TransferDataModel trn)
        {
            try
            {
                result = objModel.DeleteOwner(Convert.ToInt16(trn.TranferId));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult  excelexport()
        {
            List<TransferDataModel> TrList = new List<TransferDataModel>();
            TrList = (List<TransferDataModel>)Session["transferexport"]; 



            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Transfer From");
            dt.Columns.Add("Transfer To");
            dt.Columns.Add("Module");
            dt.Columns.Add("Transfer Date");
           


            for (int i = 0; i < TrList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    TrList[i].TranferFrom.ToString(),
                    TrList[i].TransferTo.ToString(),
                    TrList[i].ModuleName.ToString(),
                    TrList[i].TransferDate.ToString()                  
                 

                                       );
            }
        
           // DataTable dt = (DataTable)Session["transferexport"];
            //if (_downloadingData != null)
            string attachment = "attachment; filename=TransferOwnerShipReport.xls";
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

    }
}
