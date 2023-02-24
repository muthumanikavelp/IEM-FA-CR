using IEM.Areas.EOW.Models;
using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class TravelClassController : Controller
    {
        private TravelClassRepository objModel;
        int id;
        string Result;
        string result;
        public TravelClassController()
            : this(new TravelClassModel())
        {

        }
        public TravelClassController(TravelClassRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult TravelIndex()
        {
            try
            {
                List<TravelClassEntity> Rolelist = new List<TravelClassEntity>();
                {
                    Rolelist = objModel.SelectModeOfTravel().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.TravelMode.ToString(),
                        Value = item.TravelModeId.ToString()
                    });
                }
                ViewBag.Rolelist = role;
                List<TravelClassEntity> Travel = new List<TravelClassEntity>();
                Travel = objModel.SelectTravelSearch().ToList();
                return View(Travel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult TravelIndex(string filter_role, string Rolelist)
        {
            try
            {
                List<TravelClassEntity> Rolelist1 = new List<TravelClassEntity>();
                {
                    Rolelist1 = objModel.SelectModeOfTravel().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    Boolean selectedclas = false;
                    if (item.TravelModeId.ToString() == Rolelist.ToString())
                    {
                        selectedclas = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.TravelMode.ToString(),
                        Value = item.TravelModeId.ToString(),
                        Selected = selectedclas
                    });
                }
                ViewBag.Rolelist = role;
                List<TravelClassEntity> Travel = new List<TravelClassEntity>();
                Travel = objModel.SelectTravelSearch(filter_role, Rolelist).ToList();
                ViewBag.filter_role = filter_role;
                if (Travel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return View(Travel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult TravelAdd()
        {
            try
            {
                List<TravelClassEntity> Rolelist = new List<TravelClassEntity>();
                {
                    Rolelist = objModel.SelectModeOfTravel().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.TravelMode.ToString(),
                        Value = item.TravelModeId.ToString()
                    });
                }
                ViewBag.Rolelist = role;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult TravelSubmit(TravelClassEntity mode)
        {
            try
            {
                Result = objModel.SubmitTravel(mode);
                if (Result == "sub")
                {
                    result = "Record Added Successfully!";
                }
                if (Result == "duplicate")
                {
                    result = "You Can't Add Duplicate Record";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult TravelEdit(int id)
        {
            try
            {
                TravelClassEntity tra = new TravelClassEntity();
                List<TravelClassEntity> mode = new List<TravelClassEntity>();
                mode = objModel.EditTravel(id).ToList();
                tra.TravelClass = mode[0].TravelClass;
                tra.TravelId = mode[0].TravelId;
                Session["TravelId"] = mode[0].TravelId;
                tra.TravelModeId = mode[0].TravelModeId;
                tra.listofdata = new SelectList(objModel.SelectModeOfTravel().ToList(), "TravelModeId", "TravelMode", mode[0].TravelModeId.ToString());
                return PartialView(tra);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult TravelEditSubmit(TravelClassEntity mode)
        {
            try
            {
                if (Session["TravelId"] != null)
                {
                    mode.TravelId = Convert.ToInt16(Session["TravelId"]);
                }

                Result = objModel.SubmitEditTravel(mode);
                if (Result == "sub")
                {
                    result = "Record Updated Successfully!";
                }
                if (Result == "duplicate")
                {
                    result = "You Can't Edit Duplicate Record";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult TravelView(int id)
        {
            try
            {
                TravelClassEntity tra = new TravelClassEntity();
                List<TravelClassEntity> mode = new List<TravelClassEntity>();
                mode = objModel.EditTravel(id).ToList();
                tra.TravelClass = mode[0].TravelClass;
                tra.TravelId = mode[0].TravelId;
                Session["TravelId"] = mode[0].TravelId;
                tra.TravelModeId = mode[0].TravelModeId;
                tra.listofdata = new SelectList(objModel.SelectModeOfTravel().ToList(), "TravelModeId", "TravelMode", mode[0].TravelModeId.ToString());
                return PartialView(tra);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult TravelDelete(int id)
        {
            try
            {
                Session["modeid"] = id;
                TravelClassEntity tra = new TravelClassEntity();
                List<TravelClassEntity> mode = new List<TravelClassEntity>();
                mode = objModel.EditTravel(id).ToList();
                tra.TravelClass = mode[0].TravelClass;
                tra.TravelId = mode[0].TravelId;
                Session["TravelId"] = mode[0].TravelId;
                tra.TravelModeId = mode[0].TravelModeId;
                tra.listofdata = new SelectList(objModel.SelectModeOfTravel().ToList(), "TravelModeId", "TravelMode", mode[0].TravelModeId.ToString());
                return PartialView(tra);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult ModeDelete()
        {
            try
            {
                objModel.DeleteTravel(Convert.ToInt16(Session["modeid"]));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
