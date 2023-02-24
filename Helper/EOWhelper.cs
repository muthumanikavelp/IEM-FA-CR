using IEM.Areas.EOW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using IEM.Common;

namespace IEM.Helper
{

    public class EOWhelper : sqlLib
    {


        #region Get supplierlist
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public DataSet getsupplierlist()
        {
            ProcedureName = "pr_eow_com_supplierdetails";
            AddParameter("action", "supplierdetails");
            return ExecuteProcedure;
        }
        public DataSet Fetchsupplier(string suppliercode, string suppliername)
        {
            ProcedureName = "pr_eow_com_supplierdetails";
            AddParameter("supplier_gid", "suppliercode");
            AddParameter("SUPPLIERNAMAE", "suppliername");
            AddParameter("action", "suppliersearch");
            return ExecuteProcedure;
        }
        #endregion
        public DataSet Getcurrency()
        {
            ProcedureName = "pr_eow_com_gettaxrate";
            AddParameter("action", "getcurrencys");
            return ExecuteProcedure;
        }
        public DataSet GetMaster(string ViewType, string CatId, string UId)
        {
            ProcedureName = "PR_FS_Get_Master";

            AddParameter("ViewType", ViewType);
            AddParameter("CatId", CatId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet Setecfheaderdetails(EOW_Supplierinvoice data)
        {
            ProcedureName = "pr_fi_set_ecfdetails";
            AddParameter("ecf_gid", data.ecf_GID.ToString());
            AddParameter("ecf_supplier_employee", "S");
            AddParameter("ecf_suppliercode", string.IsNullOrEmpty(data.Suppliercode) ? "" : data.Suppliercode);
            AddParameter("ecf_date", string.IsNullOrEmpty(objCmnFunctions.convertoDateTimeString(data.ECF_Date).ToString()) ? "" : objCmnFunctions.convertoDateTimeString(data.ECF_Date).ToString());
            AddParameter("ecf_create_mode", "S");
            AddParameter("ecf_raiser", data.ecfraisergid);
            AddParameter("ecf_doctype_gid", "0");
            AddParameter("ecf_description", string.IsNullOrEmpty(data.ecfdescription) ? "" : data.ecfdescription);
            AddParameter("ecf_docsubtype_gid", "0");
            AddParameter("ecf_claim_month", objCmnFunctions.convertoDateTimeString(data.ECF_Date).ToString());
            AddParameter("ecf_amount",objCmnFunctions.GetRemovecommas(data.ECF_Amount));
            AddParameter("ecf_amort_flag", string.IsNullOrEmpty(data.amort) ? "" : data.amort);
            AddParameter("ecf_amort_from", string.IsNullOrEmpty(data.from) ? "" : data.from);
            AddParameter("ecf_amort_to", string.IsNullOrEmpty(data.to) ? "" : data.to);
            AddParameter("ecf_amort_desc", string.IsNullOrEmpty(data.amortdec) ? "" : data.amortdec);
            AddParameter("ecf_delmat_amount", string.IsNullOrEmpty((data.ECF_Amount)) ? "" : objCmnFunctions.GetRemovecommas(data.ECF_Amount));
            AddParameter("ecf_currency_amount", string.IsNullOrEmpty((data.Currencyamount))?"":objCmnFunctions.GetRemovecommas(data.Currencyamount));
            AddParameter("ecf_status", "0");
            AddParameter("ecf_all_status", "0");
            AddParameter("ecf_urgent_flag", "N");
            AddParameter("ecf_insert_by", string.IsNullOrEmpty(objCmnFunctions.GetLoginUserGid().ToString()) ? "" : objCmnFunctions.GetLoginUserGid().ToString());
            AddParameter("ecf_remark", string.IsNullOrEmpty(data.ecfremark) ? "" : data.ecfremark);
            if (data.ecf_GID != 0 && data.ecf_GID.ToString() != "")
            {
                AddParameter("ecf_SaveMode", "E");
            }
            else
            {
                AddParameter("ecf_SaveMode", "I");

            }
            return ExecuteProcedure;
        }


        public DataSet UpdateSupplierinvoice(EOW_SupplierModelgrid data)
        {
            ProcedureName = "pr_fi_set_invoicedetails";
            AddParameter("invoice_gid", data.Invoicegid.ToString());
            AddParameter("invoice_ecf_gid", data.ecfgid);
            AddParameter("invoice_supplier_gid", data.suppliergid);
            AddParameter("invoice_type ", "S");
            AddParameter("invoice_date", string.IsNullOrEmpty(objCmnFunctions.convertoDateTimeString(data.InvoiceDate).ToString()) ? "" : objCmnFunctions.convertoDateTimeString(data.InvoiceDate).ToString());
            AddParameter("invoice_service_month", string.IsNullOrEmpty(objCmnFunctions.convertoDateTimeString(data.SerMonth).ToString()) ? "" : objCmnFunctions.convertoDateTimeString(data.SerMonth).ToString());
            AddParameter("invoice_no", data.InvoiceNo);
            AddParameter("invoice_desc", string.IsNullOrEmpty(objCmnFunctions.Getreplacesinglequotes(data.Description)) ? "" : objCmnFunctions.Getreplacesinglequotes(data.Description));
            AddParameter("invoice_amount", objCmnFunctions.GetRemovecommas(data.Amount));
            AddParameter("invoice_wotax_amount", objCmnFunctions.GetRemovecommas(data.TaxableAmount));
            AddParameter("invoice_provision_flag", data.Provision);
            AddParameter("invoice_retention_flag", data.Retensionflg);
            AddParameter("invoice_retention_rate", data.Retensionper);
            AddParameter("invoice_retention_amount", string.IsNullOrEmpty(data.Retensionamount) ? "0" : objCmnFunctions.GetRemovecommas(data.Retensionamount.ToString()));
            AddParameter("invoice_retention_exception", data.Retensionamount);
            AddParameter("invoice_retention_releaseon", string.IsNullOrEmpty(Convert.ToString(data.Retensionrelse)) ? "" : objCmnFunctions.convertoDateTimeString(data.Retensionrelse).ToString());
            AddParameter("invoice_gst_charged", data.GstCharged);
            AddParameter("invoice_provider_location", data.ProviderLocation);
            AddParameter("invoice_gstin_vendor", string.IsNullOrEmpty(data.GstinVendor) ? "" : data.GstinVendor);
            AddParameter("invoice_receiver_location", data.ReceiverLocation);
            AddParameter("invoice_gstin_ficcl", string.IsNullOrEmpty(data.GstinFiccl) ? "" : data.GstinFiccl);
            //if (data.ecf_GID != 0 && data.ecf_GID.ToString() != "")
            //{
            //    AddParameter("ecf_SaveMode", "E");
            //}
            //else
            //{
            //    AddParameter("ecf_SaveMode", "I");

            //}
            return ExecuteProcedure;
        }

    }
}