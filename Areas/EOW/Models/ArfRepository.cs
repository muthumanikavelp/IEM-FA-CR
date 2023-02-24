using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.EOW.Models
{
    public interface ArfRepository
    {
        string selectpolicy1(string Gid);
        DataTable Selectpaymentsup();
        string cc_po(string Dcoverify);
        EOW_arfraising ecf_desc(string empid);
        IEnumerable<EOW_arfraising> SelectSupplierSearch(string SupplierName, string SupplierCode);
        IEnumerable<EOW_arfraising> SelectSupplier();
        EOW_arfraising proxy(string proxyid);
        List<GetAdvancetype> GetAdvancetype(string type);
        EOW_arfraising empdetailsadvance_id(string empid);
        // DataTable Getarfadvacedetails();
        EOW_arfraising empdetailspay_id(string empid);
        IEnumerable<EOW_arfraising> Getarfadvacedetails();
        IEnumerable<EOW_arfraising> Getarfpaymentdetails();
        IEnumerable<EOW_arfraising> Getarfattachementdetails();
        List<GetPaymode> GetPaymode(string empsup, string empsupgid);
        List<GetRef> GetRefant(string empsup);
        DataTable GetArfPaymode();
        IEnumerable<EOW_arfraising> GetarfEmployeedetails(string Empcode);
        DataTable GetarfSupplierdetails(string Suppcode);
        DataTable GetEcfDetails(string Ecfempsupgid, string ecf);
        //IEnumerable<EOW_arfraising> SelectAdvance();
        DataTable SelectAdvance(string Empcode);
        string InsertEmployeeePaymentbasic(string EmployeeeGid, string ecfgid, string amt);
        string InsertEmployeeePaymentbasicupdate(string EmployeeeGid, string ecfgid, string amt);
        string GetStatusexcel(string valatate, string action);
        string insertsuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername);
        string updatesuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername);
        DataTable Getpaymodedata(string valatate, string action);
        //IEnumerable<EOW_arfraising> Selectpayment();
        DataTable Selectpayment(string Empcode);
        //IEnumerable<EOW_arfraising> Selectattach();
        DataTable Selectattach(string Empcode);
        string SaveRequestval(EOW_arfraising cen);
        string SaveAdvance(EOW_arfraising advance);
        string Savepayment(EOW_arfraising Payment);
        List<GetBenificary> GetBenificary();
        EOW_arfraising SelectAdvanceById(int advanceId);
        EOW_arfraising SelectpaymentById(int paymentId);
        string UpdatAdvance(EOW_arfraising cen);
        string updatePayment(EOW_arfraising Payment);
        string DeletePayment(int paymentId);
        string DeleteAdvance(EOW_arfraising advancetId);
        string downloadAttachment(int EmployeeeAttachmentGID);
        string InsertEmpAtt(HttpPostedFileBase savefile, EOW_arfraising EmployeeeExpense);
        string DeleteAttachment(int attachId);
        string Insertecf(EOW_arfraising EmployeeeExpenseModel, string eempid, string user, string qeee);
        EOW_arfraising empdetails(string empid);
        // IEnumerable<EOW_arfraising> SelectViewdataarf(int ecfid, string type);
        EOW_arfraising advanceamount(string empid);
        EOW_arfraising payamount(string empid);
        string oucode(string Dcoverify);
        string fccode(string Dcoverify);
        string cccode(string Dcoverify);
        string productcode(string Dcoverify);
        string _pono(string Dcoverify);
        string GetDecnote(string subsype, string type);
        EOW_EmployeeeExpense empraiserdetails(string empid);
        EOW_EmployeeeExpense supraiserdetails(string empid);
        List<GetAttach> Getattach();
        DataTable Selectpaymentemp();
        IEnumerable<EOW_arfraising> GetarfEmployeedetailssearch();
        IEnumerable<EOW_arfraising> pono(string gid);
        IEnumerable<EOW_arfraising> cbfno(string gid);
        string Insertecfderaft(EOW_arfraising EmployeeeExpenseModel, string eempid);
        string Savepaymentcredit(EOW_arfraising Payment);
        //Dhasarathan
        string GetEmployeePayModeEraAcc(int EmployeeeGid);
        string GetSupplierBankDetailsBypayMode(string ecfgid);
        //Dhasarathan
    }
    public interface RetentionRepository
    {
        DataTable RetentionReleaseGrid_log(string Docid);
        string dummyinvoice(string ecf);
        string DeleteAttachment(int attachId);
        string InsertEmpAtt(HttpPostedFileBase savefile, Retention_Release EmployeeeExpense);
        // IEnumerable<Retention_Release> debitgrid(string invoice_ecf, string invoice);
        IEnumerable<GetRefcredit> Getrefcreditpaymode(Retention_Release expsubcat);
        //string SaveReleasedebit(Retention_Release cen);
        string SaveReleasecredit(Retention_Release cen);
        //List<Getexpensesubcategory> Getexpsubcategoryload();
        //List<Getexpensecategory> Getexpcategoryload();
        //IEnumerable<Getexpensesubcategory> Getexpsubcategory(Retention_Release expsubcat);
        //IEnumerable<Getexpensecategory> Getexpcategory(Retention_Release expcat);
        Retention_Release employeedetil();
        //List<Getfccc> Getfcccid();
        //List<Getcc> Getccid();
        //List<Getfc> Getfcid();
        //List<Getexpense> Getexpenseid();
        List<GetRefcredit> Getrefcredit();
        List<GetPaymodecredit> GetPaymodecredit();
        IEnumerable<Retention_Release> RetentionReleaseGrid();
        Retention_Release RetentionReleaseGrid_id(int Docid);
        string SaveRelease(Retention_Release cen);
        string update(Retention_Release cen);
        IEnumerable<Retention_Release> RetentionReleaseGridSearch(string filter_name, string filter_name1, string ECFNo, string InvoiceNo, string Suppliercode, string Suppliername, string extendeddate);
        string ReleaseVerifyamount(Retention_Release verify);
        string GetSerialNo();
    }
}