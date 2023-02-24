using IEM.Areas.ASMS.Models;
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
using System.Web.Providers.Entities;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class ModSummaryController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IRepository objModel;

        public ModSummaryController()
            :this(new SupDataModel()) 
        {

         }
        public ModSummaryController(IRepository objM)
        {
            objModel = objM; 
        }

        //
        // GET: /ASMS/ModSummary/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult SupplierHeaderSummary()  
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult DirectorsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ServiceHistorySummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BranchDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ManPowerDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ProductServiceDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult SubContractorDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CustomerDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BranchCityDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult AwardDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ClientDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ContactDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ContactPersonDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult TaxDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult TaxSubTypeDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult PaymentDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ActivityDetailsSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult SupAttachmentSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult OthersSummary() 
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EmpRelationshipSummary()  
        {
            try
            {
                List<ModificationSummary> lst = new List<ModificationSummary>();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
