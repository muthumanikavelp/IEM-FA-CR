
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using IEM.Areas.EOW.Models;
using IEM.App_Start;
namespace IEM.Areas.EOW.Cotrollers
{

    [NoDirectAccess]
    public class RoleMasterController : Controller
    {
        TreeNode nodeToAdd = null, ParentNodeTofind = null;
        SortedList parrentnode = new SortedList();
        SortedList newparent = new SortedList();
        ArrayList parentnode = new ArrayList();
        TreeNode node, parent = null;
        SupClassificationModel sub = new SupClassificationModel();
        private RollRepository objModel;
        public RoleMasterController()
            : this(new RoleMasterModel())
        {

        }
        public RoleMasterController(RollRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Role()
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            SupClassificationModel sub = new SupClassificationModel();
            //obj = objModel.SelectHoliday.ToList();
            return View(obj);
           
        }
        [HttpPost]
        public ActionResult Role(string filter = null)
        {
            List<SupClassificationModel> records = new List<SupClassificationModel>();
            //records = objModel.SelectHoliday().ToList();
            if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
            {
                ViewBag.filter = filter.ToString();
                records = records.Where(x => filter == null ||
                       (x.Rollname.Contains(filter))).ToList();
            }
            return View(records);
        }
        public PartialViewResult RoleNew()
        {
            return PartialView();
        }
        public ActionResult mainview23()
        {

            return View();
        }
        public RedirectResult asppage()
        {
            return Redirect("TreeView.aspx");
        }
        
       
    }
}
