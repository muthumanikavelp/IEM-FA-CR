using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using IEM.Common;
using System.Collections;

namespace IEM.Areas.EOW.Models
{
    public interface EOW_IRepository
    {

        /////////////-----------Employeee Claim--------
        IEnumerable<EOW_RefNo> EmployeeePaymentchq(string supgid, string type);
        string GetSupplieruploaadexcel(DataTable DataTable, string ecfgid, string ecfdate);
        IEnumerable<EOW_Raisercc> SelectglnumberSearch(string glnumber);
        string togetsumofinvoiceamt(string ecfgid, string strartdate, string enddate);
        IEnumerable<EOW_citys> tTravelcitydataauto(string cityname);
        IEnumerable<EOW_PO> grninwrddetview(int id, int detailsgids, string invoicegid);
        string Getinvtaxamt(string ecfgid, string invoicegid);
        IEnumerable<EOW_Doctype> GetMDoctype();
        string Insertitemdetailsnew(EOW_PO supplierpo, string id, string[] SelectedValues);
        string printstatus(string ecfgid);
        string Deletectdraftecfs(string ecfgid, string login);
        IEnumerable<EOW_EmployeeeExpense> GetEmployeeeExpensenew(string ecfid, string invoiceid);
        string InsertEmployeeeExpensenew(EOW_EmployeeeExpense EmployeeeExpense, string ecfgid, string invoicegid, string empgid);
        IEnumerable<EOW_EmployeeeExpense> SelectEmployeeeExpensebyidnew(string ecfid, string invoiceid, int id);
        string UpdateEmployeeeExpensenew(EOW_EmployeeeExpense EmployeeeExpense, string ecfgid, string invoicegid, string expactiverowid, string empgid);
        string HoldFileUploadUrl();
        string UpdateCentralecf(EOW_Supplierinvoice EmployeeeExpenseModel, string logeempid, string clmtype, string queid, string raiser);
        string UpdateCentralecffinal(EOW_Supplierinvoice EmployeeeExpenseModel, string logeempid, string ecfgid, string queid, string raiser);
        IEnumerable<CentraldataModel> SelectEmployeeSearchCode(string EmployeeCode);
        IEnumerable<CentraldataModel> SelectEmployeeSearchName(string EmployeeName);
        IEnumerable<CentraldataModel> SelectSupplierSearchname(string SupplierName);
        IEnumerable<CentraldataModel> SelectSupplierSearchcode(string SupplierCode);
        IEnumerable<EOW_EmployeeeExpense> GetprintExpense(string ecfid);
        string DeleteSupplierdebitclear(string invoicegid, string ecfgid);
        IEnumerable<EOW_Payment> GetprintPayment(string ecfgid);
        IEnumerable<EOW_TravelClaim> GetprintExpensesupplier(string ecfid);
        IEnumerable<EOW_SupplierModelgrid> GetSupplieinvoice(string ecfid);
        string InsertEmployeeePaymentbasicupdatepetty(string EmployeeeGid, string ecfgid, string amt);
        IEnumerable<EOW_Raisercc> SelectproductSearch(string productCode);
        string Selectsupplier(string loginid);
        string travelbranchflag(int id);
        IEnumerable<EOW_RefNo> RefNodatasupplierSUS(string empgid);
        IEnumerable<EOW_EmployeeeExpense> debitlinegldetailsdata(string glnumbers, string type);
        IEnumerable<EOW_EmployeeeExpense> debitlinegldetailscentraldata(string glnumbers, string types);
        IEnumerable<EOW_Raisercc> SelectoucodeSearch(string ouCode);
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodatacrn(string supgid, string type);
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodatasus(string supgid, string type);
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodataeft(string supgid, string type);
        IEnumerable<DashBoard> GetEPURejectedCount(string userlognid, string type);
        IEnumerable<DashBoard> GetEPUInprocessCount(string userlognid, string type);
        IEnumerable<DashBoard> GetPaidCount(string userlognid, string type);
        DataTable selectemmpdetailforlocal(string gid);
        DataSet selectpoitemdetails(string proserid);
        string EmployeeePaymentppxcheck(string supgid, string invoiceamt, string ecf_gid, string invoice_gid);
        IEnumerable<EOW_RefNo> EmployeeePaymentcrn(string supgid, string type);
        string EmployeeePaymentppxcheckct(string supgid, string invoiceamt, DataTable dt, string invoice_gid);
        IEnumerable<EOW_TravelClaim> GetLocalexpdatapopup(string ecfid, string traveltype);
        IEnumerable<CentraldataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<CentraldataModel> SelectEmployee();
        string GetEmployeeelocal(DataTable convertTable, DataTable uploadTable, string ecfgid, string ecfdelmatamt);
        string selectemmpdetailforlocallogn(string gid);
        string selectempbrabchcode(string gid);
        string UpdateSupplierinvoicemanual(EOW_Supplierinvoice EmployeeeExpenseModel, string ecfgid, string invoiceGid, string user, string eempid, string clam, string queid);
        IEnumerable<DashBoard> GetMydocCancelled(string userlognid, string type);
        IEnumerable<EOW_RefNo> RefNodatasuppliercrn(string empgid);
        IEnumerable<EOW_SupplierModelgrid> Lastthrrmonth(string supid);
        string GetStatuspetty(string valatate, string valatate1);
        string Insertecfdraftproxcy(EOW_EmployeeeExpense EmployeeeExpense, string id);
        string ExpenseCategorydataflag(int id);
        string Travelclassdataflag(int id);
        string selectcentralglcodeassetcat(string glexpcodeid);
        string Gettaxaval(string taxgidname, string taxgid, string supgid);
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodatasupplier(string empgid);
        IEnumerable<DashBoard> GetDashBoardUtlity(string userlognid, string countflag);
        string InsertEmployeeePaymentpetty(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string rempayam);
        string UpdateEmployeeePaymentpetty(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string payactiverowid, string beforeamt, string eraamt);
        string updateinvoicepayment(EOW_SupplierModelgrid EmployeeeExpenseModel, string supplier, string ecfgid, string invoiceGid, string clam);
        string selectcemtam(string queuegid, string type);
        string GetStatusexcelduplicate(string valatate, string valatate1, string valatate2, string valatate3,
            string valatate4, string valatate5, string valatate6, string valatate7, string valatate8, string valatate9, string valatate10, string action);
        string centralapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt, string checker_raisergid);
        void raiseapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt, string checker_raisergid);
        string UpdateCentralinvoicedash(EOW_Supplierinvoice EmployeeeExpenseModel, string logeempid, string clmtype, string queid, string raiser, string ecfgid);
        string CheckExpense(EOW_TravelClaim EmployeeeExpense);
        IEnumerable<EOW_PO> GetPoDetailsitemcen(string ecfid, string invoiceid, string id);
        string selectsupplierinvoicestatuscen(string queuegid, string type);
        string Getemployeebrach(string empgid);
        IEnumerable<DashBoard> GetStatusTypeapp();
        IEnumerable<EOW_Employeelst> SelectSupplierSearch(string SupplierName, string SupplierCode);
        string selectfcccval(string fc, string cc);
        string selectpolicy(string Gid);
        string selectfccc(string fc, string cc);
        IEnumerable<EOW_Raisermode> GetRaiserMode();
        IEnumerable<DashBoard> Getmanualecfdetails(string userlognid);
        IEnumerable<EOW_Raiserfc> fcdata();
        IEnumerable<EOW_Raisercc> ccdata();
        IEnumerable<EOW_Doctype> GetDoctype();
        IEnumerable<EOW_Currency> GetCurrency();
        IEnumerable<EOW_File> GetinvoiceAttachment(string ecfgid, string invoicegid);
        string InsertEmpAttinv(HttpPostedFileBase savefile, EOW_File EmployeeeExpense, string ecfgid, string invoicegid, string EmployeeeGid);
        string Getcurrencydata(EOW_Currency rate);
        string GetDecnote(string subsype, string type);
        string Insertecfdraft(EOW_EmployeeeExpense Expense, string id);
        List<EOW_Employeelst> getSupplierdetails();
        IEnumerable<EOW_NatureofExpenses> NatureofExpensesdata();
        IEnumerable<EOW_NatureofExpenses> NatureofExpensesdataother();
        IEnumerable<EOW_NatureofExpenses> NatureofExpensesdataothersupplier();
        IEnumerable<EOW_ExpenseCategory> ExpenseCategorydata(int id);
        IEnumerable<EOW_SubCategory> SubCategorydata(int id);
        IEnumerable<EOW_TravelClaim> GetAssetExpHsndetails(int AssetExpID, string AssetExpFlag);
        IEnumerable<EOW_PaymentMode> PaymentModedata();
        IEnumerable<EOW_Attachment> AttachmentTypedata();
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodata(string empgid, string clmtype);
        string insertcentralattment(string ecfgid);
        IEnumerable<EOW_HSN> HsnCodeList(int expanSubcatid);
        IEnumerable<EOW_Payment> SelectEmployeeePaymentid(string ecfid, string invoiceid, int id);
        IEnumerable<EOW_EmployeeeExpense> GetEmployeeeExpense(string ecfid, string invoiceid);
        IEnumerable<EOW_EmployeeeExpense> SelectEmployeeeExpensebyid(string ecfid, string invoiceid, int id);
        IEnumerable<EOW_RefNo> EmployeeePaymentRefNodatagri(string empgid, string type);
        string InsertEmployeeeBasicinvoiceupdate(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string ECFGid);
        string InsertEmployeeeBasicupdate(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string proxy, string id);
        string InsertEmployeeeExpense(EOW_EmployeeeExpense EmployeeeExpense, string ecfgid, string invoicegid);
        string InsertEmployeeePayment(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string rempayamt);
        string InsertEmployeeePaymentbasic(string EmployeeeGid, string ecfgid, string invoicegid, string amt);
        string UpdateEmployeeeExpense(EOW_EmployeeeExpense EmployeeeExpense, string ecfgid, string invoicegid, string expactiverowid);
        string DeleteEmployeeeExpense(int EmployeeeExpenseGID, string ecfgid, string invoicegid);
        string DeleteEmployeeePayment(int EmployeeePaymentGID, string ecfgid, string invoicegid, string rempayam);
        string DeleteEmployeeeAttachment(int EmployeeeAttachmentGID, string ecfgid);
        string InsertEmployeeePaymentbasicupdate(string EmployeeeGid, string ecfgid, string amt);
        string UpdateEmployeeePayment(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string payactiverowid, string beforeamt, string eraamt);
        string InsertEmployeeeBasic(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string clmtype);
        IEnumerable<EOW_EmployeeeExpense> SelectEmployeeeBasic(string empcode);
        string GetEmployeeeProxy(string EmployeeeExpense);
        string GetEmployeeeGID(string EmployeeeExpense);
        string InsertEmployeeeBasicinvoice(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string ECFGid, string clmtype);
        string selectecfgidBasic(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid);
        string selectinvoiceidBasic(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string EcfinGid, string clmtype);
        IEnumerable<EOW_Payment> GetEmployeeePayment(string ecfgid, string invoicegid);
        IEnumerable<EOW_File> GetEmployeeeAttachment(string ecfgid, string EmployeeeGid);
        string InsertEmpAtt(HttpPostedFileBase savefile, EOW_File EmployeeeExpenseModel, string p1,  string p3);

        string downloadAttachment(int EmployeeeAttachmentGID, string ecfgid);
        string Insertecf(EOW_EmployeeeExpense EmployeeeExpenseModel, string ecfgid, string ingid, string user, string empgid, string clam, string queid);
        /////////////-----------Travel Claim--------
        IEnumerable<EOW_TravelClaim> GetHsnList(int hsnid = 0);
        IEnumerable<EOW_TravelClass> tTravelClassdatadata(int id);
        IEnumerable<EOW_TravelMode> tTravelModedata();
        IEnumerable<EOW_TravelCity> tTravelcitydata();
        IEnumerable<EOW_TravelClaim> tSelectEmployeeeBasic(string empcode);
        IEnumerable<EOW_TravelClaim> GetTravelModedata(string ecfid, string invoiceid, string traveltype);
        IEnumerable<EOW_TravelClaim> GetTravelModedatasingle(string ecfid, string invoiceid, string traveltype, int id);
        string DeleteTravelExpense(int EmployeeeExpenseGID, string ecfgid, string invoicegid);
        List<EOW_Employeelst> getemployeedetails();
        List<EOW_Employeelst> getemployeedetails(string empname, string empcode);
        IEnumerable<EOW_Employeelst> GetEmployeeelist(string ecfgid);

        IEnumerable<EOW_TravelClaim> ChangeBussinessSegment(string OUCode);

        string InsertTravelModeCreate(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid);
        string InsertOtherTravelCreate(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid);

        string UpdateTravelModeCreate(EOW_TravelClaim TravelMode, string ecfgid, string invoicegid, string empgid, string gid);
        string UpdateOtherTravelCreate(EOW_TravelClaim OtherTravel, string ecfgid, string invoicegid, string empgid, string gid);

        string insertempperson(EOW_Employeelst EmployeeeExpense, string ecfgid);
        string DeleteEmployeelst(int EmployeeeExpenseGID, string ecfgid);
        //---------------------------dash Board

        IEnumerable<DashBoard> GetMydocDraft(string userlognid, string type);
        IEnumerable<DashBoard> GetMydocReject(string userlognid, string type);
        IEnumerable<DashBoard> GetDashBoardMyAppval(string userlognid, string type);
        IEnumerable<DashBoard> GetDashboardForMyApproval(string empgid);
        IEnumerable<DashBoard> doctypedata();
        IEnumerable<DashBoard> GetStatusType();
        IEnumerable<DashBoard> GetDashBoardDetails(string userlognid);
        IEnumerable<DashBoard> GetDashBoardDetailsa(string userlognid);
        IEnumerable<EOW_EmployeeeExpense> SelectViewdata(string Ecfgid, string type);
        IEnumerable<ApproverHistry> GetecfappHistory(string ecfgid, string invoicegid);
        IEnumerable<ApproverHistry> GetecfappHistorycen(string ecfgid, string invoicegid);
        string Insertapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt, string checker_raisergid, string delegates);
        string Insertapprovedactioncon(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid);
        //------------------------------local conveyance
        // IEnumerable<SheetData> GetSheetData(HttpPostedFileBase file);

        IEnumerable<EOW_Payment> GetLocalPayment(string ecfgid, string invoicegid);
        IEnumerable<EOW_TravelClaim> GetLocalexpdata(string ecfid, string traveltype);
        string GetStatusexcel(string valatate, string valatate1, string valatate2, string action);
        IEnumerable<EOW_Payment> GetEmployeeePaymentlocal(DataTable datatable, string ecfgid, string ecfdate);
        IEnumerable<EOW_TravelClaim> GetTravelModedatalocal(DataTable datatable, string ecfgid, string ecfdate);

        IEnumerable<EOW_SupplierModelgrid> GetSupplierdata(string ecfid, string invoiceid, string traveltype);
        string InsertsupplierCreate(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid);
        string InsertInsurancesupplierCreate(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid);
        IEnumerable<EOW_TravelClaim> GetSuppliserDedit(string ecfid, string invoiceid, string traveltype);
        string DeleteSupplideExpense(int EmployeeeExpenseGID, string ecfgid, string invoicegid);
        IEnumerable<EOW_TravelClaim> GetSupplierDebitsingle(string ecfid, string invoiceid, string traveltype, int id);
        IEnumerable<EOW_TravelClaim> GetSupplierINSDebitsingle(string ecfid, string invoiceid, string traveltype, int id);
        string UpdateSupplierDebit(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string rowid);
        string UpdateInsranceSupplierDebit(EOW_TravelClaim EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string rowid);
        string InsertSupplierPayment(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string suppliergid, string suppliergids);
        string DeleteSupplierPayment(int EmployeeePaymentGID, string ecfgid, string invoicegid, string rempayam, string suppliergid);
        string UpdateSupplierPayment(EOW_Payment EmployeeeExpense, string ecfgid, string invoicegid, string payactiverowid, string eraamt, string beforeamt, string supgid);
        IEnumerable<EOW_SupplierModelgrid> GetSuppliersingledata(string ecfid, string traveltype, int id);
        IEnumerable<EOW_SupplierModelgrid> GetINSSuppliersingledata(string ecfid, string traveltype, int id);
        string InsertSupplierinvoice(EOW_EmployeeeExpense EmployeeeExpense, string EmployeeeGid, string ECFGid, string clmtype);
        string selectsupplierinvoicestatus(string queuegid, string type);
        string DeleteSuppliernewlst(string invoice, string ecfgid);
        string UpdateSupplierinvoice(EOW_SupplierModelgrid EmployeeeExpense, string EmployeeeGid, string invoiceGid, string ECFGid, string clmtype, string clmtypew);
        string GetSupplierGID(string Suppliercode);
        string DeleteSupppers(int id, string ecfgid);
        string ExcelSupplierPayment(DataTable dt, string invoice, string ecfgid);

        IEnumerable<Tax> GetTax();
        IEnumerable<sinvotax> GetRate(sinvotax dt);
        IEnumerable<sinvotax> GetSupplierTax(string invoicegid);
        string InsertsupplierTaxCreate(sinvotax EmployeeeExpense, string invoicegid, string empgid, string supgid);
        IEnumerable<EOW_TravelClaim> GetSuppliserDeditdsaaler(string ecfid, string invoicegid);
        string DeleteSupplideTax(int EmployeeeExpenseGID, string invoicegid);
        IEnumerable<sinvotax> SelectSuppliertaxid(string invoiceid, int id);
        string updatesupplierTaxCreate(sinvotax EmployeeeExpense, string invoicegid, string gid, string supgid);
        IEnumerable<EOW_EmployeeeExpense> SelectEmployeeeheader(string empcode);
        //----------------------------Supplier DSA
        string GetSupplieruploaad(DataTable DataTable, string ecfgid, string ecfdate, Hashtable empchck, Hashtable empchckou);
        IEnumerable<EOW_SupplierModelgrid> GetSupplierexceldata(string ecfid, string traveltype);
        IEnumerable<EOW_TravelClaim> GetSuppliserDeditexcel(string ecfid, string traveltype);
        IEnumerable<EOW_Payment> GetSupplierPayment(string ecfgid, string invoicegid);
        string DeleteSuppliernewlstdsa(string invoice, string ecfgid);

        string InsertEmployeeeBasicupdatesup(EOW_Supplierinvoice EmployeeeExpense, string eMP_Gid, string EmployeeeGid, string proxy, string id);
        string InsertEmployeeeBasicsup(EOW_Supplierinvoice EmployeeeExpense, string eMP_Gid, string EmployeeeGid, string clmtype);
        string UpdateSupplierinvoicefinal(EOW_Supplierinvoice EmployeeeExpenseModel, string ecfgid, string invoiceGid, string user, string eempid, string clam, string queid);
        string UpdateInsuranceinvoicefinal(EOW_Supplierinvoice EmployeeeExpenseModel, string ecfgid, string invoiceGid, string user, string eempid, string clam, string queid);
        string selectsupplierinvoice(string queuegid, string str);
        IEnumerable<EOW_Supplierinvoice> SelectViewdatasupplier(string ecfid, string action);
        IEnumerable<EOW_PaymentMode> PaymentModesupplierdata(string suplierid);
        IEnumerable<DashBoard> GetDashBoardDetailssearch(string userlognid, string dcodate, string docno, string docamt, string doctype, string DocStatus);
        IEnumerable<DashBoard> GetDashBoardDetailssearcha(string userlognid, string dcodate, string docno, string docamt, string doctype, string DocStatus);
        IEnumerable<DashBoard> GetMydocAppoalc(string userlognid, string type);
        IEnumerable<DashBoard> GetMydocAppred(string userlognid, string type);

        IEnumerable<EOW_SupplierModelgrid> GetSuppliercentral(DataTable datatable, string flage);
        IEnumerable<sinvotax> GetCentralteamTax(DataTable dt, string invoicegid);
        IEnumerable<EOW_Payment> GetcentralPayment(DataTable dt, string invoicegid);
        IEnumerable<EOW_TravelClaim> GetcentralDedit(DataTable dt, string invoiceid);
        string selectcentralglcode(string glexpcodeid);
        string selectcentralglddlcode(string glexpcodeid);
        string Excelcentraldebit(DataTable DataTable, string invoicegid);
        string DeletecentralAttachment(string ecfgid);
        string UpdateCentralinvoicefinal(EOW_Supplierinvoice EmployeeeExpenseModel, string eempid, string clam, string queid, string queidq);

        IEnumerable<EOW_PO> GetPoDetails(string supplierID, string potype);
        IEnumerable<EOW_PO> Getpodetailsgrid(string values, string valuesid, string ecfid, string invoiceid, string type);
        string Insertpodetails(EOW_PO supplierpo, string id);
        IEnumerable<EOW_PO> GetPoDetailsitem(string ecfid, string invoiceid, string id);
        IEnumerable<EOW_PO> GetPoDetailssingledata(string id);
        string DeleteSupplidepo(string EmployeeeExpenseGID, string invoicegid);
        IEnumerable<EOW_PO> SelectPOSearch(string PONO, string podate, string poamt, string suppierid, string potype);
        string Insertitemdetails(EOW_PO supplierpo, string id);

        string GetPoMappingExists(string ecfgid);
        string Getbebitlineamt(string ecfgid, string invoicegid);
        string Getpomappedamt(string ecfgid, string invoicegid);
        string Gettaxmapedamt(string ecfgid, string invoicegid);

        //kathir
        IEnumerable<EOW_TravelClaim> GetAssetCategoryList();
        IEnumerable<EOW_TravelClaim> GetAssetSubCategoryList(int AssetId = 0);
        string GetEcfSubTypeGid(int EcfGid = 0);
        string GetEcfGid(int QueueGid = 0);
        IEnumerable<sinvotax> GetTaxSubTypeList(int TaxId = 0);

        IEnumerable<EOW_Supplierinvoice> SelectGLCode();
        IEnumerable<EOW_Supplierinvoice> SelectGLSearch(string EmployeeName, string EmployeeCode);
        string GetSupplierTmpGid(string trngid);

        //IEnumerable<EOW_TravelClaim> GetHsnList(int hsnid = 0);
        string CheckWithCBF(int ecf_gid = 0);
        //Dhasarathan 22-11-2016
        string GetEmployeePayModeEraAcc(int EmployeeeGid);
        string GetSupplierBankDetailsBypayMode(string ecfgid);
        string GetMaxQueueGid(string _ECFId);
        string GetDocTypeGIDEClaim(string queue_gid);
        bool SaveAttachmentDoc(string RefFlag, string RefType, string RefGid, string AttachmentName, string AttachmentDesc);
        //Dhasarathan
        string GetExpenseHsndesc(string hsngid);
        IEnumerable<RCMEnteries> GetRCMEnteries(string ECF_GID,string Invoice_Gid,string Action);
		 //Pandiaraj
        //IEnumerable<EOW_RefNo> EmployeeePaymentcrnfilter(string supgid, string type, string ecfno, string fdate, string todate);
    }
}