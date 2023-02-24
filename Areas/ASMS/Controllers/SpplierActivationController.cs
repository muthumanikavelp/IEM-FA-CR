using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

    [NoDirectAccess]
    public class SpplierActivationController : Controller
    {
        private IRepositoryRenewal IrAt;

        public SpplierActivationController()
            :this(new SupRenwalModel())
        {

        }
        public SpplierActivationController(IRepositoryRenewal IreAct)
        {
            IrAt = IreAct;
        }

        public ActionResult SupplierActivation(string id)
        {

            List<SupplierActivation> obj = new List<SupplierActivation>();
            SupplierActivation objAct = new SupplierActivation();
            DataSet getds = new DataSet();
            getds = IrAt.GetActive_emp(id);
            if (getds.Tables[0].Rows.Count>0)
            {
                //for(int i=0;i<getds.Tables[0].Rows.Count;i++)
                //{
                //    objAct = new SupplierActivation();
                
                //}

                for(int j=0;j<getds.Tables[1].Rows.Count;j++)
                {
                    objAct = new SupplierActivation();
                    objAct.Approverid = Convert.ToInt32(getds.Tables[1].Rows[j]["employee_gid"].ToString());
                    objAct.ApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                    objAct.Supplierheadergid = getds.Tables[0].Rows[0][0].ToString();
                    objAct.SupplierCode = getds.Tables[0].Rows[0][1].ToString();
                    objAct.SupplierName = getds.Tables[0].Rows[0][2].ToString();
                    obj.Add(objAct);
                }
               // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
                objAct.ApproverList = new SelectList(obj, "Approverid", "ApproverName");
                Session["listobj"] = obj;
            }

            return View(objAct);
        }


        [HttpPost]
        public ActionResult SupplierActivation(SupplierActivation obj, List<HttpPostedFileBase> fileUpload)
        {
         //   int i = 0;
            List<SupplierActivation> objj = new List<SupplierActivation>();
            if (obj.FilesToBeUploaded != null)
            {
                foreach (HttpPostedFileBase item in fileUpload)
                {
                    if (item != null && Array.Exists(obj.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                    {
                        string path = "//192.168.0.130/images/";
                        string filename = Path.GetFileName(item.FileName);
                        Request.Files[0].SaveAs(Path.Combine(path, filename));
                        //obj.arrAttachfile[] = item.FileName.ToString();
                        //i=i+1;
                    }
                }
                obj.arrAttachfile = obj.FilesToBeUploaded.Split(',');
            }
           string Result = IrAt.Update_SupplierActive(obj);
            objj = (List<SupplierActivation>)Session["listobj"];
            obj.SupplierCode = string.Empty;
            obj.SupplierName = string.Empty;
       
            obj.Todate = string.Empty;
            obj.ApproverList = new SelectList(objj, "Approverid", "ApproverName");

            ViewBag.status = Result;
            return View(obj);
        }

        public PartialViewResult Attachments(SupplierActivation obj)
        {
            List<SupplierActivation> lobj = new List<SupplierActivation>();
            //foreach (string upload in Request.Files)
            //{
            //    if (Request.Files[upload].ContentLength > 0)
            //    {
            //        string path = "//192.168.0.130/images/";
            //        string filename = Path.GetFileName(Request.Files[upload].FileName);
            //        Request.Files[upload].SaveAs(Path.Combine(path, filename));
            //    }


            //}
            //string file = obj.AttachFileName.ToString();
            //string filename
            //FileUpload fu = new FileUpload();
            //fu.FileName(obj.AttachFileName);
            //FileInfo fi = new FileInfo(obj.AttachFileName);
            //string fnam = fi.Name;
            //obj.AttachFileName = fnam;
            //obj.Today = DateTime.Now.ToString("dd-MM-yyyy");
            //lobj.Add(obj);

            //obj.AttchList = lobj.ToList();
            //ViewBag.status = "yes";
            return PartialView(obj);
        }

    }
}
