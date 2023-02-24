using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public interface RetentiorealeaseReportepository
    {
        IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReport();
        IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReport(string retentionfromdate, string retentiontodate, string suppcode, string suppname, string invoiceno, string ecfno);
        IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReportById(string reportMode, string retentionfromdate, string retentiontodate);
        IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseView(int invoice_gid);
    }
}