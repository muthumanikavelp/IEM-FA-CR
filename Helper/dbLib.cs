using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Helper
{
    public class dbLib : sqlLib
    {
        #region Variables
        private proLib plib = new proLib();
        #endregion

        #region Courier & Employee Auto Complete
        public DataSet GetAutoComplete(string SearchText, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_AutoComplete";

            AddParameter("SearchText", SearchText);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetMaster(string ViewType, string UId, int prodservice = 0)
        {
            ProcedureName = "PR_FS_Get_Master";

            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            AddParameter("ProdService", Convert.ToString(prodservice));

            return ExecuteProcedure;
        }


        public DataSet GetMaster1(string ViewType, string UId, string supid)
        {
            ProcedureName = "PR_FS_Get_Master";

            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            AddParameter("CatId", supid);
            AddParameter("EcfId",Convert.ToString( HttpContext.Current.Session["EcfIdForTax"]));

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

        public DataSet GetHSNMaster(string ViewType, string CatId, string UId,string POType)
        {
            ProcedureName = "PR_FS_Get_Master";

            AddParameter("ViewType", ViewType);
            AddParameter("CatId", CatId);
            AddParameter("UId", UId);
            AddParameter("@ProdService", POType);

            return ExecuteProcedure;
        }

        public DataSet GetStaleChequepaymodedetails(string Paymode, string ECFNO, string UId, string SuporEmpDetail, string PVNO)
        {
            ProcedureName = "PR_FS_Get_Paymode";

            AddParameter("ViewType", "Paymodedetails");
            AddParameter("Pay", Paymode);
            AddParameter("UId", UId);
            AddParameter("Ecfno", ECFNO);
            AddParameter("sup_Emp", SuporEmpDetail);
            AddParameter("PVNO", PVNO);

            return ExecuteProcedure;
        }


        public DataSet GetAutoComplete(string SearchText, string ViewType, string Code, string UId)
        {
            ProcedureName = "PR_FS_Get_AutoComplete";

            AddParameter("SearchText", SearchText);
            AddParameter("ViewType", ViewType);
            AddParameter("Code", Code);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Get Employee Role
        public DataSet GetEmployeeRole(string UId)
        {
            ProcedureName = "PR_FS_GET_ROLEEMPLOYEE";
            AddParameter("Empid", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Pouch Inward
        public DataSet GetPouchInward(string PouchId, string DateFrom, string DateTo, string CourierId, string AWBNo, string RecFrom, string UId)
        {
            ProcedureName = "PR_FS_Get_PouchInward";

            AddParameter("PouchId", PouchId);
            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("CourierId", CourierId);
            AddParameter("AWBNo", AWBNo);
            AddParameter("RecFrom", RecFrom);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetPouchInwardDetails(string PouchId, string UId)
        {
            ProcedureName = "PR_FS_Get_PouchInwardDetail";

            AddParameter("PouchId", PouchId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPouchInward(string DPGId, string Sender_Type, string Sender_Id, string RecDate, string RecFrom, string Doc_Count, string Courier_Id, string AWB_No, string Remarks, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_PouchInward";

            AddParameter("DPGId", DPGId);
            AddParameter("Sender_Type", Sender_Type);
            AddParameter("Sender_Id", Sender_Id);
            AddParameter("RecDate", RecDate);
            AddParameter("RecFrom", RecFrom);
            AddParameter("Doc_Count", Doc_Count);
            AddParameter("Courier_Id", Courier_Id);
            AddParameter("AWB_No", AWB_No);
            AddParameter("Remarks", Remarks);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Physical Receipting
        public DataSet GetPhysicalReceipt(string PouchId, string UId)
        {
            ProcedureName = "PR_FS_Get_PhysicalReceipt";

            AddParameter("Pouch_GId", PouchId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPhysicalReceipt(string PRGId, string Pouch_GId, string DocType, string DocRefNo, string Phy_Verification, string Remarks, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_PhysicalReceipt";

            AddParameter("PRGId", PRGId);
            AddParameter("Pouch_GId", Pouch_GId);
            AddParameter("DocType", DocType);
            AddParameter("DocRefNo", DocRefNo);
            AddParameter("Phy_Verification", Phy_Verification);
            AddParameter("Remarks", Remarks);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Urgent Tagging
        public DataSet GetUrgentTagging(string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string DocTypeId, string DocRefNo,
            string ApprDate, string Amount, string ViewType, string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_UrgentTagging";

            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("DocType", DocTypeId);
            AddParameter("DocRefNo", DocRefNo);
            AddParameter("ApprDate", ApprDate);
            AddParameter("Amount", Amount);
            AddParameter("ViewType", ViewType);
            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetUrgentTag(string UTagId, string EcfId, string ReqBy, string ReqDate, string Remarks, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_UrgentTagging";

            AddParameter("UTagId", UTagId);
            AddParameter("EcfId", EcfId);
            AddParameter("ReqBy", ReqBy);
            AddParameter("ReqDate", ReqDate);
            AddParameter("Remarks", Remarks);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Document Batching
        public DataSet GetDocumentBatching(string InwardDate, string DateFrom, string DateTo, string BatchNo, string ViewType, string UId)
        {
            ProcedureName = "pr_fs_get_batching";
            AddParameter("InwardDate", InwardDate);
            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("BatchNo", BatchNo);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetDocumentBatching(string BatchId, string InwardDate, string InwardFrom, string InwardTo, string IsRemoved, string UId)
        {
            ProcedureName = "pr_fs_set_batching";
            AddParameter("BatchId", BatchId);
            AddParameter("InwardDate", InwardDate);
            AddParameter("InwardFrom", InwardFrom);
            AddParameter("InwardTo", InwardTo);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Physical Pullout
        public DataSet FetchPhysicalPullout(string DateFrom, string DateTo, string DocInwNo, string DocRefNo, string DocType, string DocAmount, string ReqBy, string UId)
        {
            ProcedureName = "PR_FS_Get_Pullout";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("DocInwNo", DocInwNo);
            AddParameter("DocRefNo", DocRefNo);
            AddParameter("DocType", DocType);
            AddParameter("DocAmount", DocAmount);
            AddParameter("ReqBy", ReqBy);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPhysicalPullout(string PloId, string ReqDate, string ReqBy, string Reason, string PloDate, string DocInwNo, string UId)
        {
            ProcedureName = "PR_FS_Set_Pullout";

            AddParameter("PloId", PloId);
            AddParameter("ReqDate", ReqDate);
            AddParameter("ReqBy", ReqBy);
            AddParameter("Reason", Reason);
            AddParameter("PloDate", PloDate);
            AddParameter("DocInwNo_DocNo", DocInwNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Physical Pullout Receipt
        public DataSet SetPhysicalPulloutReceipting(string PloRId, string PloId, string RetDate, string RetBy, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_PulloutReceipt";

            AddParameter("PloRId", PloRId);
            AddParameter("PloId", PloId);
            AddParameter("RetDate", RetDate);
            AddParameter("RetBy", RetBy);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Audit Work Tray
        public DataSet GetAuditWorkTray(string DocNo, string InvoiceNo, string DocTypeId, string DocAmount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string UId)
        {
            ProcedureName = "PR_FS_Get_AuditWorkTraynew";

            AddParameter("DocNo", DocNo);
            AddParameter("InvoiceNo", InvoiceNo);
            AddParameter("DocTypeId", DocTypeId);
            AddParameter("DocAmount", DocAmount);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetAuditCheckList(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_AuditCheckList";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAuditCheckList(string EcfId, string ChkLstIds, string Status, string Remarks, string Reason, string UId)
        {
            ProcedureName = "PR_FS_Set_AuditCheckList";

            AddParameter("EcfId", EcfId);
            AddParameter("ChkLstIds", ChkLstIds);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("Reason", Reason);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAuditCheckListReason(string EcfId, string ChkLstId, string Status, string Reason, string ChkLstIds, string UId)
        {
            ProcedureName = "PR_FS_Set_CheckListReason";

            AddParameter("EcfId", EcfId);
            AddParameter("ChkLstId", ChkLstId);
            AddParameter("Status", Status);
            AddParameter("Reason", Reason);
            AddParameter("ChkLst_Ids", ChkLstIds);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetLeaseBulkApproval(string ECFIds, string Status, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_AuditCheckListLEASEDocs";
          
            AddParameter("ECFIds", ECFIds);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetIsAssigned(string ECFId, string UId)
        {
            ProcedureName = "PR_FS_Get_IsAssignedECF";

            AddParameter("ECFId", ECFId);
            //AddParameter("TAG",tag);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        #endregion

        #region GetCheckList
        public DataSet GetCheckList(string DocSubTypeId, string ParentId, string PoType, string IsActive, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_CheckListMaster";

            AddParameter("DocSubTypeId", DocSubTypeId);
            AddParameter("ParentId", ParentId);
            AddParameter("PoType", PoType);
            AddParameter("IsActive", IsActive);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetCheckList(string ChkLstId, string ParentId, string DocSubTypeId, string PoType, string Title, string IsConfirmed, string Reason, string IsActive, string DisOrder, string IsRemoved, string IsActiveFilter, string UId)
        {
            ProcedureName = "PR_FS_Set_CheckListMaster";

            AddParameter("ChkLstId", ChkLstId);
            AddParameter("ParentId", ParentId);
            AddParameter("DocSubTypeId", DocSubTypeId);
            AddParameter("PoType", PoType);
            AddParameter("Title", Title);
            AddParameter("IsConfirmed", IsConfirmed);
            AddParameter("Reason", Reason);
            AddParameter("IsActive", IsActive);
            AddParameter("DisOrder", DisOrder);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("IsActiveFilter", IsActiveFilter);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Inex Summary
        public DataSet GetInex(string InexFrom, string InexTo, string InexById, string DocAmount, string DocFrom, string DocTo, string DocTypeId, string DocNo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_Inex";

            AddParameter("InexFrom", InexFrom);
            AddParameter("InexTo", InexTo);
            AddParameter("InexById", InexById);
            AddParameter("DocAmount", DocAmount);
            AddParameter("DocFrom", DocFrom);
            AddParameter("DocTo", DocTo);
            AddParameter("DocTypeId", DocTypeId);
            AddParameter("DocNo", DocNo);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInexReview(string InexId, string Status, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_InexReviewSummary";

            AddParameter("InexId", InexId);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInexDispatch(string InexId, string DispatchTo, string BranchId, string DispatchDate, string DispatchAddress, string CourierId, string AWB_no, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_InexDispatch";

            AddParameter("InexId", InexId);
            AddParameter("DispatchTo", DispatchTo);
            AddParameter("BranchId", BranchId);
            AddParameter("DispatchDate", DispatchDate);
            AddParameter("DispatchAddress", DispatchAddress);
            AddParameter("CourierId", CourierId);
            AddParameter("AWB_no", AWB_no);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInexReprocess(string InexId, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_InexDispatchReprocess";

            AddParameter("InexId", InexId);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Document Number Details
        public DataSet GetDocument(string DocNo)
        {
            ProcedureName = "PR_FS_Get_DocNo";

            AddParameter("DocNo", DocNo);

            return ExecuteProcedure;
        }
        #endregion

        #region Hold Release
        public DataSet GetHoldRelease(string DocType, string DocNo, string DocStatus, string HoldDate, string HoldBy, string Reason, string UId)
        {
            ProcedureName = "PR_FS_Get_HoldRelease";

            AddParameter("DocTypeId", DocType);
            AddParameter("DocNo", DocNo);
            AddParameter("DocStatus", DocStatus);
            AddParameter("HoldDate", HoldDate);
            AddParameter("HoldBy", HoldBy);
            AddParameter("Reason", Reason);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetHoldRelease(string Hold_id, string ReleaseDate, string Remarks, string Attachment, string UId)
        {
            ProcedureName = "PR_FS_Set_HoldRelease";

            AddParameter("Hold_id", Hold_id);
            AddParameter("ReleaseDate", ReleaseDate);
            AddParameter("Remarks", Remarks);
            AddParameter("Attachment", Attachment);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Cheque Inventory
        public DataSet GetChequeBookDetails(string BankId, string ChqBookNo, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_ChequeInventory";

            AddParameter("BankId", BankId);
            AddParameter("ChqBookNo", ChqBookNo);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetChequeInventory(string BankId, string Date, string ChqBookNo, string ChqNoFrom,
            string ChqNoTo, string Reason, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Set_ChequeInventory";

            AddParameter("BankId", BankId);
            AddParameter("Date", Date);
            AddParameter("ChqBookNo", ChqBookNo);
            AddParameter("ChqNoFrom", ChqNoFrom);
            AddParameter("ChqNoTo", ChqNoTo);
            AddParameter("Reason", Reason);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Payment Advice & chequePrint
        public DataSet GetPaymentAdvice(string BatchNo, string PVNoFrom, string PVNoTo, string PayDateFrom, string PayDateTo, string ClaimType, string PVAmount, string EmpNameId,
                                                string EmpCodeId, string SuppCodeId, string SuppNameId, string PayMode, string ViewType, string Location, string UId)
        {
            ProcedureName = "PR_FS_Get_PaymentAdvicePrint";

            AddParameter("BatchNo", BatchNo);
            AddParameter("PVNoFrom", PVNoFrom);
            AddParameter("PVNoTo", PVNoTo);
            AddParameter("PayDateFrom", PayDateFrom);
            AddParameter("PayDateTo", PayDateTo);
            AddParameter("ClaimType", ClaimType);
            AddParameter("PVAmount", PVAmount);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("PayMode", PayMode);
            AddParameter("ViewType", ViewType);
            AddParameter("Location", Location);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetChequePrint(string BatchNo, string PVNoFrom, string PVNoTo, string PayDateFrom, string PayDateTo, string ClaimType, string PVAmountFrom, string PVAmountTo, string EmpNameId,
                                                string EmpCodeId, string SuppCodeId, string SuppNameId, string UId)
        {
            ProcedureName = "PR_FS_Get_ChequePrint";

            AddParameter("BatchNo", BatchNo);
            AddParameter("PVNoFrom", PVNoFrom);
            AddParameter("PVNoTo", PVNoTo);
            AddParameter("PayDateFrom", PayDateFrom);
            AddParameter("PayDateTo", PayDateTo);
            AddParameter("ClaimType", ClaimType);
            AddParameter("FromPVAmount", PVAmountFrom);
            AddParameter("ToPVAmount", PVAmountTo);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetChequePrint(string PvNos, string BankId, string ChqBookNo, string ChqNoFrom, string ChqNoTo, string ChqCount, string UId)
        {
            ProcedureName = "PR_FS_Set_ChequePrint";

            AddParameter("PvNos", PvNos);
            AddParameter("BankId", BankId);
            AddParameter("ChqBookNo", ChqBookNo);
            AddParameter("ChqNoFrom", ChqNoFrom);
            AddParameter("ChqNoTo", ChqNoTo);
            AddParameter("ChqCount", ChqCount);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Payment Run
        public DataSet GetPaymentRun(string Priority, string DocNo, string AutDateFrom, string AutDateTo,
            string SuppCodeId, string SuppNameId, string RaiserCodeId, string RaiserNameId, string UId)
        {
            ProcedureName = "PR_FS_Get_PaymentRun";

            AddParameter("Priority", Priority);
            AddParameter("DocNo", DocNo);
            AddParameter("AutDateFrom", AutDateFrom);
            AddParameter("AutDateTo", AutDateTo);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("RaiserCodeId", RaiserCodeId);
            AddParameter("RaiserNameId", RaiserNameId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }


        public DataSet SetPaymentRun(string ecfId, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentRun";

            AddParameter("Ecf_ids", ecfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPaymentRunDedup(string ecfId, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentRunDedup";

            AddParameter("Ecf_ids", ecfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPaymentRunAutReverse(string EcfId, string Reason, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentRunAutReverse";

            AddParameter("EcfId", EcfId);
            AddParameter("Reason", Reason);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPaymentRunAutReverseWithGL(string EcfId, string Reason, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentRunAutReverseWithGL";

            AddParameter("EcfId", EcfId);
            AddParameter("Reason", Reason);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Supplier Invoice Maker
        public DataSet GetSupplierInvoiceMaker(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_SupplierInvoiceMaker";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetInvoiceDetails(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_invoceDetail";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetTaxDetails(string InvoiceTaxgid, string InvId, string SupplierId, string TaxId, string Subtaxgid, string TaxRate, string TaxableAmt, string TaxAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_TaxDetail";

            AddParameter("InvoiceTaxgid", InvoiceTaxgid);
            AddParameter("InvId", InvId);
            AddParameter("SupplierId", SupplierId);
            AddParameter("TaxId", TaxId);
            AddParameter("Subtaxgid", Subtaxgid);
            AddParameter("TaxRate", TaxRate);
            AddParameter("TaxableAmt", TaxableAmt);
            AddParameter("TaxAmount", TaxAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        public DataSet SetCessTaxDetails(string action, string invoicetaxgid, string InvId, string TaxId, string Subtaxgid, string TaxRate, string TaxableAmt, string TaxAmount, string UId)
        {
            ProcedureName = "pr_eow_gstcess";

            AddParameter("action ", action);
            AddParameter("invoicetaxgid ", invoicetaxgid);
            AddParameter("invoicegid ", InvId);

            AddParameter("taxgid ", TaxId);
            AddParameter("taxsubtypegid ", Subtaxgid);
            AddParameter("taxableamount ", TaxableAmt);
            AddParameter("rate ", TaxRate);
            AddParameter("amount ", TaxAmount);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        //GST Details
        public DataSet SetGSTTaxDetails(string InvoiceTaxgid, string InvId, string SupplierId, string GSNapplicable, string HSNgid, string TaxId, string Subtaxgid, string TaxRate, string TaxableAmt, string TaxAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_GSTDetail";

            AddParameter("InvoiceTaxgid", InvoiceTaxgid);
            AddParameter("InvId", InvId);
            AddParameter("SupplierId", SupplierId);
            
            AddParameter("GSNapplicable", GSNapplicable);
            AddParameter("HSNgid", HSNgid);
            AddParameter("TaxId", TaxId);
            AddParameter("TaxSubtypegid", Subtaxgid);
            AddParameter("TaxableRate", TaxRate);
            AddParameter("TaxableAmount", TaxableAmt);
            AddParameter("TaxAmount", TaxAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetDebitLineDetails(string DebitlineGId, string ECFId, string invid, string expnaturegid, string expCatId, string expSubcatId,
            string CCCode, string FCCode, string OUCode, string ProductCode, string Amount, string Desc, string IsRemoved, string UId, string hsngid,string rcm)
        {
            ProcedureName = "PR_FS_Set_DebitLine";

            AddParameter("DebitlineGId", DebitlineGId);
            AddParameter("ECFId", ECFId);
            AddParameter("invid", invid);
            AddParameter("expnaturegid", expnaturegid);
            AddParameter("expCatId", expCatId);
            AddParameter("expSubcatId", expSubcatId);
            AddParameter("CCCode", CCCode);
            AddParameter("FCCode", FCCode);
            AddParameter("OUCode", OUCode);
            AddParameter("ProductCode", ProductCode);
            AddParameter("Amount", Amount);
            AddParameter("Desc", Desc);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            AddParameter("hsngid", hsngid);
            AddParameter("rcm", rcm);
            return ExecuteProcedure;
        }

        public DataSet SetCreditLineDetails(string Ecfid, string InvId, string CreditlineGId, string Id, string paymode, string RefNo, string Beneficiary,
            string Desc, string Amount, string BankId, string IsRemoved, string IfscCode, string UId)
        {
            ProcedureName = "PR_FS_Set_CreditLine";

            AddParameter("Ecfid", Ecfid);
            AddParameter("InvId", InvId);
            AddParameter("CreditlineGId", CreditlineGId);
            AddParameter("Id", Id);
            AddParameter("paymode", paymode);
            AddParameter("RefNo", RefNo);
            AddParameter("Beneficiary", Beneficiary);
            AddParameter("Desc", Desc);
            AddParameter("Amount", Amount);
            AddParameter("BankId", BankId);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("IfscCode", IfscCode);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAttachmentDetails(string EcfId, string Invoicegid, string AttachmentId, string Attachment, string AttachmentType, string Desc,
            string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_Attachment";

            AddParameter("EcfId", EcfId);
            AddParameter("Invoicegid", Invoicegid);
            AddParameter("AttachmentId", AttachmentId);
            AddParameter("Attachment", Attachment);
            AddParameter("AttachmentType", AttachmentType);
            AddParameter("Desc", Desc);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetECFHeaderDetails(string EcfId, string ReducedAmt, string ProcessedAmt, string PaymentNett, string Amort, string currencygid, string curencycode,
            string currencyrate, string currencyamt, string ecfamt, string Desc, string UId)
        {
            ProcedureName = "PR_FS_Set_ECF";

            AddParameter("EcfId", EcfId);
            AddParameter("ReducedAmt", ReducedAmt);
            AddParameter("ProcessedAmt", ProcessedAmt);
            AddParameter("PaymentNett", PaymentNett);
            AddParameter("Amort", Amort);
            AddParameter("currencygid", currencygid);
            AddParameter("curencycode", curencycode);
            AddParameter("currencyrate", currencyrate);
            AddParameter("currencyamt", currencyamt);
            AddParameter("ecfamt", ecfamt);
            AddParameter("Desc", Desc);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        public DataSet SetInvoiceHeaderDetails(string EcfId, string InvId, string InvDate, string InvNo, string Desc, string Amount, string WOTaxAmount, string Servicemonth,
            string ProvisionFlag, string amort, string RetentionFlag, string rententionRate, string rententionAmount, string ReleaseOn, string IsVerified, string IsRemoved, string GstCharged, string ProviderLocation, string GstinVendor, string ReceiverLocation, string GstinFiccl, string Suppliergid, string UId)
        {
            ProcedureName = "PR_FS_Set_InovoiceDetail";

            AddParameter("EcfId", EcfId);
            AddParameter("InvId", InvId);
            AddParameter("InvDate", InvDate);
            AddParameter("InvNo", InvNo);
            AddParameter("Desc", Desc);
            AddParameter("Amount", Amount);
            AddParameter("WOTaxAmount", WOTaxAmount);
            AddParameter("Servicemonth", Servicemonth);
            AddParameter("ProvisionFlag", ProvisionFlag);
            AddParameter("amort ", amort);
            AddParameter("RetentionFlag", RetentionFlag);
            AddParameter("rententionRate", rententionRate);
            AddParameter("rententionAmount", rententionAmount);
            AddParameter("ReleaseOn", ReleaseOn);
            AddParameter("IsVerified", IsVerified);
            AddParameter("IsRemoved", IsRemoved);

            AddParameter("GstCharged", GstCharged);
            AddParameter("ProviderLocation", ProviderLocation);
            AddParameter("GstinVendor", GstinVendor);
            AddParameter("ReceiverLocation", ReceiverLocation);
            AddParameter("GstinFiccl", GstinFiccl);
            AddParameter("Suppliergid", Suppliergid);

            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInvoiceHeaderDetailsEmp(string EcfId, string InvId, string ProviderLocation, string GstinVendor, string Suppliergid, string InvNo, string InvDate, string Amount, string Desc, string WOTaxAmount, string Servicemonth,
           string IsVerified, string IsRemoved, string GstCharged,  string ReceiverLocation, string GstinFiccl,  string UId)
        {
            ProcedureName = "PR_FS_Set_InovoiceDetailEmployee";

            AddParameter("EcfId", EcfId);
            AddParameter("InvId", InvId);
            AddParameter("ProviderLocation", ProviderLocation);
            AddParameter("GstinVendor", GstinVendor);
            AddParameter("Suppliergid", Suppliergid);
            AddParameter("InvNo", InvNo);
            AddParameter("InvDate", InvDate);
            AddParameter("Amount", Amount);
            AddParameter("Desc", Desc);            
            AddParameter("WOTaxAmount", WOTaxAmount);
            AddParameter("Servicemonth", Servicemonth);
            AddParameter("IsVerified", IsVerified);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("GstCharged", GstCharged);
            AddParameter("ReceiverLocation", ReceiverLocation);
            AddParameter("GstinFiccl", GstinFiccl);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAdhocVendor(string Name, string GSTIN, string LocationId, string Suppliergid, string IsGST, string UId)
        {
            ProcedureName = "PR_Set_AdhocVendor";

            AddParameter("Name", Name);
            AddParameter("Suppliergid", Suppliergid);
            AddParameter("GSTIN", GSTIN);
            AddParameter("LocationId", LocationId);
            AddParameter("IsGST", IsGST);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }



        public DataSet GetTaxRateSupplier(string Taxgid, string SubTaxgid, string SupplierId, string View, string UId, string invid)
        {
            ProcedureName = "PR_FS_Get_TaxRate";

            AddParameter("Taxgid", Taxgid);
            AddParameter("SubTaxgid", SubTaxgid);
            AddParameter("SupplierId", SupplierId);
            AddParameter("View", View);
            AddParameter("invid", invid);//invoice gid passed 03-08-2017
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet FetchPaymentRefNo(string SupplierId, string Keyword, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_SupplierARF";

            AddParameter("SupplierId", SupplierId);
            AddParameter("Keyword", Keyword);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetSupplierBeneficiary(string SupplierId, string Paymode)
        {
            ProcedureName = "PR_FS_Get_SupplierBeneficiary";

            AddParameter("SupplierId", SupplierId);
            AddParameter("Paymode", Paymode);

            return ExecuteProcedure;
        }

        public DataSet SetWithHoldingTax(string withholdtaxgid, string Invoicegid, string TaxId, string taxsubtypegid, string TaxRate, string TaxableAmt,
            string TaxAmount, string NetAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_WithHoldTax";

            AddParameter("withholdtaxgid", withholdtaxgid);
            AddParameter("Invoicegid", Invoicegid);
            AddParameter("TaxId", TaxId);
            AddParameter("taxsubtypegid", taxsubtypegid);
            AddParameter("TaxRate", TaxRate);
            AddParameter("TaxableAmt", TaxableAmt);
            AddParameter("TaxAmount", TaxAmount);
            AddParameter("NetAmount", NetAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAmortSchedule(string amortgid, string ecfid, string invid, string supplierId, string amount, string glno, string startdate,
            string enddate, string frequencygid, string split, string active, string isremoved, DataTable DebitlinePercent, string UId)
        {
            ProcedureName = "PR_FS_Set_AmortSchedule";

            AddParameter("amortgid", amortgid);
            AddParameter("ecfid", ecfid);
            AddParameter("invid", invid);
            AddParameter("supplierId", supplierId);
            AddParameter("amount", amount);
            AddParameter("glno", glno);
            AddParameter("startdate", startdate);
            AddParameter("enddate", enddate);
            AddParameter("frequencygid", frequencygid);
            AddParameter("split", split);
            AddParameter("active", active);
            AddParameter("isremoved", isremoved);
            AddParameter("DebitlinePercent", DebitlinePercent);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetECFAmort(string InvId, string AmortId, string UId)
        {
            ProcedureName = "PR_FS_Get_Amort";

            AddParameter("invid", InvId);
            AddParameter("AmortId", AmortId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetPOMappingDetails(string InvoicePOId, string INVId)
        {
            ProcedureName = "pr_eow_trn_Getpomappeddetails";

            AddParameter("para1", InvoicePOId);
            AddParameter("para2", INVId);
            AddParameter("action", "POmapped");

            return ExecuteProcedure;
        }

        public DataSet GetPOMappingHeader(string POHeader, string UId)
        {
            ProcedureName = "PR_FS_Get_POHeader";

            AddParameter("POHeader", POHeader);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInvoiceDetails(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_InvoiceValidation";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetInvoiceDetailsEmp(string InvId, string IsTravel, string UId)
        {
            ProcedureName = "PR_FS_Get_InvoiceValidationEmp";

            AddParameter("InvId", InvId);
            AddParameter("IsTravel", IsTravel);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }


        public DataSet ECFRunDedup(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Set_MakerDedup";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet ExcelDebitlineUpload(string InvId, DataTable Debitline, string UId)
        {
            ProcedureName = "PR_FS_SET_DebitlineExcel";

            AddParameter("InvId", InvId);
            AddParameter("Debitline", Debitline);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetDebitLineDownloadDetails(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_DownloadDebitline";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }


        public DataSet SetDebitlineReset(string INVId, string UId)
        {
            ProcedureName = "PR_FS_Set_DebitlineReset";

            AddParameter("INVId", INVId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetCreditlineByECFGID(string EcfId)
        {
            ProcedureName = "PR_FS_Get_Creditline";

            AddParameter("EcfId", EcfId);

            return ExecuteProcedure;
        }
        #endregion

        #region Cheque No Updation
        public DataSet SetChequeNoUpdation(string ChqDate, string BankId, string ChqBookNo, string ChqNoFrom, string ChqNoTo,
                            string PVNoFrom, string PVNoTo, string Reason, string ViewType, string UId)
        {
            ProcedureName = "pr_fs_Set_chequenoupdation";

            AddParameter("ChqDate", ChqDate);
            AddParameter("BankId", BankId);
            AddParameter("ChqBookNo", ChqBookNo);
            AddParameter("ChqNoFrom", ChqNoFrom);
            AddParameter("ChqNoTo", ChqNoTo);
            AddParameter("PVNoFrom", PVNoFrom);
            AddParameter("PVNoTo", PVNoTo);
            AddParameter("Reason", Reason);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetChequeNoUpdation(string PaymentDate, string BatchNo, string SuppNameId, string EmpNameId, string UId)
        {
            ProcedureName = "pr_fs_get_chequenoupdation";
            AddParameter("PaymentDate", PaymentDate);
            AddParameter("BatchNo", BatchNo);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Cheque Despatch AWB Updation
        public DataSet GetChequeDespatchAWBUpdation(string ChqDate, string PVNo, string PaymentDate, string CourierId, string BatchNo, string Location, string RaiserBranch, string ViewType, string UId)
        {
            ProcedureName = "Pr_fs_Get_ChequeDespatchupdation";
            AddParameter("ChqDate", ChqDate);
            AddParameter("PVNo", PVNo);
            AddParameter("PaymentDate", PaymentDate);
            AddParameter("CourierId", CourierId);
            AddParameter("BatchNo", BatchNo);
            AddParameter("Location", Location);
            AddParameter("RaiserBranch", RaiserBranch);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetChequeDespatchAWBUpdation(string DespatchDate, string PvNos, string CourierId, string AwbNo, string ViewType, string XmlData, string UId, string sheets)
        {
            try
            {
                ProcedureName = "Pr_fs_Set_ChequeDespatchupdation";
                AddParameter("DespatchDate", DespatchDate);
                AddParameter("PvNos", PvNos);
                AddParameter("CourierId", CourierId);
                AddParameter("AwbNo", AwbNo);
                AddParameter("ViewType", ViewType);
                AddParameter("XmlData", XmlData);
                AddParameter("UId", UId);
                AddParameter("sheets", sheets);
                return ExecuteProcedure;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Local Conveyance
        public DataSet GetLocalConveyance(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_LocalConveyance";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #region Local Conveyance Edit PayMode
        //Ramya 04 Aug 18 LC PayMode
        public DataSet GetLocalPayment(string ecfgid, string invoicegid)
        {
            ProcedureName = "pr_eow_sup_getpaymodedetails";

            AddParameter("ecf_id", ecfgid);
            AddParameter("action", "suppliercreditdetails");

            return ExecuteProcedure;
        }
        #endregion
        public DataSet GetLocalConveyanceDetails(string EcfId, string Employeegid, string Expnaturegid, string UId)
        {
            ProcedureName = "PR_FS_Get_LocalConveyanceDetail";

            AddParameter("EcfId ", EcfId);
            AddParameter("Employeegid ", Employeegid);
            AddParameter("Expnaturegid ", Expnaturegid);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet ECFRunDedupLC(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Set_MakerDedupLCF";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Travel Claim
        public DataSet GetTravelClaim(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_TravelClaim";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        //public DataSet SetTravelExpenseDetails(string ecftravelgid, string ecfId, string InvId, string expnaturegid, string expCatId, string expSubcatId, string placefrom,
        //  string placeto, string claimperiodfrm, string claimperiodto, string trasportgid, string transportclassgid, string Distance, string Rate, string FCCode,
        //    string CCCode, string OUCode, string ProductCode, string Amount, string Desc, string IsRemoved, string UId)
        public DataSet SetTravelExpenseDetails(string ecftravelgid, string invId, string ecfId, string expnaturegid, string expCatId, string expSubcatId, string placefrom,
        string placeto, string claimperiodfrm, string claimperiodto, string trasportgid, string transportclassgid, string Distance, string Rate, string FCCode,
        string CCCode, string OUCode, string ProductCode, string Amount, string Desc, string IsRemoved, string UId, string hsngid,string rcm)
        {
            ProcedureName = "PR_FS_Set_TravelExpense";

            AddParameter("ecftravelgid", ecftravelgid);
            AddParameter("ecfId", ecfId);
            AddParameter("InvId", invId);
            AddParameter("expnaturegid", expnaturegid);
            AddParameter("expCatId", expCatId);
            AddParameter("expSubcatId", expSubcatId);
            AddParameter("placefrom", placefrom);
            AddParameter("placeto", placeto);
            AddParameter("claimperiodfrm", claimperiodfrm);
            AddParameter("claimperiodto", claimperiodto);
            AddParameter("trasportgid", trasportgid);
            AddParameter("transportclassgid", transportclassgid);
            AddParameter("Distance", Distance);
            AddParameter("Rate", Rate);
            AddParameter("FCCode", FCCode);
            AddParameter("CCCode", CCCode);
            AddParameter("OUCode", OUCode);
            AddParameter("ProductCode", ProductCode);
            AddParameter("Amount", Amount);
            AddParameter("Desc", Desc);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            AddParameter("hsngid", hsngid);
            AddParameter("rcm", rcm);

            return ExecuteProcedure;
        }


        #endregion

        #region Non Travel Claim
        public DataSet GetNonTravelClaim(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_NonTravelClaim";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetNTPaymentDetails(string DebitlineGId, string ECFId, string invid, string expnaturegid, string expCatId, string expSubcatId,
          string ClaimFrm, string ClaimTo, string CCCode, string FCCode, string OUCode, string ProductCode, string Amount, string Desc, string IsRemoved, string UId, string hsngid,string rcm)
        {
            ProcedureName = "PR_FS_Set_NonTravelClaim";

            AddParameter("DebitlineGId", DebitlineGId);
            AddParameter("ECFId", ECFId);
            AddParameter("invid", invid);
            AddParameter("expnaturegid", expnaturegid);
            AddParameter("expCatId", expCatId);
            AddParameter("expSubcatId", expSubcatId);
            AddParameter("ClaimFrm", ClaimFrm);
            AddParameter("ClaimTo", ClaimTo);
            AddParameter("CCCode", CCCode);
            AddParameter("FCCode", FCCode);
            AddParameter("OUCode", OUCode);
            AddParameter("ProductCode", ProductCode);
            AddParameter("Amount", Amount);
            AddParameter("Desc", Desc);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            AddParameter("hsngid", hsngid);
            AddParameter("rcm", rcm);

            return ExecuteProcedure;
        }

        public DataSet RunDedupEmployee(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Set_MakerDedupECWOLC";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Petty Cash
        public DataSet GetEmployeePettyCash(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_PettyCashClaim";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Advance Request
        public DataSet GetAdvanceRequest(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_AdvanceRequest";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetAdvanceRequestDebit(string ecfartgid, string ECFId, string advancetypegid, string promoinvoice, string pono, string cbfno, string CCCode,
           string FCCode, string OUCode, string ProductCode, string Amount, string liQdate, string desc, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_AdvanceRequest";

            AddParameter("ecfartgid", ecfartgid);
            AddParameter("ECFId", ECFId);
            AddParameter("advancetypegid", advancetypegid);
            AddParameter("promoinvoice", promoinvoice);
            AddParameter("pono", pono);
            AddParameter("cbfno", cbfno);
            AddParameter("CCCode", CCCode);
            AddParameter("FCCode", FCCode);
            AddParameter("OUCode", OUCode);
            AddParameter("ProductCode ", ProductCode);
            AddParameter("Amount", Amount);
            AddParameter("liQdate", liQdate);
            AddParameter("desc", desc);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetWithHoldingTaxARF(string withholdtaxgid, string ecfid, string TaxId, string taxsubtypegid, string TaxRate, string TaxableAmt,
           string TaxAmount, string NetAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_WithHoldTaxARF"; // correction by Kavitha 30-06-2017

            //ProcedureName = "PR_FS_Set_WithHoldTax";
            AddParameter("withholdtaxgid", withholdtaxgid);
            AddParameter("ecfid", ecfid);
            AddParameter("TaxId", TaxId);
            AddParameter("taxsubtypegid", taxsubtypegid);
            AddParameter("TaxRate", TaxRate);
            AddParameter("TaxableAmt", TaxableAmt);
            AddParameter("TaxAmount", TaxAmount);
            AddParameter("NetAmount", NetAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet ECFRunDedupARF(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Set_MakerDedupARF";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        #endregion

        #region Supplier Invoice - DSA
        public DataSet GetSupplierInvoiceDSA(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Get_DSASuppliers";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetDSAInvoiceDetails(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_DSASuppliersExpense";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetWithHoldingTaxDSA(string withholdtaxgid, string ecfid, string TaxId, string taxsubtypegid, string TaxRate, string TaxableAmt,
           string TaxAmount, string NetAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_WithHoldTaxDSA";

            AddParameter("withholdtaxgid", withholdtaxgid);
            AddParameter("ecfid", ecfid);
            AddParameter("TaxId", TaxId);
            AddParameter("taxsubtypegid", taxsubtypegid);
            AddParameter("TaxRate", TaxRate);
            AddParameter("TaxableAmt", TaxableAmt);
            AddParameter("TaxAmount", TaxAmount);
            AddParameter("NetAmount", NetAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet ExcelDSAWHTUpload(string ecfid, DataTable Withholdtax, string UId)
        {
            ProcedureName = "PR_FS_SET_WithholdtaxExcel";

            AddParameter("ecfid", ecfid);
            AddParameter("Withholdtax", Withholdtax);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetExcelDSADebitlineUpload(string ecfgid, DataTable SupplierInvoice, string UId)
        {
            ProcedureName = "PR_FS_Set_SupplierInvoiceDSAExcel";

            AddParameter("ecfgid", ecfgid);
            AddParameter("SupplierInvoice", SupplierInvoice);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetDSAReset(string ecfgid, string UId)
        {
            ProcedureName = "PR_FS_Set_DSAReset";

            AddParameter("ecfgid", ecfgid);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet ECFRunDedupDSA(string EcfId, string UId)
        {
            ProcedureName = "PR_FS_Set_MakerDedupDSA";

            AddParameter("EcfId", EcfId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        #endregion

        #region Address Label Printing
        public DataSet GetAddressLabelPrint(string PVDateFrom, string PVDateTo, string PVNo, string PVAmount, string EmpCodeId, string EmpNameId, string SuppCodeId,
            string SuppNameId, string PayMode, string ClaimType, string BatchNo, string Location, string UId)
        {
            ProcedureName = "PR_FS_Get_AddressLabelPrint";

            AddParameter("PVDateFrom", PVDateFrom);
            AddParameter("PVDateTo", PVDateTo);
            AddParameter("PVNo", PVNo);
            AddParameter("PVAmount", PVAmount);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("PayMode", PayMode);
            AddParameter("ClaimType", ClaimType);
            AddParameter("BatchNo", BatchNo);
            AddParameter("Location", Location);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }


        public DataSet PrintAddressLabelPrint(string PvIds, string Status, string UId)
        {
            ProcedureName = "PR_FS_Set_AddressLabelPrint";

            AddParameter("PvIds", PvIds);
            AddParameter("Status", Status);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Credit Note Maker
        public DataSet GetCreditNoteMaker(string DateFrom, string DateTo, string SupplierId, string Rejected, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_CreditNoteMaker";

            AddParameter("Datefrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("SupplierId", SupplierId);
            AddParameter("Rejected", Rejected);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetInvoiceAmountDetails(string ECFNo, string InvNo, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_CRNAutoComplete";

            AddParameter("ECFNo", ECFNo);
            AddParameter("InvNo", InvNo);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }


        public DataSet SetCreditNoteMaker(string Creditnotegid, string supplierId, string ecfno, string invno, string creditnoteno,
                            string creditnoteamt, string remarks, string isremoved, string UId)
        {
            ProcedureName = "PR_FS_Set_creditnoteMaker";

            AddParameter("Creditnotegid", Creditnotegid);
            AddParameter("supplierId", supplierId);
            AddParameter("ECFNO", ecfno);
            AddParameter("invno", invno);
            AddParameter("creditnoteno", creditnoteno);
            AddParameter("creditnoteamt", creditnoteamt);
            AddParameter("remarks", remarks);
            AddParameter("isremoved", isremoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Credit Note Checker
        public DataSet GetCreditNoteChecker(string DateFrom, string DateTo, string SupplierId, string UId)
        {
            ProcedureName = "PR_FS_Get_creditnoteChecker";

            AddParameter("Datefrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("SupplierId", SupplierId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetCreditNoteChecker(string Creditnotegid, string Status, string remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_creditnoteChecker";

            AddParameter("Creditnotegid", Creditnotegid);
            AddParameter("Status", Status);
            AddParameter("remarks", remarks);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Cheque Status Update
        public DataSet GetChequeStatusUpdate(string ChqNo, string PvNo, string UId)
        {
            ProcedureName = "pr_fs_get_Chequestatusupdate";
            AddParameter("ChqNo", ChqNo);
            AddParameter("PvNo", PvNo);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }


        public DataSet SetChequeStatusUpdate(string ChqNo, string PvNo, string ChqStatus, string Date, string Reason,
            string ViewType, string XmlData, string UId)
        {
            ProcedureName = "pr_fs_set_Chequestatusupdate";
            AddParameter("ChqNo", ChqNo);
            AddParameter("PvNo", PvNo);
            AddParameter("ChqStatus", ChqStatus);
            AddParameter("ChqDate", Date);
            AddParameter("Reason", Reason);
            AddParameter("ViewType", ViewType);
            AddParameter("XmlData", XmlData);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Payment Run
        public DataSet GetPrepaidGLMaker(string ARFFrom, string ARFTo, string ARFNo, string ARFAmt, string liqdatefrom,
               string liqdateto, string Type, string Paymode, string SCode, string SName,
               string Empcode, string Empname, string Raisercode, string Raisername, string Advancetype, string Advancegl, string rejected, string UId)
        {
            ProcedureName = "PR_FS_GET_ArfGLTransfer";

            AddParameter("ARFFrom", ARFFrom);
            AddParameter("ARFTo", ARFTo);
            AddParameter("ARFNo", ARFNo);
            AddParameter("ARFAmt", ARFAmt);
            AddParameter("liqdatefrom", liqdatefrom);
            AddParameter("liqdateto", liqdateto);
            AddParameter("Type", Type);
            AddParameter("Paymode", Paymode);
            AddParameter("SCode", SCode);
            AddParameter("SName", SName);
            AddParameter("Empcode", Empcode);
            AddParameter("Empname", Empname);
            AddParameter("Raisercode", Raisercode);
            AddParameter("Raisername", Raisername);
            AddParameter("Advancetype", Advancetype);
            AddParameter("Advancegl", Advancegl);
            AddParameter("rejected", rejected);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPrepaidGLMaker(string gltransfergid, string ecfid, string ecfarfid, string newadvancedate, string newadvencetype, string newadvancegl, string remarks, string isremoved, string UId)
        {
            ProcedureName = "PR_FS_SET_ArfGLTransfer";

            AddParameter("gltransfergid", gltransfergid);
            AddParameter("ecfid", ecfid);
            AddParameter("ecfarfid", ecfarfid);
            AddParameter("newadvancedate", newadvancedate);
            AddParameter("newadvencetype", newadvencetype);
            AddParameter("newadvancegl", newadvancegl);
            AddParameter("remarks", remarks);
            AddParameter("isremoved", isremoved);
            AddParameter("uid", UId);

            return ExecuteProcedure;
        }

        public DataSet GetPrepaidGLChecker(string RequestFrom, string RequestTo, string RequestCode, string RequestName, string Paymode,
            string SCode, string SName, string Empcode, string Empname, string Type, string ARFNo, string UId)
        {
            ProcedureName = "PR_FS_GET_ArfGLTransferChecker";

            AddParameter("RequestFrom", RequestFrom);
            AddParameter("RequestTo", RequestTo);
            AddParameter("RequestName", RequestName);
            AddParameter("RequestCode", RequestCode);
            AddParameter("Paymode", Paymode);
            AddParameter("SCode", SCode);
            AddParameter("SName", SName);
            AddParameter("Empcode", Empcode);
            AddParameter("Empname", Empname);
            AddParameter("Type", Type);
            AddParameter("ARFNo", ARFNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPrepaidGLChecker(string gltransfergid, string Status, string remarks, string UId)
        {
            ProcedureName = "PR_FS_SET_ArfGLTransferChecker";

            AddParameter("gltransfergid", gltransfergid);
            AddParameter("Status", Status);
            AddParameter("remarks", remarks);
            AddParameter("uid", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region EFT
        public DataSet GetEFTMemoDetails(string MemoNo, string PvNo, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_MemoStatusUpdate";
            AddParameter("MemoNo", MemoNo);
            AddParameter("PvNo", PvNo);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SetEFTMemoDetails(string PvIds, string Status, string CancelPvId, string Reason, string LoginUserId)
        {
            ProcedureName = "PR_FS_SET_OnlineMemoFileGeneration";
            AddParameter("PvIds", PvIds);
            AddParameter("Status", Status);
            AddParameter("CancelPvId", CancelPvId);
            AddParameter("Reason", Reason);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SaveEFTDetails(string memoNo, string pvNo, string status, string date, string remarks, string viewType, string Xml, string loginUserId, string sheets)
        {
            ProcedureName = "PR_FS_Set_MemoStatusUpdate";
            AddParameter("MemoNo", memoNo);
            AddParameter("PVNo", pvNo);
            AddParameter("Status", status);
            AddParameter("Date", date);
            AddParameter("Reason", remarks);
            AddParameter("ViewType", viewType);
            AddParameter("XmlData", Xml);
            AddParameter("UId", loginUserId);
            AddParameter("sheets", sheets);
            return ExecuteProcedure;
        }

        public DataSet GetEFTMemoDetails(string PvDateFrom, string PvDateTo, string PvNo, string PvAmountFrom, string PvAmountTo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId,
 string ClaimType, string PayMode, string BankId, string BatchNo, string MemoNo, string UId)
        {
            ProcedureName = "PR_FS_GET_MemoFileGeneration";

            AddParameter("PvDateFrom", PvDateFrom);
            AddParameter("PvDateTo", PvDateTo);
            AddParameter("PvNo", PvNo);
            AddParameter("PvAmountFrom", PvAmountFrom);
            AddParameter("PvAmountTo", PvAmountTo);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("ClaimType", ClaimType);
            AddParameter("PayMode", PayMode);
            AddParameter("BankId", BankId);
            AddParameter("BatchNo", BatchNo);
            AddParameter("MemoNo", MemoNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet PrintEFTMemoDetails(string PvIds, string Paymode, string ViewType, string PayBankGId, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_OnlineMemoFilePrint";
            AddParameter("PvIds", PvIds);
            AddParameter("Paymode", Paymode);
            AddParameter("ViewType", ViewType);
            AddParameter("PayBankGId", PayBankGId);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        //Rejection Mails
        public DataSet GetEFTBulkRejectionMails(string BounceDate, string PayMode, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_OnlineMemoRejectionBulkMail";
            AddParameter("BounceDate", BounceDate);
            AddParameter("PayMode", PayMode);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet GetSingleRejectionDetails(string BounceDateFrom, string BounceDateTo, string PayMode, string Amount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_OnlineMemoRejectionSingleMail";

            AddParameter("BounceDateFrom", BounceDateFrom);
            AddParameter("BounceDateTo", BounceDateTo);
            AddParameter("PayMode", PayMode);
            AddParameter("Amount", Amount);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }
        #endregion

        #region Amort
        public DataSet GetAmortRunSearchDetails(string ECFFrom, string ECFTo, string ECFNumber, string ECFAmount, string AmortAmount, string SupplierCodeId, string SupplierNameId, string Month, string Year, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_AmortRun";
            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNumber);
            AddParameter("ECFAmt", ECFAmount);
            AddParameter("AmortAmt", AmortAmount);
            AddParameter("SCode", SupplierCodeId);
            AddParameter("SName", SupplierNameId);
            AddParameter("Month", Month);
            AddParameter("Year", Year);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SetAmortRunDetails(string amortData, string Month, string Year, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_AmortRun";
            AddParameter("Amort", amortData);
            AddParameter("Month", Month);
            AddParameter("Year", Year);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet GetAmortDetails(string ECFFrom, string ECFTo, string ECFNo, string ECFAmount, string SupplierCodeId, string SupplierNameId, string UserId)
        {
            ProcedureName = "PR_FS_Get_AmortEdit";
            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("ECFAmt", ECFAmount);
            AddParameter("SCode", SupplierCodeId);
            AddParameter("SName", SupplierNameId);
            AddParameter("UId", UserId);
            return ExecuteProcedure;

        }

        public DataSet GetAmortReScheduleDetails(string InvId, string Amount, string AmortCycle, string AmortStartDate, string AmortEndDate, string AmortSplit, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_AmortScheduleView";
            AddParameter("invid", InvId);
            AddParameter("amount", Amount);
            AddParameter("startdate", AmortStartDate);
            AddParameter("enddate", AmortEndDate);
            AddParameter("frequencygid", AmortCycle);
            AddParameter("Split", AmortSplit);
            AddParameter("uid", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SaveAmortDetailsBasedOnReSchedule(string AmortId, string ECFId, string InvId, string Amount, string CurrentAmount, string AmortCycle, string AmortStartDate, string AmortEndDate, string AmortSplit, string SupplierId, DataTable DebitlinePercent, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_AmortEdit";
            AddParameter("amortgid", AmortId);
            AddParameter("ecfid", ECFId);
            AddParameter("invid", InvId);
            AddParameter("supplierid", SupplierId);
            AddParameter("CurrentUpFrontAmt", CurrentAmount);
            AddParameter("amount", Amount);
            AddParameter("startdate", AmortStartDate);
            AddParameter("enddate", AmortEndDate);
            AddParameter("frequencygid", AmortCycle);
            AddParameter("split", AmortSplit);
            AddParameter("DebitlinePercent", DebitlinePercent);
            AddParameter("uid", LoginUserId);

            return ExecuteProcedure;
        }

        public DataSet GetAmortSplitDetails(string startdate, string enddate, string frequencygid, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_SPlit";
            AddParameter("startdate", startdate);
            AddParameter("enddate", enddate);
            AddParameter("frequencygid", frequencygid);
            AddParameter("uid", LoginUserId);
            return ExecuteProcedure;
        }
        #endregion

        #region Paymode Coversion Maker
        public DataSet GetPaymodeConversionMaker(string PayDateFrom, string PayDateTo, string PVNo, string PVAmount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string BounceDateFrom, string BounceDateTo, string ChqNo, string MemoNo, string PayMode, string ViewType, string UId)
        {
            ProcedureName = "pr_fs_Get_PaymodeConversionmaker";
            AddParameter("PayDateFrom", PayDateFrom);
            AddParameter("PayDateTo", PayDateTo);
            AddParameter("PVNo", PVNo);
            AddParameter("PVAmount", PVAmount);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("BounceDateFrom", BounceDateFrom);
            AddParameter("BounceDateTo", BounceDateTo);
            AddParameter("ChqNo", ChqNo);
            AddParameter("MemoNo", MemoNo);
            AddParameter("PayMode", PayMode);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetPaymodeConversionMaker(string PVId, string PayMode, string Beneficiary, string AccNo, string IfscCode, string BankId, string Desc, string Remarks, string UId)
        {
            ProcedureName = "pr_fs_Set_PaymodeConversionmaker";
            AddParameter("PVId", PVId);
            AddParameter("PayMode", PayMode);
            AddParameter("Beneficiary", Beneficiary);
            AddParameter("AccNo", AccNo);
            AddParameter("IfscCode", IfscCode);
            AddParameter("BankId", BankId);
            AddParameter("Desc", Desc);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Paymode Coversion Checker
        public DataSet GetPaymodeConversionChecker(string PVNo, string ReqDateFrom, string ReqDateTo, string ReqCodeId,
            string ReqNameId, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string PayMode,
             string ViewType, string UId)
        {
            ProcedureName = "pr_fs_Get_PaymodeConversionchecker";
            AddParameter("PVNo", PVNo);
            AddParameter("ReqDateFrom", ReqDateFrom);
            AddParameter("ReqDateTo", ReqDateTo);
            AddParameter("ReqCodeId", ReqCodeId);
            AddParameter("ReqNameId", ReqNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("PayMode", PayMode);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetPaymodeConversionChecker(string PaymentReIssueGId, string Status, string Remarks, string UId, string chqno)
        {
            ProcedureName = "pr_fs_Set_PaymodeConversionchecker";
            AddParameter("PaymentReIssueGId", PaymentReIssueGId);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("chqno", chqno);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region GL Uploads
        public DataSet SetGLUploads(string UploadFrom, string UploadTo, string UploadTypeId, string BatchNo, string Status, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_GLUpload";

            AddParameter("UploadFrom", UploadFrom);
            AddParameter("UploadTo", UploadTo);
            AddParameter("UploadTypeId", UploadTypeId);
            AddParameter("BatchNo", BatchNo);
            AddParameter("Status", Status);
            AddParameter("UId", LoginUserId);

            return ExecuteProcedure;
        }

        public DataSet GetGLUploadsCancellation(string UploadDate, string UploadTypeId, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_GLUpload";

            AddParameter("UploadDate", UploadDate);
            AddParameter("UploadTypeId", UploadTypeId);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Payment Reversals
        //Maker
        public DataSet GetPaymentReversals(string PVDateFrom, string PVDateTo, string PVNo, string Status, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_PaymentReversalMaker";

            AddParameter("PVDateFrom", PVDateFrom);
            AddParameter("PVDateTo", PVDateTo);
            AddParameter("PVNo", PVNo);
            AddParameter("Status", Status);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPaymentReversals(string PvId, string ReversalDate, string Reason, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentReversalMaker";

            AddParameter("PvId", PvId);
            AddParameter("ReversalDate", ReversalDate);
            AddParameter("Reason", Reason);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        //Checker
        public DataSet GetCheckerPaymentReversals(string PVDateFrom, string PVDateTo, string PVNo, string UId)
        {
            ProcedureName = "PR_FS_Get_PaymentReversalChecker";

            AddParameter("PVDateFrom", PVDateFrom);
            AddParameter("PVDateTo", PVDateTo);
            AddParameter("PVNo", PVNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetCheckerPaymentReversals(string ReversalGId, string Status, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_PaymentReversalChecker";

            AddParameter("ReversalGId", ReversalGId);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Payment Dump
        public DataSet GetPaymentDump(string BatchNo, string PaymentDate, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_PaymentDump";

            AddParameter("BatchNo", BatchNo);
            AddParameter("PaymentDate", PaymentDate);
            AddParameter("UId", LoginUserId);

            return ExecuteProcedure;
        }
        #endregion

        #region Petty Cash Alert
        public DataSet GetPettyCashAlert(string ARFDateFrom, string ARFDateTo, string ARFNo, string ARFAmount, string EmpCodeId, string EmpNameId, string BranchCodeId, string BranchNameId,
            string RaiserCodeId, string RaiserNameId, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_PettyCashMailAlert";

            AddParameter("ARFDateFrom", ARFDateFrom);
            AddParameter("ARFDateTo", ARFDateTo);
            AddParameter("ARFNo", ARFNo);
            AddParameter("ARFAmount", ARFAmount);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("BranchCodeId", BranchCodeId);
            AddParameter("BranchNameId", BranchNameId);
            AddParameter("RaiserCodeId", RaiserCodeId);
            AddParameter("RaiserNameId", RaiserNameId);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }
        #endregion

        #region StaleCheque
        //stale cheque Entry
        public DataSet GetStaleChequeEntry(string ChqDateFrom, string ChqDateTo, string ChqNo, string Amount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId,
            string PvDate, string PvNo, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_StaleChequeEntry";

            AddParameter("ChqDateFrom", ChqDateFrom);
            AddParameter("ChqDateTo", ChqDateTo);
            AddParameter("ChqNo", ChqNo);
            AddParameter("Amount", Amount);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("PvDate", PvDate);
            AddParameter("PvNo", PvNo);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet GetPVDetailsWIthECF(string PvId)
        {
            ProcedureName = "PR_FS_Get_PVDetailWithECF";

            AddParameter("PvId", PvId);
            //AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SetStaleChequeEntry(string PvIds, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_StaleChequeEntry";

            AddParameter("PvIds", PvIds);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        //stale cheque Maker
        public DataSet GetStaleChequeMaker(string ChqDateFrom, string ChqDateTo, string ChqNo, string DocAmount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId,
            string DocTypeId, string DocNo, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_StaleChequeReissueMaker";

            AddParameter("ChqDateFrom", ChqDateFrom);
            AddParameter("ChqDateTo", ChqDateTo);
            AddParameter("ChqNo", ChqNo);
            AddParameter("DocAmount", DocAmount);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("DocTypeId", DocTypeId);
            AddParameter("DocNo", DocNo);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SetStaleChequeMaker(string PvId, string BankId, string ChqBookNo, string ChqNo, string ChqDate, string Remarks, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_StaleChequeReissueMaker";

            AddParameter("PvNo", PvId);
            AddParameter("BankNames", BankId);
            AddParameter("AccountNo", ChqBookNo);
            AddParameter("benificiary", ChqNo);
            AddParameter("Paymodes", ChqDate);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        //stale cheque Checker
        public DataSet GetStaleChequeChecker(string ChqDateFrom, string ChqDateTo, string ChqNo, string DocAmount, string SuppCodeId, string SuppNameId, string EmpCodeId, string EmpNameId,
            string DocNo, string LoginUserId)
        {
            ProcedureName = "PR_FS_Get_StaleChequeReissueChecker";

            AddParameter("ChqDateFrom", ChqDateFrom);
            AddParameter("ChqDateTo", ChqDateTo);
            AddParameter("ChqNo", ChqNo);
            AddParameter("DocAmount", DocAmount);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("DocNo", DocNo);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }

        public DataSet SetStaleChequeChecker(string PvId, string Status, string Attachment, string Remarks, string LoginUserId)
        {
            ProcedureName = "PR_FS_Set_StaleChequeReissueChecker";

            AddParameter("PvId", PvId);
            AddParameter("Status", Status);
            AddParameter("Attachment", Attachment);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", LoginUserId);
            return ExecuteProcedure;
        }
        #endregion

        #region Receipt Entry
        public DataSet GetReceiptEntry(string ReceiptDateFrom, string ReceiptDateTo, string ReceiptNo, string ReceiptFrom, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string UId)
        {
            ProcedureName = "PR_FS_Get_ReceiptEntryMaker";

            AddParameter("ReceiptDateFrom", ReceiptDateFrom);
            AddParameter("ReceiptDateTo", ReceiptDateTo);
            AddParameter("ReceiptNo", ReceiptNo);
            AddParameter("ReceiptFrom", ReceiptFrom);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetReceiptEntry(string ReceiptId, string ReceiptDate, string ReceiptFrom, string Source, string Raiser, string EmpSuppNameId,
                                        string ReceiptMode, string Amount, string PayRefNo, string TranDate, string BankId, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_ReceiptEntryMaker";

            AddParameter("ReceiptId", ReceiptId);
            AddParameter("ReceiptDate", ReceiptDate);
            AddParameter("ReceiptFrom", ReceiptFrom);
            AddParameter("Source", Source);
            AddParameter("Raiser", Raiser);
            AddParameter("EmpSuppNameId", EmpSuppNameId);
            AddParameter("ReceiptMode", ReceiptMode);
            AddParameter("Amount", Amount);
            AddParameter("PayRefNo", PayRefNo);
            AddParameter("TranDate", TranDate);
            AddParameter("BankId", BankId);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetGLUploads(string ReceiptGLId, string ReceiptId, string CRFrom, string DocNo, string CRGlNo, string Description, string Amount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FS_Set_ReceiptEntryMakerCrGl";

            AddParameter("ReceiptGLId", ReceiptGLId);
            AddParameter("ReceiptId", ReceiptId);
            AddParameter("CRFrom", CRFrom);
            AddParameter("DocNo", DocNo);
            AddParameter("CRGlNo", CRGlNo);
            AddParameter("Description", Description);
            AddParameter("Amount", Amount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetReceiptChecker(string ReceiptDateFrom, string ReceiptDateTo, string ReceiptNo, string ReceiptFrom, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId,
            string PayDateFrom, string PayDateTo, string BankId, string ReceiptMode, string PayMode, string Amount, string PayRefNo, string UId)
        {
            ProcedureName = "PR_FS_Get_ReceiptEntryChecker";

            AddParameter("ReceiptDateFrom", ReceiptDateFrom);
            AddParameter("ReceiptDateTo", ReceiptDateTo);
            AddParameter("ReceiptNo", ReceiptNo);
            AddParameter("ReceiptFrom", ReceiptFrom);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);

            AddParameter("PayDateFrom", PayDateFrom);
            AddParameter("PayDateTo", PayDateTo);
            AddParameter("BankId", BankId);
            AddParameter("ReceiptMode", ReceiptMode);
            AddParameter("PayMode", PayMode);
            AddParameter("Amount", Amount);
            AddParameter("PayRefNo", PayRefNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet UpdateReceiptStatus(string ReceiptId, string Status, string Remarks, string UId)
        {
            ProcedureName = "PR_FS_Set_ReceiptEntryChecker";

            AddParameter("ReceiptId", ReceiptId);
            AddParameter("Status", Status);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Productivity Tracking
        public DataSet GetProductivityTrackingDW(string DateFrom, string DateTo, string Activity, string UId)
        {
            ProcedureName = "PR_FS_Get_ProductivityTrackingDate";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("Activity", Activity);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetProductivityTrackingUser(string DateFrom, string DateTo, string EmpNameId, string Activity, string UId)
        {
            ProcedureName = "PR_FS_Get_ProductivityTrackingUser";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("Activity", Activity);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetPaymentAuditTrail(string PVNo, string ECFNo, string InvoiceNo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId, string BankId, string ChqNo, string UId)
        {
            ProcedureName = "PR_FS_GET_PaymentAuditTrail";

            AddParameter("PVNo", PVNo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("InvoiceNo", InvoiceNo);
            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);
            AddParameter("BankId", BankId);
            AddParameter("ChqNo", ChqNo);

            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetSearchQuery(string ECFNo, string ECFDate, string DocTypeId, string RaiserId, string AuthrId, string DocStatus, string SuppCodeId, string SuppNameId,
            string InvoiceNo, string InvAmount, string ECFAmount, string PVNo, string PVDate, string InwardAWBNo, string ChqAWBNo, string EmpId, string CHQNo, string CHQAmount, string PhyReceptDateFrom,
            string PhyReceptDateTo, string ECFId, string ViewType, string UId,string pono,string cbfno)
        {
            ProcedureName = "PR_FS_GET_SearchReport";

            AddParameter("ECFNo", ECFNo);
            AddParameter("ECFDate", ECFDate);
            AddParameter("DocTypeId", DocTypeId);
            AddParameter("RaiserId", RaiserId);
            AddParameter("AuthrId", AuthrId);
            AddParameter("DocStatus", DocStatus);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);

            AddParameter("InvoiceNo", InvoiceNo);
            AddParameter("InvAmount", InvAmount);
            AddParameter("ECFAmount", ECFAmount);
            AddParameter("PVNo", PVNo);

            AddParameter("PVDate", PVDate);
            AddParameter("InwardAWBNo", InwardAWBNo);
            AddParameter("ChqAWBNo", ChqAWBNo);

            AddParameter("EmpId", EmpId);
            AddParameter("CHQNo", CHQNo);
            AddParameter("CHQAmount", CHQAmount);

            AddParameter("PhyReceptDateFrom", PhyReceptDateFrom);
            AddParameter("PhyReceptDateTo", PhyReceptDateTo);
            AddParameter("ECFId", ECFId);
            AddParameter("ViewType", ViewType);

            AddParameter("UId", UId);
            AddParameter("pono", pono);
            AddParameter("cbfno", cbfno);
            return ExecuteProcedure;
	
        }
        #endregion

        #region Provision Mapping
        public DataSet SetProvisionUpload(string Filename, string Sheetname, DataTable Provision, string UId)
        {
            ProcedureName = "PR_FS_SET_ProvisionExcel";

            AddParameter("Filename", Filename);
            AddParameter("Sheetname", Sheetname);
            AddParameter("Provision", Provision);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet FetchProvisionMapping(string PDateFrom, string PDateTo, string PAmount, string CCCode,
            string FCCode, string FC, string CC, string ProvisionBy, string UId)
        {
            ProcedureName = "PR_FS_Get_Provision";

            AddParameter("PDateFrom", PDateFrom);
            AddParameter("PDateTo", PDateTo);
            AddParameter("PAmount", PAmount);
            AddParameter("CCCode", CCCode);
            AddParameter("FCCode", FCCode);
            AddParameter("FC", FC);
            AddParameter("CC", CC);
            AddParameter("ProvisionBy", ProvisionBy);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetProvisionMapping(string InvFrom, string InvTo, string InvNo, string InvAmt,
            string ECFFrom, string ECFTo, string ECFNo, string ECFAmt, string SCode, string SName, string UId)
        {
            ProcedureName = "PR_FS_GET_ProvisionMapping";

            AddParameter("InvFrom", InvFrom);
            AddParameter("InvTo", InvTo);
            AddParameter("InvNo", InvNo);
            AddParameter("InvAmt", InvAmt);
            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("ECFAmt", ECFAmt);
            AddParameter("SCode", SCode);
            AddParameter("SName", SName);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SearchProvisionDetails(string Month, string Year, string Amount, string FC, string CC, string UId)
        {
            ProcedureName = "PR_FS_Get_ProvisionDetails";

            AddParameter("Month", Month);
            AddParameter("Year", Year);
            AddParameter("Amount", Amount);
            AddParameter("FC", FC);
            AddParameter("CC", CC);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetProvisionMapping(string provisiongid, string Invoicegid, string MAmt, string MDesc, string UId)
        {
            ProcedureName = "PR_FS_SET_ProvisionMapping";

            AddParameter("provisiongid", provisiongid);
            AddParameter("Invoicegid", Invoicegid);
            AddParameter("MAmt", MAmt);
            AddParameter("MDesc", MDesc);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetProvisionUnMapping(string InvFrom, string InvTo, string InvNo, string InvAmt,
            string ECFFrom, string ECFTo, string ECFNo, string ECFAmt, string SCode, string SName, string UId)
        {
            ProcedureName = "PR_FS_GET_ProvisionUnMapping";

            AddParameter("InvFrom", InvFrom);
            AddParameter("InvTo", InvTo);
            AddParameter("InvNo", InvNo);
            AddParameter("InvAmt", InvAmt);
            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("ECFAmt", ECFAmt);
            AddParameter("SCode", SCode);
            AddParameter("SName", SName);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetProvisionUnMapping(string provisionmapgid, string Invoicegid, string MDesc, string UId)
        {
            ProcedureName = "PR_FS_SET_ProvisionUnMapping";

            AddParameter("provisionmapgid", provisionmapgid);
            AddParameter("Invoicegid", Invoicegid);
            AddParameter("MDesc", MDesc);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Status of Shop
        public DataSet GetShopStatus(string DateFrom, string DateTo, string UId)
        {
            ProcedureName = "PR_FS_GET_StatusOfShop";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetShopStatusSummary(string DateFrom, string DateTo, string Activity, string UId)
        {
            ProcedureName = "PR_FS_GET_StatusOfShopSummary";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("Activity", Activity);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetSOSReport(string DateFrom, string DateTo, string UId)
        {
            ProcedureName = "GetSOSReport";

            AddParameter("PhyReceptDateFrom", DateFrom);
            AddParameter("PhyReceptDateTo", DateTo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Prepaid Extension
        public DataSet GetPrepaidExtension(string ARFDateFrom, string ARFDateTo, string ARFNo, string ARFAmount, string LiquidDateFrom, string LiquidDateTo, string Type, string PayMode, string EmpCodeId,
        string EmpNameId, string SuppCodeId, string SuppNameId, string RaiserCodeId, string RaiserNameId, string UId)
        {
            ProcedureName = "PR_FS_Get_PrepaidExtension";

            AddParameter("ARFDateFrom", ARFDateFrom);
            AddParameter("ARFDateTo", ARFDateTo);
            AddParameter("ARFNo", ARFNo);
            AddParameter("ARFAmount", ARFAmount);

            AddParameter("LiquidDateFrom", LiquidDateFrom);
            AddParameter("LiquidDateTo", LiquidDateTo);
            AddParameter("Type", Type);
            AddParameter("PayMode", PayMode);

            AddParameter("EmpCodeId", EmpCodeId);
            AddParameter("EmpNameId", EmpNameId);
            AddParameter("SuppCodeId", SuppCodeId);
            AddParameter("SuppNameId", SuppNameId);

            AddParameter("RaiserCodeId", RaiserCodeId);
            AddParameter("RaiserNameId", RaiserNameId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetPrepaidExtension(string ECFARFGId, string OldLiquidationDate, string NewLiquidationDate, string Remarks, string Attachment, string UId)
        {
            ProcedureName = "PR_FS_Set_PrepaidExtension";

            AddParameter("ECFARFGId", ECFARFGId);
            AddParameter("OldLiquidationDate", OldLiquidationDate);
            AddParameter("NewLiquidationDate", NewLiquidationDate);
            AddParameter("Remarks", Remarks);
            AddParameter("Attachment", Attachment);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region CBF

        public DataSet SetCBFHeader(string CBFHeaderGId, string CBFDate, string CBFEndDate, string PARPRHeaderGId, string DeviationAmount, string ProjectOwnerId, string BranchId, string ReqBy,
            string CBFMode, string IsBudgeted, string CBFApproval, string BranchType, string BudgetOwnerId, string Description, string Justification, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_CBFHeader";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("CBFDate", CBFDate);
            AddParameter("CBFEndDate", CBFEndDate);
            AddParameter("PARPRHeaderGId", PARPRHeaderGId);
            AddParameter("DeviationAmount", DeviationAmount);
            AddParameter("ProjectOwnerId", ProjectOwnerId);
            AddParameter("BranchId", BranchId);
            AddParameter("ReqBy", ReqBy);
            AddParameter("CBFMode", CBFMode);
            AddParameter("IsBudgeted", IsBudgeted);
            AddParameter("CBFApproval", CBFApproval);
            AddParameter("BranchType", BranchType);
            AddParameter("BudgetOwnerId", BudgetOwnerId);
            AddParameter("Description", Description);
            AddParameter("Justification", Justification);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetCBFDetails(string CBFDetailGId, string CBFHeaderGId, string PARPRDetailGId, string PARPRDesc, string ProductGId, string ProductGroupGId, string Remarks, string UOMGId,
            string Qty, string UnitPrice, string TotalAmount, string ChartOfAcc, string FccCode, string BudgetLine, string IsRemoved, string IsContigency, string UId)
        {
            ProcedureName = "PR_FB_Set_CBFDetails";

            AddParameter("CBFDetailGId", CBFDetailGId);
            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("PARPRDetailGId", PARPRDetailGId);
            AddParameter("PARPRDesc", PARPRDesc);
            AddParameter("ProductGId", ProductGId);
            AddParameter("ProductGroupGId", ProductGroupGId);
            AddParameter("Remarks", Remarks);
            AddParameter("UOMGId", UOMGId);
            AddParameter("Qty", Qty);
            AddParameter("UnitPrice", UnitPrice);
            AddParameter("TotalAmount", TotalAmount);
            AddParameter("ChartOfAcc", ChartOfAcc);
            AddParameter("FccCode", FccCode);
            AddParameter("BudgetLine", BudgetLine);
            AddParameter("IsRemoved", IsRemoved);
            //AddParameter("IsContigency", IsContigency);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        //public DataSet SubmitCBF(string CBFHeaderGId, string Type, string IsReject, string Remarks, string UId)
        //{
        //    ProcedureName = "PR_FB_Set_CBF";

        //    AddParameter("CBFHeaderGId", CBFHeaderGId);
        //    AddParameter("Type", Type);
        //    AddParameter("IsReject", IsReject);
        //    AddParameter("Remarks", Remarks);
        //    AddParameter("UId", UId);
        //    return ExecuteProcedure;
        //}

        public DataSet GetCBFHeader(string CBFHeaderGId, string UId)
        {
            ProcedureName = "PR_FB_Get_CBFHeader";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPARHeader(string CBFHeaderGId, string IsBudgeted, string RequestForGId, string UId)
        {
            ProcedureName = "PR_FB_Get_CBFPARHeader";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("IsBudgeted", IsBudgeted);
            AddParameter("RequestForGId", RequestForGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPARHeaderSearch(string IsBudgeted, string PARDate, string PARNo, string PARAmount, string RequestForGId, string UId)
        {
            ProcedureName = "PR_FB_Get_PARHeader";

            AddParameter("IsBudgeted", IsBudgeted);
            AddParameter("PARDate", PARDate);
            AddParameter("PARNo", PARNo);
            AddParameter("PARAmount", PARAmount);
            AddParameter("RequestForGId", RequestForGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPARDetails(string CBFHeaderGId, string PARHeaderGId, string RequestForGId, string IsBudgeted, string UId)
        {
            ProcedureName = "PR_FB_Get_PARDetails";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("PARHeaderGId", PARHeaderGId);
            AddParameter("RequestForGId", RequestForGId);
            AddParameter("IsBudgeted", IsBudgeted);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPRHeader(string CBFHeaderGId, string RequestForGId, string UId)
        {
            ProcedureName = "PR_FB_Get_CBFPRHeader";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("RequestForGId", RequestForGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPRHeaderSearch(string RequestForGId, string PRDate, string PRNo, string UId)
        {
            ProcedureName = "PR_FB_Get_PRHeader";

            AddParameter("RequestForGId", RequestForGId);
            AddParameter("PRDate", PRDate);
            AddParameter("PRNo", PRNo);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetCBFPRDetails(string CBFHeaderGId, string PRHeaderGId, string UId)
        {
            ProcedureName = "PR_FB_Get_PRDetails";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("PRHeaderGId", PRHeaderGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetCBFPRDetails(string CBFHeaderGId, string PRDetailGId, string Qty, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_CBFPRDetails";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("PRDetailGId", PRDetailGId);
            AddParameter("Qty", Qty);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetRequestBy(string UId)
        {
            ProcedureName = "PR_FB_Get_CBFHeaderRequestBy";

            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetPARAttachment(string RefGId, string RefFlag, string AttachTypeGId)
        {
            ProcedureName = "PR_FB_Get_PARAttachment";

            AddParameter("RefGId", RefGId);
            AddParameter("RefFlag", RefFlag);
            AddParameter("AttachTypeGId", AttachTypeGId);
            return ExecuteProcedure;
        }

        public DataSet SetCBFAttachment(string RefGId, string AttachmentId, string AttachmentName, string Description, string RefFlag, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_CBFAttachment";

            AddParameter("RefGId", RefGId);
            AddParameter("AttachmentId", AttachmentId);
            AddParameter("AttachmentName", AttachmentName);
            AddParameter("Description", Description);
            AddParameter("RefFlag", RefFlag);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        #endregion

        #region PO
        public DataSet GetPOCBFHeader(string CBFNo, string CBFDateFrom, string CBFDateTo, string CBFReqFor, string UId)
        {
            ProcedureName = "PR_FB_Get_POCBFHeader";

            AddParameter("CBFNo", CBFNo);
            AddParameter("CBFDateFrom", CBFDateFrom);
            AddParameter("CBFDateTo", CBFDateTo);
            AddParameter("CBFReqFor", CBFReqFor);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetPOCBFDetails(string CBFHeaderGId)
        {
            ProcedureName = "PR_FB_Get_POCBFDetail";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            return ExecuteProcedure;
        }

        public DataSet SetPOCBF(string CBFDetailGIds, string UId)
        {
            ProcedureName = "PR_FB_Set_POCBF";

            AddParameter("CBFDetailGIds", CBFDetailGIds);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetPO(string POHeaderGId, string UId)
        {
            ProcedureName = "PR_FB_Get_PO";

            AddParameter("POHeaderGId", POHeaderGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetPOTemplate(string TemplateGId, string TemplateName, string Content, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_POTemplate";

            AddParameter("TemplateGId", TemplateGId);
            AddParameter("TemplateName", TemplateName);
            AddParameter("Content", Content);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetShipment(string ShipmentGId, string PODetailGId, string ShipmentTypeGId, string BranchGId, string InchargeGId, string ShippedQty, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_POShipment";

            AddParameter("ShipmentGId", ShipmentGId);
            AddParameter("PODetailGId", PODetailGId);
            AddParameter("ShipmentTypeGId", ShipmentTypeGId);
            AddParameter("BranchGId", BranchGId);
            AddParameter("InchargeGId", InchargeGId);
            AddParameter("ShippedQty", ShippedQty);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        //add gst
        public DataSet SetPoTaxdtls(string PODetailGId, string HSNgid, string Taxsubtypeid, string Taxrate, string TaxAbleamt, string TaxAmt)
        {
            ProcedureName = "PR_FS_Set_Pogstdetails";
            AddParameter("PODetailGId", PODetailGId);
            AddParameter("HSNgid", HSNgid);
            AddParameter("Taxsubtypeid", Taxsubtypeid);
            AddParameter("Taxrate", Taxrate);
            AddParameter("TaxAbleamt", TaxAbleamt);
            AddParameter("TaxAmt", TaxAmt);
            AddParameter("Action", "Insert");
            return ExecuteProcedure;
        }
        public DataSet GetShipment(string PODetailGId, string VendorId)
        {
            ProcedureName = "PR_FB_Get_POShipment";

            AddParameter("PODetailGId", PODetailGId);
            AddParameter("VendorId", VendorId);
            return ExecuteProcedure;
        }

        public DataSet SetPODetails(string POHeaderGId, string PODetailGId, string ProductGId, string Description, string UOM, string Qty, string UnitPrice, string BaseAmount, string Discount, string Taxes, string TaxPercent, string OtherCharges, string NetAmount, string IsRemoved, string UId)
        {
            ProcedureName = "PR_FB_Set_PODetail";

            AddParameter("POHeaderGId", POHeaderGId);
            AddParameter("PODetailGId", PODetailGId);
            AddParameter("ProductGId", ProductGId);
            AddParameter("Description", Description);
            AddParameter("UOM", UOM);
            AddParameter("Qty", Qty);
            AddParameter("UnitPrice", UnitPrice);
            AddParameter("BaseAmount", BaseAmount);
            AddParameter("Discount", Discount);
            AddParameter("Taxes", Taxes);
            AddParameter("TaxPercent", TaxPercent);
            AddParameter("OtherCharges", OtherCharges);
            AddParameter("NetAmount", NetAmount);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SplitPODetails(string POHeaderGId, string PODetailGId, string ProductGId, string Description, string UOM, string Qty, string UnitPrice, string BaseAmount, string Discount, string Taxes, string OtherCharges, string NetAmount, string UId, string POPercent = null)
        {
            ProcedureName = "PR_FB_Set_PODetailSplit";

            AddParameter("POHeaderGId", POHeaderGId);
            AddParameter("PODetailGId", PODetailGId);
            AddParameter("ProductGId", ProductGId);
            AddParameter("Description", Description);
            AddParameter("UOM", UOM);
            AddParameter("Qty", Qty);
            AddParameter("UnitPrice", UnitPrice);
            AddParameter("BaseAmount", BaseAmount);
            AddParameter("Discount", Discount);
            AddParameter("Taxes", Taxes);
            AddParameter("OtherCharges", OtherCharges);
            AddParameter("NetAmount", NetAmount);
            AddParameter("UId", UId);
            AddParameter("Percent", POPercent);
            return ExecuteProcedure;
        }

        //public DataSet SubmitPO(string POHeaderGId, string POEndDate, string ProjectOwnerGId, string VendorGId, string Type, string VendorNote, string TemplateGId, string AddedTermAndCondtion, string IsRemoved, string ViewType, string IsReject, string Remarks, string UId)
        //{
        //    ProcedureName = "PR_FB_Set_PO";

        //    AddParameter("POHeaderGId", POHeaderGId);
        //    AddParameter("POEndDate", POEndDate);
        //    AddParameter("ProjectOwnerGId", ProjectOwnerGId);
        //    AddParameter("VendorGId", VendorGId);
        //    AddParameter("Type", Type);
        //    AddParameter("VendorNote", VendorNote);
        //    AddParameter("TemplateGId", TemplateGId);
        //    AddParameter("AddedTermAndCondtion", AddedTermAndCondtion);
        //    AddParameter("IsRemoved", IsRemoved);
        //    AddParameter("ViewType", ViewType);
        //    AddParameter("IsReject", IsReject);
        //    AddParameter("Remarks", Remarks);
        //    AddParameter("UId", UId);
        //    return ExecuteProcedure;
        //}

        public DataSet GetAuditEmployee(string EmployeeGId, string TitleName, string TitleGId)
        {
            ProcedureName = "PR_FB_Get_EmployeeHierarchyName";

            AddParameter("EmployeeGId", EmployeeGId);
            AddParameter("TitleName", TitleName);
            AddParameter("TitleGId", TitleGId);
            return ExecuteProcedure;
        }

        public DataSet GetPOAmentment(string PODate, string PONo, string VendorId, string UId)
        {
            ProcedureName = "PR_FB_Get_POAmendment";

            AddParameter("PODate", PODate);
            AddParameter("PONo", PONo);
            AddParameter("VendorId", VendorId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SetPOAmendment(string POGId, string UID)
        {
            ProcedureName = "PR_FB_Set_POAmendment";

            AddParameter("POGId", POGId);
            AddParameter("UID", UID);
            return ExecuteProcedure;
        }
        #endregion

       #region FA Advance Report
        public DataSet FetchAdvanceReport(string Date, string Dateto, string ECFNo, string EmpId, string SuppId, string Raiserid, string glno, string Branch, string UId)
        {
            ProcedureName = "PR_FA_Rpt_Advance";
            AddParameter("Date", Date);
            AddParameter("Dateto", Dateto);
            AddParameter("ECFNo", ECFNo);
            AddParameter("EmpId", EmpId);
            AddParameter("SuppId", SuppId);
            AddParameter("Raiserid", Raiserid);
            AddParameter("glno", glno);
            AddParameter("Branch", Branch);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion


        #region FA Retention Report
        public DataSet FetchRetentionReport(string Date, string ECFNo, string EmpId, string SuppId, string UId)
        {
            ProcedureName = "PR_FA_Rpt_Retention";

            AddParameter("Date", Date);
            AddParameter("ECFNo", ECFNo);
            AddParameter("EmpId", EmpId);
            AddParameter("SuppId", SuppId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Sales Report
        public DataSet FetchSalesReport(string DateFrom, string DateTo, string UId)
        {
            ProcedureName = "PR_FA_Rpt_Sales";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region Sales Report
        public DataSet FetchSaleInvoiceTrackerReport(string DateFrom, string DateTo, string UId)
        {
            ProcedureName = "PR_FA_Rpt_SalesinvoiceTracker";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region FA CBF Tracker
        public DataSet FetchCBFTrackerReport(string Status, string UId)
        {
            ProcedureName = "PR_FA_Rpt_CBFTracker";

            AddParameter("Status", Status);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region CBF Utilization
        public DataSet FetchCBFUtilizationReport(string CreationDateFrom, string CreationDateTo, string ApprovalDateFrom, string ApprovalDateTo, string CBFNo, string UId)
        {
            ProcedureName = "PR_FA_Rpt_CBFUtilization";

            AddParameter("CrDateFrom", CreationDateFrom);
            AddParameter("CrDateTo", CreationDateTo);
            AddParameter("AppDateFrom", ApprovalDateFrom);
            AddParameter("AppDateTo", ApprovalDateTo);
            AddParameter("CbfNo", CBFNo);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region FlexiBuy Report
        public DataSet FetchReport(string RegionId, string ViewType, string UId,string Fromdate, string Todate)
        {
            ProcedureName = "PR_FA_Get_QueueingReport";

            AddParameter("RegionId", RegionId);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            AddParameter("Fromdate", Fromdate);
            AddParameter("Todate", Todate);

            return ExecuteProcedure;
        }
        #endregion

        #region FlexiBuy Rpt
        public DataSet FetchPoInvoiceRpt(string pono, string UId, string Fromdate, string Todate)
        {
            ProcedureName = "PO_FA_Get_PoinvoiceRpt";
            AddParameter("pono", pono);
            AddParameter("UId", UId);
            AddParameter("Fromdate", Fromdate);
            AddParameter("Todate", Todate);
            return ExecuteProcedure;
        }
        #endregion
        #region FlexiBuy Rpt
        public DataSet FetchWoInvoiceRpt(string wono, string UId, string Fromdate, string Todate)
        {
            ProcedureName = "PO_FA_Get_WoinvoiceRpt";
            AddParameter("pono", wono);
            AddParameter("UId", UId);
            AddParameter("Fromdate", Fromdate);
            AddParameter("Todate", Todate);
            return ExecuteProcedure;
        }
        #endregion

        #region FlexiBuy Rpt
        public DataSet FetchCbfDataRpt(string cbfno, string UId, string Formdate, string Todate)
        {
            ProcedureName = "CBF_FA_Get_Rpt";
            AddParameter("cbfno", cbfno);
            AddParameter("UId", UId);
            AddParameter("Fromdate", Formdate);
            AddParameter("Todate", Todate);
            return ExecuteProcedure;
        }
        #endregion

        #region Inex Details
        public DataSet GetInexDetails(string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_InexDtlForRaiser";
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetInexReason(string ECFGId, string UId)
        {
            ProcedureName = "PR_FS_Get_InexAttachmentReason";

            AddParameter("ECFGId", ECFGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Download Excel
        public DataSet GetCommonExcelDownload(string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_CommonExcelDownload";

            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Reports
        public DataSet GetBankSummaryReport(string DateFrom, string DateTo, string ECFNo, string View, string UId)
        {
            ProcedureName = "PR_FS_Rpt_PPX_BankSummary";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("View", View);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetAmortReport(string EcfDateFrom, string EcfDateTo, string EcfNo, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_AmortReport";

            AddParameter("EcfDateFrom", EcfDateFrom);
            AddParameter("EcfDateTo", EcfDateTo);
            AddParameter("EcfNo", EcfNo);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetAdvanceReport(string ECFFrom, string ECFTo, string LiqFrom, string LiqTo, string ECFNo, string ViewType, string UId, string RAISERID, string VENDORNAME, string GLCODE, string LOCATION)
        {
            ProcedureName = "PR_FS_Rpt_Advance";

            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("LiqFrom", LiqFrom);
            AddParameter("LiqTo", LiqTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("RAISER", RAISERID);
            AddParameter("VENDOR", VENDORNAME);
            AddParameter("GLCODE", GLCODE);
            AddParameter("LOCATION", LOCATION);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        public DataSet GetPPXLiquidationReport(string ECFFrom, string ECFTo, string ECFNo, string UId)
        {
            ProcedureName = "PR_FS_Rpt_PPXliqSummary";

            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        public DataSet GetCreditReport(string ECFFrom, string ECFTo, string ECFNo, string ViewType, string UId, string RAISERID, string VENDORNAME, string GLCODE, string LOCATION)
        {
            ProcedureName = "PR_FS_Rpt_Credit";

            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("RAISER", RAISERID);
            AddParameter("VENDOR", VENDORNAME);
            AddParameter("GLCODE", GLCODE);
            AddParameter("LOCATION", LOCATION);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetDebitReport(string ECFFrom, string ECFTo, string ECFNo, string ViewType, string UId, string RAISERID, string VENDORNAME, string GLCODE, string LOCATION)
        {
            ProcedureName = "PR_FS_Rpt_Debit";

            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("RAISER", RAISERID);
            AddParameter("VENDOR", VENDORNAME);
            AddParameter("GLCODE", GLCODE);
            AddParameter("LOCATION", LOCATION);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }


        public DataSet GetCenvantReport(string ECFFrom, string ECFTo, string ECFNo, string UId)
        {
            ProcedureName = "PR_FS_Rpt_Cenvat";

            AddParameter("ECFFrom", ECFFrom);
            AddParameter("ECFTo", ECFTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetQCReport(string DateFrom, string DateTo, string ECFNo, string EmpId, string SuppId, string UId)
        {
            ProcedureName = "PR_FS_Rpt_QC";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("ECFNo", ECFNo);
            AddParameter("EmpId", EmpId);
            AddParameter("SuppId", SuppId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        #endregion
        
        #region Get Ecf Gid 

        public DataSet GetEcfGidByDocRefNo(string DocRefNo)
        {
            ProcedureName = "PR_FS_Get_Ecf_GId";

            AddParameter("DOCREFNO", DocRefNo);
            return ExecuteProcedure;
        }

        public DataSet GetEcfByInexID(string InexId)
        {
            ProcedureName = "PR_FS_Get_Ecf_GId_By_InexId";

            AddParameter("InexId", InexId);
            return ExecuteProcedure;

        }

        #endregion

        #region Get Supplier or Employee from ecf_gid

        public DataSet GetSupplierOREmployeeClaimType(string ecf_gid)
        {
            ProcedureName = "PR_FS_Get_SUPPLIER_EMPLOYEE_CLAIM";

            AddParameter("ECF_GID", ecf_gid);
            return ExecuteProcedure;
        }

        public DataSet IsPrepaidLiquidation(string ecf_gid)
        {
            ProcedureName = "PR_FS_IS_PREPAIDLIQUIDATION_DATA";
            AddParameter("ECFId", ecf_gid);
            return ExecuteProcedure;
        }

        #endregion

        #region Get Ecf Gid, Claim Mode and Payment Mode For Payment Alert Mail
        
        public DataSet GetEecGid_ClaimMode_PayMode(string MemoNo, string PVNo)
        {
            ProcedureName = "PR_FS_PAYMENT_ALERT_SUPPLIER_EMP";
            AddParameter("MemoNO", MemoNo);
            AddParameter("PouchNO", PVNo);
            return ExecuteProcedure;
        }

        #endregion

        #region Get PayrunVoucher Gid by cheque and pouch no.
        public DataSet GetPVID_Chq_pouch_no(string ChqNo, string PVNo)
        {
            ProcedureName = "PR_FS_GET_PVId_By_CHQ_Pouch_No";
            AddParameter("ChqNo", ChqNo);
            AddParameter("PouchNO", PVNo);
            return ExecuteProcedure;
        }
        #endregion

        public DataSet getuploaddata(string slno, string pvno)
        {
            ProcedureName = "Fs_uploaddata";
            AddParameter("slno", slno);
            AddParameter("pvno", pvno);
            return ExecuteProcedure;
        }
        //add Dhasarathan 01-12-2016
        #region Dhasarathan 01-12-2016
        public DataSet GetPoAttachments(string poheadergid)
        {
            ProcedureName = "GetPoAttachments";

            AddParameter("poheadergid", poheadergid);

            return ExecuteProcedure;
        }
        #endregion
		
        #region Approval Summary(Dashboard)
        public DataSet GetAttachmentASNew(string RefFlag, string RefGId, string UId)
        {
            ProcedureName = "PR_Get_AttachmentNew";
            AddParameter("RefFlag", RefFlag);
            AddParameter("RefGId", RefGId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region GST
        public DataSet GetGSTMaster(string ViewType, string RefId, string UId)
        {
            ProcedureName = "pr_get_gstmaster";

            AddParameter("ViewType", ViewType);
            AddParameter("RefId", RefId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetGSTNVendorDetails(string ViewType, string GSTIN, string SubrefId, string UId)
        {
            ProcedureName = "pr_get_gstmaster";

            AddParameter("ViewType", ViewType);
            AddParameter("GSTIN", GSTIN);
            AddParameter("SubrefId", SubrefId);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        #endregion

        #region Mulitple Invoice
        public DataSet GetInvoiceDetailsPettyCash(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_InvoiceDetailPettyCash";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet GetInvoiceDetailsTravel(string InvId, string UId)
        {
            ProcedureName = "PR_FS_Get_InvoiceDetailTravel";

            AddParameter("InvId", InvId);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        //added by vijayavel 20-02-2017
        public DataSet GetProcedure(string procedure_name)
        {
            ProcedureName = procedure_name;
            return ExecuteProcedure;
        }
        
        #region Dhasarathan 20-04-2017
        public DataSet GetQueryParameter(string Sqlquerygid)
        {
            ProcedureName = "pr_iem_mst_tsqlqueryparameter";

            AddParameter("Sqlquerygid", Sqlquerygid);

            return ExecuteProcedure;
        }
        #endregion

        #region Dashboard
        #region Variables
        //private proLib plib = new proLib();
        #endregion

        #region Dashboard
        public DataSet GetOverAllDashboard(string empgid, string raiser, string action)
        {
            ProcedureName = "PR_Get_DashboardCount";
            AddParameter("empgid", empgid);
            AddParameter("raiser", raiser);
            AddParameter("action", action);
            return ExecuteProcedure;
        }


        public DataSet GetCountOfForMyApproval(string employee_gid, string doctype)
        {
            ProcedureName = "proGetCountOfForMyApproval";
            AddParameter("employee_gid", employee_gid);
            AddParameter("doctype", doctype);
            return ExecuteProcedure;
        }


        public DataSet GetDashboardSummary(string empgid, string action, string requesttype, string requeststatus, string proxytype)
        {
            ProcedureName = "PR_Get_DashboardSummary";
            AddParameter("empgid", empgid);
            AddParameter("requesttype", requesttype);
            AddParameter("proxytype", proxytype);
            AddParameter("requeststatus", requeststatus);
            AddParameter("action", action);
            return ExecuteProcedure;
        }

        public DataSet GetASMSHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus, string UId)
        {
            ProcedureName = "PR_Get_HistoryASMS";
            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("RequestType", RequestType);
            AddParameter("RequestStatus", RequestStatus);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetEProcureHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus, string UId)
        {
            ProcedureName = "PR_Get_HistoryFB";
            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("RequestType", RequestType);
            AddParameter("RequestStatus", RequestStatus);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetEClaimHistory(string DateFrom, string DateTo, string RequestType, string RequestStatus, string UId)
        {
            ProcedureName = "PR_Get_HistoryEOW";
            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);
            AddParameter("RequestType", RequestType);
            AddParameter("RequestStatus", RequestStatus);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        //ASMS
        public DataSet GetASMSSupplierHeaderDetails(string Suppliergid, string Action)
        {
            ProcedureName = "PR_Get_ApproverSummaryASMS";
            AddParameter("Suppliergid", Suppliergid);
            AddParameter("Action", Action);
            return ExecuteProcedure;
        }

        //FB
        public DataSet GetFBDetails(string Refgid, string Action)
        {
            ProcedureName = "PR_Get_ApproverSummaryFB";
            AddParameter("Refgid", Refgid);
            AddParameter("Action", Action);
            return ExecuteProcedure;
        }

        //EClaims
        public DataSet GetEClaimsDetails(string ECFId, string DocSubtype)
        {
            ProcedureName = "PR_Get_ApproverSummaryEOW";
            AddParameter("ECFId", ECFId);
            AddParameter("DocSubtype", DocSubtype);
            return ExecuteProcedure;
        }

        //Eprocure Approve/Reject
        public DataSet SubmitEprocure(string RefGId, string Action, string Submit, string Remarks, string UId)
        {
            ProcedureName = "PR_Set_ApproverSummaryFB";

            AddParameter("RefGId", RefGId);
            AddParameter("Action", Action);
            AddParameter("Submit", Submit);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        //EClaim Approve/Reject
        public DataSet SubmitEClaim(string EcfId, string DocType, string Submit, string Remarks, string ConcurrentTo, string UId)
        {
            ProcedureName = "PR_Set_ApproverSummaryEOW";

            AddParameter("EcfId", EcfId);
            AddParameter("DocType", DocType);
            AddParameter("ConcurrentTo", ConcurrentTo);
            AddParameter("Submit", Submit);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet GetMaxQueueGidEClaim(string EcfId, string EmployeeGid)
        {
            ProcedureName = "pr_eow_mst_NatureofExpenses";

            AddParameter("para1", EmployeeGid);
            AddParameter("para2", EcfId);
            AddParameter("action", "GetMaxqueuegid");
            return ExecuteProcedure;
        }

        public DataSet GetDocTypeGIDEClaim(string queue_gid)
        {
            ProcedureName = "pr_eow_mst_NatureofExpenses";
            AddParameter("para1", queue_gid);
            AddParameter("action", "Getdocsubtype");
            return ExecuteProcedure;
        }

        //ASMS Approve/Reject
        public DataSet SubmitASMS(string refgid, string empgid, string requesttype, string actionremark, string queueto, string actionstatus, string skipflag, string action)
        {
            ProcedureName = "pr_asms_SubmitApproval";

            AddParameter("refgid", refgid);
            AddParameter("empgid", empgid);
            AddParameter("requesttype", requesttype);
            AddParameter("actionremark", actionremark);
            AddParameter("queueto", queueto);
            AddParameter("actionstatus", actionstatus);
            AddParameter("skipflag", skipflag);
            AddParameter("action", action);

            return ExecuteProcedure;
        }

        //Attachment SP
       
        public DataSet SetAttachmentNew(string RefFlag, string RefType, string RefGId, string AttachmentName, string Description, string UId)
        {
            ProcedureName = "PR_Set_AttachmentNew";
            AddParameter("RefFlag", RefFlag);
            AddParameter("RefType", RefType);
            AddParameter("RefGId", RefGId);
            AddParameter("AttachmentName", AttachmentName);
            AddParameter("Description", Description);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SubmitCBF(string CBFHeaderGId, string Type, string IsReject, string Remarks, string UId)
        {
            ProcedureName = "PR_FB_Set_CBF";

            AddParameter("CBFHeaderGId", CBFHeaderGId);
            AddParameter("Type", Type);
            AddParameter("IsReject", IsReject);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }

        public DataSet SubmitPO(string POHeaderGId, string POEndDate, string ProjectOwnerGId, string VendorGId, string Type, string VendorNote, string TemplateGId, string AddedTermAndCondtion, string IsRemoved, string ViewType, string IsReject, string Remarks, string UId, string VendorcontactId)
        {
            ProcedureName = "PR_FB_Set_PO";

            AddParameter("POHeaderGId", POHeaderGId);
            AddParameter("POEndDate", POEndDate);
            AddParameter("ProjectOwnerGId", ProjectOwnerGId);
            AddParameter("VendorGId", VendorGId);
            AddParameter("Type", Type);
            AddParameter("VendorNote", VendorNote);
            AddParameter("TemplateGId", TemplateGId);
            AddParameter("AddedTermAndCondtion", AddedTermAndCondtion);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("ViewType", ViewType);
            AddParameter("IsReject", IsReject);
            AddParameter("Remarks", Remarks);
            AddParameter("UId", UId);
            AddParameter("VendorcontactId", VendorcontactId);
            return ExecuteProcedure;
        }

        public DataSet GetASMSMailDetails(string RefId)
        {
            ProcedureName = "pr_asms_GetCSC";
            AddParameter("supplierheadergid", RefId);
            AddParameter("action", "getemaildetails");
            return ExecuteProcedure;
        }
        #endregion

        #region Hold Tab
        public DataSet GetHoldTabDetails(string emplogin, string action)
        {
            ProcedureName = "pr_iem_trn_ECFReport";
            AddParameter("emplogin", emplogin);
            AddParameter("action", action);
            return ExecuteProcedure;
        }

        public DataSet UpdateReleaseHold(string EcfId, string ActionRemark, string UId)
        {
            ProcedureName = "PR_FB_Set_HoldRelease";
            AddParameter("EcfId", EcfId);
            AddParameter("ActionRemark", ActionRemark);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion

        #region My Request Tab
        public DataSet GetMyRequestTabDetails(string empgid, string raiser, string proxytype)
        {
            ProcedureName = "PR_Get_DashboardMyRequest";
            AddParameter("empgid", empgid);
            AddParameter("raiser", raiser);
            AddParameter("proxytype", proxytype);
            return ExecuteProcedure;
        }
        #endregion

        #region My Approval Tab
        public DataSet GetMyApprovalTabDetails(string empgid, string proxytype)
        {
            ProcedureName = "PR_Get_DashboardForMyApproval";
            AddParameter("empgid", empgid);
            AddParameter("proxytype", proxytype);
            return ExecuteProcedure;
        }
        #endregion
        #endregion

        #region  add Gst Cbo details
        public DataSet GetGstCbolist()
        {
            ProcedureName = "Get_GstTaxcbodel";
            AddParameter("Action", "ALL");
            return ExecuteProcedure;
        }
        public DataSet GetPodtlsgst(string PODetailGId)
        {
            ProcedureName = "Get_Podetails_Sp";

            AddParameter("PODetailGId", PODetailGId);
            return ExecuteProcedure;
        }
        public DataSet Getgsttax(string PODetailGId, string ProvLocId, string Baseamt, string ReciverLocId,string HSN_GID)
        {
            ProcedureName = "Getgsttax_sp";
            AddParameter("PODetailGId", PODetailGId);
            AddParameter("ProvLocId", ProvLocId);
            AddParameter("Baseamt", Baseamt);
            AddParameter("ReciverLocId", ReciverLocId);
            AddParameter("HSN_GID", @HSN_GID);
            return ExecuteProcedure;
        }
        public DataSet SavepoGstdetails(string POHeaderGId, string PODetilsId, string ProvLocationId, string ReceiverLocationId, string HsnId, string TaxsubTypeId, string TaxablAmt, string TaxRate, string NetAmt, string Vendorid, string TaxAmt)
        {
            ProcedureName = "PR_FS_Set_Pogstdetails";
            AddParameter("PODetailGId", PODetilsId);
            AddParameter("HSNgid", HsnId);
            AddParameter("Taxsubtypeid", TaxsubTypeId);
            AddParameter("Taxrate", TaxRate);
            AddParameter("TaxAbleamt", TaxablAmt);
            AddParameter("TaxNetAmt", NetAmt);
            AddParameter("ProvidLocId", ProvLocationId);
            AddParameter("ReceiverLocId", ReceiverLocationId);
            AddParameter("Vendorid", Vendorid);
            AddParameter("TaxAmt", TaxAmt);
            AddParameter("Action", "Insert");
            return ExecuteProcedure;
        }

        public DataSet DeletePoGstdtl(string POId, string HeaderType, string VendorId, string VendorContactId)
        {
            ProcedureName = "PR_FS_Det_Pogstdtl";
            AddParameter("POId", POId);
            AddParameter("HeaderType", HeaderType);
            AddParameter("VendorId", VendorId);
            AddParameter("VendorContactId", VendorContactId);
            return ExecuteProcedure;
        }
        public DataSet DeletePoctdetails(string PODetailGId)
        {
            ProcedureName = "PR_FS_Det_Prodectshipment";
            AddParameter("PODetailGId", PODetailGId);
            return ExecuteProcedure;
        }
        public DataSet GetVendorAddressDetails(string Vendorid)
        {
            ProcedureName = "PR_FS_GetVendorAddress";
            AddParameter("VendorId", Vendorid);
            return ExecuteProcedure;
        }
        #endregion
        #region Eow Travel Save Invoice details
        public DataSet SetInvoiceTravelDetails(string EcfId, string InvId, string ProviderLocation, string GstinVendor, string Suppliergid, string InvNo,
            string InvDate, string Amount, string Desc, string WOTaxAmount, string IsVerify, string IsRemoved, string GstCharged, string ReceiverLocation,
            string GstinFiccl, string ServiceMonth, string UId)
        {
            ProcedureName = "PR_EOW_Set_InvoiceDetails";
            AddParameter("EcfId", EcfId);
            AddParameter("InvId", InvId);
            AddParameter("ProviderLocation", ProviderLocation);
            AddParameter("ReceiverLocation", ReceiverLocation);
            AddParameter("Suppliergid", Suppliergid);
            AddParameter("InvAmt", Amount);
            AddParameter("InvNo", InvNo);
            AddParameter("InvDate", plib.ConvertDate(InvDate));
            AddParameter("InvDesc", Desc);
            AddParameter("InvWitoutTax", WOTaxAmount);
            AddParameter("Verification", IsVerify);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("GstCharged", GstCharged);
            AddParameter("GstInFiccl", GstinFiccl);
            AddParameter("GstInVendor", GstinVendor);
            AddParameter("Servicemonth", plib.ConvertDate(ServiceMonth));
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
          public DataSet GetExpenseCategory(string ExpenseId)
        {
            ProcedureName = "pr_eow_mst_NatureofExpenses";
            AddParameter("id1", ExpenseId);
            AddParameter("action", "SubCategory");
            return ExecuteProcedure;
        }
        
        public DataSet GetExpenseHsn(string ExpenseId)
        {
            ProcedureName = "pr_eow_mst_NatureofExpenses";
            AddParameter("id1", ExpenseId);
            AddParameter("action", "GetExpenseHsn");
            return ExecuteProcedure;
        }
        public DataSet GetAssetHsn(string ExpenseId)
        {
            ProcedureName = "pr_eow_mst_NatureofExpenses";
            AddParameter("id1", ExpenseId);
            AddParameter("action", "GetAssetHsn");
            return ExecuteProcedure;
        }

        public DataSet LoadInvoiceHeaderDetails(string EcfId, string InvId)
        {
            ProcedureName = "PR_EOW_Set_LoadInvoiceDetails";
            AddParameter("EcfId", EcfId);
            AddParameter("InvId", InvId);
            return ExecuteProcedure;
        }
        public DataSet CheckGstInvdels(string InvId)
        {
            ProcedureName = "PR_Eow_CheckGststatus";
            AddParameter("Invid", InvId);
            return ExecuteProcedure;
        }
        public DataSet checkdebithsn(string InvId)
        {
            ProcedureName = "PR_Eow_Checkhsn";
            AddParameter("Invid", InvId);
            return ExecuteProcedure;
        }

        public DataSet SetInvoiceNonTravelDetails(string EcfId, string InvId, string ProviderLocation, string GstinVendor, string Suppliergid, string InvNo,
            string InvDate, string Amount, string Desc, string WOTaxAmount, string IsVerify, string IsRemoved, string GstCharged, string ReceiverLocation,
            string GstinFiccl, string ServiceMonth, string UId)
        {
            //Ramya
            ProcedureName = "PR_EOW_Set_InvoiceDetails";
            AddParameter("EcfId", EcfId);
            AddParameter("InvId", InvId);
            AddParameter("ProviderLocation", ProviderLocation);
            AddParameter("ReceiverLocation", ReceiverLocation);
            AddParameter("Suppliergid", Suppliergid);
            AddParameter("InvAmt", Amount);
            AddParameter("InvNo", InvNo);
            AddParameter("InvDate", plib.ConvertDate(InvDate));
            AddParameter("InvDesc", Desc);
            AddParameter("InvWitoutTax", WOTaxAmount);
            AddParameter("Verification", IsVerify);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("GstCharged", GstCharged);
            AddParameter("GstInFiccl", GstinFiccl);
            AddParameter("GstInVendor", GstinVendor);
            AddParameter("Servicemonth", plib.ConvertDate(ServiceMonth));
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }
        public DataSet supplierGstUpdate(string InvId, string GstCharged, string ProviderLocation, string GstinVendor, string ReceiverLocation, string GstinFiccl)
        {
            ProcedureName = "PR_Eow_Supllierupdate";
            AddParameter("Invid", InvId);
            AddParameter("GstCharged", GstCharged);
            AddParameter("ProviderLocation", ProviderLocation);
            AddParameter("GstinVendor", GstinVendor);
            AddParameter("ReceiverLocation", ReceiverLocation);
            AddParameter("GstinFiccl", GstinFiccl);
            return ExecuteProcedure;
        }
        public DataSet GetpoGstDetails(string poid, string Invid)
        {
            ProcedureName = "PR_EOW_GETPOGSTDETAILS";
            AddParameter("Invid", Invid);
            AddParameter("poid", poid);
            return ExecuteProcedure;
        }

        public DataSet GetVendorAutoComplete(string SearchText, string ViewType, string UId)
        {
            ProcedureName = "PR_FS_Get_AutoComplete";

            AddParameter("SearchText", SearchText);
            AddParameter("ViewType", ViewType);
            AddParameter("UId", UId);
            return ExecuteProcedure;
        }


        public DataSet Getstatebypincode(string pincodeid)
        {
            ProcedureName = "pr_iem_mst_tbranch";
            AddParameter("@branch_gid", pincodeid);
            AddParameter("action", "GETSTATEBYPINCODE");
            return ExecuteProcedure;
        }
        public DataSet Getdistrictbystate(string pincodeid, string stategid)
        {
            ProcedureName = "pr_iem_mst_tbranch";
            AddParameter("@branch_gid", pincodeid);
            AddParameter("@CITY_GID", stategid);
            AddParameter("action", "GETDISTRICTBYSTATE");
            return ExecuteProcedure;
        }

        public DataSet CheckAuthorize(string gid, string approvergid, string refType)
        {
            ProcedureName = "prAuthorizeCheck1";
            AddParameter("@gid", gid);
            AddParameter("@approver_gid", approvergid);
            AddParameter("@refType", refType);
            return ExecuteProcedure;
        }
        //Ramya Start - 01 Aug 18
        public DataSet SetEditCreditLineDetails(string Ecfid, string InvId, string CreditlineGId, string Id, string paymode, string RefNo, string Beneficiary,
            string Desc, string Amount, string BankId, string IsRemoved, string IfscCode, string UId)
        {
            ProcedureName = "PR_FS_Set_Edit_CreditLine";

            AddParameter("Ecfid", Ecfid);
            AddParameter("InvId", InvId);
            AddParameter("CreditlineGId", CreditlineGId);
            AddParameter("Id", Id);
            AddParameter("paymode", paymode);
            AddParameter("RefNo", RefNo);
            AddParameter("Beneficiary", Beneficiary);
            AddParameter("Desc", Desc);
            AddParameter("Amount", Amount);
            AddParameter("BankId", BankId);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("IfscCode", IfscCode);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }

        public DataSet SetEditECFCreditLineDetails(string Ecfid, string InvId, string CreditlineGId, string Id, string paymode, string RefNo, string Beneficiary,
            string Desc, string Amount, string BankId, string IsRemoved, string IfscCode, string UId)
        {
            ProcedureName = "PR_EOW_Set_Edit_ecfcreditline";

            AddParameter("Ecfid", Ecfid);
            AddParameter("InvId", InvId);
            AddParameter("CreditlineGId", CreditlineGId);
            AddParameter("Id", Id);
            AddParameter("paymode", paymode);
            AddParameter("RefNo", RefNo);
            AddParameter("Beneficiary", Beneficiary);
            AddParameter("Desc", Desc);
            AddParameter("Amount", Amount);
            AddParameter("BankId", BankId);
            AddParameter("IsRemoved", IsRemoved);
            AddParameter("IfscCode", IfscCode);
            AddParameter("UId", UId);

            return ExecuteProcedure;
        }
        #endregion
        //Ramya End - 01 Aug 18
        
        #region Local Conveyance
        public DataSet GetLocalConveyanceDateWaise(string DateFrom, string DateTo)
        {
            ProcedureName = "PR_EClaims_Get_LocalConveyanceByDate";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);

            return ExecuteProcedure;
        }
        #endregion;

        #region Query Invoice
        public DataSet GetQueryInvoiceDateWaise(string DateFrom, string DateTo)
        {
            ProcedureName = "PR_EClaims_GET_QueryInvoiceByDate";

            AddParameter("DateFrom", DateFrom);
            AddParameter("DateTo", DateTo);

            return ExecuteProcedure;
        }
        #endregion;

        public DataSet GetTravelClaimHistory(string EcfId, string UId)
        {
            ProcedureName = "pr_iem_tauditmakerhistory";

            AddParameter("pecf_gid", EcfId);
            AddParameter("pinvoice_gid", UId);

            return ExecuteProcedure;
        }

        public DataSet CheckrcmExists(string ecfgid, string debitlinegid, string invid, string hsngid, string providerloc, string action)
        {
            ProcedureName = "PR_FS_Get_RcmCheck";
            AddParameter("@ecfgid", ecfgid);
            AddParameter("@debitlinegid", string.IsNullOrEmpty(debitlinegid) ? "" : debitlinegid.ToString());
            AddParameter("@invid", string.IsNullOrEmpty(invid) ? "" : invid.ToString());
            AddParameter("@hsngid", string.IsNullOrEmpty(hsngid) ? "" : hsngid.ToString());
            AddParameter("@providerloc", string.IsNullOrEmpty(providerloc) ? "" : providerloc.ToString());
            AddParameter("@action", action);
            return ExecuteProcedure;
        }
    }
}