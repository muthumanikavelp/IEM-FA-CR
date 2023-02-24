using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_RptEntity
    {
        public string ReceivedDate { get; set; }
        public string Name { get; set; }
        public string Raiser { get; set; }
        public string ECFNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoiceDesc { get; set; }
        public string ecfstatus_name { get; set; }
        public string poheader_pono { get; set; }
        public string PaymentDate { get; set; }
        public string ECFStatus { get; set; }
        public string CTECFStatus { get; set; }
        public string Department { get; set; }
        public string Queue { get; set; }
        public string Aging { get; set; }
        public string queue_date { get; set; }
        public string queue_action_Date { get; set; }
        public string Depname { get; set; }
        public string Status { get; set; }

        public List<Ifams_RptEntity> RptModel { get; set; }
    }
}