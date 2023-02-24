using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.EOW.Models
{
    public interface RetentionForfeitureRepository
    {
        IEnumerable<Eow_RetentionForfeiture> RetentionReleaseGrid();
        IEnumerable<Eow_RetentionForfeiture> RetentionReleaseGrid(string releasedate, string ecfdate, string ecfno, string invoiceno, string suppcode, string suppname, string extenddate);
        Eow_RetentionForfeiture GetRetentionExtendRecordById(int invoicegid);
        DataTable GetSerialNo();
        DataTable GetRetentioninformation(int invoicegid);
        DataTable CheckData(int invoivegid);
        string Insertextenddetails(Eow_RetentionForfeiture Extendinformation);
    }
}