using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.EOW.Models
{
    public interface ArfInsRepository
    {
        string selectpolicy1(string Gid);
        DataTable Selectpaymentsup();
        string cc_po(string Dcoverify);
        EOW_ArfInsuranceraising ecf_desc(string empid);
        IEnumerable<EOW_ArfInsuranceraising> SelectSupplierSearch(string SupplierName, string SupplierCode);
        IEnumerable<EOW_ArfInsuranceraising> SelectSupplier();
        EOW_ArfInsuranceraising proxy(string proxyid);
        List<GetAdvancetype> GetAdvancetype(string type);
        EOW_ArfInsuranceraising empdetailsadvance_id(string empid);
        // DataTable Getarfadvacedetails();
        EOW_ArfInsuranceraising empdetailspay_id(string empid);
        IEnumerable<EOW_ArfInsuranceraising> Getarfadvacedetails();
        IEnumerable<EOW_ArfInsuranceraising> Getarfpaymentdetails();
        IEnumerable<EOW_ArfInsuranceraising> Getarfattachementdetails();
        List<GetPaymode> GetPaymode(string empsup, string empsupgid);
        List<GetRef> GetRefant(string empsup);
        DataTable GetArfPaymode();
        IEnumerable<EOW_ArfInsuranceraising> GetarfEmployeedetails(string Empcode);
        DataTable GetarfSupplierdetails(string Suppcode);
        DataTable GetEcfDetails(string Ecfempsupgid, string ecf);
        //IEnumerable<EOW_ArfInsuranceraising> SelectAdvance();
        DataTable SelectAdvance(string Empcode);
        string InsertEmployeeePaymentbasic(string EmployeeeGid, string ecfgid, string amt);
        string InsertEmployeeePaymentbasicupdate(string EmployeeeGid, string ecfgid, string amt);
        string GetStatusexcel(string valatate, string action);
        string insertsuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername);
        string updatesuplierpayment(string suppliergids, string ecfgid, string tolrowinvoiceamt, string suppliername);
        DataTable Getpaymodedata(string valatate, string action);
        //IEnumerable<EOW_ArfInsuranceraising> Selectpayment();
        DataTable Selectpayment(string Empcode);
        //IEnumerable<EOW_ArfInsuranceraising> Selectattach();
        DataTable Selectattach(string Empcode);
        string SaveRequestval(EOW_ArfInsuranceraising cen);
        string SaveAdvance(EOW_ArfInsuranceraising advance);
        string Savepayment(EOW_ArfInsuranceraising Payment);
        List<GetBenificary> GetBenificary();
        EOW_ArfInsuranceraising SelectAdvanceById(int advanceId);
        EOW_ArfInsuranceraising SelectpaymentById(int paymentId);
        string UpdatAdvance(EOW_ArfInsuranceraising cen);
        string updatePayment(EOW_ArfInsuranceraising Payment);
        string DeletePayment(int paymentId);
        string DeleteAdvance(EOW_ArfInsuranceraising advancetId);
        string downloadAttachment(int EmployeeeAttachmentGID);
        string InsertEmpAtt(HttpPostedFileBase savefile, EOW_ArfInsuranceraising EmployeeeExpense);
        string DeleteAttachment(int attachId);
        string Insertecf(EOW_ArfInsuranceraising EmployeeeExpenseModel, string eempid, string user, string qeee);
        EOW_ArfInsuranceraising empdetails(string empid);
        // IEnumerable<EOW_ArfInsuranceraising> SelectViewdataarf(int ecfid, string type);
        EOW_ArfInsuranceraising advanceamount(string empid);
        EOW_ArfInsuranceraising payamount(string empid);
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
        IEnumerable<EOW_ArfInsuranceraising> GetarfEmployeedetailssearch();
        IEnumerable<EOW_ArfInsuranceraising> pono(string gid);
        IEnumerable<EOW_ArfInsuranceraising> cbfno(string gid);
        string Insertecfderaft(EOW_ArfInsuranceraising EmployeeeExpenseModel, string eempid);
        string Savepaymentcredit(EOW_ArfInsuranceraising Payment);
        //Dhasarathan
        string GetEmployeePayModeEraAcc(int EmployeeeGid);
        string GetSupplierBankDetailsBypayMode(string ecfgid);
    }
}