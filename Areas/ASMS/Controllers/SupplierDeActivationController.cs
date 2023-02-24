using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using System.Data;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierDeActivationController : Controller
    {
        public IRepositoryRenewal IrD;

        public SupplierDeActivationController()
            :this(new SupRenwalModel())
        {

        }

        public SupplierDeActivationController(IRepositoryRenewal IrDc)
        {
            IrD = IrDc;
        }
      
        public ActionResult SpplierDeActive(string id)
        {

         //   List<SupplierDeActvation> obj = new List<SupplierDeActvation>();
            SupplierDeActvation objAct = new SupplierDeActvation();
            DataSet getds = new DataSet();
            getds = IrD.GetActive_emp(id);
            if (getds.Tables[0].Rows.Count > 0)
            {
              
                //for (int j = 0; j < getds.Tables[1].Rows.Count; j++)
                //{
                //    objAct = new SupplierDeActvation();
                    //objAct.Approverid = Convert.ToInt32(getds.Tables[1].Rows[j]["employee_gid"].ToString());
                    //objAct.ApproverName = getds.Tables[1].Rows[j]["employee_name"].ToString();
                    objAct.DSupplierheadergid = getds.Tables[0].Rows[0][0].ToString();
                    objAct.DSupplierCode = getds.Tables[0].Rows[0][1].ToString();
                    objAct.DSupplierName = getds.Tables[0].Rows[0][2].ToString();
                   // obj.Add(objAct);
               // }
                // objDetails.productList = new SelectList(objModel.getlist(), "productgid", "parentProduct");
             //   objAct.ApproverList = new SelectList(obj, "Approverid", "ApproverName");
                //Session["listobj"] = obj;
            }

            return View(objAct);
        }

        [HttpPost]
        public ActionResult SpplierDeActive(SupplierDeActvation objDe,List<HttpPostedFileBase> fileUpload)
        {
            if(objDe.FilesToBeUploaded!=null)
            {
                foreach (HttpPostedFileBase item in fileUpload)
                {
                    if (item != null && Array.Exists(objDe.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                    {
                        string path = "//192.168.0.130/images/";
                        string filename = Path.GetFileName(item.FileName);
                        Request.Files[0].SaveAs(Path.Combine(path, filename));
                        //obj.arrAttachfile[] = item.FileName.ToString();
                        //i=i+1;
                    }
                }
                objDe.arrDeAttachfile = objDe.FilesToBeUploaded.Split(',');

            }
            
            string Result = IrD.DeUpdate_SupplierActive(objDe);
            ViewBag.status = Result;
            return View(objDe);
        }

    }
}
