using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class sectorentity
    {
    }
    public class sectorreport
    {
        public string branch_code { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string payrunvoucheramount { get; set; }
        public string payrunvoucherpvno { get; set; }
        public string supplierheader_name { get; set; }
        public string chqno { get; set; }
        public string grossamount { get; set; }
        public string tdsamount { get; set; }
        public string netamount { get; set; }
        public string location { get; set; }
        public string awbno { get; set; }
        public string sendto { get; set; }
        public string ecfno { get; set; }
        public List<sectorreport> secrep { get; set; }

    }

    public class tdsreport
    {
        public string mode { get; set; }
        public string finyear { get; set; }
        public string deductionmonth { get; set; }
        public string ecfno { get; set; }
        public string glno { get; set; }
        public string suppliercode { get; set; }
        public string suppliername { get; set; }
        public string invoiceamount { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public string taxcode { get; set; }
        public string taxableamount { get; set; }
        public string ecfclaimmonth { get; set; }
        public List<tdsreport> tdsrep { get; set; }
    }

    public class wctreport
    {
        public string modes { get; set; }
        public string finyear { get; set; }
        public string deductionmonth { get; set; }
        public string ecfno { get; set; }
        public string glno { get; set; }
        public string suppliercode { get; set; }
        public string suppliername { get; set; }
        public string invoiceamount { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public string taxcode { get; set; }
        public string taxableamount { get; set; }
        public string ecfclaimmonth { get; set; }
        public List<wctreport> wctrep { get; set; }
    }

    public class servicetareport
    {
        public string ecfno { get; set; }
        public string panno { get; set; }
        public string vendorcode { get; set; }
        public string vendorname { get; set; }
        public string empcode { get; set; }
        public string raiser { get; set; }
        public string invoiceno { get; set; }
        public string invoicedate { get; set; }
        public string billamount { get; set; }
        public string cbsamount { get; set; }
        public string bscc { get; set; }
        public string branchname { get; set; }
        public string expcode { get; set; }
        public string subcode { get; set; }
        public string narration { get; set; }
        public string expmonth { get; set; }
        public string billstartdate { get; set; }
        public string billenddate { get; set; }
        public string productcode { get; set; }
        public string staxamount { get; set; }
        public string servicetax { get; set; }
        public string taxableamount { get; set; }
        public List<servicetareport> stax { get; set; }
    }


    public class valreport
    {
        public string ecfno { get; set; }
        public string ecfstatus { get; set; }
        public string vendorcode { get; set; }
        public string vendorname { get; set; }
        public string empcode { get; set; }
        public string empname { get; set; }
        public string ecfprosamt { get; set; }
        public string ecfamount { get; set; }
        public string paymenttranamt { get; set; }
        public string ecfnet { get; set; }
        public string prundate { get; set; }
        public string TDS { get; set; }
        public string WCT { get; set; }
        public string ptranpaymode { get; set; }
        public string chqno { get; set; }
        public string pvno { get; set; }
        public string pvouchernet { get; set; }
        public string vclaim { get; set; }
        public string chqdate { get; set; }
        public string vmemo { get; set; }
        public string paybankgl { get; set; }
        public string remarks { get; set; }
        public string oraclebatch { get; set; }

        public List<valreport> valrep { get; set; }
    }

    public class inwardreport
    {
        public string ecfno { get; set; }
        public string ecfcurrencyamount { get; set; }
        public string ecfamount { get; set; }
        public string reducedamount { get; set; }
        public string processedamount { get; set; }
        public string delmetamount { get; set; }
        public string despatchdate { get; set; }
        public string couriername { get; set; }
        public string awbno { get; set; }
        public string remarks { get; set; }
        public string description { get; set; }
        public string docinwardno { get; set; }
        public string docinwarddocno { get; set; }
        public string docbatchdate { get; set; }
        public string docinwarddate { get; set; }
        public string docinwardfrom { get; set; }
        public string docinwardto { get; set; }
        public string doccount { get; set; }
        public string docpouchreceivedfrom { get; set; }
        public string docpouchreceiveddate { get; set; }
        public string docpouchawbno { get; set; }
        public string docpouchcount { get; set; }
    }

}
