using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierRenewalController : Controller
    {
        //
        // GET: /SupplierRenewal/
        private IRepositoryRenewal Iree;
        public SupplierRenewalController() : this (new SupRenwalModel())
        {

        }
        public SupplierRenewalController(IRepositoryRenewal Ireee)
        {
            Iree = Ireee;
        }
        public ActionResult RenewalPage1(string id)
        {
            SupplierContractRenewal obj = new SupplierContractRenewal();
            List<SupplierContractRenewal> objvar1 = new List<SupplierContractRenewal>();
            objvar1 = Iree.GetSearchRenewal(id, string.Empty, string.Empty, string.Empty,string.Empty).ToList();
            obj.SupplierheadGid = objvar1[0].SupplierheadGid.ToString();
            obj.SupplierCode = objvar1[0].SupplierCode.ToString();
            obj.SupplierName = objvar1[0].SupplierName.ToString();
            obj.Cont_startDate = objvar1[0].Cont_startDate.ToString();
            obj.Cont_EndDate = objvar1[0].Cont_EndDate.ToString();
            ViewBag.status = string.Empty;
            return View(obj);
        }

        [HttpPost]
        public ActionResult RenewalPage1(SupplierContractRenewal newobj)
        {
           // SupplierContractRenewal nobj = new SupplierContractRenewal();
            //List<SupplierContractRenewal> objrenew = new List<SupplierContractRenewal>();
            try
            {
                //nobj.SupplierheadGid = newobj.SupplierheadGid;
                //nobj.SupplierCode = newobj.SupplierCode;
                //nobj.SupplierName = newobj.SupplierName;
                //nobj.Cont_startDate = newobj.Cont_startDate;
                //nobj.Cont_EndDate = newobj.Cont_EndDate;
                //nobj.Cont_EndDateNew = newobj.Cont_EndDateNew;
                //nobj.ActivityCount = newobj.ActivityCount;
                //nobj.Description = newobj.Description;
              //  objrenew.Add(nobj);
               
               //objrenew = Iree.InsertRenewal(objrenew).ToList();
                foreach (string upload in Request.Files)
                {
                    if (Request.Files[upload].ContentLength > 0)
                    {
                        string path = "//192.168.0.130/images/";
                        string filename = Path.GetFileName(Request.Files[upload].FileName);
                        Request.Files[upload].SaveAs(Path.Combine(path, filename));
                        newobj.filename += filename;
                    }
                    
                    
                }
                newobj.SupplierRenewal = Iree.InsertRenewal(newobj).ToList();
                if (newobj.SupplierRenewal.Count>0)
                {
                    ViewBag.status = "yes";
                }
                
            }
            catch(Exception ex)
            {

            }

            return View(newobj);
        }

       
    }
}
