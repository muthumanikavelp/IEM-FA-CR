using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public interface RetentionInvoiceReportRepository
    {
        IEnumerable<EOW_RetentionInvoiceReport> RetentioinInvoiceReport();
        IEnumerable<EOW_RetentionInvoiceReport> RetentioinInvoiceReport(string retentionfromdate, string retentiontodate, string suppcode, string suppname, string invoiceno, string ecfno);
    
    }
}